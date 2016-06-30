using System;
using MobageLitJson;
using System.Collections.Generic;

namespace Proxy
{
	public class GetFacebookUser
	{
		public delegate void Callback (MobageAdvertiseUser user);
		
		private static Callback _callback{ get; set;}
		
		public static void Invock(GetFacebookUser.Callback callback) {
			
			GetFacebookUser._callback = callback;

			MobageNative.GetFacebookUser();
			
			return;
		}
		
		
		public static void onNativeCallback(string result)
		{
			MobageAdvertiseUser user;
			if(result == "null"){
				user = null;
			}else{
				user = JsonMapper.ToObject<MobageAdvertiseUser>(result);
			}
			_callback(user);
		}


	}
	
	
}

