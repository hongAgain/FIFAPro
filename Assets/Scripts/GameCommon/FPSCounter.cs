using UnityEngine;

public class FPSCounter : MonoBehaviour
{
    public float itsFPSUpdateInterval = 0.5f;

    private float itsAccumulatedFrames = 0;
    private int itsFramesInInterval = 0;
    private float itsTimeLeft;
    private float itsCurrentFPS = 0.0f;
    private GUIStyle m_kGUIStyle = new GUIStyle();
    // Use this for initialization
    void Start ()
    {
        itsTimeLeft = itsFPSUpdateInterval;


        m_kGUIStyle = new GUIStyle ();
        m_kGUIStyle.fontSize = 24;
	}
	
	// Update is called once per frame
	void Update ()
    {
        itsTimeLeft -= Time.deltaTime;
        itsAccumulatedFrames += Time.timeScale / Time.deltaTime;
        itsFramesInInterval++;

        if (itsTimeLeft < 0.0f)
        {
            itsCurrentFPS = itsAccumulatedFrames / itsFramesInInterval;
            itsTimeLeft = itsFPSUpdateInterval;
            itsAccumulatedFrames = 0.0f;
            itsFramesInInterval = 0;
        }
	}

    void OnGUI ()
	{
		GUI.Label(new Rect(Screen.width - 70f, 0f, 70f, 50f), Screen.width + "*" + Screen.height);
		GUI.Label(new Rect(Screen.width - 130f, 15f, 70f, 50f), string.Format("<color=green>FPS:{0}</color>", itsCurrentFPS.ToString("F2")), m_kGUIStyle);
    }


}
