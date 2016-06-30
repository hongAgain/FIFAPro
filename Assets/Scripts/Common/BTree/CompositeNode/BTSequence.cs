namespace BehaviourTree
{
    public class BTSequence : BTComposite 
    {
        public BTSequence()
            : base()
        {
            Name = "Sequence";
            DisplayName = "Sequence";
            NodeType = "BTSequence";
        }

        public override BTResult Tick(double fTime) 
        {
            if (m_iActiveChildIndex == -1)
            {
                m_iActiveChildIndex = 0;
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