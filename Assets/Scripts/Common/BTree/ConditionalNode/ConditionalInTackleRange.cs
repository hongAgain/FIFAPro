using Common;
using Common.Tables;
using Common.Log;

namespace BehaviourTree
{
    public class ConditionalInTackleRange : BTConditional
    {
        private LLPlayer m_kPlayer = null;
        private double markAreaRadius = 4d;

        public ConditionalInTackleRange()
        {
			Name = "InTackleRange";
			DisplayName = "条件:处于可使用防守动作的距离内";
			NodeType = "ConditionalInTackleRange";
        }
        
        public override void Activate(BTDatabase kDatabase)
        {
            base.Activate(kDatabase);
            if (m_kPlayer == null)
            {
                int iID = m_kDatabase.GetDataID(BTConstant.Player);
                m_kPlayer = m_kDatabase.GetData<LLPlayer>(iID);
            }
            markAreaRadius = TableManager.Instance.AIConfig.GetItem("def_radius_dribble").Value;
        }

        //is inside the marking region and will activate the mark behavior
        public override bool Check()
        {
            LLPlayer targetController = m_kPlayer.Team.Opponent.BallController;
            if (m_kPlayer == null || targetController == null)
                return false;
            if (m_kPlayer.GetPosition().Distance(targetController.GetPosition()) <= markAreaRadius)
            {
                return true;
            }
            else
            {
                if(m_kPlayer.State == EPlayerState.Block_Tackle || m_kPlayer.State == EPlayerState.Sliding_Tackle)
                {
//                    LogManager.Instance.YellowLog("Tackle Out of Range, changed CloseMark_WithBall_NotActivated");
                    m_kPlayer.SetState (EPlayerState.CloseMark_WithBall_NotActivated);
                }
//                LogManager.Instance.YellowLog("Tackle Out of Range");
                return false;
            }
//			return (m_kPlayer.Position.Distance (targetController.Position) <= markAreaRadius);
        }
    }
}