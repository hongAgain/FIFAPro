using System;

namespace Common
{
    public enum MessageType
    {
        CreateScene = 0,
        DestroyScene,
        CreateTeam,
        DestroyTeam,
        CreatePlayer,
        RemovePlayer,
        CreateBall,
        DestroyBall,
        CameraAniEnable,
        CreateBattleDesc,
        DestroyBattleDesc,
        CreateGoalAni,
        DestroyGoalAni,
        ChangePlayerState,
        ChangeBallPosState,
        MessageSelectPlayerState,
        MessageSelectEtZoneDraw,
        MessageHomePositionUpdate,
        ExitBattle,
        PauseBattle,
        ResumeBattle,
        CtrlBallRate,
        PlayAnimation,
        SkillBegin,
        SkillEnd,
        BaseFxBegin,
        BaseFxEnd,
        FrameFrozenBegin,
        FrameFrozenEnd,
        CameraFxBegin,
        CameraFxEnd,
        GhostFxBegin,
        GhostFxEnd,
        GenValidData,
        BallVisable,
        BattleText,         // 战斗解说
        ChangeAniAngle,     // 修改动画旋转角度 
        GoalCelebration,    // 进球胜利动画
        DestoryCeleBration, // 关闭删除胜利动画场景
        HideBattleUI,       // 隐藏战斗UI
        ShowBattleUI,       // 显示战斗UI
        ShowInGoalUI,       // 进球UI
        HideInGoalUI,       // 隐藏进球UI
        ShowPerBattleUI,    // 显示战前UI
        DestroyPerBattleUI, // 隐藏战前UI
        MessageTypeCount
    }


    public abstract class Message
    {
        public MessageType Type { get; private set; }

        public Message(MessageType type)
        {
            Type = type;
        }
    }


}
