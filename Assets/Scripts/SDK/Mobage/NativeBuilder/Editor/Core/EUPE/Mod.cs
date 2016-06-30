using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Linq;
using System.Xml.Linq;
using System;


namespace NativeBuilder.EclipseEditor 
{

	public class Mod 
	{
		public XmlDocument Xml {get; private set;}

		public string path {get; private set;}

		// ===================================================================
		// ● 寄送字段
		// 这些字段为ELProject所使用，在其他地方看来，这些字段毫无意义
		// ===================================================================
		public Mod parent = null;
		public Dictionary<string, string> vars = new Dictionary<string, string> ();


		public Mod[] Reference {
			get
			{
				XmlNodeList list = this.Xml.DocumentElement.SelectNodes("reference");
				//var libs = from node in list select new Mod((node as XmlNode).Attributes["target"].Value);
				//return libs.ToArray();
				List<Mod> ret = new List<Mod>();
				foreach(XmlNode node in list){
					string refPath = node.Attributes["target"].Value;
					refPath = refPath.Replace ("${conf}", this.path);
					ret.Add(new Mod(refPath));
				}
				return ret.ToArray();
			}
		}


	 	public Mod (string path)
		{
			this.path = path;
			string xmlPath = path + "/" + "conf.xml";
			Xml = new XmlDocument ();
			Xml.Load (xmlPath);

		}

		public bool Exists
		{
			get{
				return new DirectoryInfo(this.path).Exists;
			}
		}

		// zhiyuan.peng:
		// add a new reference
		public void AddReference(string target)
		{
			XmlElement e = Xml.CreateElement("reference");
			e.SetAttribute("target", target);
			Xml.DocumentElement.AppendChild(e);
		}

	}


}


























