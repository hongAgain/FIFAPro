using UnityEditor;
using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class Packager : EditorWindow
{
	[MenuItem("FIFA Editor/Asset Bundle/Build AB Window", false, 6)]
	static void ShowBuildABWindow()
    {
        EditorWindow.CreateInstance<Packager>().Show();
    }

    private const string Streaming = "Assets/StreamingAssets";
    private static BuildAssetBundleOptions options = /*BuildAssetBundleOptions.UncompressedAssetBundle | */BuildAssetBundleOptions.DeterministicAssetBundle | 
                                            BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.CompleteAssets;

    private static ImportAssetOptions importOptions =
                                            ImportAssetOptions.ForceSynchronousImport | ImportAssetOptions.ForceUpdate;

    void OnGUI()
    {
        if (GUILayout.Button("Build CommonLua"))
        {
            BuildCommonLua(EditorUserBuildSettings.activeBuildTarget);
            
            RecordDataFile.BuildDataFile();
        }

        if (GUILayout.Button("Build Logic"))
        {
            BuildLogic(EditorUserBuildSettings.activeBuildTarget);

            RecordDataFile.BuildDataFile();
        }

        if (GUILayout.Button("Build UILua"))
        {
            BuildAllUILua(EditorUserBuildSettings.activeBuildTarget);
            
            RecordDataFile.BuildDataFile();
        }

        if (GUILayout.Button("Build All UI Prefab"))
        {
            BuildAllUIPrefab(EditorUserBuildSettings.activeBuildTarget);

            RecordDataFile.BuildDataFile();
        }

        if (GUILayout.Button("Build StaticData"))
        {
            BuildAllStaticData(EditorUserBuildSettings.activeBuildTarget);

            RecordDataFile.BuildDataFile();
        }

        if (GUILayout.Button("Build Localization"))
        {
            BuildLocalization(EditorUserBuildSettings.activeBuildTarget);

            RecordDataFile.BuildDataFile();
        }

        if (GUILayout.Button("Build Body"))
        {
            BuildBody(EditorUserBuildSettings.activeBuildTarget);

            RecordDataFile.BuildDataFile();
        }

        if (GUILayout.Button("Build Action"))
        {
            BuildAction(EditorUserBuildSettings.activeBuildTarget);

            RecordDataFile.BuildDataFile();
        }

        if (GUILayout.Button("Build Head"))
        {
            BuildHead(EditorUserBuildSettings.activeBuildTarget);
		
            RecordDataFile.BuildDataFile();
        }

        if (GUILayout.Button("Build Number"))
        {
            BuildNumber(EditorUserBuildSettings.activeBuildTarget);

            RecordDataFile.BuildDataFile();
        }

        if (GUILayout.Button("Build Scene"))
        {
            BuildScene(EditorUserBuildSettings.activeBuildTarget);

            RecordDataFile.BuildDataFile();
        }

        if (GUILayout.Button("Build Scattered Image"))
        {
            BuildScatteredImg(EditorUserBuildSettings.activeBuildTarget);

            RecordDataFile.BuildDataFile();
        }

        if (GUILayout.Button("Build Uniform"))
        {
            BuildUniform(EditorUserBuildSettings.activeBuildTarget);

            RecordDataFile.BuildDataFile();
        }
        if (GUILayout.Button("Build Battle Tables"))
        {
            BuildBattleTables(EditorUserBuildSettings.activeBuildTarget);

            RecordDataFile.BuildDataFile();
        }
        if (GUILayout.Button("Build Common Tables"))
        {
            BuildCommonTables(EditorUserBuildSettings.activeBuildTarget);

            RecordDataFile.BuildDataFile();
        }
        if (GUILayout.Button("Build Battle Prefabs"))
        {
            BuildBattlePrefab(EditorUserBuildSettings.activeBuildTarget);

            RecordDataFile.BuildDataFile();
        }
        if (GUILayout.Button("Build AI"))
        {
            BuildAI(EditorUserBuildSettings.activeBuildTarget);

            RecordDataFile.BuildDataFile();
        }

        GUI.backgroundColor = Color.green;
        if (GUILayout.Button("Build All"))
        {
            BuildAll(EditorUserBuildSettings.activeBuildTarget);

            RecordDataFile.BuildDataFile();
        }

        GUI.backgroundColor = Color.red;
        if (GUI.Button(new Rect(position.width - 100f, position.height - 60f, 80f, 30f), "CleanCache"))
        {
            FileList.DeleteLocalFileList();
        }


        GUI.Label(new Rect(position.width - 100f, position.height - 30f, 80f, 20f), "Build " + EditorUserBuildSettings.activeBuildTarget.ToString());
    }

    // 编译多对1的包
	public static void CollectAll<T>(string prefix, string path, string pattern, string saveName, BuildTarget targetPlatform) where T : UnityEngine.Object
    {
        var savePath = Versioned.PathPrefix() + prefix;
        if (Directory.Exists(savePath) == false)
        {
            Directory.CreateDirectory(savePath);
        }

        List<T> list = new List<T>();
        //foreach (var path in paths)
        {
            string[] fileNames = Directory.GetFiles(Application.dataPath + path, pattern, SearchOption.AllDirectories);
            foreach (var fileName in fileNames)
            {
                if (fileName.Contains(".meta")) continue;
                if (fileName.Contains(".DS_Store")) continue;

                var relativePath = fileName.Substring(fileName.IndexOf("Assets/"));
                T obj = AssetDatabase.LoadAssetAtPath(relativePath, typeof(T)) as T;
                list.Add(obj);
            }
        }
        BuildPipeline.BuildAssetBundle(null, list.ToArray(), savePath + saveName, options, targetPlatform);
        AssetDatabase.ImportAsset(string.Format("{0}/{1}{2}", Streaming, prefix, saveName), importOptions);
        list.Clear();
        list = null;
    }
    
    // 编译1对1的包
    public static void CollectAll<T>(string prefix, string path, string pattern, BuildTarget targetPlatform) where T : UnityEngine.Object
    {
        var savePath = Versioned.PathPrefix() + prefix;
        if (Directory.Exists(savePath) == false)
        {
            Directory.CreateDirectory(savePath);
        }

        //foreach (var path in paths)
        {
            string[] fileNames = Directory.GetFiles(Application.dataPath + path, pattern, SearchOption.TopDirectoryOnly);
            foreach (var fileName in fileNames)
            {
                if (fileName.Contains(".meta")) continue;
                if (fileName.Contains(".DS_Store")) continue;

                var relativePath = fileName.Substring(fileName.IndexOf("Assets/"));
                T obj = AssetDatabase.LoadAssetAtPath(relativePath, typeof(T)) as T;
                BuildPipeline.BuildAssetBundle(obj, null, savePath + obj.name + ".assetbundle", options, targetPlatform);
                AssetDatabase.ImportAsset(string.Format("{0}/{1}{2}", Streaming, prefix, obj.name + ".assetbundle"), importOptions);
            }
        }
    }

	public static void BuildAll(BuildTarget targetPlatform)
	{
        
		BuildCommonLua(targetPlatform);
		
		BuildLogic(targetPlatform);
		
		BuildAllUILua(targetPlatform);
		
		BuildAllUIPrefab(targetPlatform);
		
		BuildAllStaticData(targetPlatform);
		
		BuildLocalization(targetPlatform);
		
		BuildBody(targetPlatform);

	    BuildAction(targetPlatform);
		
        BuildHead(targetPlatform);

        BuildNumber(targetPlatform);

        BuildScene(targetPlatform);

        BuildScatteredImg(targetPlatform);

	    BuildUniform(targetPlatform);

        BuildBattlePrefab(targetPlatform);

        BuildAI(targetPlatform);

        BuildBattleTables(targetPlatform);

        BuildCommonTables(targetPlatform);
    }

	private static void BuildCommonLua(BuildTarget targetPlatform)
    {
        CollectAll<TextAsset>("Lua/Common/", "/Resources/Lua/Common/", "*.txt", targetPlatform);
    }

	private static void BuildLogic(BuildTarget targetPlatform)
    {
        CollectAll<TextAsset>("Lua/", "/Resources/Lua/", "*.txt", targetPlatform);
        CollectAll<TextAsset>("Lua/Game/", "/Resources/Lua/Game/", "*.txt", targetPlatform);
        CollectAll<TextAsset>("Lua/Tutorial/", "/Resources/Lua/Tutorial/", "*.txt", targetPlatform);
    }
    private static void BuildBattleTables(BuildTarget targetPlatform)
    {
        CollectAll<TextAsset>("Tables/Battle/", "/Resources/Tables/Battle", "*.json", targetPlatform);
    }

    private static void BuildCommonTables(BuildTarget targetPlatform)
    {
        CollectAll<TextAsset>("Tables/Common/", "/Resources/Tables/Common", "*.json", targetPlatform);
    }

    private static void BuildBattlePrefab(BuildTarget targetPlatform)
    {
        CollectAll<GameObject>("Battle/", "/Resources/Battle", "*.prefab", targetPlatform);
    }
    private static void BuildAI(BuildTarget targetPlatform)
    {
        CollectAll<TextAsset>("Tables/AI/", "/Resources/Tables/AI", "*.json", targetPlatform);
    }
    private static void BuildAllUILua(BuildTarget targetPlatform)
    {
        CollectAll<TextAsset>("Lua/UILua/", "/Resources/Lua/UILua/", "*.txt", targetPlatform);
    }

	private static void BuildAllUIPrefab(BuildTarget targetPlatform)
    {
        var savePath = Versioned.PathPrefix() + "UI/";

        if (Directory.Exists(savePath) == false)
        {
            Directory.CreateDirectory(savePath);
        }

	    var commonSavePath = savePath + "UICommon/";

	    if (Directory.Exists(commonSavePath) == false)
	    {
	        Directory.CreateDirectory(commonSavePath);
	    }

	    var tutorialSavePath = Versioned.PathPrefix() + "Tutorial/";
	    if (Directory.Exists(tutorialSavePath) == false)
	    {
	        Directory.CreateDirectory(tutorialSavePath);
	    }

        BuildPipeline.PushAssetDependencies();
        var font = AssetDatabase.LoadAssetAtPath("Assets/UI/Font/msyh.ttf", typeof(Font));
        BuildPipeline.BuildAssetBundle(font, null, savePath + "SharedAssets.assetbundle", options, targetPlatform);

        var dirs = Directory.GetDirectories(Application.dataPath + "/Resources/UI", "*.*", SearchOption.AllDirectories);
        foreach (var dir in dirs)
        {
            if (dir.Contains("UICommon"))
            {
                var fileNames = Directory.GetFiles(dir);

                foreach (var fileName in fileNames)
                {
                    if (fileName.Contains(".meta")) continue;
                    if (fileName.Contains(".DS_Store")) continue;

                    BuildPipeline.PushAssetDependencies();
                    var relativePath = fileName.Substring(fileName.IndexOf("Assets/"));
                    var prefab = AssetDatabase.LoadAssetAtPath(relativePath, typeof(GameObject)) as GameObject;
                    BuildPipeline.BuildAssetBundle(prefab, null, commonSavePath + prefab.name + ".assetbundle",
                        options, targetPlatform);
                    AssetDatabase.ImportAsset(
                        string.Format("{0}{1}UI/UICommon/{2}.assetbundle", Streaming, Versioned.PlatformPath, prefab.name),
                        importOptions);
                    BuildPipeline.PopAssetDependencies();
                }
            }
            else
            {
                BuildPipeline.PushAssetDependencies();
                Object mainAsset = null;
                var folderName = Path.GetFileName(dir);
                var list = new List<Object>();

                var fileNames = Directory.GetFiles(dir);
                foreach (var fileName in fileNames)
                {
                    if (fileName.Contains(".meta")) continue;
                    if (fileName.Contains(".DS_Store")) continue;

                    var relativePath = fileName.Substring(fileName.IndexOf("Assets/"));
                    var prefab = AssetDatabase.LoadAssetAtPath(relativePath, typeof(GameObject)) as GameObject;
                    if (prefab.name == folderName)
                    {
                        mainAsset = prefab;
                    }
                    list.Add(prefab);
                }
                BuildPipeline.BuildAssetBundle(mainAsset, list.ToArray(), savePath + folderName + ".assetbundle",
                    options, targetPlatform);
                AssetDatabase.ImportAsset(
                    string.Format("{0}{1}UI/{2}.assetbundle", Streaming, Versioned.PlatformPath, folderName),
                    importOptions);

                list.Clear();
                list = null;
                mainAsset = null;
                BuildPipeline.PopAssetDependencies();
            }
        }

	    var tutorial = AssetDatabase.LoadAssetAtPath("Assets/Resources/Tutorial/TutorialMgr.prefab", typeof(GameObject));
        BuildPipeline.BuildAssetBundle(tutorial, null, tutorialSavePath + "TutorialMgr.assetbundle", options, targetPlatform);
        AssetDatabase.ImportAsset(
            string.Format("{0}{1}Tutorial/TutorialMgr.assetbundle", Streaming, Versioned.PlatformPath), importOptions);

        BuildPipeline.PopAssetDependencies();
    }

	private static void BuildAllStaticData(BuildTarget targetPlatform)
    {
        CollectAll<TextAsset>("", "/Resources/Tables/", "*.json", "StaticData.assetbundle", targetPlatform);
    }

	private static void BuildLocalization(BuildTarget targetPlatform)
    {
        Object obj1 = AssetDatabase.LoadAssetAtPath("Assets/Resources/Localization.txt", typeof(TextAsset));
        Object obj2 = AssetDatabase.LoadAssetAtPath("Assets/Resources/LanguageVersion.txt", typeof(TextAsset));

		BuildPipeline.BuildAssetBundle(obj1, null, Versioned.PathPrefix() + "Localization.assetbundle", options, targetPlatform);
        BuildPipeline.BuildAssetBundle(obj2, null, Versioned.PathPrefix() + "LanguageVersion.assetbundle", options, targetPlatform);
        
        AssetDatabase.ImportAsset(string.Format("{0}{1}Localization.assetbundle", Streaming, Versioned.PlatformPath), importOptions);
        AssetDatabase.ImportAsset(string.Format("{0}{1}LanguageVersion.assetbundle", Streaming, Versioned.PlatformPath), importOptions);
    }

	private static void BuildBody(BuildTarget targetPlatform)
    {
		CollectAll<GameObject>("Char/Body/", "/Resources/Char/Body/", "*.prefab", targetPlatform);
    }

    private static void BuildAction(BuildTarget targetPlatform)
    {
        CollectAll<GameObject>("Char/", "/Resources/Char/", "*.prefab", targetPlatform);
    }

	private static void BuildHead(BuildTarget targetPlatform)
    {
        CollectAll<GameObject>("Char/Head/", "/Resources/Char/Head", "*.prefab", targetPlatform);
        CollectAll<Texture>("Textures/Uniform/Head/", "/Resources/Textures/Uniform/Head", "*.jpg", targetPlatform);
    }

	private static void BuildNumber(BuildTarget targetPlatform)
    {
        CollectAll<Texture>("Textures/Number/", "/Resources/Textures/Number", "*.png", targetPlatform);
    }

    private static void BuildScene(BuildTarget targetPlatform)
    {
        CollectAll<GameObject>("Scene/", "/Resources/Scene", "*.prefab", targetPlatform);
    }

    private static void BuildScatteredImg(BuildTarget targetPlatform)
    {
        var root = "/Resources/Textures/ScatteredImg";
        var dirs = Directory.GetDirectories(Application.dataPath + root);
        for (int i = 0; i < dirs.Length; i++)
        {
            var dir = dirs[i].Replace(Application.dataPath, "");
            var folderName = dir.Replace(root, "");
            CollectAll<Texture>("Textures/ScatteredImg/" + folderName + "/", dir, "*.png", targetPlatform);
        }
    }

    private static void BuildUniform(BuildTarget targetPlatform)
    {
        CollectAll<Texture>("Textures/alpha/",         "/Resources/Textures/alpha",         "*.png", targetPlatform);
        CollectAll<Texture>("Textures/Uniform/hose/",  "/Resources/Textures/Uniform/hose",  "*.png", targetPlatform);
        CollectAll<Texture>("Textures/Uniform/shirt/", "/Resources/Textures/Uniform/shirt", "*.png", targetPlatform);
        CollectAll<Texture>("Textures/Uniform/shoes/", "/Resources/Textures/Uniform/shoes", "*.jpg", targetPlatform);
    }
}