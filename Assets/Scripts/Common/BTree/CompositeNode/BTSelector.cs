namespace BehaviourTree
{
    /*
     * 选择节点: 只要有一个节点正在运行或是运行成功就返回
     */
    public class BTSelector : BTComposite 
    {
        public BTSelector():base()
        {
            Name = "Selector";
            DisplayName = "Selector";
            NodeType = "BTSelector";
        }
        public override BTResult Tick(double fTime)
        {
            for (int iIdx = 0; iIdx < m_kChildList.Count;iIdx++ )
            {
                BTResult kRet = m_kChildList[iIdx].Tick(fTime);
                if (BTResult.Success == kRet)
                {
                    return BTResult.Success;
                }
                else if (BTResult.Running == kRet)
                {
                    return BTResult.Running;
                }
            }
            return BTResult.Failed;
        }
	}
}