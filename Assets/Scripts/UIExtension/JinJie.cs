using UnityEngine;

public class JinJie : MonoBehaviour
{
    public Color[] stages;

    public int stage = 0;

    public void Use()
    {
        if (stage < 0 || stage >= stages.Length) return;

        foreach (var renderer in GetComponentsInChildren<Renderer>(true))
        {
            Material mat = null;
            if (Application.isPlaying)
            {
                mat = renderer.material;
            }
            else
            {
                mat = renderer.sharedMaterial;
            }
            mat.SetColor("_TintColor", stages[stage]);
        }
    }
}
