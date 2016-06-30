using System.Collections.Generic;
using UnityEngine;

public class LuaTimer : MonoBehaviour
{
    public string scriptName = "Common/LuaTimer";
    private ScriptInstaller lua = new ScriptInstaller();
    private string moduleName;

    public delegate void OnTimer();
    private Dictionary<long, Element> mElements = new Dictionary<long, Element>();
    private List<long> mTempDel = new List<long>(); 

    private long StartID = 0;

    private class Element
    {
        public long mId;
        public float mElasped = 0f;
        public bool mIgnoreTimeScale = true;
        public float mDuration = 0f;
        public OnTimer mOnTimer;

		public bool OnTimePassed(float deltaTime)
        {
            var dur = mDuration;
		    if (mDuration < 0)
		    {
		        dur = -mDuration;
		    }

		    mElasped = mElasped + deltaTime;

			if (mElasped >= dur)
            {
                if (mOnTimer != null)
                {
                    mOnTimer();
                }

                if (mDuration > 0)
                {
                    return true;
                }
                else
                {
                    mElasped = mElasped - dur;
                    return false;
                }
            }
			else
			{
                return false;
			}
		}
    }

    public static LuaTimer Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            LogView.Error("LuaTimer is singleton!");
            Destroy(gameObject);
        }
    }

    void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    void Start()
    {
        lua.Install(LuaScriptMgr.Instance, Util.GetLuaFilePath(scriptName));
        moduleName = Util.GetLuaScriptModuleName(scriptName);
    }

	// Update is called once per frame
	void Update ()
    {
        LuaScriptMgr.Instance.CallLuaFunction(moduleName + "Update", Time.time, Time.realtimeSinceStartup);

	    foreach (var kvp in mElements)
	    {
	        var delta = kvp.Value.mIgnoreTimeScale ? RealTime.deltaTime : RealTime.time;
	        if (kvp.Value.OnTimePassed(delta))
	        {
	            mTempDel.Add(kvp.Key);
	        }
	    }

	    foreach (var id in mTempDel)
	    {
	        mElements.Remove(id);
	    }
	    mTempDel.Clear();
    }

    public long AddTimer(float dur, bool ignoreTimeScale, OnTimer onTimer)
    {
        var e = new Element { mId = NextID(), mDuration = dur, mIgnoreTimeScale = ignoreTimeScale, mOnTimer = onTimer };
        mElements.Add(e.mId, e);
        return e.mId;
    }

    public void RmvTimer(long id)
    {
        mElements.Remove(id);
    }

    private long NextID()
    {
        return ++StartID;
    }
}