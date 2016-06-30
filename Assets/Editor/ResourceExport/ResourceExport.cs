using System.IO;
using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;

public static class ResourceExport
{
    

    [MenuItem("Resource/ExportAllConfigTable To Resources", false, 3)]
	public static void ExportAllConfig()
	{
		int count = 0;
		string[] files = Directory.GetFiles(ConfigTablePath, ConfigTableFilter,SearchOption.AllDirectories);
		List<string> needImportFiles = new List<string>();
		foreach(string file in files)
		{
			StreamReader sr = File.OpenText(file);
			string fileName = Path.GetFileName(file);
			string newFileName = ConfigTableExportPath + fileName + TextFileExtens;
			
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
		Debug.Log(count + " json files export ");
	}

    [MenuItem("Resource/Update Head", false, 4)]
    public static void UpdateHead()
    {
        string fromPath = Application.dataPath + "/../../FIFA/Art/3D/Animation/FBX/Head/";
        string toHeadPath = Application.dataPath + "/FBX/Head/";
        string toHeadTexPath = Application.dataPath + "/Resources/Textures/Uniform/Head/";

        List<string> fbxNameCache = new List<string>();
        string[] fbxPaths = Directory.GetFiles(fromPath, "*.fbx", SearchOption.AllDirectories);
        for (int i = 0; i < fbxPaths.Length; i++)
        {
            string from = fbxPaths[i];
            string name = from.Substring(from.LastIndexOf('\\') + 1);
            File.Copy(from, toHeadPath + name, true);

            fbxNameCache.Add(name.Substring(0, name.LastIndexOf('.')));
        }

        string[] texPaths = Directory.GetFiles(fromPath, "*.jpg", SearchOption.AllDirectories);
        for (int i = 0; i < texPaths.Length; i++)
        {
            string from = texPaths[i];
            string name = from.Substring(from.LastIndexOf('\\') + 1);
            File.Copy(from, toHeadTexPath + name, true);

            var a = name.Substring(0, name.LastIndexOf('.'));
            if (fbxNameCache.Contains(a))
            {
                fbxNameCache.Remove(a);
            }
            else
            {
                Debug.LogError("Cannot find fbx file " + a);
            }
        }

        Debug.Log("-------------------------");
        for (int i = 0; i < fbxNameCache.Count; ++i)
        {
            Debug.LogError("Cannot find jpg file " + fbxNameCache[i]);
        }

        AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport | ImportAssetOptions.Default);
        AssetDatabase.SaveAssets();
    }

    [MenuItem("Resource/Create Head Prefab", false, 5)]
    static void CreateHeadPrefab()
    {
        string headPrefabPath = "/Resources/Char/Head/";
        string headTex = "/Resources/Textures/Uniform/Head/";
        string path = "/FBX/Head/";
        string[] filePaths = Directory.GetFiles(Application.dataPath + path, "*.fbx", SearchOption.TopDirectoryOnly);
        ImportAssetOptions options = ImportAssetOptions.ForceSynchronousImport | ImportAssetOptions.ForceUpdate;

        if (Directory.Exists(Application.dataPath + headPrefabPath) == false)
        {
            Directory.CreateDirectory(Application.dataPath + headPrefabPath);
        }

        foreach (string filePath in filePaths)
        {
            //if (filePath.Contains(".DS_Store")) continue;
            //if (filePath.Contains(".meta")) continue;

            string relative = filePath.Substring(filePath.IndexOf("Assets/"));
            int from = filePath.LastIndexOf("/") + 1;
            int to = filePath.LastIndexOf(".");
            string fileName = filePath.Substring(from, to - from);
            string headTexPath = "Assets" + headTex + fileName + ".jpg";

            TextureImporter ti = TextureImporter.GetAtPath(headTexPath) as TextureImporter;
            ti.textureType = TextureImporterType.Advanced;
            ti.mipmapEnabled = false;
            ti.textureFormat = TextureImporterFormat.ARGB32;
            AssetDatabase.ImportAsset(headTexPath, options);

            GameObject fbx = AssetDatabase.LoadAssetAtPath(relative, typeof (GameObject)) as GameObject;
            //Texture tex = AssetDatabase.LoadAssetAtPath(headTexPath, typeof(Texture)) as Texture;

            GameObject head = new GameObject("Head");
            GameObject clone = Object.Instantiate(fbx) as GameObject;
            clone.name = clone.name.Replace("(Clone)", "");
            Object.DestroyImmediate(clone.GetComponent<Animator>());
            clone.transform.parent = head.transform;
            clone.transform.localPosition = Vector3.zero;
            //clone.transform.localScale = Vector3.one * 0.97f;
            MeshRenderer renderer = clone.GetComponentInChildren<MeshRenderer>();
            renderer.receiveShadows = false;
       //     head.AddComponent<AvatarHead>().Head = renderer;

            string prefabPath = "Assets" + headPrefabPath + fileName + ".prefab";
            PrefabUtility.CreatePrefab(prefabPath, head, ReplacePrefabOptions.Default);
            AssetDatabase.ImportAsset(prefabPath, options);
            Object.DestroyImmediate(head);
        }

        AssetDatabase.SaveAssets();
        Resources.UnloadUnusedAssets();
    }

    static string TextFileExtens = ".txt";

	static string ConfigTableFilter = "*.json";
	static string ConfigTablePath = Application.dataPath + "/../../config/client/";
    static string ConfigTableExportPath = Application.dataPath + "/Resources/ConfigTemplate/";

}
