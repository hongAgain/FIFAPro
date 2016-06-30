using Common;
using System.Diagnostics;
using UnityEngine;

public class PLPlayer : MonoBehaviour
{
    public void ChangeAnimationAngle(float fAngle)
    {
        m_bChangeAngle = true;
        m_fAngle = fAngle;
    }
    public void Awake()
    {
        m_bChangeAngle = false;
    }

    public void Start()
    {
        m_bChangeAngle = false;
        m_kAniObj = gameObject.transform.FindChild("Animation").gameObject;
        m_kAniPlayer = gameObject.AddComponent<AnimationPlayer>();
        m_kAniPlayer.Animation = m_kAniObj.GetComponent<Animation>();
        ChangeMaterial();
        InitHUD();
    }

    void OnBecameVisible()
    {
        gameObject.SetActive(true);
    }

    void OnBecameInvisible()
    {
        gameObject.SetActive(false);
    }

    public void SetLayer(string strLayer)
    {
        Transform kTransform = m_kAniObj.transform.FindChild("Position");
        Util.SetParentAndAllChildrenToLayer(kTransform, strLayer);
        kTransform = m_kAniObj.transform.FindChild("All_001");
        Util.SetParentAndAllChildrenToLayer(kTransform, strLayer);
    }
    public void AddGhostFx(int iFxID)
    {
        GhostFx kFxObj = gameObject.AddComponent<GhostFx>();
        kFxObj.ID = iFxID;
    }

    public void RemoveGhosFx()
    {
        GhostFx kGhostFx = gameObject.GetComponent<GhostFx>();
        if(null != kGhostFx)
            GameObject.Destroy(kGhostFx);
    }

    public void OnDestroy()
    {
        Util.DetachHUD(m_kHUDObj);
        Util.DetachHUD(m_kHUDObj2D);
    }
    
    public void Pause()
    {
        if (null != m_kAniPlayer)
            m_kAniPlayer.Pause();
    }

    public void Resume()
    {
        if (null != m_kAniPlayer)
            m_kAniPlayer.Resume();
    }
    
    public void LateUpdate()
    {
        if (null == m_kPlayer || null == gameObject)
            return;
        Vector3D kPos = m_kPlayer.GetPosition();
        transform.localPosition = new Vector3((float)kPos.X, (float)kPos.Y, (float)kPos.Z);
        m_kAniObj.transform.localRotation = Quaternion.AngleAxis((float)m_kPlayer.GetRotAngle(), new Vector3(0, 1, 0));

        if(m_bChangeAngle)
        {
            Transform kTransform = m_kAniObj.transform.FindChild("All_001");
            Vector3 kAngle = kTransform.localRotation.eulerAngles;
            kAngle.y += m_fAngle;
            Quaternion kQuat = new Quaternion();
            kQuat.eulerAngles = kAngle;
            kTransform.localRotation = kQuat;
            m_bChangeAngle = false;
        }

        ChangeSkinColor();
        OnUpdateSP();
        DrawNSInfo();
    }
    public void PlayAnimation(AniClipData kClipData)
    {
        if (null != m_kAniPlayer)
            m_kAniPlayer.Play(kClipData);
    }

    private void InitHUD()
    {
        GameObject kHUDLocater = new GameObject("HUDText");
        kHUDLocater.transform.parent = gameObject.transform;
        kHUDLocater.transform.localPosition = new Vector3(0, 2.6f, 0);
        kHUDLocater.transform.localRotation = Quaternion.identity;
        m_kHUDObj = Util.AttachHUD("Battle/PlayerHUD", kHUDLocater);
        m_kHUDObj2D = Util.Attach2D("Battle/PlayerHUD2D", kHUDLocater);
#if !GAME_AI_ONLY
        m_kHUDName = m_kHUDObj2D.transform.FindChild("Hero").gameObject;
        m_sldSP = m_kHUDObj.transform.FindChild("Hero/SP").GetComponent<UISlider>();
        m_sldSP2D = m_kHUDObj2D.transform.FindChild("Hero/SP").GetComponent<UISlider>();
        m_sprForeground = m_kHUDObj.transform.FindChild("Hero/SP/Foreground").GetComponent<UISprite>();
        m_sprForeground2D = m_kHUDObj2D.transform.FindChild("Hero/SP/Foreground").GetComponent<UISprite>();
        UIHelper.SetLabelTxt(m_kHUDObj.transform.FindChild("Hero/HeroID"), m_kPlayer.PlayerBaseInfo.HeroName);
        UIHelper.SetLabelTxt(m_kHUDObj2D.transform.FindChild("Hero/HeroID"), m_kPlayer.PlayerBaseInfo.HeroName);
        int playerType = 1;
        if (m_kPlayer.TeamType == ETeamColor.Team_Blue)
        {
            if (IsGoalKeeper)
                playerType = 2;
        }
        else if (m_kPlayer.TeamType == ETeamColor.Team_Red)
        {
            if (IsGoalKeeper)
                playerType = 4;
            else
                playerType = 3;
        }

        UIBattle.Attach2DPlayer(m_kHUDObj2D.transform.FindChild("Player").GetComponent<UISprite>(), playerType);
#endif

    }

    private void OnUpdateSP()
    {
#if !GAME_AI_ONLY
        if (UIBattle.Instance && UIBattle.Instance.Is2DModel)
        {
            if (m_kPlayer.IsCtrlBall)
            {
                m_kHUDObj2D.SetActive(true);
                m_kHUDName.SetActive(true);
                m_sldSP2D.value = (float)m_kPlayer.PlayerBaseInfo.Energy / (float)m_kPlayer.PlayerBaseInfo.Attri.stamina;

                UIBattle.SetColorSP(m_sprForeground2D, m_sldSP2D.value);
            }
            else
            {
                m_kHUDName.SetActive(false);
            }

            m_kHUDObj.SetActive(false);
        } 
        else
        {
            if (m_kPlayer.IsCtrlBall)
            {
                m_kHUDObj.SetActive(true);
                m_sldSP.value = (float)m_kPlayer.PlayerBaseInfo.Energy / (float)m_kPlayer.PlayerBaseInfo.Attri.stamina;

                UIBattle.SetColorSP(m_sprForeground, m_sldSP.value);
            }
            else
            {
                m_kHUDObj.SetActive(false);
            }
        }
#endif
    }


    #region BATTLE_DEBUG
    /// <summary>
    /// 绘制数值对抗信息
    /// </summary>
    private void DrawNSInfo()
    {
        if (false == m_kPlayer.ShowDebugInfo)
            return;

        Color kColor = Color.red;
        if(m_kPlayer.RedColor)
        {
            kColor = Color.red;
        }
        else
        {
            kColor = Color.blue;
        }

        Vector3 kPos = gameObject.transform.localPosition;
        Vector3 kStartPos = kPos;
        kStartPos.x += 0.5f;
        kStartPos.z -= 0.5f;
        Vector3 kEndPos = kStartPos;
        kEndPos.x = kStartPos.x - 1;
        UnityEngine.Debug.DrawLine(kStartPos, kEndPos, kColor);

        kStartPos = kEndPos;
        kEndPos.z += 1;
        UnityEngine.Debug.DrawLine(kStartPos, kEndPos, kColor);

        kStartPos = kEndPos;
        kEndPos.x += 1;
        UnityEngine.Debug.DrawLine(kStartPos, kEndPos, kColor);

        kStartPos = kEndPos;
        kEndPos.z -= 1;
        UnityEngine.Debug.DrawLine(kStartPos, kEndPos, kColor);
    }
    [Conditional("BATTLE_DEBUG")]
    private void ChangeSkinColor()
    {
        if (m_bGoalKeeper)
            return;
        LLPlayer kPlayer = Player as LLPlayer;

        GameObject kBody = m_kAniObj.transform.Find("Position/body").gameObject;
        GameObject kShirt = m_kAniObj.transform.Find("Position/shirt").gameObject;
        Renderer kBodyRenderer = kBody.GetComponent<Renderer>();
        Renderer kShirtRenderer = kShirt.GetComponent<Renderer>();
        if (ETeamColor.Team_Red == m_kPlayer.Team.TeamColor)
        {
            kBodyRenderer.material.SetColor("_UniformColor", Color.yellow);
            kShirtRenderer.material.SetColor("_UniformColor", Color.yellow);
        }
        else
        {
            kBodyRenderer.material.SetColor("_UniformColor", Color.blue);
            kShirtRenderer.material.SetColor("_UniformColor", Color.blue);
        }
        if (true == kPlayer.IsCtrlBall)
        {
            kBodyRenderer.material.SetColor("_UniformColor", Color.green);
            kShirtRenderer.material.SetColor("_UniformColor", Color.green);
        }
    }

    [Conditional("BATTLE_DEBUG")]
    private void ChangeMaterial()
    {

        GameObject kBody = m_kAniObj.transform.Find("Position/body").gameObject;
        GameObject kShirt = m_kAniObj.transform.Find("Position/shirt").gameObject;
        Renderer kBodyRenderer = kBody.GetComponent<Renderer>();
        Renderer kShirtRenderer = kShirt.GetComponent<Renderer>();
        if (ETeamColor.Team_Red == m_kPlayer.Team.TeamColor)
        {
            kBodyRenderer.material.SetColor("_UniformColor", Color.yellow);
            kShirtRenderer.material.SetColor("_UniformColor", Color.yellow);
        }
        else
        {
            kBodyRenderer.material.SetColor("_UniformColor", Color.blue);
            kShirtRenderer.material.SetColor("_UniformColor", Color.blue);
        }

    }

    [Conditional("BATTLE_DEBUG")]
    private void DebugDraw()
    {
        if (null == m_kPlayer)
            return;

        Color kColor = Color.red;
        if (IsGoalKeeper)
        {
            kColor = ((LLGoalKeeper)m_kPlayer).TeamType == Common.ETeamColor.Team_Red ? Color.red : Color.blue;
        }
        else
        {
            LLPlayer kPlayer = (LLPlayer)m_kPlayer;
            kColor = kPlayer.TeamType == Common.ETeamColor.Team_Red ? Color.red : Color.blue;
        }

        Vector3 kPos = gameObject.transform.localPosition;
        Vector3 kStartPos = kPos;
        kStartPos.x += 0.5f;
        kStartPos.z -= 0.5f;
        Vector3 kEndPos = kStartPos;
        kEndPos.x = kStartPos.x - 1;
        UnityEngine.Debug.DrawLine(kStartPos, kEndPos, kColor);

        kStartPos = kEndPos;
        kEndPos.z += 1;
        UnityEngine.Debug.DrawLine(kStartPos, kEndPos, kColor);

        kStartPos = kEndPos;
        kEndPos.x += 1;
        UnityEngine.Debug.DrawLine(kStartPos, kEndPos, kColor);

        kStartPos = kEndPos;
        kEndPos.z -= 1;
        UnityEngine.Debug.DrawLine(kStartPos, kEndPos, kColor);
    }
#endregion
    public LLUnit Player
    {
        get { return m_kPlayer; }
        set { m_kPlayer = value; }
    }

    public bool IsGoalKeeper
    {
        get { return m_bGoalKeeper; }
        set { m_bGoalKeeper = value; }
    }

    public AnimationPlayer AniPlayer
    {
        get { return m_kAniPlayer; }
    }

    public GameObject AniObj
    {
        get { return m_kAniObj; }
    }

    private LLUnit m_kPlayer = null;
    private AnimationPlayer m_kAniPlayer = null;
    private GameObject m_kAniObj = null;
    private GameObject m_kHUDObj = null;
    private GameObject m_kHUDObj2D = null;
    private GameObject m_kHUDName = null;
    private UISlider m_sldSP = null;
    private UISlider m_sldSP2D = null;
    private UISprite m_sprForeground = null;
    private UISprite m_sprForeground2D = null;
    private GameObject m_kMeshObj = null;
    private bool m_bGoalKeeper = false;

    private bool m_bChangeAngle;
    private float m_fAngle;
}