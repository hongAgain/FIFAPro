
using BehaviourTree;
using System.Collections.Generic;

public class GlobalBattleInfo
{
    public static readonly GlobalBattleInfo Instance = new GlobalBattleInfo();

    public float GetRealWorldTime()     // 秒级
    {
        return LLDirector.Instance.ElapseTime * m_fRealWorldRaceTime / m_fMaxTimeMatch;
    }

    public float ConvertToRealWorldTime(float fTime)
    {
        return fTime * m_fRealWorldRaceTime / m_fMaxTimeMatch;
    }

    public float ConverToGameTime(float fTime)
    {
        return fTime * m_fMaxTimeMatch / m_fRealWorldRaceTime;
    }

    public float PlaySpeed   // 战斗播放速度
    {
        get
        {
            if (m_fPlaySpeed <= 0)
                m_fPlaySpeed = 1.0f;
            return m_fPlaySpeed;
        }
        set { m_fPlaySpeed = value; }
    }

    public float MaxPlaySpeed
    {
        get
        {
            if (m_fMaxPlaySpeed <= 0.0f)
                m_fMaxPlaySpeed = 16.0f;
            return m_fMaxPlaySpeed;
        }
        set { m_fMaxPlaySpeed = value; }
    }

    public bool CanSkip         // 战斗是否可以跳过
    {
        get { return m_bCanSkip; }
        set { m_bCanSkip = value; }
    }

    public float SnatchPr
    {
        get{return m_fSnatchPr;}
        set{m_fSnatchPr = value;}
    }

    public float DeltaTime
    {
        get { return m_fGameDeltaTime; }
        set { m_fGameDeltaTime = value; }
    }
    public float MaxTimeMatch
    {
        get { return m_fMaxTimeMatch; }
        set { m_fMaxTimeMatch = value; }
    }

    public float RealWorldRaceTime
    {
        get { return m_fRealWorldRaceTime; }
        set { m_fRealWorldRaceTime = value; }
    }

    public float ScaleTime
    {
        get
        {
            if (0 > m_fScaleTime)
                return 1;
            else
                return m_fScaleTime;
        }
        set { m_fScaleTime = value; }
    }

    public float RealWorldCtrlRateInterval
    {
        get { return m_fRealWorldCtrlRateUpdateInterval; }
        set { m_fRealWorldCtrlRateUpdateInterval = value; }
    }

    // PVE 检验数据
    public List<PVEValidData> PVEDataList
    {
        get { return m_kPVEValidList; }
        set { m_kPVEValidList = value; }
    }
    // PVP 检验数据
    public List<PVPValidData> PVPDataList
    {
        get { return m_kPVPValidList; }
        set { m_kPVPValidList = value; }
    }
    // 战斗校验数据

    public string MD5Val
    {
        get { return m_strMD5Val; }
    }
    private List<PVEValidData> m_kPVEValidList = new List<PVEValidData>();      // PVE数据
    private List<PVPValidData> m_kPVPValidList = new List<PVPValidData>();      // PVP数据
    
    private float       m_fSnatchPr = 0.5f;                                     // 抢断概率
    private bool        m_bCanSkip = true;                                      // 战斗是否可跳过
    private float       m_fPlaySpeed = 1.0f;                                    // 战斗快播速度
    private float       m_fMaxPlaySpeed = 16.0f;                                // 战斗最快播放速度
    private float       m_fMaxTimeMatch = 180f;                                 // 一场比赛完整时间30*60秒  
    private float       m_fRealWorldRaceTime = 5400;                            // 单位秒 90分钟 现实世界中比赛时间
    private float       m_fGameDeltaTime = 0.0f;                                // 比赛每次更新的时间间隔 游戏时间 非现实世界时间
    private float       m_fRealWorldCtrlRateUpdateInterval = 30;                // 单位秒 真实世界的时间 单位秒
    private float       m_fScaleTime = 1;

    private string m_strMD5Val = "FifaPro007.sz.h";
}