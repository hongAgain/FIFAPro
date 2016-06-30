using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System;
using NativeBuilder;
using NativeBuilder.XCodeEditor;

namespace NativeBuilder
{
	public class BuildTask_iOS : BuildTask 
	{
		private enum TaskList
		{
			BuildXCodeProject,
			ApplyNativeBuilder
		}

		IOSBuildLevel BuildLevel;
		string BackupName;
		string modName;

		public BuildTask_iOS(IOSBuildLevel level, string backupName, string modName)
		{
			this.BuildLevel = level;
			this.BackupName = backupName;
			this.modName = modName;
		}

		private bool UseBackup
		{
			get
			{
				return !string.IsNullOrEmpty(this.BackupName);
			}
		}
		
		private bool ShouldDoTask(TaskList task)
		{
			List<TaskList> whatShouldDo = new List<TaskList>();
			switch(this.BuildLevel)
			{
			case IOSBuildLevel.UtilApplyNativeBuilder: 
				whatShouldDo.Add(TaskList.BuildXCodeProject);
				whatShouldDo.Add(TaskList.ApplyNativeBuilder);
				break;
			case IOSBuildLevel.JustXCodeProject:
				whatShouldDo.Add(TaskList.BuildXCodeProject);
				break;
			}
			if(whatShouldDo.Contains(task)) return true;
			return false;
		}

		Conf_Gloable global = null;
		string modPath = null;
		string xCodePath = null;

		public override void OnPreBuild ()
		{
			this.global = Configuration.Gloable;
			this.global.Repaire();
			// get my xupe file path
			//this.modPath = gloabal["ios.conf"];
			this.modPath = Path.Combine(this.global.IOSSrcDir, this.modName);

			// set a target xCode path 
			this.xCodePath = global["ios.project"];
			// export!
			//NativeBuilderCore.ExportIOS (xCodePath, modPath);
			
			if(EditorUserBuildSettings.activeBuildTarget != BuildTarget.iPhone)
			{
				throw new Exception("Current platform must be iOS! (now is " + EditorUserBuildSettings.activeBuildTarget + ")");
			}
			//check mod exist
			{
				if(!(new DirectoryInfo(modPath).Exists)){
					throw new Exception("xupe package not exists! (" + modPath + ")");
				}
			}
		}
		
		public override void OnBuild ()
		{

			if(this.ShouldDoTask(TaskList.BuildXCodeProject))
			{
				Debug.Log("Unity building xCode..."); 

				// check parent dir exsits
				var parent = new DirectoryInfo(xCodePath).Parent;
				if(!parent.Exists){
					parent.Create();
				}

				if(!this.UseBackup)
				{
					// build
					NativeBuilderUtility.Build(this.xCodePath, UnityEditor.BuildTarget.iPhone, UnityEditor.BuildOptions.None);

					// write build time
					XCProject project = new XCProject(this.xCodePath);
					project.BuildTime = DateTime.Now;

					//back up pure xCode Project
					PShellUtil.CopyTo(this.xCodePath, this.global.XCode_Project_Backup_Home + "/autosave", PShellUtil.FileExsitsOption.Override, PShellUtil.DirectoryExsitsOption.Override);
				}
				else
				{
					// use last version
					PShellUtil.CopyTo(Path.Combine(this.global.XCode_Project_Backup_Home, this.BackupName), this.xCodePath, PShellUtil.FileExsitsOption.Override, PShellUtil.DirectoryExsitsOption.Override);
				}

			}

			if(this.ShouldDoTask(TaskList.ApplyNativeBuilder))
			{
				Debug.Log("Apply NativeBuilder..."); 
				XUPE.ModXCodeProject(this.xCodePath, this.modPath);
			}

		}
		
		public override void OnPostBuild ()
		{
			// print log
			UnityEngine.Debug.Log("Build success, xCode Project At [" + xCodePath + "].");
		}
	}

	public enum IOSBuildLevel
	{
		JustXCodeProject,
		UtilApplyNativeBuilder
	}

}

