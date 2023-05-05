using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using lab2.Tree;

namespace lab2
{
    public class SyntaxParser
    {
        public List<Token> Tokens;
        public Dictionary<string, string> Variables;
        public Dictionary<string, string> Funcs;
        public int Position { get; set; } = 0;

        public SyntaxParser()
        {
            List<Token> tokens = new List<Token>();
        }

        public SyntaxParser(Lexer lexer)
        {
            Tokens = lexer.allTokens;
            Variables = lexer.variables;
            Funcs = lexer.funcs;
        }

        public Token? Check(List<TokenType> tokenTypes)
        {
            if (Position < Tokens.Count)
            {
                var token = Tokens[Position];

                if (tokenTypes.Contains(token.Type))
                {
                    Position++;
                    return token;
                }
            }

            return null;
        }

        public Token Expected(List<TokenType> tokenTypes)
        {
            var token = Check(tokenTypes);

            if (token == null)
            {
                throw new Exception($"After' {Tokens[Position - 1].Value}' expected '{tokenTypes[0]}'");
            }

            return token;
        }

        public INode ParseBlock()
        {
            var root = new BlockNode();

            if (Position > 1 && Tokens[Position - 1].Type != TokenType.OPENBLOCK)
            {
                var statementNode = ParseExpression();
                if (statementNode != null)
                {
                    root.AddNode(statementNode);
                }
                return root;
            }

            while (Position < Tokens.Count)
            {
                if (Check(new List<TokenType> { TokenType.CLOSEBLOCK }) != null)
                {
                    return root;
                }

                var statementNode = ParseExpression();

                if (statementNode != null)
                {
                    root.AddNode(statementNode);
                }
            }

            return root;
        }

        private INode ParseVariableType()
        {
            var token = Check(new List<TokenType> { TokenType.VARIABLETYPE } );

            if (token != null)
            {
                return new VariableTypeNode(token);
            }

            throw new Exception($"After'{Tokens[Position - 1].Value}' expected variable");
        }

        private INode ParseVariableOrConst()
        {
            var token = Check(new List<TokenType> { TokenType.INT, TokenType.FLOAT, TokenType.STRING, TokenType.CHAR });

            if (token != null)
            {
                return new ConstNode(token);
            }

            token = Check(new List<TokenType> { TokenType.VARIABLE, TokenType.FUNCEXEC });

            if (token != null)
            {
                return new VariableNode(token);
            }

            throw new Exception($"After '{Tokens[Position - 1].Value}' expected const or variable");
        }

        public INode ParseBrackets()
        {
            if (Check(new List<TokenType> { TokenType.OPENBRACKET }) != null)
            {
                var node = ParseFormula();
                Expected(new List<TokenType> { TokenType.CLOSEBRACKET });
                return node;
            }
            else
            {
                return ParseVariableOrConst();
            }
        }

        private INode ParseFormula()
        {
            var leftNode = ParseBrackets();
            var op = Check(new List<TokenType> { TokenType.UNARY, TokenType.BINARY, TokenType.STREAM, TokenType.ASSIGN });

            while (op != null)
            {
                if (op.Type == TokenType.UNARY)
                {
                    leftNode = new UnaryOperationNode(op, leftNode);
                    op = Check(new List<TokenType> { TokenType.UNARY, TokenType.BINARY, TokenType.STREAM, TokenType.ASSIGN });
                }
                else if (op.Type == TokenType.STREAM)
                {
                    Position--;
                    break;
                }
                else
                {
                    var rightNode = ParseBrackets();

                    if (leftNode is BinaryOperationNode binary)
                    {
                        binary.RightNode = new BinaryOperationNode(op, binary.RightNode, rightNode);
                        leftNode = binary;
                    }
                    else
                    {
                        leftNode = new BinaryOperationNode(op, leftNode, rightNode);
                    }

                    op = Check(new List<TokenType> { TokenType.UNARY, TokenType.BINARY, TokenType.STREAM, TokenType.ASSIGN });
                }
            }

            return leftNode;
        }

        private INode ParseCondition()
        {
            Expected(new List<TokenType> { TokenType.OPENBRACKET });
            var ifCondition = ParseFormula();
            Expected(new List<TokenType> { TokenType.CLOSEBRACKET });
            if (Tokens[Position].Type == TokenType.OPENBLOCK)
            {
                Position++;
            }
            var ifBody = ParseBlock();

            INode? elseBody = null;

            if (Check(new List<TokenType> { TokenType.CONDITIONAL }) != null)
            {
                if (Check(new List<TokenType> { TokenType.CONDITIONAL }) != null)
                {
                    elseBody = ParseCondition();
                }
                else
                {
                    elseBody = ParseBlock();
                    return new ConditionalNode(ifCondition, ifBody, elseBody);
                }
            }

            return new ConditionalNode(ifCondition, ifBody, elseBody);
        }

        private INode? ParseExpression()
        {
            if (Check(new List<TokenType> { TokenType.VARIABLETYPE }) != null)
            {
                var token = Check(new List<TokenType> { TokenType.FUNC });

                if (token != null)
                {
                    Expected(new List<TokenType> { TokenType.OPENBLOCK });
                    var body = ParseBlock();
                    Position--;
                    Expected(new List<TokenType> { TokenType.CLOSEBLOCK });
                    return new FunctionNode(token, new List<Token>(), body);
                }

                token = Check(new List<TokenType> { TokenType.VARIABLE });

                if (token != null)
                {
                    var leftNode = new VariableNode(token) as INode;
                    var op = Check(new List<TokenType> { TokenType.ASSIGN });

                    if (op != null)
                    {
                        if (Check(new List<TokenType> { TokenType.NEW }) != null)
                        {
                            var type = Check(new List<TokenType> { TokenType.VARIABLETYPE });

                            if (type != null)
                            {
                                Position--;
                                var typeNode = ParseVariableType();
                                Expected(new List<TokenType> { TokenType.OPENINDEX });
                                var index = ParseFormula();
                                Expected(new List<TokenType> { TokenType.CLOSEINDEX });
                                Expected(new List<TokenType> { TokenType.SEMICOLON });
                                var rightNode = new BinaryOperationNode(new Token("new", TokenType.NEW), typeNode, index);
                                return new BinaryOperationNode(op, leftNode, rightNode);
                            }

                            throw new Exception($"After '{Tokens[Position - 1].Value}' expected variable type");
                        }

                        var value = ParseFormula();
                        Expected(new List<TokenType> { TokenType.SEMICOLON });
                        return new BinaryOperationNode(new Token("=", TokenType.ASSIGN), leftNode, value);
                    }
                    else
                    {
                        Expected(new List<TokenType> { TokenType.SEMICOLON });
                        return null;
                    }
                }
            }

            var funcToken = Check(new List<TokenType> { TokenType.FUNCEXEC });
            if (funcToken != null)
            {
                Expected(new List<TokenType> { TokenType.SEMICOLON });
                return new FunctionExecNode(funcToken);
            }

            if (Check(new List<TokenType> { TokenType.KEYWORD }) != null)
            {
                if (Tokens[Position - 1].Value == "break" || Tokens[Position - 1].Value == "continue")
                {
                    Expected(new List<TokenType> { TokenType.SEMICOLON });
                    return new CycleKeywordsNode(Tokens[Position - 2]);
                }
                return null;
            }

            if (Check(new List<TokenType> { TokenType.VARIABLE, TokenType.INT, TokenType.FLOAT }) != null)
            {
                Position--;
                var variableNode = ParseVariableOrConst();
                var op = Check(new List<TokenType> { TokenType.UNARY, TokenType.BINARY, TokenType.STREAM, TokenType.ASSIGN });

                if (op != null)
                {
                    if (op.Type == TokenType.UNARY)
                    {
                        var unaryNode = new UnaryOperationNode(op, variableNode);
                        Expected(new List<TokenType> { TokenType.SEMICOLON });
                        return unaryNode;
                    }

                    if (Check(new List<TokenType> { TokenType.NEW }) != null)
                    {
                        var type = Check(new List<TokenType> { TokenType.VARIABLETYPE });

                        if (type != null)
                        {
                            Position--;
                            var typeNode = ParseVariableType();
                            Expected(new List<TokenType> { TokenType.OPENINDEX });
                            var value = ParseFormula();
                            Expected(new List<TokenType> { TokenType.CLOSEINDEX });
                            Expected(new List<TokenType> { TokenType.SEMICOLON });
                            return new BinaryOperationNode(new Token("new", TokenType.NEW), typeNode, value);
                        }

                        throw new Exception($"After '{Tokens[Position - 1].Value}' expected variable type");
                    }

                    var rightFormulaNode = ParseFormula();
                    var binaryNode = new BinaryOperationNode(op, variableNode, rightFormulaNode);
                    Expected(new List<TokenType> { TokenType.SEMICOLON });
                    return binaryNode;
                }
                else
                {
                    Expected(new List<TokenType> { TokenType.SEMICOLON });
                    return null;
                }

                throw new Exception($"After '{Tokens[Position - 1].Value}' expected operator");
            }

            var key = Check(new List<TokenType> { TokenType.CIN, TokenType.COUT, TokenType.WHILECYCLE, TokenType.FORCYCLE, TokenType.CONDITIONAL });
            if (key != null)
            {
                switch (key.Type)
                {
                    case TokenType.WHILECYCLE:
                        Expected(new List<TokenType> { TokenType.OPENBRACKET });
                        var condition = ParseFormula();
                        Expected(new List<TokenType> { TokenType.CLOSEBRACKET });
                        Expected(new List<TokenType> { TokenType.OPENBLOCK });
                        var body = ParseBlock();
                        Position--;
                        Expected(new List<TokenType> { TokenType.CLOSEBLOCK });
                        return new WhileNode(condition, body);
                    case TokenType.COUT:
                        var parameters = new List<INode>();
                        var op = Check(new List<TokenType> { TokenType.STREAM });

                        while (op != null)
                        {
                            var parameter = ParseFormula();
                            parameters.Add(parameter);
                            op = Check(new List<TokenType> { TokenType.STREAM });
                        }
                        if (parameters.Count == 0)
                        {
                            throw new Exception($"After '{Tokens[Position - 1].Value}' expected <<");
                        }
                        Expected(new List<TokenType> { TokenType.SEMICOLON });
                        return new CoutNode(parameters);
                    case TokenType.CIN:
                        parameters = new List<INode>();
                        op = Check(new List<TokenType> { TokenType.STREAM });
                        while (op != null)
                        {
                            var parameter = ParseFormula();
                            parameters.Add(parameter);
                            op = Check(new List<TokenType> { TokenType.STREAM });
                        }
                        if (parameters.Count == 0)
                        {
                            throw new Exception($"After '{Tokens[Position - 1].Value}' expected >>");
                        }
                        Expected(new List<TokenType> { TokenType.SEMICOLON });
                        return new CinNode(parameters);
                    case TokenType.FORCYCLE:
                        Expected(new List<TokenType> { TokenType.OPENBRACKET });
                        var init = ParseFormula();
                        Expected(new List<TokenType> { TokenType.SEMICOLON });
                        var conditionFor = ParseFormula();
                        Expected(new List<TokenType> { TokenType.SEMICOLON });
                        var iterator = ParseFormula();
                        Expected(new List<TokenType> { TokenType.CLOSEBRACKET });
                        Expected(new List<TokenType> { TokenType.OPENBLOCK });
                        var forBody = ParseBlock();
                        Position--;
                        Expected(new List<TokenType> { TokenType.CLOSEBLOCK });
                        return new ForNode(init, conditionFor, iterator, forBody);
                    case TokenType.CONDITIONAL:
                        return ParseCondition();
                }
            }

            throw new Exception($"Error on token {Position}");
        }

    }
}
