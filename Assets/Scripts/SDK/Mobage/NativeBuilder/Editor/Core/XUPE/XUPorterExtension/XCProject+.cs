using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

namespace NativeBuilder.XCodeEditor
{
	public partial class XCProject : System.IDisposable
	{
		Properties native_builder;
		public string path;

		public DateTime BuildTime 
		{
			get
			{
				var str = this.native_builder.TryGet("BuildTime", null);
				if(str == null) return DateTime.MinValue;
				return DateTime.Parse(str);
			}
			set
			{
				this.native_builder["BuildTime"] = value.ToString();
				this.native_builder.Save();
			}
		}

		public void ApplyXupemod(XUPEMod mods){

			//ext.copyFolder
			Debug.Log( "Adding copyied folders..." );
			PBXGroup modGroup = this.GetGroup( mods.xcmod.group );
			foreach( string copiedFolderPath in mods.xcmod.copiedFolder ) {
				string sourceFloderPath = mods.path + "/" + copiedFolderPath;
				
				DirectoryInfo souceDirector = new DirectoryInfo(sourceFloderPath);
				DirectoryInfo targetDirector;
				//crate director in xcode project folder
				if(modGroup != null){
					targetDirector = this.CreateDirectorInRootDir(modGroup.path);
				}else{
					targetDirector = new DirectoryInfo(this.projectRootPath);
				}
				//copy "coyied folder" into target folder
				Debug.Log( "copy '" + souceDirector.FullName + "' to '" +  targetDirector.FullName + "'.");
				PShellUtil.CopyInto(souceDirector, targetDirector);

				//add target folder reference to project
				this.AddFolder(targetDirector.FullName + "/" + souceDirector.Name, modGroup, (string[])mods.xcmod.excludes.ToArray( typeof(string) ) );
			}

			//此方法必须放在"Adding copyied folders..."之后以保证添加动态库方法的正确运行!
			//native xcmod
			this.ApplyMod(mods.xcmod);

			//modify code
			Debug.Log( "Modify code..." );
			ArrayList codeList = mods.xcmod.extCode;
			foreach(var code in codeList){
				var item = (Hashtable)code;
				XClass xclass = new XClass(this.projectRootPath + item["file"]);
				if(item["type"].Equals("writeBelow")){
					xclass.WriteBelow((string)item["target"], (string)item["code"]);
				}
				else if(item["type"].Equals("writeHead")){
					xclass.WriteHead((string)item["code"]);
				}
				else if(item["type"].Equals("replace")){
					xclass.Replace((string)item["target"], (string)item["code"]);
				}
			}

			//----------------------------------------------------------------------------------------
			//set property value
			//
			//table init
			Dictionary<string, string> table = new Dictionary<string, string> ();
			table.Add ("codeSigningEntitlements", "CODE_SIGN_ENTITLEMENTS");
			table.Add ("enableBitcode", "ENABLE_BITCODE");
			table.Add ("deploymentTarget", "IPHONEOS_DEPLOYMENT_TARGET");
			table.Add ("productName", "PRODUCT_NAME");

			//set property
			Debug.Log ("set property...");
			Hashtable propertyList = mods.xcmod.property;
			if (propertyList != null) {
				foreach (string sKey in propertyList.Keys) {

					string key = "";
					if(!table.TryGetValue(sKey, out key)){
						key = sKey;
					}
					if(key == sKey){
						var logMsg = "override properties: " + key + " => " +  (string)propertyList[sKey];
						logMsg += " (Unknown Key)";
						Debug.Log(logMsg);	
					}

					this.overwriteBuildSetting(key ,(string)propertyList[sKey], "Debug");
					this.overwriteBuildSetting(key, (string)propertyList[sKey], "Release");
	
				}
			}
			//----------------------------------------------------------------------------------------
			Debug.Log( "Copy files..." );
			//copy file
			PShellUtil.CopyAll(new DirectoryInfo(Path.Combine(mods.path, "file")), new DirectoryInfo(this.projectRootPath));


		}

		public void ApplyXupemod(string path){
			XUPEMod mod = new XUPEMod(path);
			this.ApplyXupemod(mod);
		}

		public DirectoryInfo CreateDirectorInRootDir(string name){
			DirectoryInfo d = new DirectoryInfo(this.projectRootPath);
			d.CreateSubdirectory(name);
			return d;
		}
	}

	public static class PShellUtil{

		public enum FileExsitsOption{
			Override,
			NotCopy
		}

		public enum DirectoryExsitsOption{
			Override,
			Merge,
			NotCopy
		}

		// copy one dir to another dir
		public static void CopyInto(DirectoryInfo source, DirectoryInfo target, FileExsitsOption fileOption = PShellUtil.FileExsitsOption.Override, DirectoryExsitsOption directoryOption = PShellUtil.DirectoryExsitsOption.Merge, string[] exclude = null){
			var t = target.CreateSubdirectory(source.Name);
			CopyAll(source, t, fileOption, directoryOption, exclude);
		}

		public static void CopyTo(DirectoryInfo source, DirectoryInfo target, FileExsitsOption fileOption = PShellUtil.FileExsitsOption.Override, DirectoryExsitsOption directoryOption = PShellUtil.DirectoryExsitsOption.Merge, string[] exclude = null){

			CopyAll(source, target, fileOption, directoryOption, exclude);
		}

		public static void CopyInto(string source, string target, FileExsitsOption fileOption = PShellUtil.FileExsitsOption.Override, DirectoryExsitsOption directoryOption = PShellUtil.DirectoryExsitsOption.Merge, string[] exclude = null)
		{
			DirectoryInfo s = new DirectoryInfo(source);
			DirectoryInfo t = new DirectoryInfo(target);
			CopyInto(s, t, fileOption, directoryOption, exclude); 
		}

		public static void CopyTo(string source, string target, FileExsitsOption fileOption = PShellUtil.FileExsitsOption.Override, DirectoryExsitsOption directoryOption = PShellUtil.DirectoryExsitsOption.Merge, string[] exclude = null)
		{
			DirectoryInfo s = new DirectoryInfo(source);
			DirectoryInfo t = new DirectoryInfo(target);
			CopyTo(s, t, fileOption, directoryOption, exclude);
		}

		// copy whatever in a dir to anothr dir
		public static void CopyAll(DirectoryInfo source, DirectoryInfo target, FileExsitsOption fileOption = PShellUtil.FileExsitsOption.Override, DirectoryExsitsOption directoryOption = PShellUtil.DirectoryExsitsOption.Merge, string[] exclude = null)
		{
			if (source.FullName.ToLower() == target.FullName.ToLower())
			{
				return;
			}

			// Check if the source directory exists, if not, return
			if (Directory.Exists(source.FullName) == false)
			{
				return;
			}

			// Check if the target directory exists, if not, create it.
			if (Directory.Exists(target.FullName) == false)
			{
				Directory.CreateDirectory(target.FullName);
			}
			
			// Copy each file into it's new directory.
			foreach (FileInfo fi in source.GetFiles())
			{
				if(exclude != null && Array.IndexOf(exclude, fi.Name) != -1) continue;
				//Debug.Log(@"Copying " + target.FullName + "\\" + fi.Name);
				fi.CopyTo(Path.Combine(target.ToString(), fi.Name), fileOption == FileExsitsOption.Override ? true : false);
			}
			
			// Copy each subdirectory using recursion.
			foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
			{
				if(exclude != null && Array.IndexOf(exclude, diSourceSubDir.Name) != -1) continue;

				bool exsits = Directory.Exists(target.FullName + "/" + diSourceSubDir.Name);
				bool hasEndFix = diSourceSubDir.Name.Contains(".");
				if(!hasEndFix){
					// treat as folder
					if(directoryOption == DirectoryExsitsOption.Merge){
						DirectoryInfo nextTargetSubDir = target.CreateSubdirectory(diSourceSubDir.Name);
						CopyAll(diSourceSubDir, nextTargetSubDir, fileOption, directoryOption);
					}
					else if(directoryOption == DirectoryExsitsOption.Override){
						DirectoryInfo nextTargetSubDir = target.CreateSubdirectory(diSourceSubDir.Name);
						nextTargetSubDir.Delete(true);
						CopyAll(diSourceSubDir, nextTargetSubDir, fileOption, directoryOption);
						
					}else if(directoryOption == DirectoryExsitsOption.NotCopy){
						if(exsits) return;
						DirectoryInfo nextTargetSubDir = target.CreateSubdirectory(diSourceSubDir.Name);
						CopyAll(diSourceSubDir, nextTargetSubDir, fileOption, directoryOption);
					}
				}else{
					// treat as file
					if(fileOption == FileExsitsOption.Override){
						DirectoryInfo nextTargetSubDir = target.CreateSubdirectory(diSourceSubDir.Name);
						nextTargetSubDir.Delete(true);
						CopyAll(diSourceSubDir, nextTargetSubDir, fileOption, directoryOption);
					}else if(fileOption == FileExsitsOption.NotCopy){
						if(exsits) return;
						DirectoryInfo nextTargetSubDir = target.CreateSubdirectory(diSourceSubDir.Name);
						CopyAll(diSourceSubDir, nextTargetSubDir, fileOption, directoryOption);

					}
				}

			}
		}
	}

}