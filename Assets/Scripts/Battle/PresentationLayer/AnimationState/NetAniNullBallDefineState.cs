using System.Collections;
using Common;
using Common.Log;
using System;
public class NetAniNullBallDefineState : AniBaseState
{

    private enum NetAniNullBallDefineSubState
    {
        EAS_EnterNIdleToNULLBallDefine,
        EAS_EnterNIdleToNULLBallDefine90,
        EAS_EnterNIdleToNULLBallDefine180,
        EAS_EnterNQWalkToNULLBallDefine,
        EAS_EnterNQWalkToNULLBallDefine90,
        EAS_EnterNQWalkToNULLBallDefine180,
        EAS_EnterNQRunToNULLBallDefine,
        EAS_EnterNQRunToNULLBallDefine90,
        EAS_EnterNQRunToNULLBallDefine180,
        EAS_EnterToNULLBallDefine,
        EAS_EnterToNULLBallDefine90,
        EAS_EnterToNULLBallDefine180,
        EAS_EnterHLDefineToNULLBallDefine,
        EAS_EnterHLDefineToNULLBallDefine90,
        EAS_EnterHLDefineToNULLBallDefine180,

        EAS_EnterSpecialIdleToNULLBallDefine,
        EAS_EnterSpecialIdleToNULLBallDefine90,
        EAS_EnterSpecialIdleToNULLBallDefine180,


    }
    public NetAniNullBallDefineState()
        : base(EAniState.Mark)
    {

    }

    protected override void OnRotateAngle()
    {
        
    }
    protected override void OnBegin()
    {
        m_kPreState = EAniState.Idle;
        switch (m_kPreState)
        {
            case EAniState.Idle:
                IdleStateChange(m_RoateType);
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
            case EAniState.NormalRun:
                NormalRunStateChange(m_RoateType);
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
            m_AnistateSubName = NetAniNullBallDefineSubState.EAS_EnterNIdleToNULLBallDefine90.ToString();
        }
        else if (_Rdata == RoundData.Round180)
        {
            m_AnistateSubName = NetAniNullBallDefineSubState.EAS_EnterNIdleToNULLBallDefine180.ToString();

        }
        else
            m_AnistateSubName = NetAniNullBallDefineSubState.EAS_EnterNIdleToNULLBallDefine.ToString();
    }

    private void WalkStateChange(RoundData _Rdata)
    {
        if (_Rdata == RoundData.Round90)
        {
            m_AnistateSubName = NetAniNullBallDefineSubState.EAS_EnterNQWalkToNULLBallDefine90.ToString();
        }
        else if (_Rdata == RoundData.Round180)
        {
            m_AnistateSubName = NetAniNullBallDefineSubState.EAS_EnterNQWalkToNULLBallDefine180.ToString();

        }
        else
            m_AnistateSubName = NetAniNullBallDefineSubState.EAS_EnterNQWalkToNULLBallDefine.ToString();
    }
    private void MarkStateChange(RoundData _Rdata)
    {
        if (_Rdata == RoundData.Round90)
        {
            m_AnistateSubName = NetAniNullBallDefineSubState.EAS_EnterToNULLBallDefine90.ToString();
        }
        else if (_Rdata == RoundData.Round180)
        {
            m_AnistateSubName = NetAniNullBallDefineSubState.EAS_EnterToNULLBallDefine180.ToString();

        }
        else
            m_AnistateSubName = NetAniNullBallDefineSubState.EAS_EnterToNULLBallDefine.ToString();

    }

    private void MarkBallStateChange(RoundData _Rdata)
    {
        if (_Rdata == RoundData.Round90)
        {
            m_AnistateSubName = NetAniNullBallDefineSubState.EAS_EnterHLDefineToNULLBallDefine90.ToString();
        }
        else if (_Rdata == RoundData.Round180)
        {
            m_AnistateSubName = NetAniNullBallDefineSubState.EAS_EnterHLDefineToNULLBallDefine180.ToString();

        }
        else
            m_AnistateSubName = NetAniNullBallDefineSubState.EAS_EnterHLDefineToNULLBallDefine.ToString();

    }
    private void NormalRunStateChange(RoundData _Rdata)
    {
        if (_Rdata == RoundData.Round90)
        {
            m_AnistateSubName = NetAniNullBallDefineSubState.EAS_EnterNQRunToNULLBallDefine90.ToString();
        }
        else if (_Rdata == RoundData.Round180)
        {
            m_AnistateSubName = NetAniNullBallDefineSubState.EAS_EnterNQRunToNULLBallDefine180.ToString();

        }
        else
            m_AnistateSubName = NetAniNullBallDefineSubState.EAS_EnterNQRunToNULLBallDefine.ToString();

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
            m_AnistateSubName = NetAniNullBallDefineSubState.EAS_EnterSpecialIdleToNULLBallDefine90.ToString();
        }
        else if (_Rdata == RoundData.Round180)
        {
            m_AnistateSubName = NetAniNullBallDefineSubState.EAS_EnterSpecialIdleToNULLBallDefine180.ToString();

        }
        else
            m_AnistateSubName = NetAniNullBallDefineSubState.EAS_EnterSpecialIdleToNULLBallDefine.ToString();
    }
}
