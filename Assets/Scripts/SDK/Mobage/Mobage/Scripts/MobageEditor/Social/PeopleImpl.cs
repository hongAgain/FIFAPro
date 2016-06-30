using System;
using System.Collections.Generic;
public class PeopleImpl
{
	//private string TAG = "PeopleImpl";
	private  static PeopleImpl mInstance = null;
	/*!
	 * @Return instance of PeopleImpl.
	 */
	public static PeopleImpl GetInstance()
	{
		if(mInstance == null)
		{
			mInstance = new PeopleImpl();
		}
		return mInstance;
	}
	
	/*!
	 * @GetCurrentUser
	 * @discussion 
	 * @param {string} keys The params to get current user.
	 * @param {function} onComplete The callback function that handles request complete.
	 */
	public void GetCurrentUser(string keys, SocialPFRequest.CallBackOnComplete onComplete) 
	{
	    Dictionary<string, object> info = MobageSerializer.DeSerialize(keys);
		SortedDictionary<string, object> parameters = new SortedDictionary<string, object>();
		List<string> fieldsSet = ComplyWithRequiredFields(info["user"] as List<string>);
		parameters.Add("fields", fieldsSet);
		parameters.Add("userId", "@me");
		(new SocialPFRequest()).RequestWithInfo(parameters, "people.get", onComplete);
	}
	
	/*!
	 * @GetFriends
	 * @discussion 
	 * @param {string} keys The params to get friends.
	 * @param {function} onComplete The callback function that handles request complete.
	 */
	public void GetFriends(string keys, SocialPFRequest.CallBackOnComplete onComplete)
	{
		Dictionary<string, object> info = MobageSerializer.DeSerialize(keys);
		SortedDictionary<string, object> parameters = new SortedDictionary<string, object>();
		List<string> fieldsSet = ComplyWithRequiredFields(info["user"] as List<string>);
		parameters.Add("startIndex", Convert.ToInt32(info["OptStart"]));
		parameters.Add("count", Convert.ToInt32(info["OptCount"]));
		parameters.Add("fields", fieldsSet);
		parameters.Add("userId", info["userId"]);
		parameters.Add("groupId", "@friends");
		(new SocialPFRequest()).RequestWithInfo(parameters, "people.get", onComplete);
	}
	
	/*!
	 * @GetUser
	 * @discussion 
	 * @param {string} keys The params to get user.
	 * @param {function} onComplete The callback function that handles request complete.
	 */
	public void GetUser(string keys, SocialPFRequest.CallBackOnComplete onComplete)
	{
		Dictionary<string, object> info = MobageSerializer.DeSerialize(keys);
		SortedDictionary<string, object> parameters = new SortedDictionary<string, object>();
		List<string> fieldsSet = ComplyWithRequiredFields(info["user"] as List<string>);
		parameters.Add("fields", fieldsSet);
		parameters.Add("userId", info["userId"]);
		(new SocialPFRequest()).RequestWithInfo(parameters, "people.get", onComplete);
	}
	
	/*!
	 * @GetUsers
	 * @discussion 
	 * @param {string} keys The params to get users.
	 * @param {function} onComplete The callback function that handles request complete.
	 */
	public void GetUsers(string keys, SocialPFRequest.CallBackOnComplete onComplete)
	{
		Dictionary<string, object> info = MobageSerializer.DeSerialize(keys);
		SortedDictionary<string, object> parameters = new SortedDictionary<string, object>();
		List<string> fieldsSet = ComplyWithRequiredFields(info["user"] as List<string>);
	
		parameters.Add("fields", fieldsSet);
		parameters.Add("userId", info["userId"]);
		(new SocialPFRequest()).RequestWithInfo(parameters, "people.get", onComplete);
	}
	
	/*!
	 * @GetFriendsWithGame
	 * @discussion 
	 * @param {string} keys The params to get friends of current user.
	 * @param {function} onComplete The callback function that handles request complete.
	 */
	public void GetFriendsWithGame(string keys, SocialPFRequest.CallBackOnComplete onComplete)
	{
		Dictionary<string, object> info = MobageSerializer.DeSerialize(keys);
		SortedDictionary<string, object> parameters = new SortedDictionary<string, object>();
		List<string> fieldsSet = ComplyWithRequiredFields(info["user"] as List<string>);
		parameters.Add("startIndex", Convert.ToInt32(info["OptStart"]));
		parameters.Add("count", Convert.ToInt32(info["OptCount"]));
		parameters.Add("fields", fieldsSet);
		parameters.Add("userId", info["userId"]);
		parameters.Add("groupId", "@friends");
		parameters.Add("filterBy", "hasApp");

		parameters.Add("filterOp", "equals");
		parameters.Add("filterValue", 1);
		(new SocialPFRequest()).RequestWithInfo(parameters, "people.get", onComplete);
	}

	private List<string> ComplyWithRequiredFields(List<string> fields) 
	{
		List<string> newFields = fields;
		string[] requiredFields={"id","nickname","hasApp","thumbnailUrl","age","gender"};	
		for (int i = 0 ; i < requiredFields.Length ; i++){
			if(newFields.Contains(requiredFields[i]) == false)
			{
				newFields.Add(requiredFields[i]);
			}
		}
		return newFields;
	}
}


