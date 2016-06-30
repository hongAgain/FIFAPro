using UnityEngine;

[RequireComponent(typeof(UIEventListener))]
public class CallLua : MonoBehaviour
{
    public string luaFile = "";

    public bool baseOnClick = false;
    public bool baseOnPress = false;
    public bool baseOnDragStart = false;
    public bool baseOnDrag = false;
    public bool baseOnDragOver = false;
    public bool baseOnDragOut = false;
    public bool baseOnDragEnd = false;
    public bool baseOnDrop = false;

	void Start ()
    {
	    if (LuaScriptMgr.Instance == null)
	    {
	        new LuaScriptMgr().Start();
	    }

	    LuaScriptMgr.Instance.DoFile(luaFile);

	    string fileName = Util.GetLuaScriptModuleName(luaFile);
	    UIEventListener listener = GetComponent<UIEventListener>();
	    if (baseOnClick)
	    {
	        listener.onClick = delegate(GameObject go)
	        {
                LuaScriptMgr.Instance.CallLuaFunction(fileName + "onClick", go);
	        };
	    }

	    if (baseOnPress)
	    {
	        listener.onPress = delegate(GameObject go, bool state)
	        {
                LuaScriptMgr.Instance.CallLuaFunction(fileName + "onPress", go, state);
	        };
	    }

	    if (baseOnDragStart)
	    {
	        listener.onDragStart = delegate(GameObject go)
            {
                LuaScriptMgr.Instance.CallLuaFunction(fileName + "onDragStart", go);
	        };
	    }

	    if (baseOnDrag)
	    {
	        listener.onDrag = delegate(GameObject go, Vector2 delta)
            {
                LuaScriptMgr.Instance.CallLuaFunction(fileName + "onDrag", go, delta);
	        };
	    }

	    if (baseOnDragOver)
        {
            listener.onDragOver = delegate(GameObject go)
            {
                LuaScriptMgr.Instance.CallLuaFunction(fileName + "onDragOver", go);
            };
	    }

	    if (baseOnDragOut)
	    {
	        listener.onDragOut = delegate(GameObject go)
            {
                LuaScriptMgr.Instance.CallLuaFunction(fileName + "onDragOut", go);
	        };
	    }

	    if (baseOnDragEnd)
	    {
	        listener.onDragEnd = delegate(GameObject go)
            {
                LuaScriptMgr.Instance.CallLuaFunction(fileName + "onDragEnd", go);
	        };
	    }

	    if (baseOnDrop)
	    {
	        listener.onDrop = delegate(GameObject go, GameObject o)
            {
                LuaScriptMgr.Instance.CallLuaFunction(fileName + "onDrop", go, o);
	        };
	    }
	}
}