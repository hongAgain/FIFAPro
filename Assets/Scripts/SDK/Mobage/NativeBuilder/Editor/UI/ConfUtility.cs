using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

namespace NativeBuilder
{
	public static class ConfUtility  {
		
		public static void TranslateVariable(Dictionary<string, string> dic){
			
			Dictionary<string, string> table = new Dictionary<string, string>();
			table["${project}"] = NativeBuilderUtility.UnityProjectPath;
			table["${product_name}"] = PlayerSettings.productName;
			var copy = new Dictionary<string, string>(dic);
			
			foreach(var kv in copy){
				var key = kv.Key;
				var value = kv.Value;
				foreach(var vkv in table){
					value = value.Replace(vkv.Key, vkv.Value);
				}
				dic[key] = value;
			}
		}

		public static string TranslateVariable(string value)
		{
			Dictionary<string, string> table = new Dictionary<string, string>();
			table["${project}"] = NativeBuilderUtility.UnityProjectPath;
			table["${product_name}"] = PlayerSettings.productName;

			foreach(var kv in table)
			{
				value = value.Replace(kv.Key, kv.Value);
			}
			return value;

		}

		public static void SetDefualtIfNotExsist(Dictionary<string, string> dic, string key, string default_value)
		{
			if(!dic.ContainsKey(key))
			{
				dic[key] = default_value;
			}
		}

	}

}

