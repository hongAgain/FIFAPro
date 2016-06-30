namespace BehaviourTree
{
    public class BTParallel : BTComposite 
    {
        public BTParallel()
            : base()
        {
            Name = "Parallel";
            DisplayName = "Parallel";
            NodeType = "BTParallel";
        }

		public override BTResult Tick (double fTime) 
        {
            BTResult kResult = BTResult.Failed;
            for(int iIdx = 0;iIdx < m_kChildList.Count;iIdx++)
            {
                BTResult kRet = m_kChildList[iIdx].Tick(fTime);
                if (BTResult.Failed == kRet)
                    return BTResult.Failed;
                if (BTResult.Running == kRet)
                    kResult = kRet;
            }

            return kResult;
		}
	}

}