using System;
using Common;

public enum NetAniGKWalkSubState
{

    EAS_EnterGKNIdleToFrontWalk,
    EAS_EnterGKToFrontWalk,
    EAS_EnterGKFWalkToFrontWalk, 
    EAS_EnterGKLWalkToFrontWalk,
    EAS_EnterGKRWalkToFrontWalk,

    EAS_EnterGKNIdleToBackWalk,
    EAS_EnterGKFWalkToBackWalk,
    EAS_EnterGKToBackWalk,
    EAS_EnterGKLWalkToBackWalk,
    EAS_EnterGKRWalkToBackWalk,

    EAS_EnterGKNIdleToLeftWalk,
    EAS_EnterGKFWalkToLeftWalk,
    EAS_EnterGKBWalkToLeftWalk,
    EAS_EnterGKToLeftWalk,
    EAS_EnterGKRWalkToLeftWalk,

    EAS_EnterGKNIdleToRightWalk,
    EAS_EnterGKFWalkToRightWalk,
    EAS_EnterGKBWalkToRightWalk,
    EAS_EnterGKLWalkToRightWalk,
    EAS_EnterGKToRightWalk
}


public class NetAniGKWalkState : AniBaseState
{
    public NetAniGKWalkState(EAniState kAniState):base(kAniState)
    {

    }


    protected override void OnRotateAngle()
    {

    }
}
/// <summary>
/// 门将前移
/// </summary>
public class NetAniGKFrontWalkState : NetAniGKWalkState
{
    public NetAniGKFrontWalkState() : base(EAniState.GK_FrontWalk) { }

    protected override void OnBegin()
    {
        switch (m_kPreState)
        {
            case EAniState.Idle:
                m_AnistateSubName = NetAniGKWalkSubState.EAS_EnterGKNIdleToFrontWalk.ToString();
                break;
            case EAniState.GK_FrontWalk:
                m_AnistateSubName = NetAniGKWalkSubState.EAS_EnterGKToFrontWalk.ToString();
                break;
            case EAniState.GK_BackWalk:
                m_AnistateSubName = NetAniGKWalkSubState.EAS_EnterGKFWalkToFrontWalk.ToString();
                break;
            case EAniState.GK_LeftWalk:
                m_AnistateSubName = NetAniGKWalkSubState.EAS_EnterGKLWalkToFrontWalk.ToString();
                break;
            case EAniState.GK_RightWalk:
                m_AnistateSubName = NetAniGKWalkSubState.EAS_EnterGKRWalkToFrontWalk.ToString();
                break;
        }

        base.OnBegin();
    }
}

/// <summary>
/// 门将后退
/// </summary>
public class NetAniGKBackWalkState : NetAniGKWalkState
{
    public NetAniGKBackWalkState() : base(EAniState.GK_BackWalk) { }

    protected override void OnBegin()
    {
        switch (m_kPreState)
        {
            case EAniState.Idle:
                m_AnistateSubName = NetAniGKWalkSubState.EAS_EnterGKNIdleToBackWalk.ToString();
                break;
            case EAniState.GK_FrontWalk:
                m_AnistateSubName = NetAniGKWalkSubState.EAS_EnterGKFWalkToBackWalk.ToString();
                break;
            case EAniState.GK_BackWalk:
                m_AnistateSubName = NetAniGKWalkSubState.EAS_EnterGKToBackWalk.ToString();
                break;
            case EAniState.GK_LeftWalk:
                m_AnistateSubName = NetAniGKWalkSubState.EAS_EnterGKLWalkToBackWalk.ToString();
                break;
            case EAniState.GK_RightWalk:
                m_AnistateSubName = NetAniGKWalkSubState.EAS_EnterGKRWalkToBackWalk.ToString();
                break;
        }

        base.OnBegin();
    }
}

/// <summary>
/// 门将左移
/// </summary>
public class NetAniGKLeftWalkState : NetAniGKWalkState
{
    public NetAniGKLeftWalkState() : base(EAniState.GK_LeftWalk) { }

    protected override void OnBegin()
    {

        switch (m_kPreState)
        {
            case EAniState.Idle:
                m_AnistateSubName = NetAniGKWalkSubState.EAS_EnterGKNIdleToLeftWalk.ToString();
                break;
            case EAniState.GK_FrontWalk:
                m_AnistateSubName = NetAniGKWalkSubState.EAS_EnterGKFWalkToLeftWalk.ToString();
                break;
            case EAniState.GK_BackWalk:
                m_AnistateSubName = NetAniGKWalkSubState.EAS_EnterGKBWalkToLeftWalk.ToString();
                break;
            case EAniState.GK_LeftWalk:
                m_AnistateSubName = NetAniGKWalkSubState.EAS_EnterGKToLeftWalk.ToString();
                break;
            case EAniState.GK_RightWalk:
                m_AnistateSubName = NetAniGKWalkSubState.EAS_EnterGKRWalkToLeftWalk.ToString();
                break;
        }

        base.OnBegin();
    }
}

/// <summary>
/// 门将右移
/// </summary>
public class NetAniGKRightWalkState : NetAniGKWalkState
{
    public NetAniGKRightWalkState() : base(EAniState.GK_RightWalk) { }

    protected override void OnBegin()
    {
        switch (m_kPreState)
        {
            case EAniState.Idle:
                m_AnistateSubName = NetAniGKWalkSubState.EAS_EnterGKNIdleToRightWalk.ToString();
                break;
            case EAniState.GK_FrontWalk:
                m_AnistateSubName = NetAniGKWalkSubState.EAS_EnterGKFWalkToRightWalk.ToString();
                break;
            case EAniState.GK_BackWalk:
                m_AnistateSubName = NetAniGKWalkSubState.EAS_EnterGKBWalkToRightWalk.ToString();
                break;
            case EAniState.GK_LeftWalk:
                m_AnistateSubName = NetAniGKWalkSubState.EAS_EnterGKLWalkToRightWalk.ToString();
                break;
            case EAniState.GK_RightWalk:
                m_AnistateSubName = NetAniGKWalkSubState.EAS_EnterGKToRightWalk.ToString();
                break;
        }

        base.OnBegin();
    }
}