using UnityEngine;
using System.Collections;
using UnityEditor;
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace NativeBuilder
{
	public class AndroidWindow : EditorWindow {
		

		bool build_android_project = true;
		//BuildOption build_android_project_option = BuildOption.Rebuild;
		bool apply_native_builder = true;
		bool build_apk = true;
		bool run_apk = true;
		EclipseProject backup_project = null;
		string[] mod_list = null;
		string selected_mod = null;

		string[] backup_list = null;
		string[] option_list = null;
		string selected_option = null;
		Dictionary<string, string> option_backup_mapping = new Dictionary<string, string>();

		public static void Show()
		{
			var window = EditorWindow.GetWindow<AndroidWindow>("Build Android", true) as AndroidWindow;
			window.Show(true);
		}


		private void ReadInfo()
		{
			// init
			Configuration.Gloable.Repaire();

			// read backup project
			/*
			string projectPath = Configuration.Gloable.Eclipse_Project_Backup_Home + "/Game";
			if(Directory.Exists(projectPath))
			{
				this.backup_project = new EclipseProject(projectPath);
			}
			else
			{
				backup_project = null;
			}
			*/

			// mod
			mod_list = new string[0];
			string modDir = Configuration.Gloable.AndroidSrcDir;
			var eupe_dirs = new DirectoryInfo(modDir).GetDirectories("*.eupe", SearchOption.TopDirectoryOnly);
			mod_list = (from eupe in eupe_dirs select eupe.Name).ToArray();
			if(string.IsNullOrEmpty(selected_mod))
			{
				if(mod_list.Length > 0) 
				{
					selected_mod = mod_list[0];
				}
			}

			// backup
			backup_list = new string[0];
			DirectoryInfo backupHome = new DirectoryInfo(Configuration.Gloable.Eclipse_Project_Backup_Home);
			if(backupHome.Exists)
			{
				backup_list = (from d in backupHome.GetDirectories() select d.Name).ToArray();
			}
			
			// option
			option_backup_mapping.Clear();
			option_backup_mapping.Add("Rebuild", null);
			selected_option = "Rebuild";
			foreach(string backup in backup_list)
			{
				if(backup == "autosave") option_backup_mapping.Add("Use Last Version (autosave)", backup);
				else option_backup_mapping.Add("Use '" + backup + "'", backup);
			}
			option_list = option_backup_mapping.Keys.ToArray();
			if(selected_option == null || !option_list.Contains(selected_option)) selected_option = option_list[0];
		}

		public void OnFocus()
		{
			ReadInfo();
		}

		public void OnGUI()
		{
			// build android project
			build_android_project = EditorGUILayout.Toggle("Build Android Project", build_android_project);

			// option
			if(build_android_project)
			{
				//build_option = (IOSBuildOption)EditorGUILayout.EnumPopup("Option", build_option);
				int selected = Array.IndexOf(option_list, selected_option);
				selected = EditorGUILayout.Popup("Use Backuped ?", selected, option_list);
				selected_option = option_list[selected];
			}

			// backup info
			// null means rebuild, if not null, print backup info
			if(this.BackupName != null)
			{
				// if backup_project is not current selected project, reload it.
				if(backup_project == null || backup_project.Path != Configuration.Gloable.Eclipse_Project_Backup_Home + "/" + this.BackupName + "/Game")
				{
					backup_project = new EclipseProject(Configuration.Gloable.Eclipse_Project_Backup_Home + "/" + this.BackupName + "/Game");
				}
				EditorGUILayout.LabelField("Backup Built At: " + this.backup_project.BuildTime);
			}


			if(!build_android_project)
			{
				apply_native_builder = false;
			}


			apply_native_builder = EditorGUILayout.Toggle("Apply NativeBuilder", apply_native_builder);
			{
				if(apply_native_builder)
				{
					if(mod_list.Length > 0)
					{
						int selectedIndex = Array.IndexOf(mod_list, selected_mod);
						selectedIndex = EditorGUILayout.Popup("Eupe package", selectedIndex, mod_list);
						this.selected_mod = mod_list[selectedIndex];
					}
					else
					{
						EditorGUILayout.LabelField("None of .eupe package found, Can't Build.");
					}
				}
			}
			if(!apply_native_builder)
			{
				build_apk = false;
			}else{
				build_android_project = true;
			}

			build_apk = EditorGUILayout.Toggle("Build Apk", build_apk);
			if(!build_apk)
			{
				run_apk = false;
			}else
			{
				build_android_project = true;
				apply_native_builder = true;
			}

			run_apk = EditorGUILayout.Toggle("Run Apk", run_apk);
			if(!run_apk)
			{
			}
			else
			{
				build_android_project = true;
				apply_native_builder = true;
				build_apk = true;
			}

			if(!string.IsNullOrEmpty(this.selected_mod) && !string.IsNullOrEmpty(this.selected_option))
			{
				if(GUILayout.Button("Build"))
				{
					var task = new BuildTask_Android(GetBuildLevel(), this.BackupName, this.selected_mod);
					task.Build();
				}
			}
		}
	
	
		private BuildLevel GetBuildLevel()
		{
			if(run_apk) return BuildLevel.UtilRunApk;
			if(build_apk) return BuildLevel.UtilBuildApk;
			if(apply_native_builder) return BuildLevel.UtilApplyNativeBuilder;
			if(build_android_project) return BuildLevel.JustAndroidProject;
			throw new Exception("Internal Error: GetBuildLevel(): Unknown branch");
		}

		public string BackupName
		{
			get{
				return option_backup_mapping[selected_option];
			}
		}
	}

}

