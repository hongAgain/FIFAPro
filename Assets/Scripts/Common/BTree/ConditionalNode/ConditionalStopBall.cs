namespace BehaviourTree
{

    //判断是否符合中场开球的条件
    public class ConditionalStopBall : BTConditional
    {
        public ConditionalStopBall()
        {
            Name = "StopBall";
            DisplayName = "条件:停球";
            NodeType = "ConditionalStopBall";
        }

        public override void Activate(BTDatabase kDatabase)
        {
            base.Activate(kDatabase);
            int iID = kDatabase.GetDataID(BTConstant.Player);
            m_kPlayer = kDatabase.GetData<LLPlayer>(iID);
        }

        public override bool Check()
        {
            if(null == m_kPlayer)
                return false;
         //   return m_kPlayer.IsNeedStopBall;
            return true;
        }
        private LLPlayer m_kPlayer = null;
    }
}