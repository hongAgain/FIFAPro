using UnityEngine;
using System.Collections;

[RequireComponent(typeof (UIWidget))]
public class UIHeadMenuAnchorRegister : MonoBehaviour {

	private UIWidget widget = null;
	private bool isRegistered = false;
	private int anchorID = 0;
	
	private int absoluteRightAnchor = 0;
	private int absoluteLeftAnchor = 0;

	// Use this for initialization
	void Start () {
		widget = GetComponent<UIWidget>();
		if(widget!=null && widget.isAnchored)
		{
			absoluteRightAnchor = widget.rightAnchor.absolute;
			absoluteLeftAnchor = widget.leftAnchor.absolute;
			//go register
			anchorID = UIHeadMenuController.RegisterAnchor(this);
			if(anchorID==-1)
			{
				isRegistered = false;
//				Debug.LogError("wait to Register");
				StartCoroutine(WaitToRegister(1f));
				return;
			}
			isRegistered = true;
		}
	}

	IEnumerator WaitToRegister(float timeLimit = 1f)
	{
		float timeSpent = 0f;
		while(!isRegistered && timeSpent <= timeLimit)
		{
			timeSpent += Time.deltaTime;

			//go register
			anchorID = UIHeadMenuController.RegisterAnchor(this);
			if(anchorID != -1)
			{
//				Debug.LogError("Registered");
				isRegistered = true;
				break;
			}
			yield return null;
		}
	}

	public void SetHorizontalDelta(int delta)
	{
		widget.rightAnchor.absolute += delta;
		widget.leftAnchor.absolute += delta;
	}

	public void SetHorizontalAbsolute(int absolute)
	{
		widget.rightAnchor.absolute = absoluteRightAnchor + absolute;
		widget.leftAnchor.absolute = absoluteLeftAnchor + absolute;
	}
	
	void OnDestroy()
	{
		if(isRegistered)
		{
			UIHeadMenuController.UnregisterAnchor(anchorID);
			isRegistered = false;
		}
	}
}
