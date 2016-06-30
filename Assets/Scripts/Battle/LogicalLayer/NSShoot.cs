using BehaviourTree;
using Common.Log;
using Common.Tables;
using System;
using System.Collections.Generic;

/* 
    Numerical Settler Shoot -->NSShoot
    射门数值对抗
*/
public class NSShoot
{
    public void Caculate(LLUnit kSponsor)
    {
        // 插入事件次数
        m_kSponsor = kSponsor;
        m_kShootData = kSponsor.Team.Opponent.ShootData;
        m_kEvtData = new NSEventData();
        if (m_kShootData.IsHeadShoot)
            m_kEvtData.EvtID = EEventType.ET_HeadShoot;
        else if (m_kShootData.FarShoot)
            m_kEvtData.EvtID = EEventType.ET_FarShoot;
        else
            m_kEvtData.EvtID = EEventType.ET_Shoot;
        m_kEvtData.Valid = false;
        BattleStatistics.Instance.NSEvtList.Add(m_kEvtData);
        List<LLUnit> kUnitList;
        SelectShootDefendPlayers(kSponsor, out kUnitList);

        // 区域防守球员平均力量
        double dAvgPower = 0;
        // 区域防守球员平均拦截属性
        double dAvgTackle = 0;
        // 区域防守球员平均体力
        double dAvgEnergy = 0;
        if (kUnitList.Count > 0)
        {
            for (int i = 0; i < kUnitList.Count; i++)
            {
                dAvgPower += kUnitList[i].PlayerBaseInfo.Attri.power;
                dAvgTackle += kUnitList[i].PlayerBaseInfo.Attri.tackle;
                dAvgEnergy += TableManager.Instance.EnergyTbl.GetItem(kUnitList[i].PlayerBaseInfo.Energy).Value;
            }
            dAvgTackle /= kUnitList.Count;
            dAvgEnergy /= kUnitList.Count;
            dAvgPower /= kUnitList.Count;
            m_dAvgTackle = dAvgTackle;
            m_dAvgEnergy = dAvgEnergy;
            m_dAvgPower = dAvgPower;
        }

        double dSensCoeff = TableManager.Instance.SensitivityFactorTbl.GetItem(kSponsor.PlayerBaseInfo.Attri.lv).ShootInsidePr;
        bool bRetVal = ShootOutsidePr(kSponsor, kUnitList.Count, dAvgTackle, dAvgEnergy, dAvgPower, dSensCoeff);
        if (false == bRetVal)
            return;
        if(m_kShootData.IsHeadShoot)
            dSensCoeff = TableManager.Instance.SensitivityFactorTbl.GetItem(kSponsor.PlayerBaseInfo.Attri.lv).HeadShoot;
        else if(m_kShootData.FarShoot)
            dSensCoeff = TableManager.Instance.SensitivityFactorTbl.GetItem(kSponsor.PlayerBaseInfo.Attri.lv).LongShoot;
        else
            dSensCoeff = TableManager.Instance.SensitivityFactorTbl.GetItem(kSponsor.PlayerBaseInfo.Attri.lv).Shoot;

        bRetVal = ShootSucessBaseValue(kSponsor, dAvgEnergy, dSensCoeff);
        if (false == bRetVal)
            return;
        m_bValid = true;
        
        m_dBallInPr = m_dShootInsidePr * m_dShootSuccessPr;
        m_dGKSavePr = m_dShootInsidePr * (1 - m_dShootSuccessPr);
        GenPVEValidData();
        OutputDebugInfo();
    }
    
    /// <summary>
    /// 远射 基础成功率
    /// </summary>
    /// <param name="kSponsor"></param>
    /// dAvgPower 平均体力系数
    /// <returns></returns>
    private bool ShootSucessBaseValue(LLUnit kSponsor, double dAvgPower, double dSensCoeff)
    {
        SettlementFactorItem kItem = null;
        double dShootAttr = 0;
        LLGoalKeeper kGKUnit = kSponsor.Team.Opponent.GoalKeeper;
        // 门将扑救属性
        double dGKSaveAtt = kGKUnit.PlayerBaseInfo.Attri.save;
        double dGKAtt = kGKUnit.PlayerBaseInfo.Attri.power;

        if (m_kShootData.IsHeadShoot)
        {
            kItem = TableManager.Instance.SettlementFactorTbl.GetItem("tackle"); 
            dShootAttr = kSponsor.PlayerBaseInfo.Attri.shoot;//持球球员的射门属性
            dGKAtt = kGKUnit.PlayerBaseInfo.Attri.reaction;// 门将反应属性
        }
        else if (m_kShootData.FarShoot)
        {

            kItem = TableManager.Instance.SettlementFactorTbl.GetItem("long_shot");
            dShootAttr = kSponsor.PlayerBaseInfo.Attri.longShoot;//持球球员的远射属性
            dGKAtt = kGKUnit.PlayerBaseInfo.Attri.power;// 门将力量属性
        }
        else
        {
            kItem = TableManager.Instance.SettlementFactorTbl.GetItem("shot");
            dShootAttr = kSponsor.PlayerBaseInfo.Attri.shoot;//持球球员的射门属性
            dGKAtt = kGKUnit.PlayerBaseInfo.Attri.reaction;// 门将反应属性
        }
            
        double dShootCoeff = kItem.SponsorParam1;//持球球员远射系数
        double dSponsorCoeff = kItem.SponsorParam2;//持球球员力量系数
        double dShootOutSideBasePr = kItem.BasicPr; //基础值
        double dSponsorPower = kSponsor.PlayerBaseInfo.Attri.power; // 持球球员的力量属性
        // 门将扑救系数
        double dGKSaveCoeff = kItem.ReceiverParam1;
        double dGKPowerCoeff = kItem.ReceiverParam2;

        EnergyItem kEnergyItem = TableManager.Instance.EnergyTbl.GetItem(kSponsor.PlayerBaseInfo.Energy);
        if (null == kEnergyItem)
        {
            LogManager.Instance.Log("Energy table:energy is invalid");
            return false;
        }
        double dEnergyCoeff = kEnergyItem.Value;// 持球球员体力系数
        

        double dValue = dShootAttr * dSponsorCoeff - dGKSaveAtt * dAvgPower * dGKSaveCoeff - dGKAtt * dAvgPower * dGKPowerCoeff;
        if (m_kShootData.FarShoot)
            dValue += +dSponsorPower * dEnergyCoeff * dSponsorCoeff;
        dValue /= (kSponsor.Team.TeamInfo.TeamLv * dSensCoeff + dShootOutSideBasePr);
        double dBaseVal = Math.Max(dShootOutSideBasePr * 0.1, dValue);  // 远射基础概率
        // 获得球员与射门点的距离
        ShootData kData = kSponsor.Team.Opponent.ShootData;
        double dDist = kSponsor.GetPosition().Distance(kData.GoalPos);

        double dDistDecay = (dDist - kItem.Distance);// 远射距离衰减
        dDistDecay = dDistDecay * TableManager.Instance.DistanceDecayTbl.GetCurrentPercentByDistance(dDistDecay);

        int iRedScore = 0, iBlueScore = 0;
        LLDirector.Instance.CalcScore(ref iRedScore, ref iBlueScore,false);
        int iPredictScoreDiff = Math.Abs(iRedScore - iBlueScore);
        int iScoreDiff = Math.Abs(kSponsor.Team.TeamInfo.Score - kSponsor.Team.Opponent.TeamInfo.Score);
        ConfrontationBasicItem kCBItem = TableManager.Instance.ConfrontationBasicTbl.GetItem("Point_Cof");
        if (null == kCBItem)
            return false;
        m_dScoreModify = Math.Abs(iScoreDiff - iPredictScoreDiff) * kCBItem.Value;
        m_dShootSuccessPr = dBaseVal * (1 - dDistDecay) + m_dScoreModify;
        return true;
    }



    /// <summary>
    /// 远射 射偏基础概率
    /// </summary>
    /// dAvgTackle 平均拦截属性
    /// iDefCnt 防守球员数
    /// <returns></returns>
    private bool ShootOutsidePr(LLUnit kSponsor,int iDefCnt, double dAvgTackle, double dAvgEnergy,double dAvgPower, double dSensCoeff)
    {
        SettlementFactorItem kItem = null; 
        double dShootAttr = 0;
        if (m_kShootData.IsHeadShoot)
        {
            kItem = TableManager.Instance.SettlementFactorTbl.GetItem("tackle");
            dShootAttr = kSponsor.PlayerBaseInfo.Attri.power;//持球球员的力量属性
        }
        else if (m_kShootData.FarShoot)
        {
            kItem = TableManager.Instance.SettlementFactorTbl.GetItem("long_shot");
            dShootAttr = kSponsor.PlayerBaseInfo.Attri.longShoot;//持球球员的远射属性
        }
        else
        {
            kItem = TableManager.Instance.SettlementFactorTbl.GetItem("shot");
            dShootAttr = kSponsor.PlayerBaseInfo.Attri.shoot;//持球球员的远射属性
        }
        double dShootCoeff = kItem.SponsorParam1;//持球球员远射系数
        double dOutSideBasePr = kItem.BasicPr; //基础值
        EnergyItem kEnergyItem = TableManager.Instance.EnergyTbl.GetItem(kSponsor.PlayerBaseInfo.Energy);
        if (null == kEnergyItem)
        {
            LogManager.Instance.Log("Energy table:eneryg is invalid");
            return false;
        }
        double dEnergyCoeff = kEnergyItem.Value;// 持球球员体力系数
        // 防守方拦截系数
        double dDefendTackleCoeff = kItem.ReceiverParam1;


        double dValue = 0;
        if (m_kShootData.IsHeadShoot)
        {
            dValue = (dShootAttr * dEnergyCoeff * dShootCoeff - dAvgPower * dAvgEnergy * dDefendTackleCoeff) / kSponsor.Team.TeamInfo.TeamLv * dSensCoeff + dOutSideBasePr;
        }
        else
        {
            dValue = (dShootAttr * dEnergyCoeff * dShootCoeff - dAvgTackle * dAvgEnergy * dDefendTackleCoeff) / kSponsor.Team.TeamInfo.TeamLv * dSensCoeff + dOutSideBasePr;
        }
            
        // 远射 射偏概率
        double dBaseVal = Math.Min(1, Math.Max(dOutSideBasePr * 0.1, dValue));
        double dDefendNum = Math.Min(1, 3.0 / (iDefCnt + 2));
        m_dShootInsidePr = dBaseVal * dDefendNum; // 射正概率
        return true;
    }
    /// <summary>
    /// 选择防守射门球员的算法 
    /// 算法的核心思想：
    /// 球员与门柱连线的扇形区域内的所有敌方球员
    /// </summary>
    /// <param name="kSponsor"></param>
    /// <param name="kUnitList"></param>
    /// <returns></returns>
    private bool SelectShootDefendPlayers(LLUnit kSponsor, out List<LLUnit> kUnitList)
    {
        kUnitList = new List<LLUnit>();
        if (null == kSponsor)
            return false;
        // 获取敌方球门
        LLTeam kTeam = kSponsor.Team;
        if (null == kTeam)
            return false;
        LLTeam kDefandTeam = kTeam.Opponent;
        if (null == kDefandTeam)
            return false;

        LLGoal kGoal = kDefandTeam.Goal;
        Vector3D kGoalPos = kGoal.GoalPos;
        Vector3D kLeftPos = kGoal.GoalPos;
        kLeftPos.X -= kGoal.GoalWidth;
        Vector3D kRightPos = kGoal.GoalPos;
        kRightPos.X += kGoal.GoalWidth;

        double dLAngle = MathUtil.GetAngle(kSponsor.GetPosition(), kLeftPos);
        double dRAngle = MathUtil.GetAngle(kSponsor.GetPosition(), kRightPos);

        switch (kTeam.TeamColor)
        {
            case Common.ETeamColor.Team_Blue:
                for (int iIdx = 0; iIdx < kDefandTeam.PlayerList.Count; iIdx++)
                {
                    double dAngle = MathUtil.GetAngle(kSponsor.GetPosition(), kDefandTeam.PlayerList[iIdx].GetPosition());
                    if (dAngle < dRAngle || dAngle > dLAngle)
                        kUnitList.Add(kDefandTeam.PlayerList[iIdx]);
                }
                break;
            case Common.ETeamColor.Team_Red:
                for (int iIdx = 0; iIdx < kDefandTeam.PlayerList.Count; iIdx++)
                {
                    double dAngle = MathUtil.GetAngle(kSponsor.GetPosition(), kDefandTeam.PlayerList[iIdx].GetPosition());
                    if (dAngle < dRAngle || dAngle > dLAngle)
                        continue;
                    else
                        kUnitList.Add(kDefandTeam.PlayerList[iIdx]);
                }
                break;
            default:
                break;
        }

        return true;
    }
    
    private void OutputDebugInfo()
    {
        ResetDebugInfo();

        m_kEvtData.Valid = true;
        if (m_kShootData.IsHeadShoot)
            LogManager.Instance.LogWarning("开始事件:头球射门 ===================================");
        else if (m_kShootData.FarShoot)
            LogManager.Instance.LogWarning("开始事件:远射 ===================================");
        else
            LogManager.Instance.LogWarning("开始事件:近射 ===================================");
        NSEventDetail kNSEventDetail = new NSEventDetail();
        kNSEventDetail.Name = "射正";
        kNSEventDetail.Value = m_dShootInsidePr;
        m_kEvtData.RetList.Add(kNSEventDetail);
        kNSEventDetail = new NSEventDetail();
        kNSEventDetail.Name = "进球概率";
        kNSEventDetail.Value = m_dShootSuccessPr;
        m_kEvtData.RetList.Add(kNSEventDetail);

        BattleStatistics.Instance.AddEvent(m_kSponsor, m_kEvtData.EvtID);
        BattleStatistics.Instance.AddEvent(m_kSponsor.Team.Opponent.GoalKeeper, m_kEvtData.EvtID);
        for(int i = 0;i < m_kDefList.Count;i++)
            BattleStatistics.Instance.AddEvent(m_kDefList[i], m_kEvtData.EvtID);
        BattleStatistics.Instance.AddEvent(m_kSponsor.Team.Opponent.GoalKeeper, m_kEvtData.EvtID);
        BattleStatistics.Instance.AddAttri(m_kSponsor.Team.Opponent.GoalKeeper, 15);
        BattleStatistics.Instance.AddAttri(m_kSponsor.Team.Opponent.GoalKeeper, 2);
        BattleStatistics.Instance.AddAttri(m_kSponsor.Team.Opponent.GoalKeeper, 18);

        LogManager.Instance.LogWarning("修正得分:{0}", m_dScoreModify);
        LogManager.Instance.LogWarning("射正概率:{0}", m_dShootInsidePr);
        LogManager.Instance.LogWarning("发起方");
        LogManager.Instance.LogWarning("体力:{0}", TableManager.Instance.EnergyTbl.GetItem(m_kSponsor.PlayerBaseInfo.Energy).Value);
        double dShootAttr = 0;
        if (m_kShootData.IsHeadShoot)
        {
            dShootAttr = m_kSponsor.PlayerBaseInfo.Attri.power;
            LogManager.Instance.LogWarning("2:持球球员的力量属性:{0}",dShootAttr);
            BattleStatistics.Instance.AddAttri(m_kSponsor, 2);
        }
        else if (m_kShootData.FarShoot)
        {
            dShootAttr = m_kSponsor.PlayerBaseInfo.Attri.longShoot;
            LogManager.Instance.LogWarning("5:持球球员的远射属性:{0}", dShootAttr);
            BattleStatistics.Instance.AddAttri(m_kSponsor, 5);
        }
        else
        {
            dShootAttr = m_kSponsor.PlayerBaseInfo.Attri.shoot;
            LogManager.Instance.LogWarning("3:持球球员的近射属性:{0}", dShootAttr);
            BattleStatistics.Instance.AddAttri(m_kSponsor, 3);
        }
        LogManager.Instance.LogWarning("被动方");
        LogManager.Instance.LogWarning("9:平均拦截属性:{0}", m_dAvgTackle);
        for (int i = 0; i < m_kDefList.Count; i++)
            BattleStatistics.Instance.AddAttri(m_kDefList[i], 10);
        LogManager.Instance.LogWarning("-1:平均体力:{0}", m_dAvgEnergy);
        
        LogManager.Instance.LogWarning("射正概率:{0}", m_dShootSuccessPr);
        LogManager.Instance.LogWarning("发起方");
        LogManager.Instance.LogWarning("体力:{0}", TableManager.Instance.EnergyTbl.GetItem(m_kSponsor.PlayerBaseInfo.Energy).Value);
        if (m_kShootData.IsHeadShoot)
        {
            dShootAttr = m_kSponsor.PlayerBaseInfo.Attri.shoot;
            LogManager.Instance.LogWarning("3:持球球员的射门属性:{0}", dShootAttr);
        }
        else if (m_kShootData.FarShoot)
        {
            dShootAttr = m_kSponsor.PlayerBaseInfo.Attri.longShoot;
            LogManager.Instance.LogWarning("5:持球球员的远射属性:{0}", dShootAttr);
            double dSponsorPower = m_kSponsor.PlayerBaseInfo.Attri.power; // 
            LogManager.Instance.LogWarning("2:持球球员的力量属性:{0}", dSponsorPower);
        }
        else
        {
            dShootAttr = m_kSponsor.PlayerBaseInfo.Attri.shoot;//
            LogManager.Instance.LogWarning("3:持球球员的射门属性:{0}", dShootAttr);
        }
      //  LogManager.Instance.GreenLog("2::{0}", dSponsorPower);

        LogManager.Instance.LogWarning("被动方");
        LLGoalKeeper kGKUnit = m_kSponsor.Team.Opponent.GoalKeeper;
        if (m_kShootData.IsHeadShoot || false == m_kShootData.FarShoot)
        {
            double dGKAtt = kGKUnit.PlayerBaseInfo.Attri.reaction;// 
            LogManager.Instance.LogWarning("15:门将反应属性:{0}", dGKAtt);
        }
        else if(m_kShootData.FarShoot)
        {
            double dGKAtt = kGKUnit.PlayerBaseInfo.Attri.power;// 
            LogManager.Instance.LogWarning("2:门将力量属性:{0}", dGKAtt);
        }
        double dGKSaveAtt = kGKUnit.PlayerBaseInfo.Attri.save;// 
        LogManager.Instance.LogWarning("18:门将扑救属性:{0}", dGKSaveAtt);
        LogManager.Instance.LogWarning("-1:平均体力:{0}", m_dAvgEnergy);

        if (m_kShootData.IsHeadShoot)
            LogManager.Instance.LogWarning("结束事件:头球射门 ========================");
        else if (m_kShootData.FarShoot)
            LogManager.Instance.LogWarning("结束事件:远射 ========================");
        else
            LogManager.Instance.LogWarning("结束事件:近射 ========================");
        
    }

    private void GenPVEValidData()
    {
        EEventType kType = EEventType.ET_Shoot;
        if (m_kShootData.IsHeadShoot)
            kType = EEventType.ET_HeadShoot;
        else if (m_kShootData.FarShoot)
            kType = EEventType.ET_FarShoot;
        else
            kType = EEventType.ET_Shoot;

        PVEValidData kData = new PVEValidData();
        kData.ActionID = (int)kType;
        kData.TeamColor = 0;
        if (m_kSponsor.Team.TeamColor == Common.ETeamColor.Team_Blue)
            kData.TeamColor = 1;
        kData.SponsorIDList.Add(m_kSponsor.PlayerBaseInfo.PlayerID);
        kData.SponsorIDList.Add(m_kSponsor.PlayerBaseInfo.PlayerID);
        double dVal = TableManager.Instance.EnergyTbl.GetItem(m_kSponsor.PlayerBaseInfo.Energy).Value;
        kData.SEnergyList.Add(dVal);
        kData.SEnergyList.Add(dVal);
        List<int> kDefList = new List<int>();
        for (int i = 0; i < m_kDefList.Count; i++)
            kDefList.Add(m_kDefList[i].PlayerBaseInfo.PlayerID);
        kData.DefenderIDList.Add(kDefList);
        kDefList = new List<int>();
        LLGoalKeeper kGoalKeeper = m_kSponsor.Team.Opponent.GoalKeeper;
        kDefList.Add(kGoalKeeper.PlayerBaseInfo.PlayerID);
        kData.DefenderIDList.Add(kDefList);
        dVal = TableManager.Instance.EnergyTbl.GetItem(kGoalKeeper.PlayerBaseInfo.Energy).Value;
        kData.DEnergyList.Add(m_dAvgEnergy);
        kData.DEnergyList.Add(dVal);
        kData.SponsorTeamScore = m_kSponsor.Team.TeamInfo.Score;
        kData.DefendTeamScore = m_kSponsor.Team.Opponent.TeamInfo.Score;
        m_dShootRandVal = FIFARandom.GetRandomValue(0, 1);
        kData.RandomValIdxList.Add(FIFARandom.GetCurRandomIdx());
        m_dGKSaveRandVal = FIFARandom.GetRandomValue(0, 1);
        kData.RandomValIdxList.Add(FIFARandom.GetCurRandomIdx());
        m_kShootData.NSValid = true;
        m_kShootData.GKSaveRandVal = m_dGKSaveRandVal;
        GlobalBattleInfo.Instance.PVEDataList.Add(kData);
    }
    private void ResetDebugInfo()
    {
        if (null == m_kSponsor)
            return;
        LLTeam kTeam = m_kSponsor.Team;
        LLTeam kOPTeam = kTeam.Opponent;

        for (int i = 0; i < kTeam.PlayerList.Count; i++)
        {
            kTeam.PlayerList[i].ShowDebugInfo = false;
        }
        kTeam.GoalKeeper.ShowDebugInfo = false;
        for (int i = 0; i < kOPTeam.PlayerList.Count; i++)
        {
            kOPTeam.PlayerList[i].ShowDebugInfo = false;
        }
        kTeam.GoalKeeper.ShowDebugInfo = false;

        m_kSponsor.ShowDebugInfo = true;
        m_kSponsor.RedColor = true;
        for(int i = 0;i < m_kDefList.Count;i++)
        {
            m_kDefList[i].ShowDebugInfo = true;
            m_kDefList[i].RedColor = false;
        }
    }
    public double ShootInsidePr    // 射偏概率
    {
        get { return m_dShootInsidePr; }
    }

    public double ShootSuccessPr        // 进球概率
    {
        get { return m_dShootSuccessPr; }
    }

    public double ShootRandVal
    {
        get { return m_dShootRandVal; }
    }

    public double GKSaveRandVal
    {
        get { return m_dGKSaveRandVal; }
    }
    public bool Valid
    {
        get { return m_bValid; }
    }
    private bool m_bValid = false;      // 判断是否是有效一次结算
    private ShootData m_kShootData;
    private double m_dShootInsidePr;   // 射正概率
    private double m_dShootSuccessPr;   // 射门成功概率
    private double m_dBallInPr;         // 进球概率
    private double m_dGKSavePr;         // 扑住概率
    private double m_dAvgTackle;        // 平均拦截属性
    private double m_dAvgEnergy;        // 平均体力
    private double m_dAvgPower;         // 平均力量属性
    private double m_dScoreModify = 0;  // 修正得分
    private double m_dShootRandVal = 0; // 射门随机值
    private double m_dGKSaveRandVal = 0;// 门将扑球随机值
    #region ValidData

    private LLUnit m_kSponsor = null;                       // 发起者
    private List<LLUnit> m_kDefList = new List<LLUnit>();   // 数值对抗接收者
    #endregion
    #region Debug Info
    private NSEventData m_kEvtData;
    private string m_strEvtName;
    #endregion
}