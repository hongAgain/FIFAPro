namespace BehaviourTree
{
    public class BTConditionalSequence : BTComposite 
    {
        public BTConditionalSequence()
            : base()
        {
            Name = "ConditionalSequence";
            DisplayName = "ConditionalSequence";
            NodeType = "BTConditionalSequence";
        }

        public override BTResult Tick(double fTime) 
        {
            if (0 == m_kChildList.Count)
                return BTResult.Failed;
            BTResult kRet = m_kChildList[0].Tick(fTime);
            if (BTResult.Failed == kRet)
                return BTResult.Failed;

            if (m_iActiveChildIndex == -1)
            {
                m_iActiveChildIndex = 1;
            }

            for (; m_iActiveChildIndex < m_kChildList.Count; m_iActiveChildIndex++)
            {
                BTNode kActiveChild = m_kChildList[m_iActiveChildIndex];

                switch (kActiveChild.Tick(fTime))
                {
                    case BTResult.Running:
                        IsRunnint = true;
                        return BTResult.Running;
                    case BTResult.Success:
                        kActiveChild.Clear();
                        continue;
                    case BTResult.Failed:
                        kActiveChild.Clear();
                        m_iActiveChildIndex = -1;
                        IsRunnint = false;
                        return BTResult.Failed;
                }
            }

            m_iActiveChildIndex = -1;
            IsRunnint = false;
            return BTResult.Success;
		}

        public override void Clear ()
        {
            base.Clear();
            for (int i = 0; i < m_kChildList.Count; i++)
                m_kChildList[i].Clear();

            m_iActiveChildIndex = -1;
        }

        private int m_iActiveChildIndex = -1;

	}

}