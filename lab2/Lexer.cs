using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace lab2
{
    public class Lexer
    {
        public Dictionary<string, string> variables = new();
        public Dictionary<string, string> keywords = new();
        public Dictionary<string, string> operators = new();
        public List<CustomObject> consts = new();
        public List<Token> allTokens = new List<Token> { new Token("a", TokenType.CHAR), new Token("b", TokenType.CHAR) };

        public Dictionary<string, string> keywordEx = new Dictionary<string, string>
        {
            { "do", "key word" },
            { "class", "key word" },
            { "public", "key word" },
            { "while", "key word" },
            { "for", "key word" },
            { "if", "key word" },
            { "else", "key word" },
            { "switch", "key word" },
            { "case", "key word" },
            { "default", "key word" },
            { "break", "key word" },
            { "continue", "key word" },
            { @"cout[^\.]", "key word" },
            { @"cin[^\.]", "key word" },
            { "new", "key word" },
            { "endl", "key word" },
            { "true", "key word" },
            { "false", "key word" },
            { "const", "key word" },
            { "return", "key word" },
            { @"int\*", "key word of variable type" },
            { @"string\*", "key word of variable type" },
            { @"float\*", "key word of variable type" },
            { @"bool\*", "key word of variable type" },
            { @"double\*", "key word of variable type" },
            { @"char\*", "key word of variable type" },
            { "void", "key word of variable type" },
            { "int", "key word of variable type" },
            { "string", "key word of variable type" },
            { "float", "key word of variable type" },
            { "bool", "key word of variable type" },
            { "double", "key word of variable type" },
            { "char", "key word of variable type" },
            { "long", "key word of variable type" },
            { "using", "key word of variable type" },
            { "namespace", "key word of variable type" },
            { "unsigned", "key word of variable type" },
        };

        public Dictionary<string, string> funcs = new Dictionary<string, string>
        {
            { "cin.getline()", "void" },
            { "setlocale()", "void" },
            { "strlen()", "int" },
            { "pow()", "int" }
        };


        public List<string> operatorEx = new List<string>
        {
            @"(\+\+)[\s\)\;]",
            @"(\-\-)[\s\)\;]",
            @"(\+\=)[\s\)\;]",
            @"(\*\=)[\s\)\;]",
            @"(\/\=)[\s\)\;]",
            @"(\<\<)[\s]",
            @"(\>\>)[\s]",
            @"\<\=[\s]",
            @"\>\=[\s]",
            @"\=\=[\s]",
            @"\!\=[\s]",
            @"\-[^\+\-\=\<\>\/\*\)]",
            @"\+[^\+\-\=\<\>\/\*\)]",
            @"\*[^\+\-\=\<\>\/\*\)]",
            @"\/[^\+\-\=\<\>\/\*\)]",
            @"\=[^\+\-\=\<\>\/\*\)]",
            @"\<[^\+\-\=\<\>\/\*\)]",
            @"\>[^\+\-\=\<\>\/\*\)]",
            @"\%[^\+\-\=\<\>\/\*\)]"
        };

        public List<string> operatorNon = new List<string>
        {
            @"\[",
            @"\]",
            @"\(",
            @"\)",
            @"\;",
            @"\{",
            @"\}"
        };

        public List<string> varTypes = new List<string>
        {
            @"int*",
            @"float*",
            @"double*",
            @"string*",
            @"char*",
            @"bool*",
            @"void",
            @"int",
            @"float",
            @"double",
            @"string",
            @"char",
            @"bool",
            @"namespace"
        };

        public List<string> lineEnd = new List<string>
        {
            @";",
            @"{",
            @"}",
            @")"
        };

        public List<Token> Parse(string text)
        {
            int lineNum = 0;
            List<string> lines = text.Split('\n').ToList();
            foreach (string line in lines)
            {
                int pos = 0;
                lineNum++;
                while (pos < line.Length)
                {
                    bool isValid = false;
                    if (line[pos] == ' ' || line[pos] == '\t')
                    {
                        pos++;
                        continue;
                    }
                    var currString = line.Substring(pos).Trim();

                    if (currString == "")
                    {
                        break;
                    }

                    /*if (!lineEnd.Any(x => currString.EndsWith(x)))
                    {
                        throw new Exception();
                    }*/

                    foreach (var op in operatorEx)
                    {
                        var regex = new Regex($"^{op}");
                        var match = regex.Match(currString);
                        if (match.Success)
                        {
                            isValid = true;
                            operators.TryAdd($"{match.Value.Remove(match.Value.Length - 1)}", "operator");
                            allTokens.Add(new Token(match.Value.Remove(match.Value.Length - 1), TokenType.OPERATOR));
                            pos += match.Length - 1;
                            break;
                        }
                    }

                    foreach (var keyword in keywordEx)
                    {
                        var regex = new Regex($@"^{keyword.Key}\W");
                        var match = regex.Match(currString);
                        if (match.Success)
                        {
                            isValid = true;
                            keywords.TryAdd($"{Regex.Replace(keyword.Key, @"(\[.*\]\W*|\\\W*)", "")}", $"{keyword.Value}");
                            allTokens.Add(new Token(Regex.Replace(keyword.Key, @"(\[.*\]\W*|\\*)", ""), TokenType.KEYWORD));
                            pos += allTokens[^1].Value.ToString().Length;
                            break;
                        }
                    }

                    foreach (var op in operatorNon)
                    {
                        var regex = new Regex($"^{op}");
                        var match = regex.Match(currString);
                        if (match.Success)
                        {
                            isValid = true;
                            allTokens.Add(new Token(match.Value, TokenType.OPERATOR));
                            pos += match.Length;
                            break;
                        }
                    }

                    if (isValid == false)
                    {
                        var regex = new Regex(@"^[\d]+[\.][\d]+");
                        var match = regex.Match(currString);
                        if (match.Success)
                        {
                            isValid = true;
                            var match2 = match.Value.Replace('.', ',');
                            consts.Add(float.Parse(match2));
                            allTokens.Add(new Token(float.Parse(match2), TokenType.FLOAT));
                            pos += match.Length;
                            if (varTypes.Any(x => allTokens[^3].Value.ToString().Equals(x)))
                            {
                                allTokens[^3].Type = TokenType.VARIABLETYPE;
                            }
                        }
                    }

                    if (isValid == false)
                    {
                        var regex = new Regex(@"^\d+");
                        var match = regex.Match(currString);
                        if (match.Success)
                        {
                            isValid = true;
                            consts.Add(int.Parse(match.Value));
                            allTokens.Add(new Token(int.Parse(match.Value), TokenType.INT));
                            pos += match.Length;
                            if (varTypes.Any(x => allTokens[^3].Value.ToString().Equals(x)))
                            {
                                allTokens[^3].Type = TokenType.VARIABLETYPE;
                            }
                        }
                    }

                    if (isValid == false)
                    {
                        var regex = new Regex(@"^(\w*\.)*\w+\(.*\)");
                        var match = regex.Match(currString);
                        var match2 = Regex.Replace(match.ToString(), @"\(.*\)", "()");
                        if (match.Success)
                        {
                            if (funcs.Keys.Any(x => match2.ToString().Equals(x)) || varTypes.Any(x => allTokens[^1].Value.ToString().Equals(x)))
                            {
                                isValid = true;
                                funcs.TryAdd($"{match2}", $"{allTokens[^1].Value}");
                                allTokens.Add(new Token(match.Value, TokenType.FUNCEXEC));
                                pos += match.Length;
                            }
                            else
                            {
                                throw new Exception($"{lineNum}:{pos}: undefined name {match.Value}\n\t{line}");
                            }
                            if (varTypes.Any(x => allTokens[^2].Value.ToString().Equals(x))) 
                            {
                                allTokens[^2].Type = TokenType.VARIABLETYPE;
                                allTokens[^1].Type = TokenType.FUNC;
                            }
                        }
                    }

                    if (isValid == false)
                    {
                        var regex = new Regex(@"^\w+\[.*?\]");
                        var match = regex.Match(currString);
                        var match2 = Regex.Replace(match.ToString(), @"\[.*?\]", "");
                        if (match.Success)
                        {
                            if (varTypes.Any(x => allTokens[^1].Value.ToString().Equals(x)) || variables.Keys.Any(x => match2.ToString().Equals(x)))
                            {
                                isValid = true;
                                variables.TryAdd($"{match2}", $"{allTokens[^1].Value} array");
                                allTokens.Add(new Token(match.Value, TokenType.VARIABLE));
                                pos += match.Length;
                            }
                            else
                            {
                                throw new Exception($"{lineNum}:{pos}: undefined name {match.Value}\n\t{line}");
                            }
                            if (varTypes.Any(x => allTokens[^2].Value.ToString().Equals(x)))
                            {
                                allTokens[^2].Type = TokenType.VARIABLETYPE;
                            }
                        }
                    }

                    if (isValid == false)
                    {
                        var regex = new Regex(@"^\w+");
                        var match = regex.Match(currString);
                        if (match.Success)
                        {
                            if (varTypes.Any(x => allTokens[^1].Value.ToString().ToString().Equals(x)) || variables.Keys.Any(x => match.Value.ToString().Equals(x)))
                            {
                                isValid = true;
                                if (allTokens[^1].Value.ToString().Contains('*'))
                                {
                                    variables.TryAdd($"{match.Value}", $"{allTokens[^1].Value.ToString().Remove(allTokens[^1].Value.ToString().Length-1, 1)} array");
                                }
                                else
                                {
                                    variables.TryAdd($"{match.Value}", $"{allTokens[^1].Value}");
                                }
                                allTokens.Add(new Token(match.Value, TokenType.VARIABLE));
                                pos += match.Length;
                            }
                            else
                            {
                                throw new Exception($"{lineNum}:{pos}: undefined name {match.Value}\n\t{line}");
                            }
                            if (varTypes.Any(x => allTokens[^2].Value.ToString().Equals(x)))
                            {
                                allTokens[^2].Type = TokenType.VARIABLETYPE;
                            }
                            if (varTypes.Any(x => allTokens[^3].Value.ToString().Equals(x)))
                            {
                                allTokens[^3].Type = TokenType.VARIABLETYPE;
                            }
                        }
                    }

                    if (isValid == false)
                    {
                        var regex = new Regex(@"'.*?'");
                        var match = regex.Match(currString);
                        if (match.Success)
                        {
                            isValid = true;
                            consts.Add(char.Parse(match.Value.Replace(@"'", "")));
                            allTokens.Add(new Token(char.Parse(match.Value.Replace(@"'", "")), TokenType.CHAR));
                            pos += match.Length;
                        }
                    }

                    if (isValid == false)
                    {
                        var regex = new Regex(@""".*?""");                      
                        var match = regex.Match(currString);
                        if (match.Success)
                        {
                            isValid = true;
                            consts.Add(match.Value.Replace(@"""", ""));
                            allTokens.Add(new Token(match.Value.Replace(@"""", ""), TokenType.STRING));
                            pos += match.Length;
                        }
                    }

                    if (isValid == false)
                    {
                        throw new Exception($"{lineNum}:{pos}: undefined expression\n\t{line}");
                    }
                }
            }
            allTokens.RemoveRange(0, 2);
            return allTokens;
        }
    }
}
