using Common.Log;
using UnityEngine;

[ExecuteInEditMode]
public class CameraSaver : MonoBehaviour
{
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
    public Vector3 camPosMin = new Vector3(3.04f,-0.23f,-0.5f);
    public Vector3 camPosMax = new Vector3(4f,-0.17f,-0.44f);

//    public Vector3 originalPos = new Vector3(3.04f,-0.23f,-0.5f);
    public Vector3 switchedDelta = new Vector3(4f,-0.17f,-0.44f);

    public AnimationClip[] cameraAnims;

    private static Camera skyBoxCam;

    void OnEnable()
    {
        FindCamera();

        if (skyBoxCam != null)
        {
            if (Application.isPlaying)
            {
                //Read params
                skyBoxCam.transform.position    = gloPos;
                skyBoxCam.transform.rotation    = gloRot;
                skyBoxCam.transform.localScale  = gloScale;
                skyBoxCam.clearFlags            = clearFlags;
                skyBoxCam.backgroundColor       = backgroundColor;
                skyBoxCam.cullingMask           = cullingMask;
                skyBoxCam.fieldOfView           = fieldOfView;
                skyBoxCam.nearClipPlane         = nearClipPlane;
                skyBoxCam.farClipPlane          = farClipPlane;

                if (cameraAnims != null && cameraAnims.Length > 0)
                {
                    var anim = skyBoxCam.gameObject.animation;
                    if (anim == null) anim = skyBoxCam.gameObject.AddComponent<Animation>();
                    for (int i = 0; i < cameraAnims.Length; i++)
                    {
                        if (anim.GetClip(cameraAnims[i].name) == null)
                        {
                            anim.AddClip(cameraAnims[i], cameraAnims[i].name);
                        }
                    }
                }

                SkyBoxCameraController sbcc = skyBoxCam.gameObject.AddMissingComponent<SkyBoxCameraController>();
                if(needAutoPos)
                {
                    sbcc.camAutoPosMin = camPosMin;
                    sbcc.camAutoPosMax = camPosMax;
                    sbcc.SetAutoPosition();
                }
                sbcc.originalPos = skyBoxCam.transform.position;
                sbcc.switchedDelta = switchedDelta;
            }
            else
            {
            }
        }
    }

    void FindCamera()
    {
        if (skyBoxCam == null)
        {
            GameObject cameraGO = GameObject.FindGameObjectWithTag("MainCamera");
            if (cameraGO != null)
            {
                skyBoxCam = cameraGO.camera;
            }
        }
    }

    [ContextMenu("Save Params")]
    void Save()
    {
        FindCamera();

        if (skyBoxCam != null)
        {
            //Write params
            gloPos          = skyBoxCam.transform.position;
            gloRot          = skyBoxCam.transform.rotation;
            gloScale        = skyBoxCam.transform.localScale;
            clearFlags      = skyBoxCam.clearFlags;
            backgroundColor = skyBoxCam.backgroundColor;
            cullingMask     = skyBoxCam.cullingMask;
            fieldOfView     = skyBoxCam.fieldOfView;
            nearClipPlane   = skyBoxCam.nearClipPlane;
            farClipPlane    = skyBoxCam.farClipPlane;
            if (skyBoxCam.animation != null)
            {
                int count = skyBoxCam.animation.GetClipCount();
                cameraAnims = new AnimationClip[count];
            }
        }
        else
        {
            LogManager.Instance.YellowLog("Can't find skybox camera!");
        }
    }
}