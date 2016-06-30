using Common;
using Common.Tables;

namespace BehaviourTree
{
    public class ActionDribbleNormal : ActionBasicDribble
    {  
        public ActionDribbleNormal()
        {
            Name = "DribbleNormal";
            DisplayName = "行为:常速带球";
            NodeType = "ActionDribbleNormal";
        }

        public override void Activate(BTDatabase kDatabase)
        {
            base.Activate(kDatabase);
            EnteringState = EPlayerState.NormalDribble;
            EAS_DribbleAnime = EAniState.NormalDribbl;
            dribbleVelocityRate = TableManager.Instance.AIConfig.GetItem("speed_rate_run_dribble").Value;
        }
        
        protected override void PlayAnime()
        {
            m_kPlayer.KAniData.targetPos = m_kPlayer.TargetPos;
			m_kPlayer.SetAniState(EAS_DribbleAnime);

			//temp way to handle this problem
			m_kPlayer.Team.InformStartDribble(m_kPlayer);
        }
    }
}