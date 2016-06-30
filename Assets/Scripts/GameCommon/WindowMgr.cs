using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using LuaInterface;
using Common;

public class WindowMgr
{
	public enum UIType{ UIGameWindow, UIMsgBox };

    private class Proxy : ILruItem<Proxy>
    {
		public static LRU<Proxy> lru = new LRU<Proxy>(2);

        public UIBaseWindowLua mInstance = null;
        public string mName = "";
        public UIBaseWindowLua mFather = null;
		public UIType uiType = UIType.UIGameWindow;

        public GameObject mPrefab = null;

        public bool mFullScreen = false;
        public LuaTable mLuaTables;

		public void AllResourcesDownloaded(Object[] prefabs)
        {
            if ((mPrefab != null))
            {
                if (mInstance == null)
                {
					GameObject uiClone = null;
					switch(uiType)
					{
					case UIType.UIGameWindow:
						uiClone = NGUITools.AddChild(UIParent.gameObject, mPrefab);
						break;
					case UIType.UIMsgBox:
						uiClone = NGUITools.AddChild(UISystemParent.gameObject, mPrefab);
						break;
					default:						
						uiClone = NGUITools.AddChild(UIParent.gameObject, mPrefab);
						break;
					}
                    uiClone.name = uiClone.name.Replace("(Clone)", "");

                    mInstance = uiClone.GetComponent<UIBaseWindowLua>();
                    if (prefabs == null) return;
                    foreach (var prefab in prefabs.Where(prefab => prefab.name != mPrefab.name))
                    {
                        mInstance.AddPrefab(prefab.name, prefab as GameObject);
                    }
                }
                mInstance.mLuaTables = mLuaTables;
                mFullScreen = mInstance.FullScreen;

                if (mFather != null)
                {
                    mInstance.Father = mFather;
                }

//                LuaTimer.Instance.RmvTimer(mTimerID);
            }
        }

        public void TrySaveDC()
        {
            //if (mInstance.FullScreen)
            //{
            //    var idx = mShowWindowReqList.IndexOf(this);
            //    for (var i = 0; i < idx; ++i)
            //    {
            //        var a = mShowWindowReqList[i];
            //        if (a.mInstance != null)
            //        {
            //            a.mInstance.HideForDrawCall();
            //            a.mInstance = null;
            //        }
            //    }
            //}

            if (mInstance.FullScreen)
            {
                var idx = mShowWindowReqList.IndexOf(this);
                for (var i = 0; i < idx; ++i)
                {
                    var a = mShowWindowReqList[i];
                    if (a.mInstance != null)
                    {
                        a.mInstance.Hide();
                    }
                }
            }
        }

//        private long mTimerID = 0;

        public void OnRecycle()
        {
            if (mInstance != null)
            {
				//                mTimerID = LuaTimer.Instance.AddTimer(2f, true, AutoRelease);
				mInstance.Hide();
				AutoRelease();
            }
        }

        void AutoRelease()
        {
            OnRmv();
            lru.RmvItem(this);
        }

        public void OnRmv()
        {
            if (mInstance != null)
            {
                Object.Destroy(mInstance.gameObject);
            }
        }

        public int CompareTo(Proxy other)
        {
            UIPanel p1 = null;
            UIPanel p2 = null;

            if (mInstance != null)
            {
                p1 = mInstance.GetComponent<UIPanel>();
            }

            var a = other.mInstance;
            if (a != null)
            {
                p2 = a.GetComponent<UIPanel>();
            }

            if (p1 == null)
            {
                return 1;
            }
            else if (p2 == null)
            {
                return -1;
            }
            else
            {
                return -p1.depth.CompareTo(p2.depth);
            }
        }
    }
	
	public const int UI_LAYER = 5;
	public const int UI3D_LAYER = 10;
	public const int UISystem_LAYER = 11;

    private static List<Proxy> mShowWindowReqList = new List<Proxy>();
	private static Dictionary<string,bool> mCachedCloseInstruction = new Dictionary<string,bool >(); 

    public static AssetBundle mSharedBundle = null;

	private static Transform _UIParent=null;
    public static Transform UIParent
    {
        get
        {
            //var uiCamera = NGUITools.FindCameraForLayer(UI_LAYER);
            //return uiCamera != null ? uiCamera.transform : null;
            if (_UIParent != null)
                return _UIParent;
           // if (UICamTrans != null)
           // {
            _UIParent = Client.Instance.UIRoot.transform.Find("UIAttach");
            //}
            return _UIParent;
        }
    }

	private static Transform _UISystemParent=null;
	public static Transform UISystemParent
	{
		get
		{
			if (_UISystemParent != null)
				return _UISystemParent;
			if (UICamSystemTrans != null)
			{
				_UISystemParent = UICamSystemTrans.transform.Find("TransformNode");
			}
			return _UISystemParent;
		}
	}

	public static void MoveTransformNodeDelta(float deltaX, float deltaY, float deltaZ)
	{
		if (UIParent!=null)
		{
			UIParent.localPosition = UIParent.localPosition + new Vector3 (deltaX,deltaY,deltaZ);
		}
	}

	public static void MoveTransformNodeAbsolute(float absoluteX, float absoluteY, float absoluteZ)
	{
		if (UIParent!=null)
		{
			UIParent.localPosition = new Vector3 (absoluteX,absoluteY,absoluteZ);
		}
		if (UISystemParent!=null)
		{
			UISystemParent.localPosition = new Vector3 (absoluteX,absoluteY,absoluteZ);
		}
	}
	
	private static Transform _UICamTrans = null;
	
	public static Transform UICamTrans
	{
		get
		{
			if (_UICamTrans != null)
				return _UICamTrans;
			
            Camera uiCamera = NGUITools.FindCameraForLayer(UI_LAYER);


			if (uiCamera != null)
			{
				_UICamTrans = uiCamera.transform;
			}
			return _UICamTrans;
		}
	}

	private static Transform _UICamSystemTrans = null;	
	public static Transform UICamSystemTrans
	{
		get
		{
			if (_UICamSystemTrans != null)
				return _UICamSystemTrans;			
			Camera uiCamera = NGUITools.FindCameraForLayer(UISystem_LAYER);
			if (uiCamera != null)
			{
				_UICamSystemTrans = uiCamera.transform;
			}
			return _UICamSystemTrans;
		}
	}

	private static CameraBlurBase _UICamBlur = null;
	public static CameraBlurBase UICamBlur
	{
		get
		{
			if (_UICamBlur != null)
				return _UICamBlur;
			if (UICamTrans != null)
			{
				_UICamBlur = CameraBlur.GetCameraBlurScript(UICamTrans);
			}
			return _UICamBlur;
		}
		set
		{
			_UICamBlur = value;
		}
	}

    private static Transform _UICam3DTrans = null;
    public static Transform UICam3DTrans
    {
        get
        {
            if (_UICam3DTrans != null)
                return _UICam3DTrans;

            GameObject camera = GameObject.Find("UI Root (3D)/Camera");

            if (camera != null)
            {
                _UICam3DTrans = camera.transform;
            }
            return _UICam3DTrans;
        }
    }

    public static void ShowMsgBox(string uiName, LuaTable luaTables)
    {
		ShowSystemWindow (uiName, luaTables);
//        var info = new Proxy
//        {
//            mName = uiName,
//            mFather = null,
//            mLuaTables = luaTables
//        };
//        mShowWindowReqList.Add(info);
//
//        Load(info,UIType.UIMsgBox);
    }

	public static void ShowSystemWindow(string uiName, LuaTable luaTables)
	{
		var info = new Proxy
		{
			mName = uiName,
			mFather = null,
			mLuaTables = luaTables,
			uiType = UIType.UIMsgBox
		};
		mShowWindowReqList.Add(info);
		
		Load(info);
	}

    public static void ShowWindow(string uiName)
    {
        ShowWindow(null, uiName, null);
    }

    public static void ShowWindow(string uiName, LuaTable luaTables)
    {
        ShowWindow(null, uiName, luaTables);
    }

    public static void ShowWindow(UIBaseWindowLua father, string uiName, LuaTable luaTables)
    {
        var info = FindWindow(uiName);
        if (info != null)
        {
            mShowWindowReqList.Remove(info);
            mShowWindowReqList.Add(info);

            if (info.mInstance != null)
            {
                if (info.mInstance.HasScript == false) return;

				info.mFather = father;
				info.mLuaTables = luaTables;
				info.uiType = UIType.UIGameWindow;

				info.mInstance.mLuaTables = luaTables;
                info.mInstance.Show();
                info.mInstance.DoOpenEffect();

                Proxy.lru.RmvItem(info);

                info.AllResourcesDownloaded(null);
                info.TrySaveDC();

                AdjustLayer();
            }
            else
            {
                Load(info);
            }
        }
        else
        {
            info = new Proxy
            {
                mName = uiName,
                mFather = father,
				mLuaTables = luaTables,
				uiType = UIType.UIGameWindow
            };
            mShowWindowReqList.Add(info);

            Load(info);
        }
    }

    private static void Load(Proxy info)
    {
        ResourceManager.onLoadFinished kLoadFinished = delegate(object[] kArgs)
        {
            info.mPrefab = kArgs[0] as GameObject;
            var prefabs = Resources.LoadAll(string.Format("UI/{0}/", info.mName));
            info.AllResourcesDownloaded(prefabs);

            info.TrySaveDC();
            AdjustLayer();
        };
        AssetMgr.OnGetRes onDownloadSharedAsset = delegate(AssetBundle assetBundle)
        {
            mSharedBundle = assetBundle;
        };
        AssetMgr.OnGetRes cb = delegate(AssetBundle assetBundle)
        {
            if (mSharedBundle == null)
            {
               AssetMgr.LoadAsset("UI/SharedAssets.assetbundle", null,onDownloadSharedAsset,null);
            }

            if (mCachedCloseInstruction == null)
                mCachedCloseInstruction = new Dictionary<string, bool>();
            if (mCachedCloseInstruction.ContainsKey(assetBundle.mainAsset.name) && mCachedCloseInstruction[assetBundle.mainAsset.name])
            {
                mShowWindowReqList.Remove(info);
                mCachedCloseInstruction[assetBundle.mainAsset.name] = false;
                return;
            }

            var proxy = FindWindow(assetBundle.mainAsset.name);
            proxy.mPrefab = assetBundle.mainAsset as GameObject;
            var prefabs = assetBundle.LoadAll(typeof(GameObject));
            proxy.AllResourcesDownloaded(prefabs);
            proxy.TrySaveDC();
            AdjustLayer();

        };
        var path = string.Format("UI/{0}/{0}", info.mName);
        ResourceManager.Instance.LoadUI(path,cb,kLoadFinished);
    }

    public static void CloseWindow(string uiName)
    {
        var window = FindWindow(uiName);

        if (window != null && window.mInstance)
			window.mInstance.Close ();
		else
			//cache the closing instruction
			mCachedCloseInstruction[uiName] = true;
    }

    private static Proxy FindWindow(string uiName)
    {
        var ui = mShowWindowReqList.Find( a => a.mName == uiName );
        return ui;
    }

    public static void RmvActiveWindow(UIBaseWindowLua window)
    {
        mShowWindowReqList.Remove(FindWindow(window.name));

        for (int i = mShowWindowReqList.Count - 1; i >= 0; --i)
        {
            if (mShowWindowReqList[i].mInstance != null)
            {
                mShowWindowReqList[i].mInstance.OnTop();
                break;
            }
        }
    }

    public static void AdjustLayer()
    {
        UIBaseWindowLua lastUI = null;
        foreach (var info in mShowWindowReqList.Where(info => info.mInstance != null))
        {
            info.mInstance.SetFrontOf(lastUI);

            lastUI = info.mInstance;
        }

        if (lastUI != null)
        {
            lastUI.OnTop();
        }

		CheckWindows ();
    }

	public static void CheckWindows()
	{
		if (HaveMsgBoxWindows()) 
		{
			if (UICamBlur)
            {
				UICamBlur.enabled = true;
				UICamBlur.LerpBlurUp();
			}
		}
		else
		{
			if(null != UICamBlur && UICamBlur.enabled)
				UICamBlur.LerpBlurDown();
		}
	}
	
	private static bool HaveMsgBoxWindows()
	{
		for (int i = mShowWindowReqList.Count - 1; i >= 0; --i)
		{
			if (mShowWindowReqList[i].uiType == UIType.UIMsgBox)
				return true;
		}
		return false;
	}
    public static void AlignOnCenter(UIBaseWindowLua father, System.Comparison<UIBaseWindowLua> comparison)
    {
        if (father == null) return;

        var children = father.ChildrenWindow;
        if (children.Count > 0)
        {
            var sorted = new List<UIBaseWindowLua>();

            var parent = father;
            do
            {
                var temp = parent.ChildrenWindow;
                foreach (var ui in temp.Where(ui => sorted.Contains(ui) == false && ui.gameObject.activeInHierarchy))
                {
                    sorted.Add(ui);
                }
                sorted.Add(parent);
                parent = parent.Father;
            } while (parent != null);

            if (comparison != null)
            {
                sorted.Sort(comparison);
            }

            var childrenBounds = new Bounds[sorted.Count];

            var totalX = 0f;
            for (var i = 0; i < childrenBounds.Length; ++i)
            {
                childrenBounds[i] = NGUIMath.CalculateRelativeWidgetBounds(sorted[i].transform, sorted[i].transform, true);
                totalX += childrenBounds[i].size.x;
            }

            var space = (Screen.width - totalX) / 2;
            for (var i = 0; i < childrenBounds.Length; ++i)
            {
                var window = sorted[i];
                window.MoveTo(Vector3.left * (Screen.width / 2 - space - childrenBounds[i].extents.x));
                space += childrenBounds[i].size.x;
            }
        }
        else
        {
            father.MoveBack();

            var parent = father;
            while (parent != null)
            {
                parent = parent.Father;
                if (parent != null)
                {
                    parent.MoveBack();
                }
                else
                {
                    break;
                }
            }
        }
    }

    public static int FindUILayer(UIBaseWindowLua ui)
    {
        var maxLayer = 0;
        foreach (var uiInfo in mShowWindowReqList)
        {
            var window = uiInfo.mInstance;
            if (window != null)
            {
                var layer = 0;
                while (true)
                {
                    window = window.Father;
                    if (window == null || System.Object.ReferenceEquals(window, ui))
                    {
                        break;
                    }
                    else
                    {
                        ++layer;
                    }
                }
                if (maxLayer < layer) maxLayer = layer;
            }
        }
        return maxLayer;
    }

    public static void BlockInput()
    {
		UICamTrans.GetComponent<UICamera>().eventReceiverMask.value = LayerMask.GetMask("Nothing");
    }

    public static void UnLockInput()
    {
		UICamTrans.GetComponent<UICamera>().eventReceiverMask.value = LayerMask.GetMask(LayerMask.LayerToName(UI_LAYER));
    }
	
	public static void BlockUISystemInput()
	{
		UICamSystemTrans.GetComponent<UICamera>().eventReceiverMask.value = LayerMask.GetMask("Nothing");
	}

	public static void UnLockUISystemInput()
	{
		UICamSystemTrans.GetComponent<UICamera>().eventReceiverMask.value = LayerMask.GetMask(LayerMask.LayerToName(UISystem_LAYER));
	}

    public static void Recycle(UIBaseWindowLua ui)
    {
        var item = FindWindow(ui.name);
        Proxy.lru.AddItem(item);

        if (ui.FullScreen)
        {
            var max = mShowWindowReqList.Count;
            for (var i = max - 2; i >= 0; --i)
            {
                var info = mShowWindowReqList[i];

                if (info.mInstance == null)
                {
                    Load(info);
                }
                else
                {
                    info.mInstance.Show();
                }
                if (info.mFullScreen)
                {
                    break;
                }
            }
        }
    }

    public static void ActiveUICamera(bool toggle)
    {
        var uicamera = UICamTrans;
        if (uicamera)
        {
            uicamera.camera.enabled = toggle;
        }
    }

    public static GameObject Create3DUI(GameObject prefab)
    {
        var camera3D = UICam3DTrans;
        var angle = camera3D.camera.fieldOfView / 2;
        var d = 320 / Mathf.Tan(Mathf.Deg2Rad * angle);

        var clone = NGUITools.AddChild(camera3D.gameObject, prefab);
        clone.transform.localPosition = Vector3.forward * d;

        return clone;
    }
}