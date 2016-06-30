using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
[RequireComponent(typeof (UIWidget))]
public class AutoScaleBackground : MonoBehaviour {

	public const float TARGET_WIDTH = 960f;
	public const float TARGET_HEIGHT = 640f;
	private float aspect = 1f;
	private float target = 1f;

	private UIWidget bgSprite = null;

	// Use this for initialization
	void Start ()
	{
		bgSprite = GetComponent<UIWidget>();
		if(bgSprite!=null)
		{
			bgSprite.width = (int)TARGET_WIDTH;
			bgSprite.height = (int)TARGET_HEIGHT;
		}
		Cal();
	}

	public void Cal()
	{		
		aspect = Screen.width * 1f / Screen.height;
		target = aspect / TARGET_WIDTH * TARGET_HEIGHT;
		if (aspect < TARGET_WIDTH / TARGET_HEIGHT)
		{
			transform.localScale = new Vector3 (1f/target,1f/target,1f);
//			transform.localScale = new Vector3 (1f,1f/target,1f);
		}
		else
		{
			transform.localScale = new Vector3 (target*1f,target*1f,1f);
//			transform.localScale = new Vector3 (target*1f,1f,1f);
		}
	}
	#if UNITY_EDITOR
	void Update()
	{
		Cal();
	}
	#endif
}
