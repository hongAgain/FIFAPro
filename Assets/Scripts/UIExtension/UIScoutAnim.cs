using AnimationOrTween;
using LuaInterface;
using UnityEngine;

public class UIScoutAnim : MonoBehaviour
{
    public float delay = 10f;

    private ActiveAnimation mActiveAnimation;
    private LuaFunction mShowImportant;
    private LuaFunction mHideImportant;

    private float? mPausePoint = null;

    public static void BeginAnimate(GameObject go, string clipName, LuaTable importantIdx, LuaTable rootNames, LuaFunction showImportant, LuaFunction hideImportant)
    {
        var activeAnimation = go.GetComponent<ActiveAnimation>();

        if (activeAnimation != null)
        {
            activeAnimation.Reset();
        }

        foreach (var key in importantIdx.Keys)
        {
            var rootName = rootNames[key].ToString();
            var childGo = go.transform.Find(rootName).gameObject;
            var notifier = childGo.GetComponent<UIActiveNotifier>();
            if ((bool)importantIdx[key])
            {
                if (notifier == null) notifier = childGo.AddComponent<UIActiveNotifier>();
                notifier.target = go;
                notifier.message = "Pause";
                notifier.data = key;
            }
            else
            {
                if (notifier != null)
                {
                    Object.Destroy(notifier);
                }
            }
        }

        var scoutAnim = go.GetComponent<UIScoutAnim>();
        if (scoutAnim == null) scoutAnim = go.AddComponent<UIScoutAnim>();

        scoutAnim.mActiveAnimation = ActiveAnimation.Play(go.animation, clipName, Direction.Forward);
        scoutAnim.mShowImportant = showImportant;
        scoutAnim.mHideImportant = hideImportant;
    }

    void Pause(object data)
    {
        mActiveAnimation.enabled = false;

        int idx = int.Parse(data.ToString());
        mShowImportant.Call(idx);

        mPausePoint = Time.unscaledTime;
    }

    void Update()
    {
        if (mPausePoint.HasValue)
        {
            if (mPausePoint.Value + delay <= Time.unscaledTime)
            {
                mPausePoint = null;
                mActiveAnimation.enabled = true;
                mHideImportant.Call();
            }
        }
    }

    void OnClick()
    {
        if (mPausePoint.HasValue)
        {
            mPausePoint = null;
            mActiveAnimation.enabled = true;
            mHideImportant.Call();
        }
    }
}