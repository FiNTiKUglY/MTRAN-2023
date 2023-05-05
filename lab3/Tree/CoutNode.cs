namespace lab2.Tree
{
    public class CoutNode : INode
    {
        public List<INode> Parameters { get; set; }

        public CoutNode(List<INode> parameters)
        {
            Parameters = parameters;
        }
    }
}
