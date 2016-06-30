using System;
public class MobageCNVendorId
{
	public static void  getMobageVendorId (OnGetMobageVendorIdokCallBackLib.OnSuccess onSuccess, OnGetMobageVendorIdokCallBackLib.OnError onError)
	{
		 MobageManager.getMobageVendorId( onSuccess, onError);
	}
}

