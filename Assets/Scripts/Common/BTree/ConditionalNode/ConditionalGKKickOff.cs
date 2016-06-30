using Common;
namespace BehaviourTree
{
    

    public class ConditionalGKKickOff : BTConditional
    {
        public ConditionalGKKickOff()
        {
            Name = "GKKickOff";
            DisplayName = "条件:开球";
            NodeType = "ConditionalGKKickOff";
        }

        public override bool Check()
        {
            int iID = m_kDatabase.GetDataID(BTConstant.Player);
            var kPlayer = m_kDatabase.GetData<LLGoalKeeper>(iID);
            return kPlayer.GKState == EGKState.GS_KICKOFF;
        }
    }
}