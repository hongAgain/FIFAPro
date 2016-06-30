using UnityEngine;
using UnityEditor;
#if Enable_Mobage
using NativeBuilder;
#endif
using System.Collections.Generic;

public class BuildCommands : ScriptableObject {

	private static void SetAndroidSDKPath()
	{
		EditorPrefs.SetString("AndroidSdkRoot","/Applications/adt-bundle-mac-x86_64-20131030/sdk");
	}

	public static void Build(BuildTarget targetPlatform, BuildTargetGroup btg, bool mobageSDK, bool useResources)
	{
		AssetDatabase.Refresh ();
        SetPlatformTarget(targetPlatform);
        CopyFileDataPath(targetPlatform);
	    if (!mobageSDK)
	    {
            SDKBuild.SwitchToNoSDK(targetPlatform);
            Debug.Log("SDKBuild.SwitchToNoSDK(); ----- Done");
	    }
		SDKBuild.SwitchResOrABMode (useResources, btg);
		Debug.Log ("SDKBuild.SwitchResOrABMode(); ----- Done");
		FIFABuilder.CopyLuaToResource ();
		Debug.Log ("FIFABuilder.CopyLuaToResource(); ----- Done");
		if (!useResources)
		{
			Packager.BuildAll (targetPlatform);
			Debug.Log ("Packager.BuildAll(); ----- Done");
			RecordDataFile.BuildDataFile ();
			Debug.Log ("RecordDataFile.BuildDataFile(); ----- Done");
			BuildResourceManager.DeleteResources ();
			Debug.Log ("BuildResourceManager.DeleteResources(); ----- Done");
		}
		List<string> sceneList = new List<string>();		
		foreach (var scene in EditorBuildSettings.scenes)
		{
			sceneList.Add(scene.path);
		}
		SaveAssets();
		Debug.Log("SaveAssets(); ----- Done");
	    if (mobageSDK)
	    {
#if Enable_Mobage
	        BuildTask task = null;
            switch (targetPlatform)
            {
                case BuildTarget.Android:
                    task = new BuildTask_Android(BuildLevel.UtilBuildApk, null, "user.eupe");
                    break;
                case BuildTarget.iPhone:
                    task = new BuildTask_iOS(IOSBuildLevel.UtilApplyNativeBuilder, null, "user.xupe");
                    break;
                default:
                    break;
            }
	        if (task != null)
	        {
	            task.Build();
	        }
            Debug.Log("BuildMobageSDK(); ----- Done");
#endif
	    }
        else
        {
            BuildOptions opt = BuildOptions.None;
            switch (targetPlatform)
            {
                case BuildTarget.Android:
                    BuildPipeline.BuildPlayer(sceneList.ToArray(), "FIFPro.apk", targetPlatform, opt);
                    break;
                case BuildTarget.iPhone:
                    BuildPipeline.BuildPlayer(sceneList.ToArray(), "FiFPro_iOS_Project", targetPlatform, opt);
                    break;
                default:
                    BuildPipeline.BuildPlayer(sceneList.ToArray(), "default", targetPlatform, opt);
                    break;
            }
            Debug.Log("BuildPipeline.BuildPlayer(); ----- Done");
        }
	}

	private static void SaveAssets()
	{
		AssetDatabase.SaveAssets();
		EditorApplication.SaveAssets();
	}

	private static void SetPlatformTarget(BuildTarget targetPlatform)
	{
        BuildTargetGroup btg = BuildTargetGroup.Unknown;
        
        switch(targetPlatform)
		{
            case BuildTarget.StandaloneWindows:
            case BuildTarget.StandaloneOSXIntel:
            case BuildTarget.StandaloneOSXIntel64:
            case BuildTarget.StandaloneOSXUniversal:
                Versioned.PlatformPath = "/Standalone/";
                btg = BuildTargetGroup.Standalone;
                break;
            case BuildTarget.iPhone:
                Versioned.PlatformPath = "/IPhone/";
                btg = BuildTargetGroup.iPhone;
                break;
            case BuildTarget.Android:
                Versioned.PlatformPath = "/Android/";
                btg = BuildTargetGroup.Android;
                break;
            case BuildTarget.WP8Player:
                Versioned.PlatformPath = "/WP/";
                btg = BuildTargetGroup.WP8;
                break;
		}
        //FIFA_CLIENT
        string SCRIPTING_DEFINE_SYMBOLS = PlayerSettings.GetScriptingDefineSymbolsForGroup(btg);
        if (SCRIPTING_DEFINE_SYMBOLS.Contains(Macro.FIFA_CLIENT) == false)
        {
            SCRIPTING_DEFINE_SYMBOLS += ";" + Macro.FIFA_CLIENT;
            PlayerSettings.SetScriptingDefineSymbolsForGroup(btg, SCRIPTING_DEFINE_SYMBOLS);
        }
//        string SCRIPTING_DEFINE_SYMBOLS = PlayerSettings.GetScriptingDefineSymbolsForGroup(btg);
//	    if (SCRIPTING_DEFINE_SYMBOLS.Contains(Macro.USE_ASSETBUNDLE) == false)
//	    {
//            SCRIPTING_DEFINE_SYMBOLS += ";" + Macro.USE_ASSETBUNDLE;
//            PlayerSettings.SetScriptingDefineSymbolsForGroup(btg, SCRIPTING_DEFINE_SYMBOLS);
//	    }
	}

	private static void CopyFileDataPath(BuildTarget targetPlatform)
	{
		string targetfoldername = "Android";
		switch(targetPlatform)
		{
		case BuildTarget.StandaloneWindows:
		case BuildTarget.StandaloneOSXIntel:
		case BuildTarget.StandaloneOSXIntel64:
		case BuildTarget.StandaloneOSXUniversal:
			targetfoldername = "Standalone";
			break;
		case BuildTarget.iPhone:
			targetfoldername = "IPhone";
			break;
		case BuildTarget.Android:
			targetfoldername = "Android";
			break;
		case BuildTarget.WP8Player:
			targetfoldername = "WP";
			break;
		}

		AssetDatabase.DeleteAsset(@"Assets/StreamingAssets");
		AssetDatabase.Refresh();

		AssetDatabase.CreateFolder("Assets/StreamingAssets",targetfoldername);
		AssetDatabase.CopyAsset("Assets/FileDataPath.txt","Assets/StreamingAssets/"+targetfoldername+"/FileDataPath.txt");
		AssetDatabase.Refresh();
	}

	private static void SpecialOperationOniOS()
	{
		AssetDatabase.DeleteAsset(@"Assets/Scripts/SDK/Mobage");
		AssetDatabase.Refresh();
	}
}
