using UnityEngine;
using System.Text;
using System.Collections;
using System.Collections.Generic;

public class SDKAdapter_Mobage : MonoBehaviour, ISDK
{
    public string ScriptName = "Game/MobageScript";
    private ScriptInstaller mScript = new ScriptInstaller();

    private const string APP_ID = "12000139";
    private const string SDK_CONSUMER_KEY_SANDBOX = "sdk_app_id:12000139";
    private const string SDK_CONSUMER_KEY_RELEASE = "sdk_app_id:12000139";
    private const string SDK_SECRET_SANDBOX = "bfcf910e394e4eba658183e75b0ec888";
    private const string SDK_SECRET_RELEASE = "1fc3c7cb19cfdb129791e47c2b7aa847";

    // Use this for initialization
    IEnumerator Start()
    {
        mScript.Install(LuaScriptMgr.Instance, Util.GetLuaFilePath(ScriptName));
        LuaScriptMgr.Instance.CallLuaFunction(Util.GetLuaScriptModuleName(ScriptName) + "OnInit", Application.platform.ToString());

        if (MobageCNService.isProductionMode())
        {
            MobagePlatform.Initialize(
                (int)MBG_REGION.MBG_REGION_CN,
                (int)MBG_SERVER_TYPE.MBG_PRODUCTION,
                SDK_CONSUMER_KEY_RELEASE,
                SDK_SECRET_RELEASE,
                APP_ID);
        }
        else
        {
            MobagePlatform.Initialize(
                (int)MBG_REGION.MBG_REGION_CN,
                (int)MBG_SERVER_TYPE.MBG_SANDBOX,
                SDK_CONSUMER_KEY_SANDBOX,
                SDK_SECRET_SANDBOX,
                APP_ID);
        }

        
        bool getAuthFlag = false;

        AddLoginCallBackLib.onLoginComplete LoginComplete = delegate(string userId)
        {
            LuaScriptMgr.Instance.CallLuaFunction(Util.GetLuaScriptModuleName(ScriptName) + "SetUserId", userId);
            MobageManager.registerTick();

            getAuthFlag = true;
        };

        AddLoginCallBackLib.onLoginRequired LoginRequired = delegate() { };

        AddLoginCallBackLib.onCancel LoginCancel = delegate() { };

        AddLoginCallBackLib.onError LoginError = delegate(MobageError err) { };

        LogoutListener.OnLogoutComplete onPlatformLogoutComplete = delegate() { };

        SwitchAccountProxy.OnSwitchAccount onPlatformSwitchAccount = delegate(bool success) { };

        MobagePlatform.addLoginListener(LoginComplete, LoginRequired, LoginCancel, LoginError);
        MobagePlatform.addLogoutListenner(onPlatformLogoutComplete);
        MobagePlatform.addSwitchAccountListenner(onPlatformSwitchAccount);

        MobageManager.checkLoginStatus();
        yield return null;

        while (!getAuthFlag)
        {
            yield return null;
        }
        LuaScriptMgr.Instance.CallLuaFunction(Util.GetLuaScriptModuleName(ScriptName) + "GetTempAuth");

        while (!verifyFlag)
        {
            yield return null;
        }

        LuaScriptMgr.Instance.CallLuaFunction(Util.GetLuaScriptModuleName(ScriptName) + "ReqSrvList");
        
        
        
        
        
        
        
        
        
        //MobageCNService.HideMenubar();
    }

    public void DispatchEvent(Hashtable kv, OnResponse response)
    {
        string method = (string)kv["Method"];
        if (method == "Auth")
        {
            string oauth_token = (string)kv["oauth_token"];
            MobageAuth.authorizeToken(oauth_token, AuthOnSuccess, AuthOnError);
        }
        else if (method == "ContinueTransaction")
        {
            string id = (string)kv["id"];
            MobageBankDebit.continueTransaction(id, BankDbtDlgOnSuccess, BankDbtDlgOnError, BankDbtDlgOnCancel);
        }
        else
        {
            LuaScriptMgr.Instance.CallLuaFunction(Util.GetLuaScriptModuleName(ScriptName) + "DispatchEvent", kv);
        }
    }

    #region LOGIN

    private bool verifyFlag = false;

    void AuthOnSuccess(string json)
    {
        LuaScriptMgr.Instance.CallLuaFunction(Util.GetLuaScriptModuleName(ScriptName) + "AuthOnSuccess", json);
        verifyFlag = true;
    }

    private void AuthOnError(MobageError error)
    {
        Util.LogError("AuthOnError, code = " + error.code.ToString() + ", desc = " + error.description + ".");
    }
    #endregion

    #region BUY
    private string transactionId = null;

    public void BankDbtDlgOnSuccess(MobageTransaction transaction)
    {
        print("SampleApp; at C# BankDbtDlgOnSuccess :: " + transaction.ToString());
        transactionId = transaction.id;
    }

    public void BankDbtDlgOnError(MobageError error)
    {
        print("SampleApp; at C# BankDbtDlgOnError :: " + error.description);
    }

    public void BankDbtDlgOnCancel()
    {
        print("SampleApp; at C# BankDbtDlgOnCancel :: ");
    }

    void Update()
    {
        if (transactionId != null)
        {
            LuaScriptMgr.Instance.CallLuaFunction(Util.GetLuaScriptModuleName(ScriptName) + "PaySubmit", transactionId);
            transactionId = null;
        }
    }
    #endregion
}