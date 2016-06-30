using UnityEngine;
using System.Collections;

public class CameraAutoPosition : MonoBehaviour {

	public float aspectMin = 4/3f;
	public float aspectMax = 2/1f;
	private Vector3 camPosMin = new Vector3(3.04f,-0.23f,-0.5f);
	private Vector3 camPosMax = new Vector3(4f,-0.17f,-0.44f);

	// Use this for initialization
	void Awake () {
		Initialize();
	}
	
	private void Initialize()
	{
		//lerp camera position and paramLerpOffsetCamera, using screen ratio
		float aspect = Screen.width * 1f / Screen.height;

		float lerpValue = (aspect - aspectMin)/(aspectMax - aspectMin);		

		transform.localPosition = lerpValue*(camPosMax-camPosMin)+camPosMin;
	}
}
