using Common;
using Common.Tables;
using System;

public class LLBall : LLEntity
{
    public LLBall(LLScene kScene)
    {
        m_kScene = kScene;
        CreateBalllMessage kMsg = new CreateBalllMessage(this, m_kScene);
        MessageDispatcher.Instance.SendMessage(kMsg);
    }
    public override void Destroy()
    {
        DestroyBallMessage kMsg = new DestroyBallMessage(this);
        MessageDispatcher.Instance.SendMessage(kMsg);
    }
    public override void Update(double dTime)
    {
        if (!m_bCanMove)
            return;
        switch(m_moveType)
        {
        case EBallMoveType.GroundPass:
            Move_GroundPass(dTime);
            break;            
        case EBallMoveType.HighLobPass:
        case EBallMoveType.HeadingToHighLob:
            Move_HighLobPass(dTime);
            break;
		case EBallMoveType.Shooting:
			Move_Shooting(dTime);
			break;
        default:
            Move_GroundPass(dTime);
            break;
        }
        m_iRegionID = m_kScene.GetRegionID(m_kPos);
    }

    #region 球的运动轨迹计算
//    public void SetBallMoveType(EBallMoveType _type)
//    {
//        m_moveType = _type;
//    }

    /// <summary>
    /// 轨迹完成后进行数据
    /// </summary>
    private void RemoveBallMoveData()
    {
        m_kballSourcePos = m_kballDestinaPos = Vector3D.zero;
    }
    /// <summary>
    /// 设置球的起始位置和球的重点位置
    /// </summary>
    /// <param name="_Spos"></param>
    /// <param name="_Dpos"></param>
    public void SetMoveData(Vector3D _Spos,Vector3D _Dpos)
    {
        m_kballSourcePos = _Spos;
        m_kballDestinaPos = _Dpos;
    }

    /// <summary>
    /// 短传模拟
    /// </summary>
    /// <param name="dTime"></param>
    private void Move_GroundPass(double dTime)
    {
        double dDist = m_kPos.Distance(m_kTargetPos);
        double ds = m_dVelocity * dTime;
        m_dVelocity += m_dDelta * dTime;

//        if (Math.Abs(m_dVelocity_V) <= 0.0001d && m_kPos.Z <= 0.00001d)
//        {
//            m_dVelocity_V = 0d;
//        }
//        else
//        {
//            m_dVelocity_V -= m_G * dTime;
//            m_kPos.Y += m_dVelocity_V * dTime;
//
//            if (m_kPos.Y < 0)
//            {
//                m_kPos.Y = 0;
//                m_dVelocity_V = -0.8 * m_dVelocity_V;
//            }
//        }
        m_kPos.Y = 0;

        if (ds < 0) 
        {
            ds = 0;
		}

		if (dDist > ds) 
        {
			Vector3D kDir = m_kTargetPos - m_kPos;
            kDir.Y = 0d;
			kDir = kDir.Normalize();
			
			double goalWidth = TableManager.Instance.BattleInfoTable.GetItem("GoalWidth").Value;
			double goalpostXDiff = Math.Abs(goalWidth / 2 - Math.Abs(m_kPos.X));
			
			double groundLength = TableManager.Instance.BattleInfoTable.GetItem("GroundLength").Value;
			double goalDepth = TableManager.Instance.BattleInfoTable.GetItem("GoalDepth").Value;
			bool isBallInGoalDepth = (Math.Abs(m_kPos.Z) > (groundLength / 2)) && (Math.Abs(m_kPos.Z) < (groundLength / 2 + goalDepth));
			
			if ((goalpostXDiff < 0.3) && isBallInGoalDepth) 
            {
				m_kPos.Z = m_kPos.Z + kDir.Z * ds;
			} 
            else 
            {
				m_kPos = m_kPos + kDir * ds;
			}
		} 
        else 
        {
			m_kPos = m_kTargetPos;
            #if GAME_AI_ONLY
            isGroundPass = false;
            #endif
		}
    }

    /// <summary>
    /// simulate long lobbing pass,from foot
    /// </summary>
    /// <param name="dTime">division time.</param>
    private void Move_HighLobPass(double dTime)
    {		
        Vector3D targetGroundPos = new Vector3D(m_kTargetPos.X,0,m_kTargetPos.Z);
        Vector3D currentGroundPos = new Vector3D(m_kPos.X,0,m_kPos.Z);
        double horizontalDist = currentGroundPos.Distance(targetGroundPos);
		double ds = m_dVelocity_H * dTime;
		m_dVelocity_H += m_dDelta * dTime;
		
		if (Math.Abs(m_dVelocity_V) <= 0.0001d /*&& m_kPos.Z <= 0.00001d*/)
		{
			m_dVelocity_V = 0d;
		}
		else
		{
            //move virtically
			m_dVelocity_V -= m_G * dTime;
			m_kPos.Y += m_dVelocity_V * dTime;			
			if (m_kPos.Y < 0)
			{
				m_kPos.Y = 0;
				m_dVelocity_V = -0.8 * m_dVelocity_V;
			}
		}
		
		if (ds < 0) 
        {
			ds = 0;
		}		
        if (horizontalDist > ds)
        {
            //move horizontally
			Vector3D kDir = m_kTargetPos - m_kPos;
            kDir.Y = 0;
			kDir = kDir.Normalize();
            m_kPos = m_kPos + kDir * ds;
		} 
        else 
        {
            m_kPos = m_kTargetPos;
            #if GAME_AI_ONLY
            isHighLobPass = false;
            #endif
        }
        #if GAME_AI_ONLY
        if (isHighLobPass)
        {
            timeCost += dTime;
            Common.Log.LogManager.Instance.BlackLog("Ball Time: "+timeCost);
        }
        #endif
    }
    #if GAME_AI_ONLY
    private double timeCost = 0d;
    #endif

	/// <summary>
	/// 射门模拟
	/// </summary>
	/// <param name="dTime"></param>
	private void Move_Shooting(double dTime)
	{
        double dDist = m_kPos.Distance(m_kTargetPos);
        double ds = m_dVelocity * dTime;
        m_dVelocity += m_dDelta * dTime;
        if (ds < 0)
            ds = 0;

        if (dDist > ds)
        {
            Vector3D kDir = m_kTargetPos - m_kPos;
       //     kDir.Y = 0d;
            kDir = kDir.Normalize();

            double goalWidth = TableManager.Instance.BattleInfoTable.GetItem("GoalWidth").Value;
            double goalpostXDiff = Math.Abs(goalWidth / 2 - Math.Abs(m_kPos.X));

            double groundLength = TableManager.Instance.BattleInfoTable.GetItem("GroundLength").Value;
            double goalDepth = TableManager.Instance.BattleInfoTable.GetItem("GoalDepth").Value;
            bool isBallInGoalDepth = (Math.Abs(m_kPos.Z) > (groundLength / 2)) && (Math.Abs(m_kPos.Z) < (groundLength / 2 + goalDepth));

            if ((goalpostXDiff < 0.3) && isBallInGoalDepth)
            {
                m_kPos.Z = m_kPos.Z + kDir.Z * ds;
            }
            else {
                m_kPos = m_kPos + kDir * ds;
            }
        }
        else
            m_kPos = m_kTargetPos;
    }
    #endregion


    public bool ArrivedTargetPos()
    {
        if (false == m_bCanMove)
            return false;

        double dDist = m_kPos.Distance(m_kTargetPos);
        if (dDist < 0.01 /*|| m_kRawVelocity/3 >= m_dVelocity*/)
            return true;
        return false;
    }

    public double PreCalculateFlyingTime(Vector3D kTarget)
    {
        Vector3D kDir = kTarget - m_kPos;
        kDir = kDir.Normalize();
        double dDist = kTarget.Distance(m_kPos);
        PassTimeTable kTable = TableManager.Instance.PassTimeTable;
        PassTimeItem kItem = kTable.GetItem(dDist);
        return kItem.Time * 2 / 3;
    }

    public void SetTarget(Vector3D kTarget, EBallMoveType moveType = EBallMoveType.GroundPass)
    {
        Vector3D kDir = kTarget - m_kPos;
        kDir = kDir.Normalize();
        m_kTargetPos = kTarget;
        m_kOriginalPos = m_kPos;
        m_kDir = kDir;
        double dDist = m_kTargetPos.Distance(m_kPos);

        if (moveType == EBallMoveType.GroundPass) 
		{
            PassTimeTable kTable = TableManager.Instance.PassTimeTable;
            PassTimeItem kItem = kTable.GetItem(dDist);
            m_dFlyingTime = kItem.Time * 2 / 3;
            m_dVelocity = 9 * dDist / (kItem.Time * 4);
			m_kRawVelocity = m_dVelocity;
			m_dDelta = -m_dVelocity / kItem.Time;
		}
		else if (moveType == EBallMoveType.HighLobPass) 
        {
            #if GAME_AI_ONLY
            timeCost = 0;
            #endif
            PassTimeTable kTable = TableManager.Instance.PassTimeTable;
            PassTimeItem kItem = kTable.GetItem(dDist);
            double targetHeightShift = 2.5d;
            //correction
            m_kTargetPos.Y = targetHeightShift;
			double shiftedTime = kItem.Time;

			m_dFlyingTime = shiftedTime*2/3;

			m_dVelocity_H = 9 * dDist / (shiftedTime * 4);
			m_dDelta = -m_dVelocity_H / shiftedTime;
            m_dVelocity_V = (targetHeightShift/m_dFlyingTime)+(m_G*m_dFlyingTime/2d);
			m_dVelocity = Math.Sqrt(m_dVelocity_H*m_dVelocity_H+m_dVelocity_V*m_dVelocity_V);
            m_kRawVelocity = m_dVelocity;
            Common.Log.LogManager.Instance.BlackLog("Ball FlyingTime: "+m_dFlyingTime);
		}
        else if (moveType == EBallMoveType.HeadingToHighLob) 
        {
            PassTimeTable kTable = TableManager.Instance.PassTimeTable;
            PassTimeItem kItem = kTable.GetItem(dDist);
            //time shift
            double targetHeightShift = 0;
            double shiftedTime = kItem.Time;
            
            m_dFlyingTime = shiftedTime*2/3;
            
            m_dVelocity_H = 9 * dDist / (shiftedTime * 4);
            m_dDelta = -m_dVelocity_H / shiftedTime;            
            m_dVelocity_V = (targetHeightShift/m_dFlyingTime)+(m_G*m_dFlyingTime/2d);            
            m_dVelocity = Math.Sqrt(m_dVelocity_H*m_dVelocity_H+m_dVelocity_V*m_dVelocity_V);
            m_kRawVelocity = m_dVelocity;
        }
        else if(moveType == EBallMoveType.Shooting)
        {
            ShootTimeTable kTable = TableManager.Instance.ShootTimeTbl;
            ShootTimeItem kItem = kTable.GetItem(dDist);
            m_dFlyingTime = kItem.Time * 2 / 3;
            m_dVelocity = 9 * dDist / (kItem.Time * 4);
            m_kRawVelocity = m_dVelocity;
            m_dDelta = -m_dVelocity / m_dFlyingTime;
        }
        else
        {
            PassTimeTable kTable = TableManager.Instance.PassTimeTable;
            PassTimeItem kItem = kTable.GetItem(dDist);
            m_dFlyingTime = kItem.Time * 2 / 3;
            m_dVelocity = 1.5 * dDist / kItem.Time;
            m_kRawVelocity = m_dVelocity;
            m_dDelta = -2 * m_dVelocity / (3 * kItem.Time);
        }
        m_moveType = moveType;
    }

    public override void SetPosition(Vector3D _vec)
    {
        m_kPos = _vec;
    }
    private void GetRemainFlyingTime()
    {

    }

    public double Delta
    {
        get { return m_dDelta; }
        set { m_dDelta = value; }
    }

    public double Velocity
    {
        get { return m_dVelocity; }
        set { m_dVelocity = value; }
    }

    public Vector3D TargetPos
    {
        get { return m_kTargetPos; }
    }

    public Vector3D OriginalPos
    {
        get { return m_kOriginalPos; }
    }

    public Vector3D TargetGroundPos
    {
        get 
        {            
            return new Vector3D(TargetPos.X,0,TargetPos.Z);
        }
    }

    public double FlyingTime
    {
        get{ return m_dFlyingTime; }
    }

    public double FlyingTimeRemain
    {
		get{ return m_dFlyingTime;}
    }

    public int RegionID
    {
        get { return m_iRegionID; }
    }

    public EBallState BallState
    {
        get { return m_kBallState; }
        set { m_kBallState = value; }
    }

    public bool CanMove
    {
        get { return m_bCanMove; }
        set { m_bCanMove = value; }
    }

    public EBallMoveType MoveType
    {
        get { return m_moveType;}
    }

    public Vector3D BallDir
    {
        get
        {
            return m_kDir;
        }
	}
	private double m_dVelocity_V = 0d;
	private double m_dVelocity_H = 0d;
    private double m_G = 9.8d;
    private double m_dVelocity = 5; // 球的速度
    private double m_dDelta = -5;// 加速度的值
    private Vector3D m_kTargetPos = new Vector3D();
    private Vector3D m_kOriginalPos = new Vector3D();
    private LLScene m_kScene; 
    private int m_iRegionID = 0;
    private double m_dFlyingTime = 0;
    private double m_kRawVelocity = 0;
    private bool m_bCanMove = true;
    private EBallState m_kBallState = EBallState.EBS_Running;
    private Vector3D m_kDir = Vector3D.zero;

    /// <summary>
    /// 球运动类型，如直线短传，抛物线长传
    /// </summary>
    private EBallMoveType m_moveType = EBallMoveType.NULL;

    private Vector3D m_kballSourcePos = Vector3D.zero;
    private Vector3D m_kballDestinaPos = Vector3D.zero;
    #if GAME_AI_ONLY
    public bool isGroundPass = false;
    public bool isHighLobPass = false;
    #endif
}