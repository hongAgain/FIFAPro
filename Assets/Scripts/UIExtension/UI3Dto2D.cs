//--------------------------------------------
//            NGUI: HUD Text
// Copyright © 2012 Tasharen Entertainment
//--------------------------------------------

using Common.Log;
using UnityEngine;

/// <summary>
/// Attaching this script to an object will make it visibly follow another object, even if the two are using different cameras to draw them.
/// </summary>

[AddComponentMenu("NGUI/Examples/UI3Dto2D")]
public class UI3Dto2D : MonoBehaviour
{
    public bool isBall;
    public Transform tfArrow;
    public Transform tfAnimation;

	/// <summary>
	/// 3D target that this object will be positioned above.
	/// </summary>

	public Transform target;

	/// <summary>
	/// Game camera to use.
	/// </summary>

	public Camera gameCamera;

	/// <summary>
	/// UI camera to use.
	/// </summary>

	public Camera uiCamera;

	/// <summary>
	/// Whether the children will be disabled when this object is no longer visible.
	/// </summary>

	public bool disableIfInvisible = true;

	Transform mTrans;
	bool mIsVisible = false;

	/// <summary>
	/// Cache the transform;
	/// </summary>

	void Awake () { mTrans = transform; }

	/// <summary>
	/// Find both the UI camera and the game camera so they can be used for the position calculations
	/// </summary>

	void Start()
	{
		if (target != null)
		{
			if (gameCamera == null) gameCamera = NGUITools.FindCameraForLayer(target.gameObject.layer);
			if (uiCamera == null) uiCamera = NGUITools.FindCameraForLayer(gameObject.layer);
            tfAnimation = target.parent.FindChild("Animation");
		}
		else
		{
            LogManager.Instance.LogError("Expected to have 'target' set to a valid transform", this);
			enabled = false;
		}
	}


	/// <summary>
	/// Update the position of the HUD object every frame such that is position correctly over top of its real world object.
	/// </summary>

	void Update ()
	{
        // 48 ,58 Court with height
        if (isBall)
        {
            mTrans.position = new Vector3(target.position.z * (1 / 48.0f), target.position.x * (1 / 58.0f), 0);
            mTrans.localPosition = new Vector3(mTrans.localPosition.x, mTrans.localPosition.y, mTrans.localPosition.z);
        } 
        else
        {
            mTrans.position = new Vector3(target.position.z * (1 / 48.0f), target.position.x * (1 / 58.0f), 0);
            tfArrow.localEulerAngles = new Vector3(0, 0, tfAnimation.localEulerAngles.y);
        }

	}


}
