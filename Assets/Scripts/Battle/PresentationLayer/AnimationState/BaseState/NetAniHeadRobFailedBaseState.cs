using System.Collections;
using Common;
using Common.Log;
using System;
using Common.Tables;
using System.Collections.Generic;


/// <summary>
/// 头球失败基类
/// </summary>
public class NetAniHeadRobFailedBaseState : AniBaseState 
{
    public NetAniHeadRobFailedBaseState(EAniState kstate)
        : base(kstate)
    {

    }
    public override void OnEnter()
    {
        if (m_kBall == null)
        {
            m_kBall = m_kPlayer.Team.Scene.Ball;
        }
        m_kPlayer.AniFinish = false;
        m_playTime = 0f;
        m_kOtherClipDatas.Clear();
        m_AnistateSubName = "";
        m_combineData = null;
        m_stateDelayTime = 0f;
        m_bAniFinish = false;
        m_kPlayer.CanUpdateRotate = false;
        m_invertTime = 0d;
        m_RunInverTime = 0d;
        m_iOtherIndex = 0;
        m_dRunRorateAngle = 0;
        OnAddMove();
        OnRotateAngle();
        OnCrossFade();
    }

    protected override void OnAddMove()
    {
        // 是否需要跑动到接球点 //
        double _distance = m_kPlayer.GetPosition().Distance(m_kPlayer.KAniData.targetPos);
        if (_distance > 0.5f)
        {
            m_RunStaticName = "Pao-ManPao";
            AniData _r = MatchAniHelper.Instance.GetAniDataByName(m_RunStaticName);
            AniClipData _data = MatchAniHelper.Instance.ResetClipData(_r);
            _data.Loop = true;
            m_kOtherClipDatas.Add(_data);
            m_RunInverTime = _distance / m_kPlayer.KAniData.playerSpeed;
        }
        m_stateDelayTime = (float)(m_kPlayer.KAniData.ballFlyingTime - m_RunInverTime);
        m_stateDelayTime -= 0.42f;
    }


    protected override void OnRotateAngle()
    {
        m_dRunRorateAngle = MathUtil.GetAngle(m_kPlayer.GetPosition(), m_kPlayer.KAniData.targetPos);
    }


    public override void OnUpdate(float fTime)
    {
        if (m_stateDelayTime - fTime >= 0)
        {
            m_stateDelayTime -= fTime;
            return;
        }
        if(m_kOtherClipDatas!=null&&m_kOtherClipDatas.Count>0&&m_iOtherIndex<m_kOtherClipDatas.Count-1)
        {
            if (m_playTime == 0f)
            {
                if(m_dRunRorateAngle>0d)
                {
                    m_kPlayer.SetRoteAngle(m_dRunRorateAngle);
                }
                PlayAniMessage kMsg = new PlayAniMessage(m_kPlayer, m_kOtherClipDatas[m_iOtherIndex]);
                MessageDispatcher.Instance.SendMessage(kMsg);
            }
            Vector3D nextPosition = m_kPlayer.GetPosition() + MathUtil.GetDir(m_kPlayer.GetPosition(), m_kPlayer.KAniData.targetPos) * m_kPlayer.Velocity * fTime;
            double _distance = nextPosition.Distance(m_kPlayer.KAniData.targetPos);
            //再启动跑步
            if (_distance <= 0.5f)
            {
                m_playTime = 0f;
                m_iOtherIndex++;
                return;
            }
            else
            {
                m_kPlayer.MoveToPos(m_kPlayer.KAniData.targetPos, fTime);
            }
        }
        else
        {
            OnFinish();
        }
    }
    protected override void OnFinish()
    {
        if (m_bAniFinish)
            m_kPlayer.AniFinish = false;
        m_kPlayer.SetAniState(EAniState.Special_Idle);
    }
    /// <summary>
    /// 执行动画前的动作行为
    /// </summary>
    private List<AniClipData> m_kOtherClipDatas = new List<AniClipData>();
    private double m_fAfterAngle = 0f;

    private double m_invertTime = 0d;
    private double m_RunInverTime = 0d;
    private double m_dRunRorateAngle = 0d;
    private double m_dJumpRorateAngle = 0d;
    private int m_iOtherIndex = 0;
}
