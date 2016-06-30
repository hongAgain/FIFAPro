using System;
using MobageLitJson;
using System.Collections.Generic;

namespace Proxy
{
	public class setExtraData
	{

		public static void Invock(String ext) 
		{
			MobageNative.setExtraData (ext);
		}
	}
}

