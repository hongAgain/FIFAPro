using UnityEngine;


public class FIFAButtonSound : MonoBehaviour
{

    public enum EFIFAButtonType
    {
        OnYes,
        OnNo,
        OnSelect,
    }


    public EFIFAButtonType btnType = EFIFAButtonType.OnYes;

    private string acYesPath = "Audio/AudioUI/UI_BtnSelect";
    private string acNoPath = "Audio/AudioUI/UI_BtnNo";
    private string acSelectPath = "Audio/AudioUI/UI_BtnSelect";
    private GameObject objClipYes = null;
    private GameObject objClipNo = null;
    private GameObject objClipSelect = null;


#if UNITY_3_5
	public float volume = 1f;
	public float pitch = 1f;
#else
    [Range(0f, 1f)]
    public float volume = 1f;
    [Range(0f, 2f)]
    public float pitch = 1f;
#endif


    void Start()
    {
        return;
        objClipYes = Resources.Load(acYesPath) as GameObject;
        objClipNo = Resources.Load(acNoPath) as GameObject;
        objClipSelect = Resources.Load(acSelectPath) as GameObject;
    }

	void OnClick ()
	{
        
        //if (AudioMgr.Instance().IsMuteSound)
        //    return;
      
        //if (enabled && btnType == EFIFAButtonType.OnYes)
        //{
        //    LoadButtonSound(EFIFAButtonType.OnYes);
        //    PlayOneShot(objClipYes, volume, pitch);
        //}
        //else if (enabled && btnType == EFIFAButtonType.OnNo)
        //{
        //    LoadButtonSound(EFIFAButtonType.OnNo);
        //    PlayOneShot(objClipNo, volume, pitch);
        //}
        //else if (enabled && btnType == EFIFAButtonType.OnSelect)
        //{
        //    LoadButtonSound(EFIFAButtonType.OnSelect);
        //    PlayOneShot(objClipSelect, volume, pitch);
        //}
	}

    void PlayOneShot(GameObject objGame_, float volume_, float pitch_)
    {
        GameObject obj_ = Instantiate(objGame_) as GameObject;
        obj_.name = objGame_.name;
        obj_.transform.parent = WindowMgr.UIParent;

        AudioOneShot aos = obj_.GetComponent<AudioOneShot>();
        if (aos == null)
        {
            aos = obj_.AddComponent<AudioOneShot>();
        }

        aos.PlayOneShot(volume_, pitch_);

    }

    void LoadButtonSound(EFIFAButtonType type_)
    {
        switch (type_)
        {
        case EFIFAButtonType.OnYes:
                if (objClipYes == null)
                {
                    objClipYes = Resources.Load(acYesPath) as GameObject;
                }
                
        	break;
        case EFIFAButtonType.OnNo:
            if (objClipNo == null)
                {
                    objClipNo = Resources.Load(acNoPath) as GameObject;
                }
            break;
        case EFIFAButtonType.OnSelect:
            if (objClipSelect == null)
                {
                    objClipSelect = Resources.Load(acSelectPath) as GameObject;
                }
            break;
         default:
            Util.Log("ERROR..No EFIFAButtonType");
            break;
        }
    }

    void OnDestroy()
    {
        Util.CallUnloadUnusedAssets();
    }
}
