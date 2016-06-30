using System;
using Common;

public enum NetAniGKSaveSuccessSubState
{
    // 横向扑救
    EAS_EnterGKNIdleToHorizontalSaveSuccess,
    EAS_EnterGKNFWalkToHorizontalSaveSuccess,
    EAS_EnterGKNBWalkToHorizontalSaveSuccess,
    EAS_EnterGKNLWalkToHorizontalSaveSuccess,
    EAS_EnterGKNRWalkToHorizontalSaveSuccess,

    // 侧上方扑救
    EAS_EnterGKNIdleToSlideSaveSuccess,
    EAS_EnterGKGKNFWalkToSlideSaveSuccess,
    EAS_EnterGKGKNBWalkToSlideSaveSuccess,
    EAS_EnterGKGKNLWalkToSlideSaveSuccess,
    EAS_EnterGKGKNRWalkToSlideSaveSuccess,

    // 中路蹲下	
    EAS_EnterGKGKNIdleToSaveSuccessMiddleSquate,
    EAS_EnterGKGKFWalkToSaveSuccessMiddleSquate,
    EAS_EnterGKGKBWalkToSaveSuccessMiddleSquate,
    EAS_EnterGKGKLWalkToSaveSuccessMiddleSquate,
    EAS_EnterGKGKRWalkToSaveSuccessMiddleSquate,

    // 中路跳起
    EAS_EnterGKGKNIdleToSaveSuccessMiddleJump,
    EAS_EnterGKGKFWalkToSaveSuccessMiddleJump,
    EAS_EnterGKGKBWalkToSaveSuccessMiddleJump,
    EAS_EnterGKGKLWalkToSaveSuccessMiddleJump,
    EAS_EnterGKGKRWalkToSaveSuccessMiddleJump,

    // 托出底线
    EAS_EnterGKGKNIdleToSaveSuccessOutBottom,
    EAS_EnterGKGKFWalkToSaveSuccessOutBottom,
    EAS_EnterGKGKBWalkToSaveSuccessOutBottom,
    EAS_EnterGKGKLWalkToSaveSuccessOutBottom,
    EAS_EnterGKGKRWalkToSaveSuccessOutBottom
}


public enum NetAniGKSaveFailedSubState
{

    // 横向扑救
    EAS_EnterGKGKNIdleToHorizontalFailed,
    EAS_EnterGKGKFWalkToHorizontalFailed,
    EAS_EnterGKGKBWalkToHorizontalFailed,
    EAS_EnterGKGKLWalkToHorizontalFailed,
    EAS_EnterGKGKRWalkToHorizontalFailed,

    // 侧上方扑救
    EAS_EnterGKNIdleToSlideSaveFailed,
    EAS_EnterGKGKNFWalkToSlideSaveFailed,
    EAS_EnterGKGKNBWalkToSlideSaveFailed,
    EAS_EnterGKGKNLWalkToSlideSaveFailed,
    EAS_EnterGKGKNRWalkToSlideSaveFailed,

    // 中路蹲下
    EAS_EnterGKGKNIdleToSaveFailedMiddleSquate,
    EAS_EnterGKGKFWalkToSaveFailedMiddleSquate,
    EAS_EnterGKGKBWalkToSaveFailedMiddleSquate,
    EAS_EnterGKGKLWalkToSaveFailedMiddleSquate,
    EAS_EnterGKGKRWalkToSaveFailedMiddleSquate,

    // 中路跳起
    EAS_EnterGKGKNIdleToSaveFailedMiddleJump,
    EAS_EnterGKGKFWalkToSaveFailedMiddleJump,
    EAS_EnterGKGKBWalkToSaveFailedMiddleJump,
    EAS_EnterGKGKLWalkToSaveFailedMiddleJump,
    EAS_EnterGKGKRWalkToSaveFailedMiddleJump,

    // 托出底线失败//
    EAS_EnterGKGKNIdleToSaveSuccessOutBottom,
    EAS_EnterGKGKFWalkToSaveSuccessOutBottom,
    EAS_EnterGKGKBWalkToSaveSuccessOutBottom,
    EAS_EnterGKGKLWalkToSaveSuccessOutBottom,
    EAS_EnterGKGKRWalkToSaveSuccessOutBottom,

}
/// <summary>
/// 守门员扑救方式
/// </summary>
public enum GKSaveType
{
    Left_HorizonSave,
    Righ_HorizonSave,
    Left_SlideSave,
    Right_SlideSave,

    MiddleSquate,
    MiddleJump,
}

public class NetAniGKSaveBaseState : NetAniGKBaseState
{
    public NetAniGKSaveBaseState(EAniState kState)
        : base(kState)
    {

    }

    protected void GetGKSaveType(Vector3D kTargetPos, ref GKSaveType kType)
    {
        switch (m_kPlayer.Team.TeamColor)
        {
            case ETeamColor.Team_Red:
                if (kTargetPos.X > m_kPlayer.GetPosition().X)
                {
                    if (kTargetPos.Y >= 2)
                    {
                        kType = GKSaveType.Right_SlideSave;
                    }
                    else
                    {
                        kType = GKSaveType.Righ_HorizonSave;
                    }
                }
                else if (kTargetPos.X < m_kPlayer.GetPosition().X)
                {
                    if (kTargetPos.Y >= 2)
                    {
                        kType = GKSaveType.Left_SlideSave;

                    }
                    else
                    {
                        kType = GKSaveType.Left_HorizonSave;
                    }
                }
                break;
            case ETeamColor.Team_Blue:
                if (kTargetPos.X > m_kPlayer.GetPosition().X)
                {
                    if (kTargetPos.Y >= 2)
                    {
                        kType = GKSaveType.Left_SlideSave;

                    }
                    else
                    {
                        kType = GKSaveType.Left_HorizonSave;
                    }
                }
                else if (kTargetPos.X < m_kPlayer.GetPosition().X)
                {
                    if (kTargetPos.Y >= 2)
                    {
                        kType = GKSaveType.Right_SlideSave;
                    }
                    else
                    {
                        kType = GKSaveType.Righ_HorizonSave;
                    }
                }
                break;
        }
    }

    protected void GetSubAniName()
    { 
    }
}
/// <summary>
/// 守门员扑救成功
/// </summary>
/// 

public class NetAniGKSaveSuccessState : NetAniGKSaveBaseState
{
    public NetAniGKSaveSuccessState()
        : base(EAniState.GK_Save_Success)
    {

    }

    protected override void OnRotateAngle()
    {
        
    }
    protected override void OnBegin()
    {
        m_kPreState = EAniState.Idle;
        //重载函数，得到具体的前置动画//
        GKSaveType _GkSaveType = GKSaveType.Left_HorizonSave;
        Vector3D _targetPos = m_kPlayer.KAniData.targetPos;

        if (Math.Abs(_targetPos.X) <= 1f)
        {
            if (_targetPos.Y >= 2)
            {
                _GkSaveType = GKSaveType.MiddleJump;

            }
            else
            {
                _GkSaveType = GKSaveType.MiddleSquate;
            }
            m_kPlayer.SetPosition(new Vector3D(_targetPos.X, m_kPlayer.GetPosition().Y, _targetPos.Z));
        }
        else
            GetGKSaveType(_targetPos,ref _GkSaveType);
        switch (_GkSaveType)
        {
            case GKSaveType.Left_HorizonSave:
                GetHorizonalAniSubName(true);
                break;
            case GKSaveType.Left_SlideSave:
                GetSlideAniSubName(true);
                break;
            case GKSaveType.Righ_HorizonSave:
                GetHorizonalAniSubName(false);
                break;
            case GKSaveType.Right_SlideSave:
                GetSlideAniSubName(false);
                break;
            case GKSaveType.MiddleJump:
                GetMiddleJumpSubName();
                break;
            case GKSaveType.MiddleSquate:
                GetMiddleSquateSubName();
                break;
        }
        base.OnBegin();
    }

    private void GetHorizonalAniSubName(bool _left)
    {
        switch (m_kPreState)
        {
            case EAniState.Idle:
                m_AnistateSubName = NetAniGKSaveSuccessSubState.EAS_EnterGKNIdleToHorizontalSaveSuccess.ToString();
                break;
            case EAniState.GK_FrontWalk:
                m_AnistateSubName = NetAniGKSaveSuccessSubState.EAS_EnterGKNFWalkToHorizontalSaveSuccess.ToString();
                break;
            case EAniState.GK_BackWalk:
                m_AnistateSubName = NetAniGKSaveSuccessSubState.EAS_EnterGKNBWalkToHorizontalSaveSuccess.ToString();
                break;
            case EAniState.GK_LeftWalk:
                m_AnistateSubName = NetAniGKSaveSuccessSubState.EAS_EnterGKNLWalkToHorizontalSaveSuccess.ToString();
                break;
            case EAniState.GK_RightWalk:
                m_AnistateSubName = NetAniGKSaveSuccessSubState.EAS_EnterGKNRWalkToHorizontalSaveSuccess.ToString();
                break;
        }
        if (_left == false)
        {
            m_kMDir = MirrorDirection.Right;
        }
        else
            m_kMDir = MirrorDirection.Left;
    }

    private void GetSlideAniSubName(bool _left)
    {
        switch (m_kPreState)
        {
            case EAniState.Idle:
                m_AnistateSubName = NetAniGKSaveSuccessSubState.EAS_EnterGKNIdleToSlideSaveSuccess.ToString();
                break;
            case EAniState.GK_FrontWalk:
                m_AnistateSubName = NetAniGKSaveSuccessSubState.EAS_EnterGKGKNFWalkToSlideSaveSuccess.ToString();
                break;
            case EAniState.GK_BackWalk:
                m_AnistateSubName = NetAniGKSaveSuccessSubState.EAS_EnterGKGKNBWalkToSlideSaveSuccess.ToString();
                break;
            case EAniState.GK_LeftWalk:
                m_AnistateSubName = NetAniGKSaveSuccessSubState.EAS_EnterGKGKNLWalkToSlideSaveSuccess.ToString();
                break;
            case EAniState.GK_RightWalk:
                m_AnistateSubName = NetAniGKSaveSuccessSubState.EAS_EnterGKGKNRWalkToSlideSaveSuccess.ToString();
                break;
        }
        if (_left == false)
        {
            m_kMDir = MirrorDirection.Right;
        }
        else
            m_kMDir = MirrorDirection.Left;
    }


    private void GetMiddleSquateSubName()
    {
        switch (m_kPreState)
        {
            case EAniState.Idle:
                m_AnistateSubName = NetAniGKSaveSuccessSubState.EAS_EnterGKGKNIdleToSaveSuccessMiddleSquate.ToString();
                break;
            case EAniState.GK_FrontWalk:
                m_AnistateSubName = NetAniGKSaveSuccessSubState.EAS_EnterGKGKFWalkToSaveSuccessMiddleSquate.ToString();
                break;
            case EAniState.GK_BackWalk:
                m_AnistateSubName = NetAniGKSaveSuccessSubState.EAS_EnterGKGKBWalkToSaveSuccessMiddleSquate.ToString();
                break;
            case EAniState.GK_LeftWalk:
                m_AnistateSubName = NetAniGKSaveSuccessSubState.EAS_EnterGKGKLWalkToSaveSuccessMiddleSquate.ToString();
                break;
            case EAniState.GK_RightWalk:
                m_AnistateSubName = NetAniGKSaveSuccessSubState.EAS_EnterGKGKRWalkToSaveSuccessMiddleSquate.ToString();
                break;
        }
    }

    private void GetMiddleJumpSubName()
    {
        switch (m_kPreState)
        {
            case EAniState.Idle:
                m_AnistateSubName = NetAniGKSaveSuccessSubState.EAS_EnterGKGKNIdleToSaveSuccessMiddleJump.ToString();
                break;
            case EAniState.GK_FrontWalk:
                m_AnistateSubName = NetAniGKSaveSuccessSubState.EAS_EnterGKGKFWalkToSaveSuccessMiddleJump.ToString();
                break;
            case EAniState.GK_BackWalk:
                m_AnistateSubName = NetAniGKSaveSuccessSubState.EAS_EnterGKGKBWalkToSaveSuccessMiddleJump.ToString();
                break;
            case EAniState.GK_LeftWalk:
                m_AnistateSubName = NetAniGKSaveSuccessSubState.EAS_EnterGKGKLWalkToSaveSuccessMiddleJump.ToString();
                break;
            case EAniState.GK_RightWalk:
                m_AnistateSubName = NetAniGKSaveSuccessSubState.EAS_EnterGKGKRWalkToSaveSuccessMiddleJump.ToString();
                break;
        }
    }

    protected override void OnFinish()
    {
        m_kPlayer.CanUpdateRotate = false;
    }
}

/// <summary>
/// 门将扑救失败
/// </summary>
public class NetAniGKSaveFailedState : NetAniGKSaveBaseState
{
    public NetAniGKSaveFailedState()
        : base(EAniState.GK_Save_Failed)
    {

    }

    protected override void OnRotateAngle()
    {

    }


    protected override void OnBegin()
    {
        m_kPreState = EAniState.Idle;
        //重载函数，得到具体的前置动画//
        GKSaveType _GkSaveType = GKSaveType.Left_HorizonSave;
        Vector3D _targetPos = m_kPlayer.KAniData.targetPos;

		if (Math.Abs(_targetPos.X) <= 1f)
        {
            if (_targetPos.Y >= 2)
            {
                _GkSaveType = GKSaveType.MiddleJump;
            }
            else
            {
                _GkSaveType = GKSaveType.MiddleSquate;
            }
            m_kPlayer.SetPosition(new Vector3D(_targetPos.X, m_kPlayer.GetPosition().Y, _targetPos.Z));
        }
        else
            GetGKSaveType(_targetPos, ref _GkSaveType);
        switch (_GkSaveType)
        {
            case GKSaveType.Left_HorizonSave:
                GetHorizonalAniSubName(true);
                break;
            case GKSaveType.Left_SlideSave:
                GetSlideAniSubName(true);
                break;
            case GKSaveType.Righ_HorizonSave:
                GetHorizonalAniSubName(false);
                break;
            case GKSaveType.Right_SlideSave:
                GetSlideAniSubName(false);
                break;
            case GKSaveType.MiddleJump:
                GetMiddleJumpSubName();
                break;
            case GKSaveType.MiddleSquate:
                GetMiddleSquateSubName();
                break;
        }
        base.OnBegin();
    }

    protected override void OnCrossFade()
    {
        m_failedExitTime = 0.15f;
        base.OnCrossFade();
    }

    private void GetHorizonalAniSubName(bool _left)
    {
        switch (m_kPreState)
        {
            case EAniState.Idle:
                m_AnistateSubName = NetAniGKSaveFailedSubState.EAS_EnterGKGKNIdleToHorizontalFailed.ToString();
                break;
            case EAniState.GK_FrontWalk:
                m_AnistateSubName = NetAniGKSaveFailedSubState.EAS_EnterGKGKFWalkToHorizontalFailed.ToString();
                break;
            case EAniState.GK_BackWalk:
                m_AnistateSubName = NetAniGKSaveFailedSubState.EAS_EnterGKGKBWalkToHorizontalFailed.ToString();
                break;
            case EAniState.GK_LeftWalk:
                m_AnistateSubName = NetAniGKSaveFailedSubState.EAS_EnterGKGKLWalkToHorizontalFailed.ToString();
                break;
            case EAniState.GK_RightWalk:
                m_AnistateSubName = NetAniGKSaveFailedSubState.EAS_EnterGKGKRWalkToHorizontalFailed.ToString();
                break;
        }
        if (_left == false)
        {
            m_kMDir = MirrorDirection.Right;
        }
        else
            m_kMDir = MirrorDirection.Left;
    }

    private void GetSlideAniSubName(bool _left)
    {
        switch (m_kPreState)
        {
            case EAniState.Idle:
                m_AnistateSubName = NetAniGKSaveFailedSubState.EAS_EnterGKNIdleToSlideSaveFailed.ToString();
                break;
            case EAniState.GK_FrontWalk:
                m_AnistateSubName = NetAniGKSaveFailedSubState.EAS_EnterGKGKNFWalkToSlideSaveFailed.ToString();
                break;
            case EAniState.GK_BackWalk:
                m_AnistateSubName = NetAniGKSaveFailedSubState.EAS_EnterGKGKNBWalkToSlideSaveFailed.ToString();
                break;
            case EAniState.GK_LeftWalk:
                m_AnistateSubName = NetAniGKSaveFailedSubState.EAS_EnterGKGKNLWalkToSlideSaveFailed.ToString();
                break;
            case EAniState.GK_RightWalk:
                m_AnistateSubName = NetAniGKSaveFailedSubState.EAS_EnterGKGKNRWalkToSlideSaveFailed.ToString();
                break;
        }
        if (_left == false)
        {
            m_kMDir = MirrorDirection.Right;
        }
        else
            m_kMDir = MirrorDirection.Left;
    }


    private void GetMiddleSquateSubName()
    {
        switch (m_kPreState)
        {
            case EAniState.Idle:
                m_AnistateSubName = NetAniGKSaveFailedSubState.EAS_EnterGKGKNIdleToSaveFailedMiddleSquate.ToString();
                break;
            case EAniState.GK_FrontWalk:
                m_AnistateSubName = NetAniGKSaveFailedSubState.EAS_EnterGKGKFWalkToSaveFailedMiddleSquate.ToString();
                break;
            case EAniState.GK_BackWalk:
                m_AnistateSubName = NetAniGKSaveFailedSubState.EAS_EnterGKGKBWalkToSaveFailedMiddleSquate.ToString();
                break;
            case EAniState.GK_LeftWalk:
                m_AnistateSubName = NetAniGKSaveFailedSubState.EAS_EnterGKGKLWalkToSaveFailedMiddleSquate.ToString();
                break;
            case EAniState.GK_RightWalk:
                m_AnistateSubName = NetAniGKSaveFailedSubState.EAS_EnterGKGKRWalkToSaveFailedMiddleSquate.ToString();
                break;
        }
    }

    private void GetMiddleJumpSubName()
    {
        switch (m_kPreState)
        {
            case EAniState.Idle:
                m_AnistateSubName = NetAniGKSaveFailedSubState.EAS_EnterGKGKNIdleToSaveFailedMiddleJump.ToString();
                break;
            case EAniState.GK_FrontWalk:
                m_AnistateSubName = NetAniGKSaveFailedSubState.EAS_EnterGKGKFWalkToSaveFailedMiddleJump.ToString();
                break;
            case EAniState.GK_BackWalk:
                m_AnistateSubName = NetAniGKSaveFailedSubState.EAS_EnterGKGKBWalkToSaveFailedMiddleJump.ToString();
                break;
            case EAniState.GK_LeftWalk:
                m_AnistateSubName = NetAniGKSaveFailedSubState.EAS_EnterGKGKLWalkToSaveFailedMiddleJump.ToString();
                break;
            case EAniState.GK_RightWalk:
                m_AnistateSubName = NetAniGKSaveFailedSubState.EAS_EnterGKGKRWalkToSaveFailedMiddleJump.ToString();
                break;
        }
    }

	protected override void InternalBallInState()
	{
		m_kPlayer.IsBallIn = false;
		if (m_combineData != null)
		{
			if (m_combineData.m_ballInIndex > 0 && m_iClipIdx + 1 == m_combineData.m_ballInIndex)
			{
				if (m_playTime >= m_kAniClipList[m_iClipIdx].BallInTime)
				{
					m_kPlayer.IsBallIn = true;
				}
			}
			else {
				m_kInternalState = EInternalState.BallOut;
			}
		}
	}
	protected override void InternalBallOutState()
	{
		m_kPlayer.IsBallOut = false;
		if (m_combineData != null)
		{
			if (m_combineData.m_ballOutIndex > 0 && m_iClipIdx + 1 == m_combineData.m_ballOutIndex)
			{
				if (m_playTime >= m_kAniClipList[m_iClipIdx].BallOutTime)
				{
					m_kPlayer.IsBallOut = true;
				}
			}
			else {
				m_kInternalState = EInternalState.Null;
			}
		}
	}
}

/// <summary>
/// 托出底线成功
/// </summary>
public class NetAniGKSaveSuccessOutBottomState : NetAniGKBaseState
{
    public NetAniGKSaveSuccessOutBottomState() :
        base(EAniState.GK_Save_Out_Success)
    { }

    protected override void OnBegin()
    {
        Vector3D _targetPos = m_kPlayer.KAniData.targetPos;
        //守门员的新的站立位置//
        Vector3D _newPos = new Vector3D(_targetPos.X, m_kPlayer.GetPosition().Y, _targetPos.Z);
        m_kPlayer.SetPosition (_newPos);
        switch (m_kPreState)
        {
            case EAniState.Idle:
                m_AnistateSubName = NetAniGKSaveSuccessSubState.EAS_EnterGKGKNIdleToSaveSuccessOutBottom.ToString();
                break;
            case EAniState.GK_FrontWalk:
                m_AnistateSubName = NetAniGKSaveSuccessSubState.EAS_EnterGKGKFWalkToSaveSuccessOutBottom.ToString();
                break;
            case EAniState.GK_BackWalk:
                m_AnistateSubName = NetAniGKSaveSuccessSubState.EAS_EnterGKGKBWalkToSaveSuccessOutBottom.ToString();
                break;
            case EAniState.GK_LeftWalk:
                m_AnistateSubName = NetAniGKSaveSuccessSubState.EAS_EnterGKGKLWalkToSaveSuccessOutBottom.ToString();
                break;
            case EAniState.GK_RightWalk:
                m_AnistateSubName = NetAniGKSaveSuccessSubState.EAS_EnterGKGKRWalkToSaveSuccessOutBottom.ToString();
                break;
        }

        base.OnBegin();
    }

}
