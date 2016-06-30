using System;
using Common;
using System.Collections.Generic;
using BehaviourTree;
using Common.Log;
using Common.Tables;
using System.Diagnostics;


// 带球概率信息，包含各种情况下的数据
public class DribblePrData
{
    public void Clear()
    {
        m_CareerScore.Clear();
        m_Density.Clear();
        m_Tactics.Clear();
        m_kScore.Clear();
    }

    int m_Career;   // 职业
    int m_iRegionID;    // 当前格子
    int m_iHeroID;  // 球员ID
    List<double> m_CareerScore = new List<double>();    // 职业
    List<double> m_Density = new List<double>();        // 球员密度
    List<double> m_Tactics = new List<double>();        // 战术
    List<double> m_kScore = new List<double>();         // 总得分

    public int HeroID
    {
        get { return m_iHeroID; }
        set { m_iHeroID = value; }
    }

    public int Career
    {
        get { return m_Career; }
        set { m_Career = value; }
    }

    public List<double> Density
    {
        get { return m_Density; }
        set { m_Density = value; }
    }

    public List<double> Tactics
    {
        get { return m_Tactics; }
        set { m_Tactics = value; }
    }

    public List<double> Score
    {
        get { return m_kScore; }
        set { m_kScore = value; }
    }

    public List<double> CareerScore
    {
        get { return m_CareerScore; }
        set { m_CareerScore = value; }
    }

    public int RegionID
    {
        get { return m_iRegionID; }
        set { m_iRegionID = value; }
    }
};

// 射门时发生的相关数据
public class ShootData
{
    public LLPlayer Player
    {
        get { return m_kPlayer; }
        set { m_kPlayer = value; }
    }

    public bool IsHeadShoot
    {
        get { return m_bIsHeadShoot; }
        set { m_bIsHeadShoot = value; }
    }

    public bool FarShoot
    {
        get { return m_bFarShoot; }
        set { m_bFarShoot = value; }
    }

    public bool InSideGoal
    {
        get { return m_bInSideGoal; }
        set { m_bInSideGoal = value; }
    }

    public Vector3D GoalPos
    {
        get { return m_kGoalPos; }
        set { m_kGoalPos = value; }
    }

    public double ShootSuccessPr
    {
        get { return m_dShootSuccessPr; }
        set { m_dShootSuccessPr = value; }
    }

    public bool NSValid
    {
        get { return m_bNSValid; }
        set { m_bNSValid = value; }
    }
    public bool Valid
    {
        get { return m_bValid; }
        set { m_bValid = value; }
    }
    public double GKSaveRandVal
    {
        get { return m_dGKSaveRandVal; }
        set { m_dGKSaveRandVal = value; }
    }

    private LLPlayer m_kPlayer = null;
    private bool m_bIsHeadShoot = false;
    private bool m_bFarShoot = false;
    private bool m_bInSideGoal = false;
    private double m_dShootSuccessPr = 0;           // 进球概率
    private Vector3D m_kGoalPos = Vector3D.zero; // 射门点
    private double m_dGKSaveRandVal = 0;
    private bool m_bNSValid = false;
    private bool m_bValid = false; //该数据的有效性//
}

public class TeamInfo
{
    public TeamInfo()
    {
        m_iTeamLv = 1;
        m_iScore = 0;
        m_iForamtionID = 1;
        m_strName = "";
    }
    public string TeamName
    {
        get { return m_strName; }
        set { m_strName = value; }
    }

    public int ForamtionID
    {
        get { return m_iForamtionID; }
        set { m_iForamtionID = value; }
    }

    public int Score
    {
        get { return m_iScore; }
        set { m_iScore = value; }
    }

    public float CtrlBallTime
    {
        get { return m_fCtrlBallTime; }
        set { m_fCtrlBallTime = value; }
    }

    public int FightScore
    {
        get { return m_iFightScore; }
        set { m_iFightScore = value; }
    }

    public int TeamLv
    {
        get { return m_iTeamLv; }
        set { m_iTeamLv = value; }
    }

    public int ClubID
    {
        get { return m_iClubID; }
        set { m_iClubID = value; }
    }

    private int m_iClubID;
    private int m_iFightScore;          // 战力
    private string m_strName;           // 球队名
    private int m_iForamtionID;         // 球队阵型
    private int m_iScore = 0;           // 球队得分
    private float m_fCtrlBallTime;      // 控球时间
    private int m_iTeamLv;              // 球队等级
}

public class Socore
{
    public double m_disScore;
    public double m_StageScore;
    public double m_posScore;
    public double m_passScore;
    public double m_getScore;
    public double m_DepthScore;
    public Socore(double _dis, double _et, double _dpS, double _pos, double _pass, double _get)
    {
        m_disScore = _dis;
        m_StageScore = _et;
        m_DepthScore = _dpS;
        m_getScore = _get;
        m_posScore = _pos;
        m_passScore = _pass;
    }
}
public class LLTeam
{
    // 进攻路线下球员选择方式 
    enum EPlayerSelType
    {
        PST_FORWARD = 0,
        PST_BACKWARD,
        PST_LEFT_X,
        PST_MIDDLE_X,
        PST_RIGHT_X,
        PST_NEAR_S,
        PST_FAR_S,
        PST_MIDDLE_S,
        PST_MAX
    }

    /*
     * 攻防双方共用函数
     */
    #region Common Function
    public LLTeam(ETeamColor kType, ETeamState kState)
    {
        m_kTeamColor = kType;
        m_kState = kState;

        int iID = m_kTeamAI.Database.GetDataID(BTConstant.Team);
        m_kTeamAI.Database.SetData<LLTeam>(iID, this);
        m_kTeamAI.Load("Tables/AI/TeamAI");
        m_kGoal = new LLGoal(this);
        InitEightDirs();
    }

    public void Destroy()
    {
        m_kPlayerList.Clear();
        DestroyTeamMessage kMsg = new DestroyTeamMessage(this);
        MessageDispatcher.Instance.SendMessage(kMsg);
    }

    public void Update(float fTime)
    {

        if (null == m_kTeamAI)
            return;
        UpdatePlayerRegionID();
        UpdateGuardLine();
        m_kTeamAI.Update(fTime);
    }

    public void ResetToMidKick()
    {
        if (null == m_kTeamAI)
            return;
        m_kTeamAI.Reset();
        //1.重置球员位置
        ResetToMidKickPos();
        Scene.Ball.SetPosition(Vector3D.zero);
        // 重要球员状态
        for (int i = 0; i < m_kPlayerList.Count; i++)
        {
            
            m_kPlayerList[i].KAniData.targetPos = Scene.Ball.GetPosition();
            m_kPlayerList[i].ResetAI();
            m_kPlayerList[i].ResetState();
            m_kPlayerList[i].ResetCatchBallEnergy();
            m_kPlayerList[i].SetState(EPlayerState.PS_WAITFORKICKOFF);
            m_kPlayerList[i].SetBallCtrl(false);
            PlayerList[i].SetRoteAngle(MathUtil.GetAngle(PlayerList[i].GetPosition(), Vector3D.zero));
        }
        m_kBallController = null;
    }
    public void TeamPlayerUpdate(float fTime)
    {
        for (int i = 0; i < m_kPlayerList.Count; i++)
        {
            m_kPlayerList[i].Update(fTime);
        }
        m_kGoalKeeper.Update(fTime);
    }
    public void InitBaseHomeposition()
    {
        BPositionLogic = new BattlePositionLogic();
        BPositionLogic.initBattlePlayerLogic(this);
    }
    public void CreatePlayer(List<PlayerInfo> kPlayerInfoList)
    {
        if (kPlayerInfoList.Count <= 0)
            return;
        for (int iIdx = 0; iIdx < kPlayerInfoList.Count; iIdx++)
        {
            if (ECareer.Goalkeeper == kPlayerInfoList[iIdx].Career)
                m_kGoalKeeper = new LLGoalKeeper(this, kPlayerInfoList[iIdx]);
            else
            {
                LLPlayer kPlayer = new LLPlayer(this, kPlayerInfoList[iIdx]);
                m_kPlayerList.Add(kPlayer);
            }
        }
    }

    public void RemovePlayer(LLPlayer kPlayer)
    {
        kPlayer.Destroy();
        m_kPlayerList.Remove(kPlayer);
    }

    public void ChangeTeamAI(String strFileName)
    {
        m_kTeamAI.Load(strFileName);
    }

    public void ChangeGameState(EGameState kState)
    {
        m_kScene.SetState(kState);
        switch (kState)
        {
            case EGameState.GS_FIX_PASS:
                if (null == m_kBallController)
                {
                    return;
                }
                m_kBallController.SetState(Common.EPlayerState.PS_KICKOFF);
                break;
        }
    }

    /*
     * 更新后卫线
     */
    public void UpdateGuardLine()
    {
        UpdateNewBasePosition(false);
    }

    private void SetMiddleKickOff(BattlePositionLogic.TeamBattleKeyData _data)
    {
        if (_data != null)
        {
            if (_data.m_kickOffIndex > 1)
                m_kPlayerList[_data.m_kickOffIndex].SetAniState(EAniState.Match_BeginKick);
        }
    }
    private void UpdateNewBasePosition(bool _MidlleKick)
    {
        StandType _stype = StandType.MidKick_Control;
        bool _middleKickSide = true;
        if (_MidlleKick)
        {
            _stype = StandType.MidKick_Control;
            if (m_kState == ETeamState.TS_DEFEND)
            {
                _stype = StandType.MidKick_NoControl;
                _middleKickSide = false;
            }
            BPositionLogic.RemoveBaseHomepositionDeltx();
        }
        else
        {
            _middleKickSide = false;
            _stype = StandType.BattleRun_Control;
            if (m_kState == ETeamState.TS_DEFEND)
            {
                _stype = StandType.BattleRun_NoControl;
            }
        }
        BattlePositionLogic.TeamBattleKeyData _posDatas = BPositionLogic.UpdateTeamBaseHomePosition(this, _stype, m_kScene.Ball.GetPosition());
        if (_MidlleKick)
        {
            //守门员位置
            BattlePositionLogic.PlayerPositionData _GppData = _posDatas.GetPlayerPositionByPosIndex(0, m_kGoalKeeper.PlayerBaseInfo.PosID);
            if (_GppData != null)
            {
                m_kGoalKeeper.SetAniState(EAniState.Match_ReadyIdle);
                m_kGoalKeeper.SetPosition(_GppData.m_playerPostion);
                m_kGoalKeeper.SetRoteAngle(MathUtil.GetAngle(m_kGoalKeeper.GetPosition(), m_kScene.Ball.GetPosition()));
            }
        }
        else
        {
            BattlePositionLogic.PlayerPositionData _GppData = _posDatas.GetPlayerPositionByPosIndex(0, m_kGoalKeeper.PlayerBaseInfo.PosID);
            if (_GppData != null)
            {
                m_kGoalKeeper.BaseHomeposition = _GppData.m_playerPostion;
                m_kGoalKeeper.UpdateHomePosition();
            }
        }
        //场上非守门员球员位置
        for (int i = 0; i < m_kPlayerList.Count; i++)
        {
            BattlePositionLogic.PlayerPositionData _ppData = _posDatas.GetPlayerPositionByPosIndex(i + 1, m_kPlayerList[i].PlayerBaseInfo.PosID);
            if (_ppData != null)
            {
                if (!_MidlleKick)
                {
                    m_kPlayerList[i].BaseHomeposition = _ppData.m_playerPostion + new Vector3D(0, 0, HomepositionParam); //战术AttackType影响的系数//
                    m_kPlayerList[i].UpdateHomePosition();
                }
                else
                {
                    m_kPlayerList[i].SetAniState(EAniState.Match_ReadyIdle);
                    m_kPlayerList[i].SetPosition(_ppData.m_playerPostion);
                    m_kPlayerList[i].SetRoteAngle(MathUtil.GetAngle(m_kPlayerList[i].GetPosition(), m_kScene.Ball.GetPosition()));

                }
            }
            else
            {
                LogManager.Instance.RedLog("Null--->_Posindex==" + m_kPlayerList[i].PlayerBaseInfo.PosID);
            }
        }
        if (_middleKickSide)
        {
            SetMiddleKickOff(_posDatas);
        }

    }

    public void UpdateAttackTactical(AttackType _aType,AttackChoice _choic,AttackDirection _diretion)
    {
        m_kAttackType = _aType;
        m_kAttackChoice = _choic;
        m_kAttackDir = _diretion;
        //更新homeposition系数//
        string _key = "";
        switch(m_kAttackType)
        {
            case AttackType.Attack_Define:
                _key = "strategy_balance";
                break;
            case AttackType.All_Attack:
                _key = "strategy_attack";
                break;
            case AttackType.ALL_Define:
                _key = "strategy_defense";
                break;
        }

        HomepositionParam = TableManager.Instance.AIConfig.GetItem(_key).Value;
    }
    /*
     * 改变球员速度
     */
    public void ChangePlayerVelocity(double dVelocity)
    {
        for (int i = 0; i < m_kPlayerList.Count; i++)
        {
            m_kPlayerList[i].Velocity = dVelocity;
        }
    }
    /*
     *  改变攻防状态
     *  需要处理的事件:原进攻,则失去控球权
     */
    public void ChangeState(ETeamState kState, ETeamStateChangeType kType)
    {
        if (kState == m_kState)
            return;


        m_kState = kState;
        // 取消所有的盯防状态
        //for (int i = 0; i < m_kPlayerList.Count; i++)
        //{
        //    m_kPlayerList[i].Opponent = null;
        //}
        switch (m_kState)
        {
            case ETeamState.TS_ATTACK:
                ChangeToAttack(kType);

                break;
            case ETeamState.TS_DEFEND:
                ChangeToDefend(kType);
                break;
        }
        m_kBallMarkList.Clear();
    }

    #region team state related functions
    private void ChangeToDefend(ETeamStateChangeType kType)
    {
        for (int i = 0; i < m_kPlayerList.Count; i++)
        {
            m_kPlayerList[i].ResetToDefendState();
        }

        if (null != m_kBallController)   // 失去控球权
        {
            m_kBallController.SetBallCtrl(false);
            //            if (ETeamStateChangeType.TSCT_INTERCEPT == kType)
            //            {
            ////                m_kBallController.ManMarkWithBall = false;
            ////                m_kBallController.AIEnable = true;
            //                //m_kBallController.IsNeedStopBall = false;
            ////                m_kBallController.SetState(EPlayerState.PS_INTERCEPTED_THINKING);
            //            }
            m_kBallController = null;
        }
        if (kType == ETeamStateChangeType.TSCT_INTERCEPT)
        {
            //arrange closeMark
            InformRefreshCloseMark();
        }
    }

    private void ChangeToAttack(ETeamStateChangeType kType)
    {
        // 设置控球员
        for (int i = 0; i < m_kPlayerList.Count; i++)
        {
            m_kPlayerList[i].ResetToAttackState();
            //            if (true == m_kPlayerList[i].IsCtrlBall)
            //            {
            //                m_kBallController = m_kPlayerList[i];
            ////                m_kBallController.SetBallCtrl(true);
            ////                m_kBallController.SetState(EPlayerState.Catch_GroundBall);
            //            }
            //            else
            //            {
            ////                m_kPlayerList[i].SetBallCtrl(false);
            //            }
        }
        if (kType == ETeamStateChangeType.TSCT_INTERCEPT)
        {
            //arrange closeMark
            InformRefreshAttackSupport();
        }

    }
    #endregion

    /// <summary>
    /// function to pass ball. 
    /// the moment you pass the ball, it is setControlled and owned by the "playerTo". 
    /// function does not focuse on players' state changes.
    /// </summary>
    /// <param name="playerFrom">Player from.</param>
    /// <param name="playerTo">Player to.</param>
    /// <param name="moveType">Move type.</param>
    public EBallMoveType PassBall(LLUnit playerFrom, LLPlayer playerTo, EBallMoveType moveType, bool needInformDefender = false, bool isFirstHighBall = true)
    {
        LLBall m_kBall = playerFrom.Team.Scene.Ball;

        //传球后,失去控球权
        playerFrom.SetBallCtrl(false);
        playerTo.SetBallCtrl(true);
        BallController = playerTo;

        Vector3D kDir = MathUtil.GetDir(playerFrom.GetPosition(), playerTo.GetPosition());
        kDir.Y = 0d;

        double realDistance = playerFrom.GetPosition().Distance(playerTo.GetPosition());
        EBallMoveType realMoveType = moveType;
//         realMoveType = EBallMoveType.HighLobPass; /////To Cepeheus/////
//         UnityEngine.Debug.Break();
        // calculate the dist
        double passRandomFactor = 0;
        double passRandomMinDist = 0;
        if (realMoveType == EBallMoveType.HighLobPass || realMoveType == EBallMoveType.Throughing)
        {
            passRandomFactor = TableManager.Instance.AIConfig.GetItem("Pass_high_random_percent").Value;
            passRandomMinDist = TableManager.Instance.AIConfig.GetItem("Pass_high_random_distance").Value;
        }
        else
        {
            passRandomFactor = TableManager.Instance.AIConfig.GetItem("Pass_random_percent").Value;
            passRandomMinDist = TableManager.Instance.AIConfig.GetItem("Pass_random_distance").Value;
        }

        double distOffset = FIFARandom.GetRandomValue(0, realDistance * passRandomFactor);
        double dLen = 0.391;
        if (distOffset < passRandomMinDist)
        {
            // 0.391是球与人的位移
            dLen = 0.391;
        }
        else
        {
            dLen = distOffset;
        }
        Vector3D targetBallPos = playerTo.GetPosition() - kDir * dLen;
        
        switch (realMoveType)
        {
            case EBallMoveType.GroundPass:
                playerTo.SetState(EPlayerState.Catch_GroundBall);
                if (needInformDefender)
                {
                    LLPlayer interceptDefender = Opponent.GetInterceptDefender(playerFrom, targetBallPos);
                    if (interceptDefender != null)
                    {
                        //change it for seeking a valid shortPassDefender
                        interceptDefender.MarkingStatus = EMarkStatus.NONE;
                    }

                    LLPlayer shortPassDefender = null;
                    for (int i = 0; i < Opponent.PlayerList.Count; i++)
                    {
                        if (Opponent.PlayerList[i].MarkingStatus == EMarkStatus.MARKWITHBALL &&
                           Opponent.PlayerList[i].Opponent == playerFrom)
                            shortPassDefender = Opponent.PlayerList[i];
                    }
                    //pass it
                    PassBallOut(playerTo, realMoveType, targetBallPos);
                    #if GAME_AI_ONLY
                    Scene.Ball.isGroundPass = true;
                    #endif
                    NS_Block.Caculate(playerFrom, interceptDefender);
                    InformRefreshAttackSupport();
                    Opponent.InformStartPassGroundBall(playerFrom, playerTo, interceptDefender, targetBallPos, NS_Block.InterceptPr);
                }
                break;            
            case EBallMoveType.HighLobPass:
            case EBallMoveType.HeadingToHighLob:
                if (needInformDefender)
                {
                    PassBallOut(playerTo, realMoveType, targetBallPos);
                    #if GAME_AI_ONLY
                    Scene.Ball.isHighLobPass = true;
                    #endif
                    //calculate defenders
                    LLPlayer passDefender = Opponent.GetPassHighLobBallDefender(playerFrom);
                    LLPlayer headingDefender = Opponent.GetHeadingDefender(playerTo.GetPosition() - kDir * dLen, isFirstHighBall);
                    //calculate pr
                    NS_LongPass.Caculate(playerFrom, passDefender, playerTo, headingDefender);
                    //arrange defense
                    if (NS_LongPass.Valid)
                    {
                        //pass it
                        InformRefreshAttackSupport();
                        Opponent.InformStartPassHighBall(playerFrom, playerTo, headingDefender, isFirstHighBall, NS_LongPass.HeadRobPr);
                    }
                    else
                    {
                        //pass it
                        //use random
                        InformRefreshAttackSupport();
                        Opponent.InformStartPassHighBall(playerFrom, playerTo, headingDefender, isFirstHighBall, 0.5d);
                    }
                }
                else
                {
                    //pass it
                    CheckHeadingTackle(playerTo, null, isFirstHighBall);
                }
                break;
            case EBallMoveType.Throughing:
                //not used
                PassBallOut(playerTo, realMoveType, targetBallPos);
                //cannot fail on start
                if (needInformDefender)
                {
                    LLPlayer passDefender = Opponent.GetHeadingHighLobDefender(playerFrom);
                    LLPlayer headingDefender = Opponent.GetHeadingDefender(playerTo.GetPosition() - kDir * dLen, isFirstHighBall);
                    //calculate pr
                    NS_LongPass.Caculate(playerFrom, passDefender, playerTo, headingDefender);
                    //arrange defense
                    if (NS_LongPass.Valid)
                    {
                        InformRefreshAttackSupport();
                        Opponent.InformStartPassHighBall(playerFrom, playerTo, headingDefender, isFirstHighBall, NS_LongPass.HeadRobPr);
                    }
                    else
                    {
                        //use random
                        InformRefreshAttackSupport();
                        Opponent.InformStartPassHighBall(playerFrom, playerTo, headingDefender, isFirstHighBall, 0.5d);
                    }
                }
                else
                {
                    CheckHeadingTackle(playerTo, null, isFirstHighBall);
                }
                break;
            default:
                LogManager.Instance.RedLog("PassBall realMoveType default");
                playerTo.SetState(EPlayerState.Catch_GroundBall);
                if (needInformDefender)
                {
                    LLPlayer interceptDefender = Opponent.GetInterceptDefender(playerFrom, targetBallPos);
                    if (interceptDefender != null)
                    {
                        //change it for seeking a valid shortPassDefender
                        interceptDefender.MarkingStatus = EMarkStatus.NONE;
                    }
                    
                    LLPlayer shortPassDefender = null;
                    for (int i = 0; i < Opponent.PlayerList.Count; i++)
                    {
                        if (Opponent.PlayerList[i].MarkingStatus == EMarkStatus.MARKWITHBALL &&
                            Opponent.PlayerList[i].Opponent == playerFrom)
                            shortPassDefender = Opponent.PlayerList[i];
                    }
                    //pass it
                    PassBallOut(playerTo, realMoveType, targetBallPos);
                    NS_Block.Caculate(playerFrom, interceptDefender);
                    InformRefreshAttackSupport();
                    Opponent.InformStartPassGroundBall(playerFrom, playerTo, interceptDefender, targetBallPos, NS_Block.InterceptPr);
                }
                break;
        }
        LogManager.Instance.RedLog("PassBall " + realMoveType + " =========== " + playerFrom.PlayerBaseInfo.HeroID + " -> " + playerTo.PlayerBaseInfo.HeroID);
        return realMoveType;
    }

    private void PassBallOut(LLPlayer playerTo, EBallMoveType realMoveType, Vector3D targetBallPos)
    {
        Scene.Ball.SetTarget(targetBallPos, realMoveType);
        playerTo.TargetPos = targetBallPos;
        Scene.Ball.CanMove = true;
        playerTo.DecreaseCatchBallEnergy();
    }

    /// <summary>
    /// force m_kBallController to LowDribble status from normalDribble
    /// </summary>
    public bool CheckForcedLowDribble(LLPlayer attacker)
    {
        if (attacker != null && attacker.IsCtrlBall)
        {
            if (attacker.State == EPlayerState.NormalDribble ||
               attacker.State == EPlayerState.LowDribble)
            {
                LogManager.Instance.GreenLog("attacker can ForcedLowDribble");
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// force m_kBallController to pass/shoot
    /// </summary>
    public void InformForcedBallOut()
    {
        if (m_kBallController != null && m_kBallController.IsCtrlBall)
        {
            //only happens in valid pre-State
            if (m_kBallController.State == EPlayerState.NormalDribble ||
               m_kBallController.State == EPlayerState.LowDribble)
            {
                LogManager.Instance.YellowLog("InformForcedBallOut ");
                if (m_kBallController.GetPosition().Distance(Opponent.Goal.GoalPos) < TableManager.Instance.AIConfig.GetItem("mark_event_force_shoot").Value)
                {
                    //force to shoot            
                    LogManager.Instance.YellowLog("InformForcedBallOut " + EPlayerState.Shoot);
                    m_kBallController.SetState(EPlayerState.Shoot);
                }
                else
                {
                    //force to pass
                    LogManager.Instance.YellowLog("InformForcedBallOut PlanToPass");
                    PlanToPass(m_kBallController);
                }
            }
        }
    }

    #region Pass Plan
    /// <summary>
    /// Only Used in ActionSelect And ForcedBallOut
    /// </summary>
    /// <param name="passer">Passer.</param>
    public void PlanToPass(LLPlayer passer,bool canFailFlag = true)
    {
        LogManager.Instance.ColorLog("#e500ffff","Start PlanToPass");
        //check long/short pass
        LLPlayer plannedPlayerToPass = passer.Team.SelectPlannedPlayerToPass();
        if (plannedPlayerToPass != null && plannedPlayerToPass != passer)
        {                
            if (passer.GetPosition().Distance(plannedPlayerToPass.GetPosition()) >= 
                TableManager.Instance.AIConfig.GetItem("long_distance_pass").Value)
            {
                //long pass
                passer.CastSkill(EEventType.ET_LongPassBall);
                passer.Team.NSCheckLongPass(()=>{SuccessOnBallOut(passer);});
            } 
            else
            {
                LLPlayer defender = passer.Team.GetCloseMarkDefender(passer);
                //short pass
                passer.CastSkill(EEventType.ET_ShortPassBall);
                if(canFailFlag)
                {
                    passer.Team.NSCheckShortPass(passer,
                                                 defender,
                                                 ()=>{FailOnBallOut(passer);},
                                                 ()=>{SuccessOnBallOut(passer);});
                }
                else
                {
                    passer.Team.NSCheckShortPass(passer,
                                                 defender,
                                                 ()=>{SuccessOnBallOut(passer);},
                                                 ()=>{SuccessOnBallOut(passer);});
                }
            }
        }
        else
        {
            //cannot pass
            FailOnPassPlan(passer);
        }
    }

    private void SuccessOnBallOut(LLPlayer attacker)
    {
        LogManager.Instance.ColorLog("#e500ffff","PlanToPass SuccessOnBallOut");
        attacker.SetState(EPlayerState.PassBall);
    }

    private void FailOnBallOut(LLPlayer attacker)
    {
        LogManager.Instance.ColorLog("#e500ffff", "PlanToPass FailOnBallOut Start");
        LLPlayer defender = attacker.Team.GetCloseMarkDefender(attacker);
        if (defender != null)
        {
            if (defender.Team.CheckInDefenceEventArea(defender, attacker))
            {
                LogManager.Instance.ColorLog("#e500ffff", "PlanToPass FailOnBallOut get sliding-tackled");
                //get sliding-tackled
                attacker.Opponent = defender;
                defender.Opponent = attacker;
                defender.MarkingStatus = EMarkStatus.MARKWITHBALL;
                defender.SetState(EPlayerState.Sliding_Tackle_Success);
                attacker.SetState(EPlayerState.Avoid_Sliding_Tackle_Failed);
            } 
            else
            {
                LogManager.Instance.ColorLog("#e500ffff", "PlanToPass PassBall");
                attacker.SetState(EPlayerState.PassBall);
            }
        }
    }

    private void FailOnPassPlan(LLPlayer attacker)
    {
        LogManager.Instance.ColorLog("#e500ffff","PlanToPass NormalDribble");
        attacker.SetState(EPlayerState.NormalDribble);
    }

    #region NS Checks For Passes
    public void NSCheckShortPass(LLUnit playerFrom, LLPlayer defender,
                                 System.Action onPassF,
                                 System.Action onPassS)
    {
        NS_ShortPass.Caculate(playerFrom, defender);
        if (NS_ShortPass.Valid)
        {
            double dVal = 0;
            if (NS_ShortPass.Valid)
                dVal = NS_ShortPass.RandVal;
            else
                dVal = FIFARandom.GetRandomValue(0, 1);
            if (dVal < NS_ShortPass.PassedPr)
            {
                //pass it
                if(onPassS!=null)
                    onPassS();
            }
            else
            {
                //passing failed
                if(onPassF!=null)
                    onPassF();
            }
        }
        else
        {
            LogManager.Instance.BlackLog("Calculation invalid, use random");
            //use random
            double dVal = FIFARandom.GetRandomValue(0, 1);
            if (dVal < 0.5d)
            {
                //pass it
                if(onPassS!=null)
                    onPassS();
            }
            else
            {
                //passing failed
                if(onPassF!=null)
                    onPassF();
            }
        }
    }
    
    public void NSCheckLongPass(System.Action onPassS)
    {
        //pass it
        if(onPassS!=null)
            onPassS();
    }
    #endregion
    #endregion

    public void InformDribbleRegionChanged(int formerRegionID, int latterRegionID)
    {
        //ball controller got into region latter, tweak strategy
        InformATeamRegionChanged(formerRegionID, latterRegionID);
        //inform opponent
        Opponent.InformDTeamRegionChanged(formerRegionID, latterRegionID);
    }

    public void InformATeamRegionChanged(int formerRegionID, int latterRegionID)
    {
        //check Attack-Support region change
        List<int> areaAttackSupportSoft = new List<int>(10) { 11, 12, 13, 14, 15, 16, 17, 18, 19, 20 };
        List<int> areaAttackSupportHard = new List<int>(4) { 1, 5, 6, 10 };
        List<int> areaAttackSupportDevil = new List<int>(10) { 2, 3, 4, 7, 8, 9 };
        if (areaAttackSupportSoft.Contains(latterRegionID) && !areaAttackSupportSoft.Contains(formerRegionID))
        {
            LogManager.Instance.BlackLog("InformATeamRegionChanged areaAttackSupportSoft");
            //got into areaAttackSupportSoft
            double supportNum = TableManager.Instance.AIConfig.GetItem("assist_attack_number1").Value;
            double supportRate = TableManager.Instance.AIConfig.GetItem("assist_attack_rate1").Value;
            double supportRadius = TableManager.Instance.AIConfig.GetItem("assist_attack_radius1").Value;
            ArrangeAttackSupport(supportNum, supportRate, supportRadius);
        }
        else if (areaAttackSupportHard.Contains(latterRegionID) && !areaAttackSupportHard.Contains(formerRegionID))
        {
            LogManager.Instance.BlackLog("InformATeamRegionChanged areaAttackSupportHard");
            //got into areaAttackSupportHard
            double supportNum = TableManager.Instance.AIConfig.GetItem("assist_attack_number2").Value;
            double supportRate = TableManager.Instance.AIConfig.GetItem("assist_attack_rate2").Value;
            double supportRadius = TableManager.Instance.AIConfig.GetItem("assist_attack_radius2").Value;
            ArrangeAttackSupport(supportNum, supportRate, supportRadius);
        }
        else if (areaAttackSupportDevil.Contains(latterRegionID) && !areaAttackSupportDevil.Contains(formerRegionID))
        {
            LogManager.Instance.BlackLog("InformATeamRegionChanged areaAttackSupportDevil");
            //got into areaAttackSupportDevil
            double supportNum = TableManager.Instance.AIConfig.GetItem("assist_attack_number3").Value;
            double supportRate = TableManager.Instance.AIConfig.GetItem("assist_attack_rate3").Value;
            double supportRadius = TableManager.Instance.AIConfig.GetItem("assist_attack_radius3").Value;
            ArrangeAttackSupport(supportNum, supportRate, supportRadius);
        }
    }

    public void InformDTeamRegionChanged(int formerRegionID, int latterRegionID)
    {
        List<GroundAreaItem> areas = new List<GroundAreaItem>(5){
            TableManager.Instance.GroundAreaTable.GetItem("markArea_WithBall_Hard"),
            TableManager.Instance.GroundAreaTable.GetItem("markArea_WithBall_Soft"),
            TableManager.Instance.GroundAreaTable.GetItem("markArea_WithoutBall_Soft"),
            TableManager.Instance.GroundAreaTable.GetItem("markArea_WithoutBall_Hard"),
            TableManager.Instance.GroundAreaTable.GetItem("markArea_WithoutBall_Devil")
        };
        for (int i = 0; i < areas.Count; i++)
        {
            if (areas[i].IsInside(formerRegionID) && areas[i].IsInside(latterRegionID))
                continue;
            else
            {
                LogManager.Instance.BlackLog("InformDTeamRegionChanged InformRefreshCloseMark");
                //marking area changed
                InformRefreshCloseMark();
                return;
            }
        }
    }

    public void ArrangeAttackSupport(double supportNum, double supportActivateRate, double supportDistance)
    {
        LogManager.Instance.BlackLog("ArrangeAttackSupport InformRefreshCloseMark");
        m_AttackSupportArrangement.Clear();

        List<LLPlayer> possibleSupporter = new List<LLPlayer>();
        for (int i = 0; i < PlayerList.Count; i++)
        {
            if (PlayerList[i] != BallController && PlayerList[i].NewEventAssignable())
            {
                possibleSupporter.Add(PlayerList[i]);
            }
        }

        List<Vector3D> possibleDirections = GetAttackSupportDirections(supportDistance);

        for (int i = 0; i < supportNum; i++)
        {
            if (possibleDirections.Count > 0)
            {
                if (FIFARandom.GetRandomValue(0, 1) < supportActivateRate)
                {
                    //arrange it
                    int posIndex = (int)FIFARandom.GetRandomValue(0, possibleDirections.Count - 0.5);
                    Vector3D targetPos = BallController.GetPosition() + possibleDirections[posIndex] * supportDistance;
                    LLPlayer targetSupporter = SortNearPlayers(targetPos, possibleSupporter)[0];
                    if (targetSupporter.State != EPlayerState.AttackSupport)
                    {
                        targetSupporter.TargetPos = possibleDirections[posIndex];
                        targetSupporter.SetState(EPlayerState.ToAttackSupport);
                    }

                    //save this player's offset
                    m_AttackSupportArrangement[targetSupporter] = possibleDirections[posIndex] * supportDistance;



                    LogManager.Instance.RedLog("ArrangeAttackSupport " + targetSupporter.PlayerBaseInfo.HeroID);



                    possibleDirections.RemoveAt(posIndex);
                    possibleSupporter.RemoveAt(posIndex);
                }
                else
                {
                    continue;
                }
            }
            else
                break;
        }
    }

    public bool UpdateAttackSupportPos(LLPlayer targetPlayer)
    {
        if (m_AttackSupportArrangement.ContainsKey(targetPlayer))
        {
            Vector3D targetPos = BallController.GetPosition() + m_AttackSupportArrangement[targetPlayer];
            if (IsPositionInSideCourt(targetPos))
            {
                targetPlayer.TargetPos = targetPos;
                return true;
            }
        }
        return false;
    }

    private List<Vector3D> GetAttackSupportDirections(double supportDistance)
    {
        List<Vector3D> possibleDirections = new List<Vector3D>();
        if (TeamColor == ETeamColor.Team_Blue)
        {
            if (BallController.GetPosition().X < 0)
            {
                //arrange right-forward first
                possibleDirections.Add(MathUtil.GetDirFormAngle(315));
                possibleDirections.Add(MathUtil.GetDirFormAngle(45));
                possibleDirections.Add(MathUtil.GetDirFormAngle(225));
                possibleDirections.Add(MathUtil.GetDirFormAngle(135));

            }
            else
            {
                //arrange left-forward first
                possibleDirections.Add(MathUtil.GetDirFormAngle(45));
                possibleDirections.Add(MathUtil.GetDirFormAngle(315));
                possibleDirections.Add(MathUtil.GetDirFormAngle(135));
                possibleDirections.Add(MathUtil.GetDirFormAngle(225));
            }
        }
        else
        {
            if (BallController.GetPosition().X < 0)
            {
                //arrange right-forward first
                possibleDirections.Add(MathUtil.GetDirFormAngle(225));
                possibleDirections.Add(MathUtil.GetDirFormAngle(135));
                possibleDirections.Add(MathUtil.GetDirFormAngle(315));
                possibleDirections.Add(MathUtil.GetDirFormAngle(45));

            }
            else
            {
                //arrange left-forward first
                possibleDirections.Add(MathUtil.GetDirFormAngle(135));
                possibleDirections.Add(MathUtil.GetDirFormAngle(225));
                possibleDirections.Add(MathUtil.GetDirFormAngle(45));
                possibleDirections.Add(MathUtil.GetDirFormAngle(315));
            }
        }

        List<Vector3D> possiblePositions = new List<Vector3D>();
        for (int i = 0; i < possibleDirections.Count; i++)
        {
            Vector3D targetPos = BallController.GetPosition() + possibleDirections[i] * supportDistance;
            if (IsPositionInSideCourt(targetPos))
                possiblePositions.Add(possibleDirections[i]);
        }
        return possiblePositions;
    }

    private bool IsPositionInSideCourt(Vector3D _pos)
    {
        double _width = Math.Abs(Scene.Region.Left);
        double _length = Math.Abs(Scene.Region.Right);
        if (Math.Abs(_pos.X) >= _width - 0.5d)
        {
            return false;
        }
        if (Math.Abs(_pos.Z) >= _length - 0.5d)
        {
            return false;
        }
        return true;
    }

    public void InformRefreshAttackSupport()
    {
        if (State == ETeamState.TS_ATTACK && BallController != null)
        {
            //check Attack-Support region change
            List<int> areaAttackSupportSoft = new List<int>(10) { 11, 12, 13, 14, 15, 16, 17, 18, 19, 20 };
            List<int> areaAttackSupportHard = new List<int>(4) { 1, 5, 6, 10 };
            List<int> areaAttackSupportDevil = new List<int>(10) { 2, 3, 4, 7, 8, 9 };
            if (areaAttackSupportSoft.Contains(BallController.RegionID))
            {
                //got into areaAttackSupportSoft
                double supportNum = TableManager.Instance.AIConfig.GetItem("assist_attack_number1").Value;
                double supportRate = TableManager.Instance.AIConfig.GetItem("assist_attack_rate1").Value;
                double supportRadius = TableManager.Instance.AIConfig.GetItem("assist_attack_radius1").Value;
                ArrangeAttackSupport(supportNum, supportRate, supportRadius);
            }
            else if (areaAttackSupportHard.Contains(BallController.RegionID))
            {
                //got into areaAttackSupportHard
                double supportNum = TableManager.Instance.AIConfig.GetItem("assist_attack_number2").Value;
                double supportRate = TableManager.Instance.AIConfig.GetItem("assist_attack_rate2").Value;
                double supportRadius = TableManager.Instance.AIConfig.GetItem("assist_attack_radius2").Value;
                ArrangeAttackSupport(supportNum, supportRate, supportRadius);
            }
            else if (areaAttackSupportDevil.Contains(BallController.RegionID))
            {
                //got into areaAttackSupportDevil
                double supportNum = TableManager.Instance.AIConfig.GetItem("assist_attack_number3").Value;
                double supportRate = TableManager.Instance.AIConfig.GetItem("assist_attack_rate3").Value;
                double supportRadius = TableManager.Instance.AIConfig.GetItem("assist_attack_radius3").Value;
                ArrangeAttackSupport(supportNum, supportRate, supportRadius);
            }
        }
    }

    public void InformRefreshCloseMark()
    {
        //check and refresh all players' close-marking status
        if (Opponent.State == ETeamState.TS_ATTACK && Opponent.BallController != null)
        {
            ArrangeCloseMark(Opponent.BallController);
        }
    }

    public void InformStartPassHighBall(LLUnit playerFrom, LLPlayer playerTo, LLPlayer headingDefender, bool attackerIsFirstHeading, double attackerWonPr)
    {
        LogManager.Instance.GreenLog("InformStartPassHighBall");
        DisableCloseMarkWithBall();
        ArrangeHeadingTackle(playerTo, headingDefender, attackerIsFirstHeading, attackerWonPr);
        ArrangeCloseMarkWithoutBall_AllCourt(playerTo, headingDefender);
    }

    public void InformStartPassGroundBall(LLUnit playerFrom, LLPlayer playerTo, LLPlayer interceptDefender, Vector3D targetBallPos, double interceptSuccessPr = 0.5d)
    {
        LogManager.Instance.GreenLog("InformStartPassGroundBall");
        DisableCloseMarkWithBall();
        ArrangeInterceptTackle(playerFrom, playerTo, interceptDefender, targetBallPos, interceptSuccessPr);
        ArrangeCloseMarkWithoutBall_AllCourt(playerTo, interceptDefender);
    }
    
    public void InformStartDribble(LLPlayer ballController)
    {
        LogManager.Instance.RedLog("InformStartDribble");
        Opponent.ArrangeCloseMark(ballController);
    }

    //return the player selected to do heading tackle
    private void ArrangeHeadingTackle(LLPlayer headingAttacker, LLPlayer headingDefender, bool attackerIsFirstHeading, double attackerWonPr)
    {
        if (headingDefender != null)
        {
            LogManager.Instance.YellowLog("Defender " + headingDefender.PlayerBaseInfo.HeroID + " go heading tackle");
            headingDefender.MarkingStatus = EMarkStatus.NONE;
        }
        CheckHeadingTackle(headingAttacker, headingDefender, attackerIsFirstHeading, attackerWonPr);
    }
    
    public void ArrangeInterceptTackle(LLUnit playerFrom, LLPlayer playerTo, LLPlayer interceptDefender, Vector3D targetBallPos, double interceptSuccessPr = 0.5d)
    {
        //the pos ball can get in half time
        Vector3D interceptPos = (3 * playerFrom.GetPosition() + 5 * targetBallPos) / 8d;
        interceptPos.Y = 0;
        if (interceptDefender != null)
        {
            LogManager.Instance.ColorLog("#0080ffff", "planned: Defender " + interceptDefender.PlayerBaseInfo.HeroID + " go intercept the ball");
            interceptDefender.TargetPos = interceptPos;
            interceptDefender.MarkingStatus = EMarkStatus.NONE;
            interceptDefender.Opponent = playerTo;
            interceptDefender.SetState(EPlayerState.Intercept);
            return;
        }
        LogManager.Instance.ColorLog("#0080ffff", "no planned: Defender to intercept");
    }

    public LLPlayer GetCloseMarkDefender(LLPlayer attacker)
    {
        if (attacker.Team.State == ETeamState.TS_ATTACK)
        {
            LLTeam opponentTeam = attacker.Team.Opponent;
            for (int i = 0; i < opponentTeam.PlayerList.Count; i++)
            {
                if (opponentTeam.PlayerList [i].MarkingStatus == EMarkStatus.MARKWITHBALL &&
                    opponentTeam.PlayerList [i].Opponent == attacker &&
                    opponentTeam.PlayerList[i].NewEventAssignable())
                    return opponentTeam.PlayerList [i];
            }
        }
        return null;
    }
    
    private int GetCloseMarkDefenderCount(LLPlayer _player)
    {
        List<LLPlayer> _plys = new List<LLPlayer>();
        int _count = 0;
        if (_player.Team.TeamColor == ETeamColor.Team_Blue)
            _plys = this.Scene.RedTeam.PlayerList;
        else
            _plys = this.Scene.BlueTeam.PlayerList;
        
        if (_player.Team.State == ETeamState.TS_ATTACK)
        {
            for (int i = 0; i < _plys.Count; i++)
            {
                if ((_plys [i].MarkingStatus == EMarkStatus.MARKWITHBALL ||
                     _plys [i].MarkingStatus == EMarkStatus.MARKWITHOUTBALL) && 
                    _plys [i].Opponent == _player)
                {
                    _count++;
                }
            }
        }
        return _count;
    }
    
    private LLPlayer GetInterceptDefender(LLUnit playerFrom,Vector3D targetBallPos)
    {
        double runRate = TableManager.Instance.AIConfig.GetItem("speed_rate_block").Value;
        double targetBallFlyingTime = Scene.Ball.PreCalculateFlyingTime(targetBallPos);
        //the pos ball can get in half time
        Vector3D interceptPos = (3 * playerFrom.GetPosition() + 5 * targetBallPos) / 8d;
        interceptPos.Y = 0;
        List<LLPlayer> defenderList = new List<LLPlayer>(PlayerList.Count);
        for (int i = 0; i < PlayerList.Count; i++)
        {
            if (PlayerList[i].NewEventAssignable())
                defenderList.Add(PlayerList[i]);
        }
        List<LLPlayer> NearestDefenderList = SortNearPlayers(interceptPos, defenderList);
        double nearestDistance = NearestDefenderList[0].GetPosition().Distance(interceptPos);
        //0.25d is the protection time for intercepting action
        if (nearestDistance / (NearestDefenderList[0].BaseVelocity * runRate) < targetBallFlyingTime / 2 - 0.2d)
        {
            LogManager.Instance.ColorLog("#0080ffff", "Defender " + NearestDefenderList[0].PlayerBaseInfo.HeroID + "can go intercept the ball by calculation");
            return NearestDefenderList[0];
        }
        //no defender go Intercept
        return null;
    }



    //defender's function
    private LLPlayer GetPassHighLobBallDefender(LLUnit passer)
    {
        if (passer.Team.State == ETeamState.TS_ATTACK)
        {
            List<LLPlayer> defenders = passer.Team.Opponent.PlayerList;
            for (int i = 0; i < defenders.Count; i++)
            {
                if (defenders [i].MarkingStatus == EMarkStatus.MARKWITHBALL &&
                    defenders [i].Opponent == passer &&
                    passer.GetPosition().Distance(defenders [i].GetPosition()) < 3 &&
                    defenders [i].NewEventAssignable())
                    return defenders [i];
            }
        }
        return null;
    }

    private LLPlayer GetHeadingHighLobDefender(LLUnit passer)
    {
        if (passer.Team.State == ETeamState.TS_ATTACK)
        {
            List<LLPlayer> defenders = passer.Team.Opponent.PlayerList;
            for (int i = 0; i < defenders.Count; i++)
            {
                if (defenders [i].Opponent == passer &&
                    defenders[i].State == EPlayerState.Heading_Tackle_Failed)
                    return defenders [i];
            }
        }
        return null;
    }

    //defender's function
    private LLPlayer GetHeadingDefender(Vector3D targetBallPos, bool attackerIsFirstHeading)
    {
        LLBall targetBall = Scene.Ball;
        double passBallTime = Scene.Ball.PreCalculateFlyingTime(targetBallPos);
        double runRate = TableManager.Instance.AIConfig.GetItem("speed_rate_run").Value;
        Vector3D headingPosA = targetBallPos - MathUtil.GetDir(targetBall.GetPosition(), targetBallPos) * 0.5d;
        Vector3D headingPosB = targetBallPos + MathUtil.GetDir(targetBallPos, Goal.GoalPos) * 0.5d;
        headingPosA.Y = 0;
        headingPosB.Y = 0;

        List<LLPlayer> defenderListA = new List<LLPlayer>(PlayerList.Count);
        List<LLPlayer> defenderListB = new List<LLPlayer>(PlayerList.Count);
        for (int i = 0; i < PlayerList.Count; i++)
        {
            if (PlayerList[i].NewEventAssignable())
            {
                defenderListA.Add(PlayerList[i]);
                defenderListB.Add(PlayerList[i]);
            }
        }
        List<LLPlayer> NearestDefenderListA = SortNearPlayers(headingPosA, defenderListA);
        List<LLPlayer> NearestDefenderListB = SortNearPlayers(headingPosB, defenderListB);
        double distanceA = NearestDefenderListA[0].GetPosition().Distance(headingPosA);
        double distanceB = NearestDefenderListB[0].GetPosition().Distance(headingPosB);

        if (distanceA < distanceB)
        {
            //Check A
            if (distanceA / NearestDefenderListA[0].BaseVelocity * runRate < passBallTime - 0.2d - 0.3d)
            {
                LogManager.Instance.YellowLog("planned: Defender " + NearestDefenderListA[0].PlayerBaseInfo.HeroID + " go heading tackle");
                return NearestDefenderListA[0];
            }
        }
        else
        {
            //Check B
            if (distanceB / NearestDefenderListB[0].BaseVelocity * runRate < passBallTime - 0.2d - 0.3d)
            {
                LogManager.Instance.YellowLog("planned: Defender " + NearestDefenderListB[0].PlayerBaseInfo.HeroID + " go heading tackle");
                return NearestDefenderListB[0];
            }
        }
        //no defender go heading tackle
        return null;
    }

    public void CheckAfterCatchGroundBall(LLPlayer targetPlayer)
    {
        //        double rate = FIFARandom.GetRandomValue(0, 1);
        //        if (rate < 0.5d)
        //        {
        //            // not stopping, do next state instantly
        //        LogManager.Instance.GreenLog("CheckAfterCatchGroundBall ActionSelect");
        targetPlayer.SetState(EPlayerState.ActionSelect);
        //        }
        //        else
        //        {
        //            // make a stop, decide next
        //            LogManager.Instance.GreenLog("CheckAfterCatchGroundBall Catch_GroundBall_ToIdle");
        //            targetPlayer.SetState(EPlayerState.Catch_GroundBall_ToIdle);
        //        } 
    }

    /// <summary>
    /// check whether forced break through event happens
    /// </summary>
    /// <returns><c>true</c>, if break through activated, <c>false</c> otherwise.</returns>
    public bool CheckForcedBreakThrough(LLPlayer attacker)
    {
        double rateToActivate = 0d;
        GroundAreaItem frontHalfArea = TableManager.Instance.GroundAreaTable.GetItem("frontHalf_Area");
        GroundAreaItem backHalfArea = TableManager.Instance.GroundAreaTable.GetItem("backHalf_Area");
        if (frontHalfArea.IsInside(attacker.RegionID))
        {
            rateToActivate = TableManager.Instance.AIConfig.GetItem("breakthrough__versus2").Value;
        }
        else if (backHalfArea.IsInside(attacker.RegionID))
        {
            rateToActivate = TableManager.Instance.AIConfig.GetItem("breakthrough__versus1").Value;
        }
        return (FIFARandom.GetRandomValue(0, 1) <= rateToActivate);
    }

    public void ActivateDefenceEvent(LLPlayer defender, EPlayerState defenderState, LLPlayer attacker, EPlayerState attackerState)
    {
        if (attacker != null && attacker.State != attackerState)
        {
            LogManager.Instance.GreenLog("attacker ForcedLowDribbled");
            attacker.SetState(attackerState);
        }
        //force rotate
        attacker.SetRoteAngle(MathUtil.GetAngle(attacker.GetPosition(), defender.GetPosition()));
        attacker.TargetPos = defender.GetPosition();

        //set defend break through
        defender.SetState(defenderState);
        //force rotate
        defender.SetRoteAngle(MathUtil.GetAngle(defender.GetPosition(), attacker.GetPosition()));
        defender.TargetPos = attacker.GetPosition();

        //set opponent
        defender.Opponent = attacker;
        attacker.Opponent = defender;
    }

    private void ArrangeCloseMark(LLPlayer ballController)
    {
        if (DisableCloseMarkWithBall())
        {
            //arrange again
            ArrangeCloseMarkWithBall(ballController);
        }
        ArrangeCloseMarkWithoutBall_AllCourt(ballController, null);



        int wbcount = 0;
        int withoutbcount = 0;
        for (int i = 0; i < PlayerList.Count; i++)
        {
            if (PlayerList[i].MarkingStatus == EMarkStatus.MARKWITHBALL)
                wbcount++;
            else if (PlayerList[i].MarkingStatus == EMarkStatus.MARKWITHOUTBALL)
                withoutbcount++;
        }
        LogManager.Instance.YellowLog("MarkWithBall: " + wbcount + " MarkWithoutBall: " + withoutbcount);
    }

    /// <summary>
    /// returns whether disable all is successful. MARKWITHOUTBALL is all disabled no matter what.
    /// </summary>
    /// <returns><c>true</c>, if disable all is successful, <c>false</c> otherwise.</returns>
    private bool DisableCloseMarkWithBall()
    {
        LogManager.Instance.BlackLog("DisableCloseMarkWithBall");
        bool disableAllFlag = true;
        for (int i = 0; i < PlayerList.Count; i++)
        {
            if (PlayerList[i].MarkingStatus == EMarkStatus.MARKWITHBALL && 
                (PlayerList[i].State == EPlayerState.Block_Tackle ||
                 PlayerList[i].State == EPlayerState.Sliding_Tackle ||
                 PlayerList[i].State == EPlayerState.Defend_Break_Through ||
                 PlayerList[i].State == EPlayerState.Block_Tackle_Success ||
                 PlayerList[i].State == EPlayerState.Sliding_Tackle_Success ||
                 PlayerList[i].State == EPlayerState.Defend_Break_Through_Success))
            {
                //cannot change under those states                    
                LogManager.Instance.YellowLog("Disable failed: " + PlayerList[i].State);
                disableAllFlag = false;
                continue;
            }
            if (PlayerList[i].NewEventAssignable())
            {
                //can be changed, change it
                PlayerList[i].MarkingStatus = EMarkStatus.NONE;
                PlayerList[i].SetState(EPlayerState.HomePos);
            }
        }
        return disableAllFlag;
    }

    private void ArrangeCloseMarkWithBall(LLPlayer ballController)
    {
        LogManager.Instance.YellowLog("ArrangeCloseMarkWithBall");
        LLPlayer targetPlayer = ballController;
        if (Opponent.State == ETeamState.TS_ATTACK && targetPlayer != null)
        {
            List<LLPlayer> defenderList = new List<LLPlayer>(PlayerList.Count);
            for (int i = 0; i < PlayerList.Count; i++)
            {
                if (PlayerList[i].NewEventAssignable() && 
                    !PlayerList[i].InDefensiveActions())
                    defenderList.Add(PlayerList[i]);
            }
            //find a nearest defender
            double shiftDistance = Math.Min(TableManager.Instance.AIConfig.GetItem("mark_distance").Value,
                                            targetPlayer.GetPosition().Distance(targetPlayer.Team.Opponent.Goal.GoalPos));
            Vector3D markPosition = targetPlayer.GetPosition() + MathUtil.GetDir(targetPlayer.GetPosition(), targetPlayer.Team.Opponent.Goal.GoalPos) * shiftDistance;
            List<LLPlayer> sortedPlayers = SortNearPlayers(markPosition, defenderList);

            LLPlayer selectedDefender = sortedPlayers[0];

            //choose defensive actions
            selectedDefender.MarkingStatus = EMarkStatus.MARKWITHBALL;
            selectedDefender.Opponent = targetPlayer;
            if (CheckInMarkBallArea(selectedDefender, selectedDefender.Opponent))
            {
                LogManager.Instance.YellowLog("ArrangeCloseMarkWithBall 1");
                //                ActivateDefenceEvent(selectedDefender,EPlayerState.Sliding_Tackle,selectedDefender.Opponent,EPlayerState.LowDribble);
                //                ActivateDefenceEvent(selectedDefender,EPlayerState.Block_Tackle,selectedDefender.Opponent,EPlayerState.LowDribble);
//                                ActivateDefenceEvent(selectedDefender,EPlayerState.Defend_Break_Through,selectedDefender.Opponent,EPlayerState.LowDribble);
//                                return;
                if (CheckInDefenceEventArea(selectedDefender, selectedDefender.Opponent))
                {
                    LogManager.Instance.YellowLog("ArrangeCloseMarkWithBall 2");
                    //defender at front half, attacker can be low dribbled
                    if (CheckNeedMarkBall(selectedDefender, selectedDefender.Opponent))
                    {
                        LogManager.Instance.YellowLog("ArrangeCloseMarkWithBall 3");
                        //check whether activated closemark
                        if (CheckForcedBreakThrough(selectedDefender.Opponent))
                        {
                            //inform opponent to activate break through
                            ActivateDefenceEvent(selectedDefender, EPlayerState.Defend_Break_Through, selectedDefender.Opponent, EPlayerState.LowDribble);
                            return;
                        }
                        if (CheckTacklesInCloseMark(selectedDefender.Opponent, selectedDefender))
                        {
                            //block or slide?
                            //state setting is done in function CheckTacklesInCloseMark, if it returns true;
                            return;
                        }
                        LogManager.Instance.YellowLog("ArrangeCloseMarkWithBall 4");
                    }
                    LogManager.Instance.YellowLog("ArrangeCloseMarkWithBall 5");
                }
                LogManager.Instance.YellowLog("ArrangeCloseMarkWithBall 6");
            }
            else
            {
                LogManager.Instance.YellowLog("ArrangeCloseMarkWithBall 7");
                //check whether need to get close
                if (CheckNeedGetCloseToMarkBall(selectedDefender, selectedDefender.Opponent))
                {
                    selectedDefender.SetState(EPlayerState.CloseMark_WithBall_GetCloser);
                    return;
                }
                LogManager.Instance.YellowLog("ArrangeCloseMarkWithBall 8");
            }
            LogManager.Instance.YellowLog("ArrangeCloseMarkWithBall 9");
            //not activated any defensive actions
            selectedDefender.SetState(EPlayerState.CloseMark_WithBall_NotActivated);
        }
    }

    public void CheckDefensiveEvent(LLPlayer attacker, LLPlayer defender)
    {
        //        ActivateDefenceEvent(defender,EPlayerState.Sliding_Tackle,attacker,EPlayerState.LowDribble);
        //        ActivateDefenceEvent(defender,EPlayerState.Block_Tackle,attacker,EPlayerState.LowDribble);
        //        ActivateDefenceEvent(defender,EPlayerState.Defend_Break_Through,attacker,EPlayerState.LowDribble);
        //        return;
        LogManager.Instance.YellowLog("CheckDefensiveEvent 1");
        if (CheckInDefenceEventArea(defender, attacker))
        {
            LogManager.Instance.YellowLog("CheckDefensiveEvent 2");
            //defender at front half, attacker can be low dribbled
            if (CheckNeedMarkBall(defender, attacker))
            {
                LogManager.Instance.YellowLog("CheckDefensiveEvent 3");
                //check whether activated closemark
                if (CheckForcedBreakThrough(attacker))
                {
                    //inform opponent to activate break through
                    ActivateDefenceEvent(defender, EPlayerState.Defend_Break_Through, attacker, EPlayerState.LowDribble);
                    return;
                }
                if (CheckTacklesInCloseMark(attacker, defender))
                {
                    //block or slide?
                    //state setting is done in function CheckTacklesInCloseMark, if it returns true;
                    return;
                }
                LogManager.Instance.YellowLog("CheckDefensiveEvent 4");
            }
            LogManager.Instance.YellowLog("CheckDefensiveEvent 5");
        }
        LogManager.Instance.YellowLog("CheckDefensiveEvent 6");
        //not activated any defensive actions
        defender.SetState(EPlayerState.CloseMark_WithBall_NotActivated);
    }

    private void ArrangeCloseMarkWithoutBall_AllCourt(LLPlayer ballController, LLPlayer exceptPlayer = null)
    {
        LogManager.Instance.YellowLog("ArrangeCloseMarkWithoutBall_AllCourt");
        GroundAreaItem markAreaSoft = TableManager.Instance.GroundAreaTable.GetItem("markArea_WithoutBall_Soft");
        GroundAreaItem markAreaHard = TableManager.Instance.GroundAreaTable.GetItem("markArea_WithoutBall_Hard");
        GroundAreaItem markAreaDevil = TableManager.Instance.GroundAreaTable.GetItem("markArea_WithoutBall_Devil");

        if (Opponent.State == ETeamState.TS_ATTACK && ballController != null)
        {
            //sort 3 Areaes
            List<LLPlayer> attackersInAreaSoft = new List<LLPlayer>();
            double markPlayerNumSoft = TableManager.Instance.AIConfig.GetItem("no_ball_mark_number1").Value;
            double markActivateRateSoft = TableManager.Instance.AIConfig.GetItem("no_ball_mark_rate1").Value;

            List<LLPlayer> attackersInAreaHard = new List<LLPlayer>();
            double markPlayerNumHard = TableManager.Instance.AIConfig.GetItem("no_ball_mark_number2").Value;
            double markActivateRateHard = TableManager.Instance.AIConfig.GetItem("no_ball_mark_rate2").Value;

            List<LLPlayer> attackersInAreaDevil = new List<LLPlayer>();
            double markPlayerNumDevil = TableManager.Instance.AIConfig.GetItem("no_ball_mark_number3").Value;
            double markActivateRateDevil = TableManager.Instance.AIConfig.GetItem("no_ball_mark_rate3").Value;

            double markActivateRateController = 0;
            //correct num using ballcontroller's region
            if (markAreaSoft.IsInside(ballController.RegionID))
            {
                markPlayerNumSoft -= 1;
                markActivateRateController = markActivateRateSoft;
            }
            else if (markAreaHard.IsInside(ballController.RegionID))
            {
                markPlayerNumHard -= 1;
                markActivateRateController = markActivateRateHard;
            }
            else if (markAreaDevil.IsInside(ballController.RegionID))
            {
                markPlayerNumDevil -= 1;
                markActivateRateController = markActivateRateDevil;
            }

            //sort nearest attackers, except for ball Controller
            List<LLPlayer> attackers = new List<LLPlayer>(Opponent.PlayerList.Count);
            List<LLPlayer> possibleDefender = new List<LLPlayer>(PlayerList.Count);
            for (int i = 0; i < Opponent.PlayerList.Count; i++)
            {
                if (Opponent.PlayerList[i] != ballController)
                    attackers.Add(Opponent.PlayerList[i]);

                if (PlayerList[i] != exceptPlayer 
                    && PlayerList[i].NewEventAssignable() 
                    && PlayerList[i].MarkingStatus != EMarkStatus.MARKWITHBALL)
                    possibleDefender.Add(PlayerList[i]);
            }

            attackers = SortNearPlayers(ballController.GetPosition(), attackers);
            for (int i = 0; i < attackers.Count; i++)
            {
                if (attackers[i] == ballController)
                    continue;
                LLPlayer currentDefender = attackers[i];
                if (currentDefender != exceptPlayer)
                {
                    if (markAreaSoft.IsInside(currentDefender.RegionID))
                    {
                        if (attackersInAreaSoft.Count < markPlayerNumSoft)
                            attackersInAreaSoft.Add(currentDefender);
                    }
                    else if (markAreaHard.IsInside(currentDefender.RegionID))
                    {
                        if (attackersInAreaHard.Count < markPlayerNumHard)
                            attackersInAreaHard.Add(currentDefender);
                    }
                    else// if(markAreaDevil.IsInside(currentDefender.RegionID))
                    {
                        if (attackersInAreaDevil.Count < markPlayerNumDevil)
                            attackersInAreaDevil.Add(currentDefender);
                    }
                }
            }

            if(GetCloseMarkDefender(ballController) == null)
            {
                //if no one is CloseMarking ballController, then arrange ball controller first
                Vector3D targetPosition = GetMarkWithoutBallPos(ballController.GetPosition());
                possibleDefender = SortNearPlayers(targetPosition, possibleDefender);
                //search for nearest defender
                for (int j = 0; j < possibleDefender.Count; j++)
                {
                    if (possibleDefender[j].MarkingStatus != EMarkStatus.NONE)
                        continue;
                    //arrange this one
                    if (FIFARandom.GetRandomValue(0, 1) < markActivateRateController)
                    {
                        LogManager.Instance.YellowLog("mark the controller: defender " + possibleDefender[j].PlayerBaseInfo.HeroID + " ---> attacker " + ballController.PlayerBaseInfo.HeroID);
                        possibleDefender[j].MarkingStatus = EMarkStatus.MARKWITHOUTBALL;
                        possibleDefender[j].Opponent = ballController;
                        possibleDefender[j].SetState(EPlayerState.ToCloseMark_WithoutBall);
                        possibleDefender.Remove(possibleDefender[j]);
                        break;
                    }
                }
            }
            // arrange defenders with following priority: AreaDevil -> AreaHard -> AreaSoft
            possibleDefender = ArrangeNearestDefender(attackersInAreaDevil, possibleDefender, markActivateRateDevil);
            possibleDefender = ArrangeNearestDefender(attackersInAreaHard, possibleDefender, markActivateRateHard);
            possibleDefender = ArrangeNearestDefender(attackersInAreaSoft, possibleDefender, markActivateRateSoft);

            SetCloseMarkWithoutBallArrangement();
        }
    }

    private void SetCloseMarkWithoutBallArrangement()
    {
        LogManager.Instance.YellowLog("SetCloseMarkWithoutBallArrangement");
        m_CloseMarkWithoutBallArrangement.Clear();
        for (int i = 0; i < PlayerList.Count; i++)
        {
            if(PlayerList[i].MarkingStatus == EMarkStatus.MARKWITHOUTBALL && PlayerList[i].Opponent != null)
            {
                //record this pair
                m_CloseMarkWithoutBallArrangement[PlayerList[i].Opponent] = PlayerList[i];
            }
        }
    }

    private List<LLPlayer> SortNearPlayers(Vector3D targetPos, List<LLPlayer> players)
    {
        players.Sort((LLPlayer a, LLPlayer b) =>
        {
            double distA = a.GetPosition().Distance(targetPos);
            double distB = b.GetPosition().Distance(targetPos);
            return distA.CompareTo(distB);
        });
        return players;
    }

    private List<LLPlayer> ArrangeNearestDefender(List<LLPlayer> attackers, List<LLPlayer> defenders, double activateRate)
    {
        if (defenders == null || defenders.Count == 0)
        {
            //no available defenders
            return null;
        }
        if (attackers == null || attackers.Count == 0)
        {
            return defenders;
        }
        LogManager.Instance.YellowLog("Arrange ====== ");
        
        LLPlayer attacker = null;
        LLPlayer defender = null;
        for (int i = 0; i < attackers.Count; i++)
        {
            Vector3D targetPosition = GetMarkWithoutBallPos(attackers[i].GetPosition());
            defenders = SortNearPlayers(targetPosition, defenders);
            //search for nearest defender
            for (int j = 0; j < defenders.Count; j++)
            {
                if (defenders[j].MarkingStatus != EMarkStatus.NONE)
                {
                    LogManager.Instance.LogError("This should not happen");
                    UnityEngine.Debug.Break();
                    continue;
                }//arrange this one
                if (FIFARandom.GetRandomValue(0, 1) < activateRate)
                {
                    attacker = attackers[i];
                    defender = defenders[j];
                    if (m_CloseMarkWithoutBallArrangement != null)
                    {
                        //find pre-set pair
                        if(m_CloseMarkWithoutBallArrangement.ContainsKey(attacker)
                           && defenders.Contains(m_CloseMarkWithoutBallArrangement[attacker]))
                        {
                            defender = m_CloseMarkWithoutBallArrangement[attacker];
                            LogManager.Instance.YellowLog("Original pair");
                        }
                    }

                    //arrange them
                    LogManager.Instance.YellowLog("defender "+defender.PlayerBaseInfo.HeroID + " ---> attacker" + attacker.PlayerBaseInfo.HeroID);
                    defender.MarkingStatus = EMarkStatus.MARKWITHOUTBALL;
                    defender.Opponent = attacker;
                    defender.SetState(EPlayerState.ToCloseMark_WithoutBall);
                    defenders.Remove(defender);
                    break;
                }
            }
        }
        return defenders;
    }

    private List<LLPlayer> ArrangeCloseMarkWithoutBall(LLPlayer attacker, LLPlayer defender, List<LLPlayer> possibleDefenders, double activateRate)
    {
        if (m_CloseMarkWithoutBallArrangement != null)
        {
            //find pre-set pair
            if(m_CloseMarkWithoutBallArrangement.ContainsKey(attacker)
               && possibleDefenders.Contains(m_CloseMarkWithoutBallArrangement[attacker]))
            {
                //arrange them
                LogManager.Instance.YellowLog("defender "+m_CloseMarkWithoutBallArrangement[attacker].PlayerBaseInfo.HeroID + " -- Original -> attacker" + attacker.PlayerBaseInfo.HeroID);
                m_CloseMarkWithoutBallArrangement[attacker].MarkingStatus = EMarkStatus.MARKWITHOUTBALL;
                m_CloseMarkWithoutBallArrangement[attacker].Opponent = attacker;
                m_CloseMarkWithoutBallArrangement[attacker].SetState(EPlayerState.ToCloseMark_WithoutBall);
                return possibleDefenders;
            }
        } 
        //arrange them
        LogManager.Instance.YellowLog("defender "+defender.PlayerBaseInfo.HeroID + " -- New -> attacker" + attacker.PlayerBaseInfo.HeroID);
        defender.MarkingStatus = EMarkStatus.MARKWITHOUTBALL;
        defender.Opponent = attacker;
        defender.SetState(EPlayerState.ToCloseMark_WithoutBall);
        return possibleDefenders;
    }
    
    public bool CheckInMarkBallArea(LLPlayer defender, LLPlayer attacker)
    {
        double markAreaRadius = TableManager.Instance.AIConfig.GetItem("def_radius_dribble").Value;
        if (defender == null || attacker == null)
            return false;
        return (defender.GetPosition().Distance(attacker.GetPosition()) <= markAreaRadius);
    }

    public bool CheckInDefenceEventArea(LLPlayer defender, LLPlayer attacker)
    {
        if (defender == null || attacker == null)
            return false;
        if (defender.TeamType == ETeamColor.Team_Red)
        {
            //defender need smaller z value
            return (defender.GetPosition().Z < attacker.GetPosition().Z);
        }
        else
        {
            //defender need bigger z value
            return (defender.GetPosition().Z > attacker.GetPosition().Z);
        }
    }

    private bool CheckNeedGetCloseToMarkBall(LLPlayer defender, LLPlayer attacker)
    {
        double markAreaRadius = TableManager.Instance.AIConfig.GetItem("def_radius_dribble").Value;
        double getCloserToMarkSoft = TableManager.Instance.AIConfig.GetItem("enter_mark_zone1").Value;
        double getCloserToMarkHard = TableManager.Instance.AIConfig.GetItem("enter_mark_zone2").Value;
        GroundAreaItem markAreaSoft = TableManager.Instance.GroundAreaTable.GetItem("markArea_WithBall_Hard");
        GroundAreaItem markAreaHard = TableManager.Instance.GroundAreaTable.GetItem("markArea_WithBall_Soft");

        if (attacker != null)
        {
            if (defender.GetPosition().Distance(attacker.GetPosition()) > markAreaRadius)
            {
                if (markAreaSoft.IsInside(attacker.RegionID))
                {
                    return FIFARandom.GetRandomValue(0, 1) <= getCloserToMarkSoft;
                }
                else if (markAreaHard.IsInside(attacker.RegionID))
                {
                    return FIFARandom.GetRandomValue(0, 1) <= getCloserToMarkHard;
                }
            }
        }
        return false;
    }

    private bool CheckNeedMarkBall(LLPlayer defender, LLPlayer attacker)
    {
        //attacker can be low dribbled
        if (CheckForcedLowDribble(attacker))
        {
            double markAreaRadius = TableManager.Instance.AIConfig.GetItem("def_radius_dribble").Value;
            double triggerMarkRateSoft = TableManager.Instance.AIConfig.GetItem("trigger_mark_event1").Value;
            double triggerMarkRateHard = TableManager.Instance.AIConfig.GetItem("trigger_mark_event1").Value;
            GroundAreaItem markAreaSoft = TableManager.Instance.GroundAreaTable.GetItem("markArea_WithBall_Hard");
            GroundAreaItem markAreaHard = TableManager.Instance.GroundAreaTable.GetItem("markArea_WithBall_Soft");

            if (defender == null || attacker == null)
                return false;
            if (defender.GetPosition().Distance(attacker.GetPosition()) <= markAreaRadius)
            {
                //inside area, judge each area
                if (markAreaSoft.IsInside(attacker.RegionID))
                {
                    return FIFARandom.GetRandomValue(0, 1) <= triggerMarkRateSoft;
                }
                else if (markAreaHard.IsInside(attacker.RegionID))
                {
                    return FIFARandom.GetRandomValue(0, 1) <= triggerMarkRateHard;
                }
            }
        }
        return false;
    }

    //called by defender, attacker's state is changed by defender during the animation
    // 突破事件
    public void CheckDefendBreakThrough(LLPlayer attacker, LLPlayer defender)
    {
        attacker.Opponent = defender;
        defender.Opponent = attacker;

        //status protections
        if (attacker.State != EPlayerState.NormalDribble &&
            attacker.State != EPlayerState.LowDribble)
        {
            LogManager.Instance.GreenLog("Defend_Break_Through_Failed " + attacker.State);
            defender.SetState(EPlayerState.Defend_Break_Through_Failed);
            //            attacker.SetState(EPlayerState.Break_Through_Success);
            return;
        }
        //Numeric Check
        attacker.CastSkill(EEventType.ET_BreakThrough);
        NS_Break.Caculate(attacker, defender);
        double dVal = 0;
        if (NS_Break.Valid)
            dVal = NS_Break.RandVal;
        else
            dVal = FIFARandom.GetRandomValue(0, 1);

        if (dVal < NS_Break.SucessPr)
        {
            LogManager.Instance.GreenLog("Defend_Break_Through_Failed");
            defender.SetState(EPlayerState.Defend_Break_Through_Failed);
            attacker.SetState(EPlayerState.Break_Through_Success);
        }
        else
        {
            attacker.IsChangeAniID = false;
            LogManager.Instance.GreenLog("Defend_Break_Through_Success");
            defender.SetState(EPlayerState.Defend_Break_Through_Success);
            attacker.SetState(EPlayerState.Break_Through_Failed);
        }
    }


    //called by defender, attacker's state is changed by defender during the animation
    public bool CheckTacklesInCloseMark(LLPlayer attacker, LLPlayer defender)
    {
        double rateBlockTackleSoft = TableManager.Instance.AIConfig.GetItem("intercept_versus1").Value;
        double rateSlideTackleSoft = TableManager.Instance.AIConfig.GetItem("tackle_versus1").Value;
        double rateBlockTackleHard = TableManager.Instance.AIConfig.GetItem("intercept_versus2").Value;
        double rateSlideTackleHard = TableManager.Instance.AIConfig.GetItem("tackle_versus2").Value;

        GroundAreaItem markAreaSoft = TableManager.Instance.GroundAreaTable.GetItem("backHalf_Area");
        GroundAreaItem markAreaHard = TableManager.Instance.GroundAreaTable.GetItem("frontHalf_Area");

        double rateResult = FIFARandom.GetRandomValue(0, 1);
        double[] rates = new double[2];
        if (markAreaSoft.IsInside(attacker.RegionID))
        {
            rates[0] = rateBlockTackleSoft;
            rates[1] = rateBlockTackleSoft + rateSlideTackleSoft;

        }
        else if (markAreaHard.IsInside(attacker.RegionID))
        {
            rates[0] = rateBlockTackleHard;
            rates[1] = rateBlockTackleHard + rateSlideTackleHard;
        }

        if (rateResult <= rates[0])
        {
            //blocked
            LogManager.Instance.GreenLog("Block_Tackle");
            ActivateDefenceEvent(defender, EPlayerState.Block_Tackle, attacker, EPlayerState.LowDribble);
            return true;
        }
        else if (rateResult <= rates[1])
        {
            //slided
            LogManager.Instance.GreenLog("Sliding_Tackle");
            ActivateDefenceEvent(defender, EPlayerState.Sliding_Tackle, attacker, EPlayerState.LowDribble);
            return true;
        }
        else
        {
            //not tackled
            return false;
        }
    }

    //called by defender, attacker's state is changed by defender during the animation
    // 铲球
    public void CheckSlidingTackle(LLPlayer attacker, LLPlayer defender)
    {
        attacker.Opponent = defender;
        defender.Opponent = attacker;

        //status protections
        if (attacker.State != EPlayerState.NormalDribble &&
            attacker.State != EPlayerState.LowDribble &&
            attacker.State != EPlayerState.ActionSelect)
        {
            LogManager.Instance.GreenLog("Sliding_Tackle_Failed: attacker is " + attacker.State);
            defender.SetState(EPlayerState.Sliding_Tackle_Failed);
            //            attacker.SetState(EPlayerState.Avoid_Sliding_Tackle_Success);
            return;
        }
        //numeric check
        NS_Tackle.Caculate(defender, attacker);
        double dRandVal = 0;
        if (NS_Tackle.Valid)
            dRandVal = NS_Tackle.RandVal;
        else
            dRandVal = FIFARandom.GetRandomValue(0, 1);
        if (dRandVal < NS_Tackle.SucessPr)
        {
            LogManager.Instance.GreenLog("Sliding_Tackle_Success");
            defender.SetState(EPlayerState.Sliding_Tackle_Success);
            attacker.SetState(EPlayerState.Avoid_Sliding_Tackle_Failed);
        }
        else
        {
            LogManager.Instance.GreenLog("Sliding_Tackle_Failed");
            defender.SetState(EPlayerState.Sliding_Tackle_Failed);
            attacker.SetState(EPlayerState.Avoid_Sliding_Tackle_Success);
        }
    }

    //called by defender, attacker's state is changed by defender during the animation
    //抢断事件
    public void CheckBlockTackle(LLPlayer attacker, LLPlayer defender)
    {
        attacker.Opponent = defender;
        defender.Opponent = attacker;

        //status protections
        if (attacker.State != EPlayerState.NormalDribble &&
            attacker.State != EPlayerState.LowDribble)
        {
            LogManager.Instance.GreenLog("Block_Tackle_Failed attacker is :" + attacker.State);
            defender.SetState(EPlayerState.Block_Tackle_Failed);
            //            attacker.SetState(EPlayerState.Avoid_Block_Tackle_Success);
            return;
        }
        //numeric check
        defender.CastSkill(EEventType.ET_Block);
        NS_Snatch.Caculate(defender, attacker);
        double dVal = 0;
        if (NS_Snatch.Valid)
            dVal = NS_Snatch.RandVal;
        else
            dVal = FIFARandom.GetRandomValue(0, 1);
        if (dVal < NS_Snatch.SucessPr)
        {
            LogManager.Instance.GreenLog("Block_Tackle_Success");
            defender.SetState(EPlayerState.Block_Tackle_Success);
            attacker.SetState(EPlayerState.Avoid_Block_Tackle_Failed);
        }
        else
        {
            LogManager.Instance.GreenLog("Block_Tackle_Failed");
            defender.SetState(EPlayerState.Block_Tackle_Failed);
            attacker.SetState(EPlayerState.Avoid_Block_Tackle_Success);
        }
    }

    //called by attacker
    public void CheckHeadingTackle(LLPlayer attacker, LLPlayer defender, bool attackerFirstHeading, double attackerWonPr = 0.5d)
    {
        attacker.Opponent = defender;
        if (defender != null)
        {
            defender.Opponent = attacker;
            if (FIFARandom.GetRandomValue(0, 1) < attackerWonPr)
            {
                LogManager.Instance.YellowLog("HeadingTackle Attacker Won" + attacker.PlayerBaseInfo.HeroID);
                HeadingChoices(attacker, attackerFirstHeading);
                defender.SetState(EPlayerState.Heading_Tackle_Failed);
            }
            else
            {
                LogManager.Instance.YellowLog("HeadingTackle Defender Won" + defender.PlayerBaseInfo.HeroID);
                attacker.SetState(EPlayerState.Heading_Tackle_Failed);
                HeadingChoices(defender, attackerFirstHeading);
            }
        }
        else
        {
            LogManager.Instance.GreenLog("CheckHeadingTackle no defender, attackerFirstHeading: " + attackerFirstHeading);
            HeadingChoices(attacker, attackerFirstHeading);
        }
    }

    private void HeadingChoices(LLPlayer targetPlayer, bool canPass)
    {
        if (Scene.GameState == EGameState.GS_FIX_PASS)
        {
            //will pass
            LogManager.Instance.GreenLog("HeadingChoices: EGameState.GS_FIX_PASS---EPlayerState.Heading_Tackle_ToPass");
            targetPlayer.SetState(EPlayerState.Heading_Tackle_ToPass);
        }
        else
        {
            double rate = FIFARandom.GetRandomValue(0, 1);
            bool canShoot = (targetPlayer.GetPosition().Distance(targetPlayer.Team.Opponent.Goal.GoalPos)
                             < TableManager.Instance.AIConfig.GetItem("head_shoot_distance").Value);
            if (canPass)
            {
                bool passAvailable = (targetPlayer.Team.SelectNearestPlayer(targetPlayer).GetPosition().Distance(targetPlayer.GetPosition())
                                      < TableManager.Instance.AIConfig.GetItem("max_headpass_distance").Value);
                if (canShoot)
                {
                    if (passAvailable)
                    {
                        if (rate < 0.33d)
                        {
                            LogManager.Instance.GreenLog("HeadingChoices: EPlayerState.Heading_Tackle_ToShoot");
                            targetPlayer.SetState(EPlayerState.Heading_Tackle_ToShoot);
                        }
                        else if (rate < 0.67d)
                        {
                            LogManager.Instance.GreenLog("HeadingChoices: EPlayerState.Heading_Tackle_ToPass");
                            targetPlayer.SetState(EPlayerState.Heading_Tackle_ToPass);
                        }
                        else
                        {
                            LogManager.Instance.GreenLog("HeadingChoices: EPlayerState.Heading_Tackle_ToDribble");
                            targetPlayer.SetState(EPlayerState.Heading_Tackle_ToDribble);
                        }
                    }
                    else
                    {
                        if (rate < 0.5d)
                        {
                            LogManager.Instance.GreenLog("HeadingChoices: EPlayerState.Heading_Tackle_ToShoot");
                            targetPlayer.SetState(EPlayerState.Heading_Tackle_ToShoot);
                        }
                        else
                        {
                            LogManager.Instance.GreenLog("HeadingChoices: EPlayerState.Heading_Tackle_ToDribble");
                            targetPlayer.SetState(EPlayerState.Heading_Tackle_ToDribble);
                        }
                    }
                }
                else
                {
                    if (passAvailable)
                    {
                        if (rate < 0.5d)
                        {
                            LogManager.Instance.GreenLog("HeadingChoices: EPlayerState.Heading_Tackle_ToPass");
                            targetPlayer.SetState(EPlayerState.Heading_Tackle_ToPass);
                        }
                        else
                        {
                            LogManager.Instance.GreenLog("HeadingChoices: EPlayerState.Heading_Tackle_ToDribble");
                            targetPlayer.SetState(EPlayerState.Heading_Tackle_ToDribble);
                        }
                    }
                    else
                    {
                        LogManager.Instance.GreenLog("HeadingChoices: EPlayerState.Heading_Tackle_ToDribble");
                        targetPlayer.SetState(EPlayerState.Heading_Tackle_ToDribble);
                    }
                }
            }
            else
            {
                if (canShoot)
                {
                    if (rate < 0.5d)
                    {
                        LogManager.Instance.GreenLog("HeadingChoices: EPlayerState.Heading_Tackle_ToShoot");
                        targetPlayer.SetState(EPlayerState.Heading_Tackle_ToShoot);
                    }
                    else
                    {
                        LogManager.Instance.GreenLog("HeadingChoices: EPlayerState.Heading_Tackle_ToDribble");
                        targetPlayer.SetState(EPlayerState.Heading_Tackle_ToDribble);
                    }
                }
                else
                {
                    LogManager.Instance.GreenLog("HeadingChoices: EPlayerState.Heading_Tackle_ToDribble");
                    targetPlayer.SetState(EPlayerState.Heading_Tackle_ToDribble);
                }
            }
        }
    }

    //拦截 球运行的路上
    public void CheckIntercept(LLPlayer defender)
    {
        LLTeam attackTeam = defender.Team.Opponent;
        if (attackTeam.NS_Block.Valid)
        {
            if (FIFARandom.GetRandomValue(0, 1) < attackTeam.NS_Block.InterceptPr)
            {
                //intercept successfully
                LogManager.Instance.ColorLog("#0080ffff", "CheckIntercept: EPlayerState.Intercept_Success");
                defender.SetState(EPlayerState.Intercept_Success);
                if(attackTeam.BallController!=null)
                    attackTeam.BallController.SetState(EPlayerState.Idle);
            }
            else
            {
                //intercept failed
                LogManager.Instance.ColorLog("#0080ffff", "CheckIntercept: EPlayerState.Intercept_Failed");
                defender.SetState(EPlayerState.Intercept_Failed);
            }
        }
        else
        {
            if (FIFARandom.GetRandomValue(0, 1) < 0.5d)
            {
                //intercept successfully
                LogManager.Instance.ColorLog("#0080ffff", "CheckIntercept: EPlayerState.Intercept_Success");
                defender.SetState(EPlayerState.Intercept_Success);
                if(attackTeam.BallController!=null)
                    attackTeam.BallController.SetState(EPlayerState.Idle);
            }
            else
            {
                //intercept failed
                LogManager.Instance.ColorLog("#0080ffff", "CheckIntercept: EPlayerState.Intercept_Failed");
                defender.SetState(EPlayerState.Intercept_Failed);
            }
        }
    }

    private Vector3D ForceMeetByLine(Vector3D p1Pos, double p1Vel, Vector3D p2Pos, double p2Vel)
    {
        double lerpFactor = p1Vel / (p1Vel + p2Vel);
        Vector3D meetPos = new Vector3D();
        meetPos = p1Pos * lerpFactor + p2Pos * (1 - lerpFactor);
        return meetPos;
    }

    private Vector3D ForceMeetByBezierCurve(Vector3D p1Pos, Vector3D p1Vel, Vector3D p2Pos, Vector3D p2Vel)
    {
        double lerpFactor = p1Vel.Length() / (p1Vel.Length() + p2Vel.Length());
        double tempF = 1 - lerpFactor;

        Vector3D p1VelPos = p1Pos + p1Vel;
        Vector3D p2VelPos = p2Pos + p2Vel;

        Vector3D meetPos = new Vector3D();

        meetPos = Math.Pow(tempF, 3) * p1Pos
                + 3 * lerpFactor * Math.Pow(tempF, 2) * p1VelPos
                + 3 * Math.Pow(lerpFactor, 2) * tempF * p2VelPos
                + Math.Pow(lerpFactor, 3) * p2Pos;
        return meetPos;
    }

    public Vector3D GetMarkWithoutBallPos(Vector3D attackerPos)
    {
        double minDist = 0;
        double maxDist = TableManager.Instance.AIConfig.GetItem("def_radius_def_max").Value;
        Vector3D goalDirCenter = MathUtil.GetDir(attackerPos, Goal.GoalPos);
        Vector3D goalDirLeft = MathUtil.GetDir(attackerPos, Goal.LeftCorner);
        Vector3D goalDirRight = MathUtil.GetDir(attackerPos, Goal.RightCorner);

        //double targetOffset = (minDist + maxDist) / 2;
        //Vector3D targetDir = (goalDirLeft + goalDirRight) / 2;
        double targetOffset = 3.5d;
        Vector3D targetDir = goalDirCenter;
        return attackerPos + targetDir * targetOffset;
    }

    public bool IsInMarkWithoutBallArea(LLPlayer attacker, LLPlayer defender)
    {
        double minDist = TableManager.Instance.AIConfig.GetItem("def_radius_def_min").Value;
        double maxDist = TableManager.Instance.AIConfig.GetItem("def_radius_def_max").Value;
        double realDistance = defender.GetPosition().Distance(attacker.GetPosition());

        Vector3D goalDirCenter = MathUtil.GetDir(attacker.GetPosition(), Goal.GoalPos);
        Vector3D goalDirLeft = MathUtil.GetDir(attacker.GetPosition(), Goal.LeftCorner);
        Vector3D goalDirRight = MathUtil.GetDir(attacker.GetPosition(), Goal.RightCorner);
        double goalAngleCenter = MathUtil.GetAngle(attacker.GetPosition(), Goal.GoalPos);
        double goalAngleLeft = MathUtil.GetAngle(attacker.GetPosition(), Goal.LeftCorner);
        double goalAngleRight = MathUtil.GetAngle(attacker.GetPosition(), Goal.RightCorner);

        if (MathUtil.GetMinAngle(goalAngleLeft, goalAngleCenter) < 30)
        {
            //change to a min dir with a delta-Angle 30
            goalDirLeft = MathUtil.GetDirFormAngle(goalAngleCenter - 30);
        }
        if (MathUtil.GetMinAngle(goalAngleRight, goalAngleCenter) < 30)
        {
            //change to a min dir with a delta-Angle 30
            goalDirRight = MathUtil.GetDirFormAngle(goalAngleCenter + 30);
        }

        Vector3D currentDir = MathUtil.GetDir(attacker.GetPosition(), defender.GetPosition());
        return (minDist <= realDistance) && (realDistance <= maxDist) && MathUtil.IsVectorBetween(currentDir, goalDirLeft, goalDirRight);
    }

    /*
     * 更新计算球员所在的格式
     */
    private void UpdatePlayerRegionID()
    {
        for (int i = 0; i < m_kPlayerList.Count; i++)
        {
            int iRegionID = m_kScene.GetRegionID(m_kPlayerList[i].GetPosition());
            if (iRegionID > 0)
            {
                if (ETeamColor.Team_Blue == m_kTeamColor)
                    iRegionID = 40 - iRegionID + 1;
            }

            m_kPlayerList[i].RegionID = iRegionID;
        }
    }

    // 重置球员到中场开球的位置
    public void ResetToMidKickPos()
    {

        LLBall kBall = m_kScene.Ball;
        Vector3D kBallPos = kBall.GetPosition();

        m_fGuardLine = -2.5 * m_kScene.Radius + 2.5 / 6.5;
        float fSign = m_kTeamColor == ETeamColor.Team_Red ? 1 : -1;
        if (null == m_kScene)
            return;
        UpdateNewBasePosition(true);
        m_kBattlePosItem = m_kBattlePosLogic.GetMatchBPTable(StandType.MidKick_Control);
        m_iKickUnitIdx = 0;
    }

    public void ResetMidKickAngle()
    {
        for (int i = 0; i < PlayerList.Count; i++)
        {
            PlayerList[i].SetRoteAngle(MathUtil.GetAngle(PlayerList[i].GetPosition(),Vector3D.zero));
            PlayerList[i].SetAniState(EAniState.Kick_Idle);
        }
    }
    #endregion

    #region Select Player
    public LLPlayer SelectPlannedPlayerToPass()
    {
        LLPlayer kPlayer = null;
        
        switch (Scene.GameState)
        {
            case EGameState.GS_MIDKICK:
                kPlayer = SelectMidKickPlayer();
                break;
            case EGameState.GS_NORMAL:
                kPlayer = SelectPlayer();
                break;
            case EGameState.GS_FIX_PASS:
                kPlayer = SelectMidKickPlayer();
                break;
            case EGameState.GS_SHOOT:
                kPlayer = SelectRandomPlayerInRegin();
                break;
            default:
                break;
        }
        if (null == kPlayer)
        {
            LogManager.Instance.LogError("BallControllser un selected:{0}", Scene.GameState.ToString());
        }
        return kPlayer;
    }

    // 选择控球球员
    public LLPlayer SelectBallController()
    {
        LLPlayer kPlayer = null;

        switch (Scene.GameState)
        {
            case EGameState.GS_MIDKICK:
                kPlayer = SelectMidKickPlayer();
                break;
            case EGameState.GS_NORMAL:
                kPlayer = SelectPlayer();
                break;
            case EGameState.GS_FIX_PASS: ///
                kPlayer = SelectMidKickPlayer();
                break;
            case EGameState.GS_SHOOT:
                kPlayer = SelectRandomPlayerInRegin();
                break;
            default:
                break;
        }
        if (null == kPlayer)
        {
            LogManager.Instance.LogError("BallControllser un selected:{0}", Scene.GameState.ToString());
        }
        //        LogManager.Instance.RedLog("SelectBallController m_kBallController : "+m_kBallController.PlayerBaseInfo.HeroID);
        RefreshHomePositionMessage _refresh = new RefreshHomePositionMessage(m_kScene);
        MessageDispatcher.Instance.SendMessage(_refresh);
        return kPlayer;
    }

    public LLPlayer SelectMidKickPlayer()
    {
        if (null == BallController)
        {
            LogManager.Instance.LogError("开球时当前球队没有控球队员");
            return null;
        }
        if (null == m_kBattlePosItem)
        {
            LogManager.Instance.LogError("没有初始化开场球员信息");
            return null;

        }
        uint iPlayerIdx = uint.MaxValue;
        if (m_iKickUnitIdx < m_kBattlePosItem.m_MiddleKickList.Count)
        {
            iPlayerIdx = (uint)(m_kBattlePosItem.m_MiddleKickList[m_iKickUnitIdx++]);
        }
        if (iPlayerIdx < m_kPlayerList.Count && iPlayerIdx >= 0)
            return m_kPlayerList[(int)iPlayerIdx];
        return null;
        //  return SelectNearestPlayer(BallController);
    }

    //选择身后最近的球员
    private LLPlayer SelectBehindPlayer()
    {
        if (null == m_kBallController)
        {
            LogManager.Instance.LogError("当前球队没有控球队员");
            return null;
        }

        int iNearestIdx = int.MaxValue;
        double fMinDist = 10000;
        Vector3D kPos = m_kBallController.GetPosition();


        for (int i = 0; i < m_kPlayerList.Count; i++)
        {
            if (m_kBallController == m_kPlayerList[i])
                continue;

            Vector3D kPlayerPos = m_kPlayerList[i].GetPosition();

            bool bFlag = m_kTeamColor == ETeamColor.Team_Red ? kPlayerPos.Z > kPos.Z : kPlayerPos.Z < kPos.Z;
            if (bFlag)
            {
                continue;
            }
            else
            {
                double fDist = kPos.Distance(kPlayerPos);
                if (fDist < fMinDist)
                {
                    fMinDist = fDist;
                    iNearestIdx = i;
                }
            }
        }
        if (int.MaxValue == iNearestIdx)
        {
            LogManager.Instance.LogError("找不到当前队员的身后球员");
            return m_kBallController;
        }

        return m_kPlayerList[iNearestIdx];
    }

    // 选择接球"员
    public LLPlayer SelectPlayer()
    {
        // 控球球员选择下一个接球球员
        if (null == m_kBallController)
            return null;
        return GetMatchGeterInPostion();
    }

    public LLPlayer SelectPlayerForHeadingPass(LLPlayer playerFrom,bool ignoreGameState = false)
    {
        if (!ignoreGameState && Scene.GameState == EGameState.GS_FIX_PASS)
            return SelectBallController();
        else
            return SelectNearestPlayer(playerFrom);
    }

    public LLPlayer SelectNearestPlayer(LLPlayer playerFrom)
    {
        //select a nearest player
        List<LLPlayer> teamMates = new List<LLPlayer>(PlayerList.Count);
        for (int i = 0; i < PlayerList.Count; i++)
        {
            if (playerFrom != PlayerList[i])
                teamMates.Add(PlayerList[i]);
        }
        List<LLPlayer> NearestTeamMates = SortNearPlayers(playerFrom.GetPosition(), teamMates);

        return (NearestTeamMates[0] != playerFrom) ? NearestTeamMates[0] : NearestTeamMates[1];
    }


    /// <summary>
    /// 球员的选择包含距离得分，战术得分，出球难度等来获取一个比分，进而选择最佳接球员
    /// </summary>
    /// <returns></returns>
    public LLPlayer GetMatchGeterInPostion()
    {
        List<LLPlayer> _ps = new List<LLPlayer>();
        Region _r1 = SelectAttackZone();
        Region _r2 = SelecteEtZone();
        for (int i = 0; i < m_kPlayerList.Count; i++)
        {
            if (m_kBallController == m_kPlayerList [i])
                continue;
            if (m_kPlayerList [i].EnoughCatchBallEnergy())
            {
                double _disS = GetDistanceScore(m_kPlayerList [i]);
                double _depthS = GetETacticsScore(m_kPlayerList [i], _r1, (int)ETacticsBallType.Attack_Depth);
                double _StageS = GetETacticsScore(m_kPlayerList [i], _r2, (int)ETacticsBallType.Stage_Direction);
                double _Ps = GetPlayerPosScore(m_kPlayerList [i]);
                double _Ggs = GetGeterParam(m_kPlayerList [i]);


                //passer's difficulty
                double _GpS = GetPassParam();
                double _score = (_disS + _depthS + _StageS + _Ps) * _GpS * _Ggs;
                m_kPlayerList [i].GeterScore = _score;
                m_kPlayerList [i].Socore = new Socore(_disS, _StageS, _depthS, _Ps, _GpS, _Ggs);

                _ps.Add(m_kPlayerList [i]);
            }
        }

        _ps.Sort(delegate(LLPlayer kP1, LLPlayer kP2)
        {
            if (kP1.GeterScore.CompareTo(kP2.GeterScore) > 0)
                return 1;
            else if (kP1.GeterScore.CompareTo(kP2.GeterScore) < 0)
                return -1;
            return 0;
        });
        DrawLine(_ps, _ps[_ps.Count - 1], m_kBallController);
        return _ps[_ps.Count - 1];
    }

    #region 球员距离得分
    /// <summary>
    /// 获得某球员的距离得分
    /// </summary>
    /// <param name="_player">潜在接球球员</param>
    /// <returns></returns>
    public double GetDistanceScore(LLPlayer _player)
    {
        double _dis = 0f;
        string _key = "";
        switch(m_kAttackChoice)
        {
            case AttackChoice.Short_In:
                _key = "distance_coefficient1";
                break;
            case AttackChoice.Long_Pass:
                _key = "distance_coefficient2";
                break;
            case AttackChoice.Long_Short_Combine:
                _key = "distance_coefficient";
                break;
        }
        AICfgItem kItem = TableManager.Instance.AIConfig.GetItem(_key);
        double _disParam = kItem.Value;
        _dis = m_kBallController.GetPosition().Distance(_player.GetPosition());
        //         LogManager.Instance.RedLog("GetDistanceScore==============================================");
        double _perecent = 0d;
        if(m_kAttackChoice!= AttackChoice.Long_Pass)
        {
            _perecent = TableManager.Instance.DistanceCoefficientTbl.GetCurrentPercentByDistance(_dis);

        }
        else
        {
            _perecent = TableManager.Instance.DistanceCoefficientTbl1.GetCurrentPercentByDistance(_dis);
        }
        //         LogManager.Instance.RedLog("GetDistanceScore======>HeroId===="+_player.PlayerBaseInfo.HeroID+"--------DisTance==="+_dis+"-------_Percent===="+_perecent);
        return (double)(_perecent * _disParam * 100);
    }

    #endregion

    #region 战术位置匹配得分
    /// <summary>
    /// 战术得分计算
    /// </summary>
    /// <returns></returns>
    public double GetETacticsScore(LLPlayer _player, Region _r, int type)
    {
        double _etParam = 0f;
        AICfgItem kAICfgItem;
        if (type == 1)
        {
            kAICfgItem = TableManager.Instance.AIConfig.GetItem("depth_coefficient");
        }
        else
        {
            kAICfgItem = TableManager.Instance.AIConfig.GetItem("strategy_coefficient");
        }
        _etParam = kAICfgItem.Value;
        //根据战术决定相对持球者的传球区域//
        //Region _r = SetLLpalyerLimitZone(_player.Team.TeamColor);
        if (_r.InCheckRegion(_player.GetPosition()))
        {
            return _etParam * 100;
        }
        else
            return 0f;
    }

    /// <summary>
    /// 根据战术进攻类型来决定攻击区域选择,AttackType
    /// </summary>
    /// <returns></returns>
    private Region SelectAttackZone()
    {
        Region _r;
        AttackTacticalData _data = TableManager.Instance.AttackTacticalConfig.GetDataById((int)m_kAttackType);
        if (_data == null)
        {
            LogManager.Instance.LogWarning("This etactics is null,EtName===" + m_kAttackType.ToString());
            return null;
        }
        _r = GetETZone(_data); ;
        return _r;
    }

    /// <summary>
    /// 根据战术类型确定区域,就是AttakDirection
    /// </summary>
    /// <returns></returns>
    private Region SelecteEtZone()
    {
        Region _r;
        AttackTacticalData _data = TableManager.Instance.AttackTacticalConfig.GetDataById((int)m_kAttackDir+3);
        if (_data == null)
        {
            LogManager.Instance.LogWarning("This etactics is null,EtName===" + m_kAttackType.ToString());
            return null;
        }
        _r = GetETZone(_data); ;
        return _r;
    }

    private Region GetETZone(AttackTacticalData _data)
    {
        Vector3D _leftV = Vector3D.zero;
        Vector3D _forwardV = Vector3D.zero;
        Vector3D _BackV = Vector3D.zero;
        Vector3D _RightV = Vector3D.zero;
        Region _r;
        double _length = 2 * Math.Abs(m_kScene.Region.Left);
        double _width = 2 * Math.Abs(m_kScene.Region.Top);
        if (m_kBallController.Team.TeamColor == ETeamColor.Team_Red)
        {
            _leftV = new Vector3D(-_data.leftparam * _width + m_kBallController.GetPosition().X, m_kBallController.GetPosition().Y, m_kBallController.GetPosition().Z);
            _RightV = new Vector3D(_data.rightparam * _width + m_kBallController.GetPosition().X, m_kBallController.GetPosition().Y, m_kBallController.GetPosition().Z);
            _forwardV = new Vector3D(m_kBallController.GetPosition().X, m_kBallController.GetPosition().Y, _data.forwardparam * _length + m_kBallController.GetPosition().Z);
            _BackV = new Vector3D(m_kBallController.GetPosition().X, m_kBallController.GetPosition().Y, -_data.backwardparam * _length + m_kBallController.GetPosition().Z);
            _r = new Region(_leftV.X, _BackV.Z, _RightV.X, _forwardV.Z);
        }
        else
        {
            _leftV = new Vector3D(_data.leftparam * _width + m_kBallController.GetPosition().X, m_kBallController.GetPosition().Y, m_kBallController.GetPosition().Z);
            _RightV = new Vector3D(-_data.rightparam * _width + m_kBallController.GetPosition().X, m_kBallController.GetPosition().Y, m_kBallController.GetPosition().Z);
            _forwardV = new Vector3D(m_kBallController.GetPosition().X, m_kBallController.GetPosition().Y, -_data.forwardparam * _length + m_kBallController.GetPosition().Z);
            _BackV = new Vector3D(m_kBallController.GetPosition().X, m_kBallController.GetPosition().Y, _data.backwardparam * _length + m_kBallController.GetPosition().Z);
            _r = new Region(_RightV.X, _forwardV.Z, _leftV.X, _BackV.Z);
        }
        return _r;
    }



    [Conditional("GAME_AI_ONLY")]
    private void DrawLine(List<LLPlayer> _players, LLPlayer _Splayer, LLPlayer _Pplayer)
    {
        if (SelectPlayerGUI.instance != null)
            SelectPlayerGUI.instance.BeginShow(_players, _Splayer, _Pplayer);
    }
    #endregion

    #region 接球者位置得分
    private double GetPlayerPosScore(LLPlayer _player)
    {
        double _percent = 0;
        int _region = _player.RegionID - 1;
        AICfgItem kCfgItem = TableManager.Instance.AIConfig.GetItem("fit_coefficient");
        double _param = kCfgItem.Value;
        TacticalPosCoefficientData _data = TableManager.Instance.TacticalPosCoefficientDataConfig.GetItem(_region);
        if (_data != null)
        {
            _percent = _data.GetFieldPercent((int)_player.PlayerBaseInfo.Career);
        }
        else
            LogManager.Instance.RedLog("this regionId is wrong ,this id===" + _region);

        return _percent * _param * 100;
    }

    #endregion

    #region  出球难度
    private double GetPassParam()
    {
        int _count = 0;
        if (BallController != null)
            _count = GetCloseMarkDefenderCount(this.BallController);
        double _percent = TableManager.Instance.MarkCoefficientDataConfig.GetItem(_count);
        AICfgItem kItem = TableManager.Instance.AIConfig.GetItem("mark_coefficient");
        return _percent * kItem.Value;
    }
    #region

    #region 接球难度
    private double GetGeterParam(LLPlayer _player)
    {
        int _count = 0;
        _count = GetCloseMarkDefenderCount(_player);
        double _percent = TableManager.Instance.InterceptCoefficientDataConfig.GetItem(_count);
        AICfgItem kItem = TableManager.Instance.AIConfig.GetItem("intercept_coefficient");
        return _percent * kItem.Value;
    }

    #endregion
   
    private LLPlayer SelectPlayerUnderBackRegion(int iTopLeftID, int iRightBottomID)
    {
        Vector3D kPos = m_kBallController.GetPosition();
        double dMinDist = double.MaxValue;
        LLPlayer kPlayer = null;

        for (int i = 0; i < m_kPlayerList.Count; i++)
        {
            if (m_kBallController == m_kPlayerList[i])
                continue;

            int iRegionID = m_kPlayerList[i].RegionID;
            if (iTopLeftID > iRegionID)
                continue;
            if (iRightBottomID < iRegionID)
                continue;
            double dDist = kPos.Distance(m_kPlayerList[i].GetPosition());
            if (dDist < dMinDist)
                kPlayer = m_kPlayerList[i];
        }
        if (null == kPlayer)
        {
            LogManager.Instance.Log("后场选球员: 没有球员选中");
            return m_kBallController;
        }
        return kPlayer;
    }

    private LLPlayer SelectPlayerUnderFrontRegion()
    {
        Random kRandom = new Random();
        int iRandVal = kRandom.Next(0, 100);
        double fDeltaZ = 13.14;
        Vector3D kPos = m_kBallController.GetPosition();
        List<LLPlayer> kPlayerList = new List<LLPlayer>();
        for (int i = 0; i < m_kPlayerList.Count; i++)
        {
            if (m_kBallController == m_kPlayerList[i])
                continue;
            if (iRandVal < 90)
            {
                if (Math.Abs(m_kPlayerList[i].GetPosition().Z - kPos.Z) <= fDeltaZ)
                    kPlayerList.Add(m_kPlayerList[i]);
            }
            else
            {
                if (Math.Abs(m_kPlayerList[i].GetPosition().Z - kPos.Z) > fDeltaZ)
                    kPlayerList.Add(m_kPlayerList[i]);
            }
        }

        if (0 == kPlayerList.Count)
        {
            LogManager.Instance.RedLog("前场选球员: 没有球员选中");
            return m_kBallController;
        }
        LLPlayer kPlayer = null;
        double dMinDist = double.MaxValue;
        for (int i = 0; i < kPlayerList.Count; i++)
        {
            double dDist = Math.Abs(kPlayerList[i].GetPosition().X - kPos.X);
            if (dDist < dMinDist)
                kPlayer = kPlayerList[i];
        }
        if (null == kPlayer)
        {
            LogManager.Instance.RedLog("前场选球员: 没有球员选中");
            return m_kBallController;
        }
        return kPlayer;
    }


    // 选择开球球员
    public LLPlayer SelectKickOffPlayer()
    {
        //int iIdx = m_kMidKickUnitList[0];
        //if (iIdx >= m_kPlayerList.Count)
        //    return null;
        //m_kBallController = m_kPlayerList[iIdx];
        //m_kBallController = m_kPlayerList[m_kPlayerList.Count - 1];
        if (null == m_kBattlePosItem || m_kBattlePosItem.m_MidlleKickIndex < 0 || m_kBattlePosItem.m_MidlleKickIndex >= m_kPlayerList.Count)
            return null;
        m_kBallController = m_kPlayerList[m_kBattlePosItem.m_MidlleKickIndex];
        if (null == m_kBallController)
            return null;
        m_kBallController.SetBallCtrl(true);
        LogManager.Instance.RedLog("SelectKickOffPlayer m_kBallController" + m_kBallController.PlayerBaseInfo.HeroID);
        return m_kBallController;
    }

    public LLPlayer SelectRandomPlayerInRegin()
    {
        GroundAreaItem kThrowArea = TableManager.Instance.GroundAreaTable.GetItem("throw");

        int left_top = kThrowArea.Area.X;
        int right_bottom = kThrowArea.Area.Y;
        List<LLPlayer> candidate = new List<LLPlayer>();
        for (int i = 0; i < m_kPlayerList.Count; ++i)
        {
            LLPlayer player = m_kPlayerList[i];
            if (ECareer.BackFielder == player.PlayerBaseInfo.Career)
            //   if (player.RegionID <= right_bottom && player.RegionID >= left_top)
            {
                candidate.Add(player);
            }
        }

        if (candidate.Count > 0)
        {
            int idx = (int)FIFARandom.GetRandomValue(0, candidate.Count);
            var player = candidate[idx];
            candidate.Clear();
            candidate = null;
            return player;
        }

        return null;
    }
    #endregion
    #endregion

    #endregion

    private Vector3D GetRandomHomePosition()
    {
        int _radius = (int)TableManager.Instance.AIConfig.GetItem("homeposition_random_radius").Value;
        double _x = FIFARandom.GetRandomValue(-_radius, _radius);
        double _z = FIFARandom.GetRandomValue(-_radius, _radius);
        return new Vector3D(_x, 0, _z);
    }


    /// <summary>
    /// 协助盯防球员进行对于跑位，当更新HOMEPOSITION
    /// </summary>
    private void SetSupplyMarkBaller()
    {
        LLPlayer _bPlayer = m_kScene.GetBallControler();
        if (_bPlayer == null)
            return;
        double _reinforce_num = TableManager.Instance.AIConfig.GetItem("wrap_number").Value;
        double _reinfore_param = TableManager.Instance.AIConfig.GetItem("wrap_coefficient").Value;
        Dictionary<LLPlayer, double> _ballDistance = new Dictionary<LLPlayer, double>();
        double _dis = 0f;
        for (int i = 0; i < m_kPlayerList.Count; i++)
        {
            m_kPlayerList[i].ForceHomePostionClose = false;
            _dis = m_kPlayerList[i].GetPosition().Distance(_bPlayer.GetPosition());
            _ballDistance.Add(m_kPlayerList[i], _dis);
        }

        if (_ballDistance.Count <= 0)
            return;
        List<KeyValuePair<LLPlayer, double>> _list = new List<KeyValuePair<LLPlayer, double>>(_ballDistance);
        _list.Sort(delegate(KeyValuePair<LLPlayer, double> l1, KeyValuePair<LLPlayer, double> l2)
        {
            return l1.Value.CompareTo(l2.Value);
        });
        _ballDistance.Clear();
        List<KeyValuePair<LLPlayer, double>> _players = new List<KeyValuePair<LLPlayer, double>>();
        if (_ballDistance != null)
        {
            //取出_reinforce_num个离求最近的_reinforce_num个点 然后去更新home postion
            for (int i = 0; i < _list.Count; i++)
            {
                if (i < _reinforce_num)
                {
                    _players.Add(_list[i]);
                }
            }
        }
        double _param = 1f;
        for (int i = 0; i < _players.Count; i++)
        {
            double _x = _bPlayer.GetPosition().X - _players[i].Key.GetPosition().X;
            double _z = _bPlayer.GetPosition().Z - _players[i].Key.GetPosition().Z;
            _x *= _reinfore_param * 0.1f * _param;
            _z *= _reinfore_param * 0.1f * _param;
            _players[i].Key.ActionHomePosition = new Vector3D(_x, 0, _z);
        }
    }
    /// <summary>
    /// 协助持球球员进行对于跑位，当更新HOMEPOSITION
    /// </summary>
    private void SetSupplyAttackBaller()
    {
        LLPlayer _bPlayer = m_kScene.GetBallControler();
        if (_bPlayer == null)
            return;
        double _reinforce_num = TableManager.Instance.AIConfig.GetItem("reinforce_number").Value;
        double _reinfore_param = TableManager.Instance.AIConfig.GetItem("reinforce_coefficient").Value;
        Dictionary<LLPlayer, double> _ballDistance = new Dictionary<LLPlayer, double>();
        double _dis = 0f;
        for (int i = 0; i < m_kPlayerList.Count; i++)
        {
            m_kPlayerList[i].ForceHomePostionClose = false;
            _dis = m_kPlayerList[i].GetPosition().Distance(_bPlayer.GetPosition());
            _ballDistance.Add(m_kPlayerList[i], _dis);
        }
        if (_ballDistance.Count <= 0)
            return;
        List<KeyValuePair<LLPlayer, double>> _list = new List<KeyValuePair<LLPlayer, double>>(_ballDistance);
        _list.Sort(delegate(KeyValuePair<LLPlayer, double> l1, KeyValuePair<LLPlayer, double> l2)
        {
            return l1.Value.CompareTo(l2.Value);
        });
        _ballDistance.Clear();
        List<KeyValuePair<LLPlayer, double>> _players = new List<KeyValuePair<LLPlayer, double>>();
        if (_ballDistance != null)
        {
            //取出_reinforce_num个离求最近的四_reinforce_num点 然后去更新home postion
            for (int i = 0; i < _list.Count; i++)
            {
                if (i < _reinforce_num/*&&_list[i].Value < 4f*/)
                {
                    _players.Add(_list[i]);

                }
            }
        }
        double _param = 1f;
        for (int i = 0; i < _players.Count; i++)
        {
            double _x = _bPlayer.GetPosition().X - _players[i].Key.GetPosition().X;
            double _z = _bPlayer.GetPosition().Z - _players[i].Key.GetPosition().Z;
            _x *= _reinfore_param * 0.1f * _param;
            _z *= _reinfore_param * 0.1f * _param;
            _players[i].Key.ActionHomePosition = new Vector3D(_x, 0, _z);
        }

    }

    /// <summary>
    /// 获得随机的HomePosition偏移值
    /// </summary>
    private void SetRandomHomePosition()
    {
        for (int i = 0; i < m_kPlayerList.Count; i++)
        {
            if (m_kBallController == null || m_kBallController == m_kPlayerList[i])
                continue;
            m_kPlayerList[i].RandomHomePosition = GetRandomHomePosition();
        }
    }

    public void RefreshTeamHomePosition()
    {
        for (int i = 0; i < m_kPlayerList.Count; i++)
        {
            m_kPlayerList[i].ForceHomePostionClose = false;
        }
        if (m_kState == ETeamState.TS_ATTACK)
            SetSupplyAttackBaller();
        else
            SetSupplyMarkBaller();

        BPositionLogic.ResetBaseHomepositionDeltx();
        SetRandomHomePosition();
    }

    public void InitEightDirs()
    {
        double radius = TableManager.Instance.AIConfig.GetItem("dribble_search_radius").Value;
        float angle = 360f / mEightDirs.Length;
        //0 for forward, 1 for left-forward, -1 for right-forward and so on
        int[] factor = new[] { 0, 1, -1, 2, -2, 3, -3, 4 };

        for (int i = 0; i < factor.Length; ++i)
        {
            int idx = (m_kTeamColor == ETeamColor.Team_Red) ? i : factor.Length - i - 1;
            double radiam = angle * factor[idx] * Math.PI / 180;
            Vector3D dir = Vector3D.forward * Math.Cos(radiam) - Vector3D.right * Math.Sin(radiam);
            mEightDirs[i] = new Sector(radius, angle, dir);
        }
    }

    /// <summary>
    // 0 for forward, 1 for left-forward, -1 for right-forward and so on
    // such as : int[] factor = new [] { 0, 1, -1, 2, -2, 3, -3, 4 };.
    /// </summary>
    /// <returns>The dribble dir.</returns>
    /// <param name="possibleDirs">Possible dirs.</param>
    public Vector3D SelectDribbleDir(int[] possibleDirs = null)
    {
        // 1.初始化8方向带球概率
        for (int i = 0; i < mDribbleRates.Length; ++i)
        {
            mDribbleRates[i] = 0;
        }

        m_kDribblePrData.HeroID = (int)m_kBallController.PlayerBaseInfo.HeroID;
        m_kDribblePrData.RegionID = m_kBallController.RegionID;
        m_kDribblePrData.Clear();
        var center = BallController.GetPosition();
        // 带球中进攻战术系数
        double factor1 = TableManager.Instance.AIConfig.GetItem("dribble_tactical_coefficient").Value;
        var attTacData = TableManager.Instance.AttackTacticalConfig.GetDataById((int)m_kAttackType);   // 读取进攻战术表
        for (int i = 0; i < mDribbleRates.Length; ++i)
        {
            int value = attTacData.mEightRate[i];
            mDribbleRates[i] += value * factor1;
            m_kDribblePrData.Tactics.Add(value); // 战术分
        }

        //带球中球员职业系数
        double factor2 = TableManager.Instance.AIConfig.GetItem("dribble_pro_coefficient").Value;

        // 球员职业
        var pro = TableManager.Instance.HeroTbl.GetItem((int)BallController.PlayerBaseInfo.HeroID).Pos;
        m_kDribblePrData.Career = pro;      // 球员职业
        var group = (string)TableManager.Instance.GetProperty("profession", pro.ToString(), "group");
        for (int i = 0; i < mDribbleRates.Length; ++i)
        {
            var regionId = Scene.GetRegionID(mEightDirs[i].PointOnDir + center);
            if (regionId <= 40 && regionId >= 1)
            {
                var realRegionId = m_kTeamColor == ETeamColor.Team_Red ? regionId : 40 + 1 - regionId;
                var value = TableManager.Instance.hotSpot.GetValue(realRegionId.ToString(), string.Format("group_id_{0}", group));
                mDribbleRates[i] += factor2 * value;
                m_kDribblePrData.CareerScore.Add(value);        // 职业得分
            }
            else
            {
                mDribbleRates[i] = double.MinValue;
                m_kDribblePrData.CareerScore.Add(0);            // 职业得分
            }
        }

        // 计算区域内球员密度
        for (int i = 0; i < mEightDirs.Length; ++i)
        {
            int inAreaNum = 0;
            var playerList = Opponent.PlayerList;
            for (int j = 0; j < playerList.Count; ++j)
            {
                if (mEightDirs[i].InArea(playerList[j].GetPosition(), center))
                {
                    ++inAreaNum;
                }
            }
            var factor3 = TableManager.Instance.defenceDensity.GetValue(inAreaNum.ToString());
            mDribbleRates[i] *= factor3;
            m_kDribblePrData.Density.Add(factor3);
        }

        int idx = 0;
        double mostPossible = double.MinValue;

        int[] possibleIndexes = null;
        if (possibleDirs != null)
        {
            possibleIndexes = new int[possibleDirs.Length];
            int[] dirArray = new[] { 0, 1, -1, 2, -2, 3, -3, 4 };
            for (int i = 0; i < possibleDirs.Length; i++)
            {
                for (int j = 0; j < dirArray.Length; j++)
                {
                    if (possibleDirs[i] == dirArray[j])
                    {
                        possibleIndexes[i] = j;
                        break;
                    }
                }
            }
        }
        for (int i = 0; i < mDribbleRates.Length; ++i)
        {
            m_kDribblePrData.Score.Add(mDribbleRates[i]);

            if (possibleIndexes != null)
            {
                bool isPossible = false;
                for (int j = 0; j < possibleIndexes.Length; j++)
                {
                    if (i == possibleIndexes[j])
                    {
                        isPossible = true;
                        break;
                    }
                }
                if (!isPossible)
                    continue;
            }
            if (mDribbleRates[i] >= mostPossible)
            {
                mostPossible = mDribbleRates[i];
                idx = i;
            }
        }
        return mEightDirs[idx].Dir;
    }

    public void SetTeamPlayerAIState(bool isEnable)
    {
        if (PlayerList != null && PlayerList.Count > 0)
        {
            for(int i = 0;i<PlayerList.Count;i++)
            {
                PlayerList[i].AIEnable = isEnable;
            }
        }
    }

    #region Property
    public ShootData ShootData
    {
        get { return m_kShootData; }
        set { m_kShootData = value; }
    }

    public double GuardLine
    {
        get { return m_fGuardLine; }
        set { m_fGuardLine = value; }
    }
    public double LineDist
    {
        get { return m_fLineDist; }
        set { m_fLineDist = value; }
    }
    public ETeamColor TeamColor
    {
        get { return m_kTeamColor; }

    }

    public ETeamState State
    {
        get { return m_kState; }
        set { m_kState = value; }
    }

    public LLScene Scene
    {
        get { return m_kScene; }
        set { m_kScene = value; }
    }

    public LLGoal Goal
    {
        get { return m_kGoal; }
        set { m_kGoal = value; }
    }

    public bool ReadyForChangeState
    {
        get { return m_bReadyForChangeState; }
        set { m_bReadyForChangeState = value; }
    }



    public List<LLPlayer> PlayerList
    {
        get { return m_kPlayerList; }

    }

    public LLTeam Opponent
    {
        get { return m_kOpponentTeam; }
        set { m_kOpponentTeam = value; }
    }

    public LLPlayer BallController
    {
        get { return m_kBallController; }
        set { m_kBallController = value; }
    }

    public LLGoalKeeper GoalKeeper
    {
        get { return m_kGoalKeeper; }
        set { m_kGoalKeeper = value; }
    }

    public List<LLPlayer> BallMarkList
    {
        get { return m_kBallMarkList; }

    }

    // 中场开球传球次数
    //public uint MidKickOffPassTime
    //{
    //    get { return m_iKickUnitIdx; }
    //    set { m_iKickUnitIdx = value; }
    //}

    public NSShortPass NS_ShortPass
    {
        get { return m_NSShortPass; }
    }

    public NSLongPass NS_LongPass
    {
        get { return m_NSLongPass; }
    }

    public NSShoot NS_Shoot
    {
        get { return m_NSShoot; }
    }

    public NSBreak NS_Break
    {
        get { return m_NSBreak; }
    }

    public NSSnatch NS_Snatch
    {
        get { return m_NSSnatch; }
    }

    public NSTackle NS_Tackle
    {
        get { return m_NSTackle; }
    }
    public NSBlock NS_Block
    {
        get { return m_NSBlock; }
    }

    public TeamInfo TeamInfo
    {
        get { return m_kTeamInfo; }
        set { m_kTeamInfo = value; }
    }

    public DribblePrData DribblePrData
    {
        get { return m_kDribblePrData; }
        set { m_kDribblePrData = value; }
    }
    public BattlePositionLogic BPositionLogic
    {
        get
        {
            return m_kBattlePosLogic;
        }
        set
        {
            m_kBattlePosLogic = value;
        }
    }

    public bool MidKickFinished
    {
        get
        {
            if (null == m_kBattlePosItem)
                return true;
            return m_iKickUnitIdx >= m_kBattlePosItem.m_MiddleKickList.Count;
        }
    }

    public AttackType AttackType
    {
        get { return m_kAttackType; }
        set { m_kAttackType = value; }
    }
    public AttackDirection AttackDir
    {
        get { return m_kAttackDir; }
        set { m_kAttackDir = value; }
    }
    public AttackChoice AttackChoice
    {
        get { return m_kAttackChoice; }
        set { m_kAttackChoice = value; }
    }
    public double HomepositionParam
    {
        get { return m_homepositionParam; }
        set 
        {
            m_homepositionParam = value;
        }
    }
    #endregion
    #region Member
    private LLScene m_kScene;
    private ETeamColor m_kTeamColor;
    private ETeamState m_kState;         // 进攻或是防守

    private LLPlayer m_kBallController = null;
    private double m_fGuardLine = 0.0f;
    private double m_fLineDist = 0.0f;
    private bool m_bReadyForChangeState = false;        // 申请更换状态
    private BTree m_kTeamAI = new BTree();
    private List<LLPlayer> m_kPlayerList = new List<LLPlayer>();
    private LLGoalKeeper m_kGoalKeeper = null;
    private LLTeam m_kOpponentTeam = null;
    private LLGoal m_kGoal = null;              //the goal we are defending
    private List<LLPlayer> m_kBallMarkList = new List<LLPlayer>();
    private Sector[] mEightDirs = new Sector[8];
    private double[] mDribbleRates = new double[8];         // 8 方向带球概率
    private TeamInfo m_kTeamInfo = new TeamInfo();
    private ShootData m_kShootData = new ShootData();
    private BattlePosItem m_kBattlePosItem = null;  // 中场开球控球方信息
    private int m_iKickUnitIdx = 0;                // 当前接球人员索引值
    
    private Dictionary<LLPlayer, LLPlayer> m_CloseMarkWithoutBallArrangement = new Dictionary<LLPlayer, LLPlayer>();
    private Dictionary<LLPlayer, Vector3D> m_AttackSupportArrangement = new Dictionary<LLPlayer, Vector3D>();
    #region NSLogics
    private NSShortPass m_NSShortPass = new NSShortPass();
    private NSLongPass m_NSLongPass = new NSLongPass();
    private NSShoot m_NSShoot = new NSShoot();
    private NSBreak m_NSBreak = new NSBreak();
    private NSSnatch m_NSSnatch = new NSSnatch();
    private NSTackle m_NSTackle = new NSTackle();
    private NSBlock m_NSBlock = new NSBlock();
    #endregion

    #region 选人和homeposition策略
    private AttackType m_kAttackType = AttackType.All_Attack;
    private AttackDirection m_kAttackDir = AttackDirection.Side_Middle;
    private AttackChoice m_kAttackChoice = AttackChoice.Long_Short_Combine;
    private double m_homepositionParam = 0d;
    #endregion
    private DribblePrData m_kDribblePrData = new DribblePrData();
    private BattlePositionLogic m_kBattlePosLogic = null;           // 球场上球员跑位逻辑代码
    #endregion
}
