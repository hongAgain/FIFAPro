using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System;
using UnityEditor;
using System.Xml;
using NativeBuilder.XCodeEditor;
using System.Text.RegularExpressions;



namespace NativeBuilder.EclipseEditor {
	
	public class ELProject {

		private string rootPath;
		private XmlDocument xml;
		//private Dictionary<string, string> vars = new Dictionary<string, string> ();

		public ELProject(string eclipseProjectPath)
		{
			this.xml = new XmlDocument ();
			this.rootPath = eclipseProjectPath;
			this.xml.Load (eclipseProjectPath + "/AndroidManifest.xml");

		}

		public void Apply(Mod mod)
		{
			// Zhiyuan.Peng: 变量系统已重写，现在变量数据正确的存储在Mod对象中，子Mod可以从父Mod中继承变量
			// ${project} 与 ${conf} 也作为变量处理
			ApplyVar (mod);
			// AndroidManifest Part
			{
				XmlNode androidManifest = mod.Xml.DocumentElement.SelectSingleNode ("AndroidManifest");
				if (androidManifest != null)	ApplyAndroidManifest (mod, androidManifest);	
			}
			// command Part
			{
				XmlNode commandNode = mod.Xml.DocumentElement.SelectSingleNode ("command");
				if (commandNode != null) ApplyCommands (mod, commandNode);
			}
			// Reference Part
			{
				if (mod.Reference.Length > 0) ApplyRef (mod);
			}
			// post-command Part
			{
				XmlNode commandNode = mod.Xml.DocumentElement.SelectSingleNode ("post-command");
				if (commandNode != null) ApplyCommands (mod, commandNode);
			}
		}

		/// <summary>
		/// 已文本的方式替换变量
		/// </summary>
		/// <param name="node">Node.</param>
		private void ReplaceVar(Mod mod)
		{
			var document = mod.Xml.DocumentElement;
			var s = document.InnerXml;

			foreach (var kv in mod.vars) {
				s = s.Replace("${" + kv.Key + "}", kv.Value);
			}
			document.InnerXml = s;
		}

		/// <summary>
		/// 处理AndrodManifest部分
		/// </summary>
		public void ApplyAndroidManifest(Mod mod, XmlNode androidManifestNode)
		{
			//register namespace ,it is very important!
			XmlNamespaceManager xnm = new XmlNamespaceManager (mod.Xml.NameTable);
			xnm.AddNamespace ("a", "http://schemas.android.com/apk/res/android");
	
			XmlNode AndroidManifest = androidManifestNode;//mod.Xml.DocumentElement.SelectSingleNode ("AndroidManifest");


			foreach (XmlNode node in AndroidManifest.ChildNodes) 
			{
				switch(node.Name)
				{
				case "add-element":
					this.AddElement(node);
					break;
				case "modify-element":
					this.ModifyElement(node);
					break;
				}
			}
			this.xml.Save (this.rootPath + "/AndroidManifest.xml");

			BeBeautiful (this.rootPath + "/AndroidManifest.xml");
		}

		/// <summary>
		/// 处理AndroidManifest中的add-element操作
		/// </summary>
		/// <param name="node">add-element节点</param>
		private void AddElement(XmlNode node)
		{
			Debug.Log("add-element...");
			XmlElement targetNode = WhereIsNode(node);
			
			if(targetNode != null){
				XmlNodeList nodeList = node.SelectNodes ("element")[0].ChildNodes;
				foreach (XmlNode elementNode in nodeList) {
					targetNode.AppendChild (xml.ImportNode (elementNode, true));
				}
			}

		}

		/// <summary>
		/// 处理AndroidManifest中的modify-element操作
		/// </summary>
		/// <param name="node">modify-element节点</param>
		private void ModifyElement(XmlNode node)
		{
			Debug.Log("modify-element...");
			XmlElement targetNode = WhereIsNode(node);
			
			if(targetNode != null){
				XmlNodeList propertyList = node.SelectNodes("property");
				foreach (XmlNode propertyNode in propertyList)
				{
					string attributeName = propertyNode.Attributes ["name"].Value;
					string attributeValue = propertyNode.Attributes ["value"].Value;
					
					if(targetNode.Attributes[attributeName] != null)
						targetNode.RemoveAttribute(attributeName);
					
					targetNode.SetAttribute ("a:" + attributeName, attributeValue);
					Debug.Log("Add: " + attributeName + "=" + attributeValue);	
				}
			}
		}

		public void ApplyCommands(Mod mod, XmlNode commandNode)
		{
			//XmlNode commandNode = mod.Xml.DocumentElement.SelectSingleNode ("command");
			//ReplaceVar (commandNode);
			XmlNodeList commandList = commandNode.ChildNodes;
			foreach (XmlNode command in commandList) {
				if (command.Name == "delete") {
					string deleteDir = command.Attributes["target"].Value;
					//deleteDir = filtPath(deleteDir, mod);
					if(Directory.Exists(deleteDir))
					{
						DirectoryInfo dir = new DirectoryInfo (deleteDir);
						dir.Delete (true);
					}
					else if(File.Exists(deleteDir))
					{
						FileInfo file = new FileInfo(deleteDir);
						file.Delete(); 
					}
					else
					{
						throw new Exception("'" + deleteDir + "' not exits!");
					}
				}
				if (command.Name == "copy") {
					string from = command.Attributes ["from"].Value;
					string to = command.Attributes ["to"].Value;
					//from = filtPath(from, mod);
					//to = filtPath(to, mod);
					Debug.Log("from: " + from);
					Debug.Log("to: " + to);

					if(Directory.Exists(from)){
						PShellUtil.CopyTo(new DirectoryInfo(from), new DirectoryInfo(to));
					} 
					else if(File.Exists(from)){
						FileInfo fi = new FileInfo(from);
						//FileInfo t = new FileInfo(to + "/" + fi.Name);
						//t.Directory.Create();
						fi.CopyTo(to, true);

					}else{
						throw new Exception("copy error: '" + from + "' not exsists!");
					}

					//--------------translate variables in these files--------------------
					if(command.Attributes["translate-variables"] != null)
					{
						string reg = command.Attributes["translate-variables"].Value;
						List<string> targetFiles = FindFile(to, reg);
						TranslateVariables(mod, targetFiles);
					}

				}
				if (command.Name == "copyInto") {
					string from = command.Attributes ["from"].Value;
					string into = command.Attributes ["into"].Value;
					//from = filtPath(from, mod);
					//into = filtPath(into, mod);
					Debug.Log("from: " + from);
					Debug.Log("into: " + into);
					
					if(Directory.Exists(from)){
						PShellUtil.CopyInto(new DirectoryInfo(from), new DirectoryInfo(into));
					} 
					else if(File.Exists(from)){
						FileInfo fi = new FileInfo(from);
						FileInfo to = new FileInfo(into + "/" + fi.Name);
						to.Directory.Create();
						fi.CopyTo(to.FullName, true);
						
					}else{
						throw new Exception("copy error: '" + from + "' not exsists!");
					}

					//--------------translate variables in these files--------------------
					if(command.Attributes["translate-variables"] != null)
					{
						FileInfo fi = new FileInfo(from);
						FileInfo to = new FileInfo(into + "/" + fi.Name);

						string reg = command.Attributes["translate-variables"].Value;
						List<string> targetFiles = FindFile(to.FullName, reg);
						TranslateVariables(mod, targetFiles);
					}
					
				}
				if (command.Name == "add-lib-project") {
					string modFilePath = Path.Combine(this.rootPath, "project.properties");
					string libProjectPath = command.Attributes["target"].Value;

					FileInfo file = new FileInfo (modFilePath);
					StreamWriter sw = file.AppendText();
					
					List<string> lines = new List<string>();
					lines.Add("android.library.reference.1=./" + libProjectPath);
				
					foreach (var line in lines) {
						sw.WriteLine (line);
					}
					sw.Close ();
				}
				if (command.Name == "modify-code") {
					string filePath = command.Attributes ["file"].Value;
					//filePath = filtPath(filePath, mod);
					if(!File.Exists(filePath))
						throw new Exception("'" + filePath + "' not exits!");
					string source = command.Attributes ["source"].Value;
					string target = command.Attributes ["target"].Value;
				
					XClass xc = new XClass (filePath);
					xc.Replace (source, target);
				}
				if (command.Name == "rename") {

					string from = command.Attributes ["from"].Value;
					string to = command.Attributes ["to"].Value;
					//from = filtPath(from, mod);
					//to = filtPath(to, mod);

					if(Directory.Exists(from))
					{
						Directory.Move(from, to);
					}
					if(File.Exists(from))
					{
						File.Move(from, to);
					}
				}
			}
		}


		/// <summary>
		/// Translates the variables.
		/// </summary>
		/// <param name="translateDomain">Translate domain.</param>
		private void TranslateVariables(Mod mod, List<string> files)
		{
			//deal variables
			foreach(string targetFile in files)
			{
				XClass xc = new XClass(targetFile);
				foreach(string key in mod.vars.Keys)
				{
					xc.Replace("${" + key + "}", mod.vars[key]);
				}
			}
		}

		private List<string> FindFile(string path, string regexPatten = null)
		{
			List<string> targetFiles = new List<string>();
			if(regexPatten.Contains("*"))
				regexPatten = regexPatten.Remove(0);
			try{

				foreach(string item in Directory.GetFileSystemEntries(path))
				{
					Debug.Log("files name: " + item);
					bool isFile = (File.GetAttributes(item) & FileAttributes.Directory) != FileAttributes.Directory;
					if(isFile && (regexPatten == null || Regex.IsMatch(Path.GetFileName(item), regexPatten)))
					{
						targetFiles.Add(item);
					}
					else if(!isFile)
					{
						targetFiles.AddRange(FindFile(item, regexPatten));
					}
				}
			}catch{
				throw new Exception("get files error!");
			}

			return targetFiles;
		}



		public void ApplyRef(Mod mod)
		{
			foreach(Mod lib in mod.Reference)
			{
				lib.parent = mod;
				this.Apply(lib);
			}
		}


		private void ApplyVar(Mod mod)
		{
			this.LoadVar(mod);
			this.ReplaceVar(mod);
		}

		private void LoadVar(Mod mod)
		{
			// 从父Mod继承变量
			if(mod.parent != null){
				foreach(var kv in mod.parent.vars)
				{
					mod.vars.Add(kv.Key, kv.Value);
				}
			}
			// 加载内置变量
			mod.vars["project"] = this.rootPath;
			mod.vars["conf"] = mod.path;
			// 加载自定义变量
			if (mod.Xml.DocumentElement.SelectSingleNode ("variables") != null)
			{
				var variableElement = mod.Xml.DocumentElement.SelectSingleNode ("variables");
				foreach (XmlNode var in variableElement.ChildNodes) 
				{
					if(var.NodeType !=  XmlNodeType.Element) continue;
					string source = var.Attributes["name"].Value ;
					mod.vars.Add(source, var.Attributes["value"].Value);
				}
			}
		}


		/*
		 * filt path
		 */
		/*
		private string filtPath (string path, Mod mod)
		{
			string tmp = null ;
			string modPath = mod.path;
			string projectpath = rootPath;
			if (path.Contains ("${conf}")) 
			{
				tmp = path.Replace ("${conf}", modPath);
			}
			if (path.Contains ("${project}")) 
			{
				tmp = path.Replace ("${project}", projectpath);
			}
			Debug.Log ("path= " + path);
			return tmp;
		}
		*/

		/*
		 * 通过XmlDocument处理后的Manifest会给每一个元素显示的使用命名空间，
		 * 段：这样极其不美观
		 * 所以用一个方法去掉它们
		 * （用字符串的方式）
		 */
		private void BeBeautiful(string xml)
		{
			XmlDocument tmpXml = new XmlDocument ();
			tmpXml.Load (xml);
			string str1 = tmpXml.DocumentElement.Attributes ["android:versionName"].Value;
			string str2 = tmpXml.DocumentElement.Attributes ["android:versionCode"].Value;
			string str3 = tmpXml.DocumentElement.Attributes ["android:installLocation"].Value;
			tmpXml.DocumentElement.RemoveAttribute("android:versionName");
			tmpXml.DocumentElement.RemoveAttribute("android:versionCode");
			tmpXml.DocumentElement.RemoveAttribute("android:installLocation");
			tmpXml.DocumentElement.SetAttribute ("xmlns:android", "temp233333");
			tmpXml.Save (xml);
	
			XClass xc = new XClass (xml);
			xc.Replace ("xmlns:android=\"http://schemas.android.com/apk/res/android\"", "");
			xc.Replace ("temp233333", "http://schemas.android.com/apk/res/android");

			XmlDocument newxml = new XmlDocument ();
			newxml.Load (xml);

			newxml.DocumentElement.SetAttribute ("versionName", str1);
			newxml.DocumentElement.SetAttribute ("versionCode", str2);
			newxml.DocumentElement.SetAttribute ("installLocation", str3);
			newxml.Save (xml);

			xc.Replace ("versionName", "android:versionName");
			xc.Replace ("versionCode", "android:versionCode");
			xc.Replace ("installLocation", "android:installLocation");
		}


		private static List<XmlNode> ToGenerics(XmlNodeList list, List<XmlNode> container = null)
		{
			if(container == null) container = new List<XmlNode>();
			foreach(XmlNode item in list)
			{
				container.Add(item);
			}
			return container;
		}

		/*
		 * search target node
		 */
		private XmlElement WhereIsNode(XmlNode xmlNode)
		{
			XmlNode where = xmlNode.SelectSingleNode ("where");

			// filt by path and target
			var alterNodes = ToGenerics(this.PathFilter(xmlNode));

			if(where != null)
			{
				// get contain elements
				List<XmlNode> containItems = new List<XmlNode>();
				XmlNode contain = where.SelectSingleNode ("contain");
				if(contain != null)
				{
					ToGenerics(contain.ChildNodes, containItems);
				}
				
				// filt by contain
				foreach(XmlNode alterNode in alterNodes.ToArray())
				{
					if(!IsIncludeAll(alterNode, containItems))
					{
						alterNodes.Remove(alterNode);
					}
				}
			}

			// surviver list
			if(alterNodes.Count > 1)
			{
				throw new Exception ("'contain' condition exits, Multi target nodes be found by 'contain' condition!");
			}
			else if(alterNodes.Count == 0)
			{
				throw new Exception ("Where: There is no element matches! \nwhere:" + where.InnerXml + "\n\nxml:" + this.xml.InnerXml);
			}
			return (XmlElement)alterNodes[0];
		}

		private bool IsIncludeAll(XmlNode alterNode, List<XmlNode> containItems)
		{
			foreach (XmlNode containItem in containItems) 
			{
				if(!IsInclude (alterNode, containItem)) return false;
			}
			return true;
		}
		
		/// <summary>
		/// Node里是否包含目标节点（内容上，非对象），目标节点可以是Node的任何子孙节点
		/// </summary>
		private bool IsInclude(XmlNode node, XmlNode target)
		{
			XmlNodeList alterGrandchildren = node.SelectNodes(@".//" + target.Name);
			foreach (XmlNode child in alterGrandchildren){
				bool isAllAttrSame = true;
				foreach(XmlAttribute attr in target.Attributes)
				{
					if(attr.Name.StartsWith("xmlns:")) continue;
					if(child.Attributes[attr.Name] == null)
					{
						isAllAttrSame = false;
						break;
					}
					if(child.Attributes[attr.Name].Value != target.Attributes[attr.Name].Value)
					{
						isAllAttrSame = false;
						break;
					}
				}
				if(isAllAttrSame) return true;
			}
			return false;
		}

		/// <summary>
		/// 传入一个Mode中AndroidManifest Section的子节点，
		/// 假设改节点一定有过滤条件，执行该过滤条件，返回所有符合条件的节点的列表
		/// (必要：target，可选：where)
		/// </summary>
		private XmlNodeList PathFilter(XmlNode manifestChildNode)
		{
			string target = manifestChildNode.Attributes["target"].Value;
			XmlNode where = manifestChildNode.SelectSingleNode ("where");
			// deal path node	
			string serchPath = null;
			if(where != null && where.SelectSingleNode("path") != null)
			{
				string path = where.SelectSingleNode ("path").Attributes ["value"].Value;
				serchPath = path + "/" + target;
			}
			else
			{
				serchPath = @"//" + manifestChildNode.Attributes ["target"].Value;
			}

			XmlNodeList alterNodes = xml.SelectNodes (serchPath);
			if(alterNodes.Count > 1)
			{
				Debug.Log("'path' condition exits, Multi target node be found by 'path' condition!");
			}
			else if(alterNodes.Count == 0)
			{
				Debug.Log("PathFilter(): not any element matches filter.");
				return null;
			}
			return alterNodes;
		}

	//ELProject class end
	}
}