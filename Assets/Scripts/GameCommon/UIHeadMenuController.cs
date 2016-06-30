using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIHeadMenuController : MonoBehaviour {
	
	public static UIHeadMenuController controller = null;

//	public static int HorizontalDelta = 320;
//	public static int VerticleDelta = 0;

		//effect params
	public float paramLerpTime = 1f;
	public float paramLerpOffsetAnchor = 320f;
	public float paramLerpOffsetCamera = 0.88f;
	public TweenMath.TweenType paramTweenType = TweenMath.TweenType.ELASTIC;
	public TweenMath.EaseType paramEaseType = TweenMath.EaseType.EASEIN;

	public List<BoxCollider> buttonBoxes = new List<BoxCollider> ();


	private bool isSwitchedIn = false;
	private int count = 0;
	private Dictionary<int,UIHeadMenuAnchorRegister> anchors = new Dictionary<int, UIHeadMenuAnchorRegister>();

	private int camCount = 0;
	private Dictionary<int,UIHeadMenuSceneCamRegister> cameras = new Dictionary<int, UIHeadMenuSceneCamRegister>();

	#region static functions 
	public static int RegisterAnchor(UIHeadMenuAnchorRegister anchor)
	{
		if(controller==null)
			return -1;
		controller.count++;
		controller.anchors.Add(controller.count,anchor);
		return controller.count;
	}
	
	public static void UnregisterAnchor(int anchorID)
	{		
		if(controller==null)
			return;
		controller.anchors.Remove(anchorID);
	}

	public static int RegisterCamera(UIHeadMenuSceneCamRegister cam)
	{
		if(controller==null)
			return -1;
		controller.camCount++;
		controller.cameras.Add(controller.camCount,cam);
		return controller.camCount;
	}
	
	public static void UnregisterCamera(int cameraID)
	{		
		if(controller==null)
			return;
		controller.cameras.Remove(cameraID);
	}

	public static void SwitchMenuIn(System.Action onOver = null)
	{
		if(controller==null | controller.isSwitchedIn)
			return;		
		controller.StartSwitchIn(onOver);
	}
	
	public static void SwitchMenuOut(System.Action onOver = null)
	{
		if(controller==null | !controller.isSwitchedIn)
			return;
		controller.StartSwitchOut(onOver);
	}
	#endregion 

	private void Initialize()
	{
		//lerp camera position and paramLerpOffsetCamera, using screen ratio
		float aspect = Screen.width * 1f / Screen.height;
		float aspectMin = 4/3f;
		float aspectMax = 2/1f;

		float lerpValue = (aspect - aspectMin)/(aspectMax - aspectMin);
		
		float lerpOffsetMax = 0.88f;
		float lerpOffsetMin = 1.422f;
		paramLerpOffsetCamera = lerpValue*(lerpOffsetMax-lerpOffsetMin)+lerpOffsetMin;
	}

	public void StartSwitchIn(System.Action onOver = null)
	{
		SetButtontActive(false);
		StartCoroutine(SwitchLerp(
			paramLerpTime,
			0f,
			paramLerpOffsetAnchor,
			0f,
			1f,
			paramTweenType,
			paramEaseType,
			()=>{
				isSwitchedIn = true;
				SetButtontActive(true);
				if(onOver!=null)
					onOver();
			}
		));
	}

	public void StartSwitchOut(System.Action onOver = null)
	{
		StartCoroutine(SwitchLerp(
			paramLerpTime,
			paramLerpOffsetAnchor,
			-paramLerpOffsetAnchor,
			1f,
			-1f,
			paramTweenType,
			paramEaseType,
			()=>{
				isSwitchedIn = false;
				if(onOver!=null)
					onOver();
			}
		));
	}

	IEnumerator SwitchLerp(float targetTime,
	                       float beforeMoveAnchor, float changeDistanceAnchor,
	                       float beforeMoveCamera, float changeDistanceCamera,
	                       TweenMath.TweenType ttype, TweenMath.EaseType etype, System.Action onLerpOver)
	{
		float timeProgress = 0f;
		float lerpValue = 0f;

		while(targetTime-timeProgress >= 0.01f)
		{
			timeProgress = Mathf.Clamp(timeProgress+Time.deltaTime,0,targetTime);
			lerpValue = Lerp(timeProgress,beforeMoveAnchor,changeDistanceAnchor,targetTime,ttype,etype);
			MoveAbsoluteX(lerpValue);
//
//			lerpValue = Lerp(timeProgress,beforeMoveCamera,changeDistanceCamera,targetTime,ttype,etype);
//			MoveCameras(lerpValue);
            
            lerpValue = Lerp(timeProgress,beforeMoveCamera,changeDistanceCamera,targetTime,ttype,etype);
            LerpCameras(lerpValue);



			yield return null;
		}
        MoveAbsoluteX(beforeMoveAnchor+changeDistanceAnchor);
//        MoveCameras(beforeMoveCamera+changeDistanceCamera);
        LerpCameras(beforeMoveCamera+changeDistanceCamera);
		if(onLerpOver!=null)
			onLerpOver();
	}

	private float Lerp(float timeProgress,float beforeMove,float changeDistance,float targetTime,TweenMath.TweenType ttype,TweenMath.EaseType etype)
	{
		float currentValue = beforeMove;
		//LINEAR, QUAD, CUBIC, QUART, QUINT, SINE, EXPO, CIRC, ELASTIC, BACK, BOUNCE
		if(ttype == TweenMath.TweenType.LINEAR)
			currentValue = TweenMath.Linear(timeProgress,beforeMove,changeDistance,targetTime);
		else if(ttype == TweenMath.TweenType.QUAD)				
			currentValue = TweenMath.Quad(timeProgress,beforeMove,changeDistance,targetTime,etype);
		else if(ttype == TweenMath.TweenType.CUBIC)				
			currentValue = TweenMath.Cubic(timeProgress,beforeMove,changeDistance,targetTime,etype);
		else if(ttype == TweenMath.TweenType.QUART)				
			currentValue = TweenMath.Quart(timeProgress,beforeMove,changeDistance,targetTime,etype);
		else if(ttype == TweenMath.TweenType.QUINT)				
			currentValue = TweenMath.Quint(timeProgress,beforeMove,changeDistance,targetTime,etype);
		else if(ttype == TweenMath.TweenType.SINE)				
			currentValue = TweenMath.Sine(timeProgress,beforeMove,changeDistance,targetTime,etype);
		else if(ttype == TweenMath.TweenType.EXPO)				
			currentValue = TweenMath.Expo(timeProgress,beforeMove,changeDistance,targetTime,etype);
		else if(ttype == TweenMath.TweenType.CIRC)				
			currentValue = TweenMath.Circ(timeProgress,beforeMove,changeDistance,targetTime,etype);
		else if(ttype == TweenMath.TweenType.ELASTIC)			
			currentValue = TweenMath.Linear(timeProgress,beforeMove,changeDistance,targetTime);
		//				currentValue = TweenMath.Elastic(timeProgress,beforeMove,changeDistance,targetTime,,,etype);
		else if(ttype == TweenMath.TweenType.BACK)				
			currentValue = TweenMath.Back(timeProgress,beforeMove,changeDistance,targetTime,0,etype);
		else if(ttype == TweenMath.TweenType.BOUNCE)				
			currentValue = TweenMath.Bounce(timeProgress,beforeMove,changeDistance,targetTime,etype);
		else
			currentValue = TweenMath.Linear(timeProgress,beforeMove,changeDistance,targetTime);

		return currentValue;
	}

	private void MoveAbsoluteX(float progressX)
	{
		WindowMgr.MoveTransformNodeAbsolute(progressX,0,0);
		foreach(UIHeadMenuAnchorRegister a in anchors.Values)
		{
			a.SetHorizontalAbsolute((int)progressX);
		}
	}

//	private void MoveCameras(float progressX)
//	{
//		foreach(UIHeadMenuSceneCamRegister cam in cameras.Values)
//		{
////            cam.SetPositionDelta(new Vector3 (-progressX,0f,0f));
////            cam.SetLerpPosition();
//		}
//	}

    private void LerpCameras(float lerpValue)
    {
        foreach(UIHeadMenuSceneCamRegister cam in cameras.Values)
        {
            cam.SetLerpPosition(lerpValue);
        }
    }

	private void SetButtontActive(bool willActieve)
	{
		for(int i=0;i<buttonBoxes.Count;i++)
		{
			buttonBoxes[i].enabled = willActieve;
		}
	}

	void Awake () 
	{
		UIHeadMenuController.controller = this;
		Initialize();
	}
	
	void OnDestroy()
	{		
		UIHeadMenuController.controller = null;
	}

    void MoveUpwardHide()
    {
        TweenPosition.Begin(gameObject, 1f, Vector3.up * 100f);
    }

    void RevertOrgPos()
    {
        TweenPosition.Begin(gameObject, 1f, Vector3.zero);
    }
}
