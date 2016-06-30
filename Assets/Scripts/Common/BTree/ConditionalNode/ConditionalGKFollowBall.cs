
using Common;

namespace BehaviourTree
{

    public class ConditionalGKFollowBall : BTConditional
    {
        public ConditionalGKFollowBall()
        {
            Name = "GKFollowBall";
            DisplayName = "条件:跟随球移动";
            NodeType = "ConditionalGKFollowBall";
        }

        public override bool Check()
        {
            int iID = m_kDatabase.GetDataID(BTConstant.Player);
            var kPlayer = m_kDatabase.GetData<LLGoalKeeper>(iID);
            return kPlayer.GKState == EGKState.GS_DEFAULT;
        }
    }
}