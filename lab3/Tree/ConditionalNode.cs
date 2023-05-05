namespace lab2.Tree
{
    public class ConditionalNode : INode
    {
        public INode Condition { get; set; }
        public INode Body { get; set; }
        public INode? ElseBody { get; set; }

        public ConditionalNode(INode condition, INode body, INode? elseBody)
        {
            Condition = condition;
            Body = body;
            ElseBody = elseBody;
        }
    }
}
