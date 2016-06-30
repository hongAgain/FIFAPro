using System;
using MobageLitJson;
using System.Collections.Generic;

namespace Proxy
{
	public class GetAffcode
	{

		public static String Invock() 
		{

			return MobageNative.GetAffcode ();
		}
	}
}

