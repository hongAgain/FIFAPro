using System.Collections;
using Common;
using System;
using Common.Log;

/// <summary>
/// 中场开球
/// </summary>
public class NetAniMatchBeginState : AniBaseState
{
    public enum NetAniMatchBeginSubState
    {
        EAS_ReadyToMidlePass,
    }
    public NetAniMatchBeginState()
        :base(EAniState.Match_BeginKick)
    {
        
    }

    protected override void OnBegin()
    {
        m_AnistateSubName = NetAniMatchBeginSubState.EAS_ReadyToMidlePass.ToString();
        base.OnBegin();
    }
}

