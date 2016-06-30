using UnityEngine;
using System.Collections;

public class AutoActive : MonoBehaviour 
{
    public GameObject[] objGame;
    public int activeCount = 0;
    public float delay = 0;

    private float lastTime = 0;
    private int count;
	// Use this for initialization
	void Start () 
    {
        lastTime = Time.time;
        count = activeCount;
	}
	
	// Update is called once per frame
	void Update () 
    {
	    if (Time.time - lastTime > delay && count > 0)
	    {
            objGame[activeCount-count].SetActive(true);
            lastTime = Time.time;
            count--;
	    }
	    
	}




}
