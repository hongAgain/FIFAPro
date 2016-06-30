using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System;
using UnityEditor;
using NativeBuilder;

namespace NativeBuilder.XCodeEditor
{
	public class XUPE {

		// Modify one exist xCodeProject
		public static void ModXCodeProject(string xCodeProjectPath, string xupeModPath){

			XCProject xCodeProject = new XCProject(xCodeProjectPath);
			XUPEMod mod = new XUPEMod(xupeModPath);
			xCodeProject.ApplyXupemod(mod);
			
			xCodeProject.Save();
		}

		// Copy xCodeProject, and Modify copy
		public static void ExportFromXCodeProject(string sourceXCodeProjectPath, string xupeModPath, string targetXCodeProjectPath){
			DirectoryInfo sourceDi = new DirectoryInfo(sourceXCodeProjectPath);
			if(!sourceDi.Exists){
				throw new IOException( "'" + sourceXCodeProjectPath + "' not exists!");
			}
			DirectoryInfo modDi = new DirectoryInfo(xupeModPath);
			if(!modDi.Exists){
				throw new IOException( "'" + xupeModPath + "' not exists!");
			}
			DirectoryInfo targetDi = new DirectoryInfo(targetXCodeProjectPath);
			if(targetDi.Exists){
				Debug.Log("target exits, delete");
				targetDi.Delete(true);
			}
			Debug.Log("copying '" + sourceXCodeProjectPath + "' to '" + targetXCodeProjectPath + "'...");
			PShellUtil.CopyAll(sourceDi, targetDi);
			ModXCodeProject(targetXCodeProjectPath, xupeModPath);
		}

		// Tell Unity Engine to Build one xCodeProject, and modify it.
		/*
		public static void Export(string targetXCodeProjectPath, string xupeModPath){
			if(EditorUserBuildSettings.activeBuildTarget != BuildTarget.iPhone)
			{
				throw new Exception("Current platform must be iOS! (now is " + EditorUserBuildSettings.activeBuildTarget + ")");
			}
			//check mod exist
			{
				var mod = new DirectoryInfo(xupeModPath);
				if(!mod.Exists){
					throw new Exception("xupe package not exists! (" + xupeModPath + ")");
				}
			}
			// check xCode Target Parent dir exsits
			{
				var di = new DirectoryInfo(targetXCodeProjectPath).Parent;
				if(!di.Exists){
					di.Create();
				}
			}

			Debug.Log("Unity building xCode..."); 
			NativeBuilderUtility.Build(targetXCodeProjectPath, UnityEditor.BuildTarget.iPhone, UnityEditor.BuildOptions.None);
			ModXCodeProject(targetXCodeProjectPath, xupeModPath);
		}
		*/
	}


}