using System;
using System.Collections.Generic;
public class LeaderBoardImpl
{
	private static LeaderBoardImpl mInstance = null;
	/*!
	 * @Return instance of LeaderBoardImpl.
	 */
	public static LeaderBoardImpl GetInstance()
	{
		if(mInstance == null)
		{
			mInstance = new LeaderBoardImpl();
		}
		return mInstance;
	}
	
	
	/*!
	 * @getLeaderboard
	 * @discussion 
	 * @param {string} keys The params to get leader board.
	 * @param {function} onComplete The callback function that handles request complete.
	 */
	public  void getLeaderboard(string keys, SocialPFRequest.CallBackOnComplete onComplete) {
		Dictionary<string, object> info = MobageSerializer.DeSerialize(keys);
		SortedDictionary<string, object> parameters = new SortedDictionary<string, object>();		
		parameters.Add("appId", "@app");
		parameters.Add("leaderboardId", info["leaderboardId"]);
		parameters.Add("fields", info["leaderboard"]);
		(new SocialPFRequest()).RequestWithInfo(parameters, "leaderboards.get", onComplete);
	}
	
	/*!
	 * @getLeaderboards
	 * @discussion 
	 * @param {string} keys The params to get leader bards.
	 * @param {function} onComplete The callback function that handles request complete.
	 */
	public  void getLeaderboards(string keys, SocialPFRequest.CallBackOnComplete onComplete) {
		Dictionary<string, object> info = MobageSerializer.DeSerialize(keys);
		SortedDictionary<string, object> parameters = new SortedDictionary<string, object>();		
		parameters.Add("appId", "@app");
		parameters.Add("leaderboardId", info["leaderboardId"]);
		parameters.Add("fields", info["leaderboard"]);
		(new SocialPFRequest()).RequestWithInfo(parameters, "leaderboards.get", onComplete);
	}
	
	/*!
	 * @getAllLeaderboards
	 * @discussion 
	 * @param {string} keys The params to get all leader boads.
	 * @param {function} onComplete The callback function that handles request complete.
	 */
	public  void getAllLeaderboards(string keys, SocialPFRequest.CallBackOnComplete onComplete) {
		Dictionary<string, object> info = MobageSerializer.DeSerialize(keys);
		SortedDictionary<string, object> parameters = new SortedDictionary<string, object>();		
		parameters.Add("appId", "@app");
		parameters.Add("fields", info["leaderboard"]);
		(new SocialPFRequest()).RequestWithInfo(parameters, "leaderboards.get", onComplete);
	}
	
	/*!
	 * @getTopScoresList
	 * @discussion 
	 * @param {string} keys The params to get top score list.
	 * @param {function} onComplete The callback function that handles request complete.
	 */
	public  void getTopScoresList(string keys, SocialPFRequest.CallBackOnComplete onComplete ) {
		Dictionary<string, object> info = MobageSerializer.DeSerialize(keys);
		SortedDictionary<string, object> parameters = new SortedDictionary<string, object>();		
		parameters.Add("appId", "@app");
		parameters.Add("leaderboardId", info["leaderboardId"]);
		parameters.Add("userId", "@me");
		parameters.Add("groupId", "@all");
		parameters.Add("startIndex", Convert.ToInt32(info["OptStart"]));
		parameters.Add("count", Convert.ToInt32(info["OptCount"]));
		parameters.Add("fields", info["leaderboard"]);
		(new SocialPFRequest()).RequestWithInfo(parameters, "scores.get", onComplete);
	}
	
	/*!
	 * @getFriendsScoresList
	 * @discussion 
	 * @param {string} keys The params to get friends score list.
	 * @param {function} onComplete The callback function that handles request complete.
	 */
	public  void getFriendsScoresList(string keys, SocialPFRequest.CallBackOnComplete onComplete) {
		Dictionary<string, object> info = MobageSerializer.DeSerialize(keys);
		SortedDictionary<string, object> parameters = new SortedDictionary<string, object>();		
		parameters.Add("appId", "@app");
		parameters.Add("leaderboardId", info["leaderboardId"]);
		parameters.Add("userId", "@me");
		parameters.Add("groupId", "@friends");
		parameters.Add("startIndex", Convert.ToInt32(info["OptStart"]));
		parameters.Add("count", Convert.ToInt32(info["OptCount"]));
		parameters.Add("fields", info["leaderboard"]);
		(new SocialPFRequest()).RequestWithInfo(parameters, "scores.get", onComplete);
	}
	
	/*!
	 * @getScore
	 * @discussion 
	 * @param {string} keys The params string.
	 * @param {function} onComplete The callback function that handles request complete.
	 */
	public  void getScore(string keys ,SocialPFRequest.CallBackOnComplete onComplete) {
		Dictionary<string, object> info = MobageSerializer.DeSerialize(keys);
		SortedDictionary<string, object> parameters = new SortedDictionary<string, object>();		
		parameters.Add("appId", "@app");
		parameters.Add("leaderboardId", info["leaderboardId"]);
		parameters.Add("userId", info["userId"]);
		parameters.Add("groupId", "@self");
		parameters.Add("fields", info["leaderboard"]);
		(new SocialPFRequest()).RequestWithInfo(parameters, "scores.get", onComplete);
	}
	
	/*!
	 * @updateCurrentUserScore
	 * @discussion 
	 * @param {string} keys The params to update current user score.
	 * @param {function} onComplete The callback function that handles request complete.
	 */
	public  void updateCurrentUserScore(string keys, SocialPFRequest.CallBackOnComplete onComplete) {                           	
		Dictionary<string, object> info = MobageSerializer.DeSerialize(keys);
		SortedDictionary<string, object> score = new SortedDictionary<string, object>();
		score.Add("value", Convert.ToDouble(info["value"]));
		SortedDictionary<string, object> parameters = new SortedDictionary<string, object>();	
		parameters.Add("appId", "@app");
		parameters.Add("leaderboardId", info["leaderboardId"]);
		parameters.Add("userId", "@me");
		parameters.Add("groupId", "@self");
		parameters.Add("score", score);
		List<string> fields = new List<string>();
		fields.Add("value");
		parameters.Add("fields", fields);
		(new SocialPFRequest()).RequestWithInfo(parameters, "scores.update", onComplete);
	}
	
	/*!
	 * @deleteCurrentUserScore
	 * @discussion 
	 * @param {string} keys The params to delete current user score.
	 * @param {function} onComplete The callback function that handles request complete.
	 */
	public  void deleteCurrentUserScore(string keys, SocialPFRequest.CallBackOnComplete onComplete) {
		SortedDictionary<string, object> parameters = new SortedDictionary<string, object>();		
		parameters.Add("appId", "@app");
		parameters.Add("leaderboardId", keys);
		parameters.Add("userId", "@me");
		parameters.Add("groupId", "@self");
		(new SocialPFRequest()).RequestWithInfo(parameters, "scores.delete", onComplete);
	}
}

