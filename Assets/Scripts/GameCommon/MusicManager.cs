using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MusicManager : MonoBehaviour 
{
    private Hashtable sounds = new Hashtable();

    void Add(string key, AudioClip value) {
        if (sounds[key] != null || value == null) return;
        sounds.Add(key, value);
    }


    AudioClip Get(string key) { 
        if (sounds[key] == null) return null;
        return sounds[key] as AudioClip;
    }


    public AudioClip LoadAudioClip(string path) 
	{
        AudioClip ac = Get(path);
        if (ac == null) { 
            ac = (AudioClip)Resources.Load(path, typeof(AudioClip));
            Add(path, ac);
        }
        return ac;
    }


    public bool CanPlayBackSound() {
        string key = Const.AppPrefix + "BackSound";
        int i = PlayerPrefs.GetInt(key, 1);
        return i == 1;
    }

    public void PlayBacksound(string name, bool canPlay) {
        if (audio.clip != null) 
		{
            if (name.IndexOf(audio.clip.name) > -1) {
                if (!canPlay) {
                    audio.Stop();
                    audio.clip = null;
					Util.CallUnloadUnusedAssets();
                }
                return;
            }
        }
        if (canPlay) {
            audio.loop = true;
            audio.clip = LoadAudioClip(name);
            audio.Play();
        } else {
            audio.Stop();
            audio.clip = null;
            Util.CallUnloadUnusedAssets();
        }
    }

    public bool CanPlaySoundEffect() {
        string key = Const.AppPrefix + "SoundEffect";
        int i = PlayerPrefs.GetInt(key, 1);
        return i == 1;
    }


    public void Play (AudioClip clip, Vector3 position) {
        if (!CanPlaySoundEffect()) return;
        AudioSource.PlayClipAtPoint(clip, position);
    }
}
