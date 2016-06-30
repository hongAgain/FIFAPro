using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using NativeBuilder.XCodeEditor;
using NativeBuilder.EclipseEditor;
using System.IO;
using System;

namespace NativeBuilder 
{
	
	public static class Configuration
	{

		static Conf_Gloable global = null;

		public static Conf_Gloable Gloable {
			get
			{
				if(global == null) global = new Conf_Gloable();
				return global;
			}
				
		}

		static Conf_Local local = null;
		public static Conf_Local Local
		{
			get
			{
				if(local == null) local = new Conf_Local();
				return local;
			}
		}

		public static void GenerateConfFolder()
		{
			// check main folder
			{
				DirectoryInfo di = new DirectoryInfo(NativeBuilderUtility.UnityProjectPath + "/NativeBuilderConf");
				if(!di.Exists){
					di.Create();
					Debug.Log(di.FullName + " has been created");
				}

			}
			// check conf
			if(!Gloable.Exists()){
				Gloable.Generate();
			}
			// check local
			if(!Local.Exists()){
				Local.Generate();
			}
			// check other
			// 如果在eupe目录下没有任何一个eupe，则生成一个默认的 user.eupe
			{
				var d = new DirectoryInfo(Gloable.AndroidSrcDir);
				var eupe_list = d.GetDirectories("*.eupe", SearchOption.TopDirectoryOnly);
				if(eupe_list.Length == 0)
				{
					DirectoryInfo eupe = new DirectoryInfo(Gloable.AndroidSrcDir + "/user.eupe");
					eupe.Create();
					Debug.Log(eupe.FullName + " has been created");
				}
			}
			// 如果在xupe目录下没有任何一个eupe，则生成一个默认的 user.xupe
			{
				var d = new DirectoryInfo(Gloable.AndroidSrcDir);
				var xupe_list = d.GetDirectories("*.xupe", SearchOption.TopDirectoryOnly);
				if(xupe_list.Length == 0)
				{
					DirectoryInfo xupe = new DirectoryInfo(Gloable.AndroidSrcDir + "/user.xupe");
					xupe.Create();
					Debug.Log(xupe.FullName + " has been created");
				}
			}
			DirectoryInfo lib = new DirectoryInfo(Gloable["android.lib.dir"]);
			if(!lib.Exists){
				lib.Create();
				Debug.Log(lib.FullName + " has been created");
			}
			Debug.Log("[NativeBuilder] Generate Conf Folder Completed");
		}
	}
}

