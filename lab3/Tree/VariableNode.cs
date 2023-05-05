using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab2.Tree
{
    public class VariableNode: INode
    {
        public Token Variable { get; set; }

        public VariableNode(Token variable)
        {
            Variable = variable;
        }
    }
}
