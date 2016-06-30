using UnityEngine;

[ExecuteInEditMode]
public class MultiScreen : MonoBehaviour
{
    public const float TARGET_ASPECT = 1.5f;    //960 X 640

	// Use this for initialization
	void Start ()
    {
        Cal();
    }
    
    void Cal()
    {
        int width = Screen.width;
        int height = Screen.height;
        float aspect = width * 1f / height;

        if (aspect < TARGET_ASPECT)
        {
            //Adjust based on width
            transform.localScale = Vector3.one * aspect / TARGET_ASPECT;
        }
        else
        {
            /* Adjust based on height
             * NGUI already do it
             * So we do nothing here!
             */
			transform.localScale = Vector3.one;
        }
	}

#if UNITY_EDITOR
    void Update()
    {
        if (Application.isPlaying == false)
        {
            Cal();
        }
    }
#endif
}