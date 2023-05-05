using lab2.Tree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace lab2
{
    public class Semantic
    {
        public Dictionary<string, string> Variables;
        public Dictionary<string, string> Funcs;
        private int cycleLevel = 0;
        private int conditionalLevel = 0;

        public Semantic(SyntaxParser parser)
        {
            Variables = parser.Variables;
            Funcs = parser.Funcs;
        }

        private bool CheckType(string type, string type2)
        {
            if (type != type2)
            {
                if (type == "float" && type2 == "int")
                {
                    return true;
                }
                if (type == "int" && type2 == "float")
                {
                    return true;
                }
                if (type == "int" && type2 == "char")
                {
                    return true;
                }
                if (type == "char" && type2 == "int")
                {
                    return true;
                }
                return false;
            }
            return true;
        }

        public string? Parse(INode? rootNode)
        {
            if (rootNode == null)
            {
                return null;
            }

            if (rootNode is BlockNode node)
            {
                foreach (var elem in node.Nodes)
                {
                    Parse(elem);
                }
            }

            if (rootNode is FunctionNode functionNode)
            {
                foreach (var elem in functionNode.Parameters)
                {
                    Console.Write(elem.Value.ToString());
                    Console.Write(" ");
                }
                Parse(functionNode.Body);
            }

            if (rootNode is WhileNode whileNode)
            {
                Parse(whileNode.Condition);
                cycleLevel++;
                Parse(whileNode.Body);
                cycleLevel--;
            }

            if (rootNode is ConditionalNode ifNode)
            {
                Parse(ifNode.Condition);
                conditionalLevel++;
                Parse(ifNode.Body);
                Parse(ifNode.ElseBody);
                conditionalLevel--;
            }

            if (rootNode is CycleKeywordsNode cycleKeywordsNode)
            {
                if (cycleLevel == 0 || conditionalLevel == 0) 
                {
                    throw new Exception($"Icorrect usage of {cycleKeywordsNode.Keyword.Value}");
                }
            }

            if (rootNode is CoutNode coutNode)
            {
                foreach (var elem in coutNode.Parameters)
                {
                    Parse(elem);
                }
            }

            if (rootNode is CinNode cinNode)
            {
                foreach (var elem in cinNode.Parameters)
                {
                    Parse(elem);
                }
            }

            if (rootNode is ForNode forNode)
            {
                Parse(forNode.Init);
                Parse(forNode.Condition);
                Parse(forNode.Iterator);
                cycleLevel++;
                Parse(forNode.Body);
                cycleLevel--;
            }

            if (rootNode is BinaryOperationNode binaryOperationNode)
            {
                string? type, type2;
                string variableString;
                if (binaryOperationNode.Operator.Type == TokenType.NEW)
                {
                    if (binaryOperationNode.RightNode is VariableNode sizeNode)
                    {
                        if (Variables[sizeNode.Variable.Value] != "int")
                        {
                            throw new Exception("Size of array must be int");
                        } 
                    }
                    else if (binaryOperationNode.RightNode is ConstNode sizeNode2)
                    {
                        if (sizeNode2.Constant.Value.Type != "int")
                        {
                            throw new Exception("Size of array must be int");
                        }
                    }
                    else
                    {
                        throw new Exception("Size of array must be int");
                    }
                    if (binaryOperationNode.LeftNode is VariableTypeNode typeNode)
                    {
                        return $"{typeNode.VariableType.Value} array";
                    }                   
                }
                if (binaryOperationNode.LeftNode is VariableNode variable)
                {
                    var regex = new Regex(@"^(\w*\.)*\w+\(.*\)");
                    var match = regex.Match(variable.Variable.Value);
                    var match2 = Regex.Replace(match.ToString(), @"\(.*\)", "()");
                    if (match.Success)
                    {
                        type = Funcs[match2];
                        variableString = variable.Variable.Value;
                    }
                    else
                    {
                        regex = new Regex(@"^\w+\[.*?\]");
                        match = regex.Match(variable.Variable.Value);
                        match2 = Regex.Replace(match.ToString(), @"\[.*?\]", "");
                        if (match.Success)
                        {
                            type = Variables[match2].Split(' ')[0];
                            variableString = variable.Variable.Value;
                        }
                        else
                        {
                            type = Variables[variable.Variable.Value];
                            variableString = variable.Variable.Value;
                        }
                    }
                }
                else if (binaryOperationNode.LeftNode is ConstNode @const)
                {
                    type = @const.Constant.Value.Type;
                    variableString = @const.Constant.Value;
                }
                else
                {
                    type = Parse(binaryOperationNode.LeftNode);
                    variableString = "";
                }
                if (binaryOperationNode.RightNode is VariableNode variable2)
                {
                    var regex = new Regex(@"^(\w*\.)*\w+\(.*\)");
                    var match = regex.Match(variable2.Variable.Value);
                    var match2 = Regex.Replace(match.ToString(), @"\(.*\)", "()");
                    if (match.Success)
                    {
                        type2 = Funcs[match2];
                    }
                    else
                    {
                        regex = new Regex(@"^\w+\[.*?\]");
                        match = regex.Match(variable2.Variable.Value);
                        match2 = Regex.Replace(match.ToString(), @"\[.*?\]", "");
                        if (match.Success)
                        {
                            type2 = Variables[match2].Split(' ')[0];
                        }
                        else
                        {
                            type2 = Variables[variable2.Variable.Value];
                        }
                    }
                    if (!CheckType(type, type2))
                    {
                        throw new Exception($"Instead of {variable2.Variable.Value} requires {type}, not {type2}");
                    }
                }
                else if (binaryOperationNode.RightNode is ConstNode @const)
                {
                    if (!CheckType(type, @const.Constant.Value.Type))
                    {
                        throw new Exception($"Instead of {@const.Constant.Value} requires {type}, not {@const.Constant.Value.Type}");
                    }
                }
                else
                {
                    if (!CheckType(type, Parse(binaryOperationNode.RightNode)))
                    {
                        throw new Exception($"{variableString} doesn`t equals the expression type ({type})");
                    }
                }
                return type;
            }

            if (rootNode is UnaryOperationNode unaryOperationNode)
            {
                if (unaryOperationNode.Operand is VariableNode variable)
                {
                    if (Variables[variable.Variable.Value] == "string" || Variables[variable.Variable.Value] == "void")
                    {
                        throw new Exception($"{unaryOperationNode.Operator.Value} can`t be used with {Variables[variable.Variable.Value]}");
                    }
                }
                Parse(unaryOperationNode.Operand);
            }
            return null;
        }
    }
}
