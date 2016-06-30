using System.Collections;
using Common;
using Common.Log;
using System;
using Common.Tables;


public enum NetAniHeadRobSubState
{
    //头球摆渡 //
    EAS_EnterNBIdleToHeadSlantHighPassBall,
    EAS_EnterNBIdleToHeadFaceHighPassBall,
    EAS_EnterNBIdleToHeadFrontSlant45HighPassBall,
    EAS_EnterNBIdleToHeadBackSlant45HighPassBall,
    EAS_EnterNBIdleToHeadBackHighPassBall,
    //头球射门 //
    EAS_EnterNBIdleToHeadSlantShoot,
    EAS_EnterNBIdleToHeadFaceShoot,
    EAS_EnterNBIdleToHeadFrontSlant45Shoot,
    EAS_EnterNBIdleToHeadBackSlant45Shoot,
    EAS_EnterNBIdleToHeadBackShoot,

    //一次高空争顶停球//
    //二次高空争顶停球//
    EAS_EnterNBIdleToHeadRobSuccessStop,

}

/// <summary>
/// 争顶成功传球
/// </summary>
public class NetAniHeadRobPassState : NetAniHeadRobBaseState
{
    public NetAniHeadRobPassState()
        : base(EAniState.HeadRob_Pass)
    {

    }

    public override void OnEnter()
    {
        base.OnEnter();
        m_bHeadPass = true;
    }
    protected override void OnBegin()
    {
        if (m_kPreState == EAniState.Idle)
        {
            HeadPassStateChange(m_rorateInvertAngle);
        }
        base.OnBegin();
    }


    private void HeadPassStateChange(float _angle)
    {
        if (_angle <= 10f)
        {
            m_AnistateSubName = NetAniHeadRobSubState.EAS_EnterNBIdleToHeadFaceHighPassBall.ToString();
        }
        else if ((_angle > 10f && _angle <= 45f))
        {
            m_AnistateSubName = NetAniHeadRobSubState.EAS_EnterNBIdleToHeadSlantHighPassBall.ToString();
        }
        else if (_angle > 45f && _angle <= 90f)
        {
            m_AnistateSubName = NetAniHeadRobSubState.EAS_EnterNBIdleToHeadFrontSlant45HighPassBall.ToString();
        }
        else if (_angle > 90f && _angle <= 135f)
        {
            m_AnistateSubName = NetAniHeadRobSubState.EAS_EnterNBIdleToHeadBackSlant45HighPassBall.ToString();
        }
        else if ((_angle > 135f && _angle <= 180f))
        {
            m_AnistateSubName = NetAniHeadRobSubState.EAS_EnterNBIdleToHeadBackHighPassBall.ToString();
        }
    }
}

/// <summary>
/// 争顶成功头球攻门
/// </summary>
public class NetAniHeadRobShootState : NetAniHeadRobBaseState
{
    public NetAniHeadRobShootState()
        : base(EAniState.HeadRob_Shoot)
    {

    }

    protected override void OnBegin()
    {
        if (m_kPreState == EAniState.Idle)
        {
            HeadShootStateChange(m_rorateInvertAngle);
        }
        base.OnBegin();
    }


    private void HeadShootStateChange(float _angle)
    {
        if (_angle <= 10)
        {
            m_AnistateSubName = NetAniHeadRobSubState.EAS_EnterNBIdleToHeadFaceShoot.ToString();
        }
        else if ((_angle > 10 && _angle <= 45))
        {
            m_AnistateSubName = NetAniHeadRobSubState.EAS_EnterNBIdleToHeadSlantShoot.ToString();
        }
        else if (_angle > 45 && _angle <= 90)
        {
            m_AnistateSubName = NetAniHeadRobSubState.EAS_EnterNBIdleToHeadFrontSlant45Shoot.ToString();
        }
        else if (_angle > 90 && _angle <= 135)
        {
            m_AnistateSubName = NetAniHeadRobSubState.EAS_EnterNBIdleToHeadBackSlant45Shoot.ToString();
        }
        else if ((_angle > 135 && _angle <= 180))
        {
            m_AnistateSubName = NetAniHeadRobSubState.EAS_EnterNBIdleToHeadBackShoot.ToString();
        }
    }
}

/// <summary>
/// 接高空球带球
/// </summary>
public class NetAniHeadRobChestCarryState : NetAniHeadRobBaseState
{
    public NetAniHeadRobChestCarryState()
        : base(EAniState.HeadRob_Carry)
    {

    }
    protected override void OnBegin()
    {
        switch (m_kPreState)
        {
            case EAniState.Idle:
                m_AnistateSubName = NetAniHeadRobSubState.EAS_EnterNBIdleToHeadRobSuccessStop.ToString();
                break;

        }
        base.OnBegin();
    }

}

/// <summary>
/// 第二次争顶必须是胸部停球到脚下
/// </summary>
public class NetAniHeadRob2StopState : NetAniHeadRobBaseState
{
    public NetAniHeadRob2StopState()
        : base(EAniState.HeadRob2_Stop)
    {

    }
    protected override void OnBegin()
    {
        switch (m_kPreState)
        {
            case EAniState.Idle:
                m_AnistateSubName = NetAniHeadRobSubState.EAS_EnterNBIdleToHeadRobSuccessStop.ToString();
                break;

        }
        base.OnBegin();
    }
}

/// <summary>
/// 争顶失败
/// </summary>
public class NetAniHeadRobFailedState : NetAniHeadRobFailedBaseState
{
    public NetAniHeadRobFailedState()
        : base(EAniState.Head_Tackle_Failed)
    {

    }
    public override void OnEnter()
    {
        base.OnEnter();
        double dAngle = MathUtil.GetAngle(m_kPlayer.GetPosition(), m_kBall.GetPosition()); //targetBallOutPos m_kPlayer.KAniData.targetBallOutPos//
        m_kPlayer.SetRoteAngle(dAngle);
    }
}