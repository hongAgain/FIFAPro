using System;
using System.Collections.Generic;

namespace Common
{
    public enum ETeamColor
    {
        Team_Red = 0,
        Team_Blue,
        Team_End
    }

    // 球队攻防状态
    public enum ETeamState
    {
        TS_ATTACK = 0,
        TS_DEFEND
    }


    // 球队状态切换的原因类型
    public enum ETeamStateChangeType
    {
        TSCT_SHOOT = 0,     // 射门
        TSCT_INTERCEPT,     // 抢断
        TSCT_DEBUG,
        TSCT_MAXCOUNT
    }


    public enum ECtrlBallStyle      // 控球风格
    {
        CBS_ATTACK = 1,    // 偏进攻
        CBS_DEFENCE,       // 偏防守
        CBS_BALANCE,       // 偏平衡                               
        CBS_MAX
    }
    public enum EAttackStyle        // 进攻风格
    {
        AS_LEFT = 4,                // 偏左路
        AS_RIGHT,                   // 偏右路
        AS_MIDDLE,                  // 偏中路
        AS_BALANCE,                 // 平衡型
        AS_MAX
    }

    //marking status for players
    public enum EMarkStatus
    {
        NONE,
        MARKWITHBALL,
        MARKWITHOUTBALL
    }

    public enum ETeamStateChangeDelayedType
    {
        NONE,
        DELAYED_DEFENCE,               //球队状态由守转攻,球员防守动作尚未完成,延迟进入进攻状态
        DELAYED_ATTACK               //球队状态由攻转守,球员进攻动作尚未完成,延迟进入防守状态
    }

    public enum EAniState       // 动画状态机
    {
        EAS_NULL = 0,

        Kick_Idle,                       //开球Idle
        Match_ReadyIdle,                 //赛前准备
        Match_BeginKick,                 //中场开球
        Match_ReadyKick,                 //赛前开球

        Idle,                            // 待机,无球IDLE
        Special_Idle,                    // 特殊的Idle状态，例如射门后，抢断等等
        Special_Success,                 // 特殊事件成功
        NormalDribbl,                    // 常速带球
        LowDribble,                      // 低速带球
        PassBall_Floor,                  // 传地面球球
        PassBall_High,                   // 传高空球
        HeadRob_Shoot,                   // 头球射门，射门动作状态
        Shoot_Near,                      // 近射，射门动作状态
        Shoot_Far,                       // 远射，射门动作状态
        Shoot_Far_Failed,                // 远射失败
        Shoot_Near_Failed,               // 近射失败
        HeadRob_Shoot_Failed,            // 头球射门失败
        Shoot_Far_Success,               // 远射成功
        Shoot_Near_Success,              // 近射成功
        HeadRob_Shoot_Success,           // 头球射门成功
        Mark_Ball,                       // 有球盯防,后退移动
        Mark,                            // 无球盯防
        NormalRun,                       // HomePosition常速跑位 
        Walk,                            // HomePosition低速跑位 
        Catch_GroundBall,                // 接地面球
        Break_Through,                   // 突破,进攻方持球球员突破成功，进入带传射判断
        Break_Through_Failed,            // 突破失败, 针对原带球球员 ，攻防转换，原防守方进入带传射判断
        Stop_BreakThrough_Failed,        // 阻止突破失败, 
        Stop_BreakThrough_Success,       // 阻止突破成功，
        HeadRob_Pass,                    // 一段争顶成功头球摆渡传球
        HeadRob_Carry,                   // 胸部停球带球
        HeadRob2_Stop,                   // 二段争顶胸部停球        
        Head_Tackle_Failed,              // 抢断高空争顶失败
        Ground_Tackle,                   // 铲断
        Ground_Tackle_Failed,            // 铲断失败 针对防守球员  进入Idle
        Stop_Ground_Tackle_Success,      // 躲避铲断成功
        Stop_Ground_Tackle_Failed,       // 躲避铲断失败
        Ground_Snatch,                   // 抢截
        Stop_Ground_Snatch_Success,      // 躲过抢截成功
        Stop_Ground_Snatch_Failed,       // 躲过抢截失败,攻防转换
        Ground_Snatch_Failed,            // 抢截失败, 防守球员进入Idle
        Ground_Block,                    // 拦截
        Ground_Block_Failed,             // 拦截失败 防守球员进入Idle
        GK_Save_Success,                 // 门将横向、侧上方，中路蹲下，跳起扑救
        GK_Save_Out_Success,             // 门将跳起托出底线
        GK_Save_Failed,                  // 门将扑救失败
        GK_HorizontalLeftSaveSuccess,    // 门将左向扑救成功-横向扑救
        GK_HorizontalRightSaveSuccess,   // 门将右向扑救成功-横向扑救
        GK_SlideLeftSaveSuccess,         // 门将左向扑救成功-侧上方扑救
        GK_SlideRightSaveSuccess,        // 门将右向扑救成功-侧上方扑救
        GK_SaveSuccessMiddleSquate,      // 门将扑救成功-中路蹲下
        GK_SaveSuccessMiddleJump,        // 门将扑救成功-中路跳起
        GK_SaveSuccessOutBottom,         // 门将扑救成功-托出底线
        Gk_HorizontalSaveFailed,         // 门将扑救失败-横向扑救
        GK_SlideSaveFailed,              // 门将扑救失败-侧上方扑救
        GK_SaveFailedMiddleSquate,       // 门将扑救失败-中路蹲下
        GK_SaveFailedMiddleJump,         // 门将扑救失败-中路跳起
        GK_Move,                         // 门将移动
        GK_FrontWalk,                    // 门将前移
        GK_BackWalk,                     // 门将后退
        GK_LeftWalk,                     // 门将左移
        GK_RightWalk,                    // 门将右移
        GK_ThrowBall,                    // 门将手抛球
        GK_KickBall,                     // 门将大脚开球
        GK_BigKickBall,                  // 摆球大脚开球
        GoalCeleBration,                 // 进球后的全队欢声雀跃
        GoalSad,                         // 进球后防守方的士气低落
        Max
    }

    public enum EBallMoveType
    {
        NULL,
        Shooting,
        Throughing,
        GroundPass,
        HighLobPass,
        HeadingToHighLob,
        HeadingToGround,
        HeadingToShoot
    }

    public enum EBallState
    {
        EBS_Wait = 0,
        EBS_Running,
        EBS_Max
    }

    /// <summary>
    /// 球员球队位置
    /// </summary>
    public enum ECareer
    {
        ForwardFielder = 0,
        MidFielder,
        BackFielder,
        Goalkeeper,
    }

    public enum ETacticsBallType
    {
        Attack_Depth = 1,       //纵深
        Stage_Direction = 2,    //战略
    }

    public enum AttackType
    {
        All_Attack = 1,     //全力进攻
        ALL_Define,         //稳固防守
        Attack_Define,      //攻守兼备
    }

    public enum AttackDirection
    {
        Side_Middle = 1,        //边中结合
        Middle,                 //中路进攻
        Left,                  //左路进攻
        Right,                 //右路进攻
    }

    public enum AttackChoice
    {
        Long_Short_Combine = 1 , //长短结合
        Long_Pass,          //长传冲吊
        Short_In,           //短传渗透
    }
}
