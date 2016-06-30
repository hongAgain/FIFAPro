using Common;
namespace BehaviourTree
{
    public class ActionToBlockTackle : ActionBasicToTackle
    {        
        public ActionToBlockTackle()
        {
            Name = "ToBlockTackle";
            DisplayName = "行为:发动抢截";
            NodeType = "ActionToBlockTackle";
        }        
        
        #region ====== override functions ======
        protected override void InitializeParams()
        {
            base.InitializeParams();
            
            runSpeedRate = 1d;
            enteringState = EPlayerState.Block_Tackle;
            minDistToDefend = 2d;
            maxTimeToTrack = 3d;
        }
        
        /// <summary>
        /// called only once when you arrive
        /// </summary>
        protected override void OnArrived_Execute()
        {
            //check whether block succeded
            m_kPlayer.Team.CheckBlockTackle(m_kPlayer.Opponent,m_kPlayer);
        }
        #endregion
    }
}