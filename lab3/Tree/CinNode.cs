namespace lab2.Tree
{
    public class CinNode : INode
    {
        public List<INode> Parameters { get; set; }

        public CinNode(List<INode> parameters)
        {
            Parameters = parameters;
        }
    }
}
