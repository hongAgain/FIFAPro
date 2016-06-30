using UnityEngine;

public class PLBall : MonoBehaviour
{
    public void Start()
    {

        GameObject kCameraObj = GameObject.FindGameObjectWithTag("MainCamera");
        if (null == kCameraObj)
            return;

        m_kGameCamera = kCameraObj.GetComponent<GameCamera>();

        if(null == m_kGameCamera)
            m_kGameCamera = kCameraObj.AddComponent<GameCamera>();

        m_kGameCamera.Target = gameObject.transform;

        m_ballChild = transform.FindChild("soccer_ball");

        Init2DBall();
    }

    public void Init2DBall()
    {
        GameObject kBall2D = new GameObject("Ball2D");
        kBall2D.transform.parent = gameObject.transform;
        kBall2D.transform.localPosition = new Vector3(0, 0, 0);
        kBall2D.transform.localRotation = Quaternion.identity;
        m_kBall2D = Util.Attach2DBall("Battle/Football", kBall2D);
    }


    public void LateUpdate()
    {
        if (null == m_kBall || null == gameObject)
            return;
        Vector3D kPos = m_kBall.GetPosition();
        transform.localPosition = new Vector3((float)kPos.X, (float)(kPos.Y + 0.126), (float)kPos.Z);

        if(m_kBall.CanMove)
        {
            Vector3 _target = new Vector3((float)m_kBall.TargetPos.X, (float)m_kBall.TargetPos.Y, (float)m_kBall.TargetPos.Z);
            m_ballChild.Rotate(new Vector3((float)m_kBall.Velocity* 2f, 0, 0));
      //      m_ballChild.LookAt(_target);

        }
    }

    void OnDestroy()
    {
        Util.DetachHUD(m_kBall2D);
    }

    public LLBall Ball
    {
        get { return m_kBall; }
        set { m_kBall = value; }
    }
    
    
    // 游戏暂停与继续运行时所用 
    public bool Running
    {
        get { return m_bRunning; }
        set { m_bRunning = value; }
    }

    private GameObject m_kBall2D;
    private LLBall m_kBall = null;
    private GameCamera m_kGameCamera = null;
    private Transform m_ballChild = null;
    private bool m_bRunning = true;        // 用来表示游戏是暂停还是正常运行  仅用在GamePause State
}