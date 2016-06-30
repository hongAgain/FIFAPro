using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEditor;

namespace NativeBuilder
{
	public class Conf_Gloable : Conf_Base {


		static string FILE_PATH = NativeBuilderUtility.UnityProjectPath + "/NativeBuilderConf/global.properties";

		public Conf_Gloable() : base(FILE_PATH){}

		public string TempDir { get { return TryGet("temp.dir", null); } }
		public string AndroidSrcDir { get { return TryGet("android.src.dir", null); } }
		public string IOSSrcDir { get { return TryGet("ios.src.dir", null ); } }

		public string Eclipse_Project_Backup_Home {
			get 
			{
				return TempDir + "/android/eclipse_project_backup";
			}
		}

		public string XCode_Project_Backup_Home {
			get
			{
				return TempDir + "/iOS/xCode_project_backup" ;
			}

		}

		public override void Repaire ()
		{
			if(!this.Exists())
			{
				this.Generate();
			}
			this.Reload();
			ConfUtility.SetDefualtIfNotExsist(this, "android.src.dir", "${project}/NativeBuilderConf/src/");
			//ConfUtility.SetDefualtIfNotExsist(this, "android.conf", "${project}/NativeBuilderConf/src/user.eupe");
			ConfUtility.SetDefualtIfNotExsist(this, "android.lib.dir", "${project}/NativeBuilderConf/lib/");
			ConfUtility.SetDefualtIfNotExsist(this, "android.project", "${project}/NativeBuilderProduct/eclipse_project");
			ConfUtility.SetDefualtIfNotExsist(this, "android.apk", "${project}/NativeBuilderProduct/${product_name}.apk");
			//ConfUtility.SetDefualtIfNotExsist(this, "ios.conf", "${project}/NativeBuilderConf/src/user.xupe");
			ConfUtility.SetDefualtIfNotExsist(this, "ios.src.dir", "${project}/NativeBuilderConf/src/");
			ConfUtility.SetDefualtIfNotExsist(this, "ios.project", "${project}/NativeBuilderProduct/xCode_project");
			ConfUtility.SetDefualtIfNotExsist(this, "temp.dir", "${project}/NativeBuilderProduct/temp");
			this.Save();
		
		}
					

		public override bool Exists(){
			FileInfo fi = new FileInfo(FILE_PATH);
			return fi.Exists;
		}

		public override void Generate()
		{
			if(this.Exists()){
				throw new IOException(FILE_PATH + " exsists, can't GenerateConf!");
			}
			var writer = new FileInfo(FILE_PATH).CreateText();
			string context = 
				@"
# This file is automatically generated by NativeBuilder.
# You can modify this file -- delete this file NativeBuilder will generat another one
# This file must be checked in Version Control Systems.
#
# ${project} means Unity Project dir
# ${product_name} means Unity Product name

# android conf
android.src.dir = ${project}/NativeBuilderConf/src/
android.lib.dir = ${project}/NativeBuilderConf/lib/
android.project = ${project}/NativeBuilderProduct/eclipse_project
android.apk = ${project}/NativeBuilderProduct/${product_name}.apk

# iOS conf
ios.src.dir = ${project}/NativeBuilderConf/src/
ios.project = ${project}/NativeBuilderProduct/xCode_project

# global
temp.dir = ${project}/NativeBuilderProduct/temp
				";
			
			writer.Write(context);
			writer.Close();

			this.Reload();
			Debug.Log(FILE_PATH + " has been created");
			
		}
	}

}

