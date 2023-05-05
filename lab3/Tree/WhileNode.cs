namespace lab2.Tree
{
    public class WhileNode : INode
    {
        public INode Condition { get; set; }
        public INode Body { get; set; }

        public WhileNode(INode condition, INode body)
        {
            Condition = condition;
            Body = body;
        }
    }
}
