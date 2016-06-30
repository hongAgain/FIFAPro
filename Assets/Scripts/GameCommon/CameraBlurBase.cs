using UnityEngine;
using System.Collections;
using System.Diagnostics;

public class CameraBlurBase : MonoBehaviour 
{
	public virtual void  Initialize() {}	
	public virtual void LerpBlurUp(float timeLength = 0.2f, System.Action OnLerpOver = null) {}	
	public virtual void LerpBlurDown(float timeLength = 0.2f, System.Action OnLerpOver = null) {}
//	public virtual void LerpBlurTo(float scaleFrom,float scaleTo,float timeLength = 0.2f, System.Action OnLerpOver = null) {}

}

public class CameraBlur 
{
  //  [Conditional("UNITY_IOS" ),Conditional("UNITY_IPHONE")]
    protected static void InitBlurIOS(Transform kTarget)
    {
        m_kBlurObj = kTarget.gameObject.AddMissingComponent<CameraBluriOS>();
    }

    //[Conditional("UNITY_ANDROID"),Conditional("UNITY_EDITOR")]
    //protected static void InitBlurAndroid(Transform kTarget)
    //{
    //    m_kBlurObj = kTarget.gameObject.AddMissingComponent<CameraBlurAndroid>();
    //}
	public static CameraBlurBase GetCameraBlurScript(Transform targetCamera)
	{
        InitBlurIOS(targetCamera);
     //   InitBlurAndroid(targetCamera);
        if(null != m_kBlurObj)
            m_kBlurObj.Initialize();
        return m_kBlurObj;
	}

    private static CameraBlurBase m_kBlurObj = null;
}