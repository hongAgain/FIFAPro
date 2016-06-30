using System.Collections;
using Common;
using System;
using Common.Log;

/// <summary>
/// 默认铲断成功
/// </summary>
public class NetAniSliderTackleState : AniBaseState
{
    private enum NetAniSliderTackleSubState
    {
        EAS_EnterSliderTackleToSliderTackleSuccess,

    }
    public NetAniSliderTackleState()
        : base(EAniState.Ground_Tackle)
    {
    }
    protected override void OnBegin()
    {
        m_kPreState = EAniState.Mark_Ball;
        switch (m_kPreState)
        {
            case EAniState.Mark_Ball:
                m_AnistateSubName = NetAniSliderTackleSubState.EAS_EnterSliderTackleToSliderTackleSuccess.ToString();
                break;
            default:
                LogManager.Instance.RedLog("NetAniBreakOutState===>This is prestate is error PreState===" + m_kPreState);
                break;
        }
        base.OnBegin();
    }

}

/// <summary>
/// 躲避铲断成功
/// </summary>
public class NetAniAvoidSliderTackleSuccessState : AniBaseState
{
    private enum NetAniAvoidSliderTackleSubState
    {
        EAS_EnteLowBallToAvoidSliderTackle,

    }
    public NetAniAvoidSliderTackleSuccessState()
        : base(EAniState.Stop_Ground_Tackle_Success)
    {

    }
    protected override void OnBegin()
    {
        switch (m_kPreState)
        {
            case EAniState.LowDribble:
                m_AnistateSubName = NetAniAvoidSliderTackleSubState.EAS_EnteLowBallToAvoidSliderTackle.ToString();
                break;
            default:
                LogManager.Instance.RedLog("NetAniBreakOutState===>This is prestate is error PreState===" + m_kPreState);
                break;
        }

        base.OnBegin();
    }
}

/// <summary>
/// 铲断失败
/// </summary>
public class NetAniSliderTackleFailedState : NetAniFailedBaseState
{
    public NetAniSliderTackleFailedState()
        : base(EAniState.Ground_Tackle_Failed)
    {

    }

}

/// <summary>
/// 躲避铲断失败
/// </summary>
public class NetAniAvoidSliderTackleFailedState : NetAniFailedBaseState
{
    public NetAniAvoidSliderTackleFailedState()
        : base(EAniState.Stop_Ground_Tackle_Failed)
    {

    }
}
