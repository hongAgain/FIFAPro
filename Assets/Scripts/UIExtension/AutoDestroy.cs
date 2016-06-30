using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    public bool onStart = false;
    public bool onEnable = false;
    public float delay = 0f;

    private float t = 0f;

	// Use this for initialization
	void Start ()
    {
	    if (onStart)
	    {
	        t = Time.time;
	    }
	}

    void OnEnable()
    {
        if (onEnable)
        {
            t = Time.time;
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
	    if (t + delay <= Time.time)
	    {
	        Destroy(gameObject);
	    }
	}
}