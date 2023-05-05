using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab2.Tree
{
    public class VariableTypeNode: INode
    {
        public Token VariableType { get; set; }

        public VariableTypeNode(Token variableType)
        {
            VariableType = variableType;
        }
    }
}
