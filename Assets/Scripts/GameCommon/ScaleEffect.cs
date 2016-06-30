using UnityEngine;
using System.Collections;

public class ScaleEffect : MonoBehaviour 
{
    public GameObject[] m_nodeEffect;
    public float m_openDuration = 0;
    public float m_closeDuration = 0;
    //// Use this for initialization
    //void Start () 
    //{
	
    //}

    public void DoOpenScaleEffect()
    {
        for (int i = 0; i< m_nodeEffect.Length;++i )
        {
            TweenScale ts = m_nodeEffect[i].GetComponent<TweenScale>();
            if (ts != null)
            {
                if (m_openDuration != 0)
                    ts.duration = m_openDuration;
                ts.PlayForward();
            }
        }
        
    }

    public void DoCloseScaleEffect()
    {
        for (int i = 0; i < m_nodeEffect.Length; ++i)
        {
            TweenScale ts = m_nodeEffect[i].GetComponent<TweenScale>();
            if (ts != null)
            {
                if (m_closeDuration != 0)
                    ts.duration = m_closeDuration;
                ts.PlayReverse();
            }
        }
    }

}
