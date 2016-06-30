using BehaviourTree;
using Common.Log;
using Common.Tables;
using System;
using System.Collections.Generic;

public class LLGoal 
{

    public LLGoal(LLTeam kTeam)
    {
        m_kTeam = kTeam;
        BattleInfoItem kItem = TableManager.Instance.BattleInfoTable.GetItem("GoalWidth");
        m_dGoalWidth = kItem.Value/2;
        kItem = TableManager.Instance.BattleInfoTable.GetItem("GoalHeight");
        m_dGoalHeight = kItem.Value;
        kItem = TableManager.Instance.BattleInfoTable.GetItem("GroundLength");
        m_kGoalPos.Z = kItem.Value/2;
        
        PosCfgItem kPosCfgItem = TableManager.Instance.PosCfgTbl.GetItem("GoalTarget");
        
        switch (m_kTeam.TeamColor)
        {
            case Common.ETeamColor.Team_Red:
                m_kPosList = kPosCfgItem.PosList;
                m_kGoalPos.Z = -m_kGoalPos.Z;
                //m_kLeftCorner.X = m_dGoalWidth;
                //m_kRightCorner.X = -m_dGoalWidth;
                m_dMinAngle = 285;
                m_dMaxAngle = 75;
                break;
            case Common.ETeamColor.Team_Blue:

                m_dMinAngle = 105;
                m_dMaxAngle = 255;
                for (int i = 0; i < kPosCfgItem.PosList.Count; i++)
                {
                    m_kPosList.Add(new Vector3D(kPosCfgItem.PosList[i].X, kPosCfgItem.PosList[i].Y, -kPosCfgItem.PosList[i].Z));
                }
                break;
        }

        m_kLeftCorner.X = -m_dGoalWidth;
        m_kRightCorner.X = m_dGoalWidth;
        m_kLeftCorner.Z = m_kGoalPos.Z;
        //m_kLeftCorner.Y = m_dGoalHeight;
        m_kRightCorner.Z = m_kGoalPos.Z;
        m_kShootPos = m_kGoalPos;
    }

    // kInPos 球员的位置
    // dPlayerOriAngle 球员的面向
    // kGoalPos 射门点
	public bool GetShootPoint(Vector3D kInPos, double dPlayerOriAngle, bool bNumericSettlerResult, out Vector3D kGoalPos,out bool bRotAngle, bool upperPosOnly = false)
    {
        kGoalPos = new Vector3D();
        // 计算角度
        double dLeftAngle = MathUtil.GetAngle(kInPos, m_kLeftCorner);
        double dRightAngle = MathUtil.GetAngle(kInPos, m_kRightCorner);

        // 计算球员朝向与门柱之间的夹角
        double dDeltaAngle1 = Math.Abs(dPlayerOriAngle - dLeftAngle);
        double dDeltaAngle2 = Math.Abs(dPlayerOriAngle - dRightAngle);

        // 判断是否需要转身体
		bRotAngle = dDeltaAngle1 >= m_dMinAngle && dDeltaAngle1 <= m_dMinAngle && dDeltaAngle2 >= m_dMinAngle && dDeltaAngle2 <= m_dMinAngle;
		if (bRotAngle) // 转身体
        {
            int iIdx;
            if(upperPosOnly)
            {
                iIdx = (int)FIFARandom.GetRandomValue(0, m_kPosList.Count/2 - 1);
            }
            else
            {
                iIdx = (int)FIFARandom.GetRandomValue(0, m_kPosList.Count - 1);
            }
            // 随机落点
            iIdx = (int)FIFARandom.GetRandomValue(0, m_kPosList.Count - 1);
			if (iIdx > m_kPosList.Count)
			{
				// 这种情况不应该发生，这里应该报错
				return false;
			}
            kGoalPos = m_kPosList[iIdx];
            m_kShootPos = kGoalPos;
			if (bNumericSettlerResult == true)
			{
				double xOffset = 0;
				double yOffset = 0;
				AICfgItem tempAIConfigItem;
				if (iIdx % 3 == 0)
				{
					tempAIConfigItem = TableManager.Instance.AIConfig.GetItem("shoot_deviate_side");
					if (tempAIConfigItem != null)
					{
						xOffset = tempAIConfigItem.Value;
					}
					else
					{
						xOffset = 0.5;
					}
                    LogManager.Instance.Log("xOffset: " + xOffset);
				}
				else if (iIdx % 3 == 2)
				{
					tempAIConfigItem = TableManager.Instance.AIConfig.GetItem("shoot_deviate_side");
					if (tempAIConfigItem != null)
					{
						xOffset = -tempAIConfigItem.Value;
					}
					else
					{
						xOffset = -0.5;
					}
                    LogManager.Instance.Log("xOffset: " + xOffset);
				}
				if (iIdx < 3)
				{
					tempAIConfigItem = TableManager.Instance.AIConfig.GetItem("shoot_deviate_top");
					if (tempAIConfigItem != null)
					{
						yOffset = tempAIConfigItem.Value;
					}
					else
					{
						yOffset = 0.5;
					}
                    LogManager.Instance.Log("yOffset: " + yOffset);
				}
				else if (iIdx == 4)
				{
					tempAIConfigItem = TableManager.Instance.AIConfig.GetItem("shoot_deviate_top");
					if (tempAIConfigItem != null)
					{
						yOffset = tempAIConfigItem.Value;
					}
					else
					{
						yOffset = 2.5;
					}
                    LogManager.Instance.Log("yOffset: " + yOffset);
				}
				kGoalPos.X += xOffset;
				kGoalPos.Y += yOffset;
				Vector3D targetPosFromPlayerNormalized = (kGoalPos - kInPos).normalized;
				kGoalPos += targetPosFromPlayerNormalized * 20;
				m_kShootPos = kGoalPos;
				return false;
			}
			return true;
        }

        // 不用转身体，则计算哪些点是在范围内的
        List<Vector3D> kValidPos = new List<Vector3D>();
        for (int i = 0; i < m_kPosList.Count; i++)
        {
            if((upperPosOnly && i < m_kPosList.Count/2) || !upperPosOnly)
            {
                Vector3D kPos = m_kPosList[i];
                double dAngle = MathUtil.GetAngle(kInPos, kPos);
                if (dAngle < m_dMinAngle || dAngle > m_dMaxAngle)
                {
                    kValidPos.Add(kPos);
                }
            }
        }

        if (kValidPos.Count == 0)
		{
			// 这种情况不应该发生，这里应该报错
            return false;
		}
        int iPosIdx = (int)FIFARandom.GetRandomValue(0, kValidPos.Count - 1);
        if (iPosIdx <= kValidPos.Count)
        {
            kGoalPos = kValidPos[iPosIdx];
            m_kShootPos = kGoalPos;
			if (bNumericSettlerResult == false)
			{
				double xOffset = 0;
				double yOffset = 0;
				AICfgItem tempAIConfigItem;
				if (iPosIdx % 3 == 0)
				{
					tempAIConfigItem = TableManager.Instance.AIConfig.GetItem("shoot_deviate_side");
					if (tempAIConfigItem != null)
					{
						xOffset = tempAIConfigItem.Value;
					}
					else
					{
						xOffset = 0.5;
					}
                    LogManager.Instance.Log("xOffset: " + xOffset);
				}
				else if (iPosIdx % 3 == 2)
				{
					tempAIConfigItem = TableManager.Instance.AIConfig.GetItem("shoot_deviate_side");
					if (tempAIConfigItem != null)
					{
						xOffset = -tempAIConfigItem.Value;
					}
					else
					{
						xOffset = -0.5;
					}
                    LogManager.Instance.Log("xOffset: " + xOffset);
				}
				if (iPosIdx < 3)
				{
					tempAIConfigItem = TableManager.Instance.AIConfig.GetItem("shoot_deviate_top");
					if (tempAIConfigItem != null)
					{
						yOffset = tempAIConfigItem.Value;
					}
					else
					{
						yOffset = 0.5;
					}
                    LogManager.Instance.Log("yOffset: " + yOffset);
				}
				else if (iPosIdx == 4)
				{
					tempAIConfigItem = TableManager.Instance.AIConfig.GetItem("shoot_deviate_top");
					if (tempAIConfigItem != null)
					{
						yOffset = tempAIConfigItem.Value;
					}
					else
					{
						yOffset = 2.5;
					}
                    LogManager.Instance.Log("yOffset: " + yOffset);
				}
				kGoalPos.X += xOffset;
				kGoalPos.Y += yOffset;
				Vector3D targetPosFromPlayerNormalized = (kGoalPos - kInPos).normalized;
				kGoalPos += targetPosFromPlayerNormalized * 20;
				m_kShootPos = kGoalPos;
				return false;
			}
        }
        return true;
    }

    public Vector3D GoalPos
    {
        get { return m_kGoalPos; }
        set { m_kGoalPos = value; }
    }

    public Vector3D ShootPos
    {
        get { return m_kShootPos; }
        set { m_kShootPos = value; }
    }

    public double GoalWidth
    {
        get { return m_dGoalWidth; }
        set { m_dGoalWidth = value; }
    }

    public double GoalHeight
    {
        get { return m_dGoalHeight; }
        set { m_dGoalHeight = value; }
    }

    public Vector3D LeftCorner
    {
        get { return m_kLeftCorner; }
        set { m_kLeftCorner = value; }
    }

    public Vector3D RightCorner
    {
        get { return m_kRightCorner; }
        set { m_kRightCorner = value; }
    }

    private Vector3D m_kShootPos = new Vector3D();
    private Vector3D m_kGoalPos = new Vector3D();
    private double m_dGoalWidth;
    private double m_dGoalHeight;
    private Vector3D m_kLeftCorner = Vector3D.zero;
    private Vector3D m_kRightCorner = Vector3D.zero;
    private LLTeam m_kTeam = null;
    private double m_dMinAngle = 105;
    private double m_dMaxAngle = 255;
    private List<Vector3D> m_kPosList = new List<Vector3D>();
}