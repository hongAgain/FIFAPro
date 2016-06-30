using Common;
namespace BehaviourTree
{
    public class ActionToDefendBreakThrough : ActionBasicToTackle
    {
        public ActionToDefendBreakThrough()
        {
            Name = "ToDefendBreakThrough";
            DisplayName = "行为:发动阻止突破";
            NodeType = "ActionToDefendBreakThrough";
        }

        #region ====== override functions ======
        protected override void InitializeParams()
        {
            base.InitializeParams();
            
            runSpeedRate = 1d;
            enteringState = EPlayerState.Defend_Break_Through;
            minDistToDefend = 2d;
            maxTimeToTrack = 3d;
        }
        
        /// <summary>
        /// called only once when you arrive
        /// </summary>
        protected override void OnArrived_Execute()
        {
            //check whether defend break through succeded
            m_kPlayer.Team.CheckDefendBreakThrough(m_kPlayer.Opponent,m_kPlayer);
        }
        #endregion
    }
}