//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2014 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;

/// <summary>
/// Tween the object's rotation.
/// </summary>

[AddComponentMenu("NGUI/Tween/Tween EulerAngle")]
public class TweenEulerAngle : UITweener
{
    public Vector3 from;
    public Vector3 to;

    Transform mTrans;

    public Transform cachedTransform { get { if (mTrans == null) mTrans = transform; return mTrans; } }

    /// <summary>
    /// Tween's current value.
    /// </summary>

    public Vector3 value { get { return cachedTransform.localEulerAngles; } set { cachedTransform.localEulerAngles = value; } }

    /// <summary>
    /// Tween the value.
    /// </summary>

    protected override void OnUpdate(float factor, bool isFinished)
    {
        value = Vector3.Slerp(from, to, factor);
    }

    /// <summary>
    /// Start the tweening operation.
    /// </summary>

    static public TweenEulerAngle Begin(GameObject go, float duration, Vector3 to)
    {
        TweenEulerAngle comp = UITweener.Begin<TweenEulerAngle>(go, duration);
        comp.from = comp.value;
        comp.to = to;

        if (duration <= 0f)
        {
            comp.Sample(1f, true);
            comp.enabled = false;
        }
        return comp;
    }

    [ContextMenu("Set 'From' to current value")]
    public override void SetStartToCurrentValue() { from = value; }

    [ContextMenu("Set 'To' to current value")]
    public override void SetEndToCurrentValue() { to = value; }

    [ContextMenu("Assume value of 'From'")]
    void SetCurrentValueToStart() { value = from; }

    [ContextMenu("Assume value of 'To'")]
    void SetCurrentValueToEnd() { value = to; }
}
