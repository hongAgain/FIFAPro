using System.Collections;
using Common;
using System;
using Common.Log;
/// <summary>
/// Idle 普通待机状态,包含所有的待机状态
/// </summary>
public class NetAniNullBallWalkState : AniBaseState
{

    private enum NetAniNullBallWalkSubState
    {

        EAS_EnterNQWalkToNULLBallQuickWalk,
        EAS_EnterNQWalkToNULLBallQuickWalk90,
        EAS_EnterNQWalkToNULLBallQuickWalk180,
        EAS_EnterNQRunToNULLBallQuickWalk,
        EAS_EnterNQRunToNULLBallQuickWalk90,
        EAS_EnterNQRunToNULLBallQuickWalk180,
        EAS_EnterNIdleToNULLBallQuickWalk,
        EAS_EnterNIdleToNULLBallQuickWalk90,
        EAS_EnterNIdleToNULLBallQuickWalk180,
        EAS_EnterNLDefineToNULLBallQuickWalk,
        EAS_EnterNLDefineToNULLBallQuickWalk90,
        EAS_EnterNLDefineToNULLBallQuickWalk180,
        EAS_EnterHLDefineToNULLBallQuickWalk,
        EAS_EnterHLDefineToNULLBallQuickWalk90,
        EAS_EnterHLDefineToNULLBallQuickWalk180,

        EAS_ReadyToNULLBallQuickWalk,
        EAS_ReadyToNULLBallQuickWalk90,
        EAS_ReadyToNULLBallQuickWalk180,
        EAS_BeginToNULLBallQuickWalk,
        EAS_BeginToNULLBallQuickWalk90,
        EAS_BeginToNULLBallQuickWalk180,

        EAS_EnterSpecialIdleToNULLBallQuickWalk,
        EAS_EnterSpecialIdleToNULLBallQuickWalk90,
        EAS_EnterSpecialIdleToNULLBallQuickWalk180,




    }
    public NetAniNullBallWalkState()
        : base(EAniState.Walk)
    {

    }


    protected override void OnRotateAngle()
    {

    }
    protected override void OnBegin()
    {
        switch (m_kPreState)
        {
            case EAniState.Idle:
            case EAniState.PassBall_Floor:
            case EAniState.PassBall_High:
            case EAniState.HeadRob_Pass:
            case EAniState.HeadRob_Shoot:
                IdleStateChange(m_RoateType);
                break;
            case EAniState.NormalRun:
                NormalRunStateChange(m_RoateType);
                break;
            case EAniState.Walk:
                WalkStateChange(m_RoateType);
                break;
            case EAniState.Mark:
                MarkStateChange(m_RoateType);
                break;
            case EAniState.Mark_Ball:
                MarkBallStateChange(m_RoateType);
                break;
            case EAniState.Match_ReadyIdle:
                MatchReadyIdleStateChange(m_RoateType);
                break;
            case EAniState.Match_BeginKick:
                MatchBeginKickStateChange(m_RoateType);
                break;
            case EAniState.Special_Idle:
                OtherStateChange(m_RoateType);
                break;
            default:
                //OtherStateChange(m_invertAngle);
                break;
        }
        base.OnBegin();
    }
    private void IdleStateChange(RoundData _Rdata)
    {
        if (_Rdata == RoundData.Round90)
        {
            m_AnistateSubName = NetAniNullBallWalkSubState.EAS_EnterNIdleToNULLBallQuickWalk90.ToString();
        }
        else if (_Rdata == RoundData.Round180)
        {
            m_AnistateSubName = NetAniNullBallWalkSubState.EAS_EnterNIdleToNULLBallQuickWalk180.ToString();
        }
        else
            m_AnistateSubName = NetAniNullBallWalkSubState.EAS_EnterNIdleToNULLBallQuickWalk.ToString();
    }
    private void WalkStateChange(RoundData _Rdata)
    {
        if (_Rdata == RoundData.Round90)
        {
            m_AnistateSubName = NetAniNullBallWalkSubState.EAS_EnterNQWalkToNULLBallQuickWalk90.ToString();
        }
        else if (_Rdata == RoundData.Round180)
        {
            m_AnistateSubName = NetAniNullBallWalkSubState.EAS_EnterNQWalkToNULLBallQuickWalk180.ToString();
        }
        else
            m_AnistateSubName = NetAniNullBallWalkSubState.EAS_EnterNQWalkToNULLBallQuickWalk.ToString();
    }
    private void NormalRunStateChange(RoundData _Rdata)
    {
        if (_Rdata == RoundData.Round90)
        {
            m_AnistateSubName = NetAniNullBallWalkSubState.EAS_EnterNQRunToNULLBallQuickWalk90.ToString();
        }
        else if (_Rdata == RoundData.Round180)
        {
            m_AnistateSubName = NetAniNullBallWalkSubState.EAS_EnterNQRunToNULLBallQuickWalk180.ToString();
        }
        else
            m_AnistateSubName = NetAniNullBallWalkSubState.EAS_EnterNQRunToNULLBallQuickWalk.ToString();

    }
    private void MarkBallStateChange(RoundData _Rdata)
    {
        if (_Rdata == RoundData.Round90)
        {
            m_AnistateSubName = NetAniNullBallWalkSubState.EAS_EnterHLDefineToNULLBallQuickWalk90.ToString();
        }
        else if (_Rdata == RoundData.Round180)
        {
            m_AnistateSubName = NetAniNullBallWalkSubState.EAS_EnterHLDefineToNULLBallQuickWalk180.ToString();
        }
        else
            m_AnistateSubName = NetAniNullBallWalkSubState.EAS_EnterHLDefineToNULLBallQuickWalk.ToString();


    }
    private void MarkStateChange(RoundData _Rdata)
    {
        if (_Rdata == RoundData.Round90)
        {
            m_AnistateSubName = NetAniNullBallWalkSubState.EAS_EnterNLDefineToNULLBallQuickWalk90.ToString();
        }
        else if (_Rdata == RoundData.Round180)
        {
            m_AnistateSubName = NetAniNullBallWalkSubState.EAS_EnterNLDefineToNULLBallQuickWalk180.ToString();
        }
        else
            m_AnistateSubName = NetAniNullBallWalkSubState.EAS_EnterNLDefineToNULLBallQuickWalk.ToString();
    }

    private void MatchBeginKickStateChange(RoundData _Rdata)
    {
        if (_Rdata == RoundData.Round90)
        {
            m_AnistateSubName = NetAniNullBallWalkSubState.EAS_ReadyToNULLBallQuickWalk90.ToString();
        }
        else if (_Rdata == RoundData.Round180)
        {
            m_AnistateSubName = NetAniNullBallWalkSubState.EAS_ReadyToNULLBallQuickWalk180.ToString();
        }
        else
            m_AnistateSubName = NetAniNullBallWalkSubState.EAS_ReadyToNULLBallQuickWalk.ToString();
    }

    private void MatchReadyIdleStateChange(RoundData _Rdata)
    {
        if (_Rdata == RoundData.Round90)
        {
            m_AnistateSubName = NetAniNullBallWalkSubState.EAS_BeginToNULLBallQuickWalk90.ToString();
        }
        else if (_Rdata == RoundData.Round180)
        {
            m_AnistateSubName = NetAniNullBallWalkSubState.EAS_BeginToNULLBallQuickWalk180.ToString();
        }
        else
            m_AnistateSubName = NetAniNullBallWalkSubState.EAS_BeginToNULLBallQuickWalk.ToString();
    }

    private void OtherStateChange(RoundData _Rdata)
    {
        //争顶失败、
        //持球球员突破成功，防守球员进入Idle,
        //持球球员突破失败，
        //拦截失败
        //防守球员抢截成功、防守球员铲断成功原持球球员进入idle
        //防守球员抢截失败、铲断失败、进入IDLE,
        //防守球员的拦截失败
        //射门后的Idle
        //头球攻门
        //         if (m_kPreState == EAniState.Head_Tackle_Failed
        //             || m_kPreState == EAniState.Stop_BreakThrough_Failed
        //             || m_kPreState == EAniState.Break_Through_Failed
        //             || m_kPreState == EAniState.Ground_Block_Failed
        //             || m_kPreState == EAniState.Ground_Tackle_Failed
        //             || m_kPreState == EAniState.Stop_Ground_Tackle_Success
        //             || m_kPreState == EAniState.Ground_Snatch_Failed
        //             || m_kPreState == EAniState.Stop_Ground_Snatch_Success
        //             || m_kPreState == EAniState.Shoot_Failed
        //             || m_kPreState == EAniState.Shoot_Far
        //             || m_kPreState == EAniState.Shoot_Near
        //             || m_kPreState == EAniState.HeadShoot)
        //         {
        if (_Rdata == RoundData.Round90)
        {
            m_AnistateSubName = NetAniNullBallWalkSubState.EAS_EnterSpecialIdleToNULLBallQuickWalk90.ToString();
        }
        else if (_Rdata == RoundData.Round180)
        {
            m_AnistateSubName = NetAniNullBallWalkSubState.EAS_EnterSpecialIdleToNULLBallQuickWalk180.ToString();

        }
        else
            m_AnistateSubName = NetAniNullBallWalkSubState.EAS_EnterSpecialIdleToNULLBallQuickWalk.ToString();
    }
}
