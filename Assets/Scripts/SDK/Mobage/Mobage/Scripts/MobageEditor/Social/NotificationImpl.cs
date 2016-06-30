using System;
using System.Collections.Generic;
public class NotificationImpl
{
	//private string TAG = "NotificationImpl";
	private static NotificationImpl mInstance = null;
	/*!
	 * @Return instance of NotificationImpl.
	 */
	public static NotificationImpl GetInstance()
	{
		if(mInstance == null)
		{
			mInstance = new NotificationImpl();
		}
		return mInstance;
	}
	
	
	/*!
	 * @pushSend
	 * @discussion 
	 * @param {string} keys The params to pushSend.
	 * @param {function} onComplete The callback function that handles request complete.
	 */
	public  void pushSend(string keys, SocialPFRequest.CallBackOnComplete onComplete) {
		Dictionary<string, object> info = MobageSerializer.DeSerialize(keys);
		SortedDictionary<string, object> parameters = new SortedDictionary<string, object>();
		parameters.Add("appId", "@app");
		parameters.Add("recipientId", info["recipientId"]);
		parameters.Add("groupId", "@all");
		SortedDictionary<string, object> payload = new SortedDictionary<string, object>();
		payload.Add("badge", info["badge"]);
		payload.Add("message", MobageSerializer.RevertToString(info["message"]));
		payload.Add("sound", info["sound"]);
		payload.Add("collapseKey", info["collapseKey"]);
		payload.Add("style", info["style"]);
		payload.Add("iconUrl", info["iconUrl"]);
		SortedDictionary<string, object> extras = new SortedDictionary<string, object>();
		foreach(string extra in info["extras"] as List<string>)
		{
			string[] extraInfo = extra.Split(':');
			extras.Add(extraInfo[0], extraInfo[1]);
		}
		payload.Add("extras", extras);
		
		
		SortedDictionary<string, object> remoteNotification = new  SortedDictionary<string, object>();
		remoteNotification.Add("payload", payload);
		parameters.Add("remoteNotification", remoteNotification);
		(new SocialPFRequest()).RequestWithInfo(parameters, "remotenotification.send", onComplete);
	}
	
	/*!
	 * @getPushEnabled
	 * @discussion 
	 * @param {function} onComplete The callback function that handles request complete.
	 */
	public  void getPushEnabled(SocialPFRequest.CallBackOnComplete onComplete) {
		SortedDictionary<string, object> parameters = new SortedDictionary<string, object>();
		List<string> fields = new List<string>();
		fields.Add("state");
		parameters.Add("appId", "@app");
		parameters.Add("userId", "@me");
		parameters.Add("fields", fields);
		(new SocialPFRequest()).RequestWithInfo(parameters, "remotenotification.getConfig", onComplete);
	}
	
	/*!
	 * @setPushEnabled
	 * @discussion 
	 * @param {string} enabled The param to set PushEnabled.
	 * @param {function} onComplete The callback function that handles request complete.
	 */
	public  void setPushEnabled(string enabled, SocialPFRequest.CallBackOnComplete onComplete) {
		SortedDictionary<string, object> config = new SortedDictionary<string, object>();	
		config.Add("state", enabled == "true"?1:0);
		SortedDictionary<string, object> parameters = new SortedDictionary<string, object>();
		parameters.Add("appId", "@app");
		parameters.Add("userId", "@me");
		parameters.Add("config", config);
		(new SocialPFRequest()).RequestWithInfo(parameters, "remotenotification.updateConfig", onComplete);
	}
	
	public  void handleListener(SocialPFRequest.CallBackOnComplete onComplete ) {
		
	}
	/*
	public void UpdateToken(RemoteNotificationToken token, SocialPFRequest.CallBackOnComplete onComplete)
	{
		SortedDictionary<string, object> parameters = new SortedDictionary<string, object>();	
		parameters.Add("appId", "@app");
		parameters.Add("userId", "@me");
		SortedDictionary<string, object> tokenDic= new SortedDictionary<string, object>();	
		tokenDic.Add("value", token.Value);
		tokenDic.Add("deviceId", token.DeviceId);
		tokenDic.Add("bundleId", token.BundleId);
		parameters.Add("token", token);
		(new SocialPFRequest()).RequestWithInfo(parameters, "remotenotification.updateToken", onComplete);
		Request();
	}
	*/
	
	
}
/*
public class RemoteNotificationToken
{
	string val = "";
	string deviceId = "";
	string bundleId = "";
	public string Value{ get  {return val;} }
	public string DeviceId{ get  {return deviceId;} }
	public string BundleId{ get  {return bundleId;} }
}
*/

