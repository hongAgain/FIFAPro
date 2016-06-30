using BehaviourTree;
using Common.Log;
using Common.Tables;
using System;
using System.Collections.Generic;

/* 
    Numerical Settler Tackle
    抢断数值对抗
*/
public class NSTackle
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="kSponsor"> 数值对抗发起者 </param>
    /// <param name="kDefUnit"> 数值对抗接受者 </param>
    public void Caculate(LLUnit kSponsor,LLUnit kDefUnit)
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
        SettlementFactorItem kItem = TableManager.Instance.SettlementFactorTbl.GetItem("tackle");
        EnergyItem kEnergyItem = TableManager.Instance.EnergyTbl.GetItem(kSponsor.PlayerBaseInfo.Energy);
        if (null == kEnergyItem)
        {
            LogManager.Instance.Log("Energy table:eneryg is invalid");
            return ;
        }
        EnergyItem kDefEnergyItem = TableManager.Instance.EnergyTbl.GetItem(kDefUnit.PlayerBaseInfo.Energy);
        if (null == kDefEnergyItem)
        {
            LogManager.Instance.Log("Energy table:eneryg is invalid");
            return ;
        }
        double dSensCoeff = TableManager.Instance.SensitivityFactorTbl.GetItem(kSponsor.PlayerBaseInfo.Attri.lv).Tackle; //敏感系数
        double dEnergyAttri = kEnergyItem.Value;                            //持球球员体力系数
        double dTackleAttri = kSponsor.PlayerBaseInfo.Attri.steal;          //持球球员的抢断属性
        double dTackleCoeff = kItem.SponsorParam1;                          //持球球员抢断系数
        double dDefCtrlAttri = kDefUnit.PlayerBaseInfo.Attri.control;       //防守球员控球属性
        double dDefEnergyAttri = kDefEnergyItem.Value;                      //防守球员体力系数
        double dDefCtrlCoeff = kItem.ReceiverParam1;                        //控球系数
        double dBaseVal = kItem.BasicPr;                                    //基础值

        double dVal = dTackleAttri * dEnergyAttri * dTackleCoeff - dDefCtrlAttri * dDefEnergyAttri * dDefCtrlCoeff;
        dVal /= (kSponsor.PlayerBaseInfo.Attri.lv * dSensCoeff);
        dVal += dBaseVal;
        dVal = Math.Max(dBaseVal * 0.1, dVal);
        m_dTackleSuccessPr = Math.Min(1, dVal);
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
        EEventType kType = EEventType.ET_Block;
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
        kNSEventDetail.Name = "抢断";
        kNSEventDetail.Value = m_dTackleSuccessPr;
        m_kEvtData.RetList.Add(kNSEventDetail);
        BattleStatistics.Instance.AddEvent(m_kSponsor, m_kEvtData.EvtID);
        BattleStatistics.Instance.AddEvent(m_kDefender, m_kEvtData.EvtID);
        BattleStatistics.Instance.AddAttri(m_kSponsor, 8);
        BattleStatistics.Instance.AddAttri(m_kDefender, 13);

        ResetDebugInfo();

        LogManager.Instance.LogWarning("开始事件:抢断 ===========================");
        LogManager.Instance.LogWarning("抢断概率:{0} ", m_dTackleSuccessPr);
        LogManager.Instance.LogWarning("发起方");
        LogManager.Instance.LogWarning("体力:{0}", TableManager.Instance.EnergyTbl.GetItem(m_kSponsor.PlayerBaseInfo.Energy).Value);
        LogManager.Instance.LogWarning("8:抢断属性:{0}", m_kSponsor.PlayerBaseInfo.Attri.steal);
        LogManager.Instance.LogWarning("被动方");
        LogManager.Instance.LogWarning("体力:{0}", TableManager.Instance.EnergyTbl.GetItem(m_kDefender.PlayerBaseInfo.Energy).Value);
        LogManager.Instance.LogWarning("13:控球属性:{0}", m_kDefender.PlayerBaseInfo.Attri.control);
        LogManager.Instance.LogWarning("结束事件:抢断 ===========================");

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
        get { return m_dTackleSuccessPr; }
    }

    public bool Valid
    {
        get { return m_bValid; }
    }

    public double RandVal
    {
        get { return m_dRandVal; }
    }
    private double m_dRandVal = 0;
    private LLUnit m_kSponsor;
    private LLUnit m_kDefender;
    private bool m_bValid = false;
    private double m_dTackleSuccessPr = 1; // 抢断成功概率

    private NSEventData m_kEvtData;
}