using System;
using Common;
using BehaviourTree;

public class LLGoalKeeper : LLUnit
{
    public LLGoalKeeper(LLTeam kTeam, PlayerInfo kBaseInfo)
        :base(kBaseInfo)
    {
        m_kTeam = kTeam;
        int iID = m_kGoalKeeperAI.Database.GetDataID(BTConstant.Player);
        m_kGoalKeeperAI.Database.SetData<LLGoalKeeper>(iID, this);
        iID = m_kGoalKeeperAI.Database.GetDataID(BTConstant.GoalKeeper);
        m_kGoalKeeperAI.Database.SetData<LLTeam>(iID, kTeam);
        m_kGoalKeeperAI.Load("Tables/AI/GoalKeeperAI");
        CreatePlayerMessage kMsg = new CreatePlayerMessage(this, kBaseInfo,true);
        MessageDispatcher.Instance.SendMessage(kMsg);
    }

    public override void Update(double dTime)
    {
        base.Update(dTime);
        LLBall kBall = m_kTeam.Scene.Ball;
        if (true == CanUpdateRotate)
        {
            double _dRotAngle = MathUtil.GetAngle(m_kPos, kBall.GetPosition());
            SetRoteAngle(_dRotAngle);
        } 

        m_kGoalKeeperAI.Update(dTime);
    }

    public override void MoveToPos(Vector3D targetPos, double dTime)
    {
        if (GetPosition().Distance(targetPos) > Velocity * dTime)
        {
            //move it
            Vector3D movePos = GetPosition() + DirAdjustment(MathUtil.GetDir(GetPosition(), targetPos)) * Velocity * dTime;
            SetPosition(movePos);
            //强制令球员转向进入到目标点//
            double dAngle = MathUtil.GetAngle(GetPosition(), targetPos);
            if (Math.Abs(GetRotAngle() - dAngle) > 5f)
                SetRoteAngle(dAngle);
        }
        else
        {
            //arrived
            SetPosition(targetPos);
        }
    }

    public override void Destroy()
    {
        RemovePlayerMessage kMsg = new RemovePlayerMessage(m_kTeam.TeamColor, this, true);
        MessageDispatcher.Instance.SendMessage(kMsg);
    }
  

    public EGKState GKState
    {
        get { return m_GKState; }
        set { m_GKState = value; }
    }

    public Vector3D SavePoint
    {
        get { return mSavePoint; }
        set { mSavePoint = value; }
    }

    private Vector3D mSavePoint = Vector3D.zero;
    private EGKState m_GKState = EGKState.GS_DEFAULT;
    private BTree m_kGoalKeeperAI = new BTree();
}
