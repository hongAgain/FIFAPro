using UnityEngine;

public class UIMsgBoxLua : UIBaseWindowLua
{
    protected override void DoOnDestroy()
    {
        if (hasScript)
        {
            LuaScriptMgr.Instance.CallLuaFunction(scriptModuleName + "OnDestroy", gameObject);
        }

		WindowMgr.CheckWindows();
    }

    protected override void OnCloseEffectDone()
    {
        Destroy(gameObject);
    }
}