using Common;
using Common.Tables;
using Common.Log;
using UnityEngine;
using System.Collections.Generic;
using System;

public struct ReportData
{
    public string text;
    public Color color;
}

public class UIBattle : UIBaseWindowLua
{
    public UISprite toggleMenu;
    public Transform m_sprArrow;
    public GameObject speedBtn;
    public GameObject skipBtn;
    public GameObject pauseBtn;
    public GameObject quitBtn;
    public GameObject continueBtn;
    public GameObject pauseRoot;
    public GameObject runningRoot;
    public UILabel m_lblHomeScore;
    public UILabel m_lblAwayScore;
    public UILabel m_lblHomeName;
    public UILabel m_lblAwayName;
    public UILabel m_lblTime;
    public UISliderPower m_sPower;
    public GameObject[] m_btnModeType;
    public UIBattleAttackChoice m_kAttackChoice = null;


    public static UIBattle Instance
    {
        get { return m_instance; }
    }

    public bool Is2DModel
    {
        get { return m_is2DModel; }
    }

    public Queue<ReportData> ReportQ
    {
        get { return m_reportQ; }
    }

    private static UIBattle m_instance = null; 
    private bool m_is2DModel;
    private Queue<ReportData> m_reportQ = new Queue<ReportData>();
    private float m_lastUpdateTime;
    private GameObject m_match2DRoot;
    private GameObject m_match2DCourt;
    private UILabel m_lblReport;
    private UISprite m_sprReportBg;
    enum EModeType
    {
        Btn3D,
        Btn2D,
        BtnAid,
        Max
    }
    private EModeType m_currModeType = EModeType.Btn3D;
    void Awake()
    {
        m_instance = this;
    }
    protected override void Start()
    {
        m_kCtrlRootTrans = transform.FindChild("CommonControls");
        m_kSkillRootTrans = transform.FindChild("SkillControls");
        m_match2DRoot = HUDRoot.Go.transform.FindChild("Match2D").gameObject;
        m_match2DCourt = m_kCtrlRootTrans.FindChild("Court").gameObject;
        m_lblReport = m_kCtrlRootTrans.FindChild("RunningRoot/Report").GetComponent<UILabel>();
        m_sprReportBg = m_kCtrlRootTrans.FindChild("RunningRoot/Report/Sprite").GetComponent<UISprite>();
        speedBtn = m_kCtrlRootTrans.FindChild("RunningRoot/BtnSpeed").GetComponent<UISprite>().gameObject;
        m_kCelebrateUI = m_kSkillRootTrans.FindChild("Celebrate");
        OnDestroyEvent = OnDestroyUI;

        Set2DMatch(false);

        BindUI();
        InitUIData();
    }

    void BindUI()
    {
        pauseRoot = m_kCtrlRootTrans.Find("PauseRoot").gameObject;
        pauseRoot.SetActive(false);
        runningRoot = m_kCtrlRootTrans.Find("RunningRoot").gameObject;
        runningRoot.SetActive(true);

        UIEventListener.Get(toggleMenu.gameObject).onClick += delegate(GameObject go)
        {
            toggleMenu.flip = toggleMenu.flip == UIBasicSprite.Flip.Both
                ? UIBasicSprite.Flip.Nothing
                : UIBasicSprite.Flip.Both;
            m_kAttackChoice.Show();
        };

        UIEventListener.Get(m_btnModeType[2]).onClick += delegate(GameObject objGame_)
        {
            LogManager.Instance.GreenLog("BtnAid");
            m_currModeType = EModeType.BtnAid;
            UpdateBtnColor();
        };

        UIEventListener.Get(m_btnModeType[1]).onClick += delegate(GameObject objGame_)
        {
            //LogManager.Instance.GreenLog("Btn2D");
            m_currModeType = EModeType.Btn2D;
            UpdateBtnColor();
            Set2DMatch(true);
        };

        UIEventListener.Get(m_btnModeType[0]).onClick += delegate(GameObject objGame_)
        {
            m_currModeType = EModeType.Btn3D;
            UpdateBtnColor();
            Set2DMatch(false);
        };

        UIEventListener.Get(skipBtn).onClick += delegate(GameObject go)
        {
            LLDirector.Instance.SkipBattle();
        };

        UIEventListener.Get(speedBtn).onClick += delegate (GameObject go)
        {
            LLDirector.Instance.SpeedUp();
        };

        UIEventListener.Get(pauseBtn).onClick += delegate(GameObject go)
        {
            LLDirector.Instance.Pause();
            pauseRoot.SetActive(true);
            runningRoot.SetActive(false);
        };

        UIEventListener.Get(quitBtn).onClick += delegate(GameObject go)
        {
            LLDirector.Instance.ExitBattle();
        };

        UIEventListener.Get(continueBtn).onClick += delegate(GameObject go)
        {
            LLDirector.Instance.Resume();

            pauseRoot.SetActive(false);
            runningRoot.SetActive(true);
        };

        SetSliderPower(0.5f);
        UpdateBtnColor();
    }

    public void InitUIData()
    {
        m_lblHomeName.text = LLDirector.Instance.Scene.RedTeam.TeamInfo.TeamName;
        m_lblAwayName.text = LLDirector.Instance.Scene.BlueTeam.TeamInfo.TeamName;
    }

    void UpdateBtnColor()
    {
        for (int i = 0; i < (int)EModeType.Max; ++i)
        {
            if ((int)m_currModeType == i)
            {
                m_btnModeType[i].GetComponent<UISprite>().color = new Color32(255, 255, 255, 255);
                m_sprArrow.localPosition = new Vector3(m_btnModeType[i].transform.localPosition.x, m_sprArrow.localPosition.y, 0);
            } 
            else
                m_btnModeType[i].GetComponent<UISprite>().color = new Color32(140, 140, 140, 255);
        }
    }

    void SetSliderPower(float value_)
    {
        m_sPower.CoeffValue = value_;
    }

    void Set2DMatch(bool is2D_)
    {
        m_is2DModel = is2D_;
        m_match2DRoot.SetActive(is2D_);
        m_match2DCourt.SetActive(is2D_);
    }

    // Update is called once per frame
    void Update()
    {
        m_lblHomeScore.text = LLDirector.Instance.Scene.RedTeam.TeamInfo.Score.ToString();
        m_lblAwayScore.text = LLDirector.Instance.Scene.BlueTeam.TeamInfo.Score.ToString();
        float fRealWorldTime = GlobalBattleInfo.Instance.GetRealWorldTime();
        m_lblTime.text = string.Format("{0:D2}:{1:D2}",
                                        Mathf.FloorToInt(fRealWorldTime / 60),
                                        Mathf.FloorToInt(fRealWorldTime% 60)
                                        );
        if (Time.time - m_lastUpdateTime > 1)
        {
            m_lastUpdateTime = Time.time;
            OnUpdateReport();
        }
    }

    public void OnUpdateReport()
    {
        if (m_reportQ.Count > 0)
        {
            m_lblReport.text = m_reportQ.Peek().text;
            m_sprReportBg.color = m_reportQ.Dequeue().color;
        }
    }

    public void OnCtrlBallRate(float redTime_,float blueTime_)
    {
        if ((redTime_ + blueTime_) != 0)
            SetSliderPower(redTime_ / (redTime_ + blueTime_));
        
    }

    /// <summary>
    /// 处理数据
    /// </summary>
    /// <param name="spr_"></param>
    /// <param name="value_"></param>
    public void EnqueueReportData(LLUnit kUnit_, BattleTextItem btItem_)
    {
        string strReport = btItem_.Text;

        if (kUnit_ != null)
        {
            if (btItem_.TextType == EBattleTextType.PlayerName)
            {
                strReport = string.Format(strReport, kUnit_.PlayerBaseInfo.HeroName);
            }
            else if (btItem_.TextType == EBattleTextType.TeamName)
            {
                strReport = string.Format(strReport, kUnit_.Team.TeamInfo.TeamName);
            }
        }

        ReportData rData = new ReportData();
        rData.text = strReport;
        //if (true) // Goals
        {
            rData.color = new Color(1, 1, 1, 1);
        }
        //else if (btMsg_.Unit.Team.State == ETeamState.TS_ATTACK && btMsg_.Unit.Team.TeamColor == ETeamColor.Team_Blue)
        //{
        //    rData.color = new Color32(0, 1, 0, 1);
        //} 
        //else
        //{
        //    rData.color = new Color(1, 1, 0, 1);
        //}

        m_reportQ.Enqueue(rData);
    }

    public static void SetColorSP(UISprite spr_, float value_)
    {
        if (value_ > 0.666f)
            spr_.color = new Color(20.0f / 255.0f, 230.0f / 255.0f, 20.0f / 255.0f, 1);
        else if (value_ > 0.333f)
            spr_.color = new Color(1, 140.0f / 255.0f, 20.0f / 255.0f, 1);
        else
            spr_.color = new Color(228.0f / 255.0f, 40.0f / 255.0f, 40.0f / 255.0f, 1);
    }

    public static void Attach2DPlayer(UISprite spr_, int type_)
    {
        switch (type_)
        {
            case 1:
                spr_.spriteName = "War_2d_player_b";
                break;
            case 2:
                spr_.spriteName = "War_2d_player_b_Jk";
                break;
            case 3:
                spr_.spriteName = "War_2d_player_O";
                break;
            case 4:
                spr_.spriteName = "War_2d_player_O_Jk";
                break;
            case 5:
                spr_.spriteName = "War_2d_ball";
                break;
            default:
                break;
        }

    }

    void OnDestroyUI()
    {
        LogManager.Instance.GreenLog("UIBattle...OnDestroyUI");
    }
   
    public void ShowCelebrateUI(bool bFlag,LLUnit kUnit)
    {
        if(null == m_kCelebrateUI)
            m_kCelebrateUI = m_kSkillRootTrans.FindChild("Celebrate");
        if (null == m_kCelebrateUI)
            return;
        m_kCelebrateUI.gameObject.SetActive(bFlag);
        if (true == bFlag && null != kUnit)
        {
            if (null == m_kHeroName)
                m_kHeroName = m_kCelebrateUI.FindChild("BG/PlayerName");
            UIHelper.SetLabelTxt(m_kHeroName, kUnit.PlayerBaseInfo.HeroName);

            if (null == m_kGoalNum)
                m_kGoalNum = m_kCelebrateUI.FindChild("BG/GoalNumDesc/Num");

            UIHelper.SetLabelTxt(m_kGoalNum, kUnit.PlayerBaseInfo.Score.ToString());

            UIEventListener.Get(m_kCelebrateUI.gameObject).onClick += delegate (GameObject go)
            {
                LLDirector.Instance.SkipCelebrateState();
            };

        }
    }

    public void OnDestroyPreBattleUI()
    {
        gameObject.SetActive(true);
        UnityEngine.GameObject.Destroy(m_kPreBattleUI);
        m_kPreBattleUI = null;
    }
    public void OnShowPerBattleUI(LLTeam kReadTeam,LLTeam kBlueTeam)
    {
        if (null == m_kPreBattleUI)
            m_kPreBattleUI = GameObject.Instantiate(GetPrefab("BeforeBattle")) as GameObject;
        if (null == m_kPreBattleUI)
            return;
        gameObject.SetActive(false);

        UIEventListener.Get(m_kPreBattleUI.gameObject).onClick += delegate (GameObject go)
        {
            LLDirector.Instance.SkipPreBattleUI();
        };
        m_kPreBattleUI.transform.parent = Client.Instance.UIAttach.transform;
        m_kPreBattleUI.transform.localPosition = Vector3.zero;
        m_kPreBattleUI.transform.localScale = Vector3.one;
        Transform kReadInfo = m_kPreBattleUI.transform.FindChild("BG_Top/RedTeam");
        Transform kBlueInfo = m_kPreBattleUI.transform.FindChild("BG_Top/BlueTeam");
        Transform kReadUnit = m_kPreBattleUI.transform.FindChild("BG_Bottom/RedTeam/Grid");
        Transform kBlueUnit = m_kPreBattleUI.transform.FindChild("BG_Bottom/BlueTeam/Grid");
        InitTeamInfo(kReadTeam, kReadInfo, kReadUnit);
        InitTeamInfo(kBlueTeam, kBlueInfo, kBlueUnit);
    }

    private void InitTeamInfo(LLTeam kTeam,Transform kTeamInfo,Transform kTeamUnit)
    {
        if (null == kTeam || null == kTeamInfo || null == kTeamUnit)
            return;
        Transform kNameTs = kTeamInfo.FindChild("Name");
        UIHelper.SetLabelTxt(kNameTs, kTeam.TeamInfo.TeamName);
        Transform kLvTs = kTeamInfo.FindChild("LV");
        UIHelper.SetLabelTxt(kLvTs, string.Format("LV.{0}", kTeam.TeamInfo.TeamLv));
        Transform kIconTs = kTeamInfo.FindChild("Icon");
        UITexture kTexture = kIconTs.GetComponent<UITexture>();
        kTexture.mainTexture = ResourceManager.Instance.Load(string.Format("Textures/ScatteredImg/ClubIcon/{0}", kTeam.TeamInfo.ClubID)) as Texture2D;
        Transform kIDTs, kCareerTs;
        
        for (int i = 0;i <kTeam.PlayerList.Count;i++)
        {
            LLUnit kUnit = kTeam.PlayerList[i];
            Transform kItem = kTeamUnit.GetChild(i);
            kIDTs = kItem.FindChild("ID");
            kNameTs = kItem.FindChild("Name");
            kCareerTs = kItem.FindChild("Career");
            UIHelper.SetLabelTxt(kIDTs, (i+1).ToString());
            UIHelper.SetLabelTxt(kNameTs, kUnit.PlayerBaseInfo.HeroName);
            UIHelper.SetLabelTxt(kCareerTs, kUnit.PlayerBaseInfo.CareerName.ToString());
        }
        Transform kLastItem = kTeamUnit.GetChild(kTeamUnit.childCount - 1);
        kNameTs = kLastItem.FindChild("Name");
        kIDTs = kLastItem.FindChild("ID");
        kCareerTs = kLastItem.FindChild("Career");
        UIHelper.SetLabelTxt(kIDTs, "11");
        UIHelper.SetLabelTxt(kNameTs, kTeam.GoalKeeper.PlayerBaseInfo.HeroName);
        UIHelper.SetLabelTxt(kCareerTs, kTeam.GoalKeeper.PlayerBaseInfo.CareerName.ToString());
    }

    public void OnPlayGoalCeleBration(GoalCelebrationData kData,LLUnit kUnit)
    {
        UnityEngine.GameObject _obj = null;
        if (kData != null)
        {
            _obj = ResourceManager.Instance.Load("Battle/" + kData.m_resName, true) as UnityEngine.GameObject;
        }
        GoalCeleBrationFx _fx = null;
        if (_obj != null)
            _fx = _obj.GetComponent<GoalCeleBrationFx>();
        if (_fx == null)
            _fx = _obj.AddComponent<GoalCeleBrationFx>();
        _obj.transform.localPosition = MathUtil.ConverToVector3(kData.m_parentPosition);
        _obj.transform.localEulerAngles = MathUtil.ConverToVector3(Vector3D.zero);
        m_kGoalCeleBration = _fx;
        _obj.name = kData.m_id.ToString();
        List<LLPlayer> _kLPlayer = new List<LLPlayer>();
        LLPlayer _p = kUnit as LLPlayer;
        for (int i = 0; i < kUnit.Team.PlayerList.Count; i++)
        {
            if (_kLPlayer.Count < kData.m_playerCount)
            {
                if (_kLPlayer.Count == 0)
                {
                    _kLPlayer.Add(kUnit as LLPlayer);
                }
                else
                {
                    if (kUnit.Team.PlayerList[i] != _p && Math.Abs(kUnit.PlayerBaseInfo.PosID - kUnit.PlayerBaseInfo.PosID) <= 2)
                    {
                        _kLPlayer.Add(kUnit.Team.PlayerList[i]);
                    }
                }
            }


        }
        _fx.Init(kUnit.Team, _kLPlayer, kData);
    }

    public void OnDestoryGoalCeleBration()
    {
        if (null == m_kGoalCeleBration)
            return;
        m_kGoalCeleBration.DestoryGoal();
        GameObject.Destroy(m_kGoalCeleBration.gameObject);
        m_kGoalCeleBration = null;
    }

    public Transform CtrlRoot
    {
        get { return m_kCtrlRootTrans; }
    }
    public Transform SkillRoot
    {
        get { return m_kSkillRootTrans; }
    }

    private Transform m_kCtrlRootTrans = null;
    private Transform m_kSkillRootTrans = null;
    private Transform m_kCelebrateUI = null;
    private Transform m_kHeroName = null;
    private Transform m_kGoalNum = null;
    private GameObject m_kPreBattleUI = null;
    private GameObject m_kSkipCelebrate = null;
    private GoalCeleBrationFx m_kGoalCeleBration = null;
}