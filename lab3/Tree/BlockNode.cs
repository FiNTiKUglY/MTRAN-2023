using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab2.Tree
{
    public class BlockNode: INode
    {
        public List<INode> Nodes { get; set; } = new();

        public void AddNode(INode node)
        {
            Nodes.Add(node);
        }
    }
}
