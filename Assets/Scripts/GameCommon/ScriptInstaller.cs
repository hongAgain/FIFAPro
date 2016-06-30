using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public enum InstallerState
{
    NO_INSTALL = 0,
    INSTALLING,
    COMPLETED,
    RELOAD,
}

public class ScriptInstaller
{
    public void Install(LuaScriptMgr mgr, string name)
    {
        _mgr = mgr;
        if (state == InstallerState.NO_INSTALL)
        {
            state = InstallerState.INSTALLING;
            scriptName = name;
            onScriptLoaded();
        }
    }

    public void Reload(string name)
    {
        
    }

    void onScriptLoaded()
    {
        state = InstallerState.COMPLETED;
        //_mgr.DoFile(scriptName);
        _mgr.DoString(string.Format("require '{0}'", scriptName));
    }

    LuaScriptMgr _mgr = null;
    string scriptName = "";
    InstallerState state = InstallerState.NO_INSTALL;
}

