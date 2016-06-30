using System.Collections;
using Common;
using Common.Log;
using System;
using Common.Tables;
using System.Collections.Generic;


/// <summary>
/// 接球逻辑基类
/// </summary>
public class NetAniCatchBaseState : AniBaseState
{
    public NetAniCatchBaseState(EAniState kstate)
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
        m_AnistateSubName = "";
        m_combineData = null;
        m_stateDelayTime = 0f;
        m_kPlayer.AniFinish = false;
        m_iClipSpecialIndex = 0;
        m_kAniClipSpecialList.Clear();
        m_RoateType = RoundData.Round0;
        m_rorateInvertAngle = 0f;
        m_bFaceTarget = false;
        m_bAniFinish = true;
        m_kPlayer.CanUpdateRotate = false;
        m_fAfterAngle = 0f;
        m_invertTime = 0d;
        m_RunInverTime = 0d;
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
        //检测是否需要加入跑动动画到达接球点//
        double _ballFlyTime = m_kPlayer.KAniData.ballFlyingTime;
        double _PlayerMoveSpeed = m_kPlayer.KAniData.playerSpeed;
        Vector3D _targetPos = m_kPlayer.KAniData.targetPos;
        double _resetAngle = 0f;
        int _param = 1;
        float _invertAngle = 0f;
        double _targetAngle = 0d;
        double dAngle = MathUtil.GetAngle(m_kPlayer.GetPosition(), _targetPos);
        double dDeltaAngle = Math.Abs(dAngle - m_kPlayer.GetRotAngle());
        if (dDeltaAngle >= 180d)
        {
            _param = -1;
            _resetAngle = 360 - dDeltaAngle;
        }
        else
        {
            _param = 1;
            _resetAngle = dDeltaAngle;
        }
        if (_resetAngle >= 0d && _resetAngle <= 45d)
        {
            _invertAngle = (float)_resetAngle;
        }
        else if (_resetAngle > 45d && _resetAngle <= 145d)
        {
            _invertAngle = 90f;
        }
        else if (_resetAngle > 145d && _resetAngle <= 180d)
        {
            _invertAngle = 180f;
        }
        _targetAngle = (float)(_param * _invertAngle);
        if (_targetAngle < 0d || _targetAngle > 0d)
        {
            if (_invertAngle > 0f)
            {
                AniData _r;
                string _rName = "";
                if (90d - _invertAngle <= double.Epsilon)
                {
                    _rName = "Pao_Zhuan_R";
                }
                else
                {
                    _rName = "Pao_Zhuan_B";
                }
                _r = MatchAniHelper.Instance.GetAniDataByName(_rName);
                if (_r != null)
                {
                    AniClipData _data = MatchAniHelper.Instance.ResetClipData(_r);
                    if (_data != null)
                    {
                        _data.Loop = true;
                        m_kAniClipSpecialList.Add(_data);
                        m_invertTime += _r.m_aniAllFrameTime;
                    }
                }
            }
            else
            {
                m_fAfterAngle = m_kPlayer.GetRotAngle() + _targetAngle;
                m_kPlayer.SetRoteAngle(m_fAfterAngle);
            }
        }
        else
        {
            m_fAfterAngle = m_kPlayer.GetRotAngle();
        }
        double _distance = m_kPlayer.GetPosition().Distance(_targetPos);
        if (_distance > 0.5f)
        {
            AniData _r = MatchAniHelper.Instance.GetAniDataByName(m_RunStaticName);
            AniClipData _data = MatchAniHelper.Instance.ResetClipData(_r);
            _data.Loop = true;
            m_kAniClipSpecialList.Add(_data);
            m_RunInverTime = _distance / _PlayerMoveSpeed;
        }
        //是否需要对转身动画进行镜像//
        if (_targetAngle > 0 || _targetAngle < 0)
        {
            if (m_kAniClipSpecialList.Count > 0)
            {
                //右转
                if (_targetAngle > 0)
                {
                    //=1 左线路 =0 右线路
                    if (m_kAniClipSpecialList[0].AtionSide == 1)
                    {
                        m_kAniClipSpecialList[0].Mirror = true;
                    }
                }
                else if (_targetAngle < 0) //左转//
                {
                    //=1 左线路 =0 右线路
                    if (m_kAniClipSpecialList[0].AtionSide == 0)
                    {
                        m_kAniClipSpecialList[0].Mirror = true;
                    }
                }
            }
        }

    }

    /// <summary>
    /// 是否产生延时操作
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
    protected override void OnCrossFade()
    {
        if (m_kAniClipSpecialList != null && m_kAniClipSpecialList.Count > 0)
        {
            m_kInternalState = EInternalState.Special_Ani;
        }
        else
            m_kInternalState = EInternalState.CrossFadeAni;
    }
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
            if (m_kAniClipSpecialList != null && m_kAniClipSpecialList.Count > 0 && m_iClipSpecialIndex <= m_kAniClipSpecialList.Count - 1)
            {
                if (m_playTime == 0f)
                {
                    PlayAniMessage kMsg = new PlayAniMessage(m_kPlayer, m_kAniClipSpecialList[m_iClipSpecialIndex]);
                    MessageDispatcher.Instance.SendMessage(kMsg);
                }
                m_playTime += (fTime * m_kAniClipSpecialList[m_iClipSpecialIndex].AniSpeed);
                if (m_rorateAngle != 0)
                {
                    //先转身//
                    if ((m_kAniClipSpecialList[m_iClipSpecialIndex].AllFrameTime - m_playTime) <= fTime)
                    {
                        //当需要转身时候，进行强制切换角度//
                        //m_kPlayer.RotateAngle += m_rorateAngle;
                        m_kPlayer.SetRoteAngle(m_kPlayer.GetRotAngle() + m_rorateAngle);
                        m_playTime = 0f;
                        m_iClipSpecialIndex++;
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
                        m_iClipSpecialIndex++;
                        m_kPlayer.CanMoveNext = false;
                        return;
                    }
                    else
                    {
                        m_kPlayer.CanMoveNext = true;
                        m_kPlayer.MoveToPos(m_kPlayer.KAniData.targetPos,fTime);
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
                            AniAngleChangeMeassage kMsg = new AniAngleChangeMeassage(m_kPlayer, m_rorateAngle);
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
    private double m_fAfterAngle = 0f;
    protected List<AniClipData> m_kAniClipSpecialList = new List<AniClipData>();
    protected int m_iClipSpecialIndex = 0;

    private double m_invertTime = 0d;
    private double m_RunInverTime = 0d;
}
