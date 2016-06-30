using Common.Tables;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
/// <summary>
/// 战斗统计信息
/// </summary>
/// 

public class NSEventDetail
{
    public string Name;
    public double Value;
}

public class NSEventData
{
    public EEventType EvtID;      // 事件类型
    public bool Valid = false;
    public List<NSEventDetail> RetList = new List<NSEventDetail>();
}
public class BattleStatistics
{
    public readonly static BattleStatistics Instance = new BattleStatistics();
    private BattleStatistics()
    {
        m_kRedInfo = new Dictionary<int, Dictionary<EEventType, int>>();
        m_kBlueInfo = new Dictionary<int, Dictionary<EEventType, int>>();
        m_kRedAttInfo = new Dictionary<int, Dictionary<int, int>>();
        m_kBlueAttInfo = new Dictionary<int, Dictionary<int, int>>();
        
    }

    public void Reset(LLScene kScene)
    {
        string strFileName = string.Format("{0}/statistics_{1}.txt", UnityEngine.Application.persistentDataPath, DateTime.Now.ToString("yyyyMMdd_HH_mm_ss"));
        FileStream kFileStream = new FileStream(strFileName,FileMode.Append);
        m_kStreamWriter = new StreamWriter(kFileStream, Encoding.UTF8);
        m_kStreamWriter.AutoFlush = true;
        m_kScene = kScene;
        m_kRedInfo.Clear();
        m_kBlueInfo.Clear();
        m_kRedAttInfo.Clear();
        m_kBlueInfo.Clear();
    }

    public void PrintStaticInfo()
    {
        m_kStreamWriter.WriteLine("比赛统计数据(红队VS蓝队)");
        m_kStreamWriter.WriteLine("比分\t 战力\t 阵型");
        m_kStreamWriter.WriteLine(string.Format("{0}:{1}\t {2}:{3}\t {4}:{5}",
            m_kScene.RedTeam.TeamInfo.Score, m_kScene.BlueTeam.TeamInfo.Score,
            m_kScene.RedTeam.TeamInfo.FightScore, m_kScene.BlueTeam.TeamInfo.FightScore,
            m_kScene.RedTeam.TeamInfo.ForamtionID, m_kScene.BlueTeam.TeamInfo.ForamtionID));

        m_kStreamWriter.WriteLine();
        m_kStreamWriter.WriteLine();
        m_kStreamWriter.WriteLine("====================比赛层数据统计====================");
        m_kStreamWriter.WriteLine("全场比赛各事件出现次数");
        int iValidCnt = 0;
        foreach(var kItem in m_kNSEvtDataList)
        {
            m_kStreamWriter.WriteLine(string.Format("\tg事件名{0}:次数{1}", kItem.EvtID.ToString(), kItem.RetList.Count));
            if (kItem.Valid)
                iValidCnt++;
        }

        m_kStreamWriter.WriteLine(string.Format("\t有效次数{0}", iValidCnt));
        m_kStreamWriter.WriteLine(string.Format("\t无效次数{0}", m_kNSEvtDataList.Count - iValidCnt));

        m_kStreamWriter.WriteLine("有效事件中各事件概率结果与明细");
        foreach (var kEvt in m_kNSEvtDataList)
        {
            if (false == kEvt.Valid)
                continue;
            m_kStreamWriter.WriteLine(string.Format("\t事件名{0}", kEvt.EvtID.ToString()));
            foreach(var kItem in kEvt.RetList)
            {
                m_kStreamWriter.WriteLine(string.Format("\t\t名字{0}", kItem.Name));
                m_kStreamWriter.WriteLine(string.Format("\t\t结果{0}", kItem.Value));
            }
        }

        m_kStreamWriter.WriteLine();
        m_kStreamWriter.WriteLine();
        m_kStreamWriter.WriteLine("====================球员层数据统计====================");

        m_kStreamWriter.WriteLine("全场比赛各球员对应事件名称及次数");
        m_kStreamWriter.WriteLine("红队球员");
        foreach(var kPlayer in m_kRedInfo)
        {
            m_kStreamWriter.WriteLine(string.Format("\t球员ID:{0}", kPlayer.Key));
            foreach(var kItem in kPlayer.Value)
                m_kStreamWriter.WriteLine(string.Format("\t事件名:{0}————次数:{1}", kItem.Key.ToString(),kItem.Value));
        }
        m_kStreamWriter.WriteLine("蓝队球员");
        foreach (var kPlayer in m_kBlueInfo)
        {
            m_kStreamWriter.WriteLine(string.Format("\t球员ID:{0}", kPlayer.Key));
            foreach (var kItem in kPlayer.Value)
                m_kStreamWriter.WriteLine(string.Format("\t事件名:{0}————次数:{1}", kItem.Key.ToString(), kItem.Value));
        }
        m_kStreamWriter.WriteLine();
        m_kStreamWriter.WriteLine("全场比赛各球员对应对应属性及被调用次数");
        m_kStreamWriter.WriteLine("红队球员");
        foreach (var kPlayer in m_kRedAttInfo)
        {
            m_kStreamWriter.WriteLine(string.Format("\t球员ID:{0}", kPlayer.Key));
            foreach (var kItem in kPlayer.Value)
                m_kStreamWriter.WriteLine(string.Format("\t属性id:{0}————次数:{1}", kItem.Key, kItem.Value));
        }
        m_kStreamWriter.WriteLine("蓝队球员");
        foreach (var kPlayer in m_kBlueAttInfo)
        {
            m_kStreamWriter.WriteLine(string.Format("\t球员ID:{0}", kPlayer.Key));
            foreach (var kItem in kPlayer.Value)
                m_kStreamWriter.WriteLine(string.Format("\t属性id:{0}————次数:{1}", kItem.Key, kItem.Value));
        }

        m_kStreamWriter.WriteLine("各球员战力");
        m_kStreamWriter.WriteLine("红队球员");
        foreach(var kItem in m_kScene.RedTeam.PlayerList)
            m_kStreamWriter.WriteLine(string.Format("\t球员ID:{0}—————战力{1}", kItem.PlayerBaseInfo.PlayerID,kItem.PlayerBaseInfo.FightScore));
        m_kStreamWriter.WriteLine(string.Format("\t门将ID:{0}—————战力{1}", m_kScene.RedTeam.GoalKeeper.PlayerBaseInfo.PlayerID, m_kScene.RedTeam.GoalKeeper.PlayerBaseInfo.FightScore));
        m_kStreamWriter.WriteLine("蓝队球员");
        foreach (var kItem in m_kScene.BlueTeam.PlayerList)
            m_kStreamWriter.WriteLine(string.Format("\t球员ID:{0}—————战力{1}", kItem.PlayerBaseInfo.PlayerID, kItem.PlayerBaseInfo.FightScore));
        m_kStreamWriter.WriteLine(string.Format("\t门将ID:{0}—————战力{1}", m_kScene.BlueTeam.GoalKeeper.PlayerBaseInfo.PlayerID, m_kScene.BlueTeam.GoalKeeper.PlayerBaseInfo.FightScore));
    }


    /// <summary>
    /// </summary>
    /// <param name="kUnit">事件发起者</param>
    /// <param name="kEvtType">事件类型</param>
    /// <param name="bValid">事件有效性</param>
    public void AddEvent(LLUnit kUnit, EEventType kEvtType)
    {
        if (null == kUnit)
            return;

        Dictionary<int, Dictionary<EEventType, int>> kCurDict;

        switch (kUnit.Team.TeamColor)
        {
            case Common.ETeamColor.Team_Red:
                kCurDict = m_kRedInfo;
                break;
            case Common.ETeamColor.Team_Blue:
                kCurDict = m_kBlueInfo;
                break;
            default:
                return;
        }

        int iHeroID = (int)kUnit.PlayerBaseInfo.HeroID;
        Dictionary<EEventType, int> kEvtList;
        kCurDict.TryGetValue(iHeroID, out kEvtList);
        if (null == kEvtList)
        {
            kEvtList = new Dictionary<EEventType, int>();
            kEvtList.Add(kEvtType, 1);
            kCurDict.Add(iHeroID, kEvtList);
        }
        else
        {
            if (kEvtList.ContainsKey(kEvtType))
                kEvtList[kEvtType]++;
            else
                kEvtList.Add(kEvtType, 1);
        }
    }

    public void AddAttri(LLUnit kUnit,int iAttrID)
    {
        if (null == kUnit)
            return;
        Dictionary<int, Dictionary<int, int>> kCurDict;

        switch (kUnit.Team.TeamColor)
        {
            case Common.ETeamColor.Team_Red:
                kCurDict = m_kRedAttInfo;
                break;
            case Common.ETeamColor.Team_Blue:
                kCurDict = m_kBlueAttInfo;
                break;
            default:
                return;
        }

        int iHeroID = (int)kUnit.PlayerBaseInfo.HeroID;
        Dictionary<int, int> kList;
        kCurDict.TryGetValue(iHeroID, out kList);
        if (null == kList)
        {
            kList = new Dictionary<int, int>();
            kList.Add(iAttrID, 1);
            kCurDict.Add(iHeroID, kList);
        }
        else
        {
            if (kList.ContainsKey(iAttrID))
                kList[iAttrID]++;
            else
                kList.Add(iAttrID, 1);
        }
    }

    public LLScene Scene
    {
        set { m_kScene = value; }
    }

   

    public List<NSEventData> NSEvtList
    {
        get { return m_kNSEvtDataList; }
        set { m_kNSEvtDataList = value; }
    }



    #region 成员变量
    #region 比赛统计数据
    private LLScene m_kScene;
    #endregion

    #region 比赛层统计数据
    private List<NSEventData> m_kNSEvtDataList = new List<NSEventData>();       // 有效事件概率统计
    #endregion
    #region 球员层
    /// <summary>
    /// 第一个int 表示球员ID,第二个int 表示事件次数
    /// </summary>
    private Dictionary<int, Dictionary<EEventType, int>> m_kRedInfo;           // 球员事件及次数
    private Dictionary<int, Dictionary<EEventType, int>> m_kBlueInfo;
    /// <summary>
    /// 第一个int 表示球员ID,第二个int表示属性索引值,第三个int 表示事件次数
    /// </summary>
    private Dictionary<int, Dictionary<int, int>> m_kRedAttInfo;                // 各属性被调用次数
    private Dictionary<int, Dictionary<int, int>> m_kBlueAttInfo;               // 各属性被调用次数
    // 各球员战力
    #endregion

    private StreamWriter m_kStreamWriter;

    #endregion

    
}
