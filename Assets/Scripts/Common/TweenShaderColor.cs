using UnityEngine;

public class TweenShaderColor : UITweener
{
    public string colorName = "_TintColor";
    public Color to;
    private Color from;

    private Material clone;

    protected override void Start()
    {
        clone = renderer.material;
        from = clone.GetColor(colorName);

        base.Start();
    }

    protected override void OnUpdate(float factor, bool isFinished)
    {
        if (clone != null)
        {
            Color lerp = Color.Lerp(from, to, factor);
            clone.SetColor(colorName, lerp);
        }
    }

    void OnDisable()
    {
        if (clone != null && gameObject.activeInHierarchy == false)
        {
            clone.SetColor(colorName, from);
        }
    }

    void OnDestroy()
    {
        Object.Destroy(clone);
        clone = null;
    }
}