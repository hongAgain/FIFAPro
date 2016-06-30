using System;
using System.Collections.Generic;
public class BlackListImpl
{	
	private  static BlackListImpl mInstance = null;
	/*!
	 * @Return instance of BlackListImpl.
	 */
	public static BlackListImpl GetInstance()
	{
		if(mInstance == null)
		{
			mInstance = new BlackListImpl();
		}
		return mInstance;
	}
	
	/*!
	 * @CheckBlackList
	 * @discussion 
	 * @param {string} keys The params to check black list.
	 * @param {function} onComplete The callback function that handles request complete.
	 */
	public void CheckBlackList(string keys, SocialPFRequest.CallBackOnComplete onComplete)
	{
		Dictionary<string, object> info = MobageSerializer.DeSerialize(keys);
		SortedDictionary<string, object> parameters = new SortedDictionary<string, object>();
		parameters.Add("startIndex", Convert.ToInt32(info["OptStart"]));
		parameters.Add("count", Convert.ToInt32(info["OptCount"]));
	
		parameters.Add("userId", info["userId"]);
		parameters.Add("groupId", "@all");
		if( info["targetUserId"].ToString() != "")
		parameters.Add("personId", info["targetUserId"]);
		(new SocialPFRequest()).RequestWithInfo(parameters, "blacklist.get", onComplete);	
	}
}


