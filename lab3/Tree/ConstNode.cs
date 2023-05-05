using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab2.Tree
{
    public class ConstNode: INode
    {
        public Token Constant { get; set; }

        public ConstNode(Token constant)
        {
            Constant = constant;
        }
    }
}
