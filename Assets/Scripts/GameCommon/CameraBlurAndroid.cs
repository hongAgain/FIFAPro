using UnityEngine;
using System.Collections;

public class CameraBlurAndroid : CameraBlurBase {

	private System.Action delegateOnLerpBlurSizeOver = null;

	public override void Initialize() 
	{				
		WindowMgr.UICamBlur = this;		
		enabled = false;
		return;		
	}	
	
	public override void LerpBlurUp(float timeLength = 0.2f, System.Action OnLerpOver = null)
	{
		delegateOnLerpBlurSizeOver = null;
		delegateOnLerpBlurSizeOver += OnLerpOver;
		LerpOver();
	}
	
	public override void LerpBlurDown(float timeLength = 0.2f, System.Action OnLerpOver = null)
	{
		delegateOnLerpBlurSizeOver = null;
		delegateOnLerpBlurSizeOver += OnLerpOver;
		StartCoroutine (LerpBlur (timeLength));
		
	}
	
//	public override void LerpBlurTo(float scaleFrom,float scaleTo,float timeLength = 0.2f, System.Action OnLerpOver = null)
//	{
//		delegateOnLerpBlurSizeOver = null;
//		delegateOnLerpBlurSizeOver += OnLerpOver;
//		StartCoroutine (LerpBlur(timeLength));
//	}
	
	IEnumerator LerpBlur(float timeLength)
	{
		float timeProgress = 0f;		
		while (timeLength-timeProgress >= 0.01f) 
		{
			timeProgress = Mathf.Clamp (timeProgress + Time.deltaTime, 0, timeLength);
			yield return null;
		}		
		LerpOver();
	}
	
	private void LerpOver()
	{
		if(delegateOnLerpBlurSizeOver!=null)
			delegateOnLerpBlurSizeOver();
		delegateOnLerpBlurSizeOver = null;
		
		CheckToDisable ();
	}
	
	private void CheckToDisable()
	{

	}
}