using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using NativeBuilder.XCodeEditor;
using NativeBuilder.EclipseEditor;
using System.IO;
using System;
using System.Linq;

namespace NativeBuilder 
{

	public class NativeBuilderUI {

		static string unityProjectPath =  Application.dataPath.Remove(Application.dataPath.LastIndexOf('/'));

		[MenuItem("NativeBuilder/Build/Android (Eclipse Project and Apk and Run)")]
		public static void Build_Android()
		{
			AndroidWindow.Show();
		}

		[MenuItem("NativeBuilder/Build/iOS (xCode)")]
		public static void Builder_iOS()
		{
			IOSWindow.Show();
		}

		[MenuItem("NativeBuilder/Generate Conf Folder")]
		public static void GenerateConfFolder()
		{
			Configuration.GenerateConfFolder();
		}

		[MenuItem("NativeBuilder/Online Document")]
		public static void OnlineDocument()
		{
			Application.OpenURL("https://wiki.dena.jp/pages/viewpage.action?pageId=84261813");
			
		}

		[MenuItem("NativeBuilder/Android/Open User Conf.xml")]
		public static void OpenAndroidConfFile()
		{
			var path = unityProjectPath + "/NativeBuilderConf/src/user.eupe/conf.xml";
			Debug.Log(path);
			EditorUtility.OpenWithDefaultApp(path);
		}

		[MenuItem("NativeBuilder/Android/Open Conf")]
		public static void OpenAndroidConf()
		{
			var path = unityProjectPath + "/NativeBuilderConf";
			Debug.Log(path);
			EditorUtility.OpenWithDefaultApp(path);
		}

		[MenuItem("NativeBuilder/Android/Open Product")]
		public static void OpenAndroidProduct()
		{
			var path = unityProjectPath + "/NativeBuilderProduct";
			Debug.Log(path);
			EditorUtility.OpenWithDefaultApp(path);
		}

	}
}


