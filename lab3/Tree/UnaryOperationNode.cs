using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab2.Tree
{
    public class UnaryOperationNode : INode
    {
        public Token Operator { get; set; }
        public INode Operand { get; set; }

        public UnaryOperationNode(Token op, INode operand)
        {
            Operator = op;
            Operand = operand;
        }
    }
}
