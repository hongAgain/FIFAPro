using Common;
using Common.Tables;

namespace BehaviourTree
{
    public class ActionRunToTargetPos : BTAction
    {
        public ActionRunToTargetPos()
        {
            Name = "RunToTargetPos";
            DisplayName = "行为:跑向目标位置";
            NodeType = "ActionRunToTargetPos";
        }

        protected enum InternalState
        {
            NONE,
            RUNNING,
            ARRIVED,
            AFTERARRIVED
        }

        protected InternalState iState = InternalState.NONE;
        protected LLPlayer m_kPlayer = null;
        
        protected double runSpeedRate = 1d;
        protected EAniState runAnime = EAniState.EAS_NULL;
        protected EPlayerState enteringState = EPlayerState.PS_END;
        
        public override void Activate(BTDatabase kDatabase)
        {
            base.Activate(kDatabase);
            InitializeParams();
        }

        protected override void Enter()
        {
            if (null == m_kPlayer)
            {
                int iID = m_kDatabase.GetDataID(BTConstant.Player);
                m_kPlayer = m_kDatabase.GetData<LLPlayer>(iID);
            }
            if (m_kPlayer.State == enteringState)
            {
                InitPlayer_Enter();
                if (NeedRun())
                {
                    iState = InternalState.RUNNING;
                    if(runAnime != EAniState.EAS_NULL)
                    {
                        StartRun_Enter();
                    }
                }
                else
                {     
                    iState = InternalState.ARRIVED;
                }
            }
        }
        
        protected override BTResult Execute(double dTime)
        {
            if( m_kPlayer == null || m_kPlayer.State != enteringState )
                return BTResult.Failed;

            switch(iState)
            {
                case InternalState.RUNNING:
                    UpdateTargetPos_Execute();
                    RunToTargetPos_Execute(dTime);
                    break;
                case InternalState.ARRIVED:
                    OnArrived_Execute();
                    iState = InternalState.AFTERARRIVED;
                    break;
                case InternalState.AFTERARRIVED:
                    OnAfterArrived_Execute(dTime);
                    break;
                default:
                    return BTResult.Failed;
            }
            return BTResult.Running;
        }

        protected override void Exit()
        {
            m_kPlayer = null;
        }

        #region ====== virtual functions ======
        protected virtual void InitializeParams()
        {
            runSpeedRate = TableManager.Instance.AIConfig.GetItem("speed_rate_run").Value;
            runAnime = EAniState.NormalRun;
            enteringState = EPlayerState.PS_END;
        }

        protected virtual bool NeedRun()
        {
            return false;
        }
        
        protected virtual void InitPlayer_Enter()
        {
            m_kPlayer.Velocity = m_kPlayer.BaseVelocity * runSpeedRate;
            //set target position
            //m_kPlayer.TargetPos = m_kPlayer.Opponent.TargetPos;
            //m_kPlayer.RotateAngle = MathUtil.GetAngle(m_kPlayer.Position,m_kPlayer.TargetPos);
        }
        
        protected virtual void StartRun_Enter()
        {
//            m_kPlayer.KAniData.targetPos = m_kPlayer.TargetPos;
            m_kPlayer.SetAniState(runAnime);
        }

        protected virtual void UpdateTargetPos_Execute()
        {
            
        }

        //run to my m_kPlayer.TargetPos, until we say its valid
        protected virtual void RunToTargetPos_Execute(double dTime)
        {
            if (NeedRun())
            {
                //calculate next position
                m_kPlayer.MoveToPos(m_kPlayer.TargetPos,dTime);
            }
            else
            {
                iState = InternalState.ARRIVED;
            }
        }

        /// <summary>
        /// called only once when you arrive
        /// </summary>
        protected virtual void OnArrived_Execute()
        {

        }

        protected virtual void OnAfterArrived_Execute(double dTime)
        {

        }
        #endregion
    }
}