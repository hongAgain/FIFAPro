using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using System;

namespace NativeBuilder
{
	public abstract class Conf_Base : Properties {

		public Conf_Base(string path) : base(path) {}

		public virtual void Generate(){}

		public virtual bool Exists(){ return false;}

		/// <summary>
		/// 修补这个Conf文件
		/// ● 当第一次运行时候，可能Conf文件并不存在，需要生成默认值
		///	● 当引用的SDK路径失效时，通知重新设置
		///	● 当NativeBuilder升级时，旧的配置文件可能缺少某些项目
		/// </summary>
		public virtual void Repaire(){}

		public override string this[string key]
		{
			get 
			{
				return ConfUtility.TranslateVariable(base[key]);
			}
			set
			{
				base[key] = value;
			}
		}

	}


	public class ConfigurationException : Exception
	{
		public ConfigurationException(string msg) : base(msg) {}
	}

}

