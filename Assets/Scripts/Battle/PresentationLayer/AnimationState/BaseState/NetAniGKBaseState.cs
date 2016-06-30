using Common;
using Common.Log;
using Common.Tables;
using System;
using System.Collections.Generic;

public class NetAniGKBaseState :AniBaseState
{
    public NetAniGKBaseState(EAniState kState)
        : base(kState)
    {

    }
    protected override void OnCrossFade()
    {
        double _time = 0f;
        for (int i = 0; i < m_kAniClipList.Count; i++)
        {
            if (i + 1 == m_combineData.m_ballInIndex)
            {
                _time += m_kAniClipList[i].BallInTime;
                break;
            }
            else
            {
                _time += m_kAniClipList[i].AllFrameTime;
            }
        }
        //延迟扑救，可以让扑救不成功//
        m_stateDelayTime = (float)(m_kPlayer.KAniData.ballFlyingTime - _time + m_failedExitTime);
        base.OnCrossFade();
    }
    public override void OnEnter()
    {
        m_kPlayer.CanUpdateRotate = false;
        m_failedExitTime = 0d;
        base.OnEnter();
        
    }
    protected override void OnNoRoundMirror()
    {
        if (m_kAniClipList.Count <= 0 || m_combineData.m_AnimationIds.Count <= 0)
            return;
        if (m_kAniClipList.Count > 0)
        {
            if (m_combineData != null)
            {
                if (m_kMDir == MirrorDirection.Left)
                {
                    if (m_combineData.m_ballOutIndex > 0 && m_combineData.m_ballOutIndex != m_combineData.m_ballInIndex)
                    {
                        if (m_kAniClipList[m_combineData.m_ballOutIndex - 1].AtionSide == 0)
                            m_kAniClipList[m_combineData.m_ballOutIndex - 1].Mirror = true;
                    }
                    else if (m_combineData.m_ballInIndex > 0 && m_combineData.m_ballOutIndex != m_combineData.m_ballInIndex)
                    {
                        if (m_kAniClipList[m_combineData.m_ballInIndex - 1].AtionSide == 0)
                            m_kAniClipList[m_combineData.m_ballInIndex - 1].Mirror = true;
                    }
                    else if (m_combineData.m_ballInIndex == m_combineData.m_ballOutIndex && m_combineData.m_ballOutIndex > 0)
                    {
                        if (m_kAniClipList[m_combineData.m_ballInIndex - 1].AtionSide == 0)
                            m_kAniClipList[m_combineData.m_ballInIndex - 1].Mirror = true;
                    }
                }
                if (m_kMDir == MirrorDirection.Right)
                {
                    if (m_combineData.m_ballOutIndex > 0 && m_combineData.m_ballOutIndex != m_combineData.m_ballInIndex)
                    {
                        if (m_kAniClipList[m_combineData.m_ballOutIndex - 1].AtionSide > 0)
                            m_kAniClipList[m_combineData.m_ballOutIndex - 1].Mirror = true;
                    }
                    else if (m_combineData.m_ballInIndex > 0 && m_combineData.m_ballOutIndex != m_combineData.m_ballInIndex)
                    {
                        if (m_kAniClipList[m_combineData.m_ballInIndex - 1].AtionSide > 0)
                            m_kAniClipList[m_combineData.m_ballInIndex - 1].Mirror = true;
                    }
                    else if (m_combineData.m_ballInIndex == m_combineData.m_ballOutIndex && m_combineData.m_ballOutIndex > 0)
                    {
                        if (m_kAniClipList[m_combineData.m_ballInIndex - 1].AtionSide > 0)
                            m_kAniClipList[m_combineData.m_ballInIndex - 1].Mirror = true;
                    }
                }
            }
        }
    }
    public override void OnExit()
    {
        m_kPlayer.CanUpdateRotate = true;
        base.OnExit();
    }

    protected override void GetAniMoveDir(AniClipData _data)
    {
        if (_data != null)
        {
            if (_data.StartFrame > 0)
            {
                double _angle = _data.AniAngle;
                double _PAngle = m_kPlayer.GetRotAngle() + _angle;
                Vector3D _moveDir = MathUtil.GetDirFormAngle(_PAngle);
                m_AniMoveDir = new Vector3D(-_moveDir.X,_moveDir.Y,_moveDir.Z);
            }
        }
    }
    /// <summary>
    /// 当守门员扑救行为失败时候，用于滞后扑救的行为
    /// </summary>
    protected double m_failedExitTime = 0d;
}
