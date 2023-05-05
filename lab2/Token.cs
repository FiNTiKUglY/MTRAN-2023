using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace lab2
{
    public enum TokenType
    {
        VARIABLE,
        VARIABLETYPE,
        INT,
        LONG,
        FLOAT,
        STRING,
        CHAR,
        FUNC,
        FUNCEXEC,
        OPERATOR,
        KEYWORD,
        CONDITIONAL,
        FORCYCLE,
        WHILECYCLE,
        BINARY,
        UNARY,
        STREAM,
        SEMICOLON,
        OPENBRACKET,
        CLOSEBRACKET,
        OPENBLOCK,
        CLOSEBLOCK,
        OPENINDEX,
        CLOSEINDEX,
        CIN,
        COUT,
        ASSIGN,
        NEW
    }
    public class Token
    {
        public CustomObject Value { get; set; }
        public TokenType Type { get; set; }

        public Token(CustomObject value, TokenType type)
        {
            Value = value;
            switch(type)
            {
                case TokenType.KEYWORD:
                    switch(value.ToString())
                    {
                        case "if":
                            Type = TokenType.CONDITIONAL;
                            break;
                        case "else":
                            Type = TokenType.CONDITIONAL;
                            break;
                        case "switch":
                            Type = TokenType.CONDITIONAL;
                            break;
                        case "while":
                            Type = TokenType.WHILECYCLE;
                            break;
                        case "do":
                            Type = TokenType.WHILECYCLE;
                            break;
                        case "for":
                            Type = TokenType.FORCYCLE;
                            break;
                        case "cin":
                            Type = TokenType.CIN;
                            break;
                        case "cout":
                            Type = TokenType.COUT;
                            break;
                        case "new":
                            Type = TokenType.NEW;
                            break;
                        default:
                            Type = TokenType.KEYWORD;
                            break;
                    }
                    break;
                case TokenType.OPERATOR:
                    switch (value.ToString())
                    {
                        case "+":
                            Type = TokenType.BINARY;
                            break;
                        case "-":
                            Type = TokenType.BINARY;
                            break;
                        case "*":
                            Type = TokenType.BINARY;
                            break;
                        case "/":
                            Type = TokenType.BINARY;
                            break;
                        case "%":
                            Type = TokenType.BINARY;
                            break;
                        case ">":
                            Type = TokenType.BINARY;
                            break;
                        case "<":
                            Type = TokenType.BINARY;
                            break;
                        case ">=":
                            Type = TokenType.BINARY;
                            break;
                        case "<=":
                            Type = TokenType.BINARY;
                            break;
                        case "=":
                            Type = TokenType.ASSIGN;
                            break;
                        case "==":
                            Type = TokenType.BINARY;
                            break;
                        case "+=":
                            Type = TokenType.BINARY;
                            break;
                        case "-=":
                            Type = TokenType.BINARY;
                            break;
                        case "*=":
                            Type = TokenType.BINARY;
                            break;
                        case "/=":
                            Type = TokenType.BINARY;
                            break;
                        case "++":
                            Type = TokenType.UNARY;
                            break;
                        case "--":
                            Type = TokenType.UNARY;
                            break;
                        case "<<":
                            Type = TokenType.STREAM;
                            break;
                        case ">>":
                            Type = TokenType.STREAM;
                            break;
                        case ";":
                            Type = TokenType.SEMICOLON;
                            break;
                        case "(":
                            Type = TokenType.OPENBRACKET;
                            break;
                        case ")":
                            Type = TokenType.CLOSEBRACKET;
                            break;
                        case "{":
                            Type = TokenType.OPENBLOCK;
                            break;
                        case "}":
                            Type = TokenType.CLOSEBLOCK;
                            break;
                        case "[":
                            Type = TokenType.OPENINDEX;
                            break;
                        case "]":
                            Type = TokenType.CLOSEINDEX;
                            break;
                        default:
                            Type = TokenType.OPERATOR;
                            break;
                    }
                    break;
                default:
                    Type = type;
                    break;
            }
        }
    }
}
