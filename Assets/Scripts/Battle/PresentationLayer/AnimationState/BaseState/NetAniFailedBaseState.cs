using System.Collections;
using Common;
using System;
using Common.Log;
public class NetAniFailedBaseState : AniBaseState
{
    public NetAniFailedBaseState(EAniState state)
        :base(state)
    {

    }
    public override void OnEnter()
    {
        base.OnEnter();
        m_bAniFinish = false; 
    }
    protected override void OnBegin()
    {
        //base.OnBegin();
    }
    protected override void OnFinish()
    {
        m_kPlayer.SetAniState(EAniState.Special_Idle);
    }
}
