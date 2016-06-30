using UnityEngine;
using System.Collections;
using Common;

public class RoleHelper : MonoBehaviour
{
    //public Animator mAnimator;

    //private bool inAnim = false;
    //private int state1 = Animator.StringToHash("Base Layer.QiuYuanXuanZe_XiangQianZou");
    //private int state2 = Animator.StringToHash("Base Layer.QiuYuanXuanZe_XiangHouTui");
    //private int state3 = Animator.StringToHash("Base Layer.QiuYuanXuanZe_SuiJiDongZuo");

    private Renderer rHose;
    private Renderer rShirt;
    private Renderer rBody;
    private Renderer rLeather;
    private Renderer rWrister_l;
    private Renderer rWrister_r;
    //private Renderer rShoes;
    public GameObject ball;

    void Awake()
    {
        //mAnimator = GetComponent<Animator>();
        //Destroy(GetComponentInChildren<Animation>());
		m_kAnimation = gameObject.GetComponentInChildren<Animation>();

        rHose       = transform.Find("Animation/Position/hose").renderer;
        rShirt      = transform.Find("Animation/Position/shirt").renderer;
        rBody       = transform.Find("Animation/Position/body").renderer;
        rLeather    = transform.Find("Animation/Position/leader").renderer;
        rWrister_l  = transform.Find("Animation/Position/left_wrister").renderer;
        rWrister_r  = transform.Find("Animation/Position/right_wrister").renderer;
        //rShoes      = transform.Find("Animation/Position/shoes").renderer;
        ball        = transform.Find("Animation/ball").gameObject;

        var rae = m_kAnimation.gameObject.AddComponent<RoleAnimationEvent>();
        rae.target = this;
    }

    void OnEnable()
    {
        if (m_kAnimation != null)
        {
            m_kAnimation.Play();
            Highlight(1f);
            ball.SetActive(false);
        }
    }

    public void ChangeUniform(string id)
    {
        StartCoroutine(ChangingUniform(id));
    }

    public void ChangeUniformImmidiately(string id)
    {
        Texture hose = ResourceManager.Instance.LoadTexture("Textures/Uniform/hose/" + string.Format("hose_{0}", id)) as Texture;
        Texture shirt = ResourceManager.Instance.LoadTexture("Textures/Uniform/shirt/" + string.Format("shirt_{0}", id)) as Texture;
        Texture shirtGloss = ResourceManager.Instance.LoadTexture("Textures/Uniform/shirt/" + string.Format("shirt_{0}_spe", id)) as Texture;
        
        rHose.sharedMaterial.mainTexture = hose;
        rBody.sharedMaterial.mainTexture = rShirt.sharedMaterial.mainTexture = 
            rLeather.sharedMaterial.mainTexture = rWrister_l.sharedMaterial.mainTexture = 
            rWrister_r.sharedMaterial.mainTexture = shirt;

        rBody.sharedMaterial.SetTexture("_TransGlossTex", shirtGloss);
        rShirt.sharedMaterial.SetTexture("_TransGlossTex", shirtGloss);
        rLeather.sharedMaterial.SetTexture("_TransGlossTex", shirtGloss);
        rWrister_l.sharedMaterial.SetTexture("_TransGlossTex", shirtGloss);
        rWrister_r.sharedMaterial.SetTexture("_TransGlossTex", shirtGloss);
    }

    public void Play(string animName, bool ballVisible)
    {
        if (ballVisible)
        {
            var clip = m_kAnimation.GetClip(animName);
            if (clip != null)
            {
                AnimationEvent auidoEvent1 = new AnimationEvent();
                auidoEvent1.time = 0;
                auidoEvent1.functionName = "BallVisible";
                clip.AddEvent(auidoEvent1);

                AnimationEvent auidoEvent2 = new AnimationEvent();
                auidoEvent2.time = clip.length;
                auidoEvent2.functionName = "BallUnvisible";
                clip.AddEvent(auidoEvent2);
            }
        }

        m_kAnimation.Play(animName);
        if (m_kAnimation.clip != null)
        {
            m_kAnimation.CrossFadeQueued(m_kAnimation.clip.name);
        }
    }

    //IEnumerator Animating(int toState, int checkState)
    //{
    //    int oldState = mAnimator.GetInteger("State");
    //    mAnimator.SetInteger("State", toState);

    //    var forward = transform.forward;
    //    var speed = mAnimator.GetFloat("Speed");
    //    var t = Time.time;
    //    var oldPos = transform.localPosition;

    //    while (true)
    //    {
    //        yield return null;

    //        var curInfo = mAnimator.GetCurrentAnimatorStateInfo(0);
    //        if (curInfo.nameHash == checkState)
    //        {
    //            break;
    //        }

    //        var deltaTime = Time.time - t;

    //        if ((oldPos - transform.localPosition).magnitude <= 1.5f)
    //        {
    //            transform.localPosition += forward * speed * deltaTime;
    //            t = Time.time;
    //        }
    //    }

    //    mAnimator.SetInteger("State", oldState);
    //    inAnim = false;
    //}

    IEnumerator ChangingUniform(string id)
    {
        var anim = GetComponentInChildren<Animation>();
        if (anim.IsPlaying("Idle"))
        {
            anim.CrossFade("QiuYuanHuanZhuang");
            anim.CrossFadeQueued("Idle");
        }

        var t = Time.time;
        var half_dur = 0.5f;
        var changeFlag = true;

        while (true)
        {
            yield return null;

            var elasp = Time.time - t;

            var amount = 0f;
            if (elasp <= half_dur)
            {
                amount = Mathf.Lerp(1f, 8f, elasp / half_dur);
            }
            else
            {
                if (changeFlag)
                {
                    changeFlag = false;
                    ChangeUniformImmidiately(id);
                }

                amount = Mathf.Lerp(8f, 1f, (elasp - half_dur) / half_dur);
            }

            Highlight(amount);

            if (elasp >= half_dur * 2)
            {
                yield break;
            }
        }
    }

    private void Highlight(float amount)
    {
        rShirt.sharedMaterial.SetFloat("_Amount", amount);
        rHose.sharedMaterial.SetFloat("_Amount", amount);
        rWrister_l.sharedMaterial.SetFloat("_Amount", amount);
        rWrister_r.sharedMaterial.SetFloat("_Amount", amount);
    }

    private Animation m_kAnimation = null;
}