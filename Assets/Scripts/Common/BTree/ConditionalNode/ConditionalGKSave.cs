
using Common;

namespace BehaviourTree
{

    public class ConditionalGKSave : BTConditional
    {
        public ConditionalGKSave()
        {
            Name = "GKSave";
            DisplayName = "条件:门将扑救";
            NodeType = "ConditionalGKSave";
        }

        public override bool Check()
        {
            int iID = m_kDatabase.GetDataID(BTConstant.Player);
            var kPlayer = m_kDatabase.GetData<LLGoalKeeper>(iID);
            return kPlayer.GKState == EGKState.GS_SAVE;
        }
    }
}