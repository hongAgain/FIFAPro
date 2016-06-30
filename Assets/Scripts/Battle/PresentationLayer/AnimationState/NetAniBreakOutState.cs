using System.Collections;
using Common;
using System;
using Common.Log;

/// <summary>
/// 默认为突破成功！
/// </summary>
public class NetAniBreakOutState : AniBaseState
{
    public enum NetAniBreakOutSubState
    {
        EAS_EnterNormalCarryToBreakOut,
        EAS_EnterNormalCarryToBreakOut90,
        EAS_EnterNormalCarryToBreakOut180,
        EAS_EnterLowCarryToBreakOut,
        EAS_EnterLowCarryToBreakOut90,
        EAS_EnterLowCarryToBreakOut180,

    }
    public NetAniBreakOutState()
        : base(EAniState.Break_Through)
    {
    }

    protected override void OnBegin()
    {
        m_kPreState = EAniState.NormalDribbl;
        
        switch (m_kPreState)
        {
            case EAniState.LowDribble:
                LowDribbleChange(m_RoateType);
                break;
            case EAniState.NormalDribbl:
                NormalDribbleChange(m_RoateType);
                break;
            default:
                LogManager.Instance.RedLog("NetAniBreakOutState===>This is prestate is error PreState===" + m_kPreState);
                break;
        }
        base.OnBegin();
    }


    private void LowDribbleChange(RoundData _Rdata)
    {
        if (_Rdata ==  RoundData.Round90)
        {
            m_AnistateSubName = NetAniBreakOutSubState.EAS_EnterLowCarryToBreakOut90.ToString();
        }
        else if (_Rdata == RoundData.Round180)
        {
            m_AnistateSubName = NetAniBreakOutSubState.EAS_EnterLowCarryToBreakOut180.ToString();
        }
        else
        {
            m_AnistateSubName = NetAniBreakOutSubState.EAS_EnterLowCarryToBreakOut.ToString();
        }
    }
    private void NormalDribbleChange(RoundData _Rdata)
    {
        if (_Rdata == RoundData.Round90)
        {
            m_AnistateSubName = NetAniBreakOutSubState.EAS_EnterNormalCarryToBreakOut90.ToString();
        }
        else if (_Rdata == RoundData.Round180)
        {
            m_AnistateSubName = NetAniBreakOutSubState.EAS_EnterNormalCarryToBreakOut180.ToString();
        }
        else
        {
            m_AnistateSubName = NetAniBreakOutSubState.EAS_EnterNormalCarryToBreakOut.ToString();
        }
    }
}

/// <summary>
/// 阻断突破成功
/// </summary>
public class NetAniStopBreakOutSuccessState : AniBaseState
{
    public enum NetAniStopBreakOutSuccessSubState
    {
        EAS_EnterBreakThroughToStopBreakOutSuccess,

    }
    public NetAniStopBreakOutSuccessState()
        : base(EAniState.Stop_BreakThrough_Success)
    {
    }
    protected override void OnBegin()
    {
        switch (m_kPreState)
        {
            case EAniState.Mark_Ball:
                m_AnistateSubName = NetAniStopBreakOutSuccessSubState.EAS_EnterBreakThroughToStopBreakOutSuccess.ToString();
                break;
            default:
                LogManager.Instance.RedLog("NetAniStopBreakOutSuccessState===>This is prestate is error PreState===" + m_kPreState);
                break;
        }
        base.OnBegin();
    }
}

/// <summary>
/// 突破失败
/// </summary>
public class NetAniBreakOutFailedState : NetAniFailedBaseState
{
    public NetAniBreakOutFailedState()
        :base(EAniState.Break_Through_Failed)
    {

    }
}

/// <summary>
/// 阻断突破失败
/// </summary>
public class NetAniStopBreakOutFailedState : NetAniFailedBaseState
{
    public NetAniStopBreakOutFailedState()
        : base(EAniState.Stop_BreakThrough_Failed)
    {

    }
}