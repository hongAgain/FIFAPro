using System.Collections;
using Common;
using System;
using Common.Log;

public class NetAniCarryBaseState : AniBaseState
{

    public NetAniCarryBaseState(EAniState kstate)
        : base(kstate)
    {

    }
    public override void OnEnter()
    {
        //重载OnEnter的意义在于本状态，没有出球和进球点，但是动画序列中一直会存在动画球显示//
        BallVisableMeassage _ballMsg = new BallVisableMeassage(m_kPlayer, true);
        MessageDispatcher.Instance.SendMessage(_ballMsg);
        base.OnEnter();
    }

    /// <summary>
    /// 判断是否需要旋转角度
    /// </summary>
    protected override void OnRotateAngle()
    {
        double _resetAngle = 0f;
        int _param = 1;
        float _invertAngle = 0f;
        double dAngle = MathUtil.GetAngle(m_kPlayer.GetPosition(), m_kPlayer.KAniData.targetPos);
        double _Cosangle = dAngle - m_kPlayer.GetRotAngle();
        m_iRealAngle = _Cosangle;
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
        //当带球有转身动画的时候，启动坐标变化锁//
        if (m_RoateType != RoundData.Round0)
            m_kPlayer.CanMoveNext = false;
    }
    public override void OnUpdate(float fTime)
    {
        //状态是否延迟进行动画启动//
        if (m_stateDelayTime - fTime > 0)
        {
            m_stateDelayTime -= fTime;
            return;
        }

        if (m_kAniClipList != null && m_kAniClipList.Count > 0 && m_iClipIdx <= m_kAniClipList.Count - 1)
        {
            if (m_playTime <= 0f)
            {
                //如果角度在-45-45度,动画启动前硬转身//
                if (Math.Abs(m_iRealAngle) <= 45f && m_iClipIdx == 0)
                {
                    //m_kPlayer.RotateAngle += m_iRealAngle;
                    m_kPlayer.SetRoteAngle(m_kPlayer.GetRotAngle() + m_iRealAngle);
                    
                }
                //得到动画产生的位移速度//
                GetAniMoveDir(m_kAniClipList[m_iClipIdx]);
                // 发消息需要播放动画 dracula_jin
                PlayAniMessage kMsg = new PlayAniMessage(m_kPlayer, m_kAniClipList[m_iClipIdx]);
                MessageDispatcher.Instance.SendMessage(kMsg);
                //是否启动技能标识//
                OnBeginSkill();
                //检查旋转角度是否正确//
                CheckAngleViable();
            }
            if ((!m_kAniClipList[m_iClipIdx].Loop
                && m_playTime >= m_kAniClipList[m_iClipIdx].AllFrameTime / (GlobalBattleInfo.Instance.PlaySpeed * m_kAniClipList[m_iClipIdx].AniSpeed)))
            //if (m_kAniClipList[m_iClipIdx].Loop && m_playTime > 1)
            {
                if (true == CheckCrossfadeAniEnable())
                {
                    //默认转身动画只出现在第一个动画里，动画在<-45||>45//
                    if (m_iClipIdx == 0 && Math.Abs(m_iRealAngle) > 45f)
                    {
                        //当需要转身时候，进行强制切换角度//
                        //m_kPlayer.RotateAngle += m_iRealAngle;
                        //解除人物坐标发生变化锁//
                        m_kPlayer.CanMoveNext = true;
                        m_kPlayer.SetRoteAngle(m_kPlayer.GetRotAngle() + m_iRealAngle);
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
                    m_playTime += (fTime * m_kAniClipList[m_iClipIdx].AniSpeed);
                }
            }
            else
            {
                OnAniUpdate(m_playTime);
                SimulationAnimationMove(m_playTime, fTime);
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
    private void CheckAngleViable()
    {
        if (m_iRealAngle != 0)
        {
            if(Math.Abs(m_iRealAngle) <= 45f&&m_iClipIdx==0)
            {
                double dAngle = MathUtil.GetAngle(m_kPlayer.GetPosition(), m_kPlayer.KAniData.targetPos);
                if (Math.Abs(m_kPlayer.GetRotAngle() - dAngle) > 3f)
                {
                    m_kPlayer.SetRoteAngle(dAngle);
                }
            }
            else if (Math.Abs(m_iRealAngle)>45f&&m_iClipIdx>0)
            {
                double dAngle = MathUtil.GetAngle(m_kPlayer.GetPosition(), m_kPlayer.KAniData.targetPos);
                if (Math.Abs(m_kPlayer.GetRotAngle() - dAngle) > 3f)
                {
                    m_kPlayer.SetRoteAngle(dAngle);
                }
            }

        }

    }
    public override void OnExit()
    {
        base.OnExit();
        //退出状态时候，解除位移锁//
        m_kPlayer.CanMoveNext = true;
    }
    /// <summary>
    /// 对于带球，需要最终的转身角度，而不是动画控制的角度
    /// </summary>
    protected double m_iRealAngle = 0d;

}
