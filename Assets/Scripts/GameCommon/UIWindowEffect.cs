using UnityEngine;
using System.Collections.Generic;

public class UIWindowEffect : MonoBehaviour
{
    public enum Effect
    {
        None,
        Fade,
        ScaleAndFade,
    }

    public enum OnCloseEffectDone
    {
        DependOnUser,
        Destroy,
    }
	
	public const float FADEIN_DURATION = 0.3f;
	public const float FADEOUT_DURATION = 0.15f;

	public Effect mEffect = Effect.Fade;
    public OnCloseEffectDone mOnCloseEffectDone = OnCloseEffectDone.DependOnUser;

    private EventDelegate.Callback mOnOpenEffectDoneDel;
    private EventDelegate.Callback mOnCloseEffectDoneDel;

	void Awake()
	{
		//set invisible onstart
		if (mEffect == Effect.None)
			return;

		UIPanel p = gameObject.GetComponent<UIPanel>();
		if(p!=null)
			p.alpha = 0f;

		if (mEffect == Effect.ScaleAndFade) 
		{
			//disable multi screen class
			MultiScreen ms = gameObject.GetComponent<MultiScreen> ();
			if (ms != null)
				ms.enabled = false;
		}
	}

    public void DoOpenEffect(EventDelegate.Callback onOpenEffectDoneDel)
    {
        mOnOpenEffectDoneDel = onOpenEffectDoneDel;
        switch (mEffect)
        {
            case Effect.None:
                OpenEffectDone();
                break;
            case Effect.Fade:
				FadeIn();
//                TweenAlpha.Begin(gameObject, 0f, 0f);
//                List<EventDelegate> list = TweenAlpha.Begin(gameObject, FADE_DURATION, 1f).onFinished;
//                list.Clear();
//                list.Add(new EventDelegate(OpenEffectDone));
                break;
			case Effect.ScaleAndFade:
				ScaleAndFadeIn();
				break;
        }
    }

	private void FadeIn()
	{
//		UIPanel p = gameObject.GetComponent<UIPanel>();
//		p.alpha = 0f;
		TweenAlpha ta = TweenAlpha.Begin(gameObject, FADEIN_DURATION, 1f);
		
		EventDelegate ed = new EventDelegate (OpenEffectDone);
		ed.oneShot = true;
		ta.SetOnFinished (ed);
	}

	private void ScaleAndFadeIn()
	{
		transform.localScale = Vector3.one*1.3f;
//		UIPanel p = gameObject.GetComponent<UIPanel>();
//		p.alpha = 0f;
		TweenScale.Begin(gameObject, FADEIN_DURATION, Vector3.one);
		TweenAlpha ta = TweenAlpha.Begin(gameObject, FADEIN_DURATION, 1f);

		EventDelegate ed = new EventDelegate (OpenEffectDone);
		ed.oneShot = true;
		ta.SetOnFinished (ed);
	}

	private void ScaleAndFadeOut()
	{
//		transform.localScale = new Vector3 (1f,1f,1f);
//		UIPanel p = gameObject.GetComponent<UIPanel>();
//		p.alpha = 0f;
		TweenScale.Begin(gameObject, FADEOUT_DURATION, Vector3.one*1.3f);
		TweenAlpha ta = TweenAlpha.Begin(gameObject, FADEOUT_DURATION, 0f);
		
		EventDelegate ed = new EventDelegate (CloseEffectDone);
		ed.oneShot = true;
		ta.SetOnFinished (ed);
	}

	private void FadeOut()
	{
		TweenAlpha ta = TweenAlpha.Begin(gameObject, FADEOUT_DURATION, 0f);
		
		EventDelegate ed = new EventDelegate (CloseEffectDone);
		ed.oneShot = true;
		ta.SetOnFinished (ed);
	}

    void OpenEffectDone()
    {
        if (mOnOpenEffectDoneDel != null)
        {
            mOnOpenEffectDoneDel();
        }
    }

    public void DoCloseEffect(EventDelegate.Callback onCloseEffectDoneDel)
    {
        mOnCloseEffectDoneDel = onCloseEffectDoneDel;
        switch (mEffect)
        {
            case Effect.None:
                CloseEffectDone();
                break;
            case Effect.Fade:
				FadeOut();
//                List<EventDelegate> list = TweenAlpha.Begin(gameObject, FADE_DURATION, 0f).onFinished;
//                list.Clear();
//                list.Add(new EventDelegate(CloseEffectDone));
                break;
			case Effect.ScaleAndFade:
				ScaleAndFadeOut();
                break;
        }
    }

    protected void CloseEffectDone()
    {
        if (mOnCloseEffectDone == OnCloseEffectDone.DependOnUser)
        {
            if (mOnCloseEffectDoneDel != null)
            {
                mOnCloseEffectDoneDel();
            }
        }
        else if (mOnCloseEffectDone == OnCloseEffectDone.Destroy)
        {
            Destroy(gameObject);
        }
    }
}
