using System.Collections;
using Common;
using System;
using Common.Log;

/// <summary>
/// 赛前准备
/// </summary>
public class NetAniMatchReadyState : AniBaseState
{
    private enum NetAniMatchReadySubState
    {
        EAS_ReadyMatchIdle,
    }
    public NetAniMatchReadyState()
        :base(EAniState.Match_ReadyIdle)
    {
        
    }
    protected override void OnRotateAngle()
    {
        //base.OnRotateAngle();
    }
    protected override void OnBegin()
    {
        m_AnistateSubName = NetAniMatchReadySubState.EAS_ReadyMatchIdle.ToString();
        base.OnBegin();
    }

    public override void OnUpdate(float fTime)
    {
        base.OnUpdate(fTime);
    }
}

/// <summary>
/// 赛前开球
/// </summary>
public class NetAniMatchReadyKickState : AniBaseState
{
    private enum NetAniMatchReadyKickSubState
    {
        EAS_ReadyMathPassIdle,
    }
    public NetAniMatchReadyKickState()
        : base(EAniState.Match_ReadyKick)
    {

    }
    protected override void OnRotateAngle()
    {
        //base.OnRotateAngle();
    }
    protected override void OnBegin()
    {
        m_AnistateSubName = NetAniMatchReadyKickSubState.EAS_ReadyMathPassIdle.ToString();
        base.OnBegin();
    }
}

/// <summary>
/// 中场开球
/// </summary>
public class NetAniMatchCenterKickState : AniBaseState
{
    private enum NetAniMatchCenterKickSubState
    {
        EAS_ReadyToMidlePass,
    }

    public NetAniMatchCenterKickState()
        : base(EAniState.Match_BeginKick)
    {

    }
    protected override void OnRotateAngle()
    {
        //base.OnRotateAngle();
    }
    protected override void OnBegin()
    {
        m_AnistateSubName = NetAniMatchCenterKickSubState.EAS_ReadyToMidlePass.ToString();
        base.OnBegin();
    }
}

/// <summary>
/// 中场开球
/// </summary>
public class NetAniMatchKickIdleState : AniBaseState
{
    private enum NetAniMatchKickSubIdleState
    {
        EAS_EnterNULLBallIdle,
    }

    public NetAniMatchKickIdleState()
        : base(EAniState.Kick_Idle)
    {

    }
    protected override void OnRotateAngle()
    {
        //base.OnRotateAngle();
    }
    protected override void OnBegin()
    {
        m_AnistateSubName = NetAniMatchKickSubIdleState.EAS_EnterNULLBallIdle.ToString();
        base.OnBegin();
    }
}