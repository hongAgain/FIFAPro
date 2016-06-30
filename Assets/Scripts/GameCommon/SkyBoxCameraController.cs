using UnityEngine;
using System.Collections;

public class SkyBoxCameraController : UIHeadMenuSceneCamRegister {

    //camera params
    public Vector3 gloPos;
    public Quaternion gloRot;
    public Vector3 gloScale;
    
    public CameraClearFlags clearFlags;
    public Color backgroundColor;
    public int cullingMask;
    public float fieldOfView;
    public float nearClipPlane;
    public float farClipPlane;

    public bool needAutoPos = false;
    public Vector3 camAutoPosMin = new Vector3(3.04f,-0.23f,-0.5f);
    public Vector3 camAutoPosMax = new Vector3(4f,-0.17f,-0.44f);
    
    //register params
    private bool isRegistered = false;
    private int cameraID = 0;
    
    private float aspectMin = 4/3f;
    private float aspectMax = 2/1f;

    // Use this for initialization
    void Start () {                
        base.Start();
    }

    public void SetAutoPosition()
    {
        //lerp camera position and paramLerpOffsetCamera, using screen ratio
        float aspect = Screen.width * 1f / Screen.height;
        float lerpValue = (aspect - aspectMin)/(aspectMax - aspectMin);
        transform.localPosition = lerpValue*(camAutoPosMax-camAutoPosMin)+camAutoPosMin;
    }
}
