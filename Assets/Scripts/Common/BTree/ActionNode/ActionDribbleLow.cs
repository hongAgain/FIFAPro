using Common;
using Common.Tables;

namespace BehaviourTree
{
    public class ActionDribbleLow : ActionBasicDribble
    {     
        public ActionDribbleLow()
        {
            Name = "DribbleLow";
            DisplayName = "行为:低速带球";
            NodeType = "ActionDribbleLow";
        }
        
        public override void Activate(BTDatabase kDatabase)
        {
            base.Activate(kDatabase);
            EnteringState = EPlayerState.LowDribble;
            EAS_DribbleAnime = EAniState.LowDribble;
            dribbleVelocityRate = TableManager.Instance.AIConfig.GetItem("speed_rate_walk_dribble").Value;
        }
        
        protected override void PlayAnime()
        {
            m_kPlayer.KAniData.targetPos = m_kPlayer.TargetPos;

            m_kPlayer.SetAniState(EAniState.NormalDribbl);

            m_kPlayer.SetAniState(EAS_DribbleAnime);
        }
    }
}