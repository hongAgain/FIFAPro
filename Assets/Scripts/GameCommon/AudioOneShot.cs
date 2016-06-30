using UnityEngine;
using System.Collections;

public class AudioOneShot : MonoBehaviour 
{

    private AudioSource aSource;
    private bool isDisable = false;
    //// Use this for initialization
    void OnInit()
    {
        aSource = gameObject.GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () 
    {
	    if (!aSource.isPlaying)
	    {
            Destroy(gameObject);
	    }
	    
	}

    public void PlayOneShot(float volume_, float pitch_)
    {
        if (aSource == null)
            OnInit();
   
        aSource.volume = volume_;
        aSource.pitch = volume_;

        aSource.Play();
    }

    void OnDisable()
    {
        Destroy(gameObject);
    }
}
