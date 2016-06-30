using BehaviourTree;
using Common.Log;
using Common.Tables;
using System;
using System.Collections.Generic;

/* 
    Numerical Settler Shoot
    铲球数值对抗
*/
public class NSSnatch
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="kSponsor"> 指防守方</param>
    /// <param name="kDefUnit"> 指原进攻方</param>
    public void Caculate(LLUnit kSponsor, LLUnit kDefUnit)
    {
        m_kSponsor = kSponsor;
        m_kDefender = kDefUnit;
        m_bValid = false;

        m_kEvtData = new NSEventData();
        m_kEvtData.EvtID = EEventType.ET_Snatch;
        m_kEvtData.Valid = false;
        BattleStatistics.Instance.NSEvtList.Add(m_kEvtData);
        if (null == kSponsor || null == kDefUnit)
            return;
        //（*-*盘带系数
        SettlementFactorItem kItem = TableManager.Instance.SettlementFactorTbl.GetItem("break");
        EnergyItem kEnergyItem = TableManager.Instance.EnergyTbl.GetItem(kDefUnit.PlayerBaseInfo.Energy);
        if (null == kEnergyItem)
        {
            LogManager.Instance.Log("Energy table:eneryg is invalid");
            return;
        }
        EnergyItem kDefEnergyItem = TableManager.Instance.EnergyTbl.GetItem(kSponsor.PlayerBaseInfo.Energy);
        if (null == kDefEnergyItem)
        {
            LogManager.Instance.Log("Energy table:eneryg is invalid");
            return;
        }
        double dSensCoeff = TableManager.Instance.SensitivityFactorTbl.GetItem(kSponsor.PlayerBaseInfo.Attri.lv).SlideTackle; //敏感系数
        double dEnergyAttri = kEnergyItem.Value;                            //持球球员体力系数
        double dStealAttri = kDefUnit.PlayerBaseInfo.Attri.steal;           //持球球员的抢断属性
        double dStealCoeff = kItem.ReceiverParam1;                          //持球球员铲球系数
        double dDefDribbleAttri = kDefUnit.PlayerBaseInfo.Attri.dribble;    //防守球员盘带属性
        double dDefEnergyAttri = kDefEnergyItem.Value;                      //防守球员体力系数
        double dDefDribbleCoeff = kItem.ReceiverParam1;                     //防守球员控球系数
        double dBaseVal = kItem.BasicPr;                                    //基础值

        double dVal = dStealAttri * dEnergyAttri * dStealCoeff
            - dDefDribbleAttri * dDefEnergyAttri * dDefDribbleCoeff;
        dVal /= (kSponsor.PlayerBaseInfo.Attri.lv * dSensCoeff);
        dVal += dBaseVal;
        dVal = Math.Max(dBaseVal * 0.1, dVal);
        m_dSuccessPr = Math.Min(1, dVal);
        m_bValid = true;
    }


    private void GenPVEValidData()
    {
        if (null == m_kSponsor || null == m_kDefender)
            return;
        // 传出
        EEventType kType = EEventType.ET_Snatch;
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
        m_dRandVal = FIFARandom.GetRandomValue(0, 1);
        kData.RandomValIdxList.Add(FIFARandom.GetCurRandomIdx());
        GlobalBattleInfo.Instance.PVEDataList.Add(kData);
    }
    private void OutputDebugInfo()
    {
        m_kEvtData.Valid = true;
        NSEventDetail kNSEventDetail = new NSEventDetail();
        kNSEventDetail.Name = "铲球";
        kNSEventDetail.Value = m_dSuccessPr;
        m_kEvtData.RetList.Add(kNSEventDetail);
        BattleStatistics.Instance.AddEvent(m_kSponsor, m_kEvtData.EvtID);
        BattleStatistics.Instance.AddEvent(m_kDefender, m_kEvtData.EvtID);
        BattleStatistics.Instance.AddAttri(m_kSponsor, 8);
        BattleStatistics.Instance.AddAttri(m_kDefender, 11);
        ResetDebugInfo();
        LogManager.Instance.RedLog("开始事件:铲球 ===========================");
        LogManager.Instance.RedLog("铲球概率:{0} ", m_dSuccessPr);
        LogManager.Instance.GreenLog("发起方");
        EnergyItem kEnergyItem = TableManager.Instance.EnergyTbl.GetItem(m_kDefender.PlayerBaseInfo.Energy);
        if (null == kEnergyItem)
        {
            LogManager.Instance.Log("Energy table:eneryg is invalid");
            return;
        }
        EnergyItem kDefEnergyItem = TableManager.Instance.EnergyTbl.GetItem(m_kSponsor.PlayerBaseInfo.Energy);
        if (null == kDefEnergyItem)
        {
            LogManager.Instance.Log("Energy table:eneryg is invalid");
            return;
        }
        LogManager.Instance.GreenLog("体力:{0}", TableManager.Instance.EnergyTbl.GetItem(m_kSponsor.PlayerBaseInfo.Energy).Value);
        LogManager.Instance.GreenLog("8:抢断属性:{0}", m_kSponsor.PlayerBaseInfo.Attri.steal);
        LogManager.Instance.GreenLog("被动方");
        LogManager.Instance.GreenLog("体力:{0}", TableManager.Instance.EnergyTbl.GetItem(m_kDefender.PlayerBaseInfo.Energy).Value);
        LogManager.Instance.GreenLog("11:盘带属性:{0}", m_kDefender.PlayerBaseInfo.Attri.dribble);
        LogManager.Instance.RedLog("结束事件:铲球 ===========================");
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

        if (null != m_kDefender)
        {
            m_kDefender.ShowDebugInfo = true;
            m_kDefender.RedColor = false;
        }
    }
    public double SucessPr
    {
        get { return m_dSuccessPr; }
    }

    public bool Valid
    {
        get { return m_bValid; }
    }
    public double RandVal
    {
        get { return m_dRandVal; }
    }
    private bool m_bValid = false;
    private double m_dRandVal;
    private double m_dSuccessPr; // 抢断成功概率
    private LLUnit m_kSponsor;
    private LLUnit m_kDefender;
    private NSEventData m_kEvtData;
}