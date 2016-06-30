
using Common.Tables;
namespace Common
{
    // 球场解说消息
    public class BattleTextMessage : Message
    {
        public BattleTextMessage(string strID,LLUnit kUnit)
            :base(MessageType.BattleText)
        {
            m_strID = strID;
            m_kUnit = kUnit;         
        }

        public string CondID
        {
            get { return m_strID; }
        }

        public LLUnit Unit
        {
            get { return m_kUnit; }
        }

        private LLUnit m_kUnit;
        private string m_strID;
    }

    public class BattleGoalCeleBrationMsg:Message
    {
        public BattleGoalCeleBrationMsg(GoalCelebrationData _data,LLUnit _uint)
            : base(MessageType.GoalCelebration)
        {
            m_goalCelData = _data;
            m_kUint = _uint;
        }
        public LLUnit Unit
        {
            get { return m_kUint; }
        }
        public GoalCelebrationData GData
        {
            get { return m_goalCelData; }
        }

        private GoalCelebrationData m_goalCelData;
        private LLUnit m_kUint;
    }

    public class BattleDestoryGoalCeleBrationMsg : Message
    {
        public BattleDestoryGoalCeleBrationMsg()
            :base(MessageType.DestoryCeleBration)
        {
        }
    }
}