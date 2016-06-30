using UnityEngine;
using System.Collections;
using Common.Log;

public class CameraBluriOS : CameraBlurBase {
		
	public string BlurShaderName = "Custom/CameraBluriOS";
	public Shader BlurBoxShader = null;	
	[Range(0.0f, 1.0f)]
	public float BlurSize = 0.5f;
	public int ScaleFactor = 4;
	
	private Material _BlurBoxMaterial = null;
	private Material BlurBoxMaterial
	{
		get
		{
			if(_BlurBoxMaterial == null)
			{
				_BlurBoxMaterial = new Material(BlurBoxShader);
				_BlurBoxMaterial.hideFlags = HideFlags.HideAndDontSave;	
			}
			return _BlurBoxMaterial;
		}
	}
	
	private System.Action delegateOnLerpBlurSizeOver = null;
	
	public override void Initialize() 
	{
		if(BlurBoxShader==null)
			BlurBoxShader = Shader.Find(BlurShaderName);		
		// Disable if we don't support image effects
		if (!SystemInfo.supportsImageEffects)
		{			
			LogManager.Instance.LogError ("SystemInfo.supportsImageEffects tells that graphics card of current device doesn't support post image effects");
			enabled = false;
			return;
		}		
		// Disable the image effect if the shader can't run on the users graphics card
		if (!BlurBoxShader || !BlurBoxShader.isSupported) {
            LogManager.Instance.LogError ("BlurBoxShader.isSupported tells that the shader can't run on the users graphics card");
			enabled = false;
		}
		
		WindowMgr.UICamBlur = this;
		
		enabled = false;
		return;		
	}	
	
	public override void LerpBlurUp(float timeLength = 0.2f, System.Action OnLerpOver = null)
	{
		delegateOnLerpBlurSizeOver = null;
		delegateOnLerpBlurSizeOver += OnLerpOver;
		if (ScaleFactor < 32) 
		{
			StartCoroutine (LerpBlur(ScaleFactor,32,timeLength*(32-ScaleFactor)/(32-4)));
		}
		else
		{
			//watch if it need to reset to end value
			//it's over
			LerpOver();
		}
	}
	
	public override void LerpBlurDown(float timeLength = 0.2f, System.Action OnLerpOver = null)
	{
		delegateOnLerpBlurSizeOver = null;
		delegateOnLerpBlurSizeOver += OnLerpOver;
		if (ScaleFactor > 4f) 
		{
			StartCoroutine (LerpBlur (ScaleFactor, 4, timeLength*(ScaleFactor-4)/(32-4)));
		}
		else 
		{
			//watch if it need to reset to end value
			//it's over
			LerpOver();
		}
	}
	
//	public override void LerpBlurTo(float scaleFrom,float scaleTo,float timeLength = 0.2f, System.Action OnLerpOver = null)
//	{
//		delegateOnLerpBlurSizeOver = null;
//		delegateOnLerpBlurSizeOver += OnLerpOver;
//		StartCoroutine (LerpBlur(scaleFrom,scaleTo,timeLength));
//	}
	
	IEnumerator LerpBlur(float scaleFrom,float scaleTo,float timeLength)
	{
		float timeProgress = 0f;		
		float beforeMove = scaleFrom;
		float distance = scaleTo - scaleFrom;
		
		while (timeLength-timeProgress >= 0.01f) 
		{
			timeProgress = Mathf.Clamp (timeProgress + Time.deltaTime, 0, timeLength);
			//lerp it here
			ScaleFactor = (int)TweenMath.Quart(timeProgress,beforeMove,distance,timeLength,TweenMath.EaseType.EASEOUT);				
			yield return null;
		}
		
		// fix to end here
		ScaleFactor = (int)scaleTo;
		
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
		if (ScaleFactor <= 4.005f)
		{
			ScaleFactor = 4;
			enabled = false;
		}
	}
		
	#region codes for blur effect
	void OnRenderImage (RenderTexture sourceTexture, RenderTexture destTexture)
    {
        //FourTapCone(sourceTexture, destTexture,3);
        if (BlurSize != 0 && BlurBoxShader != null)
        {
            int rtW = sourceTexture.width / ScaleFactor;
            int rtH = sourceTexture.height / ScaleFactor;
            RenderTexture buffer = RenderTexture.GetTemporary(rtW, rtH, 0);
            DownSample4x(sourceTexture, buffer);
            for (int i = 0; i < 2; i++)
            {
                RenderTexture buffer2 = RenderTexture.GetTemporary(rtW, rtH, 0);
                FourTapCone(buffer, buffer2, i);
                RenderTexture.ReleaseTemporary(buffer);
                buffer = buffer2;
            }
            Graphics.Blit(buffer, destTexture);
            RenderTexture.ReleaseTemporary(buffer);
        }
        else
        {
            //			Graphics.Blit (sourceTexture, destTexture);
        }
    }
	
	private void FourTapCone (RenderTexture source, RenderTexture dest,int iteration)
	{
		float off = BlurSize*iteration+0.5f;
		Graphics.BlitMultiTap (source, dest, BlurBoxMaterial,
		                       new Vector2(-off, -off),
		                       new Vector2(-off,  off),
		                       new Vector2( off,  off),
		                       new Vector2( off, -off)
		                       );
	}
	
	private void DownSample4x (RenderTexture source, RenderTexture dest)
	{
		float off = 1.0f;
		// Graphics.Blit(source, dest, material);
		Graphics.BlitMultiTap (source, dest, BlurBoxMaterial,
		                       new Vector2(off, off),
		                       new Vector2(-off,  off),
		                       new Vector2( off,  off),
		                       new Vector2( off, -off)
		                       );
	}
	#endregion
	
	public void OnDisable ()
	{
		if (_BlurBoxMaterial)
			DestroyImmediate (_BlurBoxMaterial);
	}
}