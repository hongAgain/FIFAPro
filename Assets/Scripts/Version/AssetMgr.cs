using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Common.Log;
using Common;

public class AssetMgr : MonoBehaviour
{
    private static AssetMgr mInstance;

    private FileList mFileList = null;

    private void Awake()
    {
        if (mInstance != null && mInstance.GetInstanceID() != this.GetInstanceID())
        {
            LogManager.Instance.LogError("AssetMgr is singleton");
            Destroy(this);
        }
    }

    private void OnDestroy()
    {
        if (mInstance == this)
        {
            mInstance = null;
        }
    }

    private class Req
    {
        /// <summary>
        /// AssetBundle Name
        /// </summary>
        public string abName;
        public string resName;
        /// <summary>
        /// Callback
        /// </summary>
        public OnGetRes cb;
        public ResourceManager.onLoadFinished loadFinished;
        public static bool operator ==(Req a, Req b)
        {
            if ((object)a != null && (object)b != null)
            {
                return a.abName == b.abName;
            }
            else
            {
                return (object)a == null && (object)b == null;
            }
        }

        public static bool operator !=(Req a, Req b)
        {
            return !(a == b);
        }

        public override int GetHashCode()
        {
            return abName.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is Req)
            {
                return this == (Req)obj;
            }
            else
            {
                return false;
            }
        }
    }

    public delegate void OnGetRes(AssetBundle ab);
    private static List<Req> mReqList = new List<Req>();

    private static void CreateInstance()
    {
        if (mInstance == null)
        {
            var go = new GameObject("AssetMgr");
            DontDestroyOnLoad(go);
            go.hideFlags = HideFlags.HideAndDontSave;
            mInstance = go.AddComponent<AssetMgr>();

            mReqList.Clear();
            mInstance.mFileList = new FileList();
            mInstance.mFileList.ReadFileData(FileList.SavePath());
            mInstance.StartCoroutine(mInstance.Loading());
        }
    }

    public static void LoadAsset(string assetBundle,string strResName, OnGetRes callback,ResourceManager.onLoadFinished kFinished)
    {
        CreateInstance();
        var req = new Req { abName = assetBundle,resName = strResName, cb = callback ,loadFinished = kFinished };
        
        if (mReqList.Contains(req)) return;
        mReqList.Add(req);
    }

    public static AssetBundle LoadAssetImmediate(string assetBundle)
    {
        CreateInstance();

        byte[] bytes = ReadAllBytes(assetBundle);
        if (null == bytes)
            return null;
        return AssetBundle.CreateFromMemoryImmediate(bytes);
    }

    private static byte[] ReadAllBytes(string assetBundle)
    {
        int fromIdx = assetBundle.LastIndexOf('/');
        string name = fromIdx == -1 ? assetBundle : assetBundle.Substring(fromIdx + 1);
        string relativePath = assetBundle.Replace(name, "");
        string strFullPath = FileList.SavePath() + relativePath;
        if (false == Directory.Exists(strFullPath))
            return null;
        string[] allFiles = Directory.GetFiles(FileList.SavePath() + relativePath, name, SearchOption.AllDirectories);

        if (allFiles != null && allFiles.Length > 0)
        {
            var bytes = File.ReadAllBytes(allFiles[0]);

            return bytes;
        }
        else
        {
            return null;
        }
    }

    private IEnumerator Loading()
    {
        while (true)
        {
            while (mReqList.Count <= 0)
            {
                yield return null;
            }

            var req = mReqList[0];
            if (mFileList.m_FileDataList.ContainsKey(req.abName) == false)
            {
                UnityEngine.Object kRetObj = null;
                if (null != req.resName)
                    kRetObj = Resources.Load(req.resName);
                if (null != req.loadFinished)
                    req.loadFinished(kRetObj);
                LogManager.Instance.LogWarning(string.Format("Havn't asset {0}", req.abName));
                mReqList.RemoveAt(0);
                continue;
            }
            //var version = mFileList.m_FileDataList[req.abName].Version;
            //WWW www = WWW.LoadFromCacheOrDownload(LoadAssetWay.URL() + req.abName, version);
            //yield return www;

            //if (string.IsNullOrEmpty(www.error))
            //{
            //    LogView.Log("[" + www.url + "] success");
            //    mReqList.RemoveAt(0);
            //    req.cb(www.assetBundle);

            //    if (www.url.Contains("SharedAssets") == false)
            //    {
            //        www.assetBundle.Unload(false);
            //    }
            //}
            //else
            //{
            //    LogView.Error(string.Format("AssetMgr try get [" + www.url + "] encounter some error. Try again!.\nError: " + www.error));
            //}

            var t = Time.realtimeSinceStartup;

            var bytes = ReadAllBytes(req.abName);
            var abReq = AssetBundle.CreateFromMemory(bytes);
            while (!abReq.isDone)
            {
                yield return null;
            }

            LogManager.Instance.LogWarning("Load " + req.abName + " cost " + (Time.realtimeSinceStartup - t).ToString() + " seconds");

            mReqList.RemoveAt(0);
            req.cb(abReq.assetBundle);

            if (req.abName.Contains("SharedAssets") == false)
            {
                abReq.assetBundle.Unload(false);
            }
        }
    }
}