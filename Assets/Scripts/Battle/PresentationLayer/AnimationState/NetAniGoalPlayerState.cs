using System.Collections;
using Common;
using System;
using Common.Log;
public enum NetAniGoalSubState
{
    EAS_EnterIdleToCelebrate,
    EAS_EnterIdleToSad,

}
public class NetAniGoalPlayerCelebrateState : AniBaseState
{
    public NetAniGoalPlayerCelebrateState()
        : base(EAniState.GoalCeleBration)
    {

    }
    protected override void OnRotateAngle()
    {
        //base.OnRotateAngle();
    }
    protected override void OnBegin()
    {
        m_AnistateSubName = NetAniGoalSubState.EAS_EnterIdleToCelebrate.ToString();
        base.OnBegin();
    }
}
public class NetAniGoalPlayerSaddedState : AniBaseState
{
    public NetAniGoalPlayerSaddedState()
        : base(EAniState.GoalSad)
    {

    }
    protected override void OnRotateAngle()
    {
        //base.OnRotateAngle();
    }
    protected override void OnBegin()
    {
        m_AnistateSubName = NetAniGoalSubState.EAS_EnterIdleToSad.ToString();
        base.OnBegin();
    }
}