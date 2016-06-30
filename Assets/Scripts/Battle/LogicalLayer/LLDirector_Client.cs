using System.Collections.Generic;
using LitJson;
using System.Diagnostics;
using System.Text;
using Common.Log;
using BehaviourTree;

public partial class LLDirector
{
    [Conditional("FIFA_CLIENT")]
    private void GameEndUIFinished_Client(float dTime)
    {
        m_bEndGame = false;
        if(false == m_bPlayAni)
        {
            m_fUIElapseTime = 0;
            m_bPlayAni = true;
            if (null != UIBattle.Instance)
            {
                UnityEngine.Transform kTransform = UIBattle.Instance.SkillRoot.FindChild("BattleDesc");
                if (null != kTransform && null != kTransform.gameObject)
                {
                    kTransform.gameObject.SetActive(true);
                    UnityEngine.Transform kLabel = kTransform.FindChild("Label");
                    UIHelper.SetLabelTxt(kLabel, Localization.Get("GameEnd"));
                    UnityEngine.Animation kAnimation = kTransform.GetComponent<UnityEngine.Animation>();
                    if (kAnimation)
                        kAnimation.Play("kaishi_in");
                }
            }
        }
        else
        {
            m_fUIElapseTime += dTime;
            if (m_fUIElapseTime > 1.5f)
            {
                m_bEndGame = true;
            }
        }
    }
  
    [Conditional("FIFA_CLIENT")]
    private void EndBattle_Client(bool bRealEnd)
    {
#if !GAME_AI_ONLY
        LuaInterface.LuaTable resultTable = (LuaInterface.LuaTable)LuaScriptMgr.Instance.CallLuaFunction("CombatData.GetEmptyResultTable")[0];
        int iRedScore = 0, iBlueScore = 0;
        CalcScore(ref iRedScore,ref iBlueScore);
        m_kScene.RedTeam.TeamInfo.Score = iRedScore;
        m_kScene.BlueTeam.TeamInfo.Score = iBlueScore;
        // 确保前面几局胜出
        if(iRedScore <= iBlueScore)
        {
            if (m_iLevelID <= 2)
                iRedScore = iBlueScore + (int)FIFARandom.GetRandomValue(1, 5);
        }
        resultTable["HomeScore"] = iRedScore;
        resultTable["AwayScore"] = iBlueScore;
        string strPVEData = GenPVEValidData();
        resultTable["PVEData"] = strPVEData;
        StringBuilder kBuilder = new StringBuilder();
 //       kBuilder.Append(m_strEnterBattleMD5);
        kBuilder.Append(m_kScene.RedTeam.TeamInfo.Score.ToString());
        kBuilder.Append(":");
        kBuilder.Append(m_kScene.BlueTeam.TeamInfo.Score.ToString());
        kBuilder.Append(strPVEData);
        kBuilder.Append(GlobalBattleInfo.Instance.MD5Val);
        resultTable["md5"] = Util.md5(kBuilder.ToString());
        resultTable["Giveup"] = bRealEnd;
        LuaScriptMgr.Instance.CallLuaFunction("CombatData.PushResult", resultTable);
        LogManager.Instance.Log(string.Format("参与计算的字符串：{0}",kBuilder.ToString()));
        LogManager.Instance.Log(string.Format("参与计算的字符串对应的MD5：{0}", resultTable["md5"]));
        WindowMgr.CloseWindow("UIBattle");
#endif
    }

    private string GenPVEValidData()
    {
        List<PVEValidData> kDataList = GlobalBattleInfo.Instance.PVEDataList;
        JsonData kJsonData = new JsonData(); 
        if(0 == kDataList.Count)
        {
            kJsonData.Add(null);
        }
        else
        {
            for (int i = 0; i < kDataList.Count; i++)
            {
                PVEValidData kData = kDataList[i];
                if (null == kData)
                    continue;
                JsonData kChildJsonData = new JsonData();
                kChildJsonData["id"] = JsonMapper.ToObject(JsonMapper.ToJson(kData.RandomValIdxList.ToArray()));      // 随机数ID
                kChildJsonData["aid"] = kData.ActionID;         // 行为ID
                kChildJsonData["ss"] = kData.SponsorTeamScore;  // 发起者球队分数
                kChildJsonData["ds"] = kData.DefendTeamScore;   // 被发起球队得分
                kChildJsonData["pd"] = JsonMapper.ToObject(JsonMapper.ToJson(kData.SponsorIDList.ToArray()));       // 发起者球员
                kChildJsonData["pdt"] = JsonMapper.ToObject(JsonMapper.ToJson(kData.SEnergyList.ToArray()));       // 发起者球员体力
                kChildJsonData["tc"] = kData.TeamColor;         // 发起者球员所属球队
                JsonData kDefObj = new JsonData();
                for(int iDefIdx= 0;iDefIdx < kData.DefenderIDList.Count;iDefIdx++ )
                {
                    kDefObj[iDefIdx.ToString()] = JsonMapper.ToObject(JsonMapper.ToJson(kData.DefenderIDList[iDefIdx].ToArray()));
                }
                kChildJsonData["did"] = kDefObj;      // 被动参与的球员idlist
                kChildJsonData["didt"] = JsonMapper.ToObject(JsonMapper.ToJson(kData.DEnergyList.ToArray()));       // 承受者球员体力（如果是多个球员，则是平均体力）
                kJsonData.Add(kChildJsonData);
            }
        }
        
        GlobalBattleInfo.Instance.PVEDataList.Clear();
        string strVal = kJsonData.ToJson();
        strVal = strVal.Replace("[null]", "[]");
        return strVal;
    }

    public void GenPVEDataMd5(string strVal)
    {
        //m_strOriData = strVal.Replace("\"skill\":[null]", "\"skill\":[]");
        //LogManager.Instance.RedLog(m_strOriData);
        //m_strEnterBattleMD5 = Util.md5(m_strOriData + GlobalBattleInfo.Instance.MD5Val);
        //LogManager.Instance.Log(string.Format("源字符串{0}", m_strOriData + GlobalBattleInfo.Instance.MD5Val));
        //LogManager.Instance.Log(string.Format("源字符串对应的MD5", m_strEnterBattleMD5));
    }

    private string m_strOriData;
    private string m_strEnterBattleMD5;
    private bool m_bPlayAni = false;
    private float m_fUIElapseTime = 0;
}
