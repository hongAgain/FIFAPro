using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.IO;

public class AuthImpl
{
	private string TAG = "AuthImpl";
	private  static AuthImpl mInstance = null;
	/*!
	 * @Return instance of AuthImpl.
	 */
	public static AuthImpl GetInstance()
	{
		if(mInstance == null)
		{
			mInstance = new AuthImpl();
		}
		return mInstance;
	}

	private  string GetResponse(string url,string consumerKey, string consumerSecret) 
	{
		
		Credentials credential = new Credentials();
		credential.setConsumerKey(consumerKey);
		credential.setConsumerSecret(consumerSecret);
		OAuth oauth = new OAuth(credential);
	    SortedDictionary<string, string> parameters = new SortedDictionary<string, string>();
		parameters.Add("oauth_callback", "oob");
	    oauth.CompleteRequestWithPostBody ("POST", url, parameters, null);
		string header = oauth.GetAuthorizationHeader();
		MLog.d(TAG, "Header:"+ header);
		HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
	    request.UserAgent = "nativesdk-android/1.2.5";
		request.Method = "POST";
   		request.ContentType = "application/json; charset=utf8";
        request.Headers.Set("Authorization", header);  

		try 
		{       
			// Execute HTTP Post Request
			HttpWebResponse wrep = (HttpWebResponse)request.GetResponse();
			if(wrep.StatusCode == HttpStatusCode.OK)
			{
	        	Stream s = wrep.GetResponseStream();
				Byte[] buff = new Byte[1024];
	        	int iread = 0;
	        	StringBuilder sb = new StringBuilder();
	        	while ((iread = s.Read(buff, 0, buff.Length)) > 0)
	        	{
	            	sb.Append(Encoding.Default.GetString(buff, 0, iread));
	        	}
				string response = sb.ToString();			 		
	       	 	s.Close();
	        	wrep.Close();
			    return response;
			}
		} 
		catch (Exception e) 
		{
			MLog.e(TAG, "error", e);
		}
		return null;
	}
	
	public string FetchToken(string consumerKey, string consumerSecret)
	{
		string url = "http://";
		url+= HostConfig.GetInstance().PFAPIDomain;
		url += "/social/api/oauth/v2/request_temporary_credential";
		PTools.SetMLogDebug (true);

		MLog.i(TAG,"fetchToken url: " + url);
		string jsonString = GetResponse(url, consumerKey, consumerSecret);
		Dictionary<string, string> parameters = DecodeURL(jsonString);
		MLog.i(TAG, "Oauth_token:" + parameters["oauth_token"].ToString() + " Oauth_token_secret:" + parameters["oauth_token_secret"].ToString());			
		return parameters["oauth_token"];
	}

	private  Dictionary<string, string> DecodeURL(string s)
	{
		Dictionary<string, string> parameters = new Dictionary<string, string>();
		if (s != null) 
		{
			string[] array = s.Split('&');
			foreach (string parameter in array) 
			{
				string[] v = parameter.Split('=');
				if (v.Length == 2) 
				{
					parameters.Add(URLDecoder.Decode(v[0]), URLDecoder.Decode(v[1]));
				} 
				else if (v.Length == 1)
				{
					parameters.Add(URLDecoder.Decode(v[0]), "");
				}
			}
		}
		return parameters;
	}
	/*!
	 * @AuthorizeToken
	 * @discussion 
	 * @param {string} token. the token value .
	 * @param {function} onComplete The callback function that handles request complete.
	 */
	public void AuthorizeToken(string token, SocialPFRequest.CallBackOnComplete onComplete) 
	{
		SortedDictionary<string, object> parameters = new SortedDictionary<string, object>();		
		parameters.Add("token",token);
		(new SocialPFRequest()).RequestWithInfo(parameters, "accesstoken.authorizeToken", onComplete);
	}
}