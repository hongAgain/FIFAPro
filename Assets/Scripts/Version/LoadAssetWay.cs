//using System.Collections.Generic;
//using UnityEngine;

//#if !USE_ASSETBUNDLE
////#define RELEASE
//#endif

//public class LoadAssetWay
//{
//    public static bool DirectReading()
//    {
//#if !USE_ASSETBUNDLE
//        return true;
//#else
//        return false;
//#endif
//    }

//    public static string URL()
//    {
//#if !USE_ASSETBUNDLE
//        return "";
//#else
//    #if RELEASE
//        return "http://172.21.186.68" + Versioned.PlatformPath;
//    #elif UNITY_EDITOR || UNITY_IPHONE
//        return "file://" + Versioned.PathPrefix();
//    #else
//        return Versioned.PathPrefix();
//    #endif
//#endif
//    }

////    public static string BuildInURL()
////    {
////#if !USE_ASSETBUNDLE
////        return "";
////#else
////    #if UNITY_EDITOR || UNITY_IPHONE
////        return "file://" + Versioned.PathPrefix();
////    #else
////        return Versioned.PathPrefix();
////    #endif
////#endif
////    }
//}

//public class AssetHelper
//{
//    private static Dictionary<string, System.WeakReference> sWeakRefs = new Dictionary<string, System.WeakReference>(); 

//    public delegate void GetAsset(AssetBundle ab);

//    public static void LoadABImmediate(string name, GetAsset cb)
//    {
//        AssetBundle ab = AssetMgr.LoadAssetImmediate(name);
//        cb(ab);
//        if (ab != null)
//        {
//            ab.Unload(false);
//        }
//    }

//    public static Object LoadAssetImmediate(string name)
//    {
//        if (LoadAssetWay.DirectReading())
//        {
//            return Resources.Load(name);
//        }
//        else
//        {
//            if (sWeakRefs.ContainsKey(name))
//            {
//                if (sWeakRefs[name].Target != null)
//                {
//                    return sWeakRefs[name].Target as Object;
//                }
//            }

//            AssetBundle ab = AssetMgr.LoadAssetImmediate(name + ".assetbundle");

//            Object obj = null;
//            if (ab != null)
//            {
//                obj = ab.mainAsset;
//                sWeakRefs[name] = new System.WeakReference(obj);
//                ab.Unload(false);
//            }

//            return obj;
//        }
//    }

//    public static Object LoadAssetImmediate(string name, string defaultName)
//    {
//        var obj = LoadAssetImmediate(name);
//        if (obj == null && string.IsNullOrEmpty(defaultName) == false)
//        {
//            string strDefault = "";
//            int idx = name.LastIndexOf('/');
//            if (idx != -1)
//            {
//                strDefault = name.Substring(0, idx + 1) + defaultName;
//            }
//            obj = LoadAssetImmediate(strDefault);
//        }
        
//        return obj;
//    }
//}