using BehaviourTree;
using Common;
using Common.Log;
using Common.Tables;
using LitJson;
using LuaInterface;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

//导演类，用于管理整个游戏比赛场景

public enum EBattleType
{
    Raid = 0,
    Ladder
}

public class PLDirector : MonoBehaviour
{
    public static PLDirector Instance;
    public void Awake()
    {
        Instance = this;
    }
    public void Start()
    {
        LLDirector.Instance.Reset();
        // 获得球队与球员信息
        if (false == Init())
            return;
        LLDirector.Instance.Start(m_iLevelID);
        m_fCurTime = Time.realtimeSinceStartup;
    }

    public void FixedUpdate()
    {
    }

    public void Update()
    {
        m_fDeltaTime = Time.realtimeSinceStartup - m_fCurTime;
        m_fCurTime = Time.realtimeSinceStartup;
        GlobalBattleInfo.Instance.DeltaTime = Time.deltaTime * GlobalBattleInfo.Instance.PlaySpeed;
        LLDirector.Instance.Update(GlobalBattleInfo.Instance.DeltaTime);       
    }

    public void OnExit()
    {
        DebugGUI kObj = gameObject.GetComponent<DebugGUI>();
        if (null != kObj)
            UnityEngine.Object.Destroy(kObj);
    }

 
    protected bool Init()
    {

#if !GAME_AI_ONLY
        // 初始化比赛所需要的各种信息
        LuaFunction kGetDataFunc = LuaScriptMgr.Instance.GetLuaFunction("CombatData.GetData");
        if(null == kGetDataFunc)
            return false;
        
        object[] kObj = kGetDataFunc.Call();
        if (null == kObj)
            return false;
        LuaTable kTable = kObj[0] as LuaTable;

        if (null == kTable)
            return false;
        //string strVal = ConvertJsonData(kTable);
        //LLDirector.Instance.GenPVEDataMd5(strVal);
        EBattleType kType = EBattleType.Raid;
        string strType = kTable["type"].ToString();
        if(strType == "raid")
        {
            kType = EBattleType.Raid;
        }
        else if(strType == "pvp")
        {
            kType = EBattleType.Ladder;
        }
        if (null != kTable["id"])
            m_iLevelID = int.Parse(kTable["id"].ToString());
        LuaTable kP1 = kTable["P1"] as LuaTable;
        LuaTable kP2 = kTable["P2"] as LuaTable;
        LuaTable kRand = kTable["ranArr"] as LuaTable;
  
        foreach (var kAttr in kRand.Values)
        {
            FIFARandom.RandomList.Add(int.Parse(kAttr.ToString()) / 100.0f);
        }
        if (null == kP1 || null == kP2)
            return false;
        if (false == InitTeamInfo(ETeamColor.Team_Red, kP1, kType)) // Home
            return false;
        if (false == InitTeamInfo(ETeamColor.Team_Blue, kP2, kType)) // Away
            return false;
#endif
        InitTeamInfoForGameAIOnly(ETeamColor.Team_Red);
        InitTeamInfoForGameAIOnly(ETeamColor.Team_Blue);
        return true;
    }

    private string ConvertJsonData(LuaTable kTable)
    {
        if (null == kTable)
            return "";

        JsonData kData = new JsonData();
        kData["id"] = int.Parse(kTable["id"].ToString());
        kData["type"] = kTable["type"].ToString();
        kData["time"] = long.Parse(kTable["time"].ToString());
        
        LuaTable kP1 = kTable["P1"] as LuaTable;
        kData["P1"] = ConvertTeamInfoToJson(kP1);
        LuaTable kP2 = kTable["P2"] as LuaTable;
        kData["P2"] = ConvertTeamInfoToJson(kP2);
        JsonData kRandArrData = new JsonData();
        LuaTable kRandArr = kTable["ranArr"] as LuaTable;
        int[] kAttrList = new int[kRandArr.Count];
        int iAttrIdx = 0;
        foreach (var kAttr in kRandArr.Values)
        {
            kAttrList[iAttrIdx++] = int.Parse(kAttr.ToString());
        }
        kData["ranArr"] = JsonMapper.ToObject(JsonMapper.ToJson(kAttrList));
        string strVal = kData.ToJson();
        return strVal;
    }

    protected JsonData ConvertTeamInfoToJson(LuaTable kTable)
    {
        JsonData kData = new JsonData();
        kData["name"] = kTable["name"].ToString();
        kData["id"] = int.Parse(kTable["id"].ToString());
        JsonData kHeroData = new JsonData();
        kData["hero"] = kHeroData;
        LuaTable kHeroTable = kTable["hero"] as LuaTable;
        JsonData kLData = new JsonData();
        LuaTable kLTable = kTable["l"] as LuaTable;
        int[] kLList = new int[kLTable.Count];
        int iIdx = 0;
        foreach (var kItem in kLTable.Values)
        {
            kLList[iIdx++] = int.Parse(kItem.ToString());
            kHeroData[kItem.ToString()] = ConvertHeroToJson(kHeroTable[kItem.ToString()] as LuaTable);
        }
        kData["l"] = JsonMapper.ToObject(JsonMapper.ToJson(kLList));
        return kData;
    }
    protected JsonData ConvertHeroToJson(LuaTable kTable)
    {
        JsonData kData = new JsonData();
        kData["id"] = int.Parse(kTable["id"].ToString());
        kData["ap"] = int.Parse(kTable["ap"].ToString());
        kData["pos"] = int.Parse(kTable["pos"].ToString());

        LuaTable kAttTable = kTable["att"] as LuaTable;
        if(kAttTable.Count > 0)
        {
            int[] kAttrList = new int[kAttTable.Count];
            int iAttrIdx = 0;
            foreach (var kAttr in kAttTable.Values)
            {
                kAttrList[iAttrIdx++] = int.Parse(kAttr.ToString());
            }
            kData["att"] = JsonMapper.ToObject(JsonMapper.ToJson(kAttrList));
        }

        LuaTable kSkillTable = kTable["skill"] as LuaTable;
        JsonData kSkillData = new JsonData();
        if (kSkillTable.Count > 0)
        {
            string[] kSkillList = new string[kSkillTable.Count];
            int iIdx = 0;
            foreach (var kItem in kSkillTable.Values)
            {
                kSkillList[iIdx++] = kItem.ToString();
            }
            kData["skill"] = JsonMapper.ToObject(JsonMapper.ToJson(kSkillList));
        }
        else
        {
            kData["skill"] = new JsonData();
            kData["skill"].Add(null);
        }
            
        //foreach (var kVal in kSkillTable.Keys)
        //{
        //    kSkillData[kVal.ToString()] = ConvertSkillToJsonData(kSkillTable[kVal.ToString()] as LuaTable);//ConvertHeroToJson(kHeroTable[kVal.ToString()] as LuaTable);
        //}
        //if (kSkillTable.Count > 0)
        //{
        //    int[] kSkillList = new int[kSkillTable.Count];
        //    int iSkillIdx = 0;
        //    foreach (var kAttr in kSkillTable.Values)
        //    {
        //        kSkillList[iSkillIdx++] = int.Parse(kAttr.ToString());
        //    }
        //    kData["skill"] = JsonMapper.ToObject(JsonMapper.ToJson(kSkillList));
        //}
        return kData;
    }

    private JsonData ConvertSkillToJsonData(LuaTable kTable)
    {
        JsonData kData = new JsonData();
        string strVal = kTable["idx"].ToString();
        if (!string.IsNullOrEmpty(strVal))
            kData["idx"] = int.Parse(strVal);
        strVal = kTable["id"].ToString();
        if (!string.IsNullOrEmpty(strVal))
            kData["id"] = int.Parse(strVal);
        strVal = kTable["lv"].ToString();
        if (!string.IsNullOrEmpty(strVal))
            kData["lv"] = int.Parse(strVal);
        return kData;
    }

    class InnerPlayerInfo
    {
        public int iIdx;
        public PlayerInfo kInfo;
    };

    protected bool InitTeamInfo(ETeamColor kTeamColor,LuaTable kTable,EBattleType kType)
    {
        int iID = int.Parse(kTable["id"].ToString());
        TeamData kTeamData = new TeamData();
        kTeamData.TeamName = kTable["name"].ToString();
        kTeamData.FormationID = iID;
        if (null != kTable["icon"])
        {
            if(false == string.IsNullOrEmpty(kTable["icon"].ToString()))
                kTeamData.ClubID = int.Parse(kTable["icon"].ToString());
            else
                kTeamData.ClubID = 48001;
        }
            
        else
            kTeamData.ClubID = 48001;
        int iFightScore = 0;
        HeroTable kHeroTbl = TableManager.Instance.HeroTbl;
        RaidNPCTable kRaidNpcTbl = TableManager.Instance.RaidNpcTbl;

        List<InnerPlayerInfo> kInnerList = new List<InnerPlayerInfo>();
        FormationItem kFormationItem = TableManager.Instance.FormationTbl.GetItem(iID);
        LuaTable kHeroTable = kTable["hero"] as LuaTable;
        foreach (var kVal in kHeroTable.Keys)
        {
            PlayerInfo kInfo = new PlayerInfo();
            LuaTable vTable = kHeroTable[kVal.ToString()] as LuaTable;
            int iPos = int.Parse(vTable["pos"].ToString());
            ECareer kCareer = ECareer.ForwardFielder;
            if(0 == iPos)
                kCareer = ECareer.Goalkeeper;
            else if (iPos == kHeroTable.Keys.Count-1)
                kCareer = ECareer.ForwardFielder;
            else if (iPos > 0 && iPos < 5)
                kCareer = ECareer.BackFielder;
            else
                kCareer = ECareer.MidFielder;
            if (null != kFormationItem && iPos < kFormationItem.ProList.Count)
                kInfo.PosID = kFormationItem.ProList[iPos];
            else
                kInfo.PosID = 0;
            ProfessionItem kProItem = TableManager.Instance.ProfessionTbl.GetItem(kInfo.PosID);
            if (null != kProItem)
                kInfo.CareerName = kProItem.Name;
            else
                kInfo.CareerName = "";
            kInfo.HeroID = uint.Parse(vTable["id"].ToString()); //id  
            kInfo.FightScore = int.Parse(vTable["ap"].ToString()); //ap 战力
            kInfo.ClubID = (uint)(kTeamData.ClubID);
            kInfo.Career = kCareer;          
            kInfo.PlayerID = int.Parse(kVal.ToString());        // key

            switch (kTeamColor)
            {
                case ETeamColor.Team_Blue:
                    {
                        if(EBattleType.Raid == kType)
                        {
                            RaidNPCItem kItem = kRaidNpcTbl.GetItem((int)kInfo.PlayerID);
                            if (null == kItem)
                                kInfo.HeroName = "unknown";
                            else
                                kInfo.HeroName = kItem.Name;
                        }
                        else if(EBattleType.Ladder == kType)
                        {
                            HeroItem kItem = kHeroTbl.GetItem((int)kInfo.HeroID);
                            if (null == kItem)
                                kInfo.HeroName = "unknown";
                            else
                                kInfo.HeroName = kItem.Name;
                        }
                    }
                    break;
                case ETeamColor.Team_Red:
                    {
                        HeroItem kItem = kHeroTbl.GetItem((int)kInfo.HeroID);
                        if (null == kItem)
                            kInfo.HeroName = "unknown";
                        else
                            kInfo.HeroName = kItem.Name;
                    }
                    
                    break;
                default:
                    break;
            }
            iFightScore += kInfo.FightScore;
            LuaTable kAttTable = (LuaTable)vTable["att"];
            int[] kAttrList = new int[kAttTable.Count];
            int iAttrIdx = 0;
            foreach (var kAttr in kAttTable.Values)
            {
                kAttrList[iAttrIdx++] = int.Parse(kAttr.ToString());
            }
            kInfo.Attri.Init(kAttrList);

            LuaTable kSkillList = (LuaTable)vTable["skill"];
            foreach (var kSkill in kSkillList.Values)
            {
                LuaTable kSkillTbl = (LuaTable)kSkill;

                PlayerInfo.SkillInfo kSkillInfo = new PlayerInfo.SkillInfo();
                if(null != kSkillTbl["id"])
                    kSkillInfo.ID = int.Parse(kSkillTbl["id"].ToString());
                if(null != kSkillTbl["lv"])
                    kSkillInfo.LV = int.Parse(kSkillTbl["lv"].ToString());
                kInfo.SkillList.Add(kSkillInfo);
            }
            kInfo.Energy = kInfo.Attri.stamina;
            InnerPlayerInfo kInnerItem = new InnerPlayerInfo();
            kInnerItem.kInfo = kInfo;
            kInnerItem.iIdx = iPos;
            kInnerList.Add(kInnerItem);
        }

        kInnerList.Sort((InnerPlayerInfo a, InnerPlayerInfo b) =>
        {
            return a.iIdx.CompareTo(b.iIdx); 
        });

        for(int i = 0;i < kInnerList.Count;i++)
            kTeamData.AddPlayer(kInnerList[i].kInfo);
        kTeamData.FightingScore = iFightScore;
        LLDirector.Instance.AddTeamData(kTeamColor, kTeamData);
        return true;
    }

    [Conditional("GAME_AI_ONLY")]
    protected void InitTeamInfoForGameAIOnly(ETeamColor kColorType)
    {
        TeamData kTeamData = new TeamData();
        kTeamData.FormationID = 1;
        FormationItem kFormationItem = TableManager.Instance.FormationTbl.GetItem(kTeamData.FormationID);
        HeroTable kHeroTbl = TableManager.Instance.HeroTbl;
        uint dwClubID = 48001;
        int iFightScore = 0;
        switch (kColorType)
        {
            case ETeamColor.Team_Blue:
                dwClubID = 48001;
                break;
            case ETeamColor.Team_Red:
                dwClubID = 48002;
                break;
        }
        int iBaseID = 50101;
        for (int i = 0; i < 11; i++)
        {
            ECareer kCareer = ECareer.ForwardFielder;
            if (0 == i)
                kCareer = ECareer.Goalkeeper;
            else if (i == 10)
                kCareer = ECareer.ForwardFielder;
            else if (i > 0 && i < 5)
                kCareer = ECareer.BackFielder;
            else
                kCareer = ECareer.MidFielder;
            PlayerInfo kInfo = new PlayerInfo();
            if (null != kFormationItem && i < kFormationItem.ProList.Count)
                kInfo.PosID = kFormationItem.ProList[i];
            else
                kInfo.PosID = 0;
            ProfessionItem kProItem = TableManager.Instance.ProfessionTbl.GetItem(kInfo.PosID);
            if (null != kProItem)
                kInfo.CareerName = kProItem.Name;
            kInfo.HeroID = (uint)(iBaseID + i);
            kInfo.ClubID = dwClubID;
            kInfo.Career = kCareer;
            kInfo.PlayerID = (int)kInfo.HeroID;
            kInfo.FightScore = (int)FIFARandom.GetRandomValue(500, 800);
            iFightScore += kInfo.FightScore;
            HeroItem kHeroItem = kHeroTbl.GetItem((int)(kInfo.HeroID));
            if (null != kHeroItem)
            {
                for(int iSkillIdx = 0; iSkillIdx < kHeroItem.SkillList.Count-1; iSkillIdx++)
                {
                    PlayerInfo.SkillInfo kSkillInfo = new PlayerInfo.SkillInfo();
                    kSkillInfo.ID = kHeroItem.SkillList[iSkillIdx];
                    kSkillInfo.LV = 1;
                    kInfo.SkillList.Add(kSkillInfo);
                }
            }
            kTeamData.AddPlayer(kInfo);
        }
        kTeamData.FightingScore = iFightScore;
        LLDirector.Instance.AddTeamData(kColorType, kTeamData);
    }

    private int m_iLevelID = 1;
    private float m_fDeltaTime;
    private float m_fCurTime;
}
