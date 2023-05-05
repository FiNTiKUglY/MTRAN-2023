using lab2.Tree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace lab2
{
    public class Starter
    {
        public CustomDictionary Variables;
        public Dictionary<string, string> VariableNames = new Dictionary<string, string>();
        public Dictionary<string, string> Funcs;
        public List<Token> Tokens;
        public INode? RootNode;

        public Starter(SyntaxParser parser, INode rootNode)
        {
            VariableNames = parser.Variables;
            Variables = new CustomDictionary(VariableNames);
            foreach(var key in parser.Variables.Keys)
            {
                Variables.Add(key, null);
            }
            Funcs = parser.Funcs;
            RootNode = rootNode;
            Tokens = parser.Tokens;
        }

        public void Start()
        {
            if (RootNode != null)
            {
                ExecBlock(RootNode);
            }
        }

        private CustomObject? BinaryOperation(CustomObject value1, CustomObject op, CustomObject value2)
        {
            switch(op.ToString())
            {
                case "+":
                    return value1 + value2;
                    break;
                case "-":
                    return value1 - value2;
                    break;
                case "*":
                    return value1 * value2;
                    break;
                case "/":
                    return value1 / value2;
                    break;
                case "%":
                    return value1 % value2;
                case "new":
                    if (value1.ToString() == "int")
                    {
                        return new List<int>(Enumerable.Repeat(0, value2));
                    }
                    else if (value2.ToString() == "float")
                    {
                        return new List<float>(Enumerable.Repeat(0.0f, value2));
                    }
                    break;
                default:
                    return null;
                    break;
            }
            return null;
        }

        private bool CompareOperation(CustomObject value1, CustomObject op, CustomObject value2)
        {
            switch (op.ToString())
            {
                case ">":
                    return value1 > value2;
                    break;
                case ">=":
                    return value1 >= value2;
                    break;
                case "<":
                    return value1 < value2;
                    break;
                case "<=":
                    return value1 <= value2;
                    break;
                case "!=":
                    return value1 != value2;
                    break;
                case "==":
                    return value1 == value2;
                    break;
                default:
                    return false;
                    break;
            }
            return false;
        }

        private void ExecBlock(INode node)
        {
            if (node == null)
            {
                return;
            }
            if (node is BlockNode blockNode)
            {
                foreach(var statementNode in blockNode.Nodes)
                {
                    if (statementNode is FunctionNode functionNode)
                    {
                        ExecBlock(functionNode.Body);
                    }
                    else if (statementNode is BinaryOperationNode binaryNode)
                    {
                        ExecBinary(binaryNode);
                    }
                    else if (statementNode is UnaryOperationNode unaryNode)
                    {
                        ExecUnary(unaryNode);
                    }
                    else if (statementNode is FunctionExecNode functionExecNode)
                    {
                        ExecFunction(functionExecNode);
                    }
                    else if(statementNode is CoutNode coutNode)
                    {
                        foreach (var param in coutNode.Parameters)
                        {
                            Console.Write(ExecParam(param).ToString());
                        }
                    }
                    else if (statementNode is CinNode cinNode)
                    {
                        foreach (var param in cinNode.Parameters)
                        {
                            if (param is VariableNode variableNode)
                            {
                                Variables[variableNode.Variable.Value] = Console.ReadLine();
                            }
                        }
                    }
                    else if (statementNode is ForNode forNode)
                    {
                        ExecBinary(forNode.Init);
                        while (CheckCondition(forNode.Condition))
                        {
                            if (forNode.Body is BlockNode block)
                            {
                                ExecBlock(block);
                            }
                            if (forNode.Iterator is BinaryOperationNode binaryNode2)
                            {
                                ExecBinary(binaryNode2);
                            }
                            else if (forNode.Iterator is UnaryOperationNode unaryNode2)
                            {
                                ExecUnary(unaryNode2);
                            }
                        }
                    }
                    else if (statementNode is WhileNode whileNode)
                    {
                        while (CheckCondition(whileNode.Condition))
                        {
                            if (whileNode.Body is BlockNode block)
                            {
                                ExecBlock(block);
                            }
                        }
                    }
                    else if (statementNode is ConditionalNode conditionalNode)
                    {
                        ExecConditional(conditionalNode);
                    }
                }
            }
        }

        private void ExecConditional(INode node)
        {
            if (node == null)
            {
                return;
            }
            if (node is ConditionalNode conditionalNode)
            {
                if (CheckCondition(conditionalNode.Condition))
                {
                    if (conditionalNode.Body is BlockNode block)
                    {
                        ExecBlock(block);
                    }
                }
                else
                {
                    if (conditionalNode.ElseBody != null)
                    {
                        ExecConditional(conditionalNode.ElseBody);
                    }
                }
            }
            else if (node is BlockNode blockNode)
            {
                ExecBlock(blockNode);
            }
        }

        private CustomObject? ExecParam(INode node)
        {
            if (node is ConstNode constNode)
            {
                return constNode.Constant.Value;
            }
            else if (node is VariableNode variableNode)
            {
                if (variableNode.Variable.Type == TokenType.FUNCEXEC)
                {
                    return ExecFunction(variableNode);
                }
                return Variables[variableNode.Variable.Value];
            }
            else if (node is BinaryOperationNode binaryNode)
            {
                return ExecBinary(binaryNode);
            }
            return null;
        }

        private void ExecUnary(INode node)
        {
            if (node == null)
            {
                return;
            }
            if (node is UnaryOperationNode unaryNode)
            {
                var variable = unaryNode.Operand as VariableNode;
                if (unaryNode.Operator.Value == "++")
                {
                    Variables[variable.Variable.Value] += 1;
                }
                else if (unaryNode.Operator.Value == "--")
                {
                    Variables[variable.Variable.Value] -= 1;
                }
            }
            return;
        }

        public CustomObject? ExecBinary(INode node)
        {
            if (node == null)
            {
                return null;
            }
            if (node is BinaryOperationNode binaryNode)
            {
                if (binaryNode.Operator.Type == TokenType.ASSIGN)
                {
                    var variable = binaryNode.LeftNode as VariableNode;
                    if (binaryNode.RightNode is VariableNode variable2)
                    {
                        if (variable2.Variable.Type == TokenType.FUNCEXEC)
                        {
                            Variables[variable.Variable.Value] = ExecFunction(variable2);
                            return null;
                        }
                        Variables[variable.Variable.Value] = Variables[variable2.Variable.Value];
                        return null;
                    }
                    else if (binaryNode.RightNode is ConstNode constant)
                    {
                        Variables[variable.Variable.Value] = constant.Constant.Value;
                        return null;
                    }
                    else if (binaryNode.RightNode is BinaryOperationNode binaryNode2)
                    {
                        Variables[variable.Variable.Value] = ExecBinary(binaryNode2);
                        return null;
                    }        
                }
                else
                {
                    if (binaryNode.LeftNode is VariableNode variable1 && binaryNode.RightNode is VariableNode variable2)
                    {
                        return BinaryOperation(Variables[variable1.Variable.Value], binaryNode.Operator.Value, Variables[variable2.Variable.Value]);
                    }
                    else if (binaryNode.LeftNode is ConstNode const1 && binaryNode.RightNode is ConstNode const2)
                    {
                        return BinaryOperation(const1.Constant.Value, binaryNode.Operator.Value, const2.Constant.Value);
                    }
                    else if (binaryNode.LeftNode is VariableNode variable3 && binaryNode.RightNode is ConstNode const3)
                    {
                        return BinaryOperation(Variables[variable3.Variable.Value], binaryNode.Operator.Value, const3.Constant.Value);
                    }
                    else if (binaryNode.LeftNode is ConstNode const4 && binaryNode.RightNode is VariableNode variable4)
                    {
                        return BinaryOperation(const4.Constant.Value, binaryNode.Operator.Value, Variables[variable4.Variable.Value]);
                    }
                    else if (binaryNode.LeftNode is VariableNode variable5 && binaryNode.RightNode is BinaryOperationNode binaryOperation)
                    {
                        return BinaryOperation(Variables[variable5.Variable.Value], binaryNode.Operator.Value, ExecBinary(binaryOperation));
                    }
                    else if (binaryNode.LeftNode is ConstNode const5 && binaryNode.RightNode is BinaryOperationNode binaryOperation2)
                    {
                        return BinaryOperation(const5.Constant.Value, binaryNode.Operator.Value, ExecBinary(binaryOperation2));
                    }
                    else if (binaryNode.LeftNode is VariableTypeNode type && binaryNode.RightNode is VariableNode variable6)
                    {
                        return BinaryOperation(type.VariableType.Value, binaryNode.Operator.Value, Variables[variable6.Variable.Value]);
                    }
                    else if (binaryNode.LeftNode is VariableTypeNode type2 && binaryNode.RightNode is ConstNode const6)
                    {
                        return BinaryOperation(type2.VariableType.Value, binaryNode.Operator.Value, const6.Constant.Value);
                    }

                }
            }
            return null;
        }

        private CustomObject? ExecFunction(INode functionExecNode)
        {
            string functionStr;
            string[] functionParams = {""};
            if (functionExecNode is VariableNode tmp1)
            {
                functionStr = tmp1.Variable.Value.ToString().Replace(" ", "").Replace("(", " ").Replace(")", "").Replace(",", " ");
                functionParams = functionStr.Split(" ");
            }
            else if (functionExecNode is FunctionExecNode tmp2)
            {
                functionStr = tmp2.Func.Value.ToString().Replace(" ", "").Replace("(", " ").Replace(")", "").Replace(",", " ");
                functionParams = functionStr.Split(" ");
            }
            
            if (functionParams[0] == "strlen")
            {
                return Variables[functionParams[1]].ToString().Length;
            }
            else if (functionParams[0] == "cin.getline")
            {
                Variables[functionParams[1]] = Console.ReadLine();
                return null;
            }
            else if (functionParams[0] == "pow")
            {
                return (int)Math.Pow(ParseParam(functionParams[1]), ParseParam(functionParams[2]));
            }
            return null;
        }

        private bool CheckCondition(INode node)
        {
            var binaryNode = node as BinaryOperationNode;
            if (binaryNode.LeftNode is VariableNode variable1 && binaryNode.RightNode is VariableNode variable2)
            {
                return CompareOperation(Variables[variable1.Variable.Value], binaryNode.Operator.Value, Variables[variable2.Variable.Value]);
            }
            else if (binaryNode.LeftNode is ConstNode const1 && binaryNode.RightNode is ConstNode const2)
            {
                return CompareOperation(const1.Constant.Value, binaryNode.Operator.Value, const2.Constant.Value);
            }
            else if (binaryNode.LeftNode is VariableNode variable3 && binaryNode.RightNode is ConstNode const3)
            {
                return CompareOperation(Variables[variable3.Variable.Value], binaryNode.Operator.Value, const3.Constant.Value);
            }
            else if (binaryNode.LeftNode is ConstNode const4 && binaryNode.RightNode is VariableNode variable4)
            {
                return CompareOperation(const4.Constant.Value, binaryNode.Operator.Value, Variables[variable4.Variable.Value]);
            }
            else if (binaryNode.LeftNode is VariableNode variable5 && binaryNode.RightNode is BinaryOperationNode binaryOperation)
            {
                return CompareOperation(Variables[variable5.Variable.Value], binaryNode.Operator.Value, ExecBinary(binaryOperation));
            }
            else if (binaryNode.LeftNode is ConstNode const5 && binaryNode.RightNode is BinaryOperationNode binaryOperation2)
            {
                return CompareOperation(const5.Constant.Value, binaryNode.Operator.Value, ExecBinary(binaryOperation2));
            }
            return false;
        }

        private CustomObject ParseParam(string param)
        {
            Lexer lexer = new Lexer();
            lexer.variables = VariableNames;
            lexer.Parse(param.Insert(param.Length, ";"));
            SyntaxParser parser = new SyntaxParser(lexer);
            BlockNode root = parser.ParseBlock() as BlockNode;
            Semantic semantic = new Semantic(parser);
            semantic.Parse(root);
            if (root.Nodes.Count > 0)
            {
                return ExecBinary(root.Nodes[0]);
            }
            else if (int.TryParse(param, out int intResult))
            {
                return intResult;
            }
            else if (float.TryParse(param, out float floatResult)) {
                return floatResult;
            }
            else
            {
                return Variables[param];
            }
        }
    }
}
