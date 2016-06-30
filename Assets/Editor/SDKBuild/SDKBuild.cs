using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;
using System.Xml;

public class SDKBuild : EditorWindow
{
    private string mDefaultConfigPath = Application.dataPath + "/Editor/SDKBuild/SDKConfig.xml";
    private XmlDocument mConfigFile;
    private List<SDKConfig> mConfigList = new List<SDKConfig>();
    private string mNewConfigName = "";

    private SDKConfig mCurrent;
    private Dictionary<RuntimePlatform, BuildTargetGroup> platformBuildTargetDic;
    
    [MenuItem("Window/SDK_Config_Window")]
    static void ShowWindow()
    {
        SDKBuild instance = EditorWindow.GetWindow<SDKBuild>();
		instance.OnInit(EditorUserBuildSettings.activeBuildTarget);
        instance.Show();
    }

	void OnInit(BuildTarget targetPlatform)
    {
        mConfigFile = new XmlDocument();
        if (File.Exists(mDefaultConfigPath) == false)
        {
            XmlDeclaration decl = mConfigFile.CreateXmlDeclaration("1.0", "utf-8", "");
            mConfigFile.AppendChild(decl);

            XmlElement root = mConfigFile.CreateElement("Root");
            mConfigFile.AppendChild(root);
            CreateNewSDKConfig("NoSDK");

            mCurrent = mConfigList[0];
        }
        else
        {
            mConfigFile.Load(mDefaultConfigPath);

            foreach (XmlNode node in mConfigFile.DocumentElement.ChildNodes)
            {
                SDKConfig config = SDKConfig.Create(node as XmlElement);
                config.ReadFromXml();
                mConfigList.Add(config);
            }
        }
        
        BuildTargetGroup btg = BuildTargetGroup.Unknown;
		switch (targetPlatform)
        {
			case BuildTarget.StandaloneWindows:
			case BuildTarget.StandaloneOSXIntel:
			case BuildTarget.StandaloneOSXIntel64:
				btg = BuildTargetGroup.Standalone;
                break;
            case BuildTarget.iPhone:
                btg = BuildTargetGroup.iPhone;
                break;
            case BuildTarget.Android:
                btg = BuildTargetGroup.Android;
                break;
            case BuildTarget.WP8Player:
                btg = BuildTargetGroup.WP8;
                break;
            default:
                {
                    string errMsg = "Unsupported BuildTarget " + EditorUserBuildSettings.activeBuildTarget.ToString() + "!\n";
                    errMsg += "We only support: \n    "
						+ BuildTarget.StandaloneWindows.ToString() + "\n"
						+ BuildTarget.StandaloneOSXIntel.ToString() + "\n"
						+ BuildTarget.StandaloneOSXIntel64.ToString() + "\n"
						+ BuildTarget.iPhone.ToString() + "\n"
                        + BuildTarget.Android.ToString() + "\n"
                        + BuildTarget.WP8Player.ToString() + "\n";
                    Debug.LogError(errMsg);
                }
                return;
        }
        string groupMacro = PlayerSettings.GetScriptingDefineSymbolsForGroup(btg);
        string[] macros = groupMacro.Split(';');
		
//		Debug.Log("build debug Init groupMacro:"+groupMacro);
        foreach (string macro in macros)
        {
//			Debug.Log("build debug Init outside foreach macro:"+macro);
            foreach (SDKConfig a in mConfigList)
			{
//				Debug.Log("build debug Init inside foreach a:"+a.mName);				
				if (macro.Contains(a.mName))
                {
                    mCurrent = a;
//					Debug.Log("build debug Init inside same a:"+a.mName);
                    goto END_FIND_CURRENT;
                }
            }
        }
//		Debug.Log("build debug Init "+btg+" no match groupMacro:"+groupMacro);
    END_FIND_CURRENT:
        return;
    }

    void CreateNewSDKConfig(string configName)
    {
        XmlElement noneSDK = mConfigFile.CreateElement("SDK");
        mConfigFile.DocumentElement.AppendChild(noneSDK);
        SDKConfig config = SDKConfig.Create(configName, noneSDK);
        config.WriteToXml(mConfigFile);

        mConfigList.Add(config);

        Save();
    }

    void OnGUI()
    {
        float x = 500f;
        float y = 300f;

        GUILayout.BeginArea(new Rect(10f, 10f, position.width - 80f, position.height - 10f));
        for (int i = 0; i < mConfigList.Count; ++i)
        {
            GUI.Box(new Rect(0, i * y, x, y), "");
            GUILayout.BeginArea(new Rect(0, i * y, x, y));
            SDKConfig config = mConfigList[i];
            config.mScrollPos = GUILayout.BeginScrollView(config.mScrollPos);

            if (config.mWillDelete == false)
            {
                Color orgContent = GUI.contentColor;
                GUI.contentColor = Color.yellow;
                GUILayout.Label(config.mName);
                GUI.contentColor = orgContent;

                GUILayout.BeginHorizontal();

                GUILayout.BeginVertical();

                GUILayout.BeginHorizontal();
                #region SDK RELEVANT OBJS
                GUILayout.BeginVertical();
                Color org = GUI.backgroundColor;
                List<string> fileList = config.mFileList;
                for (int j = 0; j < config.mObjList.Count; ++j)
                {
                    GUILayout.BeginHorizontal();
                    UnityEngine.Object obj = EditorGUILayout.ObjectField(config.mObjList[j], typeof(UnityEngine.Object), false);
                    if (config.mObjList[j] != obj)
                    {
                        config.mObjList[j] = obj;
                        fileList[j] = AssetDatabase.GetAssetPath(obj);
                    }

                    GUI.backgroundColor = Color.red;
                    if (GUILayout.Button("-"))
                    {
                        fileList.RemoveAt(j);
                        config.mObjList.RemoveAt(j);
                        --j;
                    }
                    GUI.backgroundColor = org;
                    GUILayout.EndHorizontal();
                }
                GUILayout.EndVertical();

                GUI.backgroundColor = Color.green;
                if (GUILayout.Button(new GUIContent("+", "Add Relevant Objects"), GUILayout.MaxWidth(20f)))
                {
                    fileList.Add("");
                    config.mObjList.Add(null);
                }
                GUI.backgroundColor = org;
                #endregion
                GUILayout.EndHorizontal();

                GUILayout.EndVertical();

                GUILayout.FlexibleSpace();

                #region PLATFORMS
                List<BuildTargetGroup> platforms = config.mPlatforms;
                List<BuildTargetGroup> allPlatforms = new List<BuildTargetGroup>();
                allPlatforms.Add(BuildTargetGroup.Standalone);
                allPlatforms.Add(BuildTargetGroup.iPhone);
                allPlatforms.Add(BuildTargetGroup.Android);
                allPlatforms.Add(BuildTargetGroup.WP8);

                foreach (BuildTargetGroup btg in platforms)
                {
                    allPlatforms.Remove(btg);
                }

                string[] names = new string[allPlatforms.Count];
                for (int j = 0; j < names.Length; ++j)
                {
                    names[j] = allPlatforms[j].ToString();
                }
                GUILayout.BeginVertical();
                GUILayout.BeginHorizontal();
                if (names.Length > 0)
                {
                    config.mSelectPlatformIdx = EditorGUILayout.Popup("", config.mSelectPlatformIdx, names);

                    GUI.backgroundColor = Color.green;
                    if (GUILayout.Button(new GUIContent("+", "Add NewPlatform"), GUILayout.MaxWidth(20f)))
                    {
                        config.mPlatforms.Add(allPlatforms[config.mSelectPlatformIdx]);
                    }
                    GUI.backgroundColor = org;
                }
                GUILayout.EndVertical();
                for (int j = 0; j < config.mPlatforms.Count; ++j)
                {
                    GUILayout.BeginHorizontal();
                    GUI.enabled = false;
                    EditorGUILayout.Popup(0, new string[1] { config.mPlatforms[j].ToString() });
                    GUI.enabled = true;
                    GUI.backgroundColor = Color.red;
                    if (GUILayout.Button("-"))
                    {
                        config.mPlatforms.RemoveAt(j);
                        --j;
                    }
                    GUI.backgroundColor = org;
                    GUILayout.EndHorizontal();
                }
                GUILayout.EndVertical();
                #endregion

                GUILayout.EndHorizontal();

                #region SDK SAVE AND SWITCH
                if (GUILayout.Button("Save"))
                {
                    config.WriteToXml(mConfigFile);
                    Save();
                }

                GUI.enabled = (mCurrent != config);
                if (GUILayout.Button(GUI.enabled ? "Switch To This" : "Current is " + config.mName))
                {
                    if (mCurrent != null)
                    {
                        foreach (string path in mCurrent.mFileList)
                        {
                            UnInstall(path);
                        }
                    }

                    foreach (string path in config.mFileList)
                    {
                        Install(config.mName, path);
                    }

                    foreach (BuildTargetGroup btg in config.mPlatforms)
                    {
                        string groupMacro = PlayerSettings.GetScriptingDefineSymbolsForGroup(btg);
                        if (mCurrent != null)
                        {
                            groupMacro = groupMacro.Replace("Enable_" + mCurrent.mName + ";", "");
                            groupMacro = groupMacro.Replace("Enable_" + mCurrent.mName, "");
                        }

                        if (groupMacro.Length == 0)
                        {
                            groupMacro += "Enable_" + config.mName;
                        }
                        else
                        {
                            groupMacro += ";Enable_" + config.mName;
                        }
                        PlayerSettings.SetScriptingDefineSymbolsForGroup(btg, groupMacro);
                    }
                    mCurrent = config;
                }
                GUI.enabled = true;

                GUI.backgroundColor = Color.red;
                if (GUILayout.Button("Delete"))
                {
                    config.mWillDelete = true;
                }
                GUI.backgroundColor = org;
                #endregion

            }
            else
            {
                Color org = GUI.backgroundColor;

                GUILayout.Label("Are you sure to \ndelete config \"" + config.mName + "\"?");

                GUILayout.BeginHorizontal();
                GUI.backgroundColor = Color.red;
                if (GUILayout.Button("Yes"))
                {
                    mConfigFile.DocumentElement.RemoveChild(config.mElement);
                    mConfigList.RemoveAt(i);
                    --i;
                    Save();
                }

                GUI.backgroundColor = Color.green;
                if (GUILayout.Button("No"))
                {
                    config.mWillDelete = false;
                }
                GUILayout.EndHorizontal();

                GUI.backgroundColor = org;
            }
            GUILayout.EndScrollView();
            GUILayout.EndArea();
            GUILayout.FlexibleSpace();
        }

        GUILayout.EndArea();

        mNewConfigName = GUI.TextField(new Rect(position.width - 80f, 10f, 70f, 20f), mNewConfigName);
        if (GUI.Button(new Rect(position.width - 80f, 30f, 70f, 20f), "New SDK"))
        {
            CreateNewSDKConfig(mNewConfigName);
        }

        if (GUI.Button(new Rect(position.width - 80f, position.height - 30f, 70f, 20f), "Reload"))
        {
			OnInit(EditorUserBuildSettings.activeBuildTarget);
        }
    }

	private static void SwitchSDK(string sdkName,BuildTarget targetPlatform)
	{
		SDKBuild instance = EditorWindow.GetWindow<SDKBuild>();
		instance.OnInit(targetPlatform);
		instance.SwitchToSpecificSDK(sdkName);
		instance.Close();
	}
	
	public void SwitchToSpecificSDK(string sdkName)
	{
		SDKConfig targetConfig = null;
		for (int i = 0; i < mConfigList.Count; ++i)
		{
			if(!mConfigList[i].mName.Equals(sdkName))
				continue;
			//switch to this sdk env
			targetConfig = mConfigList[i];
			if (mCurrent != null)
			{				
				foreach (string path in mCurrent.mFileList)
				{
					UnInstall(path);
				}
			}				
			foreach (string path in targetConfig.mFileList)
			{
				Install(targetConfig.mName, path);
			}				
			foreach (BuildTargetGroup btg in targetConfig.mPlatforms)
			{
				Debug.LogError("build debug BuildTargetGroup:"+btg);
				string groupMacro = PlayerSettings.GetScriptingDefineSymbolsForGroup(btg);
				if (mCurrent != null)
				{
					Debug.LogError("build debug groupMacro:"+groupMacro);
					groupMacro = groupMacro.Replace("Enable_" + mCurrent.mName + ";", "");
					groupMacro = groupMacro.Replace("Enable_" + mCurrent.mName, "");
//					Debug.LogError("build debug mCurrent.mName:"+mCurrent.mName);
				}
//				Debug.LogError("build debug groupMacro after:"+groupMacro);
				if (groupMacro.Length == 0)
				{
					groupMacro += "Enable_" + targetConfig.mName;
				}
				else
				{
					groupMacro += ";Enable_" + targetConfig.mName;
				}
				Debug.LogError("build debug groupMacro final:"+groupMacro);
				PlayerSettings.SetScriptingDefineSymbolsForGroup(btg, groupMacro);
			}
			mCurrent = targetConfig;
			mCurrent.WriteToXml(mConfigFile);
			Save();
		}
	}
//	[MenuItem("BuildCommands/SwitchToNoSDK")]
	public static void SwitchToNoSDKInEditor()
	{
		SwitchToNoSDK (EditorUserBuildSettings.activeBuildTarget);
	}

	public static void SwitchToNoSDK(BuildTarget targetPlatform)
	{
		SwitchSDK("NoSDK",targetPlatform);
		AssetDatabase.Refresh();
	}

//	[MenuItem("BuildCommands/SwitchToMobageSDK")]
	public static void SwitchToMobageSDKInEditor()
	{
		SwitchToMobageSDK (EditorUserBuildSettings.activeBuildTarget);
	}

	public static void SwitchToMobageSDK(BuildTarget targetPlatform)
	{
		SwitchSDK("Mobage",targetPlatform);
		AssetDatabase.Refresh();
	}

	public static void SwitchResOrABMode(bool toRes, BuildTargetGroup targetPlatformG)
	{		
		string groupMacro = PlayerSettings.GetScriptingDefineSymbolsForGroup(targetPlatformG);
		if (toRes)
		{
			Debug.LogError("Switch to use Res in platform:"+targetPlatformG);
			if(groupMacro.Contains(Macro.USE_ASSETBUNDLE))
			{
				groupMacro.Replace(";"+Macro.USE_ASSETBUNDLE,"");
			}
		} 
		else 
		{
			Debug.LogError("Switch to use AB in platform:"+targetPlatformG);
			if(!groupMacro.Contains(Macro.USE_ASSETBUNDLE))
			{
				groupMacro += ";" + Macro.USE_ASSETBUNDLE;
			}
		}
		PlayerSettings.SetScriptingDefineSymbolsForGroup(targetPlatformG, groupMacro);
	}

    void Save()
    {
        mConfigFile.Save(mDefaultConfigPath);
        int idx = mDefaultConfigPath.IndexOf("Assets");
        AssetDatabase.ImportAsset(mDefaultConfigPath.Substring(idx), ImportAssetOptions.ForceSynchronousImport | ImportAssetOptions.ForceUpdate);
    }

    void Install(string configName, string projectPath)
    {
        int idx = projectPath.LastIndexOf("/");
        string s = projectPath.Substring(idx + 1);
        string dirFromPath = Application.dataPath + "/../SDK/" + configName + "/" + s;

        FileInfo info = new FileInfo(dirFromPath);
        if ((info.Attributes & FileAttributes.Directory) != 0)
        {
            s = projectPath.Replace("Assets/", "");

            string dirToPath = Application.dataPath + "/" + s;
            Debug.Log("Install Copy folder from [" + dirFromPath + "] to [" + dirToPath + "]");
            FIF_FileUtil.CopyFolderTo(dirFromPath + "/", dirToPath);
        }
        else
        {
            s = projectPath.Replace("Assets/", "");

            string dirToPath = Application.dataPath + "/" + s;
            Debug.Log("Install Copy file from [" + dirFromPath + "] to [" + dirToPath + "]");
            FIF_FileUtil.CopyFileTo(dirFromPath, dirToPath);
        }
    }

    void UnInstall(string projectPath)
    {
        string s = projectPath.Replace("Assets/", "");
        string dirFromPath = Application.dataPath + "/" + s;

        FileInfo info = new FileInfo(dirFromPath);
        if ((info.Attributes & FileAttributes.Directory) != 0)
        {
            int idx = projectPath.LastIndexOf("/");
            string folder = projectPath.Substring(idx + 1);

            string dirToPath = Application.dataPath + "/../SDK/" + mCurrent.mName + "/" + folder + "/";
            Debug.Log("UnInstall Move folder from [" + dirFromPath + "] to [" + dirToPath + "]");
            FIF_FileUtil.CopyFolderTo(dirFromPath + "/", dirToPath);

            Directory.CreateDirectory(dirFromPath + "/").Delete(true);
        }
        else
        {
            string dirToPath = Application.dataPath + "/../SDK/" + mCurrent.mName + "/" + info.Name;
            Debug.Log("UnInstall Move file from [" + dirFromPath + "] to [" + dirToPath + "]");
            FIF_FileUtil.CopyFileTo(dirFromPath, dirToPath);
            info.Delete();
        }
    }
}

class SDKConfig
{
    public string mName = "None";
    public List<string> mFileList = new List<string>();

#if UNITY_EDITOR
    public XmlElement mElement;
    public List<UnityEngine.Object> mObjList = new List<UnityEngine.Object>();
    public bool mWillDelete;
    public Vector2 mScrollPos;
    public List<BuildTargetGroup> mPlatforms = new List<BuildTargetGroup>();
    public int mSelectPlatformIdx = 0;
#endif

    public void ReadFromXml()
    {
        //mName = mElement.GetAttribute("Name");
        XmlElement element = null;
        element = mElement.ChildNodes[0] as XmlElement;

        for (int i = 0; i < element.ChildNodes.Count; ++i)
        {
            mFileList.Add(element.ChildNodes[i].InnerText);
            mObjList.Add(AssetDatabase.LoadAssetAtPath(mFileList[i], typeof(UnityEngine.Object)));
        }

        element = mElement.ChildNodes[1] as XmlElement;
        if (element == null) return;
        for (int i = 0; i < element.ChildNodes.Count; ++i)
        {
            mPlatforms.Add((BuildTargetGroup)Enum.Parse(typeof(BuildTargetGroup), element.ChildNodes[i].InnerText));
        }
    }

    /*
       Format:
        <SDK Name="Mobage">
            <FilePaths>
                <Path0>""</Path0>
            </FilePaths>
            <Platforms>
                <Platform0>""</Platform0>
            </Platforms>
        </SDK>
     */

    public void WriteToXml(XmlDocument doc)
    {
        mElement.SetAttribute("Name", mName);

        FillElement<string>("FilePaths", "Path", mFileList, doc);
        FillElement<BuildTargetGroup>("Platforms", "Platform", mPlatforms, doc);
    }

    void FillElement<T>(string name, string nodePrefix, List<T> values, XmlDocument doc)
    {
        XmlElement element = null;
        if (mElement.HasChildNodes)
        {
            foreach (XmlNode node in mElement.ChildNodes)
            {
                if (node.Name == name)
                {
                    element = node as XmlElement;
                    break;
                }
            }
        }
        if (element == null)
        {
            element = doc.CreateElement(name);
            mElement.AppendChild(element);
        }

        int i = 0;
        for (; i < values.Count; ++i)
        {
            if (i < element.ChildNodes.Count)
            {
                element.ChildNodes[i].InnerText = values[i].ToString();
            }
            else
            {
                XmlNode node = doc.CreateElement(nodePrefix + i.ToString());
                node.InnerText = values[i].ToString();
                element.AppendChild(node);
            }
        }

        for (; i < element.ChildNodes.Count; )
        {
            element.RemoveChild(element.ChildNodes[i]);
        }
    }

    public static SDKConfig Create(string configName, XmlElement element)
    {
        SDKConfig config = new SDKConfig();
        config.mName = configName;
        config.mElement = element;

        return config;
    }

    public static SDKConfig Create(XmlElement element)
    {
        if (element == null) return null;
        else if (element.HasAttribute("Name") == false) return null;
        
        string name = element.GetAttribute("Name");
        return Create(name, element);
    }
}