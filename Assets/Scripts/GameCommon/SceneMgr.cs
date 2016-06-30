using Common;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SceneMgr : Single<SceneMgr, SceneMgr>
{
	public static void LoadLevel(string levelName)
	{
		Application.LoadLevel(levelName);
	}

    private GameObject curLevelState = null;

    public GameObject ChangeLevelState(string levelStateName, Vector3 atPos)
    {
        //ClearLevelState();

        //var prefab = AssetHelper.LoadAssetImmediate("Scene/" + levelStateName) as GameObject;
        //curLevelState = Instantiate(prefab) as GameObject;
        curLevelState = ResourceManager.Instance.Load("Scene/" + levelStateName, true) as GameObject;
        if (curLevelState != null)
        {
            curLevelState.transform.position = atPos;
        } 
        else
        {
            Common.Log.LogManager.Instance.RedLog(levelStateName + "== null");
        }
        

        return curLevelState;
    }

    public GameObject CreateLevelState(string levelStateName)
    {
        return ResourceManager.Instance.Load("Scene/" + levelStateName, true) as GameObject;
    }

    public void ClearLevelState()
    {
        if (curLevelState != null)
        {
            Object.Destroy(curLevelState);
            curLevelState = null;
        }
    }
	
	void OnLevelWasLoaded()
	{
		StartCoroutine(NotificationOnSceneLoaded(Application.loadedLevelName));
	}
	
	IEnumerator NotificationOnSceneLoaded(string newSceneName)
	{
		yield return null;
		LuaScriptMgr.Instance.CallLuaFunction("SceneManager.OnLoadScene", newSceneName);
	}
	/// <summary>
	/// load an empty scene first, then go to empty "train" scene
	/// </summary>

//	private Queue<string> mLoadedSceneName = new Queue<string>(); 
    //private static SceneMgr instance;

    //private static SceneMgr Instance
    //{
    //    get
    //    {
    //        if (instance == null)
    //        {
    //            instance = Object.FindObjectOfType<SceneMgr>();
    //        }
    //        if (instance == null)
    //        {
    //            GameObject go = new GameObject("SceneMgr");
    //            DontDestroyOnLoad(go);
    //            instance = go.AddComponent<SceneMgr>();
    //        }

    //        return instance;
    //    }
    //}

    //void OnDestroy()
    //{
    //    if (instance == this)
    //    {
    //        instance = null;
    //    }
    //}

//	void OnLevelWasLoaded()
//	{
//		mLoadedSceneName.Enqueue(Application.loadedLevelName);
//	}

//    void Update()
//    {
//        if (mLoadedSceneName.Count > 0)
//        {
//            switch (mLoadedSceneName.Dequeue())
//            {
//			case "Empty":
//                new GameObject("Prepare", typeof(PrepareMatchAsset));
//                break;
//            }
//        }
//    }


}
