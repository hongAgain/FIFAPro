using Common.Log;
using UnityEngine;


public class AnimationPlayer : MonoBehaviour
{
    public void Start()
    {
        //     gameObject.AddComponent<Ghost>();
        AniBall = gameObject.transform.FindChild("Animation/ball").gameObject;
        if (AniBall != null)
            AniBall.SetActive(false);
    }
    public void Play(AniClipData kData)
    {
        m_kAniClipData = kData;
        if (null == m_kAnimation.GetClip(kData.AniName))
        {
            LogManager.Instance.RedLog(kData.AniName + " is null ,Check it in AnimationPlayer");
            return;
        }
        m_kAniState = m_kAnimation[kData.AniName];
        m_kAniState.speed = kData.AniSpeed * GlobalBattleInfo.Instance.PlaySpeed;
        // m_fCrossFadeTime = m_kAniState.length / 3;
        if (true == m_kAniClipData.Mirror)
        {
            m_kAnimation.transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            m_kAnimation.transform.localScale = new Vector3(1, 1, 1);
        }
        m_kAnimation.Play(kData.AniName);
    }

    public void ScaleTime(float fScaleTime)
    {
        if (null != m_kAniState)
            m_kAniState.speed = m_kAniState.speed * fScaleTime;
    }


    public void Pause()
    {
        if (null == m_kAniState)
            return;
    //    m_kAniState.speed = m_kAniState.speed * 0.1f;
        m_fSpeed = m_kAniState.speed;
        m_kAniState.speed = 0;
    }

    public void Resume()
    {
        if (null == m_kAniState)
            return;
        m_kAniState.speed = m_fSpeed;
    }

    public void SetBallVisable(bool _Visable)
    {
        if (AniBall != null)
            AniBall.SetActive(_Visable);
    }
    public Animation Animation
    {
        get { return m_kAnimation; }
        set { m_kAnimation = value; }
    }

    public AnimationState CurAnimationState
    {
        get { return m_kAniState; }
    }

    public AniClipData AniClipData
    {
        get { return m_kAniClipData; }
    }

    public GameObject AniBall
    {
        set { m_kPLball = value; }
        get { return m_kPLball; }
    }
    private float m_fCrossFadeTime = 0;
    private Animation m_kAnimation;
    private AnimationState m_kAniState;
    private AniClipData m_kAniClipData;
    private string m_strAniName;
    private float m_fSpeed = 0;

    private GameObject m_kPLball = null;
}

