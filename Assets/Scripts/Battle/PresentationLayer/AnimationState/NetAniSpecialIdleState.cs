using System.Collections;
using Common;
using System;
using Common.Log;


/// <summary>
/// 特殊的Idle，例如抢断，拦截等等,还有射门成功
/// m_kPreState == EAniState.Head_Tackle_Failed
//             || m_kPreState == EAniState.Stop_BreakThrough_Failed
//             || m_kPreState == EAniState.Break_Through_Failed
//             || m_kPreState == EAniState.Ground_Block_Failed
//             || m_kPreState == EAniState.Ground_Tackle_Failed
//             || m_kPreState == EAniState.Stop_Ground_Tackle_Success
//             || m_kPreState == EAniState.Ground_Snatch_Failed
//             || m_kPreState == EAniState.Stop_Ground_Snatch_Success
//             || m_kPreState == EAniState.Shoot_Failed
//             || m_kPreState == EAniState.Shoot_Near
//             || m_kPreState == EAniState.Shoot_Far
//             || m_kPreState == EAniState.HeadShoot
/// </summary>
public class NetAniSpecialIdleState : AniBaseState
{

    private enum NetAniSpecialIdlSubState
    {
        EAS_EnterTackleFailedToSpecialIdle,
        EAS_EnterHLDefineToBeTackleFailedIdle,
        EAS_EnterHLDefineToBeBreakOutFailedIdle,
        EAS_EnterNearShootToFrustrationToIdle,
        EAS_EnterFarShootToFrustrationToIdle,
        EAS_EnterNearShootToCelebrationSuccess,
        EAS_EnterNULLBallIdleToHeadRobFailedIdle,
        EAS_EnterHeadShootToFrustrationToIdle,
        EAS_EnterHeadShootToHeadShootIdle,

        EAS_EnterBreakOutFailedToIdle,
        EAS_EnterNIdleToInterceptFailedIdle,

        EAS_EnterBeTackledToIdle,
        EAS_EnteBeSliderTackledToIdle,


    }
    public NetAniSpecialIdleState()
        : base(EAniState.Special_Idle)
    {

    }
    protected override void OnBegin()
    {
        switch (m_kPreState)
        {
            case EAniState.Stop_Ground_Tackle_Failed:
                m_AnistateSubName = NetAniSpecialIdlSubState.EAS_EnteBeSliderTackledToIdle.ToString();
                break;
            case EAniState.Stop_Ground_Snatch_Failed:
                m_AnistateSubName = NetAniSpecialIdlSubState.EAS_EnterBeTackledToIdle.ToString();
                break;
            case EAniState.Ground_Snatch_Failed:
                m_AnistateSubName = NetAniSpecialIdlSubState.EAS_EnterTackleFailedToSpecialIdle.ToString();
                break;
            case EAniState.Ground_Tackle_Failed:
                m_AnistateSubName = NetAniSpecialIdlSubState.EAS_EnterHLDefineToBeTackleFailedIdle.ToString();
                break;
            case EAniState.Stop_BreakThrough_Failed:
                m_AnistateSubName = NetAniSpecialIdlSubState.EAS_EnterHLDefineToBeBreakOutFailedIdle.ToString();
                break;
            case EAniState.Shoot_Far_Failed:
                m_AnistateSubName = NetAniSpecialIdlSubState.EAS_EnterFarShootToFrustrationToIdle.ToString();
                break;
            case EAniState.Shoot_Near_Failed:
                m_AnistateSubName = NetAniSpecialIdlSubState.EAS_EnterNearShootToFrustrationToIdle.ToString();
                break;
            case EAniState.Shoot_Far:
            case EAniState.Shoot_Near:
                m_AnistateSubName = NetAniSpecialIdlSubState.EAS_EnterNearShootToFrustrationToIdle.ToString();
                break;
            case EAniState.Ground_Block_Failed:
                m_AnistateSubName = NetAniSpecialIdlSubState.EAS_EnterNIdleToInterceptFailedIdle.ToString();
                break;
            case EAniState.Break_Through_Failed:
                m_AnistateSubName = NetAniSpecialIdlSubState.EAS_EnterBreakOutFailedToIdle.ToString();
                break;
            case EAniState.Head_Tackle_Failed:
                m_AnistateSubName = NetAniSpecialIdlSubState.EAS_EnterNULLBallIdleToHeadRobFailedIdle.ToString();
                break;
            case EAniState.HeadRob_Shoot_Failed:
                m_AnistateSubName = NetAniSpecialIdlSubState.EAS_EnterHeadShootToHeadShootIdle.ToString();
                break;

        }
        base.OnBegin();
    }
}
