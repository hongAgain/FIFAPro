using Common;
using Common.Tables;
using System.Collections.Generic;


public class LLScene
{
    public LLScene(string strName)
    {
        m_strSceneName = strName;
        InitGameStateList();
        InitRegions();
        CreateSceneMessage kMsg = new CreateSceneMessage(m_strSceneName, this);
        MessageDispatcher.Instance.SendMessage(kMsg);
        m_fCtrlRateInterval = GlobalBattleInfo.Instance.ConverToGameTime(GlobalBattleInfo.Instance.RealWorldCtrlRateInterval);
        BattleStatistics.Instance.Reset(this);
    }

    public void InitScene(TeamData kRedData, TeamData kBlueData)
    {
        CreateBall();
        switch(m_kHomeTeamColor)
        {
            case ETeamColor.Team_Red:
                CreateTeam(ETeamColor.Team_Red, ETeamState.TS_ATTACK, kRedData);
                CreateTeam(ETeamColor.Team_Blue, ETeamState.TS_DEFEND, kBlueData);
                break;
            case ETeamColor.Team_Blue:
                CreateTeam(ETeamColor.Team_Red, ETeamState.TS_DEFEND, kRedData);
                CreateTeam(ETeamColor.Team_Blue, ETeamState.TS_ATTACK, kBlueData);
                break;
        }
        m_kRedTeam.Opponent = m_kBlueTeam;
        m_kBlueTeam.Opponent = m_kRedTeam;
        SetState(EGameState.GS_INIT);
    }

    public void Pause()
    {
        if (m_kCurState.State == EGameState.GS_PAUSE)
            return;

        m_kPauseState = m_kCurState;
        m_kCurState = m_kGameStateList[EGameState.GS_PAUSE];
        PauseBattleMessage kMsg = new PauseBattleMessage();
        MessageDispatcher.Instance.SendMessage(kMsg);
    }

    public void Resume()
    {
        if (m_kCurState.State != EGameState.GS_PAUSE)
            return;
        m_kCurState = m_kPauseState;
        ResumeBattleMessage kMsg = new ResumeBattleMessage();
        MessageDispatcher.Instance.SendMessage(kMsg);
    }

    public void CreateTeam(ETeamColor kType, ETeamState kState, TeamData kData)
    {
        LLTeam kTeam = new LLTeam(kType, kState);
        kTeam.TeamInfo.ForamtionID = kData.FormationID;
        kTeam.TeamInfo.TeamName = kData.TeamName;
        kTeam.TeamInfo.FightScore = kData.FightingScore;
        kTeam.TeamInfo.ClubID = kData.ClubID;
        CreateTeamMessage kMsg = new CreateTeamMessage(kTeam);
        MessageDispatcher.Instance.SendMessage(kMsg);
        switch (kType)
        {
            case ETeamColor.Team_Blue:
                m_kBlueTeam = kTeam;
                break;
            case ETeamColor.Team_Red:
                m_kRedTeam = kTeam;
                break;
        }
        
        kTeam.Scene = this;
        
        kTeam.CreatePlayer(kData.PlayerList);
        kTeam.InitBaseHomeposition();
    }

    public void CreateBall()
    {
        if (null != m_kBall)
        {
            m_kBall.Destroy();
            m_kBall = null;
        }
        m_kBall = new LLBall(this);
        m_kBall.SetPosition(Vector3D.zero);
    }

    public void Update(float fTime)
    {
        ChangeTeamState();
        UpdateCtrlRate(fTime);
        if (null != m_kNextState)
        {
            m_kPreState = m_kCurState;
            m_kCurState = m_kNextState;
            m_kNextState = null;
            if (null != m_kPreState)
                m_kPreState.OnExit();
            if (null != m_kCurState)
                m_kCurState.OnEnter();
        }
        if (null != m_kCurState)
            m_kCurState.OnUpdate(fTime);
    }

    private void UpdateCtrlRate(float fTime)
    {
        m_fCtrlRateElapseTime += fTime;
        if(m_fCtrlRateElapseTime < m_fCtrlRateInterval)
        {
            LLTeam kTeam = null;
            if (ETeamState.TS_ATTACK == m_kRedTeam.State)
                kTeam = m_kRedTeam;
            else
                kTeam = m_kBlueTeam;

            kTeam.TeamInfo.CtrlBallTime += fTime;
        }
        else
        {
            m_fCtrlRateElapseTime = 0;
            // 更新控球率
            CtrlBallRateMessage kMsg = new CtrlBallRateMessage(m_kRedTeam.TeamInfo.CtrlBallTime, m_kBlueTeam.TeamInfo.CtrlBallTime);
            MessageDispatcher.Instance.PostMessage(kMsg);
            //m_kRedTeam.TeamInfo.CtrlBallTime = 0;
            //m_kBlueTeam.TeamInfo.CtrlBallTime = 0;
        }
    }

	//两队攻防转换
    private void ChangeTeamState()
    {
        if (false == m_bRequireChangeState)
            return;
        m_bRequireChangeState = false;
        if (true == m_bChangeTeamStateForce)
        {
            ETeamState kRedState = m_kRedTeam.State;
            ETeamState kBlueState = m_kBlueTeam.State;
            m_kRedTeam.ChangeState(kBlueState, m_kTeamStateChangeType);
            m_kBlueTeam.ChangeState(kRedState, m_kTeamStateChangeType);
        }
        else
        {
            if (false == m_kRedTeam.ReadyForChangeState ||
                false == m_kBlueTeam.ReadyForChangeState)
            {
                m_kRedTeam.ReadyForChangeState = false;
                m_kBlueTeam.ReadyForChangeState = false;
                return;
            }
            ETeamState kRedState = m_kRedTeam.State;
            ETeamState kBlueState = m_kBlueTeam.State;
            m_kRedTeam.ChangeState(kBlueState, m_kTeamStateChangeType);
            m_kBlueTeam.ChangeState(kRedState, m_kTeamStateChangeType);
        }
        m_kRedTeam.ReadyForChangeState = false;
        m_kBlueTeam.ReadyForChangeState = false;
        m_kTeamStateChangeType = ETeamStateChangeType.TSCT_MAXCOUNT;
        m_bChangeTeamStateForce = false;
        m_kBall.CanMove = true;
    }
    public void ChangeTeamFormation(int iFormationID, ETeamColor kTeamType)
    {
        LLTeam kTeam = ETeamColor.Team_Red == kTeamType ? m_kRedTeam : m_kBlueTeam;
        if (null == kTeam)
            return;
        kTeam.TeamInfo.ForamtionID = iFormationID;
        kTeam.ResetToMidKickPos();
    }

    public int GetRegionID(Vector3D kPos)
    {
        if (false == m_kRegion.CheckInRegion(kPos))
            return 0;

        BattleInfoItem kItem = TableManager.Instance.BattleInfoTable.GetItem("GroundLength");
        double fLength = kItem.Value / 2;
        kItem = TableManager.Instance.BattleInfoTable.GetItem("GroundWidth");
        double fWidth = kItem.Value / 2;
        m_kRegion = new Region(fLength, -fWidth, -fLength, fWidth);

        m_fRadius = fLength / 4;
        double fGridWidth = fWidth * 2 / 5;
        int iX = (int)((kPos.X + fWidth) / fGridWidth) + 1;
        int iZ = (int)((fLength - kPos.Z) / m_fRadius) * 5;
        ////简单粗爆的方法，需要优化
        //for (int i = 0; i < m_kSubRegionList.Count; i++)
        //{
        //    if (m_kSubRegionList[i].CheckInRegion(kPos))
        //        return (int)m_kSubRegionList[i].ID;
        //}
        int iRegionID = iZ + iX;
        return iRegionID;
    }

    public void SetState(EGameState kState)
    {
        if (false == m_kGameStateList.ContainsKey(kState))
            return;
        m_kNextState = m_kGameStateList[kState];
    }

    public void Destroy()
    {
        // 删除场景
        DestroySceneMessage kMsg = new DestroySceneMessage(m_strSceneName);
        MessageDispatcher.Instance.SendMessage(kMsg);
        // 删除球队
        if (null != m_kRedTeam)
            m_kRedTeam.Destroy();
        if (null != m_kBlueTeam)
            m_kBlueTeam.Destroy();
        m_kRedTeam = null;
        m_kBlueTeam = null;
        // 删除球
        DestroyBallMessage kDestroyBall = new DestroyBallMessage(m_kBall);
        MessageDispatcher.Instance.SendMessage(kDestroyBall);
        // Reset Battle Comment
        BattleCommentMgr.Instance.Reset();
    }

    public void RequireTeamStateChange(ETeamStateChangeType kType, bool bForce)
    {
        m_bRequireChangeState = true;
        m_bChangeTeamStateForce = bForce;
        m_kTeamStateChangeType = kType;
    }

    private void InitRegions()
    {
        BattleInfoItem kItem = TableManager.Instance.BattleInfoTable.GetItem("GroundLength");
        double fLength = kItem.Value / 2;
        kItem = TableManager.Instance.BattleInfoTable.GetItem("GroundWidth");
        double fWidth = kItem.Value / 2;
        m_kRegion = new Region(fLength, -fWidth, -fLength, fWidth);

        m_fRadius = fLength / 4;
        double fGridWidth = fWidth * 2 / 5;
        //创建子Region
        uint uiRegionID = 1;
        for (int ix = 0; ix < 8; ix++)
        {
            for (int iy = 0; iy < 5; iy++)
            {
                Region kRegion = new Region(fLength - ix * m_fRadius, -fWidth + iy * fGridWidth, fLength - (ix + 1) * m_fRadius, -fWidth + (iy + 1) * fGridWidth, uiRegionID);
                m_kSubRegionList.Add(kRegion);
                uiRegionID++;
            }
        }
    }

    private void InitGameStateList()
    {
        m_kGameStateList.Add(EGameState.GS_INIT, new GameStateInit(this));
        m_kGameStateList.Add(EGameState.GS_PAUSE, new GameStatePause(this));
        m_kGameStateList.Add(EGameState.GS_HALFEND, new GameStateHalfEnd(this));
        m_kGameStateList.Add(EGameState.GS_SHOOT, new GameStateShoot(this));
        m_kGameStateList.Add(EGameState.GS_MIDKICK, new GameStateMidKick(this));
        m_kGameStateList.Add(EGameState.GS_NORMAL, new GameStateNormal(this));
        m_kGameStateList.Add(EGameState.GS_FIX_PASS, new GameStateFixPass(this));
        m_kGameStateList.Add(EGameState.GS_CELEBRATION, new GameStateCelebration(this));
    }

    private void UpdateBallCtrlRate()
    {

    }

    public void RefreshOtherHomePosition()
    {
        if (GameState != EGameState.GS_NORMAL)
            return;
        BlueTeam.RefreshTeamHomePosition();
        RedTeam.RefreshTeamHomePosition();
    }

    public LLPlayer GetBallControler()
    {
        if (BlueTeam.State == ETeamState.TS_ATTACK)
            return BlueTeam.BallController;
        else
            return RedTeam.BallController;
    }
    public string SceneName
    {
        get { return m_strSceneName; }

    }

    public LLTeam RedTeam
    {
        get { return m_kRedTeam; }
    }

    public LLTeam BlueTeam
    {
        get { return m_kBlueTeam; }
    }

    public double Radius
    {
        get { return m_fRadius; }
    }

    public LLBall Ball
    {
        get { return m_kBall; }
        set { m_kBall = value; }
    }

    public Region Region
    {
        get { return m_kRegion; }
    }

    public List<Region> SubRegionList
    {
        get { return m_kSubRegionList; }
    }

    public EGameState GameState
    {
        get { return m_kCurState.State; }
        private set {; }
    }

    public IGameState CurState
    {
        get { return m_kCurState; }
    }

    public ETeamColor HomeTeamColor
    {
        get { return m_kHomeTeamColor; }
        set { m_kHomeTeamColor = value; }
    }

    private LLTeam m_kRedTeam = null;
    private LLTeam m_kBlueTeam = null;
    private string m_strSceneName;
    private Region m_kRegion;
    private double m_fRadius = 0.0f;
    private LLBall m_kBall = null;
    private List<Region> m_kSubRegionList = new List<Region>();

    private IGameState m_kCurState = null;
    private IGameState m_kPreState = null;
    private IGameState m_kNextState = null;
    private IGameState m_kPauseState = null;
    private Dictionary<EGameState, IGameState> m_kGameStateList = new Dictionary<EGameState, IGameState>();
    private bool m_bRequireChangeState = false;
    private bool m_bChangeTeamStateForce = false;
    private ETeamStateChangeType m_kTeamStateChangeType = ETeamStateChangeType.TSCT_MAXCOUNT;
    private float m_fCtrlRateElapseTime = 0;
    private float m_fCtrlRateInterval = 0;
    private ETeamColor m_kHomeTeamColor = ETeamColor.Team_Red;
}