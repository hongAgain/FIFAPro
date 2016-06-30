using Common;
namespace BehaviourTree
{
    public class ActionToSlidingTackle : ActionBasicToTackle
    {
        public ActionToSlidingTackle()
        {
            Name = "ToSlidingTackle";
            DisplayName = "行为:发动铲断";
            NodeType = "ActionToSlidingTackle";
        }

        #region ====== override functions ======
        protected override void InitializeParams()
        {
            base.InitializeParams();
            
            runSpeedRate = 1d;
            enteringState = EPlayerState.Sliding_Tackle;
            minDistToDefend = 3d;
            maxTimeToTrack = 3d;
        }
                
        /// <summary>
        /// called only once when you arrive
        /// </summary>
        protected override void OnArrived_Execute()
        {
            //check whether sliding succeded
            m_kPlayer.Team.CheckSlidingTackle(m_kPlayer.Opponent,m_kPlayer);
        }
        #endregion
    }
}