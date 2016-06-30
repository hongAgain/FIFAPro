using UnityEngine;
using System.Collections;

public class UIGlowAnimationHelper : MonoBehaviour {

	static string glowAnimationName = "";
	
//	public Transform topRoot = null;
//	public Transform midRoot = null;
//	public Transform bottomRoot = null;

	public UIWidget topWidget = null;
	public UIWidget midWidget = null;
	public UIWidget bottomWidget = null;

	public UISprite bg = null;
	public UISprite glowLine = null;
	
	private Vector3 topStartPos = new Vector3 ();
	private Vector3 topEndPos = new Vector3 ();
	private Vector3 midStartPos = new Vector3 ();
	private Vector3 midEndPos = new Vector3 ();
	private Vector3 bottomStartPos = new Vector3 ();
	private Vector3 bottomEndPos = new Vector3 ();

	// Use this for initialization
	void Start () 
	{
		if (bg != null && glowLine != null) 
		{
			Calculate ();
			Initialize();
			StartCoroutine (StartAnimeCommands());
		}
		else
		{
			gameObject.SetActive(false);
		}
	}

//	void OnEnable()
//	{
//		if (bg != null && glowLine != null) 
//		{
//			Calculate ();
//			Initialize();
//			StartCoroutine (StartAnimeCommands());
//		}
//		else
//		{
//			gameObject.SetActive(false);
//		}
//	}

	private void Calculate()
	{
		//calculate pos
		Vector3[] bgLocalCorner  = new Vector3[4];
		bgLocalCorner [0] = transform.InverseTransformPoint (bg.worldCenter);		
		bgLocalCorner [0] = new Vector3 (bgLocalCorner [0].x-bg.width/2f,bgLocalCorner [0].y-bg.height/2f,0);
		bgLocalCorner [1] = new Vector3 (bgLocalCorner [0].x,bgLocalCorner [0].y+bg.height,0);
		bgLocalCorner [2] = new Vector3 (bgLocalCorner [0].x+bg.width, bgLocalCorner [0].y+bg.height,0);
		bgLocalCorner [3] = new Vector3 (bgLocalCorner [0].x+bg.width, bgLocalCorner [0].y,0);

		bottomStartPos = bgLocalCorner[0] + new Vector3 (bottomWidget.width/2f,0,0);
		bottomEndPos = bgLocalCorner[3] - new Vector3 (bottomWidget.width/2f,0,0);

		topStartPos = bgLocalCorner[2] - new Vector3 (topWidget.width/2f,0,0);
		topEndPos = bgLocalCorner[1] + new Vector3 (topWidget.width/2f,0,0);

		Vector3[] glowLineLocalCorner  = new Vector3[4];	
		glowLineLocalCorner [0] = transform.InverseTransformPoint (glowLine.worldCenter);		
		glowLineLocalCorner [0] = new Vector3 (glowLineLocalCorner [0].x-glowLine.width/2f,glowLineLocalCorner [0].y-glowLine.height/2f,0);
		glowLineLocalCorner [1] = new Vector3 (glowLineLocalCorner [0].x,glowLineLocalCorner [0].y+glowLine.height,0);
		glowLineLocalCorner [2] = new Vector3 (glowLineLocalCorner [0].x+glowLine.width, glowLineLocalCorner [0].y+glowLine.height,0);
		glowLineLocalCorner [3] = new Vector3 (glowLineLocalCorner [0].x+glowLine.width, glowLineLocalCorner [0].y,0);

		midStartPos = (glowLineLocalCorner [0] + glowLineLocalCorner [1]) / 2f + new Vector3(midWidget.width/2f,0,0);
		midEndPos = (glowLineLocalCorner [2] + glowLineLocalCorner [3]) / 2f - new Vector3(midWidget.width/2f,0,0);
	}

	private void Initialize()
	{
		topWidget.cachedTransform.localPosition = topStartPos;
		midWidget.cachedTransform.localPosition = midStartPos;
		bottomWidget.cachedTransform.localPosition = bottomStartPos;
		
		topWidget.depth = bg.depth - 1;
		midWidget.depth = glowLine.depth + 1;
		bottomWidget.depth = bg.depth - 1;
		
		topWidget.alpha = 0f;
		midWidget.alpha = 0f;
		bottomWidget.alpha = 0f;
	}

	private void DisableAnime()
	{
		topWidget.cachedTransform.localPosition = topStartPos;
		midWidget.cachedTransform.localPosition = midStartPos;
		bottomWidget.cachedTransform.localPosition = bottomStartPos;
		
		topWidget.alpha = 0f;
		midWidget.alpha = 0f;
		bottomWidget.alpha = 0f;
	}

	IEnumerator StartAnimeCommands()
	{
		float startTime = Time.time;
		float timeProgress = 0f;

		timeProgress += 1f;
		while (Time.time - startTime < timeProgress)
			yield return null;

		// mid widget start floating
		Move (midWidget.cachedTransform,midStartPos,midEndPos,4f);
		Fade (midWidget.cachedTransform,0f,1f,0.5f);

		timeProgress += 3.5f;
		while (Time.time - startTime < timeProgress)
			yield return null;

		//fade out mid widget
		Fade (midWidget.cachedTransform,1f,0f,0.5f);

		timeProgress += 0.5f;
		while (Time.time - startTime < timeProgress)
			yield return null;

		Move (topWidget.cachedTransform,topStartPos,topEndPos,2f);
		Move (bottomWidget.cachedTransform,bottomStartPos,bottomEndPos,2f);
		Fade (topWidget.cachedTransform,0f,1f,1f);
		Fade (bottomWidget.cachedTransform,0f,1f,1f);

		timeProgress += 1f;
		while (Time.time - startTime < timeProgress)
			yield return null;

		Fade (topWidget.cachedTransform,1f,0f,1f);
		Fade (bottomWidget.cachedTransform,1f,0f,1f);

		timeProgress += 1f;
		while (Time.time - startTime < timeProgress)
			yield return null;

		DisableAnime ();
	}

	private void Move(Transform tf, Vector3 from, Vector3 to, float time, System.Action callback = null)
	{
		tf.localPosition = from;
		TweenPosition tp = TweenPosition.Begin(tf.gameObject, time, to);
		tp.onFinished.Clear();
		EventDelegate dele = new EventDelegate (()=>{
			if(callback!=null)
				callback();
		});
		dele.oneShot = true;
		tp.SetOnFinished(dele);
	}

	private void Fade(Transform tf, float alphaFrom, float alphaTo, float time,  System.Action callback = null)
	{
		UIWidget uw = tf.GetComponent<UIWidget>();
		uw.alpha = alphaFrom;
		TweenAlpha ta = TweenAlpha.Begin(tf.gameObject, time, alphaTo);
		ta.onFinished.Clear();
		EventDelegate dele = new EventDelegate (()=>{
			if(callback!=null)
				callback();
		});
		dele.oneShot = true;
		ta.SetOnFinished(dele);
	}
}
