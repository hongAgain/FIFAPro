using System.Collections;
using Common;
using Common.Log;
using System;


/// <summary>
/// 从idle进入拦截状态,此状态表示默认拦截成功
/// </summary>
public class NetAniInterceptState : AniBaseState
{
    private enum NetAniInterceptSubState
    {
        EAS_EnterTackleToTackleSuccess,       
    }

    public NetAniInterceptState()
        : base(EAniState.Ground_Block)
    {

    }
    protected override void OnBegin()
    {
        switch(m_kPreState)
        {
            case EAniState.Idle:
                m_AnistateSubName = NetAniInterceptSubState.EAS_EnterTackleToTackleSuccess.ToString();
                break;
        }
        base.OnBegin();
    }

}


/// <summary>
/// 拦截失败
/// </summary>
public class NetAniInterceptFailedState : NetAniFailedBaseState
{
    public NetAniInterceptFailedState()
        : base(EAniState.Ground_Block_Failed)
    {

    }

}
