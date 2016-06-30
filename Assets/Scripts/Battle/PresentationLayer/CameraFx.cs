
using Common;
using Common.Tables;
using System.Collections.Generic;
using UnityEngine;

public class CameraFx : MonoBehaviour
{

    protected enum EMainState
    {
        Prepare = 0,
        WaitForStart = 1 ,
        Running,
        Finished,
        End
    }

    protected enum ERunState
    {
        CalcPos = 1,// 计算镜头初始位置
        UpdatePos,    // 正常运行
        End,
    }

    void Awake()
    {
        m_kCamera = gameObject.GetComponent<Camera>();
        m_kCamera.enabled = false; 
    }
    void Start()
    {
    }

    public void Init(List<int> kIDList,double dSkillDurTime,int iSkillID)
    {
        m_kIDList = kIDList;
        m_iIDIdx = 0;
        m_kState = EMainState.Prepare;
        m_iSkillID = iSkillID;
        //m_bInitialized = true;
        //

        m_fDurTime = (float)dSkillDurTime;
    }
    public void UnInit()
    {
        DisableBlackScreen();
    }

    // 黑屏
    private void EnableBlackScreen()
    {
        if (false == m_kEffectItem.BlackScreen)
            return;
        if (true == m_bBlackScreen)
            return;

        for (int i = 0; i < m_kEffectItem.FxIDList.Count; i++)
        {
            BaseFxBeginMessage kMsg = new BaseFxBeginMessage(m_kUnit.Player, m_kEffectItem.FxIDList[i],m_iSkillID);
            MessageDispatcher.Instance.SendMessage(kMsg);
        }
        m_kCamera.enabled = true;
        m_bBlackScreen = true;  
        int iMask = 1 << LayerMask.NameToLayer("SceneFx");
        m_kUnit.SetLayer("SceneFx");
        Camera kCamera = gameObject.GetComponent<Camera>();
        kCamera.cullingMask = iMask;
        if (m_kEffectItem.GhostFxID > 0)
        {
            GhostFxBeginMessage kMsg = new GhostFxBeginMessage(m_kEffectItem.GhostFxID, m_kUnit.Player);
            MessageDispatcher.Instance.SendMessage(kMsg);
        }

        GlobalBattleInfo.Instance.ScaleTime = (float)m_kEffectItem.ScaleTime;
        GlobalBattleInfo.Instance.PlaySpeed = (float)m_kEffectItem.ScaleTime;
        FrameFrozenBeginMessage kFrozenMsg = new FrameFrozenBeginMessage(m_kEffectItem.ScaleTime);
        kFrozenMsg.AddUnit(m_kUnit.Player);
        MessageDispatcher.Instance.SendMessage(kFrozenMsg);
    }

    private void DisableBlackScreen()
    {
        if (false == m_bBlackScreen)
            return;
        m_kUnit.SetLayer("Default");
        m_bBlackScreen = false;
        Camera kCamera = gameObject.GetComponent<Camera>();
        kCamera.cullingMask = -1;
        if (m_kEffectItem.GhostFxID > 0)
        {
            GhostFxEndMessage kMsg = new GhostFxEndMessage(m_kUnit.Player);
            MessageDispatcher.Instance.PostMessage(kMsg);
        }

        for (int i = 0; i < m_kEffectItem.FxIDList.Count; i++)
        {
            BaseFxEndMessage kMsg = new BaseFxEndMessage(m_kUnit.Player, m_kEffectItem.FxIDList[i]);
            MessageDispatcher.Instance.PostMessage(kMsg);
        }

        GlobalBattleInfo.Instance.ScaleTime /= (float)m_kEffectItem.ScaleTime;
        GlobalBattleInfo.Instance.PlaySpeed /= (float)m_kEffectItem.ScaleTime;
        FrameFrozenEndMessage kFrozenMsg = new FrameFrozenEndMessage(m_dScaleTime);
        MessageDispatcher.Instance.SendMessage(kFrozenMsg);
    }

    void Update()
    {
        switch(m_kState)
        {
            case EMainState.Prepare:
                OnPrepare();
                break;
            case EMainState.WaitForStart:
                OnWaitForStart();
                break;
            case EMainState.Running:
                OnRunning();
                break;
            case EMainState.End:
                DisableBlackScreen();
                if (m_iIDIdx >= m_kIDList.Count)
                {
                    CameraFxEndMessage kMsg = new CameraFxEndMessage();
                    MessageDispatcher.Instance.PostMessage(kMsg);
                }
                else
                    m_kState = EMainState.Prepare;
                break;
            default:
                break;
        }
    }

    private void OnPrepare()
    {
        if(m_iIDIdx >= m_kIDList.Count)
        {
            m_kState = EMainState.End;
            return;
        }
        else
        {
            CameraEffectTable kTable = TableManager.Instance.CameraEffectTbl;
            if (null == kTable)
            {
                m_kState = EMainState.End;
                return;
            }
            m_kEffectItem = kTable.GetItem(m_kIDList[m_iIDIdx++]);
            if (null == m_kEffectItem)
            {
                m_kState = EMainState.End;
                return;
            }
        }
        // 获得动画数据
        OnGetAniClipData();
        if (null == m_kAniClipData)
        {
            m_kState = EMainState.End;
            return;
        }
        
        float dEndTime = 0;
        switch(m_kEffectItem.EndType)
        {
            case ECameraEndType.Ani_Ball_Finished:
                dEndTime = m_kAniClipData.AllFrameTime;
                break;
            case ECameraEndType.Ani_Ball_Out:
                dEndTime = m_kAniClipData.BallOutTime;
                break;
            case ECameraEndType.Event_End:
                dEndTime = m_fDurTime;
                break;
        }

        switch(m_kEffectItem.StartType)
        {
            case ECameraStartType.Event_Start:
                break;
            case ECameraStartType.Ani_Ball_Out:
                m_fDurTime = dEndTime - m_kAniClipData.BallOutTime;
                break;
            case ECameraStartType.Ani_Ball_In:
                m_fDurTime = dEndTime - m_kAniClipData.BallInTime;
                break;
            default:
                break;
        }
        m_dElevationAngle = (float)m_kEffectItem.ElevationAngle;
        m_dCameraSpeed = m_kEffectItem.CameraSpeed;
        m_dLenSpeed = (float)(m_kEffectItem.EndLen - m_kEffectItem.StartLen) / m_fDurTime;
        m_dAzimuthAngle_Speed = (float)(m_kEffectItem.EndAzimuthAngle - m_kEffectItem.StartAzimuthAngle) / m_fDurTime;

        if (0 >= m_kEffectItem.LenTime)
            m_fLenTime = m_fDurTime;
        else
            m_fLenTime = (float)m_kEffectItem.LenTime;

        if (0 >= m_kEffectItem.AngleTime)
            m_fAngleTime = m_fDurTime;
        else
            m_fAngleTime = (float)m_kEffectItem.AngleTime;

        m_kState = EMainState.WaitForStart;
        m_kRunState = ERunState.CalcPos;
    }
    private void OnGetAniClipData()
    {
        if (null == m_kUnit)
            return;
        AnimationPlayer kPlayer = m_kUnit.transform.GetComponent<AnimationPlayer>();
        if (null == kPlayer)
            return;
        m_kAniClipData = kPlayer.AniClipData;
    }

    private void OnWaitForStart()
    {
        if(null == m_kEffectItem)
        {
            // 数据为空
            m_kState = EMainState.End;
            return;
        }
        switch(m_kEffectItem.StartType)
        {
            case ECameraStartType.Event_Start:
                m_kState = EMainState.Running;
                break;
            case ECameraStartType.Ani_Ball_In:
                if(null == m_kUnit)
                {
                    // 数据不对，没有对象，所以无法取到动画相关数据
                    m_kState = EMainState.End;
                }
                else
                {
                    if(m_kUnit.Player.IsBallIn)
                    {
                        m_kState = EMainState.Running;
                    }
                }
                break;
            case ECameraStartType.Ani_Ball_Out:
                if (null == m_kUnit)
                {
                    // 数据不对，没有对象，所以无法取到动画相关数据
                    m_kState = EMainState.End;
                }
                else
                {
                    if (m_kUnit.Player.IsBallOut)
                    {
                        m_kState = EMainState.Running;
                    }
                }
                break;
            default:
                m_kState = EMainState.End;
                break;
        }
    }

    private void OnRunning()
    {
        switch(m_kRunState)
        {
            case ERunState.CalcPos:
                OnCalcPos();
                break;
            case ERunState.UpdatePos:
                EnableBlackScreen();
                OnUpdatePos();
                break;
            case ERunState.End:
                m_kState = EMainState.End;
                break;
            default:
                break;
        }
    }


    private void OnCalcPos()
    {
        switch(m_kEffectItem.AngleType)
        {
            case ECameraType.Play_Goal_Line:
                CalcPos_Player_Goal_Line();
                break;
            case ECameraType.Camera_Goal_Line:
                CalcPos_Player_Goal_Line();
                break;
            default:
                break;
        }
        m_kRunState = ERunState.UpdatePos;
    }

    private void OnUpdatePos()
    {
        switch (m_kEffectItem.AngleType)
        {
            case ECameraType.Play_Goal_Line:
                On_Update_Player_Goal_Line();
                if (m_fAngleTime < 0 || m_fLenTime < 0)
                    m_kRunState = ERunState.End;
                break;
            case ECameraType.Camera_Goal_Line:
                On_Update_Camera_Goal_Line();
                break;
            default:
                break;
        }
    }

    private void CalcPos_Player_Goal_Line()
    {
        double dBaseAngle = MathUtil.GetAngle(m_kUnit.Player.GetPosition(), m_kUnit.Player.Team.Goal.GoalPos);
        m_dAzimuthAngle_Cur = (float)(dBaseAngle + m_kEffectItem.StartAzimuthAngle + 90);
        m_dAzimuthAngle_End = (float)(dBaseAngle + m_kEffectItem.EndAzimuthAngle + 90);
        m_dCurLen = (float)m_kEffectItem.StartLen;
        On_Update_Player_Goal_Line();
    }

    //private void CalcPos_Camera_Goal_Line()
    //{
    //    double dBaseAngle = MathUtil.GetAngle(m_kUnit.Player.Position, m_kUnit.Player.Team.Goal.GoalPos);
    //    m_dAzimuthAngle_Cur = (float)(dBaseAngle + m_kEffectItem.StartAngle + 90);
    //    m_dAzimuthAngle_End = (float)(dBaseAngle + m_kEffectItem.EndAngle + 90);
    //    m_dCurLen = (float)m_kEffectItem.StartLen;
    //    On_Update_Player_Goal_Line();
    //}
    

    private void On_Update_Camera_Goal_Line()
    {
        //    m_dCurLen += (m_dLenSpeed * GlobalBattleInfo.Instance.DeltaTime);

        Vector3D kGoalPos = m_kUnit.Player.Team.Opponent.Goal.GoalPos;
        Vector3D kCameraPos = MathUtil.ConverToVector3D(transform.position);
        Vector3D kDir = new Vector3D(kGoalPos.X,kCameraPos.Y,kGoalPos.Z) - kCameraPos;
        kDir.Y += m_kEffectItem.TargetHeight;
        kDir = kDir.Normalize();
        double dDist = kGoalPos.Distance(kCameraPos);
        if(dDist < m_kEffectItem.EndLen)
            m_kRunState = ERunState.End;
        else
        {
            kCameraPos += (kDir * m_dCameraSpeed);
            transform.position = MathUtil.ConverToVector3(kCameraPos);
        }
    }

    private void On_Update_Player_Goal_Line()
    {
        if(m_fAngleTime > 0)
        {
            m_dAzimuthAngle_Cur += (m_dAzimuthAngle_Speed * GlobalBattleInfo.Instance.DeltaTime);
        }

        if(m_fLenTime > 0)
        {
            m_dCurLen += (m_dLenSpeed * GlobalBattleInfo.Instance.DeltaTime);
        }
        
        float fXOZLen = m_dCurLen * Mathf.Cos(m_dElevationAngle * Mathf.PI / 180);
        float fY = m_dCurLen * Mathf.Sin(m_dElevationAngle * Mathf.PI / 180);
        float fX = fXOZLen * Mathf.Cos(m_dAzimuthAngle_Cur * Mathf.PI / 180);
        float fZ = fXOZLen * Mathf.Sin(m_dAzimuthAngle_Cur * Mathf.PI / 180);
        Vector3 kPos = m_kUnit.transform.position;
        kPos += new Vector3(fX, fY, fZ);
        gameObject.transform.localPosition = kPos;
        Vector3 kDir = m_kUnit.transform.position - transform.position;
        kDir.y += (float)m_kEffectItem.TargetHeight;
        kDir.Normalize();
        gameObject.transform.localRotation = Quaternion.LookRotation(kDir);

        m_fAngleTime -= GlobalBattleInfo.Instance.DeltaTime;
        m_fLenTime -= GlobalBattleInfo.Instance.DeltaTime;


    }

    //public int ID
    //{
    //    get { return m_kIDList; }
    //    set { m_kIDList = value; }
    //}

    public PLPlayer Unit
    {
        get { return m_kUnit; }
        set { m_kUnit = value; }
    }

    private CameraEffectItem m_kEffectItem = null;
    private bool m_bInitialized = false;
    private bool m_bStart = false;
    
    private List<int> m_kIDList = new List<int>();

    private Camera m_kCamera = null;
    private PLPlayer m_kUnit = null;
    private bool m_bBlackScreen = false;
    private EMainState m_kState = EMainState.WaitForStart;
    private ERunState m_kRunState = ERunState.CalcPos;
    private float m_dAzimuthAngle_Cur;              // 方位角
    private float m_dCurLen;    // 长度
    private float m_dAzimuthAngle_End;
    private float m_dAzimuthAngle_Speed;
    private float m_dLenSpeed;
    private float m_dElevationAngle = 15.0f;    //仰角     （0-180）
    private AniClipData m_kAniClipData = null;
    private float m_fLenTime;
    private float m_fAngleTime;
    private float m_fDurTime = 0;               // 效果总时长
    private int m_iIDIdx = 0;
    private bool m_bFrameFrozen = false; // 帧冻结
    private double m_dStartFrameTime = 0.0;         // 开始帧冻结的时间
    private double m_dFrameFrozenElapseTime = 0.0;  // 帧冻结已过时间
    private double m_dFrameFrozenDuration = 0.0;    // 帧冻结持续时间
    private double m_dScaleTime = 1.0;              // 帧冻结持续时间缩放比例
    private bool m_bStartScale = false;             // 帧冻结是否已经开始
    private double m_dCameraSpeed = 1;              // 相机移动速度
    private int m_iSkillID; //技能ID
}