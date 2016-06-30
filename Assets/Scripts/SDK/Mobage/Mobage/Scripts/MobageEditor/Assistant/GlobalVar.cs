using System;


public class GlobalVar
{
	private string userID = "";
	private string appID = "";
	private static GlobalVar mInstance = null;
	public static GlobalVar GetInstance()
	{
		if(mInstance == null)
		{
			mInstance = new GlobalVar();
		}
		return mInstance;
	}
	
	public string UserID 
	{
		get { return userID;}
		set { userID = value;}
	}
	public string AppID
	{
		get { return appID;}
		set { appID = value;}
	}
}


