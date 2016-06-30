using System.Collections;
using Common;
using System;
using Common.Log;
/// <summary>
/// 常速带球行为
/// </summary>
public class NetAniNormalCarryState : NetAniCarryBaseState
{

    private enum NetAniNormalCarrySubState
    {
        // 常速带球->//
        EAS_EnterToNormalCarry,
        EAS_EnterToNormalCarry90,
        EAS_EnterToNormalCarry180,
        // 低速带球->//
        EAS_EnterLowCarryToNormalCarry,
        EAS_EnterLowCarryToNormalCarry90,
        EAS_EnterLowCarryToNormalCarry180,

        //接地面球->//
        EAS_EnterGetFloorBallToNormalCarryBall,
        EAS_EnterGetFloorBallToNormalCarryBall90,
        EAS_EnterGetFloorBallToNormalCarryBall180,

     
        //胸部顶球接地面常速带球//
        EAS_EnterHeadRobSuccessStopToNormalCarryBall,
        EAS_EnterHeadRobSuccessStopToNormalCarryBall90,
        EAS_EnterHeadRobSuccessStopToNormalCarryBall180,

        // 铲断事件成功//
        EAS_EnterSliderTackleToNormalCarryBall,
        EAS_EnterSliderTackleToNormalCarryBall90,
        EAS_EnterSliderTackleToNormalCarryBall180,
        //突破成功//
        EAS_EnterBreakOutSuccessToNormalBall,
        EAS_EnterBreakOutSuccessToNormalBall90,

        //抢截事件成功//
        EAS_EnterTackleSuccessToNormalCarry,
        EAS_EnterTackleSuccessToNormalCarry90,
        EAS_EnterTackleSuccessToNormalCarry180,
        //拦截事件成功//
        EAS_EnterInterceptSuccessToNormalCarryBall,
        EAS_EnterInterceptSuccessToNormalCarryBall90,
        EAS_EnterInterceptSuccessToNormalCarryBall180,

        //阻断突破成功//
        EAS_EnterStopBreakOutSuccessToNormalCarryBall,
        EAS_EnterStopBreakOutSuccessToNormalCarryBall90,
        EAS_EnterStopBreakOutSuccessToNormalCarryBall180,

        //躲过抢截成功//
        EAS_EnterStopTackleToNormalCarryBall,
        EAS_EnterStopTackleToNormalCarryBall90,

        //躲过铲断成功//
        EAS_EnterStopSliderTackleToNormalCarryBall,
        EAS_EnterStopSliderTackleToNormalCarryBall90,

    }
    public NetAniNormalCarryState()
        : base(EAniState.NormalDribbl)
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
            case EAniState.Catch_GroundBall: //接地面球//
                CatchGroundBallStateChange(m_RoateType);
                break;
            case EAniState.Stop_BreakThrough_Success://突破失败类似行为时候，防守球员前置状态为有球盯防，进入到常速带球//
                SpecialStateChange(m_RoateType);
                break;
            default: //当处于抢断、拦截、铲断特殊事件//
                SpecialState2Change(m_RoateType);
                break;

        }
        base.OnBegin();
    }

    
    private void NormalDribbleStateChange(RoundData _Rdata)
    {
        if (_Rdata == RoundData.Round90)
        {
            m_AnistateSubName = NetAniNormalCarrySubState.EAS_EnterToNormalCarry90.ToString();

        }
        else if (_Rdata == RoundData.Round180)
        {
            m_AnistateSubName = NetAniNormalCarrySubState.EAS_EnterToNormalCarry180.ToString();

        }
        else
        {
            m_AnistateSubName = NetAniNormalCarrySubState.EAS_EnterToNormalCarry.ToString();

        }
    }
    private void LowDribbleStateChange(RoundData _Rdata)
    {
        if (_Rdata == RoundData.Round90)
        {
            m_AnistateSubName = NetAniNormalCarrySubState.EAS_EnterLowCarryToNormalCarry90.ToString();

        }
        else if (_Rdata == RoundData.Round180)
        {
            m_AnistateSubName = NetAniNormalCarrySubState.EAS_EnterLowCarryToNormalCarry180.ToString();

        }
        else
        {
            m_AnistateSubName = NetAniNormalCarrySubState.EAS_EnterLowCarryToNormalCarry.ToString();

        }
    }

    private void CatchGroundBallStateChange(RoundData _Rdata)
    {
        if (_Rdata == RoundData.Round90)
        {
            m_AnistateSubName = NetAniNormalCarrySubState.EAS_EnterGetFloorBallToNormalCarryBall90.ToString();

        }
        else if (_Rdata == RoundData.Round180)
        {
            m_AnistateSubName = NetAniNormalCarrySubState.EAS_EnterGetFloorBallToNormalCarryBall180.ToString();
        }
        else
        {
            m_AnistateSubName = NetAniNormalCarrySubState.EAS_EnterGetFloorBallToNormalCarryBall.ToString();

        }
    }
    /// <summary>
    /// 阻止突破成功
    /// </summary>
    /// <param name="_angle"></param>
    private void StopBreakOutStateChange(RoundData _Rdata)
    {
        if (_Rdata == RoundData.Round90)
        {
            m_AnistateSubName = NetAniNormalCarrySubState.EAS_EnterStopBreakOutSuccessToNormalCarryBall90.ToString();

        }
        else if (_Rdata == RoundData.Round180)
        {
            m_AnistateSubName = NetAniNormalCarrySubState.EAS_EnterStopBreakOutSuccessToNormalCarryBall180.ToString();
        }
        else
        {
            m_AnistateSubName = NetAniNormalCarrySubState.EAS_EnterStopBreakOutSuccessToNormalCarryBall.ToString();

        }
    }


    /// <summary>
    /// 某些类似于持球发动行为来影响防守球员行为
    /// </summary>
    /// <param name="_angle"></param>
    private void SpecialStateChange(RoundData _Rdata)
    {
        if (m_kPreState == EAniState.Stop_BreakThrough_Success)
        {
            //阻止突破成功//
            StopBreakOutStateChange(_Rdata);
        }

    }


    private void SpecialState2Change(RoundData _Rdata)
    {
        if (m_kPreState == EAniState.Ground_Block)
        {
            //拦截成功,防守球员进入带球//
            GroundBlockStateChange(_Rdata);
        }
        else if (m_kPreState == EAniState.Ground_Tackle)
        {
            //铲断成功//
            GroundTackleStateChange(_Rdata);
        }
        else if (m_kPreState == EAniState.Ground_Snatch)
        {
            //拦截成功//
            GroundSnatchStateChange(_Rdata);
        }
        else if (m_kPreState == EAniState.HeadRob_Carry
            ||m_kPreState == EAniState.HeadRob2_Stop)
        {
            //胸部停球,也就是所谓的接高空球后常速带球//
            HeadRobSuccessStopStateChange(_Rdata);
        }
        else if (m_kPreState == EAniState.Break_Through)//持球球员突破成功，直接从突破进入带球状态//
        {
            //突破成功//
            BreakThroughStateChange(_Rdata);
        }
        else if (m_kPreState == EAniState.Stop_Ground_Snatch_Success)
        {
            //躲避抢截成功//
            AvoidSnatchSuccessChange(_Rdata);
        }
        else if (m_kPreState == EAniState.Stop_Ground_Tackle_Success)
        {
            //躲避铲断成功//
            AvoidTackleSuccessChange(_Rdata);
        }
    }
    private void GroundTackleStateChange(RoundData _Rdata)
    {
        if (_Rdata == RoundData.Round90)
        {

            m_AnistateSubName = NetAniNormalCarrySubState.EAS_EnterSliderTackleToNormalCarryBall90.ToString();

        }
        else if (_Rdata == RoundData.Round180)
        {
            m_AnistateSubName = NetAniNormalCarrySubState.EAS_EnterSliderTackleToNormalCarryBall180.ToString();

        }
        else
        {
            m_AnistateSubName = NetAniNormalCarrySubState.EAS_EnterSliderTackleToNormalCarryBall.ToString();
        }
    }
    private void GroundBlockStateChange(RoundData _Rdata)
    {
        if (_Rdata == RoundData.Round90)
        {

            m_AnistateSubName = NetAniNormalCarrySubState.EAS_EnterInterceptSuccessToNormalCarryBall90.ToString();

        }
        else if (_Rdata == RoundData.Round180)
        {
            m_AnistateSubName = NetAniNormalCarrySubState.EAS_EnterInterceptSuccessToNormalCarryBall180.ToString();

        }
        else
            m_AnistateSubName = NetAniNormalCarrySubState.EAS_EnterInterceptSuccessToNormalCarryBall.ToString();
    }
    private void GroundSnatchStateChange(RoundData _Rdata)
    {
        if (_Rdata == RoundData.Round90)
        {

            m_AnistateSubName = NetAniNormalCarrySubState.EAS_EnterTackleSuccessToNormalCarry90.ToString();

        }
        else if (_Rdata == RoundData.Round180)
        {
            m_AnistateSubName = NetAniNormalCarrySubState.EAS_EnterTackleSuccessToNormalCarry180.ToString();

        }
        else
            m_AnistateSubName = NetAniNormalCarrySubState.EAS_EnterTackleSuccessToNormalCarry.ToString();
    }
    private void BreakThroughStateChange(RoundData _Rdata)
    {
        if (_Rdata == RoundData.Round90)
        {
            m_AnistateSubName = NetAniNormalCarrySubState.EAS_EnterBreakOutSuccessToNormalBall90.ToString();
        }
        else if (_Rdata == RoundData.Round0)
        {
            m_AnistateSubName = NetAniNormalCarrySubState.EAS_EnterBreakOutSuccessToNormalBall.ToString();
        }
        else
            m_AnistateSubName = NetAniNormalCarrySubState.EAS_EnterBreakOutSuccessToNormalBall90.ToString();
    }
    private void HeadRobSuccessStopStateChange(RoundData _Rdata)
    {
        if (_Rdata == RoundData.Round90)
        {
            m_AnistateSubName = NetAniNormalCarrySubState.EAS_EnterHeadRobSuccessStopToNormalCarryBall90.ToString();

        }
        else if (_Rdata == RoundData.Round180)
        {
            m_AnistateSubName = NetAniNormalCarrySubState.EAS_EnterHeadRobSuccessStopToNormalCarryBall180.ToString();
        }
        else
        {
            m_AnistateSubName = NetAniNormalCarrySubState.EAS_EnterHeadRobSuccessStopToNormalCarryBall.ToString();
        }
    }
    private void AvoidSnatchSuccessChange(RoundData _Rdata)
    {
        if (_Rdata == RoundData.Round90)
        {
            m_AnistateSubName = NetAniNormalCarrySubState.EAS_EnterStopTackleToNormalCarryBall90.ToString();

        }
        else if (_Rdata == RoundData.Round0)
        {
            m_AnistateSubName = NetAniNormalCarrySubState.EAS_EnterStopTackleToNormalCarryBall.ToString();

        }
    }

    private void AvoidTackleSuccessChange(RoundData _Rdata)
    {
        if (_Rdata == RoundData.Round90)
        {
            m_AnistateSubName = NetAniNormalCarrySubState.EAS_EnterStopSliderTackleToNormalCarryBall90.ToString();

        }
        else if (_Rdata == RoundData.Round0)
        {
            m_AnistateSubName = NetAniNormalCarrySubState.EAS_EnterStopSliderTackleToNormalCarryBall.ToString();

        }
    }
}
