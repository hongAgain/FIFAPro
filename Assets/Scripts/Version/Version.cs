using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class Versioned
{
//    public static string PlatformPath()
//    {
//#if UNITY_ANDROID
//        return "/Android/";
//#elif UNITY_IPHONE
//        return "/IPhone/";
//#else
//        return "/WP/";
//#endif
//    }

	//recoded for unrecognized buildtarget in batchmode
	private static string _platformPath = null;
	public static string PlatformPath
	{
		get
		{
			if(_platformPath==null)
			{
				#if UNITY_ANDROID
			    	return "/Android/";
				#elif UNITY_IPHONE
				    return "/IPhone/";
				#else
				    return "/WP/";
				#endif
			}
			else
			{
				return _platformPath;
			}
		}
		set
		{
			_platformPath = value;
		}
	}

    public static string PathPrefix()
    {
		//Debug.LogError("PathPrefix check : "+Application.streamingAssetsPath + PlatformPath);
        return Application.streamingAssetsPath + PlatformPath;
    }

    private String mFile = "";
    public String File { get { return mFile; } }

    private string mMD5 = "";
    public string MD5 { get { return mMD5; } }

    private int mVersion = 1;

    private bool mValid = true;
    public bool Valid { get { return mValid; } }

    public int Version
    {
        get { return mVersion; }
        set { mVersion = value; }
    }
    
    public int TotalLength { get { return ToJson().Length; } }

#if UNITY_EDITOR
    public void OnBuild(string newMD5)
    {
        if (mMD5 != newMD5)
        {
            mMD5 = newMD5;
            ++mVersion;
        }
    }
#endif

    public Versioned(string json)
    {
        FromJson(json);
    }

    public Versioned(string file, string md5, int version)
    {
        mFile = file;
        mMD5 = md5;
        mVersion = version;
    }

    public void FromJson(string json)
    {
        var jsonObj = NGUIJson.jsonDecode(json);
        if (jsonObj != null)
        {
            var data = (System.Collections.Hashtable)jsonObj;
            mFile = (string)data["File"];
            mMD5 = (string)data["MD5"];
            mVersion = (int)(double)data["Version"];

            mValid = true;
        }
        else
        {
            mValid = false;
        }
    }

    public string ToJson()
    {
        return "{" + "\"File\":\"" + mFile + "\"," + "\"MD5\":\"" + mMD5 + "\"," + "\"Version\":" + mVersion + "}\n";
    }
}

public class FileList
{
    public static string SavePath()
    {
        return Application.persistentDataPath + "/";
    }

    public static void DeleteLocalFileList()
    {
        Caching.CleanCache();

        foreach (var file in Directory.GetFiles(SavePath(), "*.*", SearchOption.TopDirectoryOnly))
        {
            File.Delete(file);
        }

        foreach (var dir in Directory.GetDirectories(SavePath(), "*.*", SearchOption.TopDirectoryOnly))
        {
            Directory.Delete(dir, true);
        }
    }

    public readonly Dictionary<string, Versioned> m_FileDataList = new Dictionary<string, Versioned>();

    public const string SAVE_FILE_NAME = "FileDataPath.txt";

    public void ReadFileData(string strPath)
    {
        var fullPath = strPath + SAVE_FILE_NAME;
        if (!File.Exists(fullPath))
        {
            return;
        }

        var srReadFile = new StreamReader(fullPath);
        FillData(srReadFile.ReadToEnd());
        srReadFile.Close();
    }

    public void FillData(string str)
    {
        var splits = str.Split('\n');
        foreach (var temp in splits)
        {
            if (!string.IsNullOrEmpty(temp))
            {
                var file = new Versioned(temp);
                if (file.Valid) m_FileDataList.Add(file.File, file);
            }
        }
    }

    public void Update(string key, Versioned value)
    {
        m_FileDataList[key] = value;
        Save(SavePath());
    }
    
    public void Save(string prefix)
    {
        var fullPath = prefix + SAVE_FILE_NAME;
        using (var fileStream = File.Open(fullPath, FileMode.Create))
        {
            var content = "";
            foreach (var data in m_FileDataList)
            {
                content += data.Value.ToJson();
            }

            var buffer = Encoding.UTF8.GetBytes(content.ToCharArray());

            fileStream.Write(buffer, 0, buffer.Length);
            fileStream.Close();
        }
    }

    public List<string> Compare(FileList latest)
    {
        var needUpdateList = new List<string>();

        foreach (var kvp in latest.m_FileDataList)
        {
            if (m_FileDataList.ContainsKey(kvp.Key))
            {
                if (m_FileDataList[kvp.Key].Version != kvp.Value.Version)
                {
                    needUpdateList.Add(kvp.Key);
                }
            }
            else
            {
                needUpdateList.Add(kvp.Key);
            }
        }

        return needUpdateList;
    }
}