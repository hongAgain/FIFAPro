using System;
using MobageLitJson;
using System.Collections.Generic;

namespace Proxy
{
	public class GetCurrentUser
	{
		public delegate void OnSuccessDelegate (MobageUser user);
		public delegate void OnErrorDelegate (MobageError err);

		private static OnSuccessDelegate OnSuccess{ get; set;}
		private static OnErrorDelegate OnError{ get; set;}

		// People
		public static void Invock(List<string> fields, 
		                                  GetCurrentUser.OnSuccessDelegate onSuccess, 
		                                  GetCurrentUser.OnErrorDelegate onError) {
			
			GetCurrentUser.OnSuccess = onSuccess;
			GetCurrentUser.OnError = onError;
			
			string chunk = GetCurrentUser.Encode(fields);
			
			MobageNative.SocialRequestDispatcherGetCurrentUser(chunk);
			
			return;
		}

		public static string Encode(List<string> fields) 
		{
			int i = 0;
			string chunk = "";
			if(fields == null){
				chunk = "user::null";
			}else{
				chunk = "user::{";
				while ( i < fields.Count ) {
					chunk += fields[i] as string;
					i++;
					if(i < fields.Count){
						chunk += ",";
					}else{
						break;
					}
				}
				chunk += "}";
			}
			
			return MobageDispatcher.ToUTF8(chunk);
		}

		public static void onNativeSuccess(string utf8string)
		{
			string key = MobageDispatcher.UTF8ToUnicode(utf8string);
			
			// START:: to aboid LitJson exception cause NativeSDK interface is different(iOS, Android).
			key = userUtilityForUserAge(key);
			// END::to aboid LitJson exception cause NativeSDK interface is different(iOS, Android).

			try
			{

				MobageUser out_user = JsonMapper.ToObject<MobageUser> (key);
				OnSuccess(out_user);
			}
			catch
			{
				MobageError out_err = new MobageError();
				out_err.code = 500;
				out_err.description = "Internal error.";
				OnError(out_err);
			}
		}
		
		public static void onNativeError(string key)
		{
			MobageError out_err;
			try
			{
				out_err = JsonMapper.ToObject<MobageError> (key);
				OnError(out_err);
			}
			catch
			{
				out_err = new MobageError();
				out_err.code = 500;
				out_err.description = "Internal error.";
				OnError(out_err);
			}
		}

		// START:: to aboid LitJson exception cause NativeSDK interface is different(iOS, Android).
		private static string userUtilityForUserAge(string key){
			int num = key.IndexOf("age");
			if(num<0){
				return key;
			}
			if(key[(num+4)]==':'){
				num+=5;
				if(key[num]=='"'){
					key = key.Remove(num,1);
				}
				for (/*num--*/ ; num < key.Length ; num++ ){
					if(key[num]==',' || key[num]=='}'){
						if(key[(num-1)]=='"'){
							key = key.Remove((num-1),1);
						}
						break;
					}
				}
			}
			return key;
		}
		// END::to aboid LitJson exception cause NativeSDK interface is different(iOS, Android).
	}

}

