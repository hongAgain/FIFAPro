using Common;
using System.Collections.Generic;

public class CreateSceneMessage : Message
{
    public CreateSceneMessage(string strSceneName,LLScene kScene)
        : base(MessageType.CreateScene)
    {
        m_strSceneName = strSceneName;
        m_kScene = kScene;
    }

    public string SceneName
    {
        get { return m_strSceneName; }
        
    }
    public LLScene Scene
    {
        get { return m_kScene; }
        
    }
    private string m_strSceneName;
    private LLScene m_kScene;
}

public class DestroySceneMessage : Message
{
    public DestroySceneMessage(string strSceneName)
        : base(MessageType.DestroyScene)
    {
        m_strSceneName = strSceneName;
    }

    public string SceneName
    {
        get { return m_strSceneName; }
        
    }
    private string m_strSceneName;
}

public class CreateTeamMessage : Message
{
    public CreateTeamMessage(LLTeam kTeam)
        : base(MessageType.CreateTeam)
    {
        m_kTeam = kTeam;
    }

    public LLTeam Team
    {
        get { return m_kTeam; }
        
    }
    private LLTeam m_kTeam;
}

public class DestroyTeamMessage : Message
{
    public DestroyTeamMessage(LLTeam kTeam)
        : base(MessageType.DestroyTeam)
    {
        m_kTeam = kTeam;
    }

    public LLTeam Team
    {
        get { return m_kTeam; }
        
    }
    private LLTeam m_kTeam;
}

public class CreateBalllMessage : Message
{
    public CreateBalllMessage(LLBall kBall,LLScene kScene)
        : base(MessageType.CreateBall)
    {
        m_kBall = kBall;
        m_kScene = kScene;
    }
    public LLBall Ball
    {
        get { return m_kBall; }
        
    }

    public LLScene Scene
    {
        get { return m_kScene; }
        
    }
    private LLBall m_kBall;
    private LLScene m_kScene;
}
public class DestroyBallMessage : Message
{
    public DestroyBallMessage(LLBall kBall)
        : base(MessageType.DestroyBall)
    {
        m_kBall = kBall;
    }
    public LLBall Ball
    {
        get { return m_kBall; }
        
    }
    private LLBall m_kBall;
}

public class CameraAniEnableMessage : Message
{
    public CameraAniEnableMessage()
        :base(MessageType.CameraAniEnable)
    {

    }
}

public class CreateBattleDescMessage : Message
{
    public CreateBattleDescMessage(string strDesc)
        :base(MessageType.CreateBattleDesc)
    {
        m_strDesc = strDesc;
    }

    public string Desc
    {
        get { return m_strDesc; }
    }

    private string m_strDesc;
}

public class DestroyBattleDescMessage : Message
{
    public DestroyBattleDescMessage()
        : base(MessageType.DestroyBattleDesc)
    {
    }
}


public class CreateGoalAniMessage : Message
{
    public CreateGoalAniMessage()
        : base(MessageType.CreateGoalAni)
    {
    }
}

public class DestroyGoalAniMessage : Message
{
    public DestroyGoalAniMessage()
        : base(MessageType.DestroyGoalAni)
    {
    }
}
public class ChangeBallPosMessage : Message
{
    public ChangeBallPosMessage(Vector3D kPos,LLBall kBall)
        : base(MessageType.ChangeBallPosState)
    {
        m_kPos = kPos;
        m_kBall = kBall;
    }

    public LLBall Ball
    {
        get { return m_kBall; }
        
    }

    public Vector3D Pos
    {
        get { return m_kPos; }
        
    }

    private LLBall m_kBall;
    private Vector3D m_kPos;
}

public class RefreshHomePositionMessage:Message
{
    private LLScene m_LScene;
    public LLScene GetScene
    {
        get
        {
            return m_LScene;
        }
        
    }
    public RefreshHomePositionMessage(LLScene _scene)
        : base(MessageType.MessageHomePositionUpdate)
    {
        m_LScene = _scene;
    }
}

public class ExitBattleMessage : Message
{
    public ExitBattleMessage()
        :base (MessageType.ExitBattle)
    { }
}

public class PauseBattleMessage : Message
{
    public PauseBattleMessage():
        base(MessageType.PauseBattle)
    {}
}

public class ResumeBattleMessage : Message
{
    public ResumeBattleMessage():
        base(MessageType.ResumeBattle)
    {}
}

public class CtrlBallRateMessage : Message
{
    public CtrlBallRateMessage(float fRedTime,float fBlueTime):
        base(MessageType.CtrlBallRate)
    {
        m_fBlueTime = fBlueTime;
        m_fRedTime = fRedTime;
    }

    public float BlueTime
    {
        get { return m_fBlueTime; }
        set { m_fBlueTime = value; }
    }

    public float RedTime
    {
        get { return m_fRedTime; }
        set { m_fRedTime = value; } 
    }

    private float m_fBlueTime = 0;
    private float m_fRedTime = 0;
}

public class PlayAniMessage : Message
{
    public PlayAniMessage(LLUnit kUnit,AniClipData kClipData):
        base(MessageType.PlayAnimation)
    {
        m_kUnit = kUnit;
        m_kClipData = kClipData;
    }
    public LLUnit Player
    {
        get { return m_kUnit; }
        set { m_kUnit = value; }
    }

    public AniClipData AniClipData
    {
        get { return m_kClipData; }
        set { m_kClipData = value; }
    }
    private LLUnit m_kUnit = null;
    private AniClipData m_kClipData = null;
}

//public class GenValidDataMessage : Message
//{
//    public GenValidDataMessage(int iActID,int iHeroID,List<int> kNpcIDList,int iScore,int iNpcScore):
//        base(MessageType.GenValidData)
//    {
//        m_iActionID = iActID;
//        m_iHeroID = iHeroID;
//        m_kNpcIDList = kNpcIDList;
//        m_iScore = iScore;
//        m_iNpcScore = iNpcScore;
//    }

//    public int HeroID
//    {
//        get { return m_iHeroID; }
//        set { m_iHeroID = value; }
//    }

//    public List<int> NpcIDList
//    {
//        get { return m_kNpcIDList; }
//        set { m_kNpcIDList = value; }
//    }

//    public int ActionID
//    {
//        get { return m_iActionID; }
//        set { m_iActionID = value; }
//    }

//    public int Score
//    {
//        get { return m_iScore; }
//        set { m_iScore = value; }
//    }

//    public int NpcScore
//    {
//        get { return m_iNpcScore; }
//        set { m_iNpcScore = value; }
//    }
//    private int m_iActionID;
//    private int m_iHeroID;
//    private List<int> m_kNpcIDList = new List<int>();
//    private int m_iScore;           // 玩家得分
//    private int m_iNpcScore;        // NPC得分
//}

public class BallVisableMeassage : Message
{
    /// <summary>
    /// 控制动画球和场景球的显示和隐藏
    /// </summary>
    /// <param name="_uint"></param>
    /// <param name="_visable"></param>
    public BallVisableMeassage(LLUnit _uint,bool _visable)
        :base(MessageType.BallVisable)
    {
        m_kUint = _uint;
        m_bVisable = _visable;
    }

    public LLUnit Kuint
    {
        set { m_kUint = value; }
        get { return m_kUint; }
    }

    public bool Visable
    {
        set { m_bVisable = true; }
        get { return m_bVisable; }
    }
    private LLUnit m_kUint;
    private bool m_bVisable;
}

public class AniAngleChangeMeassage : Message
{
    public AniAngleChangeMeassage(LLUnit kUnit,float fAngle)
        :base(MessageType.ChangeAniAngle)
    {
     
        m_kUint = kUnit;
        m_fAngle = fAngle;
    }

    public LLUnit Unit
    {
        get { return m_kUint; }
    }

    public float Angle
    {
        get { return m_fAngle; }
    }

    private LLUnit m_kUint;
    private float m_fAngle;

}


public class ShowBattleUIMessage : Message
{
    public ShowBattleUIMessage()
        : base(MessageType.ShowBattleUI)
    {

    }
}

public class HideBattleUIMessage : Message
{
    public HideBattleUIMessage()
        : base(MessageType.HideBattleUI)
    {

    }
}
public class ShowInGoalUIMessage : Message
{
    public ShowInGoalUIMessage(LLUnit kUnit)
        : base(MessageType.ShowInGoalUI)
    {
        m_kUnit = kUnit;
    }

    public LLUnit Unit
    {
        get { return m_kUnit; }
    }

    private LLUnit m_kUnit;
}

public class HideInGoalUIMessage : Message
{
    public HideInGoalUIMessage()
        : base(MessageType.HideInGoalUI)
    {

    }
}
public class ShowPreBattleUIMessage : Message
{
    public ShowPreBattleUIMessage(LLTeam kRedTeam,LLTeam kBlueTeam)
        : base(MessageType.ShowPerBattleUI)
    {
        m_kRedTeam = kRedTeam;
        m_kBlueTeam = kBlueTeam;
    }


    public LLTeam RedTeam
    {
        get { return m_kRedTeam; }
    }

    public LLTeam BlueTeam
    {
        get { return m_kBlueTeam; }
    }

    private LLTeam m_kRedTeam;
    private LLTeam m_kBlueTeam;
}

public class DestroyPreBattleUIMessage : Message
{
    public DestroyPreBattleUIMessage()
        : base(MessageType.DestroyPerBattleUI)
    {

    }
}