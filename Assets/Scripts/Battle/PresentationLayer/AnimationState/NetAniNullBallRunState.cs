using System.Collections;
using Common;
using System;
using Common.Log;
/// <summary>
/// Idle 普通待机状态,包含所有的待机状态
/// </summary>
public class NetAniNullBallRunState : AniBaseState
{

    private enum NetAniNullBallRunSubState
    {

        EAS_EnterToNULLBallQuickRun,
        EAS_EnterToNULLBallQuickRun90,
        EAS_EnterToNULLBallQuickRun180,

        EAS_EnterNQWalkToNULLBallQuickRun,
        EAS_EnterNQWalkToNULLBallQuickRun90,
        EAS_EnterNQWalkToNULLBallQuickRun180,


        EAS_EnterNIdleToNULLBallQuickRun,
        EAS_EnterNIdleToNULLBallQuickRun90,
        EAS_EnterNIdleToNULLBallQuickRun180,

        EAS_EnterNLDefineToNULLBallQuickRun,
        EAS_EnterNLDefineToNULLBallQuickRun90,
        EAS_EnterNLDefineToNULLBallQuickRun180,

        EAS_EnterHLDefineToNULLBallQuickRun,
        EAS_EnterHLDefineToNULLBallQuickRun90,
        EAS_EnterHLDefineToNULLBallQuickRun180,


        EAS_BeginToNULLBallQuickRun,
        EAS_BeginToNULLBallQuickRun90,
        EAS_BeginToNULLBallQuickRun180,

        EAS_ReadyToNULLBallQuickRun,
        EAS_ReadyToNULLBallQuickRun90,
        EAS_ReadyToNULLBallQuickRun180,

        EAS_EnterSpecialIdleToNULLBallQuickRun,
        EAS_EnterSpecialIdleToNULLBallQuickRun90,
        EAS_EnterSpecialIdleToNULLBallQuickRun180,

    }
    public NetAniNullBallRunState()
        : base(EAniState.NormalRun)
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
            case EAniState.Walk:
                WalkStateChange(m_RoateType);
                break;
            case EAniState.NormalRun:
                NormalRunStateChange(m_RoateType);
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
        }
        base.OnBegin();
    }

    private void IdleStateChange(RoundData _Rdata)
    {
        if (_Rdata == RoundData.Round90)
        {
            m_AnistateSubName = NetAniNullBallRunSubState.EAS_EnterNIdleToNULLBallQuickRun90.ToString();
        }
        else if (_Rdata == RoundData.Round180)
        {
            m_AnistateSubName = NetAniNullBallRunSubState.EAS_EnterNIdleToNULLBallQuickRun180.ToString();
        }
        else
            m_AnistateSubName = NetAniNullBallRunSubState.EAS_EnterNIdleToNULLBallQuickRun.ToString();
    }
    private void WalkStateChange(RoundData _Rdata)
    {
        if (_Rdata == RoundData.Round90)
        {
            m_AnistateSubName = NetAniNullBallRunSubState.EAS_EnterNQWalkToNULLBallQuickRun90.ToString();
        }
        else if (_Rdata == RoundData.Round180)
        {
            m_AnistateSubName = NetAniNullBallRunSubState.EAS_EnterNQWalkToNULLBallQuickRun180.ToString();
        }
        else
            m_AnistateSubName = NetAniNullBallRunSubState.EAS_EnterNQWalkToNULLBallQuickRun.ToString();

    }
    private void NormalRunStateChange(RoundData _Rdata)
    {
        if (_Rdata == RoundData.Round90)
        {
            m_AnistateSubName = NetAniNullBallRunSubState.EAS_EnterToNULLBallQuickRun90.ToString();
        }
        else if (_Rdata == RoundData.Round180)
        {
            m_AnistateSubName = NetAniNullBallRunSubState.EAS_EnterToNULLBallQuickRun180.ToString();
        }
        else
            m_AnistateSubName = NetAniNullBallRunSubState.EAS_EnterToNULLBallQuickRun.ToString();

    }
    private void MarkBallStateChange(RoundData _Rdata)
    {
        if (_Rdata == RoundData.Round90)
        {
            m_AnistateSubName = NetAniNullBallRunSubState.EAS_EnterHLDefineToNULLBallQuickRun90.ToString();
        }
        else if (_Rdata == RoundData.Round180)
        {
            m_AnistateSubName = NetAniNullBallRunSubState.EAS_EnterHLDefineToNULLBallQuickRun180.ToString();
        }
        else
            m_AnistateSubName = NetAniNullBallRunSubState.EAS_EnterHLDefineToNULLBallQuickRun.ToString();


    }
    private void MarkStateChange(RoundData _Rdata)
    {
        if (_Rdata == RoundData.Round90)
        {
            m_AnistateSubName = NetAniNullBallRunSubState.EAS_EnterNLDefineToNULLBallQuickRun90.ToString();
        }
        else if (_Rdata == RoundData.Round180)
        {
            m_AnistateSubName = NetAniNullBallRunSubState.EAS_EnterNLDefineToNULLBallQuickRun180.ToString();
        }
        else
            m_AnistateSubName = NetAniNullBallRunSubState.EAS_EnterNLDefineToNULLBallQuickRun.ToString();
    }

    private void MatchBeginKickStateChange(RoundData _Rdata)
    {
        if (_Rdata == RoundData.Round90)
        {
            m_AnistateSubName = NetAniNullBallRunSubState.EAS_BeginToNULLBallQuickRun90.ToString();
        }
        else if (_Rdata == RoundData.Round180)
        {
            m_AnistateSubName = NetAniNullBallRunSubState.EAS_BeginToNULLBallQuickRun180.ToString();
        }
        else
            m_AnistateSubName = NetAniNullBallRunSubState.EAS_BeginToNULLBallQuickRun.ToString();
    }

    private void MatchReadyIdleStateChange(RoundData _Rdata)
    {
        if (_Rdata == RoundData.Round90)
        {
            m_AnistateSubName = NetAniNullBallRunSubState.EAS_ReadyToNULLBallQuickRun90.ToString();
        }
        else if (_Rdata == RoundData.Round180)
        {
            m_AnistateSubName = NetAniNullBallRunSubState.EAS_ReadyToNULLBallQuickRun180.ToString();
        }
        else
            m_AnistateSubName = NetAniNullBallRunSubState.EAS_ReadyToNULLBallQuickRun.ToString();
    }

    private void OtherStateChange(RoundData _Rdata)
    {
        if (_Rdata == RoundData.Round90)
        {
            m_AnistateSubName = NetAniNullBallRunSubState.EAS_EnterSpecialIdleToNULLBallQuickRun90.ToString();
        }
        else if (_Rdata == RoundData.Round180)
        {
            m_AnistateSubName = NetAniNullBallRunSubState.EAS_EnterSpecialIdleToNULLBallQuickRun180.ToString();

        }
        else
            m_AnistateSubName = NetAniNullBallRunSubState.EAS_EnterSpecialIdleToNULLBallQuickRun.ToString();
    }

}
