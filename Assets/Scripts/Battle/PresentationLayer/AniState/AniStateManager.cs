using Common;
using Common.Log;
using System.Collections.Generic;

public class AniStateManager
{
    public AniStateManager(LLUnit kPlayer)
    {
        m_kPlayer = kPlayer;
        RegisterStates();
    }

    public void SetState(EAniState kState)
    {
        if (null == m_kCurState)
        {
            // check whether state key is valid
            if(m_kStateList.ContainsKey(kState))
            {
                m_kCurState = m_kStateList[kState];
                m_kCurState.Player = m_kPlayer;
                m_bChangeState = true;
            }
            return;
        }

        if (m_kCurState.State == kState)
            return;

        if (m_kStateList.ContainsKey(kState))
        {
            m_kPreState = m_kCurState;
            m_kCurState = m_kStateList[kState];
            m_kCurState.Player = m_kPlayer;
            if(null != m_kPreState)
                m_kCurState.PreState = m_kPreState.State;
            //m_bChangeState = true;
            // 进入清除状态更换//
            if (null != m_kPreState)
            {
                m_kPreState.OnExit();
            }

            if (null != m_kCurState)
            {
                if (null != m_kPreState)
                    m_kCurState.PreState = m_kPreState.State;
                m_kCurState.OnEnter();
            }
        }
        else
        {
            LogManager.Instance.LogError("Animation State Invalid With Name {0}", kState);
        }
    }

    public void Update(double fTime)
    {
//         if(m_bChangeState)
//         {
// 
//             if (null != m_kPreState)
//             {
//                 m_kPreState.OnExit();
//             }
//                 
//             if (null != m_kCurState)
//             {
//                 if (null != m_kPreState)
//                     m_kCurState.PreState = m_kPreState.State;
//                 m_kCurState.OnEnter();
//             }
// 
//             m_bChangeState = false;
//         }

        if (null != m_kCurState)
            m_kCurState.OnUpdate((float)fTime);

    }

    private void RegisterStates()
    {
 		//新的动画状态机//
        m_kStateList.Add(EAniState.Idle, new NetAniNullBallIdleState());
        m_kStateList.Add(EAniState.Special_Idle, new NetAniSpecialIdleState());
        m_kStateList.Add(EAniState.NormalDribbl, new NetAniNormalCarryState());
        m_kStateList.Add(EAniState.LowDribble, new NetAniLowCarryState());
        m_kStateList.Add(EAniState.PassBall_Floor, new NetAniPassBallFloorState());
        m_kStateList.Add(EAniState.PassBall_High, new NetAniPassBallHighState());
        m_kStateList.Add(EAniState.Shoot_Near, new NetAniFootNearShootState());
        m_kStateList.Add(EAniState.Shoot_Far, new NetAniFootFarShootState());
        m_kStateList.Add(EAniState.Mark_Ball, new NetAniHaveBallDefineState());
        m_kStateList.Add(EAniState.Mark, new NetAniNullBallDefineState());
        m_kStateList.Add(EAniState.NormalRun, new NetAniNullBallRunState());
        m_kStateList.Add(EAniState.Walk, new NetAniNullBallWalkState());
        m_kStateList.Add(EAniState.Catch_GroundBall, new NetAniCatchFloorBallState());
        m_kStateList.Add(EAniState.Break_Through,new NetAniBreakOutState());
        m_kStateList.Add(EAniState.Break_Through_Failed, new NetAniBreakOutFailedState());
        m_kStateList.Add(EAniState.Stop_BreakThrough_Failed, new NetAniStopBreakOutFailedState());
        m_kStateList.Add(EAniState.Stop_BreakThrough_Success, new NetAniStopBreakOutSuccessState());
        m_kStateList.Add(EAniState.HeadRob_Pass, new NetAniHeadRobPassState());
        m_kStateList.Add(EAniState.HeadRob_Shoot, new NetAniHeadRobShootState());
        m_kStateList.Add(EAniState.HeadRob_Carry,new NetAniHeadRobChestCarryState());
        m_kStateList.Add(EAniState.HeadRob2_Stop,new NetAniHeadRob2StopState());
        m_kStateList.Add(EAniState.Head_Tackle_Failed, new NetAniHeadRobFailedState());
        m_kStateList.Add(EAniState.Ground_Tackle, new NetAniSliderTackleState());
        m_kStateList.Add(EAniState.Ground_Tackle_Failed, new NetAniSliderTackleFailedState());
        m_kStateList.Add(EAniState.Stop_Ground_Tackle_Success, new NetAniAvoidSliderTackleSuccessState());
        m_kStateList.Add(EAniState.Stop_Ground_Tackle_Failed, new NetAniAvoidSliderTackleFailedState());
        m_kStateList.Add(EAniState.Ground_Snatch, new NetAniTackleState());
        m_kStateList.Add(EAniState.Ground_Snatch_Failed, new NetAniTackleFailedState());
        m_kStateList.Add(EAniState.Stop_Ground_Snatch_Failed, new NetAniStopTackleFailedState());
        m_kStateList.Add(EAniState.Stop_Ground_Snatch_Success, new NetAniStopTackleSuccessState());
        m_kStateList.Add(EAniState.Ground_Block, new NetAniInterceptState());
        m_kStateList.Add(EAniState.Ground_Block_Failed,new NetAniInterceptFailedState());

		m_kStateList.Add(EAniState.Shoot_Near_Success, new NetAniFootNearShootSuccessState());
		m_kStateList.Add(EAniState.Shoot_Far_Success, new NetAniFootFarShootSuccessState());
		m_kStateList.Add(EAniState.HeadRob_Shoot_Success, new NetAniHeadShootSuccessState());
		m_kStateList.Add(EAniState.Shoot_Near_Failed, new NetAniFootNearShootFailedState());
		m_kStateList.Add(EAniState.Shoot_Far_Failed, new NetAniFootFarShootFailedState());
		m_kStateList.Add(EAniState.HeadRob_Shoot_Failed, new NetAniHeadShootFailedState());

        m_kStateList.Add(EAniState.Kick_Idle, new NetAniMatchKickIdleState());
        m_kStateList.Add(EAniState.Match_ReadyIdle,new NetAniMatchReadyState());
        m_kStateList.Add(EAniState.Match_BeginKick, new NetAniMatchBeginState());
        m_kStateList.Add(EAniState.Match_ReadyKick, new NetAniMatchReadyKickState());
        m_kStateList.Add(EAniState.GoalCeleBration, new NetAniGoalPlayerCelebrateState());
        m_kStateList.Add(EAniState.GoalSad, new NetAniGoalPlayerSaddedState());

		m_kStateList.Add(EAniState.GK_FrontWalk, new NetAniGKFrontWalkState());
		m_kStateList.Add(EAniState.GK_BackWalk, new NetAniGKBackWalkState());
		m_kStateList.Add(EAniState.GK_LeftWalk, new NetAniGKLeftWalkState());
		m_kStateList.Add(EAniState.GK_RightWalk, new NetAniGKRightWalkState());
        m_kStateList.Add(EAniState.GK_Save_Success, new NetAniGKSaveSuccessState());
        m_kStateList.Add(EAniState.GK_Save_Failed, new NetAniGKSaveFailedState());
        m_kStateList.Add(EAniState.GK_Save_Out_Success, new NetAniGKSaveSuccessOutBottomState());

        m_kStateList.Add(EAniState.GK_ThrowBall, new NetAniGKThrowBallState());
        m_kStateList.Add(EAniState.GK_KickBall, new NetAniGKKickBallState());
        m_kStateList.Add(EAniState.GK_BigKickBall, new NetAniGKBigKickBallState());

    }

    public AniBaseState CurState
    {
        get { return m_kCurState; }
        
    }

    public AniBaseState PreState
    {
        get { return m_kPreState; }
        
    }

    private AniBaseState m_kPreState = null;
    private AniBaseState m_kCurState = null;
    private bool m_bChangeState = false;
    private LLUnit m_kPlayer = null;
    private Dictionary<EAniState, AniBaseState> m_kStateList = new Dictionary<EAniState, AniBaseState>();
}