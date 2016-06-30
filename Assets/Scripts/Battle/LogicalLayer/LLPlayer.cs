using System;
using Common;
using Common.Log;
using BehaviourTree;
using System.Collections.Generic;

public struct StateExtraData
{
   public double ElapseTime;
   public EPlayerState State;
}

public class PerFrameData
{
    public EPlayerState State;
    public int FrameCount;
    public List<bool> ArivedTarget = new List<bool>();
    public List<int> FrameIDList = new List<int>();
}

public class LLPlayer : LLUnit
{
    public LLPlayer(LLTeam kTeam,PlayerInfo kBaseInfo)
        :base(kBaseInfo)
    {
        m_kTeam = kTeam;
        int iID = m_kPlayerAI.Database.GetDataID(BTConstant.Player);
        m_kPlayerAI.Database.SetData<LLPlayer>(iID, this);
        iID = m_kPlayerAI.Database.GetDataID(BTConstant.Team);
        m_kPlayerAI.Database.SetData<LLTeam>(iID, kTeam);
        m_kPlayerAI.Load("Tables/AI/PlayerAI");
        CreatePlayerMessage kMsg = new CreatePlayerMessage(this, kBaseInfo, false);
        MessageDispatcher.Instance.SendMessage(kMsg);
    }
    

    public override void Update(double dTime)
    {
        base.Update(dTime);
        UpdateOritation();
        if (null == m_kPlayerAI)
            return;
        if (m_bAIEnable)
            m_kPlayerAI.Update(dTime);
    }

    /*
     * 更新球员朝向
     */
    protected void UpdateOritation()
    {
//        if (m_bRotCanUpdate)
//        {
//            if (null == m_kTeam.Scene || null == m_kTeam.Scene.Ball)
//                return;
//            LLBall kBall = m_kTeam.Scene.Ball;
//            double fAngle = MathUtil.GetAngle(m_kPos, kBall.Position);
//            if (fAngle == 90.0 || fAngle == 270.0)
//            {
//                RotateAngle = RotateAngle;
//                return;
//            }
//            fAngle = Math.Abs(fAngle - RotateAngle);
//            if (fAngle > 45 && fAngle < 180)
//            {
//                // 调整角度
//                RotateAngle = RotateAngle + fAngle - 45;
//            }
//            else if (fAngle > 180 && fAngle < 315)
//            {
//                RotateAngle = (RotateAngle - (fAngle + 45) + 360) % 360;
//            }
//            else
//                RotateAngle = RotateAngle;
//        }
    }

    

    public void SetState(EPlayerState kState)
    {
//        if (Team.State == ETeamState.TS_DEFEND && MarkingStatus == EMarkStatus.MARKWITHBALL)
//        {
//            LogManager.Instance.YellowLog(PlayerBaseInfo.HeroID+" MARKWITHBALL State Change =====> "+m_kPlayerState+" --> "+kState);
//        }
//        if (Team.State == ETeamState.TS_DEFEND && MarkingStatus == EMarkStatus.MARKWITHOUTBALL)
//        {
//            LogManager.Instance.YellowLog(PlayerBaseInfo.HeroID+" MARKWITHOUTBALL State Change =====> "+m_kPlayerState+" --> "+kState);
//        }
//        if (Team.State == ETeamState.TS_ATTACK && IsCtrlBall )
//        {
//            LogManager.Instance.RedLog(PlayerBaseInfo.HeroID+" BallController State Change =====> "+m_kPlayerState+" --> "+kState);
//        }

        //m_kStopWatch.Stop();
        //StateExtraData kExtraData = new StateExtraData();
        //kExtraData.ElapseTime = m_kStopWatch.Elapsed.Seconds;
        //kExtraData.State = m_kPlayerState;
        //m_kStateList.Add(kExtraData);
        if (m_kPlayerState == EPlayerState.NormalDribble && kState == EPlayerState.NormalDribble)
            m_bContinueDribble = true;
        else
            m_bContinueDribble = false;
        m_kPlayerState = kState;
        //m_kStopWatch.Start();


    }

    public void ResetToDefendState()
    {
        SetBallCtrl(false);
        
        if (m_kStateChangeDelayedType == ETeamStateChangeDelayedType.DELAYED_ATTACK)
        {
            
        } 
        else
        {
            if (NextState != EPlayerState.PS_END)
            {
                SetState(NextState);
                NextState = EPlayerState.PS_END;
            } else
            {
                SetState(EPlayerState.HomePos);
            }
            ResetState();
        }
    }

    public void ResetToAttackState()
    {
        if (m_kStateChangeDelayedType == ETeamStateChangeDelayedType.DELAYED_DEFENCE)
        {
            LogManager.Instance.YellowLog("Player "+PlayerBaseInfo.HeroID+" Delayed Defence");
        }
        else
        {
            if (NextState != EPlayerState.PS_END)
            {
                SetState(NextState);
                NextState = EPlayerState.PS_END;
            }
            else if (IsCtrlBall)
            {
                SetState(EPlayerState.ActionSelect);
            }
            else
            {
                SetState(EPlayerState.HomePos);
            }
            ResetState();
        }
    }

    public void ResetDelayedState()
    {
        if(StateChangeDelayedType == ETeamStateChangeDelayedType.DELAYED_DEFENCE)
        {
            StateChangeDelayedType = ETeamStateChangeDelayedType.NONE;
            ResetToAttackState();
            Team.InformRefreshAttackSupport();
        }
        else if(StateChangeDelayedType == ETeamStateChangeDelayedType.DELAYED_ATTACK)
        {
            StateChangeDelayedType = ETeamStateChangeDelayedType.NONE;
            ResetToDefendState();
            //inform team to refresh closeMarking status for all players
            Team.InformRefreshCloseMark();      
        }
    }

    public void ResetState()
    {
        CanMoveNext = true;
        CanUpdateRotate = true;
        MarkingStatus = EMarkStatus.NONE;
        Opponent = null;
        StateChangeDelayedType = ETeamStateChangeDelayedType.NONE;
    }



    public bool ArrivedTargetPos()
    {
        if (m_kPos == m_kTargetPos)
        {
            return true;
        }
        return false;
    }

    public bool NewEventAssignable()
    {
        return StateChangeDelayedType == ETeamStateChangeDelayedType.NONE;
    }

    public bool InCloseMarkActions()
    {
        return (State == EPlayerState.Block_Tackle || 
                State == EPlayerState.Sliding_Tackle || 
                State == EPlayerState.Defend_Break_Through || 
                State == EPlayerState.Block_Tackle_Success || 
                State == EPlayerState.Block_Tackle_Failed || 
                State == EPlayerState.Sliding_Tackle_Success || 
                State == EPlayerState.Sliding_Tackle_Failed || 
                State == EPlayerState.Defend_Break_Through_Success || 
                State == EPlayerState.Defend_Break_Through_Failed);
    }

    public bool InDefensiveActions()
    {
        return (State == EPlayerState.Block_Tackle || 
                State == EPlayerState.Sliding_Tackle || 
                State == EPlayerState.Defend_Break_Through || 
                State == EPlayerState.Block_Tackle_Success || 
                State == EPlayerState.Block_Tackle_Failed || 
                State == EPlayerState.Sliding_Tackle_Success || 
                State == EPlayerState.Sliding_Tackle_Failed || 
                State == EPlayerState.Defend_Break_Through_Success || 
                State == EPlayerState.Defend_Break_Through_Failed || 
                State == EPlayerState.Intercept || 
                State == EPlayerState.Intercept_Success || 
                State == EPlayerState.Intercept_Failed);
    }

    public ETeamStateChangeDelayedType StateChangeDelayedType
    {
        get { return m_kStateChangeDelayedType;}
        set { m_kStateChangeDelayedType = value;}
    }

    public EPlayerState State
    {
        get { return m_kPlayerState; }
        
    }

    //use this state after team change state
    public EPlayerState NextState
    {
        get { return m_kNextState; }
        set { m_kNextState = value; }        
    }

    public int[] NextPossibleDribbleDir
    {
        get{ return m_NextPossibleDribbleDir;}
        set{ m_NextPossibleDribbleDir = value;}
    }

	public double TimeToIdleAfterFail
	{
		get { return m_TimeToIdleAfterFail;}
		set { m_TimeToIdleAfterFail = value;}
	}

    public uint GuardLineIDX
    {
        get { return m_uiGuardLineIDX; }
        set { m_uiGuardLineIDX = value; }
    }
    public uint GuardLineIDZ
    {
        get { return m_uiGuardLineIDZ; }
        set { m_uiGuardLineIDZ = value; }
    }
    public Vector3D TargetPos
    {
        get { return m_kTargetPos; }
        set { m_kTargetPos = OffsideFieldJudge(value); }
    }

    public double MinDistanceToKeep
    {
        get {return m_MinDistanceToKeep;}
        set {m_MinDistanceToKeep = value;}
    }
    
    public double MaxDistanceToKeep
    {
        get {return m_MaxDistanceToKeep;}
        set {m_MaxDistanceToKeep = value;}
    }

    public EMarkStatus MarkingStatus
    {
        get { return m_MarkingStatus;}
        set { m_MarkingStatus = value;}
    }

    public LLPlayer Opponent
    {
        get { return m_kOpponent; }
        set { m_kOpponent = value; }
    }

    public bool AIEnable
    {
        get { return m_bAIEnable; }
        set { m_bAIEnable = value; }
    }

    public double GeterScore
    {
        set
        {
            m_geterScore = value;
        }
        get
        {
            return m_geterScore;
        }
    }
    public Socore Socore
    {
        set
        {
            m_score = value;
        }
        get
        {
            return m_score;
        }
    }
    public uint RegionZ
    {
        set
        {
            m_region_z = value;
        }
        get
        {
            return m_region_z;
        }
    }
    public uint RegionX
    {
        set
        {
            m_region_x = value;
        }
        get
        {
            return m_region_x;
        }
    }

    public bool ContinueDribble
    {
        get { return m_bContinueDribble; }
        
    }

    private EMarkStatus m_MarkingStatus = EMarkStatus.NONE; //盯防状态：无、有球、无球

    private Vector3D m_kHomePos = new Vector3D();
    private ETeamStateChangeDelayedType m_kStateChangeDelayedType = ETeamStateChangeDelayedType.NONE;
    private EPlayerState m_kPlayerState = EPlayerState.HomePos;
    private EPlayerState m_kNextState = EPlayerState.PS_END;

    private int[] m_NextPossibleDribbleDir = null;
	private double m_TimeToIdleAfterFail = 1d;

    private uint m_region_x;
    private uint m_region_z;
    private uint m_uiGuardLineIDX = 0;
    private uint m_uiGuardLineIDZ = 0;
    private Vector3D m_kTargetPos = new Vector3D();
    private double m_MinDistanceToKeep = 4d;
    private double m_MaxDistanceToKeep = 6d;
    private LLPlayer m_kOpponent = null; //被盯防//
    private bool m_bAIEnable = true;
    private double m_geterScore;
    private Socore m_score;
    private bool m_bContinueDribble = false;
}
