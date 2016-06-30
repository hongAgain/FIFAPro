using Common;
using System;
using System.Collections.Generic;
/// <summary>
/// 逻辑层的球员基础信息层
/// </summary>
/// 
public class PlayerAttri
{
    public void Init(int[] kAttri)
    {
        m_kAttriList.Clear();
        for (int i = 0; i < kAttri.Length; i++)
            m_kAttriList.Add(kAttri[i]);
    }
    private int GetAttr(int index)
    {
        if (m_kAttriList == null || index >= m_kAttriList.Count)
        {
            return 0;
        }
        else
        {
            return m_kAttriList[index];
        }
    }

    public int lv
    {
        get { return 1; }
    }
    public int stamina { get { return GetAttr(0); } }   // 体力
    public int speed { get { return GetAttr(1); } } //速度
    public int power { get { return GetAttr(2); } } //力量
    public int shoot { get { return GetAttr(3); } } //射术
    public int freeKick { get { return GetAttr(4); } } //任意球
    public int longShoot { get { return GetAttr(5); } } //远射
    public int breakThrough { get { return GetAttr(6); } } //突破
    public int mark { get { return GetAttr(7); } }//盯人
    public int steal { get { return GetAttr(8); } }//抢断
    public int tackle { get { return GetAttr(9); } }//铲球
    public int intercept { get { return GetAttr(10); } }//拦截
    public int dribble { get { return GetAttr(11); } }//盘带
    public int longPass { get { return GetAttr(12); } }//长传
    public int control { get { return GetAttr(13); } }//控球
    public int shortPass { get { return GetAttr(14); } }//短传
    public int reaction { get { return GetAttr(15); } }//反应
    public int pudian { get { return GetAttr(16); } }//扑点
    public int chuji { get { return GetAttr(17); } }//出击
    public int save { get { return GetAttr(18); } }//扑救

    private List<int> m_kAttriList = new List<int>();
}


public class PlayerInfo
{
    public class SkillInfo
    {
        public int LV;
        public int ID;
    }
    public PlayerInfo()
    {
        m_kPlayerID = 0;
        m_kCareer = ECareer.ForwardFielder;
        m_dwHeroID = 0;
        m_dwClubID = Int32.MaxValue;
    }
    public PlayerInfo(int _playerId, ECareer _pptype, uint dwHeroID)
    {
        m_kPlayerID = _playerId;
        m_kCareer = _pptype;
        m_dwHeroID = dwHeroID;
        m_dwClubID = Int32.MaxValue;
    }
    public int PlayerID
    {
        get { return m_kPlayerID; }
        set { m_kPlayerID = value; }
    }
    public uint HeroID
    {
        get { return m_dwHeroID; }
        set { m_dwHeroID = value; }
    }

    public string HeroName
    {
        get { return m_strHeroName; }
        set { m_strHeroName = value; }
    }

    public uint ClubID
    {
        get { return m_dwClubID; }
        set { m_dwClubID = value; }
    }

    public ECareer Career
    {
        get { return m_kCareer; }
        set { m_kCareer = value; }
    }

    

    public List<SkillInfo> SkillList
    {
        get { return m_kSkillList; }
        set { m_kSkillList = value; }
    }

    public int Energy
    {
        get { return m_iEnergy; }
        set { m_iEnergy = value; }
    }

    public int Score
    {
        get { return m_iScore; }
        set { m_iScore = value; }
    }

    public PlayerAttri Attri
    {
        get { return m_kAttr; }
        set { m_kAttr = value; }
    }

    public int PosID
    {
        get { return m_iPosID; }
        set { m_iPosID = value; }
    }

    public int FightScore
    {
        get { return m_iFightScore; }
        set { m_iFightScore = value; }
    }

    public string CareerName
    {
        get { return m_strCareerName; }
        set { m_strCareerName = value; }
    }

    private string m_strCareerName; // 职业名称
    private int m_iPosID = -1;          // 球员站位ID
    private string m_strHeroName = "AI"; // 球员名字
    private uint m_dwHeroID;        //模型ID
    private uint m_dwClubID;        // 球服ID
    private int m_iEnergy;          // 球员体力
    private int m_kPlayerID;        // 球员ID
    private int m_iFightScore;      // 战力
    private ECareer m_kCareer;      // 球员职业
    private List<SkillInfo> m_kSkillList = new List<SkillInfo>();
    private PlayerAttri m_kAttr = new PlayerAttri();
    private int m_iScore = 0;       // 球员本场比赛得分
}
