using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System;
using UnityEditor;
using System.Diagnostics;

namespace NativeBuilder
{
	public class Ant {

		public static FileInfo getAntFormAntSDK(string ant_path)
		{
			FileInfo ant;
			{
				var di = new DirectoryInfo(ant_path);
				if(!di.Exists)
				{
					throw new IOException("ant sdk not exsists!");
				}
				var ANDROID = OSUtil.Platform == Platform.Mac ? "ant" : "ant.bat";
				var files = di.GetFiles(ANDROID, SearchOption.AllDirectories);
				if(files.Length == 0)
				{
					throw new IOException("None '" + ANDROID + "' was found in ant sdk");
				}
				else if(files.Length > 1)
				{
					UnityEngine.Debug.Log("Mutiple '" + ANDROID + "' was found, use this one: " + files[0].FullName);
				}
				ant = files[0];
			}
			return ant;
		}

		public static bool CheckIsAntSDK(string path)
		{
			if (!Directory.Exists (path))
			{
				return false;
			}
			
			// check folder exists
			DirectoryInfo di = new DirectoryInfo(path);
			if(!di.Exists) return false;
			
			// check adb
			var ADB = OSUtil.Platform == Platform.Mac ? "ant" : "ant.bat";
			if(di.GetFiles(ADB, SearchOption.AllDirectories).Length == 0) return false;
			
			return true;
		}

		
		public static void ant_clean(string projectPath, string ant_path)
		{
			var ant = getAntFormAntSDK(ant_path);
			
			var code = Exec.Run(ant.FullName, "clean -buildfile " + projectPath + "/build.xml");
			//var code = Exec.RunEx(ant.FullName, false, "clean", "-buildfile", projectPath + "/build.xml"); 
			if(code != 0){
				throw new IOException("Error in ant clean");
			}
			
		}
		
		public static void ant_debug(string projectPath, string ant_path)
		{
			var ant = getAntFormAntSDK(ant_path);
			
			var code2 = Exec.Run(ant.FullName, "debug -buildfile " + projectPath + "/build.xml");
			//var code2 = Exec.RunEx(ant.FullName, false, "debug", "-buildfile", projectPath + "/build.xml");
			if(code2 != 0){
				throw new IOException("Error in ant debug");
			}
		}
		
		public static void ant_release(string projectPath, string ant_path)
		{
			var ant = getAntFormAntSDK(ant_path);
			
			var code2 = Exec.Run(ant.FullName, "release -buildfile " + projectPath + "/build.xml");
			//var code2 = Exec.RunEx(ant.FullName, false, "realase", "-buildfile", projectPath + "/build.xml");
			if(code2 != 0){
				throw new IOException("Error in ant release");
			}
		}
		

	}
}
