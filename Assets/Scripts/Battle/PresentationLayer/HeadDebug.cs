using UnityEngine;
using System.Collections;
using Common.Log;

public class HeadDebug : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	  
        if(transform.localScale!=Vector3.one)
        {
            LogManager.Instance.LogWarning("this scale=="+transform.localScale);
            
            if(m_animation!=null)
            {
                string _animationName = GetCurrentPlayingAnimationClip(m_animation.gameObject);
                LogManager.Instance.LogWarning("CurrentAnimationName==" + _animationName);
            }
        }

	}

    public static string GetCurrentPlayingAnimationClip(GameObject go)
    {

        if (go == null)
        {
            return string.Empty;
        }
        foreach (AnimationState anim in go.animation)
        {
            if (go.animation.IsPlaying(anim.name))
            {
                return anim.name;
            }
        }
        return string.Empty;
    }


    public Animation m_animation;
}
