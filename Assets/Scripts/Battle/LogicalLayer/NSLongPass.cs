using BehaviourTree;
using Common.Log;
using Common.Tables;
using System;
using System.Collections.Generic;

/* 
    Numerical Settler Shoot
    射门数值对抗
*/
public class NSLongPass
{
    public void Caculate(LLUnit kSponsor,LLUnit kDefender,LLUnit kBallGetter,LLUnit kBallGetterDef)
    {
        
        m_kSponsor = kSponsor;
        m_kDefender = kDefender;
        m_kBallGetter = kBallGetter;
        m_kBallGetterDef = kBallGetterDef;
        m_bValid = false;

        if (EGameState.GS_FIX_PASS == m_kSponsor.Team.Scene.GameState)
        {
            m_dPassSuccessPr = 1;
            m_dHeadRobPr = 1;
            m_bValid = true;
            return;
        }

        m_kEvtData = new NSEventData();
        m_kEvtData.EvtID = EEventType.ET_Snatch;
        m_kEvtData.Valid = false;
        BattleStatistics.Instance.NSEvtList.Add(m_kEvtData);
        if (false == CalcPassHardPr(kSponsor, kDefender, kBallGetter))
            return;
        if (false == CalcHeadRobPr(kBallGetter, kBallGetterDef))
            return;
        m_dPassSuccessPr = m_dPassHardPr * m_dHeadRobPr;
        m_bValid = true;

        //GenPVEValidData();
        OutputDebugInfo();
    }

    // 计算传球难度概率
    private bool CalcPassHardPr(LLUnit kSponsor, LLUnit kDefender, LLUnit kBallGetter)
    {
        if (null == kSponsor || null == kDefender ||null == kBallGetter)
        {
            m_dPassHardPr = 1;
            return true;
        }
            
        SettlementFactorItem kItem = TableManager.Instance.SettlementFactorTbl.GetItem("long_pass");
        double dBaseVal = kItem.BasicPr;
        // 传球球员的长传属性
        double dPassAttri = kSponsor.PlayerBaseInfo.Attri.longPass;
        // 体力系数
        EnergyItem kEnergyItem = TableManager.Instance.EnergyTbl.GetItem(kSponsor.PlayerBaseInfo.Energy);
        if (null == kEnergyItem)
        {
            LogManager.Instance.Log("Energy table:eneryg is invalid");
            return false;
        }
        double dEnergyCoeff = kEnergyItem.Value;// 持球球员体力系数
        // 传球球员长传系数
        double dLongPassCoeff = kItem.SponsorParam1;
        // 盯防传球的球员拦截属性
        double dDefTackleAttri = kDefender.PlayerBaseInfo.Attri.intercept;
        // 盯防传球的球员体力系数
        kEnergyItem = TableManager.Instance.EnergyTbl.GetItem(kDefender.PlayerBaseInfo.Energy);
        if (null == kEnergyItem)
        {
            LogManager.Instance.Log("Energy table:eneryg is invalid");
            return false;
        }
        double dDefEnergy = kEnergyItem.Value;// 盯防球员体力系数
        // 拦截系数
        double dIntercetCoeff = kItem.ReceiverParam1;
        // 敏感系数
        double dSensCoeff = TableManager.Instance.SensitivityFactorTbl.GetItem(kSponsor.PlayerBaseInfo.Attri.lv).LongPass;

        double dValue = dPassAttri * dEnergyCoeff * dLongPassCoeff - dDefTackleAttri * dDefEnergy * dIntercetCoeff;
        dValue /= kSponsor.PlayerBaseInfo.Attri.lv * dSensCoeff;
        dValue += dBaseVal;
        dValue = Math.Max(dBaseVal * 0.1, dValue);
        dValue = Math.Min(1, dValue);
        double dDist = kSponsor.GetPosition().Distance(kBallGetter.GetPosition());  // 传球与接球球员距离
        double dDistDecay = (dDist - kItem.Distance);
        dDistDecay = dDistDecay * TableManager.Instance.DistanceDecayTbl.GetCurrentPercentByDistance(dDistDecay);
        m_dPassHardPr = dValue * dDistDecay;// 接球难度
        return true;
    }
    
    private bool CalcHeadRobPr(LLUnit kSponsor,LLUnit kDefender)
    {
        if(null == kDefender)
        {
            m_dHeadRobPr = 1;
            return false; 
        }
        //接球球员的力量属性
        double dPowerAttri = kSponsor.PlayerBaseInfo.Attri.power;
        //球球员的体力系数
        EnergyItem kEnergyItem = TableManager.Instance.EnergyTbl.GetItem(kSponsor.PlayerBaseInfo.Energy);
        if (null == kEnergyItem)
        {
            LogManager.Instance.Log("Energy table:eneryg is invalid");
            return false;
        }
        double dEnergyAttri = kEnergyItem.Value;// 接球球员体力系数
        //体力系数
        SettlementFactorItem kItem = TableManager.Instance.SettlementFactorTbl.GetItem("head");
        double dEnergyCoeff = kItem.SponsorParam1;
        //盯防接球的球员力量属性
        double dDefPowerAttri = kDefender.PlayerBaseInfo.Attri.power;
        //盯防接球的球员体力系数
        kEnergyItem = TableManager.Instance.EnergyTbl.GetItem(kDefender.PlayerBaseInfo.Energy);
        if (null == kEnergyItem)
        {
            LogManager.Instance.Log("Energy table:eneryg is invalid");
            return false;
        }
        double dDefEneryCoeff= kEnergyItem.Value;// 盯防接球的球员体力系数
        //力量系数
        double dPowerCoeff = kItem.ReceiverParam1;
        //接球球员的控球属性
        double dCtrlAttri = kSponsor.PlayerBaseInfo.Attri.control;
        //控球系数 
        double dCtrlCoeff = kItem.SponsorParam2;
        //盯防接球的球员控球属性 
        double dDefCtrlAttri = kDefender.PlayerBaseInfo.Attri.control;
        //控球系数
        double dDefCtrlCoeff = kItem.ReceiverParam2;
        //敏感系数
        double dSenseCoeff = TableManager.Instance.SensitivityFactorTbl.GetItem(kSponsor.PlayerBaseInfo.Attri.lv).Head;
        //基础值
        double dBaseVal = kItem.BasicPr;

        double dVal = dPowerAttri * dEnergyAttri * dEnergyCoeff
            - dDefPowerAttri * dDefEneryCoeff * dPowerCoeff
            + dCtrlAttri * dEnergyAttri * dCtrlCoeff
            - dDefCtrlAttri * dDefEneryCoeff * dDefCtrlCoeff;
        dVal /= (kSponsor.PlayerBaseInfo.Attri.lv * dSenseCoeff);
        dVal += dBaseVal;
        dVal = Math.Max(dBaseVal * 0.1, dVal);
        m_dHeadRobPr = Math.Min(1, dVal);
        return true;
    }

    private void GenPVEValidData()
    {
        if (null == m_kSponsor || null == m_kDefender)
            return;
        EEventType kType = EEventType.ET_LongPassBall;

        PVEValidData kData = new PVEValidData();
        kData.ActionID = (int)kType;
        kData.TeamColor = 0;
        if (m_kSponsor.Team.TeamColor == Common.ETeamColor.Team_Blue)
            kData.TeamColor = 1;
        kData.SponsorIDList.Add(m_kSponsor.PlayerBaseInfo.PlayerID);
        List<int> kDefList = new List<int>();
        kDefList.Add(m_kDefender.PlayerBaseInfo.PlayerID);
        kData.DefenderIDList.Add(kDefList);
        kData.SponsorTeamScore = m_kSponsor.Team.TeamInfo.Score;
        kData.DefendTeamScore = m_kSponsor.Team.Opponent.TeamInfo.Score;
        kData.RandomValIdxList.Add(FIFARandom.GetCurRandomIdx());
        GlobalBattleInfo.Instance.PVEDataList.Add(kData);
    }
    private void OutputDebugInfo()
    {
        //if (null == m_kSponsor || null == m_kDefender)
        //{
        //    return;
        //}


        ResetDebugInfo();
        LogManager.Instance.LogWarning("开始事件:长传 ===========================");
        LogManager.Instance.LogWarning("传出概率:{0} ", m_dPassSuccessPr);
        LogManager.Instance.LogWarning("发起方");
        if (null == m_kSponsor)
        {
            LogManager.Instance.LogWarning("发起方为空");
            return;
        }
        if (null == m_kDefender)
        {
            LogManager.Instance.LogWarning("盯防球员为空");
            return;
        }
        LogManager.Instance.LogWarning("体力:{0}", TableManager.Instance.EnergyTbl.GetItem(m_kSponsor.PlayerBaseInfo.Energy).Value);
        LogManager.Instance.LogWarning("12:长传属性:{0}", m_kSponsor.PlayerBaseInfo.Attri.longPass);
        LogManager.Instance.LogWarning("被动方");
        LogManager.Instance.LogWarning("体力:{0}", TableManager.Instance.EnergyTbl.GetItem(m_kDefender.PlayerBaseInfo.Energy).Value);
        LogManager.Instance.LogWarning("10:拦截属性:{0}", m_kDefender.PlayerBaseInfo.Attri.intercept);
        
        LogManager.Instance.LogWarning("争顶成功概率:{0} ", m_dHeadRobPr);
        LogManager.Instance.LogWarning("发起方");

        if (null == m_kBallGetter)
        {
            LogManager.Instance.LogWarning("发起方为空");
            return;
        }
        if (null == m_kBallGetterDef)
        {
            LogManager.Instance.LogWarning("盯防球员为空");
            return;
        }
        LogManager.Instance.LogWarning("体力:{0}", TableManager.Instance.EnergyTbl.GetItem(m_kBallGetter.PlayerBaseInfo.Energy).Value);
        LogManager.Instance.LogWarning("2:力量属性:{0}", m_kBallGetter.PlayerBaseInfo.Attri.power);
        LogManager.Instance.LogWarning("13:控球属性:{0}", m_kBallGetter.PlayerBaseInfo.Attri.control);
        LogManager.Instance.LogWarning("被动方");
        LogManager.Instance.LogWarning("体力:{0}", TableManager.Instance.EnergyTbl.GetItem(m_kBallGetterDef.PlayerBaseInfo.Energy).Value);
        LogManager.Instance.LogWarning("2:力量属性:{0}", m_kBallGetterDef.PlayerBaseInfo.Attri.power);
        LogManager.Instance.LogWarning("13:控球属性:{0}", m_kBallGetterDef.PlayerBaseInfo.Attri.control);

        LogManager.Instance.LogWarning("结束事件:长传 ===========================");


        m_kEvtData.Valid = true;
        NSEventDetail kNSEventDetail = new NSEventDetail();
        kNSEventDetail.Name = "长传";
        kNSEventDetail.Value = m_dPassSuccessPr;
        m_kEvtData.RetList.Add(kNSEventDetail);
        kNSEventDetail = new NSEventDetail();
        kNSEventDetail.Name = "争顶";
        kNSEventDetail.Value = m_dHeadRobPr;
        m_kEvtData.RetList.Add(kNSEventDetail);

        BattleStatistics.Instance.AddEvent(m_kSponsor, m_kEvtData.EvtID);
        BattleStatistics.Instance.AddEvent(m_kDefender, m_kEvtData.EvtID);
        BattleStatistics.Instance.AddEvent(m_kBallGetter, m_kEvtData.EvtID);
        BattleStatistics.Instance.AddEvent(m_kBallGetterDef, m_kEvtData.EvtID);
        BattleStatistics.Instance.AddAttri(m_kSponsor, 12);
        BattleStatistics.Instance.AddAttri(m_kDefender, 10);
        BattleStatistics.Instance.AddAttri(m_kBallGetter, 13);
        BattleStatistics.Instance.AddAttri(m_kBallGetterDef, 13);
    }

    private void ResetDebugInfo()
    {
        if (null == m_kSponsor)
            return;
        LLTeam kTeam = m_kSponsor.Team;
        LLTeam kOPTeam = kTeam.Opponent;

        for(int i = 0;i < kTeam.PlayerList.Count;i++)
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
        if(null != m_kDefender)
        {
            m_kDefender.ShowDebugInfo = true;
            m_kDefender.RedColor = false;
        }

        if(null != m_kBallGetter)
        {
            m_kBallGetter.ShowDebugInfo = true;
            m_kBallGetter.RedColor = true;
        }

        if(null != m_kBallGetterDef)
        {
            m_kBallGetterDef.ShowDebugInfo = true;
            m_kBallGetterDef.RedColor = false;
        }
    }
    public bool Valid
    {
        get { return m_bValid; }
    }

    public double PassSuccessPr
    {
        get { return m_dPassSuccessPr; }
    }

    public double HeadRobPr
    {
        get { return m_dHeadRobPr; }
    }
    private bool m_bValid = false;          // 判断是否是有效一次结算
    private double m_dPassHardPr = 0;       // 传球难度概率
    private double m_dHeadRobPr = 0;        // 头球争顶概率
    private double m_dPassSuccessPr = 0;    // 传球成功概率

    #region 
    private double m_dAvgTackle;            // 平均拦截属性
    private LLUnit m_kSponsor;
    private LLUnit m_kDefender;
    private LLUnit m_kBallGetter;
    private LLUnit m_kBallGetterDef;
    private NSEventData m_kEvtData;
    #endregion
}