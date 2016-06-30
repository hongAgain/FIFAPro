using System.Collections;

public delegate void OnResponse(object obj);

public interface ISDK
{
    void DispatchEvent(Hashtable kv, OnResponse response);
}

public class NoSDK : ISDK
{
    public NoSDK()
    {
        new ScriptInstaller().Install(LuaScriptMgr.Instance, "Game/NoSDK");
    }

    public void DispatchEvent(Hashtable kv, OnResponse response)
    {
        Util.Log("NoSDK handle DispatchEvent, do nothing!");
    }
}