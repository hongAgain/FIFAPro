using System.Collections;
using Common;
using Common.Log;
using System;
using Common.Tables;
using System.Collections.Generic;
/// <summary>
/// 头球相关基类
/// </summary>
public class NetAniHeadRobBaseState :AniBaseState
{
    public NetAniHeadRobBaseState(EAniState kstate)
        : base(kstate)
    {

    }


    public override void OnEnter()
    {
        if (m_kBall == null)
        {
            m_kBall = m_kPlayer.Team.Scene.Ball;
        }
        m_iClipIdx = 0;
        m_iLoopTime = 1;
        m_playTime = 0f;
        m_rorateAngle = 0f;
        m_kAniClipList.Clear();
        m_kOtherClipDatas.Clear();
        m_AnistateSubName = "";
        m_combineData = null;
        m_stateDelayTime = 0f;
        m_kPlayer.AniFinish = false;
        m_RoateType = RoundData.Round0;
        m_rorateInvertAngle = 0f;
        m_bFaceTarget = false;
        m_bAniFinish = true;
        m_kPlayer.CanUpdateRotate = false;
        m_fAfterAngle = 0f;
        m_invertTime = 0d;
        m_RunInverTime = 0d;
        m_iOtherIndex = 0;
        m_bHeadPass = false;
        OnAddMove();
        OnRotateAngle();
        OnBegin();
        OnDelay();
        OnRoundMirror();
        OnNoRoundMirror();
        OnCrossFade();
    }

    protected override void OnAddMove()
    {
       // 是否需要跑动到接球点 //
        double _distance = m_kPlayer.GetPosition().Distance(m_kPlayer.KAniData.targetPos);
        if (_distance > 0.5f)
        {   
            //是否需要转向面向接球点节点//
            double _RorateAngle = MathUtil.GetAngle(m_kPlayer.GetPosition(), m_kPlayer.KAniData.targetPos);
            m_kPlayer.SetRoteAngle(_RorateAngle);
            m_RunStaticName = "Pao-ManPao";
            AniData _r = MatchAniHelper.Instance.GetAniDataByName(m_RunStaticName);
            AniClipData _data = MatchAniHelper.Instance.ResetClipData(_r);
            _data.Loop = true;
            m_kOtherClipDatas.Add(_data);
            m_RunInverTime = _distance / m_kPlayer.KAniData.playerSpeed;
        }
        else
        {
            double _RorateAngle = MathUtil.GetAngle(m_kPlayer.GetPosition(), m_kBall.GetPosition());
            m_kPlayer.SetRoteAngle(_RorateAngle);
        }
    }
    /// <summary>
    /// 重写角度，来决定最后动画,决定目标位置发生变化
    /// </summary>
    protected override void OnRotateAngle()
    {
        //争顶停球，这个headRobAvil为false，其他都有状态//
        if (!m_kPlayer.KAniData.headRobAvil) 
            return;
        double _resetAngle = 0f;
        int _param = 1;
        float _invertAngle = 0f;
        double dAngle = MathUtil.GetAngle(m_kPlayer.GetPosition(), m_kPlayer.KAniData.targetBallOutPos); //targetBallOutPos m_kPlayer.KAniData.targetBallOutPos//
        double _Cosangle = dAngle - (m_kPlayer.GetRotAngle());
        double dDeltaAngle = Math.Abs(_Cosangle);
        if (_Cosangle > 0d)
        {
            if (_Cosangle >= 180d)
            {
                _param = -1;
            }
            else
            {
                _param = 1;
            }

        }
        else
        {
            if (_Cosangle <= -180d)
            {
                _param = 1;
            }
            else
            {
                _param = -1;
            }
        }
        if (dDeltaAngle >= 180d)
        {
            _resetAngle = 360 - dDeltaAngle;
        }
        else
        {
            _resetAngle = dDeltaAngle;
        }
        if (_resetAngle >= 0d && _resetAngle <= 45d)
        {
            _invertAngle = 0f;
            m_RoateType = RoundData.Round0;
        }
        else if (_resetAngle > 45d && _resetAngle <= 145d)
        {
            _invertAngle = 90f;
            m_RoateType = RoundData.Round90;
        }
        else if (_resetAngle > 145d && _resetAngle <= 180d)
        {
            _invertAngle = 180f;
            m_RoateType = RoundData.Round180;
        }
        m_rorateInvertAngle = (float)_resetAngle;
        m_rorateAngle = (float)(_param * _invertAngle);
    }
    /// <summary>
    /// 是否产生延时
    /// </summary>
    private void OnDelay()
    {
        //算出动画到接球帧数的动画时间//
        if (m_combineData != null && m_combineData.m_ballInIndex > 0)
        {
            m_invertTime += (m_RunInverTime + m_kAniClipList[m_combineData.m_ballInIndex - 1].BallInTime);
        }
        //计算出多少时间后启动这个动画//
        m_stateDelayTime = (float)(m_kPlayer.KAniData.ballFlyingTime - m_invertTime);
    }
    /// <summary>
    /// 重写，先转身，然后是否延迟，接着跑动，提前起跳接球
    /// </summary>
    /// <param name="fTime"></param>
    public override void OnUpdate(float fTime)
    {
        //状态是否延迟进行动画启动//
        if (m_stateDelayTime - fTime > 0)
        {
            m_stateDelayTime -= fTime;
            return;
        }
        if (m_kInternalState == EInternalState.Special_Ani)
        {
            if (m_kOtherClipDatas != null && m_kOtherClipDatas.Count > 0 && m_iOtherIndex <= m_kOtherClipDatas.Count - 1)
            {
                if (m_playTime == 0f)
                {
                    PlayAniMessage kMsg = new PlayAniMessage(m_kPlayer, m_kOtherClipDatas[m_iOtherIndex]);
                    MessageDispatcher.Instance.SendMessage(kMsg);
                }
                m_playTime += (fTime * m_kOtherClipDatas[m_iOtherIndex].AniSpeed);
                if (m_rorateAngle != 0)
                {
                    //先转身//
                    if ((m_kOtherClipDatas[m_iOtherIndex].AllFrameTime - m_playTime) <= fTime)
                    {
                        //当需要转身时候，进行强制切换角度//
                        //m_kPlayer.RotateAngle += m_rorateAngle;
                        m_kPlayer.SetRoteAngle(m_kPlayer.GetRotAngle() + m_rorateAngle);
                        m_playTime = 0f;
                        m_iOtherIndex++;
                        m_rorateAngle = 0;
                        return;
                    }
                }
                else
                {
                    Vector3D nextPosition = m_kPlayer.GetPosition() + MathUtil.GetDir(m_kPlayer.GetPosition(), m_kPlayer.KAniData.targetPos) * m_kPlayer.Velocity * fTime;
                    double _distance = nextPosition.Distance(m_kPlayer.KAniData.targetPos);
                    //再启动跑步
                    if (_distance <= 0.5f)
                    {
                        m_playTime = 0f;
                        m_iOtherIndex++;
                        m_kPlayer.CanMoveNext = false;
                        //视角面向球//
                        double _LastAngle = MathUtil.GetAngle(m_kPlayer.GetPosition(), m_kBall.GetPosition());
                        m_kPlayer.SetRoteAngle(_LastAngle);
                        return;
                    }
                    else
                    {
                        m_kPlayer.CanMoveNext = true;
                        m_kPlayer.MoveToPos(m_kPlayer.KAniData.targetPos, fTime);
                    }
                }

            }
            else
            {
                m_iClipIdx = 0;
                m_playTime = 0f;

                m_kInternalState = EInternalState.CrossFadeAni;
            }
        }

        else
        {
            if (m_kAniClipList != null && m_kAniClipList.Count > 0 && m_iClipIdx <= m_kAniClipList.Count - 1)
            {
                if (m_playTime <= 0f)
                {
                    //如果角度在-45-45度,动画启动前硬转身//
                    if (Math.Abs(m_rorateAngle) <= 45f && m_iClipIdx == 0)
                    {
                        //m_kPlayer.RotateAngle += m_rorateAngle;
                        m_kPlayer.SetRoteAngle(m_kPlayer.GetRotAngle() + m_rorateAngle);
                    }
                    //得到动画产生的位移速度//
                    GetAniMoveDir(m_kAniClipList[m_iClipIdx]);
                    // 发消息需要播放动画 dracula_jin
                    PlayAniMessage kMsg = new PlayAniMessage(m_kPlayer, m_kAniClipList[m_iClipIdx]);
                    MessageDispatcher.Instance.SendMessage(kMsg);
                }
                if ((!m_kAniClipList[m_iClipIdx].Loop
                    && m_playTime >= m_kAniClipList[m_iClipIdx].AllFrameTime / (GlobalBattleInfo.Instance.PlaySpeed * m_kAniClipList[m_iClipIdx].AniSpeed)))
                //if (m_kAniClipList[m_iClipIdx].Loop && m_playTime > 1)
                {
                    if (true == CheckCrossfadeAniEnable())
                    {
                        //默认转身动画只出现在第一个动画里，动画在<-45||>45//
                        if (m_iClipIdx == 0 && Math.Abs(m_rorateAngle) > 45f)
                        {
                            //当需要转身时候，进行强制切换角度//
                            //m_kPlayer.RotateAngle += m_rorateAngle;
                            m_kPlayer.SetRoteAngle(m_kPlayer.GetRotAngle() + m_rorateAngle);
                            float fRotAngle = m_kAniClipList[m_iClipIdx].AniRoundAngle;
                            AniAngleChangeMeassage kMsg = new AniAngleChangeMeassage(m_kPlayer, -fRotAngle);
                            MessageDispatcher.Instance.SendMessage(kMsg);
                        }
                        m_iClipIdx++;
                        if (m_iClipIdx <= m_kAniClipList.Count)
                        {
                            m_playTime = 0f;
                            m_kInternalState = EInternalState.CrossFadeAni;
                        }
                    }
                    else
                    {
                        OnAniUpdate(m_playTime);
                        SimulationAnimationMove(m_playTime, fTime);
                        if (m_iClipIdx < m_kAniClipList.Count)
                            m_playTime += (fTime * m_kAniClipList[m_iClipIdx].AniSpeed);
                    }
                }
                else
                {
                    OnAniUpdate(m_playTime);
                    SimulationAnimationMove(m_playTime, fTime);
                    if (m_iClipIdx < m_kAniClipList.Count)
                        m_playTime += (fTime * m_kAniClipList[m_iClipIdx].AniSpeed);
                }
            }
            else
            {
                if (m_bAniFinish)
                    m_kPlayer.AniFinish = true;
                OnFinish();
            }

        }
        

    }
    protected override void OnCrossFade()
    {
        if (m_kOtherClipDatas != null && m_kOtherClipDatas.Count > 0)
        {
            m_kInternalState = EInternalState.Special_Ani;
        }
        else
            m_kInternalState = EInternalState.CrossFadeAni;
    }


    protected override void OnNoRoundMirror()
    {
        if (m_bHeadPass)
        {
            if (m_kAniClipList.Count <= 0 || m_combineData.m_AnimationIds.Count <= 0)
                return;
            OnMirrorAnim();
        }
        else
            base.OnNoRoundMirror();
 
    }
    private void OnMirrorAnim()
    {
        double dAngle = MathUtil.GetAngle(m_kPlayer.GetPosition(), m_kPlayer.KAniData.targetBallOutPos);
        double _lastAngle = m_kPlayer.GetRotAngle() + m_rorateAngle;
        _lastAngle = (_lastAngle + 360) % 360;
        if (dAngle > 270d && _lastAngle < 90d)
            dAngle -= 360d;
        double _afterAngle = dAngle - _lastAngle;

        //动画数据里面存在的左右线路//
        if (_afterAngle < 0d
            && m_combineData != null
            && (m_combineData.m_ballOutIndex > 0
            || m_combineData.m_ballInIndex > 0))
        {
            m_kMDir = MirrorDirection.Left;
        }
        else if (_afterAngle >= 0d
            && m_combineData != null
            && (m_combineData.m_ballOutIndex > 0
            || m_combineData.m_ballInIndex > 0))
        {
            m_kMDir = MirrorDirection.Right;
        }
        if (m_kAniClipList.Count > 0)
        {
            if (m_combineData != null)
            {
                if (m_kMDir == MirrorDirection.Left)
                {
                    if (m_combineData.m_ballOutIndex > 0 && m_combineData.m_ballOutIndex != m_combineData.m_ballInIndex)
                    {
                        if (m_kAniClipList[m_combineData.m_ballOutIndex - 1].AtionSide == 0 && m_kAniClipList[m_combineData.m_ballOutIndex - 1].BallOutTime > 0)
                        {
                            m_kAniClipList[m_combineData.m_ballOutIndex - 1].Mirror = true;
                        }
                    }
                    else if (m_combineData.m_ballInIndex > 0 && m_combineData.m_ballOutIndex != m_combineData.m_ballInIndex)
                    {
                        if (m_kAniClipList[m_combineData.m_ballInIndex - 1].AtionSide == 0 && m_kAniClipList[m_combineData.m_ballInIndex - 1].BallInTime > 0)
                        {
                            m_kAniClipList[m_combineData.m_ballInIndex - 1].Mirror = true;
                        }
                    }
                    else if (m_combineData.m_ballInIndex == m_combineData.m_ballOutIndex && m_combineData.m_ballOutIndex > 0)
                    {
                        if (m_kAniClipList[m_combineData.m_ballInIndex - 1].AtionSide == 0 && m_kAniClipList[m_combineData.m_ballOutIndex - 1].BallInTime > 0)
                        {
                            m_kAniClipList[m_combineData.m_ballInIndex - 1].Mirror = true;
                        }
                    }
                }
                if (m_kMDir == MirrorDirection.Right)
                {
                    if (m_combineData.m_ballOutIndex > 0 && m_combineData.m_ballOutIndex != m_combineData.m_ballInIndex)
                    {
                        if (m_kAniClipList[m_combineData.m_ballOutIndex - 1].AtionSide > 0 && m_kAniClipList[m_combineData.m_ballOutIndex - 1].BallOutTime > 0)
                        {
                            m_kAniClipList[m_combineData.m_ballOutIndex - 1].Mirror = true;
                        }
                    }
                    else if (m_combineData.m_ballInIndex > 0 && m_combineData.m_ballOutIndex != m_combineData.m_ballInIndex)
                    {
                        if (m_kAniClipList[m_combineData.m_ballInIndex - 1].AtionSide > 0 && m_kAniClipList[m_combineData.m_ballInIndex - 1].BallInTime > 0)
                        {
                            m_kAniClipList[m_combineData.m_ballInIndex - 1].Mirror = true;
                        }
                    }
                    else if (m_combineData.m_ballInIndex == m_combineData.m_ballOutIndex && m_combineData.m_ballOutIndex > 0)
                    {
                        if (m_kAniClipList[m_combineData.m_ballInIndex - 1].AtionSide > 0 && m_kAniClipList[m_combineData.m_ballInIndex - 1].BallInTime > 0)
                        {
                            m_kAniClipList[m_combineData.m_ballInIndex - 1].Mirror = true;
                        }
                    }
                }

            }
        }

    }

    /// <summary>
    /// 执行动画前的动作行为
    /// </summary>
    private List<AniClipData> m_kOtherClipDatas = new List<AniClipData>();
    private double m_fAfterAngle = 0f;

    private double m_invertTime = 0d;
    private double m_RunInverTime = 0d;

    private int m_iOtherIndex = 0;

    protected bool m_bHeadPass = false;
    
}
