using UnityEngine;
using System.Collections;
using Common;
using Common.Log;
using System;

public enum NetAniTackleSubState
{
    EAS_EnterTackleToTackleSuccess,
    EAS_EnterLowBallToAvoidTackleBreakOutSuccess
}
/// <summary>
/// 抢截成功
/// </summary>
public class NetAniTackleState :AniBaseState
{

    public NetAniTackleState()
        :base(EAniState.Ground_Snatch)
    {

    }

    protected override void OnBegin()
    {
        switch(m_kPreState)
        {
            case EAniState.Mark_Ball:
                m_AnistateSubName = NetAniTackleSubState.EAS_EnterTackleToTackleSuccess.ToString();
                break;
        }
        base.OnBegin();
    }
}


/// <summary>
/// 抢截失败
/// </summary>
public class NetAniTackleFailedState : NetAniFailedBaseState
{
    public NetAniTackleFailedState()
        : base(EAniState.Ground_Snatch_Failed)
    {

    }

}

/// <summary>
/// 躲避抢截失败
/// </summary>
public class NetAniStopTackleFailedState : NetAniFailedBaseState
{
    public NetAniStopTackleFailedState()
        : base(EAniState.Stop_Ground_Snatch_Failed)
    {

    }



}

/// <summary>
/// 躲避抢截成功,
/// </summary>
public class NetAniStopTackleSuccessState : AniBaseState
{
    public NetAniStopTackleSuccessState()
        : base(EAniState.Stop_Ground_Snatch_Success)
    {

    }

    protected override void OnRotateAngle()
    {
       
    }
    protected override void OnBegin()
    {
        switch(m_kPreState)
        {
            case EAniState.LowDribble:
                m_AnistateSubName = NetAniTackleSubState.EAS_EnterLowBallToAvoidTackleBreakOutSuccess.ToString();
                break;
        }
    }
}