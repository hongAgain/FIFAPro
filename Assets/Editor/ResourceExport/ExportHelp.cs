using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using UnityEditor;
using Object = UnityEngine.Object;

public class ExportHelpers
{
    public static List<T> CollectAll<T>(string path) where T : Object
    {
        List<T> l = new List<T>();
        string[] files = Directory.GetFiles(path);

        foreach (string file in files)
        {
            if (file.Contains(".meta")) continue;
            T asset = (T)AssetDatabase.LoadAssetAtPath(file, typeof(T));
            if (asset == null) throw new Exception("Asset is not " + typeof(T) + ": " + file);
            l.Add(asset);
        }
        return l;
    }

    public static List<T> CollectAll<T>(string path, string filter) where T : Object
    {
        List<T> l = new List<T>();
        string[] files = Directory.GetFiles(path, filter);

        foreach (string file in files)
        {
            if (file.Contains(".meta")) continue;
            T asset = (T)AssetDatabase.LoadAssetAtPath(file, typeof(T));
            if (asset == null) throw new Exception("Asset is not " + typeof(T) + ": " + file);
            l.Add(asset);
        }
        return l;
    }


    public static List<T> CollectAll<T>(string path, string filter, SearchOption op) where T : Object
    {
        if (op != SearchOption.AllDirectories)
        {
            return null;
        }

        List<T> l = new List<T>();
        string[] directs = Directory.GetDirectories(path);

        foreach (string direct in directs)
        {
            List<T> temp = CollectAll<T>(direct, filter);
            foreach (T obj in temp)
            {
                l.Add(obj);
            }
            temp.Clear();
        }
        return l;
    }


    public static void CreateAssetbundlesDir()
    {
        if (!Directory.Exists(EditorGlobalData.AssetbundlePath))
            Directory.CreateDirectory(EditorGlobalData.AssetbundlePath);
    }

    public static void AssetBundlesClear(string mode)
    {
        string[] existingAssetbundles = Directory.GetFiles(EditorGlobalData.AssetbundlePath);
        foreach (string bundle in existingAssetbundles)
        {
            if (bundle.EndsWith(".assetbundle") && bundle.Contains("/assetbundles/" + mode))
                File.Delete(bundle);
        }
    }


    public static void DestroyComponentGameObject<T>(UnityEngine.GameObject gameObject) where T : UnityEngine.Component
    {
        foreach (T skinMeshRender in gameObject.GetComponentsInChildren<T>())
        {
            Object.DestroyImmediate(skinMeshRender.gameObject);
        }
    }

    public static void SetAnimationCullingType(UnityEngine.GameObject gameObject, UnityEngine.AnimationCullingType type)
    {
        foreach (UnityEngine.Animation anim in gameObject.GetComponentsInChildren<UnityEngine.Animation>())
        {
            anim.cullingType = type;
        }
    }

    public static void BuildAssetBundle(string path, List<Object> objs)
    {
        StringBuilder log = new StringBuilder(1024);
        log.Append("BuildAssetBundle >> ");
        log.Append(path);
        log.Append("(");
        foreach (Object obj in objs)
        {
            log.Append(obj.name);
            log.Append(", ");
        }
        log.Append(")");
        UnityEngine.Debug.Log(log.ToString());
        //
        BuildPipeline.BuildAssetBundle(null, objs.ToArray(), EditorGlobalData.AssetbundlePath + path, BuildAssetBundleOptions.CollectDependencies);
    }
    public static void BuildAssetBundle(string path, Object obj)
    {
        StringBuilder log = new StringBuilder(1024);
        log.Append("BuildAssetBundle >> ");
        log.Append(path);
        log.Append("(");
        log.Append(obj.name);
        log.Append(")");
        UnityEngine.Debug.Log(log.ToString());
        //
        List<Object> objs = new List<Object>();
        objs.Add(obj);
        BuildPipeline.BuildAssetBundle(null, objs.ToArray(), EditorGlobalData.AssetbundlePath + path, BuildAssetBundleOptions.CollectDependencies);
    }
    public static Object GetPrefab(UnityEngine.GameObject go, string name)
    {
        Object tempPrefab = PrefabUtility.CreateEmptyPrefab("Assets/" + name + ".prefab");
        tempPrefab = PrefabUtility.ReplacePrefab(go, tempPrefab);
        Object.DestroyImmediate(go);
        return tempPrefab;
    }
    public static Object GetPrefab(UnityEngine.GameObject go, string name, bool destroy)
    {
        Object tempPrefab = PrefabUtility.CreateEmptyPrefab("Assets/" + name + ".prefab");
        tempPrefab = PrefabUtility.ReplacePrefab(go, tempPrefab);
        if (destroy)
        {
            Object.DestroyImmediate(go);
        }
        return tempPrefab;
    }

    public static string GetFilePathWithoutExtens(string path)
    {
        int index = path.LastIndexOf(".");
        return path.Substring(0, index);
    }

    public static string GetFileName(string path)
    {
        int index = path.LastIndexOf("/");
        return path.Substring(index, path.Length - index - 1);
    }
    
}

public static class EditorGlobalData
{

    public static string AssetbundlePath { get { return path; } }

    static string path = "assetbundles" + Path.DirectorySeparatorChar;
    //shaojin.han add hack code
    //static string path = "G:/"+ "assetbundles" + Path.DirectorySeparatorChar;
}