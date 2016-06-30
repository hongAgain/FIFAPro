using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Xml;
using NativeBuilder.EclipseEditor;
using UnityEditor;
using System.IO;

namespace NativeBuilder
{

	delegate T Delegate<T>();
	
	public class AssertException : Exception
	{
		public AssertException(string msg) : base(msg) {}
	}

	public class UserCancelException : Exception
	{
		public UserCancelException(string msg) : base(msg) {}
	}

	internal static class BuildTask_AndroidUtility {


		public static List<AssertException> CheckAsserts(Mod mod, Conf_Local local, bool useBackUp, Conf_Gloable global)
		{
			// check asset
			var exceptionList = BuildTask_AndroidUtility.CheckAsserts(local["android.sdk"], local["ant.sdk"], mod);
			
			// check Java Home
			if(OSUtil.Platform == Platform.Windows){
				string java_home = Environment.GetEnvironmentVariable("JAVA_HOME");
				Debug.Log("java_home:" + java_home);
				if(java_home == null){
					exceptionList.Add(new AssertException("'JAVA_HOME' is not found."));
				}
			}
			
			// check Platform
			if(EditorUserBuildSettings.activeBuildTarget != BuildTarget.Android)
			{
				exceptionList.Add(new AssertException("Current platform is not Android. (now is " + EditorUserBuildSettings.activeBuildTarget + ")"));
			}

			// check backup
			if(useBackUp)
			{
				var di = new DirectoryInfo(global["temp.dir"] + "/android/eclipse_project_backup");
				if(!di.Exists)
				{
					exceptionList.Add(new AssertException("Eclipse project backup not exsits. retry with setting 'Build Option' to 'Rebuild' in NativeBuilder Android Build pannel. (Once Eclipse project be rebuilt, NativeBuilder will backup it)\npath:" + di.FullName));
				}
			}

			return exceptionList;
		}

		private static List<AssertException> CheckAsserts(string android_sdk_path, string ant_path, Mod mod)
		{
			XmlNode asserts = mod.Xml.DocumentElement.SelectSingleNode("assert");
			var exceptionList = new List<AssertException>();
			
			if(asserts != null){
				foreach(XmlNode node in asserts.ChildNodes)
				{
					try{
						switch(node.Name)
						{
						case "android-sdk":
							CheckAndroidSdk(node, android_sdk_path);
							break;
						case "jdk":
							CheckJdk(node);
							break;
						case "ant":
							CheckAnt(node, ant_path);
							break; 
						default:
							Debug.LogWarning("Unknown node: '" + node.Name +"' in assert partment in file: " + mod.path);
							break;
						}
					}
					catch(AssertException e)
					{
						exceptionList.Add(e);
					}
				}
			}
			
			
			foreach(Mod lib in mod.Reference)
			{
				exceptionList.AddRange(BuildTask_AndroidUtility.CheckAsserts(android_sdk_path, ant_path, lib));
			}
			
			return exceptionList;
		}


		
		private static void CheckAndroidSdk(XmlNode node, string android_sdk_path)
		{
			if(node.Attributes["min-target"] != null) CheckAndroidSDK_minTarget(node, android_sdk_path);
			if(node.Attributes["use-jdk"] != null ) CheckAndroidSDK_useJdk(node, android_sdk_path);
		}
		
		private static void CheckAndroidSDK_minTarget(XmlNode node, string android_sdk_path)
		{
			// check min-target
			Debug.Log("Check Android SDK...");
			string min_target = node.Attributes["min-target"].Value;
			int myTargetLevel = Android.GetMaxTargetLevel(android_sdk_path);
			if(myTargetLevel >= int.Parse(min_target)){
				return;
			}
			throw new AssertException("Android SDK need " + min_target + " or higher, now is " + myTargetLevel);
		}
		
		private static void CheckAndroidSDK_useJdk(XmlNode node, string android_sdk_path)
		{
			// check use-jdk
			Version target_version = new Version(node.Attributes["use-jdk"].Value);
			
			XmlDocument doc = new XmlDocument();
			string filename = android_sdk_path + "/tools/ant/build.xml";
			doc.Load(filename);
			XmlNodeList properties = doc.DocumentElement.SelectNodes("property");
			Fun getPropertyValue = (name) =>
			{
				foreach(XmlNode p in properties)
				{
					if(p.Attributes["name"].Value != name) continue;
					return p.Attributes["value"].Value;
				}
				return null;
			};
			Version my_java_target = new Version(getPropertyValue("java.target"));
			Version my_java_source = new Version(getPropertyValue("java.source"));
			
			string msg = "";
			if(target_version != my_java_target)
			{
				msg += "'java.target' must be set to '" + target_version  + "', now is '" + my_java_target + "'";
			}
			if(target_version != my_java_source)
			{
				if(msg != "") msg += "; ";
				msg += "'java.source' must be set to '" + target_version + "', now is '" + my_java_source + "'";
			}
			if(msg != "")
			{
				msg += "(in android-sdk file:'" + filename + "').";
				throw new AssertException(msg);
			}
		}
		
		delegate String Fun(String name);
		
		private static void CheckJdk(XmlNode node)
		{
			Debug.Log("Check JDK...");
			string min_version = node.Attributes["min-version"].Value;
			
			string output = Exec.RunGetOutput("java", "-version", true);
			string firstLine = output.Substring(0, output.IndexOf("\n"));
			int s = firstLine.IndexOf("\"");
			int e = firstLine.LastIndexOf("\"");
			string version_name = firstLine.Substring(s + 1, e - s -1);
			if(version_name.Contains("_")){
				version_name = version_name.Substring(0, version_name.IndexOf("_"));
			}
			
			Version targetVersion = new Version(min_version);
			Version myVersion = new Version(version_name);
			
			if(myVersion < targetVersion)
			{
				throw new AssertException("Java version need '" + min_version + "' or higher, now is: " + version_name + "(miss JAVA_HOME?. use 'java -version' to check version).");
			}
		}
		
		private static void CheckAnt(XmlNode node, string ant_path)
		{
			Debug.Log("Check Ant...");
			string min_version = node.Attributes["min-version"].Value;
			
			var ant_exec = Ant.getAntFormAntSDK(ant_path);
			string output = Exec.RunGetOutput(ant_exec.FullName, "-version", true);
			string[] words = output.Split(' ');
			
			Delegate<string> find_version_word = () => 
			{
				foreach(var w in words)
				{
					if(w.Contains(".")){
						return w;
					}
				}
				return null;
			};
			string version_word = find_version_word();
			
			Version targetVersion = new Version(min_version);
			Version myVersion = new Version(version_word);
			
			if(myVersion < targetVersion){
				throw new AssertException("Ant version need '" + min_version + "' or heigher, now is " + version_word + "(use 'ant -version' to check).");
			}
			
		}
	}


}

