using lab2.Tree;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab2
{
    public class CustomDictionary
    {
        public Dictionary<string, CustomObject?> _dictionary;
        public Dictionary<string, string> VariableNames { get; set; }

        public CustomDictionary(Dictionary<string, string> names) 
        {
            _dictionary = new Dictionary<string, CustomObject?>();
            VariableNames = names;
        }

        public void Add(string key, CustomObject? value)
        {
            _dictionary.Add(key, value);
        }

        public CustomObject? this[string key]
        {
            get
            {
                if (key.ToString().Contains('['))
                {
                    string valueName = key.ToString().Replace(" ", "").Replace("[", " ").Replace("]", "");
                    string[] valueIndex = valueName.Split(" ");
                    return _dictionary[valueIndex[0]][ParseParam(valueIndex[1])];
                }
                else
                {
                    return _dictionary[key];
                }
            }
            set
            {
                if (key.ToString().Contains('['))
                {
                    string valueName = key.ToString().Replace(" ", "").Replace("[", " ").Replace("]", "");
                    string[] valueIndex = valueName.Split(" ");
                    _dictionary[valueIndex[0]][ParseParam(valueIndex[1])] = value;
                }
                else
                {
                    _dictionary[key] = value;
                }
            }
        }
        private CustomObject? ParseParam(string param)
        {
            Lexer lexer = new Lexer();
            lexer.variables = VariableNames;
            lexer.Parse(param.Insert(param.Length, ";"));
            SyntaxParser parser = new SyntaxParser(lexer);
            BlockNode root = parser.ParseBlock() as BlockNode;
            Starter starter = new Starter(parser, root);
            starter.Variables._dictionary = _dictionary;
            if (root.Nodes.Count > 0)
            {
                return starter.ExecBinary(root.Nodes[0]);
            }
            else if (int.TryParse(param, out int intResult))
            {
                return intResult;
            }
            else if (float.TryParse(param, out float floatResult))
            {
                return floatResult;
            }
            else
            {
                return _dictionary[param];
            }
        }
    }
}
