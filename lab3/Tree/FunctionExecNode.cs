using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab2.Tree
{
    public class FunctionExecNode: INode
    {
        public Token Func { get; set; }
        public FunctionExecNode(Token func)
        {
            Func = func;
        }
    }
}
