using System;
using MobageLitJson;
using System.Collections.Generic;

namespace Proxy
{
	public class HasCustomerServicePage
	{

		public static bool Invock() 
		{
			return MobageNative.HasCustomerServicePage ();
		}
	}
}

