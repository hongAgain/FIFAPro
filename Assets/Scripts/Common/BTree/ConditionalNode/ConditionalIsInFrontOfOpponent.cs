using Common;

namespace BehaviourTree
{
    public class ConditionalIsInFrontOfOpponent : BTConditional
    {
        public ConditionalIsInFrontOfOpponent()
        {
            Name = "IsInFrontOfOpponent";
            DisplayName = "条件:防守方位于进攻方正面";
            NodeType = "ConditionalIsInFrontOfOpponent";
        }
        
        public override bool Check()
        {
            if (m_kPlayer == null)
            {
                int iID = m_kDatabase.GetDataID(BTConstant.Player);
                m_kPlayer = m_kDatabase.GetData<LLPlayer>(iID);
            }
            if (m_kPlayer == null || m_kPlayer.Team.Opponent.BallController == null)
                return false;
            Vector3D targetDir = MathUtil.GetDir(m_kPlayer.Team.Opponent.BallController.GetPosition(), m_kPlayer.Team.Opponent.BallController.TargetPos);
            Vector3D opponentDir = MathUtil.GetDir(m_kPlayer.Team.Opponent.BallController.GetPosition(), m_kPlayer.GetPosition());

            return Vector3D.Dot(targetDir, opponentDir) > 0;
        }
        private LLPlayer m_kPlayer = null;
    }
}