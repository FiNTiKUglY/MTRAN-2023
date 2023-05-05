namespace lab2.Tree
{
    public class ForNode : INode
    {
        public INode Init { get; set; }
        public INode Condition { get; set; }
        public INode Iterator { get; set; }
        public INode Body { get; set; }

        public ForNode(INode init, INode condition, INode iterator, INode body)
        {
            Init = init;
            Condition = condition;
            Iterator = iterator;
            Body = body;
        }
    }
}
