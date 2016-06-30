using Common;
using Common.Log;

namespace BehaviourTree
{
    public class ConditionalAtDefendBreakThroughArea : BTConditional
    {
        private LLPlayer m_kPlayer = null;

        public ConditionalAtDefendBreakThroughArea()
        {
			Name = "AtDefendBreakThroughArea";
			DisplayName = "条件:位于防守突破位置";
			NodeType = "ConditionalAtDefendBreakThroughArea";
        }
        
        public override bool Check()
        {
            if (m_kPlayer == null)
            {
                int iID = m_kDatabase.GetDataID(BTConstant.Player);
                m_kPlayer = m_kDatabase.GetData<LLPlayer>(iID);
            }
            if (m_kPlayer == null || m_kPlayer.Team.Opponent.BallController == null)
                return false;

            //state protection
            if (m_kPlayer.State != EPlayerState.Defend_Break_Through)
                return false;

//            if (m_kPlayer.Team.CheckInDefendBreakThroughArea(m_kPlayer, m_kPlayer.Team.Opponent.BallController))
//            {
//                LogManager.Instance.YellowLog("ConditionalAtDefendBreakThroughArea true");
//                return true;
//            }
//            else
//            {                
//                LogManager.Instance.YellowLog("ConditionalAtDefendBreakThroughArea false");
//                return false;
//            }
			return m_kPlayer.Team.CheckInDefenceEventArea(m_kPlayer,m_kPlayer.Team.Opponent.BallController);
        }
    }
}