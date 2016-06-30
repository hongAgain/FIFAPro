namespace BehaviourTree
{
    /*
     * 组合节点的功能：可以添加各个节点然后顺序执行，或是添加条件来决定哪些节点能运行
     */
    public class BTComposite : BTNode 
    {
        public BTComposite():base()
        {
            Name = "Composite";
            DisplayName = "Composite";
            NodeType = "BTComposite";
        }
	}
}