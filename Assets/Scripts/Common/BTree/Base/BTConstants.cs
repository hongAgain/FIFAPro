namespace BehaviourTree
{
    public enum BTResult 
    {
		Success,
		Failed,
		Running,
	}
    public enum BTLogic
    {
        And,
        Or,
    }

    public class BTConstant
    {
        public static readonly BTConstant Instance = new BTConstant();
        private BTConstant()
        {
        }

        public int AttackMaxRegionID
        {
            get { return m_iAttackMaxRegionID; }
            set { m_iAttackMaxRegionID = value; }
        }
        public int AttackMinRegionID
        {
            get { return m_iAttackMinRegionID; }
            set { m_iAttackMinRegionID = value; }
        }
        public int DefendMaxRegionID
        {
            get { return m_iDefendMaxRegionID; }
            set { m_iDefendMaxRegionID = value; }
        }

        public int DefendMinRegionID
        {
            get { return m_iDefendMinRegionID; }
            set { m_iDefendMinRegionID = value; }
        }

        private int m_iAttackMaxRegionID = 15;          // 无球盯防进攻方的最大ID
        private int m_iAttackMinRegionID = 1;           // 无球盯防进攻方的最小ID
        private int m_iDefendMaxRegionID = 40;          // 无球盯防防守方的最大ID
        private int m_iDefendMinRegionID = 26;          // 无球盯防防守方的最小ID
        public static readonly double MinDist = 200;    // 停球位置与人最近的距离
        public static readonly double MaxDist = 300;    // 停球位置与人的最远距离
        public static readonly string Type = "Type";
        public static readonly string Name = "Name";
        public static readonly string DisplayName = "DisplayName";
        public static readonly string DescName = "DescName";
        public static readonly string Argument = "Argument";
        public static readonly string Child = "Child";
        public static readonly string Team = "Team";
        public static readonly string Player = "Player";
        public static readonly string GoalKeeper = "GoalKeeper";
        public static readonly string BallTargetPos = "BallTargetPos";
    }
}