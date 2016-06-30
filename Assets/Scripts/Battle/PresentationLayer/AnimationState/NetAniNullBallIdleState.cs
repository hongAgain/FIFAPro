using System.Collections;
using Common;
using System;
using Common.Log;
/// <summary>
/// 无球等待Idle
/// </summary>
public class NetAniNullBallIdleState : AniBaseState
{

    private enum NetAniNullBallIdleSubState
    {
        // 开球无状态进入idle//
        EAS_EnterNULLBallIdle,

        EAS_EnterReadyToNBallIdle,
        EAS_EnterReadyToNBallIdle90,
        EAS_EnterReadyToNBallIdle180,
        EAS_EnterBeginToNBallIdle,
        EAS_EnterBeginToNBallIdle90,
        EAS_EnterBeginToNBallIdle180,


        EAS_EnterToNULLBallIdle,
        EAS_EnterToNULLBallIdle90,
        EAS_EnterToNULLBallIdle180,

        EAS_EnterNQRunToNULLBallIdle,
        EAS_EnterNQRunToNULLBallIdle90,
        EAS_EnterNQRunToNULLBallIdle180,

        EAS_EnterNQWalkToNULLBallIdle,
        EAS_EnterNQWalkToNULLBallIdle90,
        EAS_EnterNQWalkToNULLBallIdle180,

        EAS_EnterNLDefineToNULLBallIdle,
        EAS_EnterNLDefineToNULLBallIdle90,
        EAS_EnterNLDefineToNULLBallIdle180,

        EAS_EnterHLDefineToNULLBallIdle,
        EAS_EnterHLDefineToNULLBallIdle90,
        EAS_EnterHLDefineToNULLBallIdle180,

        EAS_EnterSpecialIdleToNULLBallIdle,
        EAS_EnterSpecialIdleToNULLBallIdle90,
        EAS_EnterSpecialIdleToNULLBallIdle180,

        //传球之后的Idle //
        EAS_EnterPassBallToNBallIdle,
    }
    public NetAniNullBallIdleState()
        : base(EAniState.Idle)
    {

    }

    protected override void OnRotateAngle()
    {
        //base.OnRotateAngle();
    }
    public override void OnEnter()
    {
        base.OnEnter();
        if (m_kPlayer is LLGoalKeeper)
            m_kPlayer.CanUpdateRotate = true;
    }

    protected override void OnBegin()
    {
        switch (m_kPreState)
        {
            case EAniState.Match_BeginKick:
                MatchBeginStateChange(m_RoateType);
                break;
            case EAniState.Match_ReadyIdle:
                MatchReadyStateChange(m_RoateType);
                break;
            case EAniState.Idle:
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
            case EAniState.Special_Idle:
                OtherStateChange(m_RoateType);
                break;
            case EAniState.PassBall_High:
            case EAniState.PassBall_Floor:
                m_AnistateSubName = NetAniNullBallIdleSubState.EAS_EnterPassBallToNBallIdle.ToString();
				break;
		    case EAniState.GK_ThrowBall:
				m_AnistateSubName = NetAniNullBallIdleSubState.EAS_EnterToNULLBallIdle.ToString();
				break;
		    case EAniState.GK_KickBall:
				m_AnistateSubName = NetAniNullBallIdleSubState.EAS_EnterToNULLBallIdle.ToString();
				break;
		    case EAniState.GK_BigKickBall:
				m_AnistateSubName = NetAniNullBallIdleSubState.EAS_EnterToNULLBallIdle.ToString();
				break;
            default: //默认任意状态进入无球IDLE//
                m_AnistateSubName = NetAniNullBallIdleSubState.EAS_EnterNULLBallIdle.ToString();
                break;
        }
        base.OnBegin();
    }
    private void MatchReadyStateChange(RoundData _Rdata)
    {
        if (_Rdata == RoundData.Round90)
        {
            m_AnistateSubName = NetAniNullBallIdleSubState.EAS_EnterReadyToNBallIdle90.ToString();
        }
        else if (_Rdata == RoundData.Round180)
        {
            m_AnistateSubName = NetAniNullBallIdleSubState.EAS_EnterReadyToNBallIdle180.ToString();
        }
        else
            m_AnistateSubName = NetAniNullBallIdleSubState.EAS_EnterReadyToNBallIdle.ToString();
    }
    private void MatchBeginStateChange(RoundData _Rdata)
    {
        if (_Rdata == RoundData.Round90)
        {
            m_AnistateSubName = NetAniNullBallIdleSubState.EAS_EnterBeginToNBallIdle90.ToString();
        }
        else if (_Rdata == RoundData.Round180)
        {
            m_AnistateSubName = NetAniNullBallIdleSubState.EAS_EnterBeginToNBallIdle180.ToString();
        }
        else
            m_AnistateSubName = NetAniNullBallIdleSubState.EAS_EnterBeginToNBallIdle.ToString();
    }
    private void IdleStateChange(RoundData _Rdata)
    {
        if (_Rdata == RoundData.Round90)
        {
            m_AnistateSubName = NetAniNullBallIdleSubState.EAS_EnterToNULLBallIdle90.ToString();
        }
        else if (_Rdata == RoundData.Round180)
        {
            m_AnistateSubName = NetAniNullBallIdleSubState.EAS_EnterToNULLBallIdle180.ToString();
        }
        else
            m_AnistateSubName = NetAniNullBallIdleSubState.EAS_EnterToNULLBallIdle.ToString();
    }
    private void WalkStateChange(RoundData _Rdata)
    {
        if (_Rdata == RoundData.Round90)
        {
            m_AnistateSubName = NetAniNullBallIdleSubState.EAS_EnterNQWalkToNULLBallIdle90.ToString();
        }
        else if (_Rdata == RoundData.Round180)
        {
            m_AnistateSubName = NetAniNullBallIdleSubState.EAS_EnterNQWalkToNULLBallIdle180.ToString();
        }
        else
            m_AnistateSubName = NetAniNullBallIdleSubState.EAS_EnterNQWalkToNULLBallIdle.ToString();
    }
    private void NormalRunStateChange(RoundData _Rdata)
    {
        if (_Rdata == RoundData.Round90)
        {
            m_AnistateSubName = NetAniNullBallIdleSubState.EAS_EnterNQRunToNULLBallIdle90.ToString();
        }
        else if (_Rdata == RoundData.Round180)
        {
            m_AnistateSubName = NetAniNullBallIdleSubState.EAS_EnterNQRunToNULLBallIdle180.ToString();
        }
        else
            m_AnistateSubName = NetAniNullBallIdleSubState.EAS_EnterNQRunToNULLBallIdle.ToString();

    }
    private void MarkBallStateChange(RoundData _Rdata)
    {
        if (_Rdata == RoundData.Round90)
        {
            m_AnistateSubName = NetAniNullBallIdleSubState.EAS_EnterHLDefineToNULLBallIdle90.ToString();
        }
        else if (_Rdata == RoundData.Round180)
        {
            m_AnistateSubName = NetAniNullBallIdleSubState.EAS_EnterHLDefineToNULLBallIdle180.ToString();
        }
        else
            m_AnistateSubName = NetAniNullBallIdleSubState.EAS_EnterHLDefineToNULLBallIdle.ToString();


    }
    private void MarkStateChange(RoundData _Rdata)
    {
        if (_Rdata == RoundData.Round90)
        {
            m_AnistateSubName = NetAniNullBallIdleSubState.EAS_EnterNLDefineToNULLBallIdle90.ToString();
        }
        else if (_Rdata == RoundData.Round180)
        {
            m_AnistateSubName = NetAniNullBallIdleSubState.EAS_EnterNLDefineToNULLBallIdle180.ToString();
        }
        else
            m_AnistateSubName = NetAniNullBallIdleSubState.EAS_EnterNLDefineToNULLBallIdle.ToString();
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
        if (_Rdata == RoundData.Round90)
        {
            m_AnistateSubName = NetAniNullBallIdleSubState.EAS_EnterSpecialIdleToNULLBallIdle90.ToString();
        }
        else if (_Rdata == RoundData.Round180)
        {
            m_AnistateSubName = NetAniNullBallIdleSubState.EAS_EnterSpecialIdleToNULLBallIdle180.ToString();

        }
        else
            m_AnistateSubName = NetAniNullBallIdleSubState.EAS_EnterSpecialIdleToNULLBallIdle.ToString();

    }
}
