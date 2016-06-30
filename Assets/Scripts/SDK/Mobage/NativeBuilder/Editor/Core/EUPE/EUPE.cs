using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System;
using UnityEditor;
using System.Diagnostics;
using NativeBuilder;

/// <summary>
/// modify eclipse project
/// </summary>
namespace NativeBuilder.EclipseEditor {

	public class EUPE {

		//modify eclipse project
		public static void ModEclipseProject(string eclipseProjectPath, string modPath){
			ELProject project = new ELProject (eclipseProjectPath);
			Mod mod = new Mod (modPath);
			ModEclipseProject(project, mod);
		}

		public static void ModEclipseProject(ELProject project, Mod mod){
			project.Apply (mod);
			UnityEngine.Debug.Log ("Success. ");
		}

		/*
		//copy eclipse project and apply modifacation 
		public static void Export(string targetEclipseProjectPath, string eupeModPath)
		{
			Mod mod = new Mod(eupeModPath);

			Export(targetEclipseProjectPath, mod);
		}
		*/

	}

}

