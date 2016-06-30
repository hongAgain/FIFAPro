using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

namespace NativeBuilder
{
	public class EclipseProject 
	{
		Properties project;
		Properties ant;
		Properties native_builder;
		public string Path {get;private set;}

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

		public EclipseProject(string path)
		{
			this.Path = path;
			var fi = new FileInfo(path + "/project.properties");
			if(fi.Exists){
				this.project = Properties.Open(fi.FullName);
			}
			this.native_builder = new Properties(path + "/native_builder.properties");
		}

		public List<EclipseProject> GetSubproject()
		{
			var ret = new List<EclipseProject>();
			if(project == null) return ret;
			int index = 1;
			var s = "android.library.reference.";
			var key = s + index;
			while(project.ContainsKey(key))
			{
				var relativePath = project[key];
				var subPath = System.IO.Path.Combine(this.Path, relativePath);
				ret.Add(new EclipseProject(subPath));
				index ++;
				key = s + index;
			}
			return ret;
		}

		/// <summary>
		/// make project and all sub-project (recursive) can be ant 
		/// </summary>
		public void Antlize(string android_sdk)
		{
			Android.android_update(this.Path, android_sdk);
			Android.MakeSrcDir(this.Path);
			var sub_list = GetSubproject();
			foreach(var sub in sub_list)
			{
				sub.Antlize(android_sdk);
			}
			return;
		}

		public void SetKeystore(string keystore_Path, string storepass, string alias, string aliaspass)
		{
			if(this.ant == null){
				this.ant = Properties.New(this.Path + "/ant.properties");
			}
			this.ant["key.store"] = keystore_Path;
			this.ant["key.store.password"] = storepass;
			this.ant["key.alias"] = alias;
			this.ant["key.alias.password"] = aliaspass;
			this.ant.Save();
		}

		public bool KeystoreExists
		{
			get
			{
				return File.Exists(this.Path + "/ant.properties");
			}
		}

	}

}

