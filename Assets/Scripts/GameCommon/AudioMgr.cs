using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioMgr : MonoBehaviour 
{
    public enum EAudioType
    {
        NoneAudio = 0,
        EventAudio = 1,
        BackgroudAudio = 2,
        OtherAudio = 3,
    }

    private string m_keyMusicVolume = "MusicVolume";
    private string m_keySoundVolume = "SoundVolume";
    private string m_strBGAudioPath = "Audio/AudioBG/";
    private string m_strUIAudioPath = "Audio/AudioUI/";

    private List<GameObject> m_bgMusic = new List<GameObject>();
    private List<GameObject> m_eventMusic = new List<GameObject>();

    private bool isMuteMusic = false;
    public bool IsMuteMusic
    {
        get { return isMuteMusic; }
        set { isMuteMusic = value; }
    }
    private bool isMuteSound = false;
    public bool IsMuteSound
    {
        get { return isMuteSound; }
        set { isMuteSound = value; }
    }

    private string currBGMusicName;
    public string CurrBGMusicName
    {
        get { return currBGMusicName; }
    }

    private float m_musicVolume;
    public float MusicVolume
    {
        get { return m_musicVolume; }
        set
        {
            m_musicVolume = value;
            PlayerPrefs.SetFloat(m_keyMusicVolume, m_musicVolume);
        }
    }

    private float m_soundVolume;
    public float SoundVolume
    {
        get { return m_soundVolume; }
        set
        {
            m_soundVolume = value;
            PlayerPrefs.SetFloat(m_keySoundVolume, m_soundVolume);
        }
    }

    private static AudioMgr m_audioMgr;
	// Use this for initialization
	void Start () 
    {
        m_audioMgr = this;

        if (PlayerPrefs.HasKey(m_keyMusicVolume))
        {
            m_musicVolume = PlayerPrefs.GetFloat(m_keyMusicVolume);
        }
        else
        {
            MusicVolume = 0.6f;
        }

        if (PlayerPrefs.HasKey(m_keySoundVolume))
        {
            m_soundVolume = PlayerPrefs.GetFloat(m_keySoundVolume);
        }
        else
        {
            SoundVolume = 1.0f;
        }
	}

    public static AudioMgr Instance()
    {
        return m_audioMgr;
    }

    public void PlayBGMusic(string name_)
    {
        if (IsMuteMusic)
            return;
        if (string.Equals(currBGMusicName, name_))
            return;

        GameObject objGame_ = Resources.Load(m_strBGAudioPath + name_) as GameObject;
        if (objGame_ != null)
        {
            GameObject bgMusicTemp = Instantiate(objGame_) as GameObject;
            bgMusicTemp.name = name_;
            bgMusicTemp.transform.parent = WindowMgr.UIParent;
            bgMusicTemp.GetComponent<AudioSource>().volume = m_musicVolume;

            m_bgMusic.Add(bgMusicTemp);
            currBGMusicName = name_;
        }
    }

    public void PauseBGMusic(string name_, bool isPause_)
    {
        foreach (GameObject obj in m_bgMusic)
        {
            if (obj.name == name_)
            {
                AudioSource aSource = obj.GetComponent<AudioSource>();
                if(aSource != null)
                {
                    if (isPause_)
                    {
                        aSource.Pause();
                    } 
                    else
                    {
                        aSource.Play();
                    }
                }
            }
            
        }
    }



    public void DestroyBGMusic(string name_)
    {
        foreach (GameObject obj in m_bgMusic)
        {
            if(obj.name == name_)
            {
                m_bgMusic.Remove(obj);
                Destroy(obj);
                break;
            }
        }

        Util.CallUnloadUnusedAssets();
    }



}
