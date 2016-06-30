using BehaviourTree;
using Common;
using Common.Tables;
using System;
using System.Collections.Generic;
using Common.Log;
public enum NetAniPassBallSubState
{
    //低速带球 //
    EAS_EnterLowCarryToFacePassFloorBall,
    EAS_EnterLowCarryToSlantPassFloorBall,
    EAS_EnterLowCarryToFacePassHighBall,
    EAS_EnterLowCarryToSlantPassHighBall,
    EAS_EnterLowCarryToFacePassFloorBall90,
    EAS_EnterLowCarryToSlantPassFloorBall90,
    EAS_EnterLowCarryToFacePassHighBall90,
    EAS_EnterLowCarryToSlantPassHighBall90,
    EAS_EnterLowCarryToFacePassFloorBall180,
    EAS_EnterLowCarryToSlantPassFloorBall180,
    EAS_EnterLowCarryToFacePassHighBall180,
    EAS_EnterLowCarryToSlantPassHighBall180,

    //常速带球 //
    EAS_EnterNormalCarryToFacePassFloorBall,
    EAS_EnterNormalCarryToSlantPassFloorBall,
    EAS_EnterNormalCarryToFacePassHighBall,
    EAS_EnterNormalCarryToSlantPassHighBall,
    EAS_EnterNormalCarryToFacePassFloorBall90,
    EAS_EnterNormalCarryToSlantPassFloorBall90,
    EAS_EnterNormalCarryToFacePassHighBall90,
    EAS_EnterNormalCarryToSlantPassHighBall90,
    EAS_EnterNormalCarryToFacePassFloorBall180,
    EAS_EnterNormalCarryToSlantPassFloorBall180,
    EAS_EnterNormalCarryToFacePassHighBall180,
    EAS_EnterNormalCarryToSlantPassHighBall180,

    //突破成功 //
    EAS_EnterBreakOutSuccessToFacePassFloorBall,
    EAS_EnterBreakOutSuccessToSlantPassFloorBall,
    EAS_EnterBreakOutSuccessToFacePassHighBall,
    EAS_EnterBreakOutSuccessToSlantPassHighBall,
    EAS_EnterBreakOutSuccessToFacePassFloorBall90,
    EAS_EnterBreakOutSuccessToSlantPassFloorBall90,
    EAS_EnterBreakOutSuccessToFacePassHighBall90,
    EAS_EnterBreakOutSuccessToSlantPassHighBall90,
    EAS_EnterBreakOutSuccessToFacePassFloorBall180,
    EAS_EnterBreakOutSuccessToSlantPassFloorBall180,
    EAS_EnterBreakOutSuccessToFacePassHighBall180,
    EAS_EnterBreakOutSuccessToSlantPassHighBall180,

    //接地面球 //
    EAS_EnterGetFloorBallToFacePassFloorBall,
    EAS_EnterGetFloorBallToSlantPassFloorBall,
    EAS_EnterGetFloorBallToFacePassHighBall,
    EAS_EnterGetFloorBallToSlantPassHighBall,
    EAS_EnterGetFloorBallToFacePassFloorBall90,
    EAS_EnterGetFloorBallToSlantPassFloorBall90,
    EAS_EnterGetFloorBallToFacePassHighBall90,
    EAS_EnterGetFloorBallToSlantPassHighBall90,
    EAS_EnterGetFloorBallToFacePassFloorBall180,
    EAS_EnterGetFloorBallToSlantPassFloorBall180,
    EAS_EnterGetFloorBallToFacePassHighBall180,
    EAS_EnterGetFloorBallToSlantPassHighBall180,

    // 抢截事件成功//
    EAS_EnterTackleSuccessToFacePassFloorBall,
    EAS_EnterTackleSuccessToSlantPassFloorBall,
    EAS_EnterTackleSuccessToFacePassHighBall,
    EAS_EnterTackleSuccessToSlantPassHighBall,
    EAS_EnterTackleSuccessToFacePassFloorBall90,
    EAS_EnterTackleSuccessToSlantPassFloorBall90,
    EAS_EnterTackleSuccessToFacePassHighBall90,
    EAS_EnterTackleSuccessToSlantPassHighBall90,
    EAS_EnterTackleSuccessToFacePassFloorBall180,
    EAS_EnterTackleSuccessToSlantPassFloorBall180,
    EAS_EnterTackleSuccessToFacePassHighBall180,
    EAS_EnterTackleSuccessToSlantPassHighBall180,

    // 铲断事件成功//
    EAS_EnterSliderTackleToFacePassFloorBall,
    EAS_EnterSliderTackleToSlantPassFloorBall,
    EAS_EnterSliderTackleToFacePassHighBall,
    EAS_EnterSliderTackleToSlantPassHighBall,
    EAS_EnterSliderTackleToFacePassFloorBall90,
    EAS_EnterSliderTackleToSlantPassFloorBall90,
    EAS_EnterSliderTackleToFacePassHighBall90,
    EAS_EnterSliderTackleToSlantPassHighBall90,
    EAS_EnterSliderTackleToFacePassFloorBall180,
    EAS_EnterSliderTackleToSlantPassFloorBall180,
    EAS_EnterSliderTackleToFacePassHighBall180,
    EAS_EnterSliderTackleToSlantPassHighBall180,

    //阻止突破事件成功//
    EAS_EnterStopBreakOutSuccessToFacePassFloorBall,
    EAS_EnterStopBreakOutSuccessToSlantPassFloorBall,
    EAS_EnterStopBreakOutSuccessToFacePassHighBall,
    EAS_EnterStopBreakOutSuccessToSlantPassHighBall,
    EAS_EnterStopBreakOutSuccessToFacePassFloorBall90,
    EAS_EnterStopBreakOutSuccessToSlantPassFloorBall90,
    EAS_EnterStopBreakOutSuccessToFacePassHighBall90,
    EAS_EnterStopBreakOutSuccessToSlantPassHighBall90,
    EAS_EnterStopBreakOutSuccessToFacePassFloorBall180,
    EAS_EnterStopBreakOutSuccessToSlantPassFloorBall180,
    EAS_EnterStopBreakOutSuccessToFacePassHighBall180,
    EAS_EnterStopBreakOutSuccessToSlantPassHighBall180,


    //胸部停球接地面传球//
    EAS_EnterHeadRobSuccessStopToFacePassFloorBall,
    EAS_EnterHeadRobSuccessStopToSlantPassFloorBall,
    EAS_EnterHeadRobSuccessStopToFacePassHighBall,
    EAS_EnterHeadRobSuccessStopToSlantPassHighBall,
    EAS_EnterHeadRobSuccessStopToFacePassFloorBall90,
    EAS_EnterHeadRobSuccessStopToSlantPassFloorBall90,
    EAS_EnterHeadRobSuccessStopToFacePassHighBall90,
    EAS_EnterHeadRobSuccessStopToSlantPassHighBall90,
    EAS_EnterHeadRobSuccessStopToFacePassFloorBall180,
    EAS_EnterHeadRobSuccessStopToSlantPassFloorBall180,
    EAS_EnterHeadRobSuccessStopToFacePassHighBall180,
    EAS_EnterHeadRobSuccessStopToSlantPassHighBall180,


    //拦截事件成功//
    EAS_EnterInterceptSuccessToNormalCarryBall180,
    EAS_EnterInterceptSuccessToFacePassFloorBall,
    EAS_EnterInterceptSuccessToSlantPassFloorBall,
    EAS_EnterInterceptSuccessToFacePassHighBall,
    EAS_EnterInterceptSuccessToSlantPassHighBall,
    EAS_EnterInterceptSuccessToFacePassFloorBall90,
    EAS_EnterInterceptSuccessToSlantPassFloorBall90,
    EAS_EnterInterceptSuccessToFacePassHighBall90,
    EAS_EnterInterceptSuccessToSlantPassHighBall90,
    EAS_EnterInterceptSuccessToFacePassFloorBall180,
    EAS_EnterInterceptSuccessToSlantPassFloorBall180,
    EAS_EnterInterceptSuccessToFacePassHighBall180,
    EAS_EnterInterceptSuccessToSlantPassHighBall180,

    //躲过抢截事件成功//
    EAS_EnterStopTackleToFacePassFloorBall,
    EAS_EnterStopTackleToSlantPassFloorBall,
    EAS_EnterStopTackleToFacePassHighBall,
    EAS_EnterStopTackleToSlantPassHighBall,
    EAS_EnterStopTackleToFacePassFloorBall90,
    EAS_EnterStopTackleToSlantPassFloorBall90,
    EAS_EnterStopTackleToFacePassHighBall90,
    EAS_EnterStopTackleToSlantPassHighBall90,
    EAS_EnterStopTackleToFacePassFloorBall180,
    EAS_EnterStopTackleToSlantPassFloorBall180,
    EAS_EnterStopTackleToFacePassHighBall180,
    EAS_EnterStopTackleToSlantPassHighBall180,

    //躲过铲断事件成功 //
    EAS_EnterStopSliderTackleToFacePassFloorBall,
    EAS_EnterStopSliderTackleToSlantPassFloorBall,
    EAS_EnterStopSliderTackleToFacePassHighBall,
    EAS_EnterStopSliderTackleToSlantPassHighBall,
    EAS_EnterStopSliderTackleToFacePassFloorBall90,
    EAS_EnterStopSliderTackleToSlantPassFloorBall90,
    EAS_EnterStopSliderTackleToFacePassHighBall90,
    EAS_EnterStopSliderTackleToSlantPassHighBall90,
    EAS_EnterStopSliderTackleToFacePassFloorBall180,
    EAS_EnterStopSliderTackleToSlantPassFloorBall180,
    EAS_EnterStopSliderTackleToFacePassHighBall180,
    EAS_EnterStopSliderTackleToSlantPassHighBall180,

}

/// <summary>
/// 传地面球
/// </summary>
public class NetAniPassBallFloorState : NetAniPassBaseState
{

    public NetAniPassBallFloorState()
        : base(EAniState.PassBall_Floor)
    {

    }
    protected override void OnBegin()
    {
        switch (m_kPreState)
        {
            case EAniState.LowDribble:
                LowDribbleStateChange(m_RoateType, m_bFaceTarget);
                break;
            case EAniState.NormalDribbl:
                NormalDribbleStateChange(m_RoateType, m_bFaceTarget);
                break;
            case EAniState.Catch_GroundBall:
                CatchGroundBallStateChange(m_RoateType, m_bFaceTarget);
                break;
            case EAniState.Break_Through:  //突破成功//
                BreakThroughStateChange(m_RoateType, m_bFaceTarget);
                break;
            case EAniState.Stop_BreakThrough_Success:   //阻止突破成功//
                AvoidBreakThroughStateChange(m_RoateType, m_bFaceTarget);
                break;
            case EAniState.Ground_Tackle: //铲断成功//
                SlickTackleSuccessChange(m_RoateType, m_bFaceTarget);
                break;
            case EAniState.Stop_Ground_Tackle_Success: //躲避铲断成功//
                AvoidSliderTackleSuccessChange(m_RoateType, m_bFaceTarget);
                break;
            case EAniState.Ground_Snatch:  //抢截事件成功//
                SnatchSuccessChange(m_RoateType, m_bFaceTarget);
                break;
            case EAniState.Stop_Ground_Snatch_Success:  //躲避抢截事件成功//
                AvoidSnatchSuccessChange(m_RoateType, m_bFaceTarget);
                break;
            case EAniState.Ground_Block:  //拦截成功//
                BlockSuccessChange(m_RoateType, m_bFaceTarget);
                break;
            case EAniState.HeadRob_Carry:
            case EAniState.HeadRob2_Stop:
                HeadRobSuccessStopSuccessChange(m_RoateType, m_bFaceTarget);
                break;
        }
        base.OnBegin();
    }
    private void LowDribbleStateChange(RoundData _Rdata, bool _face)
    {
        if (_Rdata == RoundData.Round90)
        {
            if (_face)
            {
                m_AnistateSubName = NetAniPassBallSubState.EAS_EnterLowCarryToFacePassFloorBall90.ToString();
            }
            else
            {
                m_AnistateSubName = NetAniPassBallSubState.EAS_EnterLowCarryToSlantPassFloorBall90.ToString();
            }
        }
        else if (_Rdata == RoundData.Round180)
        {
            if (_face)
            {
                m_AnistateSubName = NetAniPassBallSubState.EAS_EnterLowCarryToFacePassFloorBall180.ToString();
            }
            else
            {
                m_AnistateSubName = NetAniPassBallSubState.EAS_EnterLowCarryToSlantPassFloorBall180.ToString();
            }
        }
        else
        {
            if (_face)
            {
                m_AnistateSubName = NetAniPassBallSubState.EAS_EnterLowCarryToFacePassFloorBall.ToString();
            }
            else
            {
                m_AnistateSubName = NetAniPassBallSubState.EAS_EnterLowCarryToSlantPassFloorBall.ToString();
            }
        }
    }
    private void NormalDribbleStateChange(RoundData _Rdata, bool _face)
    {
        if (_Rdata == RoundData.Round90)
        {
            if (_face)
            {
                m_AnistateSubName = NetAniPassBallSubState.EAS_EnterNormalCarryToFacePassFloorBall90.ToString();
            }
            else
            {
                m_AnistateSubName = NetAniPassBallSubState.EAS_EnterNormalCarryToSlantPassFloorBall90.ToString();
            }
        }
        else if (_Rdata == RoundData.Round180)
        {
            if (_face)
            {
                m_AnistateSubName = NetAniPassBallSubState.EAS_EnterNormalCarryToFacePassFloorBall180.ToString();
            }
            else
            {
                m_AnistateSubName = NetAniPassBallSubState.EAS_EnterNormalCarryToSlantPassFloorBall180.ToString();
            }
        }
        else
        {
            if (_face)
            {
                m_AnistateSubName = NetAniPassBallSubState.EAS_EnterNormalCarryToFacePassFloorBall.ToString();
            }
            else
            {
                m_AnistateSubName = NetAniPassBallSubState.EAS_EnterNormalCarryToSlantPassFloorBall.ToString();
            }
        }
    }
    private void CatchGroundBallStateChange(RoundData _Rdata, bool _face)
    {
        if (_Rdata == RoundData.Round90)
        {
            if (_face)
            {
                m_AnistateSubName = NetAniPassBallSubState.EAS_EnterGetFloorBallToFacePassFloorBall90.ToString();
            }
            else
            {
                m_AnistateSubName = NetAniPassBallSubState.EAS_EnterGetFloorBallToSlantPassFloorBall90.ToString();
            }
        }
        else if (_Rdata == RoundData.Round180)
        {
            if (_face)
            {
                m_AnistateSubName = NetAniPassBallSubState.EAS_EnterGetFloorBallToFacePassFloorBall180.ToString();
            }
            else
            {
                m_AnistateSubName = NetAniPassBallSubState.EAS_EnterGetFloorBallToSlantPassFloorBall180.ToString();
            }
        }
        else
        {
            if (_face)
            {
                m_AnistateSubName = NetAniPassBallSubState.EAS_EnterGetFloorBallToFacePassFloorBall.ToString();
            }
            else
            {
                m_AnistateSubName = NetAniPassBallSubState.EAS_EnterGetFloorBallToSlantPassFloorBall.ToString();
            }
        }
    }

    private void BreakThroughStateChange(RoundData _Rdata, bool _face)
    {
        if (_Rdata == RoundData.Round90)
        {
            if (_face)
            {
                m_AnistateSubName = NetAniPassBallSubState.EAS_EnterBreakOutSuccessToFacePassFloorBall90.ToString();
            }
            else
            {
                m_AnistateSubName = NetAniPassBallSubState.EAS_EnterBreakOutSuccessToSlantPassFloorBall90.ToString();
            }
        }
        else if (_Rdata == RoundData.Round180)
        {
            if (_face)
            {
                m_AnistateSubName = NetAniPassBallSubState.EAS_EnterBreakOutSuccessToFacePassFloorBall180.ToString();
            }
            else
            {
                m_AnistateSubName = NetAniPassBallSubState.EAS_EnterBreakOutSuccessToSlantPassFloorBall180.ToString();
            }
        }
        else
        {
            if (_face)
            {
                m_AnistateSubName = NetAniPassBallSubState.EAS_EnterBreakOutSuccessToFacePassFloorBall.ToString();
            }
            else
            {
                m_AnistateSubName = NetAniPassBallSubState.EAS_EnterBreakOutSuccessToSlantPassFloorBall.ToString();
            }
        }
    }

    private void AvoidBreakThroughStateChange(RoundData _Rdata, bool _face)
    {
        if (_Rdata == RoundData.Round90)
        {
            if (_face)
            {
                m_AnistateSubName = NetAniPassBallSubState.EAS_EnterStopBreakOutSuccessToFacePassFloorBall90.ToString();
            }
            else
            {
                m_AnistateSubName = NetAniPassBallSubState.EAS_EnterStopBreakOutSuccessToSlantPassFloorBall90.ToString();
            }
        }
        else if (_Rdata == RoundData.Round180)
        {
            if (_face)
            {
                m_AnistateSubName = NetAniPassBallSubState.EAS_EnterStopBreakOutSuccessToFacePassFloorBall180.ToString();
            }
            else
            {
                m_AnistateSubName = NetAniPassBallSubState.EAS_EnterStopBreakOutSuccessToSlantPassFloorBall180.ToString();
            }
        }
        else
        {
            if (_face)
            {
                m_AnistateSubName = NetAniPassBallSubState.EAS_EnterStopBreakOutSuccessToFacePassFloorBall.ToString();
            }
            else
            {
                m_AnistateSubName = NetAniPassBallSubState.EAS_EnterStopBreakOutSuccessToSlantPassFloorBall.ToString();
            }
        }
    }
    private void SlickTackleSuccessChange(RoundData _Rdata, bool _face)
    {
        if (_Rdata == RoundData.Round90)
        {
            if (_face)
            {
                m_AnistateSubName = NetAniPassBallSubState.EAS_EnterSliderTackleToFacePassFloorBall90.ToString();
            }
            else
            {
                m_AnistateSubName = NetAniPassBallSubState.EAS_EnterNormalCarryToSlantPassFloorBall90.ToString();
            }
        }
        else if (_Rdata == RoundData.Round180)
        {
            if (_face)
            {
                m_AnistateSubName = NetAniPassBallSubState.EAS_EnterSliderTackleToFacePassFloorBall180.ToString();
            }
            else
            {
                m_AnistateSubName = NetAniPassBallSubState.EAS_EnterSliderTackleToSlantPassFloorBall180.ToString();
            }
        }
        else
        {
            if (_face)
            {
                m_AnistateSubName = NetAniPassBallSubState.EAS_EnterSliderTackleToFacePassFloorBall.ToString();
            }
            else
            {
                m_AnistateSubName = NetAniPassBallSubState.EAS_EnterSliderTackleToSlantPassFloorBall.ToString();
            }
        }
    }

    private void AvoidSliderTackleSuccessChange(RoundData _Rdata, bool _face)
    {
        if (_Rdata == RoundData.Round90)
        {
            if (_face)
            {
                m_AnistateSubName = NetAniPassBallSubState.EAS_EnterStopSliderTackleToFacePassFloorBall90.ToString();
            }
            else
            {
                m_AnistateSubName = NetAniPassBallSubState.EAS_EnterStopSliderTackleToSlantPassFloorBall90.ToString();
            }
        }
        else if (_Rdata == RoundData.Round180)
        {
            if (_face)
            {
                m_AnistateSubName = NetAniPassBallSubState.EAS_EnterStopSliderTackleToFacePassFloorBall180.ToString();
            }
            else
            {
                m_AnistateSubName = NetAniPassBallSubState.EAS_EnterStopSliderTackleToSlantPassFloorBall180.ToString();
            }
        }
        else
        {
            if (_face)
            {
                m_AnistateSubName = NetAniPassBallSubState.EAS_EnterStopSliderTackleToFacePassFloorBall.ToString();
            }
            else
            {
                m_AnistateSubName = NetAniPassBallSubState.EAS_EnterStopSliderTackleToSlantPassFloorBall.ToString();
            }
        }
    }

    private void HeadRobSuccessStopSuccessChange(RoundData _Rdata, bool _face)
    {
        if (_Rdata == RoundData.Round90)
        {
            if (_face)
            {
                m_AnistateSubName = NetAniPassBallSubState.EAS_EnterHeadRobSuccessStopToFacePassFloorBall90.ToString();
            }
            else
            {
                m_AnistateSubName = NetAniPassBallSubState.EAS_EnterHeadRobSuccessStopToSlantPassFloorBall90.ToString();
            }
        }
        else if (_Rdata == RoundData.Round180)
        {
            if (_face)
            {
                m_AnistateSubName = NetAniPassBallSubState.EAS_EnterHeadRobSuccessStopToFacePassFloorBall180.ToString();
            }
            else
            {
                m_AnistateSubName = NetAniPassBallSubState.EAS_EnterHeadRobSuccessStopToSlantPassFloorBall180.ToString();
            }
        }
        else
        {
            if (_face)
            {
                m_AnistateSubName = NetAniPassBallSubState.EAS_EnterHeadRobSuccessStopToFacePassFloorBall.ToString();
            }
            else
            {
                m_AnistateSubName = NetAniPassBallSubState.EAS_EnterHeadRobSuccessStopToSlantPassFloorBall.ToString();
            }
        }
    }
    private void SnatchSuccessChange(RoundData _Rdata, bool _face)
    {
        if (_Rdata == RoundData.Round90)
        {
            if (_face)
            {
                m_AnistateSubName = NetAniPassBallSubState.EAS_EnterTackleSuccessToFacePassFloorBall90.ToString();
            }
            else
            {
                m_AnistateSubName = NetAniPassBallSubState.EAS_EnterTackleSuccessToSlantPassFloorBall90.ToString();
            }
        }
        else if (_Rdata == RoundData.Round180)
        {
            if (_face)
            {
                m_AnistateSubName = NetAniPassBallSubState.EAS_EnterTackleSuccessToFacePassFloorBall180.ToString();
            }
            else
            {
                m_AnistateSubName = NetAniPassBallSubState.EAS_EnterTackleSuccessToSlantPassFloorBall180.ToString();
            }
        }
        else
        {
            if (_face)
            {
                m_AnistateSubName = NetAniPassBallSubState.EAS_EnterTackleSuccessToFacePassFloorBall.ToString();
            }
            else
            {
                m_AnistateSubName = NetAniPassBallSubState.EAS_EnterTackleSuccessToSlantPassFloorBall.ToString();
            }
        }
    }
    private void AvoidSnatchSuccessChange(RoundData _Rdata, bool _face)
    {
        if (_Rdata == RoundData.Round90)
        {
            if (_face)
            {
                m_AnistateSubName = NetAniPassBallSubState.EAS_EnterStopTackleToFacePassFloorBall90.ToString();
            }
            else
            {
                m_AnistateSubName = NetAniPassBallSubState.EAS_EnterStopTackleToSlantPassFloorBall90.ToString();
            }
        }
        else if (_Rdata == RoundData.Round180)
        {
            if (_face)
            {
                m_AnistateSubName = NetAniPassBallSubState.EAS_EnterStopTackleToFacePassFloorBall180.ToString();
            }
            else
            {
                m_AnistateSubName = NetAniPassBallSubState.EAS_EnterStopTackleToSlantPassFloorBall180.ToString();
            }
        }
        else
        {
            if (_face)
            {
                m_AnistateSubName = NetAniPassBallSubState.EAS_EnterStopTackleToFacePassFloorBall.ToString();
            }
            else
            {
                m_AnistateSubName = NetAniPassBallSubState.EAS_EnterStopTackleToSlantPassFloorBall.ToString();
            }
        }
    }
    private void BlockSuccessChange(RoundData _Rdata, bool _face)
    {
        if (_Rdata == RoundData.Round90)
        {
            if (_face)
            {
                m_AnistateSubName = NetAniPassBallSubState.EAS_EnterInterceptSuccessToFacePassFloorBall90.ToString();
            }
            else
            {
                m_AnistateSubName = NetAniPassBallSubState.EAS_EnterInterceptSuccessToSlantPassFloorBall90.ToString();
            }
        }
        else if (_Rdata == RoundData.Round180)
        {
            if (_face)
            {
                m_AnistateSubName = NetAniPassBallSubState.EAS_EnterInterceptSuccessToFacePassFloorBall180.ToString();
            }
            else
            {
                m_AnistateSubName = NetAniPassBallSubState.EAS_EnterInterceptSuccessToSlantPassFloorBall180.ToString();
            }
        }
        else
        {
            if (_face)
            {
                m_AnistateSubName = NetAniPassBallSubState.EAS_EnterInterceptSuccessToFacePassFloorBall.ToString();
            }
            else
            {
                m_AnistateSubName = NetAniPassBallSubState.EAS_EnterInterceptSuccessToSlantPassFloorBall.ToString();
            }
        }
    }

}

/// <summary>
/// 传高空球
/// </summary>
public class NetAniPassBallHighState : NetAniPassBaseState
{

    public NetAniPassBallHighState()
        : base(EAniState.PassBall_High)
    {

    }

    protected override void OnBegin()
    {
        m_kPreState = EAniState.NormalDribbl;
        switch (m_kPreState)
        {
            case EAniState.LowDribble:
                LowDribbleStateChange(m_RoateType, m_bFaceTarget);
                break;
            case EAniState.NormalDribbl:
                NormalDribbleStateChange(m_RoateType, m_bFaceTarget);
                break;
            case EAniState.Catch_GroundBall:
                CatchGroundBallStateChange(m_RoateType, m_bFaceTarget);
                break;
            case EAniState.Break_Through:  //突破成功//
                BreakThroughStateChange(m_RoateType, m_bFaceTarget);
                break;
            case EAniState.Stop_BreakThrough_Success:   //阻止突破成功//
                AvoidBreakThroughStateChange(m_RoateType, m_bFaceTarget);
                break;
            case EAniState.Ground_Tackle: //铲断成功//
                SlickTackleSuccessChange(m_RoateType, m_bFaceTarget);
                break;
            case EAniState.Stop_Ground_Tackle_Success: //躲避铲断成功//
                AvoidSliderTackleSuccessChange(m_RoateType, m_bFaceTarget);
                break;
            case EAniState.Ground_Snatch:  //抢截事件成功//
                SnatchSuccessChange(m_RoateType, m_bFaceTarget);
                break;
            case EAniState.Stop_Ground_Snatch_Success:  //躲避抢截事件成功//
                AvoidSnatchSuccessChange(m_RoateType, m_bFaceTarget);
                break;
            case EAniState.Ground_Block:  //拦截成功//
                BlockSuccessChange(m_RoateType, m_bFaceTarget);
                break;
            case EAniState.HeadRob_Carry://胸部停球
            case EAniState.HeadRob2_Stop: //头球争顶2段成功，头球摆渡//
                HeadRobSuccessStopChange(m_RoateType, m_bFaceTarget);
                break;
        }

        base.OnBegin();
    }


    private void LowDribbleStateChange(RoundData _Rdata, bool _face)
    {
        if (_Rdata == RoundData.Round90)
        {
            if (_face)
            {
                m_AnistateSubName = NetAniPassBallSubState.EAS_EnterLowCarryToFacePassHighBall90.ToString();
            }
            else
            {
                m_AnistateSubName = NetAniPassBallSubState.EAS_EnterLowCarryToSlantPassHighBall90.ToString();
            }
        }
        else if (_Rdata == RoundData.Round180)
        {
            if (_face)
            {
                m_AnistateSubName = NetAniPassBallSubState.EAS_EnterLowCarryToFacePassHighBall180.ToString();
            }
            else
            {
                m_AnistateSubName = NetAniPassBallSubState.EAS_EnterLowCarryToSlantPassHighBall180.ToString();
            }
        }
        else
        {
            if (_face)
            {
                m_AnistateSubName = NetAniPassBallSubState.EAS_EnterLowCarryToFacePassHighBall.ToString();
            }
            else
            {
                m_AnistateSubName = NetAniPassBallSubState.EAS_EnterLowCarryToSlantPassHighBall.ToString();
            }
        }
    }
    private void NormalDribbleStateChange(RoundData _Rdata, bool _face)
    {
        if (_Rdata == RoundData.Round90)
        {
            if (_face)
            {
                m_AnistateSubName = NetAniPassBallSubState.EAS_EnterNormalCarryToFacePassHighBall90.ToString();
            }
            else
            {
                m_AnistateSubName = NetAniPassBallSubState.EAS_EnterNormalCarryToSlantPassHighBall90.ToString();
            }
        }
        else if (_Rdata == RoundData.Round180)
        {
            if (_face)
            {
                m_AnistateSubName = NetAniPassBallSubState.EAS_EnterNormalCarryToFacePassHighBall180.ToString();
            }
            else
            {
                m_AnistateSubName = NetAniPassBallSubState.EAS_EnterNormalCarryToSlantPassHighBall180.ToString();
            }
        }
        else
        {
            if (_face)
            {
                m_AnistateSubName = NetAniPassBallSubState.EAS_EnterNormalCarryToFacePassHighBall.ToString();
            }
            else
            {
                m_AnistateSubName = NetAniPassBallSubState.EAS_EnterNormalCarryToSlantPassHighBall.ToString();
            }
        }
    }
    private void CatchGroundBallStateChange(RoundData _Rdata, bool _face)
    {
        if (_Rdata == RoundData.Round90)
        {
            if (_face)
            {
                m_AnistateSubName = NetAniPassBallSubState.EAS_EnterGetFloorBallToFacePassHighBall90.ToString();
            }
            else
            {
                m_AnistateSubName = NetAniPassBallSubState.EAS_EnterGetFloorBallToSlantPassHighBall90.ToString();
            }
        }
        else if (_Rdata == RoundData.Round180)
        {
            if (_face)
            {
                m_AnistateSubName = NetAniPassBallSubState.EAS_EnterGetFloorBallToFacePassHighBall180.ToString();
            }
            else
            {
                m_AnistateSubName = NetAniPassBallSubState.EAS_EnterGetFloorBallToSlantPassHighBall180.ToString();
            }
        }
        else
        {
            if (_face)
            {
                m_AnistateSubName = NetAniPassBallSubState.EAS_EnterGetFloorBallToFacePassHighBall.ToString();
            }
            else
            {
                m_AnistateSubName = NetAniPassBallSubState.EAS_EnterGetFloorBallToSlantPassHighBall.ToString();
            }
        }
    }

    private void BreakThroughStateChange(RoundData _Rdata, bool _face)
    {
        if (_Rdata == RoundData.Round90)
        {
            if (_face)
            {
                m_AnistateSubName = NetAniPassBallSubState.EAS_EnterBreakOutSuccessToFacePassHighBall90.ToString();
            }
            else
            {
                m_AnistateSubName = NetAniPassBallSubState.EAS_EnterBreakOutSuccessToSlantPassHighBall90.ToString();
            }
        }
        else if (_Rdata == RoundData.Round180)
        {
            if (_face)
            {
                m_AnistateSubName = NetAniPassBallSubState.EAS_EnterBreakOutSuccessToFacePassHighBall180.ToString();
            }
            else
            {
                m_AnistateSubName = NetAniPassBallSubState.EAS_EnterBreakOutSuccessToSlantPassHighBall180.ToString();
            }
        }
        else
        {
            if (_face)
            {
                m_AnistateSubName = NetAniPassBallSubState.EAS_EnterBreakOutSuccessToFacePassHighBall.ToString();
            }
            else
            {
                m_AnistateSubName = NetAniPassBallSubState.EAS_EnterBreakOutSuccessToSlantPassHighBall.ToString();
            }
        }
    }

    private void AvoidBreakThroughStateChange(RoundData _Rdata, bool _face)
    {
        if (_Rdata == RoundData.Round90)
        {
            if (_face)
            {
                m_AnistateSubName = NetAniPassBallSubState.EAS_EnterStopBreakOutSuccessToFacePassHighBall90.ToString();
            }
            else
            {
                m_AnistateSubName = NetAniPassBallSubState.EAS_EnterStopBreakOutSuccessToSlantPassHighBall90.ToString();
            }
        }
        else if (_Rdata == RoundData.Round180)
        {
            if (_face)
            {
                m_AnistateSubName = NetAniPassBallSubState.EAS_EnterStopBreakOutSuccessToFacePassHighBall180.ToString();
            }
            else
            {
                m_AnistateSubName = NetAniPassBallSubState.EAS_EnterStopBreakOutSuccessToSlantPassHighBall180.ToString();
            }
        }
        else
        {
            if (_face)
            {
                m_AnistateSubName = NetAniPassBallSubState.EAS_EnterStopBreakOutSuccessToFacePassHighBall.ToString();
            }
            else
            {
                m_AnistateSubName = NetAniPassBallSubState.EAS_EnterStopBreakOutSuccessToSlantPassHighBall.ToString();
            }
        }
    }
    private void SlickTackleSuccessChange(RoundData _Rdata, bool _face)
    {
        if (_Rdata == RoundData.Round90)
        {
            if (_face)
            {
                m_AnistateSubName = NetAniPassBallSubState.EAS_EnterSliderTackleToFacePassHighBall90.ToString();
            }
            else
            {
                m_AnistateSubName = NetAniPassBallSubState.EAS_EnterNormalCarryToSlantPassHighBall90.ToString();
            }
        }
        else if (_Rdata == RoundData.Round180)
        {
            if (_face)
            {
                m_AnistateSubName = NetAniPassBallSubState.EAS_EnterSliderTackleToFacePassHighBall180.ToString();
            }
            else
            {
                m_AnistateSubName = NetAniPassBallSubState.EAS_EnterSliderTackleToSlantPassHighBall180.ToString();
            }
        }
        else
        {
            if (_face)
            {
                m_AnistateSubName = NetAniPassBallSubState.EAS_EnterSliderTackleToFacePassHighBall.ToString();
            }
            else
            {
                m_AnistateSubName = NetAniPassBallSubState.EAS_EnterSliderTackleToSlantPassHighBall.ToString();
            }
        }
    }

    private void AvoidSliderTackleSuccessChange(RoundData _Rdata, bool _face)
    {
        if (_Rdata == RoundData.Round90)
        {
            if (_face)
            {
                m_AnistateSubName = NetAniPassBallSubState.EAS_EnterStopSliderTackleToFacePassHighBall90.ToString();
            }
            else
            {
                m_AnistateSubName = NetAniPassBallSubState.EAS_EnterStopSliderTackleToSlantPassHighBall90.ToString();
            }
        }
        else if (_Rdata == RoundData.Round180)
        {
            if (_face)
            {
                m_AnistateSubName = NetAniPassBallSubState.EAS_EnterStopSliderTackleToFacePassHighBall180.ToString();
            }
            else
            {
                m_AnistateSubName = NetAniPassBallSubState.EAS_EnterStopSliderTackleToSlantPassHighBall180.ToString();
            }
        }
        else
        {
            if (_face)
            {
                m_AnistateSubName = NetAniPassBallSubState.EAS_EnterStopSliderTackleToFacePassHighBall.ToString();
            }
            else
            {
                m_AnistateSubName = NetAniPassBallSubState.EAS_EnterStopSliderTackleToSlantPassHighBall.ToString();
            }
        }
    }
    private void HeadRobSuccessStopChange(RoundData _Rdata, bool _face)
    {
        if (_Rdata == RoundData.Round90)
        {
            if (_face)
            {
                m_AnistateSubName = NetAniPassBallSubState.EAS_EnterHeadRobSuccessStopToFacePassHighBall90.ToString();
            }
            else
            {
                m_AnistateSubName = NetAniPassBallSubState.EAS_EnterHeadRobSuccessStopToSlantPassHighBall90.ToString();
            }
        }
        else if (_Rdata == RoundData.Round180)
        {
            if (_face)
            {
                m_AnistateSubName = NetAniPassBallSubState.EAS_EnterHeadRobSuccessStopToFacePassHighBall180.ToString();
            }
            else
            {
                m_AnistateSubName = NetAniPassBallSubState.EAS_EnterHeadRobSuccessStopToSlantPassHighBall180.ToString();
            }
        }
        else
        {
            if (_face)
            {
                m_AnistateSubName = NetAniPassBallSubState.EAS_EnterHeadRobSuccessStopToFacePassHighBall.ToString();
            }
            else
            {
                m_AnistateSubName = NetAniPassBallSubState.EAS_EnterHeadRobSuccessStopToSlantPassHighBall.ToString();
            }
        }
    }
    private void SnatchSuccessChange(RoundData _Rdata, bool _face)
    {
        if (_Rdata == RoundData.Round90)
        {
            if (_face)
            {
                m_AnistateSubName = NetAniPassBallSubState.EAS_EnterTackleSuccessToFacePassHighBall90.ToString();
            }
            else
            {
                m_AnistateSubName = NetAniPassBallSubState.EAS_EnterTackleSuccessToSlantPassHighBall90.ToString();
            }
        }
        else if (_Rdata == RoundData.Round180)
        {
            if (_face)
            {
                m_AnistateSubName = NetAniPassBallSubState.EAS_EnterTackleSuccessToFacePassHighBall180.ToString();
            }
            else
            {
                m_AnistateSubName = NetAniPassBallSubState.EAS_EnterTackleSuccessToSlantPassHighBall180.ToString();
            }
        }
        else
        {
            if (_face)
            {
                m_AnistateSubName = NetAniPassBallSubState.EAS_EnterTackleSuccessToFacePassHighBall.ToString();
            }
            else
            {
                m_AnistateSubName = NetAniPassBallSubState.EAS_EnterTackleSuccessToSlantPassHighBall.ToString();
            }
        }
    }
    private void AvoidSnatchSuccessChange(RoundData _Rdata, bool _face)
    {
        if (_Rdata == RoundData.Round90)
        {
            if (_face)
            {
                m_AnistateSubName = NetAniPassBallSubState.EAS_EnterStopTackleToFacePassHighBall90.ToString();
            }
            else
            {
                m_AnistateSubName = NetAniPassBallSubState.EAS_EnterStopTackleToSlantPassHighBall90.ToString();
            }
        }
        else if (_Rdata == RoundData.Round180)
        {
            if (_face)
            {
                m_AnistateSubName = NetAniPassBallSubState.EAS_EnterStopTackleToFacePassHighBall180.ToString();
            }
            else
            {
                m_AnistateSubName = NetAniPassBallSubState.EAS_EnterStopTackleToSlantPassHighBall180.ToString();
            }
        }
        else
        {
            if (_face)
            {
                m_AnistateSubName = NetAniPassBallSubState.EAS_EnterStopTackleToFacePassHighBall.ToString();
            }
            else
            {
                m_AnistateSubName = NetAniPassBallSubState.EAS_EnterStopTackleToSlantPassHighBall.ToString();
            }
        }
    }
    private void BlockSuccessChange(RoundData _Rdata, bool _face)
    {
        if (_Rdata == RoundData.Round90)
        {
            if (_face)
            {
                m_AnistateSubName = NetAniPassBallSubState.EAS_EnterInterceptSuccessToFacePassHighBall90.ToString();
            }
            else
            {
                m_AnistateSubName = NetAniPassBallSubState.EAS_EnterInterceptSuccessToSlantPassHighBall90.ToString();
            }
        }
        else if (_Rdata == RoundData.Round180)
        {
            if (_face)
            {
                m_AnistateSubName = NetAniPassBallSubState.EAS_EnterInterceptSuccessToFacePassHighBall180.ToString();
            }
            else
            {
                m_AnistateSubName = NetAniPassBallSubState.EAS_EnterInterceptSuccessToSlantPassHighBall180.ToString();
            }
        }
        else
        {
            if (_face)
            {
                m_AnistateSubName = NetAniPassBallSubState.EAS_EnterInterceptSuccessToFacePassHighBall.ToString();
            }
            else
            {
                m_AnistateSubName = NetAniPassBallSubState.EAS_EnterInterceptSuccessToSlantPassHighBall.ToString();
            }
        }
    }

}

