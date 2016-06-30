
using System.Collections;
using Common;
using Common.Log;
using System;
public class NetAniHaveBallDefineState : AniBaseState
{

    private enum NetAniHaveBallDefineSubState
    {
        EAS_EnterNIdleToHaveBallDefine, // 从idle来
        EAS_EnterNIdleToHaveBallDefine90,
        EAS_EnterNIdleToHaveBallDefine180,
        EAS_EnterNQWalkToHaveBallDefine,
        EAS_EnterNQWalkToHaveBallDefine90,
        EAS_EnterNQWalkToHaveBallDefine180,
        EAS_EnterNQRunToHaveBallDefine,
        EAS_EnterNQRunToHaveBallDefine90,
        EAS_EnterNQRunToHaveBallDefine180,
        EAS_EnterNLDefineToHaveBallDefine,
        EAS_EnterNLDefineToHaveBallDefine90,
        EAS_EnterNLDefineToHaveBallDefine180,
        EAS_EnterToHaveBallDefine,
        // 事件后 指拦截、抢断、铲球、突破、头球、射门等idle下//
        EAS_EnterSpecialIdleToHaveBallDefine,
        EAS_EnterSpecialIdleToHaveBallDefine90,
        EAS_EnterSpecialIdleToHaveBallDefine180,



    }
    public NetAniHaveBallDefineState()
        : base(EAniState.Mark_Ball)
    {

    }


    protected override void OnBegin()
    {
        m_kPreState = EAniState.Idle;
        switch (m_kPreState)
        {

            case EAniState.Idle:
                IdleStateChange(m_RoateType);
                break;
            case EAniState.Walk:
                WalkStateChange(m_RoateType);
                break;
            case EAniState.Mark:
                MarkStateChange(m_RoateType);
                break;
            case EAniState.Special_Idle:
                OtherStateChange(m_RoateType);
                break;
            default:
                //OtherStateChange(_invertAngle);
                break;
        }
        base.OnBegin();
    }


    private void IdleStateChange(RoundData _Rdata)
    {
        if (_Rdata == RoundData.Round90)
        {
            m_AnistateSubName = NetAniHaveBallDefineSubState.EAS_EnterNIdleToHaveBallDefine90.ToString();
        }
        else if (_Rdata == RoundData.Round180)
        {
            m_AnistateSubName = NetAniHaveBallDefineSubState.EAS_EnterNIdleToHaveBallDefine180.ToString();

        }
        else
            m_AnistateSubName = NetAniHaveBallDefineSubState.EAS_EnterNIdleToHaveBallDefine.ToString();
    }

    private void WalkStateChange(RoundData _Rdata)
    {
        if (_Rdata == RoundData.Round90)
        {
            m_AnistateSubName = NetAniHaveBallDefineSubState.EAS_EnterNQWalkToHaveBallDefine90.ToString();
        }
        else if (_Rdata == RoundData.Round180)
        {
            m_AnistateSubName = NetAniHaveBallDefineSubState.EAS_EnterNQWalkToHaveBallDefine180.ToString();

        }
        else
            m_AnistateSubName = NetAniHaveBallDefineSubState.EAS_EnterNQWalkToHaveBallDefine.ToString();
    }
    private void MarkStateChange(RoundData _Rdata)
    {
        if (_Rdata == RoundData.Round90)
        {
            m_AnistateSubName = NetAniHaveBallDefineSubState.EAS_EnterNLDefineToHaveBallDefine90.ToString();
        }
        else if (_Rdata == RoundData.Round180)
        {
            m_AnistateSubName = NetAniHaveBallDefineSubState.EAS_EnterNLDefineToHaveBallDefine180.ToString();

        }
        else
            m_AnistateSubName = NetAniHaveBallDefineSubState.EAS_EnterNLDefineToHaveBallDefine.ToString();

    }

    private void OtherStateChange(RoundData _Rdata)
    {
        //争顶失败、
        //持球球员突破成功，防守球员进入Idle,
        //持球球员突破失败，
        //拦截失败
        //防守球员抢截成功、防守球员铲断成功原持球球员进入idle
        //防守球员抢截失败、铲断失败、进入IDLE,
        //防守球员的拦截失败
        //射门后的Idle
        //头球攻门
        if (_Rdata == RoundData.Round90)
        {
            m_AnistateSubName = NetAniHaveBallDefineSubState.EAS_EnterSpecialIdleToHaveBallDefine90.ToString();
        }
        else if (_Rdata == RoundData.Round180)
        {
            m_AnistateSubName = NetAniHaveBallDefineSubState.EAS_EnterSpecialIdleToHaveBallDefine180.ToString();

        }
        else
            m_AnistateSubName = NetAniHaveBallDefineSubState.EAS_EnterSpecialIdleToHaveBallDefine.ToString();
    }
}
