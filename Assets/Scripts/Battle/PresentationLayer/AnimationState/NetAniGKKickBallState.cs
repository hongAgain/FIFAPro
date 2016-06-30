using System;
using Common;

public enum NetAniGKKickBallSubState
{

    EAS_EnterGKHoldBallToThrowBallToIdle,
    EAS_EnterGKHoldBallToKickBallToIdle,
    EAS_EnterGKNBIdleToBigKickBallToIdle,
}

/// <summary>
/// 门将手抛球
/// </summary>
public class NetAniGKThrowBallState : AniBaseState
{
    public NetAniGKThrowBallState() : base(EAniState.GK_ThrowBall) { }

    protected override void OnBegin()
    {
        switch (m_kPreState)
        {
            case EAniState.GK_Save_Success:
				m_AnistateSubName = NetAniGKKickBallSubState.EAS_EnterGKHoldBallToThrowBallToIdle.ToString();
				break;
            case EAniState.GK_Save_Out_Success:
                m_AnistateSubName = NetAniGKKickBallSubState.EAS_EnterGKHoldBallToThrowBallToIdle.ToString();
                break;
        }

        base.OnBegin();
    }
}

/// <summary>
/// 门将大脚开球
/// </summary>
public class NetAniGKKickBallState : AniBaseState
{
    public NetAniGKKickBallState() : base(EAniState.GK_KickBall) { }

    protected override void OnBegin()
    {
        switch (m_kPreState)
        {
            case EAniState.GK_Save_Success:
            case EAniState.Idle:
            case EAniState.GK_Save_Out_Success:
                m_AnistateSubName = NetAniGKKickBallSubState.EAS_EnterGKHoldBallToKickBallToIdle.ToString();
                break;
        }

        base.OnBegin();
    }
}

/// <summary>
/// 摆球大脚开球
/// </summary>
public class NetAniGKBigKickBallState : AniBaseState
{
    public NetAniGKBigKickBallState() : base(EAniState.GK_BigKickBall) { }

    protected override void OnBegin()
    {
        switch (m_kPreState)
        {
            case EAniState.GK_Save_Success:
            case EAniState.GK_Save_Out_Success:
                m_AnistateSubName = NetAniGKKickBallSubState.EAS_EnterGKHoldBallToKickBallToIdle.ToString();
                break;
            case EAniState.Idle:
                m_AnistateSubName = NetAniGKKickBallSubState.EAS_EnterGKNBIdleToBigKickBallToIdle.ToString();
                break;
        }

        base.OnBegin();
    }
}
