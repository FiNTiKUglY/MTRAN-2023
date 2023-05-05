using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab2.Tree
{
    public class FunctionNode: INode
    {
        public Token Function { get; set; }
        public List<Token> Parameters { get; set; }
        public INode Body { get; set; }

        public FunctionNode(Token function, List<Token> parameters, INode body)
        {
            Function = function;
            Parameters = parameters;
            Body = body;
        }
    }
}
