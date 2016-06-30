using System.Collections;
using Common;
using System;
using Common.Log;
/// <summary>
/// 常速带球行为
/// </summary>
public class NetAniLowCarryState : NetAniCarryBaseState
{

    private enum NetAniLowCarrySubState
    {
        EAS_EnterToLowCarry,
        EAS_EnterToLowCarry90,
        EAS_EnterToLowCarry180,

        EAS_EnterNormalCarryToLowCarry,
        EAS_EnterNormalCarryToLowCarry90,
        EAS_EnterNormalCarryToLowCarry180,
        EAS_EnterGetFloorBallToLowCarryBall,
        EAS_EnterGetFloorBallToLowCarryBall90,
        EAS_EnterGetFloorBallToLowCarryBall180,
        //胸部停球接地面低速带球//
        EAS_EnterHeadRobSuccessStopToLowCarryBall,
        EAS_EnterHeadRobSuccessStopToLowCarryBall90,
        EAS_EnterHeadRobSuccessStopToLowCarryBall180,



    }
    public NetAniLowCarryState()
        : base(EAniState.LowDribble)
    {

    }

    protected override void OnBegin()
    {
        switch (m_kPreState)
        {
            case EAniState.NormalDribbl:
                NormalDribbleStateChange(m_RoateType);
                break;
            case EAniState.LowDribble:
                LowDribbleStateChange(m_RoateType);
                break;
            case EAniState.Catch_GroundBall:
                FloorBallStateChange(m_RoateType);
                break;
            case EAniState.HeadRob_Carry:
            case EAniState.HeadRob2_Stop:
                HeadRobSuccessStopStateChange(m_RoateType);
                break;
        }
        base.OnBegin();
    }

    private void NormalDribbleStateChange(RoundData _rData)
    {
        if (_rData == RoundData.Round90)
        {
            m_AnistateSubName = NetAniLowCarrySubState.EAS_EnterNormalCarryToLowCarry90.ToString();
        }
        else if (_rData == RoundData.Round180)
        {
            m_AnistateSubName = NetAniLowCarrySubState.EAS_EnterNormalCarryToLowCarry180.ToString();
        }
        else
        {
            m_AnistateSubName = NetAniLowCarrySubState.EAS_EnterNormalCarryToLowCarry.ToString();
        }
    }
    private void LowDribbleStateChange(RoundData _rData)
    {
        if (_rData == RoundData.Round90)
        {
            m_AnistateSubName = NetAniLowCarrySubState.EAS_EnterToLowCarry90.ToString();

        }
        else if (_rData == RoundData.Round180)
        {
            m_AnistateSubName = NetAniLowCarrySubState.EAS_EnterToLowCarry180.ToString();

        }
        else
        {
            m_AnistateSubName = NetAniLowCarrySubState.EAS_EnterToLowCarry.ToString();

        }
    }

    private void FloorBallStateChange(RoundData _rData)
    {
        if (_rData == RoundData.Round90)
        {
            m_AnistateSubName = NetAniLowCarrySubState.EAS_EnterGetFloorBallToLowCarryBall90.ToString();

        }
        else if (_rData == RoundData.Round180)
        {
            m_AnistateSubName = NetAniLowCarrySubState.EAS_EnterGetFloorBallToLowCarryBall180.ToString();

        }
        else
        {
            m_AnistateSubName = NetAniLowCarrySubState.EAS_EnterGetFloorBallToLowCarryBall.ToString();

        }
    }
    private void HeadRobSuccessStopStateChange(RoundData _rData)
    {
        if (_rData == RoundData.Round90)
        {
            m_AnistateSubName = NetAniLowCarrySubState.EAS_EnterHeadRobSuccessStopToLowCarryBall90.ToString();

        }
        else if (_rData == RoundData.Round180)
        {
            m_AnistateSubName = NetAniLowCarrySubState.EAS_EnterHeadRobSuccessStopToLowCarryBall180.ToString();

        }
        else
        {
            m_AnistateSubName = NetAniLowCarrySubState.EAS_EnterHeadRobSuccessStopToLowCarryBall.ToString();

        }
    }
}
