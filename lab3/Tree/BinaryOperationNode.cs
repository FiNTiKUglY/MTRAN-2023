using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab2.Tree
{
    public class BinaryOperationNode : INode
    {
        public Token Operator { get; set; }
        public INode LeftNode { get; set; }
        public INode RightNode { get; set; }

        public BinaryOperationNode(Token op, INode leftNode, INode rightNode)
        {
            Operator = op;
            LeftNode = leftNode;
            RightNode = rightNode;
        }
    }
}
