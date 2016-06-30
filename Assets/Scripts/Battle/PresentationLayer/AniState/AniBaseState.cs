using Common;
using Common.Log;
using Common.Tables;
using System;
using System.Collections.Generic;
public class AniClipData
{
    public AniClipData()
    {
        //镜像//
        m_bMirror = false;
        //转身//
        m_bTurnBack = false;
        //延迟时间//
        m_fDelayTime = 0;
        //触球时间//
        m_fBallInTime = 0;
        //出球时间//
        m_fBallOutTime = 0;
        //接球的偏移值//
        m_kBallInOffset = Vector3D.zero;
        //出球的偏移值//
        m_kBallOutOffset = Vector3D.zero;
        //动画总时间//
        m_fAllFrameTime = 0f;
        // 动画是否循环播放
        m_bLoop = false;
        // 动画播放速度
        m_fAniSpeed = 1.2f;
        //动画产生位移的速度
        m_MoveSpeed = 0f;
        //动画位移角度//
        m_fAniAngle = 0f;
        //动画位移的开始帧//
        m_startMoveFrame = 0f;
        //动画位移的结束帧//
        m_endMoveFrame = 0f;
        //循环动画的持续时间//
        m_loopDurationTime = 0f;

        m_actionSide = 0;
        m_aniRoundAngle = 0;

    }

    public bool Mirror
    {
        get { return m_bMirror; }
        set { m_bMirror = value; }
    }
    public bool TurnBack
    {
        get { return m_bTurnBack; }
        set { m_bTurnBack = value; }
    }

    public float AniSpeed
    {
        get { return m_fAniSpeed; }
        set { m_fAniSpeed = value; }
    }

    public string AniName
    {
        get { return m_strAniName; }
        set { m_strAniName = value; }
    }

    public float DelayTime
    {
        get { return m_fDelayTime; }
        set { m_fDelayTime = value; }
    }

    public float BallInTime
    {
        get { return m_fBallInTime; }
        set { m_fBallInTime = value; }
    }

    public float BallOutTime
    {
        get { return m_fBallOutTime; }
        set { m_fBallOutTime = value; }
    }

    public Vector3D BallInOffset
    {
        get { return m_kBallInOffset; }
        set { m_kBallInOffset = value; }
    }
    public Vector3D BallOutOffset
    {
        get { return m_kBallOutOffset; }
        set { m_kBallOutOffset = value; }
    }
    public float AllFrameTime
    {
        get { return m_fAllFrameTime; }
        set { m_fAllFrameTime = value; }
    }

    public bool Loop
    {
        get { return m_bLoop; }
        set { m_bLoop = value; }
    }


    public float TargetSpeed
    {
        set { m_MoveSpeed = value; }
        get { return m_MoveSpeed; }
    }

    public float StartFrame
    {
        set { m_startMoveFrame = value; }
        get { return m_startMoveFrame; }
    }
    public float EndFrame
    {
        set { m_endMoveFrame = value; }
        get { return m_endMoveFrame; }
    }
    public float AniAngle
    {
        set { m_fAniAngle = value; }
        get { return m_fAniAngle; }
    }

    public float LoopDurationTime
    {
        set { m_loopDurationTime = value; }
        get { return m_loopDurationTime; }
    }

    public int AtionSide
    {
        set { m_actionSide = value; }
        get { return m_actionSide; }
    }

    public int AniRoundAngle
    {
        set { m_aniRoundAngle = value; }
        get { return m_aniRoundAngle; }
    }
    private bool m_bMirror;
    private string m_strAniName;
    private bool m_bTurnBack;
    private float m_fDelayTime;
    private float m_fBallInTime;
    private float m_fBallOutTime;
    private Vector3D m_kBallInOffset;
    private Vector3D m_kBallOutOffset;
    private float m_fAllFrameTime;
    private float m_fAniSpeed;
    private bool m_bLoop;

    private float m_MoveSpeed;
    private float m_startMoveFrame;
    private float m_endMoveFrame;
    private float m_fAniAngle;
    private float m_loopDurationTime = 999f;
    private int m_actionSide = 0;
    private int m_aniRoundAngle = 0;
}

public class AniBaseState
{
  
    public enum RoundData
    {
        Round0,
        Round90,
        Round180,
    }

    public enum MirrorDirection
    {
        Null,
        Left,
        Right,
    }
    protected enum EInternalState
    {
        /// <summary>
        /// 首帧事件
        /// </summary>
        CrossFadeAni = 0,
        Normal,
        BallIn,
        BallOut,
        PlayCanMove,
        WaitNext,
        /// <summary>
        /// 特殊的动画状态
        /// </summary>
        Special_Ani,
        Null ,
    }

    public AniBaseState(EAniState kState)
    {
        m_kAniState = kState;
    }

    public virtual void OnEnter()
    {
        if (m_kBall == null)
        {
            m_kBall = m_kPlayer.Team.Scene.Ball;
        }
        m_kPlayer.IsBallIn = false;
        m_kPlayer.IsBallOut = false;
        m_iClipIdx = 0;
        m_iLoopTime = 1;
        m_playTime = 0f;
        m_rorateAngle = 0f;
        m_kAniClipList.Clear();
        m_AnistateSubName = "";
        m_combineData = null;
        m_stateDelayTime = 0f;
        m_kPlayer.AniFinish = false;
        m_RoateType = RoundData.Round0;
        m_rorateInvertAngle = 0f;
        m_bFaceTarget = false;
        m_bAniFinish = true;
        m_kPlayer.CanUpdateRotate = false;
        m_kPlayer.SkillBegin = false;
        m_skillAniIndex = 0;
        m_kPlayer.CanMoveNext = true;//控制人物坐标发生变化的标志启动//
        OnRotateAngle();
        OnBegin();
        OnRoundMirror();
        OnNoRoundMirror();
        OnCrossFade();
    }


    protected virtual void onSkillDataChange()
    {
        if (m_kPlayer.IsChangeAniID)
        {
            int _id = 0;
            m_skillLogic.OnSkillRotateAngle(m_kPlayer, m_skillAniIndex, out _id);
            AniData _data = MatchAniHelper.Instance.GetAniDataById(_id);
            AniClipData _clipData = MatchAniHelper.Instance.ResetClipData(_data);
            if (m_kPlayer.SkillChangeAngle <= 0)
            {
                if (m_combineData != null)
                    m_kAniClipList[m_combineData.m_skillIndex] =_clipData;
                else
                {
                    LogManager.Instance.RedLog("This combine data is null,State==" + State);
                }
            }
            else
            {
                double dAngle = MathUtil.GetAngle(m_kPlayer.GetPosition(), m_kPlayer.KAniData.targetPos);
                m_kPlayer.SetRoteAngle(dAngle + m_kPlayer.SkillChangeAngle);
                m_rorateAngle = 0f;
                m_kAniClipList.Clear();
                m_kAniClipList.Add(_clipData);
            }
            m_kPlayer.IsChangeAniID = false;
        }

    }


    /// <summary>
    /// 决定读取数据类型已经做相应的判断
    /// </summary>
    protected virtual void OnBegin()
    {
        ReadDataToJson();
        onSkillDataChange();
    }

    /// <summary>
    /// 判断是否需要旋转角度
    /// </summary>
    protected virtual void OnRotateAngle()
    {
        double _resetAngle = 0f;
        int _param = 1;
        float _invertAngle = 0f;
        double dAngle = MathUtil.GetAngle(m_kPlayer.GetPosition(), m_kPlayer.KAniData.targetPos);
        double _Cosangle = dAngle - m_kPlayer.GetRotAngle();
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
    /// 检测是否需要加入转身位移等动作
    /// </summary>
    protected virtual void OnAddMove()
    {

    }
    protected virtual void OnCrossFade()
    {
        m_kInternalState = EInternalState.CrossFadeAni;
    }

    /// <summary>
    /// 检测是否需要进行转身动画镜像
    /// </summary>
    protected virtual void OnRoundMirror()
    {
        if (m_rorateAngle > 0 || m_rorateAngle < 0)
        {
            if (m_kAniClipList.Count > 0)
            {
                //右转
                if (m_rorateAngle > 0)
                {
                    //=1 左线路 =0 右线路
                    if (m_kAniClipList[0].AtionSide == 1)
                    {
                        m_kAniClipList[0].Mirror = true;
                    }
                }
                else if (m_rorateAngle < 0) //左转//
                {
                    //=1 左线路 =0 右线路
                    if (m_kAniClipList[0].AtionSide == 0)
                    {
                        m_kAniClipList[0].Mirror = true;
                    }
                }
            }
        }
    }
    /// <summary>
    /// 非转身镜像
    /// </summary>
    protected virtual void OnNoRoundMirror()
    {
        if (m_kAniClipList.Count <= 0 || m_combineData.m_AnimationIds.Count <= 0)
            return;
        OnMirrorAnim();
    }

    private void OnMirrorAnim()
    {
        double dAngle = MathUtil.GetAngle(m_kPlayer.GetPosition(), m_kPlayer.KAniData.targetPos);
        double _lastAngle = m_kPlayer.GetRotAngle() + m_rorateAngle;
        _lastAngle = (_lastAngle + 360) % 360;
        if (dAngle > 270d&&_lastAngle<90d)
            dAngle -= 360d;
        double _afterAngle = dAngle - _lastAngle;

        //动画数据里面存在的左右线路//
        if (_afterAngle < 0d
            && m_combineData != null
            && (m_combineData.m_ballOutIndex > 0
            || m_combineData.m_ballInIndex > 0))
        {
            m_kMDir = MirrorDirection.Left;
            LogManager.Instance.RedLog("目标在人物左侧,Player Angle===" + m_kPlayer.GetRotAngle() + "  RorateAngle===" + m_rorateAngle + "  _afterAngle===" + _afterAngle + " dAngle===" + dAngle + " LastAngle==" + _lastAngle);
        }
        else if (_afterAngle >= 0d
            && m_combineData != null
            && (m_combineData.m_ballOutIndex > 0
            || m_combineData.m_ballInIndex > 0))
        {
            m_kMDir = MirrorDirection.Right;
            LogManager.Instance.RedLog("目标在人物右侧,Player Angle===" + m_kPlayer.GetRotAngle() + "  RorateAngle===" + m_rorateAngle + "  _afterAngle===" + _afterAngle + " dAngle===" + dAngle + " LastAngle==" + _lastAngle);
        }
        if (m_kAniClipList.Count > 0)
        {
            if (m_combineData != null)
            {
                if (m_kMDir == MirrorDirection.Left)
                {
                    if (m_combineData.m_ballOutIndex > 0 && m_combineData.m_ballOutIndex != m_combineData.m_ballInIndex)
                    {
                        if (m_kAniClipList[m_combineData.m_ballOutIndex - 1].AtionSide == 0 && m_kAniClipList[m_combineData.m_ballOutIndex - 1].BallOutTime>0)
                        {
                            m_kAniClipList[m_combineData.m_ballOutIndex - 1].Mirror = true;
                            LogManager.Instance.RedLog("出球或者进球点在右线路,AnimationName===" + m_kAniClipList[m_combineData.m_ballOutIndex - 1].AniName);
//                             UnityEngine.Debug.Break();
                        }
                    }
                    else if (m_combineData.m_ballInIndex > 0 && m_combineData.m_ballOutIndex != m_combineData.m_ballInIndex)
                    {
                        if (m_kAniClipList[m_combineData.m_ballInIndex - 1].AtionSide == 0 && m_kAniClipList[m_combineData.m_ballInIndex - 1].BallInTime > 0)
                        {
                            m_kAniClipList[m_combineData.m_ballInIndex - 1].Mirror = true;
                            LogManager.Instance.RedLog("出球或者进球点在右线路,AnimationName===" + m_kAniClipList[m_combineData.m_ballInIndex - 1].AniName);
//                             UnityEngine.Debug.Break();
                        }
                    }
                    else if (m_combineData.m_ballInIndex == m_combineData.m_ballOutIndex && m_combineData.m_ballOutIndex > 0)
                    {
                        if (m_kAniClipList[m_combineData.m_ballInIndex - 1].AtionSide == 0 && m_kAniClipList[m_combineData.m_ballOutIndex - 1].BallInTime > 0)
                        {
                            m_kAniClipList[m_combineData.m_ballInIndex - 1].Mirror = true;
                            LogManager.Instance.RedLog("出球或者进球点在右线路,AnimationName===" + m_kAniClipList[m_combineData.m_ballInIndex - 1].AniName);
//                             UnityEngine.Debug.Break();
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
                            LogManager.Instance.RedLog("出球或者进球点在左线路,AnimationName===" + m_kAniClipList[m_combineData.m_ballOutIndex - 1].AniName);
//                             UnityEngine.Debug.Break();
                        }
                    }
                    else if (m_combineData.m_ballInIndex > 0 && m_combineData.m_ballOutIndex != m_combineData.m_ballInIndex)
                    {
                        if (m_kAniClipList[m_combineData.m_ballInIndex - 1].AtionSide > 0 && m_kAniClipList[m_combineData.m_ballInIndex - 1].BallInTime > 0)
                        {
                            m_kAniClipList[m_combineData.m_ballInIndex - 1].Mirror = true;
                            LogManager.Instance.RedLog("出球或者进球点在左线路,AnimationName===" + m_kAniClipList[m_combineData.m_ballInIndex - 1].AniName);
//                             UnityEngine.Debug.Break();
                        }
                    }
                    else if (m_combineData.m_ballInIndex == m_combineData.m_ballOutIndex && m_combineData.m_ballOutIndex > 0)
                    {
                        if (m_kAniClipList[m_combineData.m_ballInIndex - 1].AtionSide > 0 && m_kAniClipList[m_combineData.m_ballInIndex - 1].BallInTime > 0)
                        {
                            m_kAniClipList[m_combineData.m_ballInIndex - 1].Mirror = true;
                            LogManager.Instance.RedLog("出球或者进球点在左线路,AnimationName===" + m_kAniClipList[m_combineData.m_ballInIndex - 1].AniName);
//                             UnityEngine.Debug.Break();
                        }
                    }
                }
               
            }
        }
       
    }

    public virtual void OnExit()
    {
        m_iClipIdx = 0;
        m_iLoopTime = 1;
        m_playTime = 0f;
        m_rorateAngle = 0f;
        m_kAniClipList.Clear();
        m_AnistateSubName = "";
        m_combineData = null;
        m_stateDelayTime = 0f;
        m_kPlayer.AniFinish = false;
        m_kPlayer.IsBallIn = false;
        m_kPlayer.IsBallOut = false;
        m_RoateType = RoundData.Round0;
        m_rorateInvertAngle = 0f;
        m_bFaceTarget = false;
        m_bAniFinish = true;
        m_kPlayer.CanUpdateRotate = true;
        m_kPlayer.KAniData.headRobAvil = false;
    }

    public virtual void OnUpdate(float fTime)
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
                    //m_kPlayer.RotateAngle += m_rorateAngle;
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



    protected void OnAniUpdate(float fTime)
    {

        switch (m_kInternalState)
        {
            case EInternalState.CrossFadeAni:
                InternalCrossFadeAniState();
                break;
            case EInternalState.BallIn:
                InternalBallInState();
                break;
            case EInternalState.BallOut:
                InternalBallOutState();
                break;
            default:
                break;
        }
    }


    /// <summary>
    /// 关于是否启动技能，可以在这里复写
    /// </summary>
    protected virtual void OnBeginSkill()
    {

    }

    protected void InternalCrossFadeAniState()
    {
        if (0 == m_kAniClipList.Count)
        {
            LogManager.Instance.RedLog("Clip Data is null");
            return;
        }
        m_kInternalState = EInternalState.BallIn;
    }

    protected virtual void InternalBallInState()
    {
        m_kPlayer.IsBallIn = false;
        if (m_combineData != null)
        {
            if (m_combineData.m_ballInIndex > 0 && m_iClipIdx + 1 == m_combineData.m_ballInIndex)
            {
                if (m_playTime >= m_kAniClipList[m_iClipIdx].BallInTime)
                {
                    m_kPlayer.IsBallIn = true;
                    m_kInternalState = EInternalState.BallOut;
                    BallVisableMeassage _ballMsg = new BallVisableMeassage(m_kPlayer, true);
                    MessageDispatcher.Instance.SendMessage(_ballMsg);
                    //                     LogManager.Instance.RedLog("Yes,is Ball in");
                }
            }
            else
                m_kInternalState = EInternalState.BallOut;
        }
        else
            LogManager.Instance.RedLog("This combine data is null,State==" + State);

    }
    protected virtual void InternalBallOutState()
    {
        m_kPlayer.IsBallOut = false;
        if (m_combineData != null)
        {
            if (m_combineData.m_ballOutIndex > 0 && m_iClipIdx + 1 == m_combineData.m_ballOutIndex)
            {
                if (m_playTime >= m_kAniClipList[m_iClipIdx].BallOutTime)
                {
                    m_kPlayer.IsBallOut = true;
                    m_kInternalState = EInternalState.Null;
                    Vector3D _v = MathUtil.GetNewVector3dForWorld(m_kAniClipList[m_iClipIdx].BallOutOffset,
                        m_kPlayer.GetRotAngle(),
                        m_kAniClipList[m_iClipIdx].Mirror == true ? -1 : 1);
                    m_kBall.SetPosition(m_kPlayer.GetPosition() + _v);
                    m_kBall.SetTarget(m_kBall.GetPosition());

                    BallVisableMeassage _ballMsg = new BallVisableMeassage(m_kPlayer, false);
                    MessageDispatcher.Instance.SendMessage(_ballMsg);
                    //                     LogManager.Instance.RedLog("Yes,is Ball Out");
                }
            }
            else
                m_kInternalState = EInternalState.Null;
        }
        else
            LogManager.Instance.RedLog("This combine data is null,State==" + State);
    }

    protected virtual void OnFinish()
    {
        m_kPlayer.CanUpdateRotate = true;
    }
    protected void ReadDataToJson()
    {
        if (m_AnistateSubName != string.Empty)
        {
            AniStateLayer _layer = MatchAniHelper.Instance.GetAniIdByStateName(m_AnistateSubName);
            if (_layer == null)
            {
                LogManager.Instance.RedLog("Enter " + State + " ,This is error state,stateName===" + m_AnistateSubName);
                return;
            }
            m_skillAniIndex = _layer.m_stateIndex;
            List<AniData> _datas = MatchAniHelper.Instance.GetAnimationDataByAniType(_layer.m_stateId, out m_combineData);
            if (_datas != null && _datas.Count > 0)
            {
                for (int i = 0; i < _datas.Count; i++)
                {
                    AniClipData _data = MatchAniHelper.Instance.ResetClipData(_datas[i]);
                    m_kAniClipList.Add(_data);
                }
            }
        }
        else
            LogManager.Instance.RedLog(m_AnistateSubName + " is  SubName is NULL,Check it,State==" + State + "  Prestate===" + m_kPreState
                + " PlayerId===" + m_kPlayer.PlayerBaseInfo.PlayerID + "team==" + m_kPlayer.Team.TeamColor);
    }

    protected virtual void GetAniMoveDir(AniClipData _data)
    {
        if (_data != null)
        {
            if (_data.StartFrame > 0)
            {
                double _angle = _data.AniAngle;
                double _PAngle = m_kPlayer.GetRotAngle() + _angle;
                Vector3D _moveDir = MathUtil.GetDirFormAngle(_PAngle);
                m_AniMoveDir = _moveDir;
            }
        }
    }

    protected virtual void SimulationAnimationMove(float _Durtime, float _invertTime)
    {
        if (m_kAniClipList[m_iClipIdx] != null)
        {
            if (m_kAniClipList[m_iClipIdx].StartFrame > 0
                && _Durtime >= m_kAniClipList[m_iClipIdx].StartFrame
                && _Durtime <= m_kAniClipList[m_iClipIdx].EndFrame)
            {
                Vector3D _v = m_kPlayer.GetPosition()+m_AniMoveDir * _invertTime * m_kAniClipList[m_iClipIdx].TargetSpeed;
                m_kPlayer.SetPosition (_v) ;
            }

        }
    }

    /// <summary>
    /// 检查是否可以切换到下一个动画序列
    /// </summary>
    /// <returns></returns>
    public bool CheckCrossfadeAniEnable()
    {
        int iCurInternalVal = (int)m_kInternalState;

        if(m_combineData.m_ballInIndex == m_combineData.m_ballOutIndex && 0 != m_combineData.m_ballInIndex)
        {
            if (m_combineData.m_ballOutIndex > 0 && m_iClipIdx + 1 == m_combineData.m_ballOutIndex)
            {
                if (iCurInternalVal > (int)EInternalState.BallOut)
                    return true;
            }
        }
        else
        {
            if (m_combineData.m_ballInIndex > 0 && m_iClipIdx + 1 == m_combineData.m_ballInIndex)
            {
                if (iCurInternalVal > (int)EInternalState.BallIn)
                    return true;
            }
            else if (m_combineData.m_ballOutIndex > 0 && m_iClipIdx + 1 == m_combineData.m_ballOutIndex)
            {
                if (iCurInternalVal > (int)EInternalState.BallOut)
                    return true;
            }
            else if (0 == m_combineData.m_ballOutIndex)
            {
                if (iCurInternalVal > (int)EInternalState.BallOut)
                    return true;
            }
            else if (0 == m_combineData.m_ballInIndex)
            {
                if (iCurInternalVal > (int)EInternalState.BallIn)
                    return true;
            }
        }

       
        return false;
    }

    public EAniState State
    {
        get { return m_kAniState; }
        private set { m_kAniState = value; }
    }

    public EAniState PreState
    {
        get { return m_kPreState; }
        set { m_kPreState = value; }
    }

    public LLUnit Player
    {
        get { return m_kPlayer; }
        set { m_kPlayer = value; }
    }


    protected LLUnit m_kPlayer = null;
    protected EAniState m_kAniState = EAniState.EAS_NULL;
    protected EAniState m_kPreState = EAniState.EAS_NULL;
    protected int m_iLoopTime = 1;
    protected int m_iClipIdx = 0;
    protected List<AniClipData> m_kAniClipList = new List<AniClipData>();
    /// <summary>
    /// 动画播放时间
    /// </summary>
    protected float m_playTime = 0f;
    protected LLBall m_kBall = null;
    protected float m_fCrossFadeTime = -1;

    protected float m_fAniSpeed = 0; // for game pause and resume
    protected float m_rorateAngle = 0;
    protected float m_rorateInvertAngle = 0f;
    protected MirrorDirection m_kMDir = MirrorDirection.Null;
    /// <summary>
    /// 标注期望本次希望产生的行为是出球还是入球
    /// </summary>
    protected EInternalState m_kInternalState = EInternalState.CrossFadeAni;

    protected string m_AnistateSubName = "";
    protected AniCombineData m_combineData = null;
    protected Vector3D m_AniMoveDir = Vector3D.zero;
    protected float m_stateDelayTime = 0f;
    protected RoundData m_RoateType = RoundData.Round0;
    protected bool m_bFaceTarget = false;
    protected bool m_bAniFinish = false;
    protected NetAniSkillLogic m_skillLogic = new NetAniSkillLogic();
    protected int m_skillAniIndex = 0;

    protected string m_RunStaticName = "Pao-ManPao";
}