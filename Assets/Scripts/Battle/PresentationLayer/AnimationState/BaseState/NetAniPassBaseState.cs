using Common;
using Common.Log;
using Common.Tables;
using System;
using System.Collections.Generic;

/// <summary>
/// 传球逻辑基类
/// </summary>
public class NetAniPassBaseState : AniBaseState
{
    public NetAniPassBaseState(EAniState kstate)
        : base(kstate)
    {

    }


    public override void OnEnter()
    {
        base.OnEnter();
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
                if (Math.Abs(m_rorateAngle) <= 45f && m_iClipIdx == 0)
                {
                    m_kPlayer.SetRoteAngle(m_kPlayer.GetRotAngle() + m_rorateAngle);
                }
                //得到动画产生的位移速度//
                GetAniMoveDir(m_kAniClipList[m_iClipIdx]);
                // 发消息需要播放动画 dracula_jin
                PlayAniMessage kMsg = new PlayAniMessage(m_kPlayer, m_kAniClipList[m_iClipIdx]);
                MessageDispatcher.Instance.SendMessage(kMsg);
                //是否启动技能标识//
                OnBeginSkill();
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
                        LogManager.Instance.RedLog("Player Angle==" + m_kPlayer.GetRotAngle());
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

    protected override void OnRotateAngle()
    {
        double _resetAngle = 0f;
        int _param = 1;
        float _invertAngle = 0f;
        double dAngle = MathUtil.GetAngle(m_kPlayer.GetPosition(), m_kPlayer.KAniData.targetPos);
        double _Cosangle = dAngle - m_kPlayer.GetRotAngle();
        double dDeltaAngle = Math.Abs(_Cosangle);
        if (_Cosangle > 0d)
        {
            if (_Cosangle>=180d)
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
            if (_Cosangle <=- 180d)
            {
                _param =1;
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
        m_bFaceTarget = true;
        if ((_resetAngle > 10d && _resetAngle <= 80d) || (_resetAngle >= 110d && _resetAngle <= 170d))
        {
            m_bFaceTarget = false;
        }
    }
}
