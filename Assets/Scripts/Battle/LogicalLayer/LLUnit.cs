using BehaviourTree;
using Common;
using Common.Log;
using Common.Tables;
using System;
using System.Collections.Generic;

/*
 * 球员与守门员的继承对象
 */
public class LLUnit : LLEntity
{
    public class LogicalStateData
    {
        public Vector3D targetPos;              // 通用出球目标点
        public Vector3D targetBallPos1;         // 守门员摆球开球，该参数表示球的目标位置//
        public Vector3D targetBallOutPos;       // 出球目标点,仅用于头球争顶//
        public double ballFlyingTime;
        public bool shootRotateFlag;
        public double playerSpeed;
        public bool headRobAvil = false;      //控制头球争顶在胸部头球情况下，targetBallOutPos无效//
    }

    public LLUnit(PlayerInfo kInfo)
    {
        m_kBaseInfo = kInfo;
        m_kAniStateMgr = new AniStateManager(this);
        m_dampEnergyTime = (int)TableManager.Instance.AIConfig.GetItem("energy_delta_time").Value;
        m_dampEnergyValue = (int)TableManager.Instance.AIConfig.GetItem("energy_delta_value").Value;

        m_CatchBallMaxEnergy = TableManager.Instance.AIConfig.GetItem("pass_pick_value").Value;
        m_CatchBallEnergy = m_CatchBallMaxEnergy;
        m_CatchBallMinEnergyNeeded = 5d;
        m_CatchBallRecoverEnergy = 1d;
        m_CatchBallDecreaseEnergy = TableManager.Instance.AIConfig.GetItem("catch_ball_consume").Value;


        m_dVelocity = TableManager.Instance.AIConfig.GetItem("dribble_velocity").Value;
        m_dBaseVelocity = m_dVelocity;
        m_kSkillPlayer = new SkillPlayer(this);
    }
    public void SetAniState(EAniState kState)
    {
        if (null == m_kAniStateMgr)
            return;

        //        uint targetID = 0;
        //        for(int i=0;i<Team.PlayerList.Count;i++)
        //        {
        //            if(Team.PlayerList[i].MarkingStatus == EMarkStatus.MARKWITHBALL)
        //            {
        //                targetID = Team.PlayerList[i].m_kBaseInfo.HeroID;
        //                break;
        //            }
        //
        //        }
        //        if (Team.State == ETeamState.TS_DEFEND && PlayerBaseInfo.HeroID == targetID)
        //        {
        //            LogManager.Instance.BlackLog(PlayerBaseInfo.HeroID + " MARKWITHBALL "+ AniStateMgr.CurState+" -> "+kState);
        //        }





        m_kAniStateMgr.SetState(kState);
    }

    public override void Update(double dTime)
    {
        m_kSkillPlayer.Update(dTime);
        m_kAniStateMgr.Update(dTime);
        UpdateCalcEnergy(dTime);
        UpdateCatchBallEnergy(dTime);
    }

    private void UpdateCalcEnergy(double dTime_)
    {
        if (m_lastCalcTime > m_dampEnergyTime)
        {
            if (m_kBaseInfo.Energy > 0)
                m_kBaseInfo.Energy -= m_dampEnergyValue;

            m_lastCalcTime = 0;
        }
        else
        {
            m_lastCalcTime += dTime_;
        }
    }

    private void UpdateCatchBallEnergy(double dTime)
    {
        double energyIncrease = dTime * m_CatchBallRecoverEnergy;
        if (m_CatchBallEnergy + energyIncrease > m_CatchBallMaxEnergy)
        {
            m_CatchBallEnergy = m_CatchBallMaxEnergy;
        }
        else
        {
            m_CatchBallEnergy += energyIncrease;
        }
    }

    public bool EnoughCatchBallEnergy()
    {
        return m_CatchBallEnergy >= m_CatchBallMinEnergyNeeded;
    }

    public void DecreaseCatchBallEnergy()
    {
        m_CatchBallEnergy = m_CatchBallEnergy <= m_CatchBallDecreaseEnergy ? 0 : m_CatchBallEnergy - m_CatchBallDecreaseEnergy;
    }

    public void ResetCatchBallEnergy()
    {
        m_CatchBallEnergy = m_CatchBallMaxEnergy;
    }

    public override void Destroy()
    {
        RemovePlayerMessage kMsg = new RemovePlayerMessage(m_kTeam.TeamColor, this, false);
        MessageDispatcher.Instance.SendMessage(kMsg);
    }

    public void CastSkill(EEventType kEvtType)
    {
        if (null == m_kSkillPlayer)
            return;
        m_kSkillPlayer.CastSkill(kEvtType);
    }

    public void ResetAI()
    {
        if (null == m_kPlayerAI)
            return;
        m_kPlayerAI.Reset();
    }

    public AniStateManager AniStateMgr
    {
        get { return m_kAniStateMgr; }
    }

    public void ResetBallInState(bool _ballIn)
    {
        m_bIsBallIn = _ballIn;
    }
    public void ResetBallOutState(bool _ballIn)
    {
        m_bIsBallIn = _ballIn;
    }

    public void SetBallCtrl(bool bFlag)
    {
        m_bCtrlBall = bFlag;
    }

    public void MoveByDir(Vector3D moveDir, double dTime)
    {
        Vector3D movePos = GetPosition() + DirAdjustment(moveDir) * Velocity * dTime;
        SetPosition(movePos);
    }

    public virtual void MoveToPos(Vector3D targetPos, double dTime)
    {
        if (!CanMoveNext)
            return;
        Vector3D _pos = OffsideFieldJudge(targetPos);
        if(_pos==targetPos)
        {
            if (GetPosition().Distance(targetPos) > Velocity * dTime)
            {
                //move it
                Vector3D movePos = GetPosition() + DirAdjustment(MathUtil.GetDir(GetPosition(), targetPos)) * Velocity * dTime;
                SetPosition(movePos);
                //强制令球员转向进入到目标点//
                double dAngle = MathUtil.GetAngle(GetPosition(), targetPos);
                if ((Math.Abs(GetRotAngle() - dAngle)+360)%360 > 5f)
                    SetRoteAngle(dAngle);
            }
            else
            {
                //arrived
                SetPosition(targetPos);
            }
        }
        else
        {
             (this as LLPlayer).TargetPos = OffsideFieldJudge(targetPos);
        }

    }

    public void UpdateHomePosition()
    {
        if (null == m_kTeam)
            return;
        m_kHomePos = m_baseHomePosition + m_randomHomePosition + m_ActionHomePosition;
        m_kHomePos = OffsideFieldJudge(m_kHomePos);
        if (m_ActionHomePosition != Vector3D.zero)
        {
            m_forceHomePosition = true;
        }
        if (!m_forceHomePosition)
            m_ActionHomePosition = Vector3D.zero;
    }

    public bool ContainSkill(int iSkillID)
    {
        for (int i = 0; i < PlayerBaseInfo.SkillList.Count - 1; i++)
        {
            if (PlayerBaseInfo.SkillList[i].ID == iSkillID)
                return true;
        }
        return false;
    }



    /// <summary>
    /// 矫正homeposition是否越位
    /// </summary>
    /// <param name="_pos"></param>
    /// <returns></returns>
    public Vector3D OffsideFieldJudge(Vector3D _pos)
    {
        double _width = 34.29d/* Math.Abs(m_kTeam.Scene.Region.Left)*/;
        double _length = 52.75d/*Math.Abs(m_kTeam.Scene.Region.Right)*/;
        if (Math.Abs(_pos.X) >= _width)
        {
            if (_pos.X <= 0)
            {
                _pos.X = -(_width - 1f);
            }
            else
                _pos.X = (_width - 1f);
        }
        if (Math.Abs(_pos.Z) >= _length)
        {
            if (_pos.Z <= 0)
            {
                _pos.Z = -(_length - 1f);
            }
            else
                _pos.Z = (_length - 1f);
        }
        return new Vector3D(_pos.X, _pos.Y, _pos.Z);
    }

    public Vector3D DirAdjustment(Vector3D originalDir)
    {
        originalDir.Y = 0;
        return originalDir.Normalize();
    }

    public bool IsCtrlBall
    {
        get { return m_bCtrlBall; }

    }
    public Vector3D HomePos
    {
        get { return m_kHomePos; }
        set { m_kHomePos = value; }
    }
    public ETeamColor TeamType
    {
        get { return m_kTeam.TeamColor; }
    }

    public LLTeam Team
    {
        get { return m_kTeam; }
    }

    public int RegionID
    {
        get { return m_iRegionID; }
        set { m_iRegionID = value; }
    }

    public double Velocity
    {
        get { return m_dVelocity; }
        set { m_dVelocity = value; }
    }

    public double BaseVelocity
    {
        get { return m_dBaseVelocity; }
    }

    public bool IsBallIn
    {
        get { return m_bIsBallIn; }
        set { m_bIsBallIn = value; }
    }
    public bool IsBallOut
    {
        get { return m_bIsBallOut; }
        set { m_bIsBallOut = value; }
    }
    public bool CanUpdateRotate
    {
        get { return m_bRotCanUpdate; }
        set { m_bRotCanUpdate = value; }
    }
    public PlayerInfo PlayerBaseInfo
    {
        get { return m_kBaseInfo; }
        set { m_kBaseInfo = value; }
    }

    public AniStateManager StateMgr
    {
        get { return m_kAniStateMgr; }
    }

    public bool AniFinish
    {
        set { m_bFinishAnimation = value; }
        get { return m_bFinishAnimation; }
    }

    public LogicalStateData KAniData
    {
        set { m_kAnimData = value; }
        get { return m_kAnimData; }
    }

    public bool SkillBegin
    {
        set { m_bSkillBegin = value; }
        get { return m_bSkillBegin; }
    }

    /// <summary>
    /// 表示是否需要修改动画组合ID
    /// </summary>
    public bool IsChangeAniID
    {
        get { return m_bChangeAniIDList; }
        set { m_bChangeAniIDList = value; }
    }

    /// <summary>
    /// 技能中角度差
    /// </summary>
    public double SkillChangeAngle
    {
        get { return m_dSkillChangeAngle; }
        set { m_dSkillChangeAngle = value; }
    }
    /// <summary>
    /// 表示组合动画ID,如果为null表示无效值
    /// </summary>
    public SkillAppearItem SkillAniItem
    {
        get { return m_iSACIDItemInfo; }
        set { m_iSACIDItemInfo = value; }
    }


    public bool ForceHomePostionClose
    {
        get
        {
            return m_forceHomePosition;
        }
        set
        {
            m_forceHomePosition = value;
        }
    }
    public Vector3D BaseHomeposition
    {
        get { return m_baseHomePosition; }
        set { m_baseHomePosition = value; }
    }
    public Vector3D RandomHomePosition
    {
        get { return m_randomHomePosition; }
        set { m_randomHomePosition = value; }
    }
    public Vector3D ActionHomePosition
    {
        get { return m_ActionHomePosition; }
        set { m_ActionHomePosition = value; }
    }

    protected LLTeam m_kTeam;
    protected BTree m_kPlayerAI = new BTree();
    protected bool m_bRotCanUpdate = true;

    private double m_dVelocity = 6.5d;
    private double m_dBaseVelocity = 6.5d;
    private bool m_bIsBallIn = false;
    private bool m_bIsBallOut = false;
    private LogicalStateData m_kAnimData = new LogicalStateData();

    private PlayerInfo m_kBaseInfo;
    private AniStateManager m_kAniStateMgr = null;
    private bool m_bFinishAnimation = false;
    private int m_dampEnergyTime;   // 体力衰减间隔
    private int m_dampEnergyValue; // 体力衰减数值
    private double m_lastCalcTime = 0;  // 上一次计算体力的时间
    //catch ball protection
    private double m_CatchBallEnergy = 10d;
    private double m_CatchBallMaxEnergy = 10d;
    private double m_CatchBallMinEnergyNeeded = 5d;
    private double m_CatchBallRecoverEnergy = 1d;
    private double m_CatchBallDecreaseEnergy = 1d;

    private bool m_bCtrlBall = false;       // 是否控球
    private SkillPlayer m_kSkillPlayer = null;
    private int m_iRegionID = 0;
    private bool m_bChangeAniIDList = false;            // 是否需要修改动画组合ID
    private SkillAppearItem m_iSACIDItemInfo = null;  // 技能动画列表
    private double m_dSkillChangeAngle; // 技能中角度差
    protected bool m_bSkillBegin = false;
    #region homePostion
    private Vector3D m_kHomePos = new Vector3D();
    private bool m_forceHomePosition = false;
    private Vector3D m_baseHomePosition = Vector3D.zero;
    private Vector3D m_randomHomePosition = Vector3D.zero;
    private Vector3D m_ActionHomePosition = Vector3D.zero;
    #endregion


    #region DebugInfo

    public bool ShowDebugInfo
    {
        get { return m_bShowDebugInfo; }
        set { m_bShowDebugInfo = value; }
    }
    public bool RedColor
    {
        get { return m_bRedColor; }
        set { m_bRedColor = value; }
    }
    private bool m_bRedColor = false;
    private bool m_bShowDebugInfo = false;
    #endregion
}