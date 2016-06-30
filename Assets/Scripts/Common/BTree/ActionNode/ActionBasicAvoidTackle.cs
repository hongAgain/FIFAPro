using Common;
namespace BehaviourTree
{
    public class ActionBasicAvoidTackle : BTAction
    {
        private enum EState
        {
            None,
            AvoidTackle
        }
        
        protected LLPlayer m_kPlayer = null;
        private EState eState = EState.None;
        
        //derived classes should initialize these :
        protected EPlayerState EnteringState = EPlayerState.PS_END;
        protected EPlayerState LeavingState = EPlayerState.PS_END;
        protected EAniState EAS_AvoidTackle = EAniState.EAS_NULL;

        protected ETeamStateChangeDelayedType ETSCDT_DelayType = ETeamStateChangeDelayedType.NONE;
        protected bool needClearDelayType = true;

        protected Vector3D defendingDir = new Vector3D();
        
        
        public ActionBasicAvoidTackle()
        {
            Name = "BasicAvoidTackle";
            DisplayName = "行为:躲避防守行为基类";
            NodeType = "ActionBasicAvoidTackle";
        }
        
        protected override void Enter()
        {
            if (null == m_kPlayer)
            {
                int iID = m_kDatabase.GetDataID(BTConstant.Player);
                m_kPlayer = m_kDatabase.GetData<LLPlayer>(iID);
            }
            if (m_kPlayer.State == EnteringState)
            {
                eState = EState.AvoidTackle;
                m_kPlayer.SetAniState(EAS_AvoidTackle);
                m_kPlayer.StateChangeDelayedType = ETSCDT_DelayType;
                m_kPlayer.SetRoteAngle(MathUtil.GetAngle(m_kPlayer.GetPosition(), m_kPlayer.Opponent.GetPosition()));
                defendingDir = MathUtil.GetDir(m_kPlayer.GetPosition(), m_kPlayer.Opponent.GetPosition());
            }
            else
            {
                eState = EState.None;
            }
        }
        
        protected override BTResult Execute(double fTime)
        {
            if (m_kPlayer.State != EnteringState)
            {
                return BTResult.Failed;
            }
            switch(eState)
            {
                case EState.AvoidTackle:
                    OnAvoiding(fTime);
                    break;
                default:
                    return BTResult.Failed;
            }
            return BTResult.Running;
        }
        
        private void OnAvoiding(double fTime)
        {
            if(m_kPlayer.AniFinish)
            {
                OnAvoidingOver();
                m_kPlayer.SetState(LeavingState);
                if(needClearDelayType)
                    m_kPlayer.ResetDelayedState();
                return;
            }
        }

        protected virtual void OnAvoidingOver()
        {

        }
                
        protected override void Exit()
        {
            m_kPlayer = null;
            eState = EState.None;
        }
    }
}