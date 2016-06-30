using UnityEngine;
using UnityEditor;
using System.IO;
using System.Diagnostics;
using System;

namespace NativeBuilder 
{
	
	public class ReportableTask
	{
		public string name;
		public Action<ProcessHandller> action;
		public ProcessHandller processHandller;
		
		public void Run(){
			UnityEngine.Debug.Log("[" + this.name + "]: Start...");
			this.action(processHandller);
			UnityEngine.Debug.Log("[" + this.name + "]: Complete.");
		}

	}
}


