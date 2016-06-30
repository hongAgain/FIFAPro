using UnityEngine;

public class GMCommond : MonoBehaviour
{
#if ENABLE_GMCOMMOND
	private string scriptName = "Game/GMCommond";
    private ScriptInstaller lua = new ScriptInstaller();

    private abstract class InputRecord
    {
        const float areaSizeX = 100f;
        const float areaSizeY = 100f;
        protected Rect openArea = Rect.MinMaxRect(Screen.width * 0.9f, Screen.height * 0.9f, Screen.width, Screen.height);
        public delegate void Callback();
        public Callback OnPress5Seconds;

        private float mPressTime = 0f;

        public abstract void Update();

        public void AddPressTime(float t)
        {
            mPressTime += t;
            if (mPressTime >= 3f)
            {
                mPressTime = 0f;
                if (OnPress5Seconds != null)
                {
                    OnPress5Seconds();
                }
            }
        }
    }

    private class TouchInputRecord : InputRecord
    {
        public override void Update()
        {
            if (Input.touchCount > 0)
            {
                Touch t = Input.GetTouch(0);
                if (openArea.Contains(t.position))
                {
                    AddPressTime(t.deltaTime);
                }
            }
        }
    }

    private class MouseInputRecord : InputRecord
    {
        private float lastPressTime;
        public override void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                lastPressTime = Time.realtimeSinceStartup;
            }
            if (Input.GetMouseButton(0))
            {
                Vector2 v = Input.mousePosition;
                if (openArea.Contains(new Vector2(v.x, Screen.height - v.y)))
                {
                    AddPressTime(Time.realtimeSinceStartup - lastPressTime);
                    lastPressTime = Time.realtimeSinceStartup;
                }
            }
        }
    }

    private class NoneInputRecord : InputRecord
    {
        public override void Update()
        {
        }
    }

    private InputRecord mInputRecord;

    void Start()
    {
        lua.Install(LuaScriptMgr.Instance, Util.GetLuaFilePath(scriptName));
#if (UNITY_ANDROID && UNITY_IPHONE) && !UNITY_EDITOR
        mInputRecord = new TouchInputRecord();
#elif UNITY_EDITOR
        mInputRecord = new MouseInputRecord();
#else
        mInputRecord = new NoneInputRecord();
#endif
        mInputRecord.OnPress5Seconds = EnableGM;
    }

    void Update()
    {
        mInputRecord.Update();
    }

    void OnGUI()
    {
        LuaScriptMgr.Instance.CallLuaFunction(Util.GetLuaScriptModuleName(scriptName) + "OnGUI");
    }

    void EnableGM()
    {
        LuaScriptMgr.Instance.CallLuaFunction(Util.GetLuaScriptModuleName(scriptName) + "Enable");
    }
#endif
}