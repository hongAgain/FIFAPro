using UnityEngine;
using System.Collections;
using Common;
using Common.Log;
using Common.Tables;
using AnimationOrTween;



//导演类，用于管理整个游戏比赛场景

public class CharView : MonoBehaviour
{
    public void Awake()
    {
    }

    public void Start()
    {
        StartCoroutine(InitClient());
    }

    public void OnGUI()
    {
        GUIContent kGUIContext = new GUIContent();
        kGUIContext.text = "应用";
        if(GUI.Button(new Rect(10,10,100,20), kGUIContext))
        {
            OnApply();
        }
        kGUIContext = new GUIContent();
        kGUIContext.text = "重置";
        if (GUI.Button(new Rect(10, 30, 100, 20), kGUIContext))
        {
            OnReset();
        }

        kGUIContext = new GUIContent();
        kGUIContext.text = "重置进度条动画";
        if (GUI.Button(new Rect(10, 60, 100, 30), kGUIContext))
        {
            OnProgressBar();
        }
    }

    protected void OnProgressBar()
    {
        if (null == m_kBarAni)
            return;
        m_kBarAni.Play("Goal");
        //    ActiveAnimation.Play(m_kBarAni, "Skill_in", Direction.Forward);
        //if (m_bFlag)
        //{
        //    m_bFlag = true;
        //   // m_kBarAni.Play("Skill_in");
        //    ActiveAnimation.Play(m_kBarAni, "Skill_in", Direction.Forward);
        //}
        //else
        //{
        //    m_bFlag = false;
        //    ActiveAnimation.Play(m_kBarAni, "Skill_in", Direction.Reverse);
        //}

    }

    protected void OnReset()
    {
        m_kPlayer.Player.SetPosition(new Vector3D(PlayerPos.x, PlayerPos.y, PlayerPos.z));
        m_kBall.SetPosition(new Vector3D(BallPos.x, BallPos.y, BallPos.z));
        LLPlayer _kPlayer = m_kPlayer.Player as LLPlayer;
        _kPlayer.SetRoteAngle(0);
    }
    protected void OnApply()
    {
        if (null == m_kPlayer)
            return;
        m_kPlayer.Player.KAniData.targetPos = new Vector3D(Position.x, Position.y, Position.z);
        m_kPlayer.Player.SetAniState(AniState);
		if (m_kPlayer.Player.AniStateMgr.PreState != null) {
			m_PreAnistate = m_kPlayer.Player.AniStateMgr.PreState.State;
		}
        //m_kPlayer.Player.RotateAngle = RotAngle;
        Vector3D kTargetPos = MathUtil.GetDirFormAngle(RotAngle) * Distance;
        if (false == GoalKeeper)
        {
            LLPlayer kPlayer = m_kPlayer.Player as LLPlayer;
            kPlayer.TargetPos = kPlayer.GetPosition()+ kTargetPos;
        }

        m_kBall.SetTarget(m_kBall.GetPosition() + kTargetPos);
    }

    public void Update()
    {
        if (null != m_kLuaMgr)
            m_kLuaMgr.Update();
       if (null != m_kPlayer && null != m_kPlayer.Player)
       {
           m_kPlayer.Player.Update(Time.deltaTime);
           m_kBall.Update(Time.deltaTime);
           if (false == GoalKeeper)
           {
               LLPlayer kPlayer = m_kPlayer.Player as LLPlayer;
               // 球员的朝向是向着目标点的
               Vector3D kDir = MathUtil.GetDir(kPlayer.GetPosition(), kPlayer.TargetPos);
               double fDist = kPlayer.GetPosition().Distance(kPlayer.TargetPos);
               double ds = kPlayer.Velocity * Time.deltaTime;
             
           }
           else
           {

           }
       }
    }

    void OnDestroy()
    {
        if (null != m_kLuaMgr)
            m_kLuaMgr.Destroy();
    }
    

    protected void InitComponent()
    {
        gameObject.AddComponent<ResourceManager>();
        gameObject.AddComponent<AvatarManager>();
        m_kDispatcher = MessageDispatcher.Instance;
        m_kMsgProc = new MessageProcessor(MessageDispatcher.Instance);
        m_kLuaMgr = new LuaScriptMgr();
        m_kLuaMgr.Start();

    }
    private IEnumerator InitClient()
    {
        LogManager.Instance.Log("Initialize start...");
        InitComponent();
        LogManager.Instance.Log("Initialize Lua Module ...");

        IEnumerator it = TableManager.Instance.InitTables();
        while (it.MoveNext())
        {
            yield return it.Current;
        }
        LogManager.Instance.Log("Initialize Table Module ...");
        CreatePlayer();
        yield return null;
        LogManager.Instance.Log("Initialize end...");
        yield return null;

        m_kPlayer = GameObject.Find("50101").gameObject.GetComponent<PLPlayer>();
        GameObject kBarObj = GameObject.Find("UI Root/UIBattle");
        if (null != kBarObj)
            m_kBarAni = kBarObj.GetComponentInChildren<Animation>();
        yield return null;
    }

    private void CreatePlayer()
    {
        LLScene kScene = new LLScene("Scene/Court01_night");
        LLTeam kTeam = new LLTeam(ETeamColor.Team_Red, ETeamState.TS_ATTACK);
        kTeam.TeamInfo.ForamtionID = 1;
        kTeam.TeamInfo.TeamName = "ABC";
        kTeam.Scene = kScene;
        CreateTeamMessage kMsg = new CreateTeamMessage(kTeam);
        m_kDispatcher.SendMessage(kMsg);
        PlayerInfo kInfo = new PlayerInfo();
        kInfo.HeroID = 50101;
        kInfo.ClubID = 48001;
        kInfo.Career = ECareer.ForwardFielder;
        LLPlayer kPlayer = new LLPlayer(kTeam, kInfo);
        kPlayer.AIEnable = false;
        m_kBall = new LLBall(kScene);
        kScene.Ball = m_kBall;
        m_kBall.SetPosition(Vector3D.zero);

    }


    private bool m_bFlag = false;
    private Animation m_kBarAni = null;
    public EAniState AniState = EAniState.Idle;
    public Vector3 Position = new Vector3(0,0,10);  //球员的状态的目标点，涉及转向等//
    public Vector3 PlayerPos = new Vector3();       //球员的初始点//
    public Vector3 BallPos = new Vector3();         //球的位置//
    public float RotAngle = 0;
    public float Distance = 5;
    public bool GoalKeeper = false;
    private LLBall m_kBall = null; 
    private PLPlayer m_kPlayer = null;
    private LuaScriptMgr m_kLuaMgr = null;
    private MessageProcessor m_kMsgProc = null;
    public EAniState m_PreAnistate = EAniState.Idle;
    private MessageDispatcher m_kDispatcher = null;
}
