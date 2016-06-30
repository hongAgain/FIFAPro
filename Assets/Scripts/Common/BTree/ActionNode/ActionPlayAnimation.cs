using Common;
namespace BehaviourTree
{
    public class ActionPlayAnimation : BTAction
    {
        public ActionPlayAnimation()
        {
            Name = "PlayAnimation";
            DisplayName = "行为:播放动画";
            NodeType = "ActionPlayAnimation";
        }

        protected override void Enter()
        {
            if (null == m_kPlayer)
            {
                int iID = m_kDatabase.GetDataID(BTConstant.Player);
                m_kPlayer = m_kDatabase.GetData<LLPlayer>(iID);
            }
            if (null != m_kPlayer)
                m_kTeam = m_kPlayer.Team;
        }

        /// <summary>
        /// Called every frame if the action node is active.
        /// </summary>
        protected override BTResult Execute(double fTime)
        {
            BTResult kRet = BTResult.Success;
            if (null == m_kTeam)
            {
                return kRet;
            }
            //m_kTeam.Sel
            // 球队选择可以接球的球员
            LLPlayer kPlayer = m_kTeam.SelectBallController();
            if (null != kPlayer)
            {
                m_kPlayer.SetBallCtrl(false); //传球后,失去控球权
                m_kPlayer.SetState(EPlayerState.HomePos);
                LLScene m_kScene = m_kTeam.Scene;
                LLBall kBall = null;
                if (null != m_kScene)
                    kBall = m_kScene.Ball;
                if (null != kBall)
                    kBall.SetTarget(kPlayer.GetPosition());

            }
            return BTResult.Success;
        }

        /// <summary>
        /// Called when the action node finishes.
        /// </summary>
        protected override void Exit()
        {
            m_kPlayer = null;
        }

        private LLTeam m_kTeam = null;
        private LLPlayer m_kPlayer = null;
    }
}