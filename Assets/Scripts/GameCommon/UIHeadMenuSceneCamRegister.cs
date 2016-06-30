using UnityEngine;
using System.Collections;

public class UIHeadMenuSceneCamRegister : MonoBehaviour {

	private bool isRegistered = false;
	private int cameraID = 0;

    public Vector3 originalPos = new Vector3(3.04f,-0.23f,-0.5f);
    public Vector3 switchedDelta = new Vector3(4f,-0.17f,-0.44f);

	// Use this for initialization
	protected void Start () {
		//go register
		cameraID = UIHeadMenuController.RegisterCamera(this);
		if(cameraID==-1)
		{
			isRegistered = false;
			StartCoroutine(WaitToRegister(1f));
			return;
		}
		isRegistered = true;
	}

    public void CheckAndRegister()
    {
        //go register
        if(!isRegistered || cameraID==-1)
        {
            cameraID = UIHeadMenuController.RegisterCamera(this);
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
			cameraID = UIHeadMenuController.RegisterCamera(this);
			if(cameraID != -1)
			{
				isRegistered = true;
				break;
			}
			yield return null;
		}
	}
	
//	public void SetPositionDelta(Vector3 deltaPos)
//	{
//		transform.localPosition = originalPos + deltaPos;
//	}

    public void SetLerpPosition(float lerpValue)
    {
        transform.localPosition = originalPos + switchedDelta * lerpValue;
    }
	
	void OnDestroy()
	{
		if(isRegistered)
		{
			UIHeadMenuController.UnregisterCamera(cameraID);
			isRegistered = false;
		}
	}
}
