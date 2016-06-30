using Common;
using Common.Log;
namespace BehaviourTree
{
    //judge whether this player is marking with ball
    public class ConditionalIsMarkWithBall : BTConditional
    {        
        private LLPlayer m_kPlayer = null;

        public ConditionalIsMarkWithBall()
        {
            Name = "IsMarkWithBall";
            DisplayName = "条件:有球盯防";
            NodeType = "ConditionalIsMarkWithBall";
        }

        public override bool Check()
        {
			if (m_kPlayer == null)
			{
				int iID = m_kDatabase.GetDataID(BTConstant.Player);
				m_kPlayer = m_kDatabase.GetData<LLPlayer>(iID);
			}
            if (m_kPlayer == null)
                return false;

            if (m_kPlayer.MarkingStatus == EMarkStatus.MARKWITHBALL)
            {
                if (m_kPreHeroID != m_kPlayer.PlayerBaseInfo.HeroID || m_kPreState != m_kPlayer.State)
                {
                    LogManager.Instance.YellowLog("盯防球员的行为：====>PlayerId==" + m_kPlayer.PlayerBaseInfo.HeroID
                        + "  队伍==" + m_kPlayer.Team.TeamColor
                        + "  状态==" + m_kPlayer.State);
                    m_kPreState = m_kPlayer.State;
                    m_kPreHeroID = m_kPlayer.PlayerBaseInfo.HeroID;
                    
                }
            }
            return m_kPlayer.MarkingStatus == EMarkStatus.MARKWITHBALL;
        }
        private EPlayerState m_kPreState = EPlayerState.PS_END;
        private uint m_kPreHeroID = 0;
    }
}