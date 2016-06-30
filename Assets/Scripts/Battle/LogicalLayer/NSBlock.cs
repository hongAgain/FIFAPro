﻿using BehaviourTree;
using Common.Log;
using Common.Tables;
using System;
using System.Collections.Generic;

/* 
    Numerical Settler Block
    拦截
*/
public class NSBlock
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="kSponsor"> 数值对抗发起者</param>
    /// <param name="kDefUnit"> 拦截球员</param>
    public void Caculate(LLUnit kSponsor,LLUnit kDefUnit)
    {
        m_kSponsor = kSponsor;
        m_kDefender = kDefUnit;
        m_bValid = false;
        m_kEvtData = new NSEventData();
        m_kEvtData.EvtID = EEventType.ET_Snatch;
        m_kEvtData.Valid = false;
        BattleStatistics.Instance.NSEvtList.Add(m_kEvtData);
        if (false == CalcInteceptPr(kSponsor, kDefUnit))
            return;
        m_bValid = true;
        m_dRandVal = FIFARandom.GetRandomValue(0, 1);
        //GenPVEValidData();
        OutputDebugInfo();
    }
    

    /// <summary>
    /// 计算球被拦截的概率
    /// </summary>
    /// <param name="kSponsor"> 事件发起者 </param>
    /// <param name="kDefUnit"> 事件对抗者 </param>
    /// <returns></returns>
    private bool CalcInteceptPr(LLUnit kSponsor, LLUnit kDefUnit)
    {
        if(null == kSponsor || null == kDefUnit)
        {
            m_dInterceptPr = 1;
            return false;
        }
        SettlementFactorItem kItem = TableManager.Instance.SettlementFactorTbl.GetItem("short_pass_intercept");
        EnergyItem kEnergyItem = TableManager.Instance.EnergyTbl.GetItem(kSponsor.PlayerBaseInfo.Energy);
        if (null == kEnergyItem)
        {
            LogManager.Instance.Log("Energy table:eneryg is invalid");
            return false;
        }
        EnergyItem kDefEnergyItem = TableManager.Instance.EnergyTbl.GetItem(kDefUnit.PlayerBaseInfo.Energy);
        if (null == kDefEnergyItem)
        {
            LogManager.Instance.Log("Energy table:eneryg is invalid");
            return false;
        }

        double dSensCoeff = TableManager.Instance.SensitivityFactorTbl.GetItem(kSponsor.PlayerBaseInfo.Attri.lv).ShortPassIntercept; //敏感系数
        double dEnergyAttri = kEnergyItem.Value;                            //持球球员体力系数
        double dPassAttri = kSponsor.PlayerBaseInfo.Attri.shortPass;        //持球球员的短传属性
        double dPassCoeff = kItem.SponsorParam1;                            //持球球员短传系数
        double dDefInteceptAttri = kDefUnit.PlayerBaseInfo.Attri.intercept; //防守球员拦截属性
        double dDefEnergyAttri = kDefEnergyItem.Value;                      //防守球员体力系数
        double dInterceptCoeff = kItem.ReceiverParam1;                      //拦截系数
        double dBaseVal = kItem.BasicPr;                                    //基础值

        double dVal = dPassAttri * dEnergyAttri * dPassCoeff - dDefInteceptAttri * dDefEnergyAttri * dInterceptCoeff;
        dVal /= (kSponsor.PlayerBaseInfo.Attri.lv * dSensCoeff);
        dVal += dBaseVal;
        dVal = Math.Max(dBaseVal * 0.1, dVal);
        m_dInterceptPr = Math.Min(1, dVal);
        return true;
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
        kNSEventDetail.Name = "拦截";
        kNSEventDetail.Value = m_dInterceptPr;
        m_kEvtData.RetList.Add(kNSEventDetail);
        BattleStatistics.Instance.AddEvent(m_kSponsor, m_kEvtData.EvtID);
        BattleStatistics.Instance.AddEvent(m_kDefender, m_kEvtData.EvtID);
        BattleStatistics.Instance.AddAttri(m_kSponsor, 14);
        BattleStatistics.Instance.AddAttri(m_kDefender,10);
        ResetDebugInfo();
        LogManager.Instance.LogWarning("开始事件:拦截 ===========================");
        LogManager.Instance.LogWarning("拦截概率:{0} ", m_dInterceptPr);
        LogManager.Instance.LogWarning("发起方");
        if (null == m_kDefender)
        {
            LogManager.Instance.LogWarning("拦截球员为空");
            return;
        }
        LogManager.Instance.LogWarning("体力:{0}", TableManager.Instance.EnergyTbl.GetItem(m_kSponsor.PlayerBaseInfo.Energy).Value);
        LogManager.Instance.LogWarning("14:短传属性:{0}", m_kSponsor.PlayerBaseInfo.Attri.shortPass);
        LogManager.Instance.LogWarning("被动方");
        LogManager.Instance.LogWarning("体力:{0}", TableManager.Instance.EnergyTbl.GetItem(m_kDefender.PlayerBaseInfo.Energy).Value);
        LogManager.Instance.LogWarning("10:拦截属性:{0}", m_kDefender.PlayerBaseInfo.Attri.intercept);

        LogManager.Instance.LogWarning("结束事件:拦截 ===========================");

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
    /// <summary>
    ///  被拦截概率
    /// </summary>
    public double InterceptPr   
    {
        get { return m_dInterceptPr; }
    }

    /// <summary>
    /// 判断数据对抗是否有效
    /// </summary>
    public bool Valid
    {
        get { return m_bValid; }
    }

    public double RandVal
    {
        get { return m_dRandVal; }
    }
    private double m_dRandVal;
    private double m_dInterceptPr;          // 被拦截概率
    private bool m_bValid = false;
    #region

    private LLUnit m_kSponsor;
    private LLUnit m_kDefender;
    private NSEventData m_kEvtData;
    #endregion
}