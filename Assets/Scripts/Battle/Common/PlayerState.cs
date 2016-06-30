namespace Common
{
    public enum EPlayerState            // 球员一级状态
    {
        Default = 0,                    // 默认状态
        PS_WAIT_IDLE,                   // 原地待机
        PS_WAITFORKICKOFF,              // 等待开球
        PS_KICKOFF,                     // 开球
//        PS_WAITING_FOR_BALL_ARRIVED,    // 控球球员处于等球到脚下的状态
   //   PassBall,                       // 停球对人传球
//        PS_AFTERPASSBALL,               // 跟随状态
//        PS_STOPPING_THINKING,           // 停球思考
   //     Shoot,              // 停球射门
  //    NormalDribble,                  // 带球
//        PS_SEARCHING_BALL,              // 失球后，寻球
//        PS_CLOSEMARKING_ENTERING,       // 进入盯防状态
//        PS_CLOSEMARK_WITH_BALL,         // 有球盯防
//        PS_CLOSEMARK_WITHOUT_BALL,      // 无球盯防
//        PS_INTERCEPT_BALL,              // 截球
//        PS_INTERCEPTED_THINKING,        // 被抢球后思考状态
//        PS_FOLLOWING_TARGET,            // 跟随状态
        
        PassBallIdle,                   // 传球后待机




        // 新状态
        ActionSelect,                   // 行为选择状态
        Idle,                           // 待机 作用:射门后所有球员都要原地待机
		IdleAfterFail,
        NormalDribble,                  // 常速带球
        LowDribble,                     // 低速带球
        PassBall,                       // 传球
        Shoot,                          // 射门
        Shoot_Head,                     // 头球射门
        Shoot_Idle,                     // 射门后idle
        Fill_Gap,                       // 接应
        HomePos,                        // HomePosition跑位 

        Catch_GroundBall,               // 接地面球
        Catch_HighBall,                 // 接高空球,防守方永远只有这一种争高球,因为一旦抢到球，就变为进攻方
        Catch_SecondHighBall,           // 接高空摆渡球,针对进攻方
        
//        Catch_GroundBall_ToIdle,        // 接地面球并停球
//        Catch_GroundBall_ToPass,
//        Catch_GroundBall_ToShoot,
//        Catch_GroundBall_ToDribble,

        Heading_Tackle_ToPass,          //高空争顶成功,并头球传球
        Heading_Tackle_ToShoot,         //高空争顶成功,并头球射门
        Heading_Tackle_ToDribble,       //高空争顶成功,并转移到脚下
        Heading_Tackle_ChestToDribble,  //高空争顶成功,并转移到脚下
        Heading_Tackle_Failed,          //高空争顶失败,

        //进攻方 进攻动作
        Break_Through_Success,          // 突破成功,针对进攻方
        Break_Through_Failed,           // 突破失败,针对原带球球员
        Avoid_Sliding_Tackle_Success,   // 躲过铲断成功 针对进攻方
        Avoid_Sliding_Tackle_Failed,    // 躲过铲断失败 针对进攻方
        Avoid_Block_Tackle_Success,     // 躲过抢截成功 针对进攻方
        Avoid_Block_Tackle_Failed,      // 躲过抢截失败 针对进攻方

        //防守方 防守动作
        Defend_Break_Through,           // 阻止突破
        Defend_Break_Through_Success,   // 阻止突破成功
        Defend_Break_Through_Failed,    // 阻止突破失败
        Sliding_Tackle,                 // 铲断
        Sliding_Tackle_Failed,          // 铲断失败 针对防守方
        Sliding_Tackle_Success,         // 铲断成功,功能待定,针对进攻方的动画，做的是进攻方被 铲断后的动画
        Intercept,                      // 拦截
        Intercept_Failed,               // 拦截失败 针对防守方
        Intercept_Success,              // 拦截成功,功能待定,针对进攻方的动画，做的是进攻方被拦截后的动画
        Block_Tackle,                   // 抢截
        Block_Tackle_Failed,            // 抢截失败 针对防守方
        Block_Tackle_Success,           // 抢截成功,功能待定,针对进攻方的动画，做的是进攻方被抢截后的动画

        ToAttackSupport,                // 跑向进攻接应位置
        AttackSupport,                  // 进攻接应
		
		Match_ReadyIdle,				// 中场开球（普遍待机）
		Match_BeginKick,				// 中场开球（开球）
		Match_ReadyKick,				// 中场开球（开球待机）
        
		CloseMark_WithBall_GetCloser,	// 靠近有球盯防目标
		CloseMark_WithBall_NotActivated,// 有球盯防未触发

        ToCloseMark_WithoutBall,        // 无球盯防准备状态
        CloseMark_WithoutBall,          // 无球盯防动作

        PS_END
    }

    public enum EGKState
    {
        GS_DEFAULT = 0,                 // 默认状态
        GS_GUARD,                       // 警戒状态
        GS_SAVE,                        // 扑球
        GS_KICKOFF,                     // 开球
    }
}