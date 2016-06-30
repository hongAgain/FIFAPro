
using UnityEngine;
using System.Collections;
using Common;

//导演类，用于管理整个游戏比赛场景

public class DebugGUI : MonoBehaviour
{
    public static DebugGUI Instance = null;
    public void Awake()
    {
        Instance = this;
    }
    public void Start()
    {
    }

    public void Update()
    {
        if (null == m_kScene)
        {
            m_kScene = LLDirector.Instance.Scene;
       //     m_currentAttackTeam = m_kScene.RedTeam.State == ETeamState.TS_ATTACK? m_kScene.RedTeam:m_kScene.BlueTeam;
        }
        RefreshData();
    }
    void OnGUI()
    {
        GUILayout.BeginArea(new Rect(10 , 10, Screen.width/2 - 20, Screen.height / 4));
        GUILayout.BeginVertical();
        if(true == m_bPause)
        {
            if (GUILayout.Button("继续"))
            {
                LLDirector.Instance.Resume();
                m_bPause = false;
            }
        }
        else
        {
            if (GUILayout.Button("暂停"))
            {
                LLDirector.Instance.Pause();
                m_bPause = true;
            }
        }

        //if(GUILayout.Button("开启相机特效"))
        //{
        //    CameraFxBeginMessage kMsg = new CameraFxBeginMessage(1,m_kScene.RedTeam.PlayerList[0]);
        //    foreach(var kItem in m_kScene.RedTeam.PlayerList)
        //    {
        //        kMsg.AddUnit(kItem);
        //    }
        //    MessageDispatcher.Instance.SendMessage(kMsg);
        //}
        //if (GUILayout.Button("关闭相机特效"))
        //{
        //    CameraFxEndMessage kMsg = new CameraFxEndMessage();
        //    MessageDispatcher.Instance.SendMessage(kMsg);
        //}
        m_bShowDebugUI = GUILayout.Toggle(m_bShowDebugUI, "是否显示调试界面");
        m_bDribbleInfo = GUILayout.Toggle(m_bDribbleInfo, "八向带球信息");
        m_bCtrlBallStyle = GUILayout.Toggle(m_bCtrlBallStyle, "战术风格之控球篇");
        m_bSelectPlayer = GUILayout.Toggle(m_bSelectPlayer, "选择球员详情信息"); 
        //m_bShowGuardLine = GUILayout.Toggle(m_bShowGuardLine, "是否显示基准线信息");
        //GUILayout.BeginHorizontal();
        //m_bShowTeamInfo = GUILayout.Toggle(m_bShowTeamInfo, "显示所有球队信息");
        //m_bShowAttackInfo = GUILayout.Toggle(m_bShowAttackInfo, "显示进攻方信息");
        //m_bShowDefendInfo = GUILayout.Toggle(m_bShowDefendInfo, "显示防守方信息");
        //GUILayout.EndHorizontal();
        GUILayout.EndVertical();
        GUILayout.EndArea();
        
        if(m_bDribbleInfo)
        {
            if (null == m_kDriibleGUI)
                m_kDriibleGUI = gameObject.AddComponent<DribbleGUI>();
        }
        else
        {
            if (null != m_kDriibleGUI)
                GameObject.Destroy(m_kDriibleGUI);
            m_kDriibleGUI = null;
        }

        if (m_bCtrlBallStyle)
        {
            if (null == m_kCtrlBallStyleGUI)
                m_kCtrlBallStyleGUI = gameObject.AddComponent<CtrlBallStyleGUI>();
        }
        else
        {
            if (null != m_kCtrlBallStyleGUI)
                GameObject.Destroy(m_kCtrlBallStyleGUI);
            m_kCtrlBallStyleGUI = null;
        }
        if(m_bSelectPlayer)
        {
            if (m_kSPlayer == null)
            {
                m_kSPlayer = gameObject.AddComponent<SelectPlayerGUI>();
            }
            else
                m_kSPlayer.enabled = true;
        }
        else
        {
             if(m_kSPlayer!=null)
             {
                 m_kSPlayer.enabled = false;
             }
        }
        if (m_bShowDebugUI)
        {
         //   OnDrawDegugOption();
            //DrawCtrlBallStyle();
            //DrawPassBallStyle();
            //DrawAttackStyle();
            //DrawAttackTendence();
            //DrawDefenceTendency();
            //DrawShapeAdjust();
            //DrawVelocityOption();
            
        }
    }

    private void OnDrawDegugOption()
    {
        Vector2 kBoxPos = new Vector2(10, Screen.height - 140);
        Vector2 kBoxSize = new Vector2(200, 130);
        GUI.Box(new Rect(kBoxPos.x, kBoxPos.y, kBoxSize.x, kBoxSize.y), "调试选项开关");
        GUIStyle kStyle = new GUIStyle();
        Vector2 kSize = kStyle.CalcSize(new GUIContent("调试选项开关"));
        float fLeftBase = kBoxPos.x + 10;
        float fTopPos = kBoxPos.y +  kSize.y * 1.5f;
        GUILayout.BeginArea(new Rect(fLeftBase, fTopPos, kBoxSize.x - 20, kBoxSize.y - kSize.y * 1.5f));
        GUILayout.BeginVertical();
        GUILayout.BeginHorizontal();
        if(GUILayout.Button("抢断概率应用"))
        {
            OnApplyIntecept();
        }
        m_strSnatchPr = GUILayout.TextField(m_strSnatchPr);
        GUILayout.EndHorizontal();
        //GUILayout.BeginHorizontal();
        //if (GUILayout.Button("重新开始"))
        //    Restart();
        //if (GUILayout.Button(m_strPause))
        //    Pause();
        //GUILayout.EndHorizontal();
        //if (GUILayout.Button("攻防转换"))
        //    ChangeState();
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("更换阵型"))
            ChangeTeamStyle();
        m_strTeamID = GUILayout.TextField(m_strTeamID);
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();        // 球队选择
        GUILayout.Label("球队选择");
        bool bActive = m_kTeamColor == ETeamColor.Team_Red ? true:false;
        bActive = GUILayout.Toggle(bActive, "红队");
        if (bActive)
        {
            m_kTeamColor = ETeamColor.Team_Red;
        }
        bActive = m_kTeamColor == ETeamColor.Team_Red ? false : true;
        bActive = GUILayout.Toggle(bActive, "蓝队");
        if (bActive)
        {
            m_kTeamColor = ETeamColor.Team_Blue;
        }
        GUILayout.EndHorizontal();          
        GUILayout.EndVertical();
        GUILayout.EndArea();
    }


    private void DrawCtrlBallStyle()
    {
        // 控球风格
        //Vector2 kBoxPos = new Vector2(220, Screen.height - 140);
        //Vector2 kBoxSize = new Vector2(120, 130);
        //GUI.Box(new Rect(kBoxPos.x, kBoxPos.y, kBoxSize.x, kBoxSize.y), "控球风格");
        //GUIStyle kStyle = new GUIStyle();
        //Vector2 kSize = kStyle.CalcSize(new GUIContent("控球风格"));
        //float fLeftBase = kBoxPos.x + 10;
        //float fTopPos = kBoxPos.y + kSize.y * 1.5f;;

        //GUILayout.BeginArea(new Rect(fLeftBase, fTopPos, kBoxSize.x, kBoxSize.y));
        //GUILayout.BeginVertical();
        //bool bActive = m_kCtrlBallStyle == ECtrlBallStyle.CBS_ATTACK ? true:false;
        //bActive = GUILayout.Toggle(bActive, "偏进攻");
        //if(bActive)
        //{
        //    m_kCtrlBallStyle = ECtrlBallStyle.CBS_ATTACK;
        //}
        //bActive = m_kCtrlBallStyle == ECtrlBallStyle.CBS_BALANCE ? true : false;
        //bActive = GUILayout.Toggle(bActive, "偏平衡");
        //if (bActive)
        //{
        //    m_kCtrlBallStyle = ECtrlBallStyle.CBS_BALANCE;
        //}
        //bActive = m_kCtrlBallStyle == ECtrlBallStyle.CBS_DEFENCE ? true : false;
        //bActive = GUILayout.Toggle(bActive, "偏防守");
        //if (bActive)
        //{
        //    m_kCtrlBallStyle = ECtrlBallStyle.CBS_DEFENCE;
        //}
        //if (null != m_currentAttackTeam)
        //    m_currentAttackTeam.CtrlBallStyle = m_kCtrlBallStyle;
        //GUILayout.EndVertical();
        //GUILayout.EndArea();
    }
    private void DrawVelocityOption()
    {
        // 全局参数
        Vector2 kBoxPos = new Vector2(610, Screen.height - 140);
        Vector2 kBoxSize = new Vector2(250, 130);
        GUI.Box(new Rect(kBoxPos.x, kBoxPos.y, kBoxSize.x, kBoxSize.y), "全局参数");
        GUIStyle kStyle = new GUIStyle();
        Vector2 kSize = kStyle.CalcSize(new GUIContent("全局参数"));
        float fLeftBase = kBoxPos.x + 10;
        float fTopPos = kBoxPos.y + kSize.y * 1.5f;

        GUILayout.BeginArea(new Rect(fLeftBase, fTopPos, kBoxSize.x - 20, kBoxSize.y - 20));
        GUILayout.BeginVertical();
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("球员速度"))
        {
            if (null != m_currentAttackTeam)
                m_currentAttackTeam.ChangePlayerVelocity(System.Convert.ToDouble(m_strBallVelocity));
        }
        m_strPlayerVelocity = GUILayout.TextField(m_strPlayerVelocity);
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("足球速度"))
        {
            m_kScene.Ball.Velocity = System.Convert.ToDouble(m_strBallVelocity);
        }
        m_strBallVelocity = GUILayout.TextField(m_strBallVelocity);
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("球的加速度"))
        {
            m_kScene.Ball.Delta = System.Convert.ToDouble(m_strBallDelta);
        }
        m_strBallDelta = GUILayout.TextField(m_strBallDelta);
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
        GUILayout.EndArea();
    }

    void Restart()
    {
        if (LLDirector.Instance.IsPlaying)
            LLDirector.Instance.Stop();
        LLDirector.Instance.Restart("Scene/Court01_night", System.Convert.ToUInt32(m_strTeamID));
        m_kScene = null;
        m_currentAttackTeam = null;
    }

    void Pause()
    {
        if ("暂停" == m_strPause)
            m_strPause = "继续";
        else
            m_strPause = "暂停";
        if (LLDirector.Instance.IsPlaying)
            LLDirector.Instance.Pause();

    }

    void ChangeState()
    {
        if (LLDirector.Instance.IsPlaying)
            LLDirector.Instance.ChangeState();
    }

    void ChangeTeamStyle()
    {
        if (LLDirector.Instance.IsPlaying)
            LLDirector.Instance.ChangeTeamFormation(int.Parse(m_strTeamID), m_kTeamColor);
    }
    void OnApplyIntecept()
    {
        GlobalBattleInfo.Instance.SnatchPr = float.Parse(m_strSnatchPr);
    }

    void RefreshData()
    {
        if (null == m_currentAttackTeam || null == m_kScene)
            return;
    }

    private void ChangeTeam()
    {
        RefreshData();
    }

    public bool ShowTeamInfo
    {
        get { return m_bShowTeamInfo; }
        
    }

    public bool ShowAttackTeamInfo
    {
        get { return m_bShowAttackInfo; }
        
    }
    public bool ShowDefendTeamInfo
    {
        get { return m_bShowDefendInfo; }
        
    }

    public bool ShowGuardLine
    {
        get { return m_bShowGuardLine; }
        set { m_bShowGuardLine = value; }
    }

    private string m_strPause = "暂停";
    private string m_strTeamID = "1";
    private ETeamColor m_kTeamColor = ETeamColor.Team_End;
    private EAttackStyle m_kAttackStyle = EAttackStyle.AS_MIDDLE;
    private bool m_bShowDebugUI = true;
    private LLTeam m_currentAttackTeam = null;
    private LLScene m_kScene = null;
    private bool m_bShowTeamInfo = false;
    private bool m_bShowAttackInfo = false;
    private bool m_bShowDefendInfo = false;
    private bool m_bShowGuardLine = false;
    private string m_strPlayerVelocity = "2.66";
    private string m_strBallVelocity = "5";
    private string m_strBallDelta = "-4.68";
    private string m_strBallDist = "2";
    private string m_strSnatchPr = "0.5";
    private bool m_bPause = false; // 游戏暂停
    


    // 八向带球信息
    private bool m_bDribbleInfo = false;  // 显示带球信息
    private DribbleGUI m_kDriibleGUI = null;

    // 战术选择界面
    private bool m_bCtrlBallStyle = false;
    private CtrlBallStyleGUI m_kCtrlBallStyleGUI = null;

    //选择球员信息//
    private bool m_bSelectPlayer = false;
    private SelectPlayerGUI m_kSPlayer = null;
}
