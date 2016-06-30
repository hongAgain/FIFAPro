using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace NativeBuilder.XCodeEditor 
{
	public class XCPlist
	{
		string plistPath;
		bool plistModified;

		// URLTypes constant --- plist
		const string BundleUrlTypes = "CFBundleURLTypes";
		const string BundleTypeRole = "CFBundleTypeRole";
		const string BundleUrlName = "CFBundleURLName";
		const string BundleUrlSchemes = "CFBundleURLSchemes";

		// URLTypes constant --- projmods
		const string PlistUrlType = "urltype";

		// Pzy: add Note:
		// "urltype" property
		const string PlistRole = "role";
		const string PlistEditor = "Editor";
		const string PlistName = "name";
		const string PlistSchemes = "schemes";

		public XCPlist(string plistPath)
		{
			this.plistPath = plistPath;
		}

		public void Process(Hashtable plist)
		{
			Dictionary<string, object> dict = (Dictionary<string, object>)PlistCS.Plist.readPlist(plistPath);
			foreach( DictionaryEntry entry in plist)
			{
				this.AddPlistItems((string)entry.Key, entry.Value, dict);
			}
			if (plistModified)
			{
				PlistCS.Plist.writeXml(dict, plistPath);
			}
		}

		// pzy:
		// add Note:
		// "key" is proptery in "plist" jsonObject in mod file
		// "value" is value for "key" in mod
		// "dic" is plist.info's image in target xCode project
		public void AddPlistItems(string key, object value, Dictionary<string, object> dict)
		{
			Debug.Log ("AddPlistItems: key=" + key);
			
			if (key.CompareTo(PlistUrlType) == 0)
			{
				processUrlTypes((ArrayList)value, dict);
			}
			else
			{
				// pzy:
				// let's mod can sepecify plist key-value

				// I delete these code:
				//Debug.Log("unknown plist key : " + key);

				// I add these code :
				dict[key] = value;
			}
		}

		private void processUrlTypes(ArrayList urltypes, Dictionary<string, object> dict)
		{
			List<object> bundleUrlTypes;
			if (dict.ContainsKey(BundleUrlTypes))
			{
				bundleUrlTypes = (List<object>)dict[BundleUrlTypes];
			}
			else
			{
				bundleUrlTypes = new List<object>();
			}
			
			foreach(Hashtable table in urltypes)
			{
				string role = (string)table[PlistRole];
				if (string.IsNullOrEmpty(role))
				{
					role = PlistEditor;
				}
				string name = (string)table[PlistName];
				ArrayList shcemes = (ArrayList)table[PlistSchemes];
				
				// new schemes
				List<object> urlTypeSchemes = new List<object>();
				foreach(string s in shcemes)
				{
					urlTypeSchemes.Add(s);
				}

				// pzy:
				// "name" is used as id to try to select a exists UrlItem to modify, when not fount add new.
				// but when "name" is null or EmptyString it means always add a new UrlItem. 
				// not try to found a exsit UrlItem which's id is EmptyString.

				// I delete these code:
				//Dictionary<string, object> urlTypeDict = this.findUrlTypeByName(bundleUrlTypes, name);

				// and add thes code:
				Dictionary<string, object> urlTypeDict = null;
				if(!string.IsNullOrEmpty(name)) urlTypeDict = this.findUrlTypeByName(bundleUrlTypes, name);
				//End

				if (urlTypeDict == null)
				{
					urlTypeDict = new Dictionary<string, object>();
					urlTypeDict[BundleTypeRole] = role;
					urlTypeDict[BundleUrlName] = name;
					urlTypeDict[BundleUrlSchemes] = urlTypeSchemes;
					bundleUrlTypes.Add(urlTypeDict);
				}
				else
				{
					urlTypeDict[BundleTypeRole] = role;
					urlTypeDict[BundleUrlSchemes] = urlTypeSchemes;
				}
				plistModified = true;
			}
			dict[BundleUrlTypes] = bundleUrlTypes;
		}
		
		private Dictionary<string, object> findUrlTypeByName(List<object> bundleUrlTypes, string name)
		{
			if ((bundleUrlTypes == null) || (bundleUrlTypes.Count == 0))
				return null;
			
			foreach(Dictionary<string, object> dict in bundleUrlTypes)
			{
				// zhiyuan.peng
				// fix BUG - when not contains 'BundleUrlName', it's crush
				// ADD this code:
				if(!dict.ContainsKey(BundleUrlName)) continue;
				// end 

				string _n = (string)dict[BundleUrlName];
				if (string.Compare(_n, name) == 0)
				{
					return dict;
				}
			}
			return null;
		}
	}
}
