namespace BehaviourTree
{

    //判断是否符合中场开球的条件
    public class ConditionalUnStopBall : BTConditional
    {
        public ConditionalUnStopBall()
        {
            Name = "UnStopBall";
            DisplayName = "条件:不停球";
            NodeType = "ConditionalUnStopBall";
        }


        public override void Activate(BTDatabase kDatabase)
        {
            base.Activate(kDatabase);
            int iID = kDatabase.GetDataID(BTConstant.Player);
            m_kPlayer = kDatabase.GetData<LLPlayer>(iID);
        }

        public override bool Check()
        {
            if (null == m_kPlayer)
                return false;
           // return (!m_kPlayer.IsNeedStopBall) && (m_kPlayer.CtrlBall);
            return false;
        }
        private LLPlayer m_kPlayer = null;

    }
}