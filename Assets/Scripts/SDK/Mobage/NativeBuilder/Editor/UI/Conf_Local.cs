using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using System;
using System.IO;

namespace NativeBuilder
{
	public class Conf_Local : Conf_Base 
	{
		static string FILE_PATH = NativeBuilderUtility.UnityProjectPath + "/NativeBuilderConf/local.properties";	

		public Conf_Local() : base(FILE_PATH){}


		public override bool Exists()
		{
			FileInfo fi = new FileInfo(FILE_PATH);
			return fi.Exists;
		}

		public override void Repaire ()
		{
			if(!this.Exists())
			{
				this.Generate();
			}
			this.Reload();
			ConfUtility.SetDefualtIfNotExsist(this, "android.sdk", "<please edit this line>");
			ConfUtility.SetDefualtIfNotExsist(this, "ant.sdk", "<please edit this line>");

			//check
			while(!Android.CheckIsAndroidSDK(this["android.sdk"])){
				bool b = EditorUtility.DisplayDialog("Android SDK Location invalid:","Please set one", "Select", "Cancel Task");
				if(!b){
					throw new Exception("User Canceled in Android SDK Select");
				}
				var path = EditorUtility.OpenFolderPanel("Select Android SDK root foler", "", "");
				this["android.sdk"] = path;
			}
			while(!Ant.CheckIsAntSDK(this["ant.sdk"])){
				bool b = EditorUtility.DisplayDialog("Ant Location invalid:","Please set one", "Select", "Cancel Task");
				if(!b){
					throw new Exception("User Canceled in Ant Select");
				}
				var path = EditorUtility.OpenFolderPanel("Select Ant root foler", "", "");
				this["ant.sdk"] = path;
			}
			this.Save();
		}

		
		public override void Generate()
		{
			
			FileInfo fi = new FileInfo(FILE_PATH);
			if(fi.Exists){
				throw new IOException(fi.FullName + " exsists, can't GenerateLocal!");
			}
			var writer = fi.CreateText();
			string context = 
				@"
# This file must *NOT* be checked into Version Control Systems,
# as it contains information specific to your local configuration.
#
# ${project} means Unity Project dir

# android conf
android.sdk = <please edit this line>
ant.sdk = <please edit this line>
				";
			
			writer.Write(context);
			writer.Close();
			
			Debug.Log(fi.FullName + " has benn created");
			
		}

	}

}

