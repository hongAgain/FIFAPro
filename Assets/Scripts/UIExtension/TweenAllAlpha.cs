using UnityEngine;

public class TweenAllAlpha : UITweener
{
    bool mCached = false;
    UIRect[] mRects;

    public UIRect[] exceptArr;

    public float alpha = 0f;

    void Cache()
    {
        mCached = true;
        mRects = GetComponentsInChildren<UIRect>();

        if (exceptArr != null)
        {
            for (int i = 0; i < mRects.Length; i++)
            {
                for (int j = 0; j < exceptArr.Length; j++)
                {
                    if (mRects[i] == exceptArr[j])
                    {
                        mRects[i] = null;
                    }
                }
            }
        }
    }
    
    /// <summary>
    /// Tween the value.
    /// </summary>

    protected override void OnUpdate(float factor, bool isFinished)
    {
        if (mCached == false) Cache();

        if (mRects != null)
        {
            foreach (var rect in mRects)
            {
                if (rect != null)
                {
                    rect.alpha = alpha;
                }
            }
        }
    }
}