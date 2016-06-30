using BehaviourTree;
using Common.Log;
using Common.Tables;
using System;
using System.Collections.Generic;

/* 
    Numerical Settler Shoot
    突破数值对抗
*/
public class NSBreak
{
    public void Caculate(LLUnit kSponsor,LLUnit kDefUnit)
    {
        m_kSponsor = kSponsor;
        m_kDefender = kDefUnit;
        m_bValid = false;
        m_kEvtData = new NSEventData();
        m_kEvtData.EvtID = EEventType.ET_Snatch;
        m_kEvtData.Valid = false;
        BattleStatistics.Instance.NSEvtList.Add(m_kEvtData);

        SettlementFactorItem kItem = TableManager.Instance.SettlementFactorTbl.GetItem("break");
        EnergyItem kEnergyItem = TableManager.Instance.EnergyTbl.GetItem(kSponsor.PlayerBaseInfo.Energy);
        if (null == kEnergyItem)
        {
            LogManager.Instance.Log("Energy table:eneryg is invalid");
            return;
        }
        EnergyItem kDefEnergyItem = TableManager.Instance.EnergyTbl.GetItem(kDefUnit.PlayerBaseInfo.Energy);
        if (null == kDefEnergyItem)
        {
            LogManager.Instance.Log("Energy table:eneryg is invalid");
            return;
        }
        double dSensCoeff = TableManager.Instance.SensitivityFactorTbl.GetItem(kSponsor.PlayerBaseInfo.Attri.lv).Break; //敏感系数
        double dEnergyAttri = kEnergyItem.Value;                            //持球球员体力系数
        double dBreakAttri = kSponsor.PlayerBaseInfo.Attri.breakThrough;    //持球球员的突破属性
        double dBreakCoeff = kItem.SponsorParam1;                           //持球球员突破系数
        double dDefMarkAttri = kDefUnit.PlayerBaseInfo.Attri.mark;          //防守球员盯防属性
        double dDefEnergyAttri = kDefEnergyItem.Value;                      //防守球员体力系数
        double dDefCtrlCoeff = kItem.ReceiverParam1;                        //防守球员控球系数
        double dDribbleAttri = kSponsor.PlayerBaseInfo.Attri.dribble;       //持球球员的盘带属性
        double dDribbleCoeff = kItem.SponsorParam2;                         //持球球员盘带系数
        double dDefStealAttri = kDefUnit.PlayerBaseInfo.Attri.steal;        //防守球员抢断属性
        double dStealCoeff = kItem.ReceiverParam2;                          //抢断系数
        double dBaseVal = kItem.BasicPr;                                    //基础值

        double dVal = dBreakAttri * dEnergyAttri * dBreakCoeff 
            - dDefMarkAttri * dDefEnergyAttri * dDefCtrlCoeff
            + dDribbleAttri * dEnergyAttri * dDribbleCoeff 
            - dDefStealAttri * dDefEnergyAttri * dStealCoeff;
        dVal /= (kSponsor.PlayerBaseInfo.Attri.lv * dSensCoeff);
        dVal += dBaseVal;
        dVal = Math.Max(dBaseVal * 0.1, dVal);
        m_dSuccessPr = Math.Min(1, dVal);
        m_bValid = true;

        m_dRandVal = FIFARandom.GetRandomValue(0, 1);
        //GenPVEValidData();
        OutputDebugInfo();
    }


    private void GenPVEValidData()
    {
        if (null == m_kSponsor || null == m_kDefender)
            return;

        // 传出
        EEventType kType = EEventType.ET_BreakThrough;
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
        kNSEventDetail.Name = "突破";
        kNSEventDetail.Value = m_dSuccessPr;
        m_kEvtData.RetList.Add(kNSEventDetail);
        BattleStatistics.Instance.AddEvent(m_kSponsor, m_kEvtData.EvtID);
        BattleStatistics.Instance.AddEvent(m_kDefender, m_kEvtData.EvtID);
        BattleStatistics.Instance.AddAttri(m_kSponsor, 6);
        BattleStatistics.Instance.AddAttri(m_kDefender, 7);
        ResetDebugInfo();
        LogManager.Instance.LogWarning("开始事件:突破 ===========================");
        LogManager.Instance.LogWarning("突破概率:{0} ", m_dSuccessPr);
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
        LogManager.Instance.LogWarning("6:突破属性:{0}", m_kSponsor.PlayerBaseInfo.Attri.breakThrough);
        LogManager.Instance.LogWarning("被动方");
        LogManager.Instance.LogWarning("体力:{0}", TableManager.Instance.EnergyTbl.GetItem(m_kDefender.PlayerBaseInfo.Energy).Value);
        LogManager.Instance.LogWarning("7:盯防属性:{0}", m_kDefender.PlayerBaseInfo.Attri.mark);
        LogManager.Instance.LogWarning("结束事件:突破 ===========================");

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
    private double m_dRandVal;
    private bool m_bValid = false;
    private double m_dSuccessPr; // 突破成功概率
    private LLUnit m_kSponsor;
    private LLUnit m_kDefender;
    private NSEventData m_kEvtData;
}