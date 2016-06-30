using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections;
//using System.Collections.Generic;

public class BuildResourceManager {

	public const string RESOURCES_PATH = "Assets/Resources/";
	
	public static bool bNeedMoveAssetBack = false;

	public static void DeleteResources()
	{	
		string path = RESOURCES_PATH.EndsWith(@"/") ? RESOURCES_PATH.Substring(0, RESOURCES_PATH.Length - 1) : RESOURCES_PATH;
		Debug.Log("DeleteAsset(); ----- path"+path);
		if(AssetDatabase.DeleteAsset(path))
		{			
			Debug.Log("DeleteAsset(); ----- Done");
		}
		else
		{
			Debug.Log("DeleteAsset(); ----- Failed");
		}
//		if (Directory.Exists (RESOURCES_PATH))
//		{
//			UnityEditor.FileUtil.DeleteFileOrDirectory(RESOURCES_PATH);
//			AssetDatabase.Refresh();
//			Debug.Log("DeleteAsset(); ----- Done");;
//		}
//		else
//		{
//			Debug.Log("DeleteAsset(); ----- Failed");;
//		}
		AssetDatabase.Refresh();
	}
}