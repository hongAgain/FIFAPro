using UnityEngine;
using System.Collections;
using Common;
using System;
using Common.Log;


public enum NetAniFootShootSubState
{
    //抢截事件成功//
    EAS_EnterTackleSuccessToFaceNearShootDoor,
    EAS_EnterTackleSuccessToFaceNearShootDoor90,
    EAS_EnterTackleSuccessToFaceNearShootDoor180,
    EAS_EnterTackleSuccessToFaceFarShootDoor,
    EAS_EnterTackleSuccessToFaceFarShootDoor90,
    EAS_EnterTackleSuccessToFaceFarShootDoor180,
    EAS_EnterTackleSuccessToSlant45NearShootDoor,
    EAS_EnterTackleSuccessToSlant45NearShootDoor90,
    EAS_EnterTackleSuccessToSlant45NearShootDoor180,
    EAS_EnterTackleSuccessToSlant45FarShootDoor,
    EAS_EnterTackleSuccessToSlant45FarShootDoor90,
    EAS_EnterTackleSuccessToSlant45FarShootDoor180,
    EAS_EnterTackleSuccessToSlant90NearShootDoor,
    EAS_EnterTackleSuccessToSlant90NearShootDoor90,
    EAS_EnterTackleSuccessToSlant90NearShootDoor180,
    EAS_EnterTackleSuccessToSlant90FarShootDoor,
    EAS_EnterTackleSuccessToSlant90FarShootDoor90,
    EAS_EnterTackleSuccessToSlant90FarShootDoor180,


    //铲断事件成功//
    EAS_EnterSliderTackleToFaceNearShoot,
    EAS_EnterSliderTackleToFaceNearShoot90,
    EAS_EnterSliderTackleToFaceNearShoot180,
    EAS_EnterSliderTackleToFaceFarShoot,
    EAS_EnterSliderTackleToFaceFarShoot90,
    EAS_EnterSliderTackleToFaceFarShoot180,
    EAS_EnterSliderTackleToSlant45NearShootDoor,
    EAS_EnterSliderTackleToSlant45NearShootDoor90,
    EAS_EnterSliderTackleToSlant45NearShootDoor180,
    EAS_EnterSliderTackleToSlant45FarShootDoor,
    EAS_EnterSliderTackleToSlant45FarShootDoor90,
    EAS_EnterSliderTackleToSlant45FarShootDoor180,
    EAS_EnterSliderTackleToSlant90NearShootDoor,
    EAS_EnterSliderTackleToSlant90NearShootDoor90,
    EAS_EnterSliderTackleToSlant90NearShootDoor180,
    EAS_EnterSliderTackleToSlant90FarShootDoor,
    EAS_EnterSliderTackleToSlant90FarShootDoor90,
    EAS_EnterSliderTackleToSlant90FarShootDoor180,


    //阻止突破事件成功//
    EAS_EnterStopBreakOutSuccessToFaceNearShoot,
    EAS_EnterStopBreakOutSuccessToFaceNearShoot90,
    EAS_EnterStopBreakOutSuccessToFaceNearShoot180,
    EAS_EnterStopBreakOutSuccessToFaceFarShoot,
    EAS_EnterStopBreakOutSuccessToFaceFarShoot90,
    EAS_EnterStopBreakOutSuccessToFaceFarShoot180,
    EAS_EnterStopBreakOutSuccessToSlant45NearShootDoor,
    EAS_EnterStopBreakOutSuccessToSlant45NearShootDoor90,
    EAS_EnterStopBreakOutSuccessToSlant45NearShootDoor180,
    EAS_EnterStopBreakOutSuccessToSlant45FarShootDoor,
    EAS_EnterStopBreakOutSuccessToSlant45FarShootDoor90,
    EAS_EnterStopBreakOutSuccessToSlant45FarShootDoor180,
    EAS_EnterStopBreakOutSuccessToSlant90NearShootDoor,
    EAS_EnterStopBreakOutSuccessToSlant90NearShootDoor90,
    EAS_EnterStopBreakOutSuccessToSlant90NearShootDoor180,
    EAS_EnterStopBreakOutSuccessToSlant90FarShootDoor,
    EAS_EnterStopBreakOutSuccessToSlant90FarShootDoor90,
    EAS_EnterStopBreakOutSuccessToSlant90FarShootDoor180,


    //拦截事件成功//
    EAS_EnterInterceptSuccessToFaceNearShoot,
    EAS_EnterInterceptSuccessToFaceNearShoot90,
    EAS_EnterInterceptSuccessToFaceNearShoot180,
    EAS_EnterInterceptSuccessToFaceFarShoot,
    EAS_EnterInterceptSuccessToFaceFarShoot90,
    EAS_EnterInterceptSuccessToFaceFarShoot180,
    EAS_EnterStopInterceptToSlant45NearShootDoor,
    EAS_EnterStopInterceptToSlant45NearShootDoor90,
    EAS_EnterStopInterceptToSlant45NearShootDoor180,
    EAS_EnterStopInterceptToSlant45FarShootDoor,
    EAS_EnterStopInterceptToSlant45FarShootDoor90,
    EAS_EnterStopInterceptToSlant45FarShootDoor180,
    EAS_EnterStopInterceptToSlant90NearShootDoor,
    EAS_EnterStopInterceptToSlant90NearShootDoor90,
    EAS_EnterStopInterceptToSlant90NearShootDoor180,
    EAS_EnterStopInterceptToSlant90FarShootDoor,
    EAS_EnterStopInterceptToSlant90FarShootDoor90,
    EAS_EnterStopInterceptToSlant90FarShootDoor180,


    //常速带球//
    EAS_EnterNormalCarryToFaceNearShootDoor,
    EAS_EnterNormalCarryToFaceFarShootDoor,
    EAS_EnterNormalCarryToFaceNearShootDoor90,
    EAS_EnterNormalCarryToFaceFarShootDoor90,
    EAS_EnterNormalCarryToFaceNearShootDoor180,
    EAS_EnterNormalCarryToFaceFarShootDoor180,
    EAS_EnterNormalCarryToSlant45NearShootDoor,
    EAS_EnterNormalCarryToSlant45FarShootDoor,
    EAS_EnterNormalCarryToSlant45NearShootDoor90,
    EAS_EnterNormalCarryToSlant45FarShootDoor90,
    EAS_EnterNormalCarryToSlant45NearShootDoor180,
    EAS_EnterNormalCarryToSlant45FarShootDoor180,
    EAS_EnterNormalCarryToSlant90NearShootDoor,
    EAS_EnterNormalCarryToSlant90FarShootDoor,
    EAS_EnterNormalCarryToSlant90NearShootDoor90,
    EAS_EnterNormalCarryToSlant90FarShootDoor90,
    EAS_EnterNormalCarryToSlant90NearShootDoor180,
    EAS_EnterNormalCarryToSlant90FarShootDoor180,


    //突破//
    EAS_EnterBreakOutSuccessToFaceNearShootDoor,
    EAS_EnterBreakOutSuccessToFaceFarShootDoor,
    EAS_EnterBreakOutSuccessToFaceNearShootDoor90,
    EAS_EnterBreakOutSuccessToFaceFarShootDoor90,
    EAS_EnterBreakOutToSlant45NearShootDoor,
    EAS_EnterBreakOutToSlant45FarShootDoor,
    EAS_EnterBreakOutToSlant45NearShootDoor90,
    EAS_EnterBreakOutToSlant45FarShootDoor90,
    EAS_EnterBreakOutToSlant90NearShootDoor,
    EAS_EnterBreakOutToSlant90FarShootDoor,
    EAS_EnterBreakOutToSlant90NearShootDoor90,
    EAS_EnterBreakOutToSlant90FarShootDoor90,


    //低速带球//
    EAS_EnterLowCarryToFaceNearShootDoor,
    EAS_EnterLowCarryToFaceFarShootDoor,
    EAS_EnterLowCarryToFaceNearShootDoor90,
    EAS_EnterLowCarryToFaceFarShootDoor90,
    EAS_EnterLowCarryToFaceNearShootDoor180,
    EAS_EnterLowCarryToFaceFarShootDoor180,
    EAS_EnterLowCarryToSlant45NearShootDoor,
    EAS_EnterLowCarryToSlant45FarShootDoor,
    EAS_EnterLowCarryToSlant45NearShootDoor90,
    EAS_EnterLowCarryToSlant45FarShootDoor90,
    EAS_EnterLowCarryToSlant45NearShootDoor180,
    EAS_EnterLowCarryToSlant45FarShootDoor180,
    EAS_EnterLowCarryToSlant90NearShootDoor,
    EAS_EnterLowCarryToSlant90FarShootDoor,
    EAS_EnterLowCarryToSlant90NearShootDoor90,
    EAS_EnterLowCarryToSlant90FarShootDoor90,
    EAS_EnterLowCarryToSlant90NearShootDoor180,
    EAS_EnterLowCarryToSlant90FarShootDoor180,


    //头球攻门//
    EAS_EnterNBIdleToHeadSlantShoot,
    EAS_EnterNBIdleToHeadFaceShoot,
    EAS_EnterNBIdleToHeadFrontSlant45Shoot,
    EAS_EnterNBIdleToHeadBackSlant45Shoot,
    EAS_EnterNBIdleToHeadBackShoot,


    //胸部顶球之后接地面的射门//
    EAS_EnterHeadRobSuccessStopToFaceNearShoot,
    EAS_EnterHeadRobSuccessStopToFaceFarShoot,
    EAS_EnterHeadRobSuccessStopToFaceNearShoot90,
    EAS_EnterHeadRobSuccessStopToFaceFarShoot90,
    EAS_EnterHeadRobSuccessStopToFaceNearShoot180,
    EAS_EnterHeadRobSuccessStopToFaceFarShoot180,
    EAS_EnterHeadRobSuccessStopToSlant45NearShoot,
    EAS_EnterHeadRobSuccessStopToSlant45FarShoot,
    EAS_EnterHeadRobSuccessStopToSlant45NearShoot90,
    EAS_EnterHeadRobSuccessStopToSlant45FarShoot90,
    EAS_EnterHeadRobSuccessStopToSlant45NearShoot180,
    EAS_EnterHeadRobSuccessStopToSlant45FarShoot180,
    EAS_EnterHeadRobSuccessStopToSlant90NearShoot,
    EAS_EnterHeadRobSuccessStopToSlant90FarShoot,
    EAS_EnterHeadRobSuccessStopToSlant90NearShoot90,
    EAS_EnterHeadRobSuccessStopToSlant90FarShoot90,
    EAS_EnterHeadRobSuccessStopToSlant90NearShoot180,
    EAS_EnterHeadRobSuccessStopToSlant90FarShoot180,


    //射门（传球->不停球近射）,接球球员无球快跑赶赴停球点进行立即射门//
    EAS_EnterGetFloorBallNoStopToNearShootDoor,


    //接地面球//
    EAS_EnterGetFloorBallToFaceNearShootDoor,
    EAS_EnterGetFloorBallToFaceFarShootDoor,
    EAS_EnterGetFloorBallToFaceNearShootDoor90,
    EAS_EnterGetFloorBallToFaceFarShootDoor90,
    EAS_EnterGetFloorBallToFaceNearShootDoor180,
    EAS_EnterGetFloorBallToFaceFarShootDoor180,
    EAS_EnterGetFloorBallToSlant45NearShootDoor,
    EAS_EnterGetFloorBallToSlant45FarShootDoor,
    EAS_EnterGetFloorBallToSlant45NearShootDoor90,
    EAS_EnterGetFloorBallToSlant45FarShootDoor90,
    EAS_EnterGetFloorBallToSlant45NearShootDoor180,
    EAS_EnterGetFloorBallToSlant45FarShootDoor180,
    EAS_EnterGetFloorBallToSlant90NearShootDoor,
    EAS_EnterGetFloorBallToSlant90FarShootDoor,
    EAS_EnterGetFloorBallToSlant90NearShootDoor90,
    EAS_EnterGetFloorBallToSlant90FarShootDoor90,
    EAS_EnterGetFloorBallToSlant90NearShootDoor180,
    EAS_EnterGetFloorBallToSlant90FarShootDoor180,


    //躲过抢截事件成功// 
    EAS_EnterStopTackleToFaceNearShoot,
    EAS_EnterStopTackleToFaceFarShoot,
    EAS_EnterStopTackleToFaceNearShoot90,
    EAS_EnterStopTackleToFaceFarShoot90,
    EAS_EnterStopTackleToSlant45NearShootDoor,
    EAS_EnterStopTackleToSlant45FarShootDoor,
    EAS_EnterStopTackleToSlant45NearShootDoor90,
    EAS_EnterStopTackleToSlant45FarShootDoor90,
    EAS_EnterStopTackleToSlant90NearShootDoor,
    EAS_EnterStopTackleToSlant90FarShootDoor,
    EAS_EnterStopTackleToSlant90NearShootDoor90,
    EAS_EnterStopTackleToSlant90FarShootDoor90,


    //躲过铲断事件成功//
    EAS_EnterStopSliderTackleToFaceNearShoot,
    EAS_EnterStopSliderTackleToFaceFarShoot,
    EAS_EnterStopSliderTackleToFaceNearShoot90,
    EAS_EnterStopSliderTackleToFaceFarShoot90,
    EAS_EnterStopSliderTackleToSlant45NearShootDoor,
    EAS_EnterStopSliderTackleToSlant45FarShootDoor,
    EAS_EnterStopSliderTackleToSlant45NearShootDoor90,
    EAS_EnterStopSliderTackleToSlant45FarShootDoor90,
    EAS_EnterStopSliderTackleToSlant90NearShootDoor,
    EAS_EnterStopSliderTackleToSlant90FarShootDoor,
    EAS_EnterStopSliderTackleToSlant90NearShootDoor90,
    EAS_EnterStopSliderTackleToSlant90FarShootDoor90,

	// 事件后 idle//
	EAS_EnterNearShootToCelebrationSuccess,
	EAS_EnterNearShootToFrustrationToIdle,
	EAS_EnterFarShootToFrustrationToIdle,
	EAS_EnterHeadShootToHeadShootIdle,
	EAS_EnterHeadShootToFrustrationToIdle,

    //争顶事件成功,头球射门//
    //表里没有// 
    EAS_EnterHeadRobSuccessToFaceNearShoot,
    EAS_EnterHeadRobSuccessToFaceFarShoot,
    EAS_EnterHeadRobSuccessToFaceNearShoot90,
    EAS_EnterHeadRobSuccessToFaceFarShoot90,
    EAS_EnterHeadRobSuccessToFaceNearShoot180,
    EAS_EnterHeadRobSuccessToFaceFarShoot180,
    EAS_EnterHeadRobSuccessToSlant45NearShoot,
    EAS_EnterHeadRobSuccessToSlant45FarShoot,
    EAS_EnterHeadRobSuccessToSlant45NearShoot90,
    EAS_EnterHeadRobSuccessToSlant45FarShoot90,
    EAS_EnterHeadRobSuccessToSlant45NearShoot180,
    EAS_EnterHeadRobSuccessToSlant45FarShoot180,
    EAS_EnterHeadRobSuccessToSlant90NearShoot,
    EAS_EnterHeadRobSuccessToSlant90FarShoot,
    EAS_EnterHeadRobSuccessToSlant90NearShoot90,
    EAS_EnterHeadRobSuccessToSlant90FarShoot90,
    EAS_EnterHeadRobSuccessToSlant90NearShoot180,
    EAS_EnterHeadRobSuccessToSlant90FarShoot180,
}

/// <summary>
/// 非头球射门之近射
/// </summary>
public class NetAniFootNearShootState : NetAniShootBaseState
{
    public NetAniFootNearShootState() : base(EAniState.Shoot_Near) { }

    private double m_resetAngle = 0d;
    private int m_shootOrientation = 0;// 0: 正射；1: 斜射；2: 90度射门
    
    protected override void OnRotateAngle()
    {
        m_resetAngle = 0d;
        m_shootOrientation = 0;
        double dAngle = MathUtil.GetAngle(m_kPlayer.GetPosition(), m_kPlayer.KAniData.targetPos);    // 动作方向
        double dDeltaAngle = dAngle - m_kPlayer.GetRotAngle();    // 人物方向到动作方向的夹角
        dDeltaAngle %= 360;
        if (dDeltaAngle < 0)
        {
            dDeltaAngle += 360;
        }
		if (dDeltaAngle > 105 && dDeltaAngle < 255) {
            double _Angle = m_kPlayer.GetRotAngle() + 180d;
            _Angle %= 360d;
            m_kPlayer.SetRoteAngle(_Angle);
		}
        dDeltaAngle = dAngle - m_kPlayer.GetRotAngle();  // 人物方向到动作方向的夹角
		dDeltaAngle %= 360;
		if (dDeltaAngle < 0)
		{
			dDeltaAngle += 360;
		}
		
		// 确定是否需要镜像
		if (dDeltaAngle <= 105)
        {
            m_resetAngle = dDeltaAngle;
            m_kMDir = MirrorDirection.Left;
        }
        else if (dDeltaAngle >= 255)
        {
            m_resetAngle = 360 - dDeltaAngle;
            m_kMDir = MirrorDirection.Right;
        }
        else
        {
            LogManager.Instance.Log("不应该出现射门角度不在 -105 ~ 105 之间这种情况");
            return;
        }

        // 确定射门方式
        if (m_resetAngle <= 15)
        {
            m_shootOrientation = 0;
        }
        else if (m_resetAngle < 75)
        {
            m_shootOrientation = 1;
        }
        else if (m_resetAngle <= 105)
        {
            m_shootOrientation = 2;
        }
    }

    protected override void OnBegin()
    {
        switch (m_kPreState)
        {
            case EAniState.Ground_Snatch: // 抢截
                GroundSnatchChange(m_shootOrientation);
                break;
            case EAniState.Ground_Tackle: // 铲断
                GroundTackleChange(m_shootOrientation);
                break;
            case EAniState.Stop_BreakThrough_Success: // 阻止突破成功
                StopBreakOutChange(m_shootOrientation);
                break;
            case EAniState.Ground_Block: // 拦截
                InterceptChange(m_shootOrientation);
                break;
            case EAniState.NormalDribbl: // 常速带球
                NormalDribbleChange(m_shootOrientation);
                break;
            case EAniState.Break_Through: // 突破
                BreakOutSuccessChange(m_shootOrientation);
                break;
            case EAniState.LowDribble: // 低速带球
                LowDribbleChange(m_shootOrientation);
                break;
            case EAniState.HeadRob_Carry:
            case EAniState.HeadRob2_Stop:
                HeadRobSuccessStopChange(m_shootOrientation);
                break;
            case EAniState.NormalRun: // 进行无停球射门
                m_AnistateSubName = NetAniFootShootSubState.EAS_EnterGetFloorBallNoStopToNearShootDoor.ToString();
                break;
            case EAniState.Catch_GroundBall: //接地面球
                FloorBallChange(m_shootOrientation);
                break;
            case EAniState.Stop_Ground_Snatch_Success: // 躲过抢截成功
                StopTackleChange(m_shootOrientation);
                break;
            case EAniState.Stop_Ground_Tackle_Success: // 躲避铲断成功
                StopSliderTackleChange(m_shootOrientation);
                break;
        }
        base.OnBegin();
    }
    private void GroundSnatchChange(int _shootOrientation)
    {
        switch (_shootOrientation)
        {
            case 0:
                m_AnistateSubName = NetAniFootShootSubState.EAS_EnterTackleSuccessToFaceNearShootDoor.ToString();
                break;
            case 1:
                m_AnistateSubName = NetAniFootShootSubState.EAS_EnterTackleSuccessToSlant45NearShootDoor.ToString();
                break;
            case 2:
                m_AnistateSubName = NetAniFootShootSubState.EAS_EnterTackleSuccessToSlant90NearShootDoor.ToString();
                break;
        }
    }

    private void GroundTackleChange(int _shootOrientation)
    {
        switch (_shootOrientation)
        {
            case 0:
                m_AnistateSubName = NetAniFootShootSubState.EAS_EnterSliderTackleToFaceNearShoot.ToString();
                break;
            case 1:
                m_AnistateSubName = NetAniFootShootSubState.EAS_EnterSliderTackleToSlant45NearShootDoor.ToString();
                break;
            case 2:
                m_AnistateSubName = NetAniFootShootSubState.EAS_EnterSliderTackleToSlant90NearShootDoor.ToString();
                break;
        }
    }

    private void StopBreakOutChange(int _shootOrientation)
    {
        switch (_shootOrientation)
        {
            case 0:
                m_AnistateSubName = NetAniFootShootSubState.EAS_EnterStopBreakOutSuccessToFaceNearShoot.ToString();
                break;
            case 1:
                m_AnistateSubName = NetAniFootShootSubState.EAS_EnterStopBreakOutSuccessToSlant45NearShootDoor.ToString();
                break;
            case 2:
                m_AnistateSubName = NetAniFootShootSubState.EAS_EnterStopBreakOutSuccessToSlant90NearShootDoor.ToString();
                break;
        }
    }

    private void InterceptChange(int _shootOrientation)
    {
        switch (_shootOrientation)
        {
            case 0:
                m_AnistateSubName = NetAniFootShootSubState.EAS_EnterInterceptSuccessToFaceNearShoot.ToString();
                break;
            case 1:
                m_AnistateSubName = NetAniFootShootSubState.EAS_EnterStopInterceptToSlant45NearShootDoor.ToString();
                break;
            case 2:
                m_AnistateSubName = NetAniFootShootSubState.EAS_EnterStopInterceptToSlant90NearShootDoor.ToString();
                break;
        }
    }

    private void NormalDribbleChange(int _shootOrientation)
    {
        switch (_shootOrientation)
        {
            case 0:
                m_AnistateSubName = NetAniFootShootSubState.EAS_EnterNormalCarryToFaceNearShootDoor.ToString();
                break;
            case 1:
                m_AnistateSubName = NetAniFootShootSubState.EAS_EnterNormalCarryToSlant45NearShootDoor.ToString();
                break;
            case 2:
                m_AnistateSubName = NetAniFootShootSubState.EAS_EnterNormalCarryToSlant90NearShootDoor.ToString();
                break;
        }
    }

    private void BreakOutSuccessChange(int _shootOrientation)
    {
        switch (_shootOrientation)
        {
            case 0:
                m_AnistateSubName = NetAniFootShootSubState.EAS_EnterBreakOutSuccessToFaceNearShootDoor.ToString();
                break;
            case 1:
                m_AnistateSubName = NetAniFootShootSubState.EAS_EnterBreakOutToSlant45NearShootDoor.ToString();
                break;
            case 2:
                m_AnistateSubName = NetAniFootShootSubState.EAS_EnterBreakOutToSlant90NearShootDoor.ToString();
                break;
        }
    }

    private void LowDribbleChange(int _shootOrientation)
    {
        switch (_shootOrientation)
        {
            case 0:
                m_AnistateSubName = NetAniFootShootSubState.EAS_EnterLowCarryToFaceNearShootDoor.ToString();
                break;
            case 1:
                m_AnistateSubName = NetAniFootShootSubState.EAS_EnterLowCarryToSlant45NearShootDoor.ToString();
                break;
            case 2:
                m_AnistateSubName = NetAniFootShootSubState.EAS_EnterLowCarryToSlant90NearShootDoor.ToString();
                break;
        }
    }

    private void HeadRobSuccessStopChange(int _shootOrientation)
    {
        switch (_shootOrientation)
        {
            case 0:
                m_AnistateSubName = NetAniFootShootSubState.EAS_EnterHeadRobSuccessStopToFaceNearShoot.ToString();
                break;
            case 1:
                m_AnistateSubName = NetAniFootShootSubState.EAS_EnterHeadRobSuccessStopToSlant45NearShoot.ToString();
                break;
            case 2:
                m_AnistateSubName = NetAniFootShootSubState.EAS_EnterHeadRobSuccessStopToSlant90NearShoot.ToString();
                break;
        }
    }


    private void FloorBallChange(int _shootOrientation)
    {
        switch (_shootOrientation)
        {
            case 0:
                m_AnistateSubName = NetAniFootShootSubState.EAS_EnterGetFloorBallToFaceNearShootDoor.ToString();
                break;
            case 1:
                m_AnistateSubName = NetAniFootShootSubState.EAS_EnterGetFloorBallToSlant45NearShootDoor.ToString();
                break;
            case 2:
                m_AnistateSubName = NetAniFootShootSubState.EAS_EnterGetFloorBallToSlant90NearShootDoor.ToString();
                break;
        }
    }

    private void StopTackleChange(int _shootOrientation)
    {
        switch (_shootOrientation)
        {
            case 0:
                m_AnistateSubName = NetAniFootShootSubState.EAS_EnterStopTackleToFaceNearShoot.ToString();
                break;
            case 1:
                m_AnistateSubName = NetAniFootShootSubState.EAS_EnterStopTackleToSlant45NearShootDoor.ToString();
                break;
            case 2:
                m_AnistateSubName = NetAniFootShootSubState.EAS_EnterStopTackleToSlant90NearShootDoor.ToString();
                break;
        }
    }

    private void StopSliderTackleChange(int _shootOrientation)
    {
        switch (_shootOrientation)
        {
            case 0:
                m_AnistateSubName = NetAniFootShootSubState.EAS_EnterStopSliderTackleToFaceNearShoot.ToString();
                break;
            case 1:
                m_AnistateSubName = NetAniFootShootSubState.EAS_EnterStopSliderTackleToSlant45NearShootDoor.ToString();
                break;
            case 2:
                m_AnistateSubName = NetAniFootShootSubState.EAS_EnterStopSliderTackleToSlant90NearShootDoor.ToString();
                break;
        }
    }

}

/// <summary>
/// 非头球射门之远射
/// </summary>
public class NetAniFootFarShootState : NetAniShootBaseState
{
    public NetAniFootFarShootState()
        : base(EAniState.Shoot_Far)
    {

    }

    private double m_resetAngle = 0d;
    private int m_shootOrientation = 0;// 0: 正射；1: 斜射；2: 90度射门

    protected override void OnRotateAngle()
    {
        m_resetAngle = 0d;
        m_shootOrientation = 0;
        double dAngle = MathUtil.GetAngle(m_kPlayer.GetPosition(), m_kPlayer.KAniData.targetPos);    // 动作方向
        double dDeltaAngle = dAngle - m_kPlayer.GetRotAngle();    // 人物方向到动作方向的夹角
		dDeltaAngle %= 360;
		if (dDeltaAngle < 0)
		{
			dDeltaAngle += 360;
		}
		if (dDeltaAngle > 105 && dDeltaAngle < 255) {
            double _Angle = m_kPlayer.GetRotAngle() + 180d;
            _Angle %= 360d;
            m_kPlayer.SetRoteAngle(_Angle);
// 			m_kPlayer.RotateAngle += 180;
// 			m_kPlayer.RotateAngle %= 360;
		}
		dDeltaAngle = dAngle - m_kPlayer.GetRotAngle();    // 人物方向到动作方向的夹角
		dDeltaAngle %= 360;
		if (dDeltaAngle < 0)
		{
			dDeltaAngle += 360;
		}
		
			// 确定是否需要镜像
        if (dDeltaAngle <= 105)
        {
            m_resetAngle = dDeltaAngle;
            m_kMDir = MirrorDirection.Left;
        }
        else if (dDeltaAngle >= 255)
        {
            m_resetAngle = 360 - dDeltaAngle;
            m_kMDir = MirrorDirection.Right;
        }
        else
        {
            LogManager.Instance.Log("不应该出现射门角度不在 -105 ~ 105 之间这种情况");
            return;
        }

        // 确定射门方式
        if (m_resetAngle <= 15)
        {
            m_shootOrientation = 0;
        }
        else if (m_resetAngle < 75)
        {
            m_shootOrientation = 1;
        }
        else if (m_resetAngle <= 105)
        {
            m_shootOrientation = 2;
        }
	}
    protected override void OnBegin()
    {
        switch (m_kPreState)
        {
            case EAniState.Ground_Snatch: // 抢截
                GroundSnatchChange(m_shootOrientation);
                break;
            case EAniState.Ground_Tackle: // 铲断
                GroundTackleChange(m_shootOrientation);
                break;
            case EAniState.Stop_BreakThrough_Success: // 阻止突破成功
                StopBreakOutChange(m_shootOrientation);
                break;
            case EAniState.Ground_Block: // 拦截
                InterceptChange(m_shootOrientation);
                break;
            case EAniState.NormalDribbl: // 常速带球
                NormalDribbleChange(m_shootOrientation);
                break;
            case EAniState.Break_Through: // 突破
                BreakOutSuccessChange(m_shootOrientation);
                break;
            case EAniState.LowDribble: // 低速带球
                LowDribbleChange(m_shootOrientation);
                break;
            case EAniState.HeadRob_Carry:
            case EAniState.HeadRob2_Stop: // 二段争顶胸部停球
                HeadRobSuccessStopChange(m_shootOrientation);
                break;
            case EAniState.NormalRun: // 进行无停球射门
                m_AnistateSubName = NetAniFootShootSubState.EAS_EnterGetFloorBallNoStopToNearShootDoor.ToString();
                break;
            case EAniState.Catch_GroundBall: //接地面球
                FloorBallChange(m_shootOrientation);
                break;
            case EAniState.Stop_Ground_Snatch_Success: // 躲过抢截成功
                StopTackleChange(m_shootOrientation);
                break;
            case EAniState.Stop_Ground_Tackle_Success: // 躲避铲断成功
                StopSliderTackleChange(m_shootOrientation);
                break;
        }
        base.OnBegin();
    }
	
    private void GroundSnatchChange(int _shootOrientation)
    {
        switch (_shootOrientation)
        {
            case 0:
                m_AnistateSubName = NetAniFootShootSubState.EAS_EnterTackleSuccessToFaceFarShootDoor.ToString();
                break;
            case 1:
                m_AnistateSubName = NetAniFootShootSubState.EAS_EnterTackleSuccessToSlant45FarShootDoor.ToString();
                break;
            case 2:
                m_AnistateSubName = NetAniFootShootSubState.EAS_EnterTackleSuccessToSlant90FarShootDoor.ToString();
                break;
        }
    }

    private void GroundTackleChange(int _shootOrientation)
    {
        switch (_shootOrientation)
        {
            case 0:
                m_AnistateSubName = NetAniFootShootSubState.EAS_EnterSliderTackleToFaceFarShoot.ToString();
                break;
            case 1:
                m_AnistateSubName = NetAniFootShootSubState.EAS_EnterSliderTackleToSlant45FarShootDoor.ToString();
                break;
            case 2:
                m_AnistateSubName = NetAniFootShootSubState.EAS_EnterSliderTackleToSlant90FarShootDoor.ToString();
                break;
        }
    }

    private void StopBreakOutChange(int _shootOrientation)
    {
        switch (_shootOrientation)
        {
            case 0:
                m_AnistateSubName = NetAniFootShootSubState.EAS_EnterStopBreakOutSuccessToFaceFarShoot.ToString();
                break;
            case 1:
                m_AnistateSubName = NetAniFootShootSubState.EAS_EnterStopBreakOutSuccessToSlant45FarShootDoor.ToString();
                break;
            case 2:
                m_AnistateSubName = NetAniFootShootSubState.EAS_EnterStopBreakOutSuccessToSlant90FarShootDoor.ToString();
                break;
        }
    }

    private void InterceptChange(int _shootOrientation)
    {
        switch (_shootOrientation)
        {
            case 0:
                m_AnistateSubName = NetAniFootShootSubState.EAS_EnterInterceptSuccessToFaceFarShoot.ToString();
                break;
            case 1:
                m_AnistateSubName = NetAniFootShootSubState.EAS_EnterStopInterceptToSlant45FarShootDoor.ToString();
                break;
            case 2:
                m_AnistateSubName = NetAniFootShootSubState.EAS_EnterStopInterceptToSlant90FarShootDoor.ToString();
                break;
        }
    }

    private void NormalDribbleChange(int _shootOrientation)
    {
        switch (_shootOrientation)
        {
            case 0:
                m_AnistateSubName = NetAniFootShootSubState.EAS_EnterNormalCarryToFaceFarShootDoor.ToString();
                break;
            case 1:
                m_AnistateSubName = NetAniFootShootSubState.EAS_EnterNormalCarryToSlant45FarShootDoor.ToString();
                break;
            case 2:
                m_AnistateSubName = NetAniFootShootSubState.EAS_EnterNormalCarryToSlant90FarShootDoor.ToString();
                break;
        }
    }

    private void BreakOutSuccessChange(int _shootOrientation)
    {
        switch (_shootOrientation)
        {
            case 0:
                m_AnistateSubName = NetAniFootShootSubState.EAS_EnterBreakOutSuccessToFaceFarShootDoor.ToString();
                break;
            case 1:
                m_AnistateSubName = NetAniFootShootSubState.EAS_EnterBreakOutToSlant45FarShootDoor.ToString();
                break;
            case 2:
                m_AnistateSubName = NetAniFootShootSubState.EAS_EnterBreakOutToSlant90FarShootDoor.ToString();
                break;
        }
    }

    private void LowDribbleChange(int _shootOrientation)
    {
        switch (_shootOrientation)
        {
            case 0:
                m_AnistateSubName = NetAniFootShootSubState.EAS_EnterLowCarryToFaceFarShootDoor.ToString();
                break;
            case 1:
                m_AnistateSubName = NetAniFootShootSubState.EAS_EnterLowCarryToSlant45FarShootDoor.ToString();
                break;
            case 2:
                m_AnistateSubName = NetAniFootShootSubState.EAS_EnterLowCarryToSlant90FarShootDoor.ToString();
                break;
        }
    }

    private void HeadRobSuccessStopChange(int _shootOrientation)
    {
        switch (_shootOrientation)
        {
            case 0:
                m_AnistateSubName = NetAniFootShootSubState.EAS_EnterHeadRobSuccessStopToFaceFarShoot.ToString();
                break;
            case 1:
                m_AnistateSubName = NetAniFootShootSubState.EAS_EnterHeadRobSuccessStopToSlant45FarShoot.ToString();
                break;
            case 2:
                m_AnistateSubName = NetAniFootShootSubState.EAS_EnterHeadRobSuccessStopToSlant90FarShoot.ToString();
                break;
        }
    }


    private void FloorBallChange(int _shootOrientation)
    {
        switch (_shootOrientation)
        {
            case 0:
                m_AnistateSubName = NetAniFootShootSubState.EAS_EnterGetFloorBallToFaceFarShootDoor.ToString();
                break;
            case 1:
                m_AnistateSubName = NetAniFootShootSubState.EAS_EnterGetFloorBallToSlant45FarShootDoor.ToString();
                break;
            case 2:
                m_AnistateSubName = NetAniFootShootSubState.EAS_EnterGetFloorBallToSlant90FarShootDoor.ToString();
                break;
        }
    }

    private void StopTackleChange(int _shootOrientation)
    {
        switch (_shootOrientation)
        {
            case 0:
                m_AnistateSubName = NetAniFootShootSubState.EAS_EnterStopTackleToFaceFarShoot.ToString();
                break;
            case 1:
                m_AnistateSubName = NetAniFootShootSubState.EAS_EnterStopTackleToSlant45FarShootDoor.ToString();
                break;
            case 2:
                m_AnistateSubName = NetAniFootShootSubState.EAS_EnterStopTackleToSlant90FarShootDoor.ToString();
                break;
        }
    }

    private void StopSliderTackleChange(int _shootOrientation)
    {
        switch (_shootOrientation)
        {
            case 0:
                m_AnistateSubName = NetAniFootShootSubState.EAS_EnterStopSliderTackleToFaceFarShoot.ToString();
                break;
            case 1:
                m_AnistateSubName = NetAniFootShootSubState.EAS_EnterStopSliderTackleToSlant45FarShootDoor.ToString();
                break;
            case 2:
                m_AnistateSubName = NetAniFootShootSubState.EAS_EnterStopSliderTackleToSlant90FarShootDoor.ToString();
                break;
        }
    }
}

/// <summary>
/// 头球射门
/// </summary>
public class NetAniHeadShootState : NetAniShootBaseState
{
    public NetAniHeadShootState() : base(EAniState.HeadRob_Shoot) { }


    private double m_resetAngle = 0d;
    private int m_shootOrientation = 0; // 0: 正射；1: 前45度；2: 90度；3: 后45度；4: 后蹭
    protected override void OnRotateAngle()
    {
        m_shootOrientation = 0; // 0: 正射；1: 前45度；2: 90度；3: 后45度；4: 后蹭
        Vector3D ballInPos = m_kPlayer.KAniData.targetPos;
        Vector3D ballOutPos = m_kPlayer.KAniData.targetBallOutPos;
        double ballInAngle = MathUtil.GetAngle(m_kPlayer.GetPosition(), ballInPos);    // 人的位置到球的位置的向量的角度
        double ballOutAngle = MathUtil.GetAngle(m_kPlayer.GetPosition(), ballOutPos);    // 人的位置到出球位置的向量的角度

        double dBallInDeltaAngle = ballInAngle - m_kPlayer.GetRotAngle(); // 球的方向相对人物方向方向的夹角
        dBallInDeltaAngle %= 360;
        if (dBallInDeltaAngle < 0)
        {
            dBallInDeltaAngle += 360;
        }

        // 确定是否需要镜像和转180度
        if (dBallInDeltaAngle <= 90)
        {
            m_kMDir = MirrorDirection.Right;
        }
        else if (dBallInDeltaAngle <= 180)
        {
            double _Angle = m_kPlayer.GetRotAngle() + 180d;
            _Angle %= 360d;
            m_kPlayer.SetRoteAngle(_Angle);
            if (dBallInDeltaAngle < 0)
            {
                dBallInDeltaAngle += 360;
            }
            m_kMDir = MirrorDirection.Left;
        }
        else if (dBallInDeltaAngle <= 270)
        {
            double _Angle = m_kPlayer.GetRotAngle() + 180d;
            _Angle %= 360d;
            m_kPlayer.SetRoteAngle(_Angle);
            if (dBallInDeltaAngle < 0)
            {
                dBallInDeltaAngle += 360;
            }
            m_kMDir = MirrorDirection.Right;
        }
        else
        { // 270 ~ 360
            m_kMDir = MirrorDirection.Left;
        }

        double dBallOutDeltaAngle = ballOutAngle - m_kPlayer.GetRotAngle(); // 出球的方向相对人物方向方向的夹角
        dBallOutDeltaAngle %= 360;
        if (dBallOutDeltaAngle < 0)
        {
            dBallOutDeltaAngle += 360;
        }

        double dBallOutDeltaAngleAbs = Math.Abs(dBallOutDeltaAngle - 180);
        dBallOutDeltaAngleAbs = 180 - dBallOutDeltaAngleAbs;

        if (dBallOutDeltaAngleAbs <= 15)
        {
            m_shootOrientation = 0;
        }
        else if (dBallOutDeltaAngleAbs <= 75)
        {
            m_shootOrientation = 1;
        }
        else if (dBallOutDeltaAngleAbs <= 105)
        {
            m_shootOrientation = 2;
        }
        else if (dBallOutDeltaAngleAbs <= 165)
        {
            m_shootOrientation = 3;
        }
        else
        { // 165 ~ 180
            m_shootOrientation = 4;
        }
    }

    protected override void OnBegin()
    {

        switch (m_kPreState)
        {
            case EAniState.Idle:
                IdleChange(m_shootOrientation);
                break;
        }
        base.OnBegin();
    }

    private void IdleChange(int _shootOrientation)
    {
        switch (_shootOrientation)
        {
            case 0:
                m_AnistateSubName = NetAniFootShootSubState.EAS_EnterNBIdleToHeadFaceShoot.ToString();
                break;
            case 1:
                m_AnistateSubName = NetAniFootShootSubState.EAS_EnterNBIdleToHeadFrontSlant45Shoot.ToString();
                break;
            case 2:
                m_AnistateSubName = NetAniFootShootSubState.EAS_EnterNBIdleToHeadSlantShoot.ToString();
                break;
            case 3:
                m_AnistateSubName = NetAniFootShootSubState.EAS_EnterNBIdleToHeadBackSlant45Shoot.ToString();
                break;
            case 4:
                m_AnistateSubName = NetAniFootShootSubState.EAS_EnterNBIdleToHeadBackShoot.ToString();
                break;
        }
    }
}

/// <summary>
/// 近射成功
/// </summary>
public class NetAniFootNearShootSuccessState : AniBaseState
{
	public NetAniFootNearShootSuccessState() : base(EAniState.Shoot_Near_Success) { }
	
	public override void OnEnter()
	{
		base.OnEnter();
		m_bAniFinish = false;
	}
	
	protected override void OnBegin()
	{
		switch (m_kPreState)
		{
		case EAniState.Shoot_Near:
			m_AnistateSubName = NetAniFootShootSubState.EAS_EnterNearShootToCelebrationSuccess.ToString();
			break;
		}
		base.OnBegin();
	}
}

/// <summary>
/// 远射成功
/// </summary>
public class NetAniFootFarShootSuccessState : AniBaseState
{
	public NetAniFootFarShootSuccessState() : base(EAniState.Shoot_Far_Success) { }
	
	public override void OnEnter()
	{
		base.OnEnter();
		m_bAniFinish = false;
	}
	
	protected override void OnBegin()
	{
		switch (m_kPreState)
		{
		case EAniState.Shoot_Far:
			// 没见到远射成功庆祝动画，用了近射的 //
			m_AnistateSubName = NetAniFootShootSubState.EAS_EnterNearShootToCelebrationSuccess.ToString();
			break;
		}
		base.OnBegin();
	}
}

/// <summary>
/// 头球射门成功
/// </summary>
public class NetAniHeadShootSuccessState : AniBaseState
{
	public NetAniHeadShootSuccessState() : base(EAniState.HeadRob_Shoot_Success) { }
	
	public override void OnEnter()
	{
		base.OnEnter();
		m_bAniFinish = false;
	}
	
	protected override void OnBegin()
	{
		switch (m_kPreState)
		{
		case EAniState.HeadRob_Shoot:
			m_AnistateSubName = NetAniFootShootSubState.EAS_EnterHeadShootToHeadShootIdle.ToString();
			break;
		}
		base.OnBegin();
	}
}

/// <summary>
/// 近射失败
/// </summary>
public class NetAniFootNearShootFailedState : NetAniFailedBaseState
{
    public NetAniFootNearShootFailedState() : 
        base(EAniState.Shoot_Near_Failed) 
    { 
    }

    public override void OnEnter()
    {
        base.OnEnter();
        m_bAniFinish = false;
    }

}

/// <summary>
/// 远射失败
/// </summary>
public class NetAniFootFarShootFailedState : NetAniFailedBaseState
{
    public NetAniFootFarShootFailedState() :
        base(EAniState.Shoot_Far_Failed)
    {
    }

}

/// <summary>
/// 头球射门失败
/// </summary>
public class NetAniHeadShootFailedState : NetAniFailedBaseState
{
    public NetAniHeadShootFailedState() : 
        base(EAniState.HeadRob_Shoot_Failed) 
    {
    }

}
