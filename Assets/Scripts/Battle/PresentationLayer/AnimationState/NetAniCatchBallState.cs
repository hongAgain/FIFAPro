using System.Collections;
using Common;
using Common.Log;
using System;
using Common.Tables;


public enum NetAniCatchBallSubState
{
    EAS_EnterNBIdleToGetFloorBall,
    EAS_EnterNBIdleToHeadRobSuccess,
}

/// <summary>
///接地面球
/// </summary>
public class NetAniCatchFloorBallState : NetAniCatchBaseState
{
    public NetAniCatchFloorBallState()
        : base(EAniState.Catch_GroundBall)
    {

    }
    protected override void OnBegin()
    {
        m_kPreState = EAniState.Idle;
        if (m_kPreState == EAniState.Idle)
        {
            m_AnistateSubName = NetAniCatchBallSubState.EAS_EnterNBIdleToGetFloorBall.ToString();
        }
        base.OnBegin();
    }
}




