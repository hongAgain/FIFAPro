using UnityEngine;
using UnityEditor;
using System;
using System.IO;

public class RecordDataFile
{
    private static FileList mOldFileList = null;
    private static FileList mCurCreating = null;

    static System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();

    [MenuItem("Resource/BuildFileList", false, 1)]
    public static void BuildDataFile()
    {
        AssetDatabase.Refresh();
        var fullRootPath = Versioned.PathPrefix();

        mOldFileList = new FileList();
        mCurCreating = new FileList();

        mOldFileList.ReadFileData(fullRootPath);

        TraversalFolder(fullRootPath);

        mCurCreating.Save(fullRootPath);
        mCurCreating.Save(Application.dataPath + "/");
		AssetDatabase.Refresh();
    }

    static void TraversalFolder(string folderPath_)
    {
        var theFolder = new DirectoryInfo(folderPath_);
        var dirInfo = theFolder.GetDirectories("*", SearchOption.AllDirectories);
        var allDirs = new DirectoryInfo[dirInfo.Length + 1];
        Array.Copy(dirInfo, 0, allDirs, 0, dirInfo.Length);
        allDirs[allDirs.Length - 1] = theFolder;

        foreach (var folder in allDirs)
        {
            var fileInfo = folder.GetFiles();
            foreach (var file in fileInfo)
            {
                //Debug.Log(file.Name);
                //Debug.Log(GetMD5StringFromFile(file.FullName));
                //Debug.Log(GetMD5Hash16FromFile(file.FullName));
                //Debug.Log(GetFilePathKey(file.FullName));
                if (IsMetaFile(file.Name) || file.Name.Contains(FileList.SAVE_FILE_NAME) || file.Name.Contains("DS_Store"))
                {
                    continue;
                }

                var filePathKey = GetFilePathKey(file.FullName);
                var md5Str = BitConverter.ToString(GetMD5HashFromFile(file.FullName));

                Versioned versioned = null;
                if (mOldFileList.m_FileDataList.TryGetValue(filePathKey, out versioned))
                {
                    //Debug.Log(versioned.ToJson());
                    versioned.OnBuild(md5Str);
                    //Debug.LogWarning(versioned.ToJson());
                }
                else
                {
                    versioned = new Versioned(filePathKey, md5Str, 1);
                }
                mCurCreating.m_FileDataList.Add(filePathKey, versioned);
            }
        }
    }

    static string GetFilePathKey(string directoryName_)
    {
        var dir = directoryName_.Replace("\\", "/");
        var root = Versioned.PathPrefix();
        var index = dir.IndexOf(root) + root.Length;
        var fileKey = directoryName_.Substring(index);
        fileKey = fileKey.Replace("\\", "/");

        return fileKey;
    }

    public static byte[] GetMD5HashFromFile(string fileName)
    {
        FileStream file = new FileStream(fileName, FileMode.Open);
        byte[] retVal = md5.ComputeHash(file);
        file.Close();
        return retVal;
    }

    public static string GetMD5Hash16FromFile(string fileName)
    {
        FileStream file = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
        byte[] retVal = md5.ComputeHash(file);
        file.Close();

        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        foreach (byte b in retVal)
        {
            sb.Append(b.ToString("x2"));
        }
        return sb.ToString();
    }

    public static string GetMD5StringFromFile(string fileName)
    {
        FileStream file = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
        byte[] hash_byte = md5.ComputeHash(file);
        string str = System.BitConverter.ToString(hash_byte);
        //        str = str.Replace("-", "");
        string[] charTemp = str.Split(new char[] { '-' });
        int md5Number = 0;
        int iPrefixNumber = Convert.ToInt32(charTemp[0], 16);
        foreach (string strTemp in charTemp)
        {
            md5Number += Convert.ToInt32(strTemp, 16);
        }

        //Debug.Log("md5Number: " + md5Number);
        return (iPrefixNumber.ToString() + md5Number.ToString());
    }

    public static bool IsMetaFile(string strFile_)
    {
        return strFile_.Contains(".meta");
    }
}