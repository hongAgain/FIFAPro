namespace BehaviourTree
{
    public class ActionTeamStateChange : BTAction
    {
        public ActionTeamStateChange()
        {
            Name = "ChangeTeamStae";
            DisplayName = "行为:攻防转换";
            NodeType = "ActionTeamStateChange";
        }

        public override void Activate(BTDatabase kDatabase)
        {
            base.Activate(kDatabase);
            // if (null != m_kDatabase)
            // {
            //     int iID = kDatabase.GetDataID(BTConstant.Team);
            //     m_kTeam = kDatabase.GetData<LLTeam>(iID);
            // }

        }
        protected override BTResult Execute(double fTime)
        {
        //    m_kTeam.CheckBallCtrl();
       //     m_kTeam.Scene.ChangeTeamState(false, ETeamStateChangeType.TSCT_DEBUG);
            return BTResult.Success;
        }

      //  private LLTeam m_kTeam = null;
    }
}