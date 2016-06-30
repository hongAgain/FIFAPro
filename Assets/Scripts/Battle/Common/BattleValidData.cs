
using BehaviourTree;
using System.Collections.Generic;

public class PVEValidData
{
    public List<int> SponsorIDList
    {
        get { return m_iSponsorIDList; }
        set { m_iSponsorIDList = value; }
    }

    public List<List<int>> DefenderIDList
    {
        get { return m_iDefenderIDList; }
        set { m_iDefenderIDList = value; }
    }

    public int ActionID
    {
        get { return m_iActionID; }
        set { m_iActionID = value; }
    }

    public int SponsorTeamScore
    {
        get { return m_iSponsorScore; }
        set { m_iSponsorScore = value; }
    }

    public List<int> RandomValIdxList
    {
        get { return m_iRadValIdxList; }
        set { m_iRadValIdxList = value; }
    }

    public int DefendTeamScore
    {
        get { return m_iDefendScore; }
        set { m_iDefendScore = value; }
    }

    public int TeamColor
    {
        get { return m_iTeamColor; }
        set { m_iTeamColor = value; }
    }

    public List<double> SEnergyList
    {
        get { return m_kSponsorEnergyList; }
        set { m_kSponsorEnergyList = value; }
    }
    public List<double> DEnergyList
    {
        get { return m_kDefEnergyList; }
        set { m_kDefEnergyList = value; }
    }

    private List<double> m_kSponsorEnergyList = new List<double>();     // 事件发起者体力
    private List<double> m_kDefEnergyList = new List<double>();         // 事件被动者体力
    private List<int> m_iRadValIdxList = new List<int>();               // 随机数索引值
    private List<int> m_iSponsorIDList = new List<int>();               // 玩家球员ID
    private List<List<int>> m_iDefenderIDList = new List<List<int>>();  // NPC球员ID
    private int m_iActionID;                                            // 事件类型ID
    private int m_iSponsorScore;                                        // 玩家得分
    private int m_iDefendScore;                                         // NPC得分
    private int m_iTeamColor;                                           // 0 表示红队 1 表示蓝队
}

public class PVPValidData
{

}
//public enum EActionType
//{
//    ActionID_Shoot = 1,     // 射门
//    ActionID_FarShoot,      // 远射
//    ActionID_HeadShoot,     // 头球射门
//    ActionID_LongPass,      // 长传
//    ActionID_ShortPass,     // 短传
//    ActionID_Block,         // 抢断事件
//    ActionID_Snatch,        // 铲球
//    ActionID_Break,         // 突破
//    ActionID_HeadRob,       // 头球争顶
//}
// 数据检验
public class DataValidManager
{
    public static DataValidManager Instance = new DataValidManager();

    private DataValidManager()
    {

    }

    
    //public void AddValidData(EActionType kType,LLUnit kSponsor,List<LLUnit> kUnitList,bool bPVEMode)
    //{
    //    switch(kType)
    //    {
    //        case EActionType.ActionID_Shoot:
    //            AddShoot(kType,kSponsor,kUnitList,bPVEMode);
    //            break;
    //        case EActionType.ActionID_GKSave:
    //            AddGkSave(kType, kSponsor, kUnitList, bPVEMode);
    //            break;
    //        default:
    //            break;
    //    }



    //}

    //private void AddShoot(EActionType kType, LLUnit kSponsor, List<LLUnit> kUnitList, bool bPVEMode)
    //{
    //    if(bPVEMode)
    //    {

    //    }
    //    else
    //    {

    //    }
    //}
    //private void AddGkSave(EActionType kType, LLUnit kSponsor, List<LLUnit> kUnitList, bool bPVEMode)
    //{
    //    if (null == kSponsor || 0 == kUnitList.Count)
    //        return;
    //    if (bPVEMode)
    //    {
    //        PVEValidData kData = new PVEValidData();
    //        kData.ActionID = (int)kType;
    //        kData.TeamColor = 0;
    //        if(kSponsor.Team.TeamColor == Common.ETeamColor.Team_Blue)
    //            kData.TeamColor = 1;
    //        kData.SponsorIDList = kSponsor.PlayerBaseInfo.PlayerID;
    //        for (int i = 0; i < kUnitList.Count; i++)
    //            kData.DefenderIDList.Add(kUnitList[i].PlayerBaseInfo.PlayerID);
    //        kData.SponsorTeamScore = kSponsor.Team.TeamInfo.Score;
    //        kData.DefendTeamScore = kSponsor.Team.Opponent.TeamInfo.Score;
    //        kData.RandomValIdx = FIFARandom.GetCurRandomIdx();
    //        GlobalBattleInfo.Instance.PVEDataList.Add(kData);
    //    }
    //    else
    //    {

    //    }
    //}
}