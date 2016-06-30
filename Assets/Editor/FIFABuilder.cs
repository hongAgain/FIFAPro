using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;
using UnityEngine;
using UnityEditor;

public class FIFABuilder : MonoBehaviour
{
    [MenuItem("FIFA Editor/CopyLua", false, 1)]
    public static void CopyLuaToResource()
    {

        string LuaPath = Application.dataPath + "/Lua/";
        string LuaTextPath = Application.dataPath + "/Resources/Lua/";
        string LuaFileFilter = "*.lua";
        string TextFileExtens = ".txt";
        int count = 0;
        string[] files = Directory.GetFiles(LuaPath, LuaFileFilter, SearchOption.AllDirectories);
        List<string> needImportFiles = new List<string>();
        foreach (string file in files)
        {
            StreamReader sr = File.OpenText(file);
            string fileName = Path.GetFileName(file);
            string pathName = Path.GetDirectoryName(file) + "/";
            pathName = pathName.Replace(LuaPath, LuaTextPath);
            fileName = fileName.Replace(".lua", TextFileExtens);
            string newFileName = pathName + fileName;

            if (Directory.Exists(pathName) == false)
            {
                Directory.CreateDirectory(pathName);
            }

            if (File.Exists(newFileName))
            {
                File.Delete(newFileName);
            }
            int index = newFileName.IndexOf("Assets");
            string importPath = newFileName.Substring(index);
            needImportFiles.Add(importPath);

            StreamWriter sw = File.CreateText(newFileName);
            sw.Write(sr.ReadToEnd());
            sw.Close();
            sr.Close();
            ++count;
        }

        foreach (string newFile in needImportFiles)
        {
            AssetDatabase.ImportAsset(newFile, ImportAssetOptions.ForceSynchronousImport | ImportAssetOptions.ForceUpdate);
        }
        Debug.Log(count + " lua files export ");
    }
	
	[MenuItem("FIFA Editor/Lua/Gen Lua Wrap Files", false, 2)]
	public static void Binding()
	{
		LuaBinding.Binding ();
	}
	
	[MenuItem("FIFA Editor/Lua/Clear LuaBinder File + Wrap Files", false, 3)]
	public static void ClearLuaBinder ()
	{
		LuaBinding.ClearLuaBinder ();
	}


	[MenuItem("FIFA Editor/Build Commands/SwitchToNoSDK", false, 4)]
	public static void SwitchToNoSDK()
	{
		SDKBuild.SwitchToNoSDK (EditorUserBuildSettings.activeBuildTarget);
	}
		
	[MenuItem("FIFA Editor/Build Commands/SwitchToMobageSDK", false, 5)]
	public static void SwitchToMobageSDK()
	{
		SDKBuild.SwitchToMobageSDK (EditorUserBuildSettings.activeBuildTarget);
	}

	[MenuItem("FIFA Editor/Build Commands/BuildAndroid", false, 16)]
	public static void BuildAndroid()
	{
        BuildCommands.Build(BuildTarget.Android, BuildTargetGroup.Android, false, false);
	}
	
	[MenuItem("FIFA Editor/Build Commands/BuildiOS", false, 17)]
	public static void BuildiOS()
	{
        BuildCommands.Build(BuildTarget.iPhone, BuildTargetGroup.iPhone, false, false);
	}

	[MenuItem("FIFA Editor/Build Commands/BuildAndroid_R", false, 18)]
	public static void BuildAndroid_R()
	{
        BuildCommands.Build(BuildTarget.Android, BuildTargetGroup.Android, false, true);
	}
	
	[MenuItem("FIFA Editor/Build Commands/BuildiOS_R", false, 19)]
	public static void BuildiOS_R()
	{
        BuildCommands.Build(BuildTarget.iPhone, BuildTargetGroup.iPhone, false, true);
	}

    [MenuItem("FIFA Editor/Build Commands/BuildAndroid_Mobage_R", false, 20)]
    public static void BuildAndroid_Mobage_R()
    {
        BuildCommands.Build(BuildTarget.Android, BuildTargetGroup.Android, true, true);
    }

    [MenuItem("FIFA Editor/Build Commands/BuildiOS_Mobage_R", false, 21)]
    public static void BuildiOS_Mobage_R()
    {
        BuildCommands.Build(BuildTarget.iPhone, BuildTargetGroup.iPhone, true, true);
    }
}