using Common;
using Common.Log;

namespace BehaviourTree
{

    //判断是否符合中场开球的条件
    public class ConditionalControlBall : BTConditional
    {
        public ConditionalControlBall()
        {
            Name = "ConditionalControlBall";
            DisplayName = "条件:控球状态";
            NodeType = "ConditionalControlBall";
        }

        public override bool Check()
        {
            if (null == m_kPlayer)
            {
                int iID = m_kDatabase.GetDataID(BTConstant.Player);
                m_kPlayer = m_kDatabase.GetData<LLPlayer>(iID);
            }

            if (null == m_kPlayer)
                return false;
            if (true == m_kPlayer.IsCtrlBall)
            {
                if(m_kPreState != m_kPlayer.State)
                {
                    LogManager.Instance.RedLog("持球球员的行为：====>PlayerId==" + m_kPlayer.PlayerBaseInfo.HeroID
                        + "  队伍==" + m_kPlayer.Team.TeamColor
                        + "  状态==" + m_kPlayer.State);
                    m_kPreState = m_kPlayer.State;

                }
                return true;
            }
            //additional temporary condition
            return IsPassingBall() || IsLosingBall();
        }

        private bool IsPassingBall()
        {
            if( !m_kPlayer.IsCtrlBall &&
               (m_kPlayer.State == EPlayerState.PassBall ||
             m_kPlayer.State == EPlayerState.Heading_Tackle_ToPass))
            {
                //wait for the end of passBall state
                return true;
            }
            return false;
        }

        private bool IsLosingBall()
        {
            return (m_kPlayer.State == EPlayerState.Break_Through_Failed ||
                    m_kPlayer.State == EPlayerState.Avoid_Block_Tackle_Failed ||
                    m_kPlayer.State == EPlayerState.Avoid_Sliding_Tackle_Failed ||
                    m_kPlayer.State == EPlayerState.Heading_Tackle_Failed);            
        }


        private LLPlayer m_kPlayer = null;
        private EPlayerState m_kPreState = EPlayerState.PS_END;
    }
}