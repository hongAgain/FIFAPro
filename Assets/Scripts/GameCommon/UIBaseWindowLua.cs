using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LuaInterface;
using Common;

[RequireComponent(typeof (UIPanel), typeof (UIWindowEffect))]
public class UIBaseWindowLua : MonoBehaviour
{
    public string ScriptName = "";
    protected bool hasScript = false;

    public bool HasScript
    {
        get { return hasScript; }
    }

    protected string scriptModuleName = "";
    private ScriptInstaller scriptInstaller = new ScriptInstaller();

    private List<UIBaseWindowLua> mChildrenWindow = new List<UIBaseWindowLua>();

    public List<UIBaseWindowLua> ChildrenWindow
    {
        get { return mChildrenWindow; }
    }

    public LuaTable mLuaTables;

    protected enum CloseReason
    {
        Normal,
        DirectDestroy,
        HideForDrawCall,
    }

    protected CloseReason mCloseReason = CloseReason.DirectDestroy;
    public bool FullScreen = false;
    public bool autoCollider = true;

    private UIBaseWindowLua father = null;

    public UIBaseWindowLua Father
    {
        get { return father; }
        set
        {
            father = value;
            father.AddChildWindow(this);
            WindowMgr.AlignOnCenter(father, null);
        }
    }
    public System.Action OnDestroyEvent;
    #region Call by Unity

    protected virtual void Start()
    {
        if (collider == null && autoCollider)
        {
            var boxCollider = gameObject.AddComponent<BoxCollider>();
            boxCollider.isTrigger = true;
            boxCollider.size = new Vector3(2000f, 2000f, 1f);
        }

        hasScript = true;
        scriptModuleName = Util.GetLuaScriptModuleName(ScriptName);
        scriptInstaller.Install(LuaScriptMgr.Instance, Util.GetLuaFilePath("UILua/" + ScriptName));
        //var t = Time.realtimeSinceStartup;

        LuaScriptMgr.Instance.CallLuaFunction(scriptModuleName + "OnStart", gameObject, mLuaTables);
        //LogView.Warn("\"" + name + "\" do start time " + (Time.realtimeSinceStartup - t));

        DoOpenEffect();
    }

    private void OnEnable()
    {
        WindowMgr.AlignOnCenter(father, null);
    }

    private void OnDisable()
    {
        WindowMgr.AlignOnCenter(father, null);
    }

    private void OnDestroy()
    {
        AlignChildren();

        DealChildWindow();

        DoOnDestroy();

        if (OnDestroyEvent != null)
            OnDestroyEvent();
    }

    #endregion

    public void HideForDrawCall()
    {
        mCloseReason = CloseReason.HideForDrawCall;
        Destroy(gameObject);
    }

    public void Hide()
    {
        if (gameObject.activeInHierarchy)
        {
            foreach (var child in mChildrenWindow)
            {
                child.Hide();
            }
			gameObject.SetActive(false);

            if (hasScript) LuaScriptMgr.Instance.CallLuaFunction(scriptModuleName + "OnHide");
        }
    }

    public void Show()
    {
        if (!gameObject.activeInHierarchy)
        {
            gameObject.SetActive(true);
            if (hasScript) LuaScriptMgr.Instance.CallLuaFunction(scriptModuleName + "OnShow");
        }
		else
		{
			if (hasScript) LuaScriptMgr.Instance.CallLuaFunction(scriptModuleName + "OnShowUpdate", mLuaTables);
		}
    }

    public void Close()
    {
        mCloseReason = CloseReason.Normal;
        foreach (var child in mChildrenWindow)
        {
            child.Close();
        }

        GetComponent<UIWindowEffect>().DoCloseEffect(OnCloseEffectDone);
//        if (GetComponent<ScaleEffect>() != null)
//            GetComponent<ScaleEffect>().DoCloseScaleEffect();
    }

    public void DoOpenEffect()
    {
        GetComponent<UIWindowEffect>().DoOpenEffect(OnOpenEffectDone);
//        if (GetComponent<ScaleEffect>() != null)
//            GetComponent<ScaleEffect>().DoOpenScaleEffect();
    }

    protected virtual void OnCloseEffectDone()
    {
        WindowMgr.Recycle(this);
    }

    void OnOpenEffectDone()
    {
        LuaScriptMgr.Instance.CallLuaFunction("LuaTutorial.OnOpenWindow", gameObject);
    }

    protected virtual void DoOnDestroy()
    {
        if (hasScript)
        {
            LuaScriptMgr.Instance.CallLuaFunction(scriptModuleName + "OnDestroy");
        }
    }

    #region Children Window

    private void AlignChildren()
    {
        if (mChildrenWindow.Count == 0 && father != null)
        {
            father.RmvChildWindow(this);
            WindowMgr.AlignOnCenter(father, null);
        }
    }

    protected void DealChildWindow()
    {
        foreach (var child in mChildrenWindow)
        {
            switch (mCloseReason)
            {
                case CloseReason.DirectDestroy:
                    Destroy(child.gameObject);
                    break;
                case CloseReason.HideForDrawCall:
                    child.HideForDrawCall();
                    break;
            }
        }

        if (father != null && mCloseReason == CloseReason.DirectDestroy)
        {
            father.RmvChildWindow(this);

            var layer = 1 + WindowMgr.FindUILayer(this);
            while (--layer >= 0)
            {
                father.MoveBack();
            }
        }

        if (mCloseReason != CloseReason.HideForDrawCall)
        {
            mChildrenWindow.Clear();
            WindowMgr.RmvActiveWindow(this);
        }
    }

    public void AddChildWindow(UIBaseWindowLua child)
    {
        if (mChildrenWindow.Contains(child) == false)
        {
            mChildrenWindow.Add(child);
        }
    }

    public void RmvChildWindow(UIBaseWindowLua child)
    {
        mChildrenWindow.Remove(child);
    }

    #endregion

    #region Layer Relation

    public void SetFrontOf(UIBaseWindowLua window)
    {
        var selfPanel = GetComponent<UIPanel>();
        if (window != null)
        {
            var windowPanel = window.GetComponent<UIPanel>();
            selfPanel.depth = windowPanel.depth + 10;
        }
        else
        {
            selfPanel.depth = 0;
        }

        AdjustSelfPanelDepth();
    }

    public void AdjustSelfPanelDepth()
    {
        var selfPanel = GetComponent<UIPanel>();

        foreach (var panel in GetComponentsInChildren<UIPanel>())
        {
            if (panel == selfPanel) continue;
            var depth = panel.depth;
            panel.depth = depth%10 + selfPanel.depth;
        }
    }

    private Stack<Vector3> mPositionStack = new Stack<Vector3>();

    public void MoveTo(Vector3 to)
    {
        mPositionStack.Push(transform.localPosition);
        TweenPosition.Begin(gameObject, 0.3f, to);
    }

    public void MoveBack()
    {
        if (mPositionStack.Count > 0)
        {
            TweenPosition.Begin(gameObject, 0.3f, mPositionStack.Pop());
        }
    }

    #endregion

    private Dictionary<string, GameObject> mAdditionPrefabs = new Dictionary<string, GameObject>();

    public void AddPrefab(string prefabName, GameObject g)
    {
        if (mAdditionPrefabs.ContainsKey(prefabName))
        {
            return;
        }
        mAdditionPrefabs.Add(prefabName, g);
    }

    public GameObject GetPrefab(string prefabName)
    {
        GameObject prefab = null;
        mAdditionPrefabs.TryGetValue(prefabName, out prefab);
        return prefab;
    }

    #region ResourceBar

    private static GameObject sResourceBar = null;

    public enum ResourceBar
    {
        NotNeed,
        Hide,
        Lobby,
        UI_Have_Resource,
        UI_No_Resource,
    }

    public ResourceBar resourcesBarMode = ResourceBar.NotNeed;
    public string titleKey = "";

    public void OnTop()
    {
        StopCoroutine(ControlResourceBar());
        StartCoroutine(ControlResourceBar());
        AdjustSelfPanelDepth();
    }

    protected IEnumerator ControlResourceBar()
    {
        if (resourcesBarMode != ResourceBar.NotNeed)
        {
            if (sResourceBar == null)
            {
                ResourceManager.onLoadFinished kLoadFinished = delegate(object[] kArgs)
                {

                    sResourceBar = NGUITools.AddChild(WindowMgr.UIParent.gameObject, kArgs[0] as GameObject);

                    new ScriptInstaller().Install(LuaScriptMgr.Instance, Util.GetLuaFilePath("UILua/UIHeadScript"));
                    LuaScriptMgr.Instance.CallLuaFunction("UIHeadScript.OnStart", sResourceBar);
                };
                AssetMgr.OnGetRes cb = delegate(AssetBundle ab)
                {
                    GameObject prefab = (ab.mainAsset as GameObject);
                    sResourceBar = NGUITools.AddChild(WindowMgr.UIParent.gameObject, prefab);

                    new ScriptInstaller().Install(LuaScriptMgr.Instance, Util.GetLuaFilePath("UILua/UIHeadScript"));
                    LuaScriptMgr.Instance.CallLuaFunction("UIHeadScript.OnStart", sResourceBar);
                };

                ResourceManager.Instance.LoadUI("UI/UIHead/UIHead",cb,kLoadFinished);

                while (sResourceBar == null)
                {
                    yield return null;
                }
            }

            LuaScriptMgr.Instance.CallLuaFunction("UIHeadScript.SwitchMode", (int)resourcesBarMode);

            if (string.IsNullOrEmpty(titleKey) == false)
            {
                sResourceBar.GetComponent<UIPanel>().depth = GetComponent<UIPanel>().depth + 9;
				LuaScriptMgr.Instance.CallLuaFunction("UIHeadScript.SetWindowInfo", titleKey, Util.LocalizeString(titleKey), this);
            }
        }
    }

    #endregion
}