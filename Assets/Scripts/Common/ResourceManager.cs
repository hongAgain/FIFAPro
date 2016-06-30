//#define SERVER_MODE

using UnityEngine;
using System.Collections;
using System;
using System.Diagnostics;
using System.Collections.Generic;
public enum ResType
{
    ResType_UI = 0,
    ResType_GameObject,
    ResType_Audio,
    ResType_Text,
    ResType_Atlas,
    ResType_Texture,
    ResType_Animation,
    ResType_MaxNum
}

namespace Common
{
    public class ResourceManager : MonoBehaviour
    {
        private ResourceManager() { }
        public static ResourceManager Instance;

        public delegate void onLoadFinished(params object[] kArgs);

        void Awake()
        {
            Instance = this;
            CheckAssentBundleFlag();
        }

        [Conditional(Macro.USE_ASSETBUNDLE)]
        private void CheckAssentBundleFlag()
        {
            m_bUseBundle = true;
        }

        public void LoadAsync(string strName, ResType kType, onLoadFinished loadFinished)
        {
            StartCoroutine(DoLoadAsync(kType, strName, loadFinished));
        }

        public void LoadAsync(string strName, onLoadFinished loadFinished)
        {
            LoadAsync(strName, ResType.ResType_GameObject, loadFinished);
        }

        public UnityEngine.Object Load(string strPath,string strFileName,ResType kType,bool bInstantiate = false)
        {
            string strResName = strPath + strFileName;
            UnityEngine.Object kRetObj = null;
            if (m_kWeakRefList.ContainsKey(strResName))
            {
                if (m_kWeakRefList[strResName].Target != null)
                {
                    kRetObj = m_kWeakRefList[strResName].Target as UnityEngine.Object;
                    if (kRetObj != null)
                        return DoInstantiate(kType, kRetObj, bInstantiate);
                }
            }

            AssetBundle kAssetBundle = AssetMgr.LoadAssetImmediate(strResName + ".assetbundle");
            if (kAssetBundle != null)
            {
                kRetObj = kAssetBundle.mainAsset;
                m_kWeakRefList[strResName] = new WeakReference(kRetObj);
                kAssetBundle.Unload(false);
            }
            if (null == kRetObj)
                kRetObj = Resources.Load(strResName, GetType(kType));
            return DoInstantiate(kType, kRetObj, bInstantiate);
        }

        public UnityEngine.Object Load(string strResName, bool bInstantiate = false )
        {
            string strPath = "", strFileName = "";
            GetPathAndFile(strResName, ref strPath, ref strFileName);
            return Load(strPath,strFileName, ResType.ResType_GameObject, bInstantiate);
        }

        public void LoadAllTables(onLoadFinished kOnLoadFinished)
        {
            AssetBundle kAssetBundle = AssetMgr.LoadAssetImmediate("StaticData.assetbundle");
            if (kAssetBundle != null)
            {
                UnityEngine.Object[] kObjList = kAssetBundle.LoadAll();
                for (int i = 0; i < kObjList.Length; i++)
                {
                    TextAsset kTextAsset = kObjList[i] as TextAsset;
                    kOnLoadFinished(kTextAsset.name, kTextAsset.text);
                }
                kAssetBundle.Unload(false);
            }
            else
            {
                TextAsset[] kTextList = Resources.LoadAll<TextAsset>("Tables/");
                for (int i = 0; i < kTextList.Length;i++ )
                {
                    kOnLoadFinished(kTextList[i].name, kTextList[i].text);
                }
                Resources.UnloadUnusedAssets();
            }
        }


        public string LoadText(string strResName)
        {
            string strPath = "", strFileName = "";
            GetPathAndFile(strResName, ref strPath, ref strFileName);
            TextAsset kTextAsset = Load(strPath,strFileName, ResType.ResType_Text) as TextAsset;
            if(null == kTextAsset)
                return null;
            return kTextAsset.text;
        }
        public byte[] LoadLua(string strResName)
        {
            string strPath = "", strFileName = "";
            GetPathAndFile(strResName, ref strPath, ref strFileName);
            TextAsset kTextAsset = Load(strPath, strFileName, ResType.ResType_Text) as TextAsset;
            if(null == kTextAsset)
                return null;
            return kTextAsset.bytes;
        }

        public void LoadUI(string strResName, AssetMgr.OnGetRes kOnGetRes,onLoadFinished kLoadFinished)
        {
            string strPath = "", strFileName = "";
          //  if(m_bUseBundle)
          //  {
                GetPathAndFile(strResName, ref strPath, ref strFileName,false);
                AssetMgr.LoadAsset(strPath + ".assetbundle", strResName,kOnGetRes, kLoadFinished);
            // }
            // else
            // {
            //UnityEngine.Object kRetObj = Resources.Load(strResName);
            //if (null != kLoadFinished)
            //    kLoadFinished(kRetObj);
            //}
        }

        public Texture LoadTexture(string strResName)
        {
            string strPath = "", strFileName = "";
            GetPathAndFile(strResName, ref strPath, ref strFileName);
            Texture kTex = Load(strPath, strFileName, ResType.ResType_Texture) as Texture;
            if(null == kTex)
            {
                kTex = Load(strPath,"Default", ResType.ResType_Texture) as Texture;
            }
            return kTex;
        }


        private void GetPathAndFile(string strIn,ref string strPath ,ref string strFileName,bool bContainSlash = true)
        {
            strIn = strIn.Replace('\\', '/');
            int iIdx = strIn.LastIndexOf("/");
            if (-1 == iIdx)
            {
                strPath = "";
                strFileName = strIn;
            }
            else
            {
                if(bContainSlash)
                    strPath = strIn.Substring(0, iIdx+1);                    
                else
                    strPath = strIn.Substring(0, iIdx);
                strFileName = strIn.Substring(iIdx + 1);
            }
        }
        private Type GetType(ResType kResType)
        {
            Type kType = null;

            switch (kResType)
            {
                case ResType.ResType_Text:
                    kType = typeof(UnityEngine.TextAsset);
                    break;
                case ResType.ResType_Audio:
                    kType = typeof(UnityEngine.AudioClip);
                    break;
                case ResType.ResType_Texture:
                    kType = typeof(UnityEngine.Texture);
                    break;
                case ResType.ResType_Animation:
                    kType = typeof(UnityEngine.AnimationClip);
                    break;
                case ResType.ResType_UI:
                case ResType.ResType_GameObject:
                case ResType.ResType_Atlas:
                    kType = typeof(UnityEngine.GameObject);
                    break;
            }
            return kType;
        }

        private IEnumerator DoLoadAsync(ResType kType, string strResName, onLoadFinished loadFinished, bool bInstantiate = false)
        {
            UnityEngine.Object kResObj = null;

            if (m_bUseBundle)
            {

            }
            else
            {
                ResourceRequest kReq = Resources.LoadAsync(strResName, GetType(kType));
                yield return kReq;
                kResObj = kReq.asset;
            }
            UnityEngine.Object kOutObj = DoInstantiate(kType, kResObj, bInstantiate);
            if (null != loadFinished)
            {
                loadFinished(kOutObj);
            }
        }

        private UnityEngine.Object DoInstantiate(ResType kType, UnityEngine.Object kObj,bool bInstantiate)
        {
            if (null == kObj)
            {
                return null;
            }

            if (kType == ResType.ResType_Text
                || kType == ResType.ResType_Audio
                || kType == ResType.ResType_Atlas
                || kType == ResType.ResType_Texture)
            {
                return kObj;
            }
            else
            {
                if (true == bInstantiate)
                    return Instantiate(kObj);
                return kObj;
            }
        }

        private bool m_bUseBundle = false;
        private Dictionary<string, WeakReference> m_kWeakRefList = new Dictionary<string, WeakReference>(); 

    }

}