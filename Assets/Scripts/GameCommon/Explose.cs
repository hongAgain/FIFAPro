using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(UIPanel))]
public class Explose : MonoBehaviour
{
    public float from = 1f;
    public float to = 1f;
    public float duration = 1f;
    public float delay = 0f;
    public bool autoStart = true;

    private UIPanel panel = null;

    private bool inTween = false;

    void Start()
    {
        panel = GetComponent<UIPanel>();

        if (autoStart)
        {
            Begin();
        }
    }

    [ContextMenu("Tween")]
    void Begin()
    {
        if (inTween == false)
        {
            inTween = true;
            StartCoroutine(Tween());
        }
    }

    IEnumerator Tween()
    {
        float beginAt = Time.time;

        while (Time.time <= beginAt + duration + delay)
        {
            //Debug.Log(string.Format("{0} | {1} | {2}", Time.time, beginAt, delay));
            if (Time.time >= beginAt + delay)
            {
                float rate = Mathf.Lerp(from, to, (Time.time - beginAt - delay / duration));
                foreach (var dc in panel.drawCalls)
                {
                    var mat = dc.dynamicMaterial;
                    if (mat.HasProperty("_Rate"))
                    {
                        mat.SetFloat("_Rate", rate);
                    }
                }

                //BroadcastMessage("MarkAsChanged");
                //panel.Refresh();
            }

            yield return null;
        }

        inTween = false;
    }
}