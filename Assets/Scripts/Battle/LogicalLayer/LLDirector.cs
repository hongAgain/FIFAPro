using System;
using Common;
using System.Collections.Generic;
using BehaviourTree;

public class TeamData
{
    public int FormationID
    {
        get { return m_iFormationID; }
        set { m_iFormationID = value; }
    }

    public List<PlayerInfo> PlayerList
    {
        get { return m_kPlayerList; }
    }

    public string TeamName
    {
        get { return m_strTeamName; }
        set { m_strTeamName = value; }
    }

    public int FightingScore
    {
        get { return m_iFightingScore; }
        set { m_iFightingScore = value; }
    }

    public int ClubID
    {
        get { return m_dwClubID; }
        set { m_dwClubID = value; }
    }

    public void AddPlayer(PlayerInfo kPlayer)
    {
        m_kPlayerList.Add(kPlayer);
    }

    private int m_dwClubID;
    private int m_iFightingScore;   // 战力
    private int m_iFormationID;
    private string m_strTeamName;   // 球队名字
    List<PlayerInfo> m_kPlayerList = new List<PlayerInfo>();
}

public partial class LLDirector
{
    enum EState
    {
        FirstHalf = 0,
        SecondHalf,
        Pause,
        Skip,
        Finished,
        Invalid
    }
    public static readonly LLDirector Instance = new LLDirector();

    public void Reset()
    {
        m_kTeamData.Clear();
    }

    public void Start(int iLevelID = 1)
    {
        m_iLevelID = iLevelID;
        m_kState = EState.FirstHalf;
        m_fTotalTime = m_fRemainingTime = m_fHalfTime * 2;  //秒
        Play("Scene/Court01");
    }
    public void Update(float fTime)
    {
        switch (m_kState)
        {
            case EState.FirstHalf:
                OnFirstHalfState(fTime);
                break;
            case EState.SecondHalf:
                OnSecondHalfState(fTime);
                break;
            case EState.Skip:
                OnSkipState();
                break;
            case EState.Finished:
                OnFinishedState(fTime);
                break;
            case EState.Pause:
                break;
            default:
                break;
        }
    }

    protected void OnSkipState()
    {
        GlobalBattleInfo.Instance.PlaySpeed = GlobalBattleInfo.Instance.MaxPlaySpeed;
        float fFixDeltaTime = 0.3f* GlobalBattleInfo.Instance.PlaySpeed;
        float fTimeRecord = m_fRemainingTime;
        while (m_fRemainingTime > 0)
        {
            BattleUpdate(fFixDeltaTime);
            m_fRemainingTime -= fFixDeltaTime;
        }
        m_fRemainingTime = fTimeRecord;
        EndBattle(false);
        m_kState = EState.Invalid;
    }

    protected void OnFinishedState(float fTime)
    {
        GameEndUIFinished_Client(fTime);
        if (false == m_bEndGame)
            return;
        EndBattle(false);
        m_kState = EState.Invalid;
    }
    protected void OnFirstHalfState(float fTime)
    {
        if (m_fRemainingTime < m_fHalfTime)
        {
            if (false == m_bWaitForFirstHalfEnd)
            {
                m_kState = EState.SecondHalf;
                if (null != m_kScene)
                {
                    m_kScene.SetState(EGameState.GS_HALFEND);
                }
            }
            else
                BattleUpdate(fTime);
        }
        else
            BattleUpdate(fTime);
    }

    protected void OnSecondHalfState(float fTime)
    {
        if (m_fRemainingTime < 0)
        {
            m_kState = EState.Finished;
        }
        else
            BattleUpdate(fTime);
    }
    protected void BattleUpdate(float fTime)
    {
        if(m_bEnableCalcTime)
            m_fRemainingTime -= fTime;
        if (null != m_kScene)
            m_kScene.Update(fTime);
    }

    public void Pause()
    {
        if (null == m_kScene)
            return;
        m_kScene.Pause();
        m_kPauseState = m_kState;
        m_kState = EState.Pause;
    }
    public void Resume()
    {
        if (null == m_kScene)
            return;
        m_kScene.Resume();
        m_kState = m_kPauseState;
    }
    public void Stop()
    {

    }

    public void Destroy()
    {

    }

    public void ChangeState()
    {
        if (null == m_kScene)
            return;

        m_kScene.RequireTeamStateChange(ETeamStateChangeType.TSCT_DEBUG, true);
    }

    public void ChangeTeamFormation(int uiID, ETeamColor kTeamType)
    {
        if (null == m_kScene)
            return;
        m_kScene.ChangeTeamFormation(uiID, kTeamType);
    }

    public void Play(string strSceneName)
    {
        m_bWaitForFirstHalfEnd = false;
        m_bEndGame = false;
        GlobalBattleInfo.Instance.PlaySpeed = 1;
        if (null != m_kScene)
        {
            if (m_kScene.SceneName != strSceneName)
            {
                m_kScene.Destroy();
                m_kScene = new LLScene(strSceneName);
            }
        }
        else
        {
            m_kScene = new LLScene(strSceneName);
        }
        m_bIsPlaying = true;
        m_kScene.InitScene(m_kTeamData[ETeamColor.Team_Red], m_kTeamData[ETeamColor.Team_Blue]);
    }


    public void Restart(string strSceneName, UInt32 uiTeamID)
    {
        if (null != m_kScene)
        {
            m_kScene.Destroy();
        }
        m_kScene = new LLScene(strSceneName);
        m_kScene.InitScene(m_kTeamData[ETeamColor.Team_Red], m_kTeamData[ETeamColor.Team_Blue]);
    }

    public void SkipBattle()
    {
        m_kState = EState.Skip;
    }

    public void SpeedUp()
    {
        if (GlobalBattleInfo.Instance.PlaySpeed * 2 < GlobalBattleInfo.Instance.MaxPlaySpeed)
            GlobalBattleInfo.Instance.PlaySpeed *= 2;
    }

    public void ExitBattle()
    {
        DestroyPreBattleUIMessage kMsg = new DestroyPreBattleUIMessage();
        MessageDispatcher.Instance.SendMessage(kMsg);
        m_kState = EState.Finished;
    }


    private void EndBattle(bool bRealEnd)
    {
        BattleStatistics.Instance.PrintStaticInfo();
        EndBattle_Client(bRealEnd);
        if (null != m_kScene)
        {
            m_kScene.Destroy();
            m_kScene = null;
        }
        ExitBattleMessage kMsg = new ExitBattleMessage();
        MessageDispatcher.Instance.SendMessage(kMsg);
    }
    

    public bool IsPlaying
    {
        get { return m_bIsPlaying; }

    }

    public LLScene Scene
    {
        get { return m_kScene; }
    }

    public float ElapseTime
    {
        get { return m_fTotalTime - m_fRemainingTime; }
    }

    public float RemaingTime
    {
        get { return m_fRemainingTime; }
    }

    public void AddTeamData(ETeamColor kColor, TeamData kData)
    {
        m_kTeamData.Add(kColor, kData);
    }

    public void CalcScore(ref int iRedScore, ref int iBlueScore, bool bSkip = true)
    {
        int iRedFight = m_kScene.RedTeam.TeamInfo.FightScore;
        int iBlueFight = m_kScene.BlueTeam.TeamInfo.FightScore;

        int iRealWorldTime = (int)GlobalBattleInfo.Instance.GetRealWorldTime() / 60;       // 分钟
        float fTimeParam = (90 - iRealWorldTime) / 90.0f;
        if (false == bSkip)
            fTimeParam = iRealWorldTime;
        int iRed_Base_Score = (int)FIFARandom.GetRandomValue(0, 3);
        int iBlue_Base_Score = (int)FIFARandom.GetRandomValue(0, 3);

        int iMinFight = Math.Min(iRedFight, iBlueFight);
        int iRed_Extra = Math.Min((iRedFight - iMinFight) * 10 / iMinFight, 5);
        int iBlue_Extra = Math.Min((iBlueFight - iMinFight) * 10 / iMinFight, 5);

        iRedScore = (int)((iRed_Base_Score + iRed_Extra) * fTimeParam) + m_kScene.RedTeam.TeamInfo.Score;
        iBlueScore = (int)((iBlue_Base_Score + iBlue_Extra) * fTimeParam) + m_kScene.BlueTeam.TeamInfo.Score;

        int iRandScore = (int)(FIFARandom.GetRandomValue(0, 2) * fTimeParam);
        int iRandVal = (int)FIFARandom.GetRandomValue(0, 100);
        if (iRandVal > 50)
        {
            iRedScore += iRandScore;
        }
        else {
            iBlueScore += iRandScore;
        }

    }

    public void SkipCelebrateState()
    {
        if (null != m_kScene)
        {
            m_kScene.SetState(EGameState.GS_MIDKICK);
        }
    }
    public void SkipPreBattleUI()
    {
        if (null == m_kScene)
            return;
        if (EGameState.GS_INIT != m_kScene.GameState)
            return;
        GameStateInit kGameState = m_kScene.CurState as GameStateInit;
        kGameState.SkipPreBattleUI = true;
    }

    public bool EnableCalcTime
    {
        get { return m_bEnableCalcTime; }
        set { m_bEnableCalcTime = value; }
    }

    public bool WaitForFirstHalfEnd
    {
        get { return m_bWaitForFirstHalfEnd; }
        set { m_bWaitForFirstHalfEnd = value; }
    }

    public int LevelID
    {
        get { return m_iLevelID; }
        set { m_iLevelID = value; }
    }

    private bool m_bIsPlaying = false;
    private LLScene m_kScene = null;

    
    private Dictionary<ETeamColor, TeamData> m_kTeamData = new Dictionary<ETeamColor, TeamData>();
    private float m_fRemainingTime = 0;// 比赛剩余时间
    private float m_fTotalTime = 0;
    private EState m_kState = EState.Invalid;
    private float m_fHalfTime = GlobalBattleInfo.Instance.MaxTimeMatch / 2; // 秒
    private EState m_kPauseState = EState.Invalid;
    private bool m_bEnableCalcTime = false;
    private bool m_bEndGame = false;
    private bool m_bWaitForFirstHalfEnd = false;
    private int m_iLevelID = 1;
}
