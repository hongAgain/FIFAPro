using Common;
using Common.Log;
using Common.Tables;
using System;
using System.Collections.Generic;

/// <summary>
/// 射门逻辑基类
/// </summary>
public class NetAniShootBaseState : AniBaseState
{
	public NetAniShootBaseState(EAniState kstate)
         :base(kstate)
    {

    }

    public override void OnEnter()
    {
        base.OnEnter();
     //    m_bAniFinish = false;
	}
    protected override void OnBeginSkill()
    {
        if (m_combineData != null)
        {
            if (m_iClipIdx + 1 == m_combineData.m_ballOutIndex && m_kAniClipList[m_iClipIdx].BallOutTime > 0)
            {
                m_kPlayer.SkillBegin = true;
            }
        }
        else
            LogManager.Instance.RedLog("NetAniShootBaseState.combineData is Null,Check it");
    }
}
