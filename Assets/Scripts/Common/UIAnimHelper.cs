using UnityEngine;

public class UIAnimHelper : MonoBehaviour
{
    public UITweener[] activeOnEnable;

    void OnEnable()
    {
        if (activeOnEnable != null)
        {
            for (int i = 0; i < activeOnEnable.Length; i++)
            {
                activeOnEnable[i].enabled = true;
                activeOnEnable[i].ResetToBeginning();
            }
        }
    }
}