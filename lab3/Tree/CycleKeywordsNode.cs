using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab2.Tree
{
    public class CycleKeywordsNode : INode
    {
        public Token Keyword { get; set; }
        public CycleKeywordsNode(Token keyword)
        {
            Keyword = keyword;
        }
    }
}
