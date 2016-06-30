using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

namespace NativeBuilder
{
	public class Properties : Dictionary<string, string>
	{
		public string head_comments = "";
		public string path{get; private set;}

		public static Properties Open(string path)
		{
			var p = new Properties();
			p.Load(path);
			return p;
		}

		public virtual void Load(string path)
		{
			this.Clear();
			head_comments = null;
			this.path = path;
			FileInfo fi = new FileInfo(path);
			if(!fi.Exists)
			{
				throw new IOException("'" + path + "' not exsists!");
			}
			StreamReader reader = fi.OpenText();
			int lineIndex = 0;
			bool head = true;
			while(!reader.EndOfStream)
			{
				var line = reader.ReadLine().Trim();
				lineIndex ++;
				if(line.StartsWith("#") || line == "") 
				{
					if(head)
					{
						this.head_comments += line + "\n";
					}
					continue;
				}
				if(line.Contains("=")){
					head = false;
					var temp = line.Split("=".ToCharArray());
					if(temp.Length == 2){
						var k = temp[0].Trim();
						var v = temp[1].Trim();
						this.Add(k, v);
						//Debug.Log(k + " = " + v);
						continue;
					}else{
						Debug.LogWarning(path + " line " + lineIndex + " Error: "+ line);
					}
				}
				Debug.LogWarning(path + " line " + lineIndex + " Error: "+ line);
			}
			//Debug.Log(conf.Count + " properties loaded.");
			reader.Close();
		}

		public virtual void Reload()
		{
			if(this.path == null)
			{
				throw new InvalidOperationException("Can't call Reload() with path is null.");
			}
			this.Load(this.path);
		}

		public static Properties New(string path)
		{
			Properties p = new Properties();
			p.path = path;
			return p;
		}

		public Properties()
		{

		}

		public Properties(string path)
		{
			if(File.Exists(path))
			{
				this.Load(path);
			}
			else
			{
				this.path = path;
			}
		}

		 
		public void Save(){
			if(this.path == null)
			{
				throw new InvalidOperationException("Can't call Save() with path is null.");
			}
			this.SaveAs(this.path);
		}

		public void SaveAs(string path)
		{
			FileInfo fi = new FileInfo(path);
			var writer = fi.CreateText();
			writer.Write(this.head_comments);
			foreach(var kv in this)
			{
				writer.WriteLine(kv.Key + "=" + kv.Value);
			}
			writer.Close();
		}

		// do not show hide warning
		#pragma warning disable 0114
		public virtual string this[string key]
		{
			get
			{
				try
				{
					return base[key];
				}
				catch(KeyNotFoundException){
					throw new KeyNotFoundException("property '" + key + "' not exists in '" + path + "'");
				}
				
			}
			set
			{
				base[key] = value;
			}
		}

		public string TryGet(string key, string default_value)
		{
			if(!this.ContainsKey(key)) return default_value;
			return this[key];
		}
	}


}

