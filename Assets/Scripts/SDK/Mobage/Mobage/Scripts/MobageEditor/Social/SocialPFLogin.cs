using System;
using System.Collections.Generic;
using System.Net;
using System.IO;
using System.Text;
using MobageLitJson;
using UnityEngine;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Threading;

public class SocialPFLogin
{
	private static string TAG = "SocialPFLogin";
	private static SocialPFLogin mInstance = null;
	private string mUserName = "";
	private string mPassword = "";
	public delegate void CallBackOnComplete(string response);
	private CallBackOnComplete OnLoginComplete = null;
	
	/*!
	 * @Return instance of SocialPFlogin.
	 */
	public static SocialPFLogin GetInstance()
	{
		if(mInstance == null) 
		{
			mInstance = new SocialPFLogin();
		}
		return mInstance;
	}
	
	/*!
	 * @Set info and create threading to login.
	 * @discussion 
	 * @param {string} userName The userName for login.
	 * @param {string} password The password for login.
	 * @param {function} onLoginComplate The callback function that handles login complete.
	 */
	public void LoginWithInfo(string userName, string password, CallBackOnComplete onLoginComplate)
	{
		mUserName = userName;
		mPassword = password;
		OnLoginComplete =  onLoginComplate;
	    Thread loginThread = new Thread(Login);
	    loginThread.Start();
	}
	
	/*!
	 * @Login  save response info into credentials.
	 * @Execute callback function
	 */
	public void Login()
	{	  
		try 
		{
			string responseString = LoginWithUsernameAndPassword();
			if(responseString != null)
			{
				MLog.d(TAG, "responseString:" + responseString); 
	   			ResponseInfo response = JsonMapper.ToObject<ResponseInfo>(responseString); 
				// update credentials
				Credentials.GetInstance().setToken(response.oauth_token);
				Credentials.GetInstance().setTokenSecret(response.oauth_token_secret);
				GlobalVar.GetInstance().UserID = response.user_id;
				OnLoginComplete(response.user_id);
				return;
			}
		}
		catch (Exception e) 
		{
			MLog.e(TAG, "error", e);

		}
		OnLoginComplete(null);
	}
	
	
	private bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
    { 
	   // always accept      
       return true;
    }
	
	/*!
	 * @Login with userName and Password.Create OAuth and use http get method
	 */
	private string LoginWithUsernameAndPassword()
	{
        string requestMethod = "GET";
		OAuth oauth = new OAuth();
 		SortedDictionary<string, string> xoauthParams = CreateOAuthParams(State.ON_LOGIN);
		xoauthParams.Add("debug_login_id",mUserName);
		xoauthParams.Add("debug_login_pw",mPassword);
		xoauthParams.Add("oauth_callback","ngcore:///oauth_callback");
		string url = HostConfig.GetInstance().GetPFLoginURL();
		oauth.completeRequest(requestMethod, url, xoauthParams);
		url += "?" + oauth.getQueryStrings();
		ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(CheckValidationResult);
		HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url.Replace("https","http"));
		//HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
		request.Method = requestMethod;
		request.UserAgent = "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_8_0) AppleWebKit/537.1 (KHTML, like Gecko) Chrome/21.0.1180.89 Safari/537.1 Android";
		HostConfig.GetInstance().LoginURL =  url;
		MLog.d(TAG, "loginWithUsernameAndPassword url:"+url);
		try 
		{       
			//HTTP Get Request
			HttpWebResponse wrep = (HttpWebResponse)request.GetResponse();
			if(wrep.StatusCode == HttpStatusCode.OK)
			{
				MLog.d(TAG, wrep.StatusCode.ToString());
	        	Stream s = wrep.GetResponseStream();
				Byte[] buff = new Byte[1024];
	        	int iread = 0;
	        	StringBuilder sb = new StringBuilder();
	        	while ((iread = s.Read(buff, 0, buff.Length)) > 0)
	        	{
	            	sb.Append(Encoding.UTF8.GetString(buff, 0, iread));
	        	} 
		        string response = sb.ToString(); 
	       	 	s.Close();
	        	wrep.Close();
				return  response;
			} 
		}
		catch (Exception e) 
		{
			MLog.e(TAG, "error", e);
		}
	    return null;
	}
	
	/*!
	 * @Create SortedDictionary to save tag of state
	 * @discussion 
	 * @param {State} Enum value of current state.
	 */
	private SortedDictionary<string, string> CreateOAuthParams(State state) {
		SortedDictionary<string, string> xoauthParams = new SortedDictionary<string, string>();
		xoauthParams.Add("on_login", (state == State.ON_LOGIN) ? "1" : "");
		xoauthParams.Add("on_launch", (state == State.ON_LAUNCH) ? "1" : "");
		xoauthParams.Add("on_resume", (state == State.ON_RESUME) ? "1" : "");
		xoauthParams.Add("on_game_init", (state == State.ON_INIT) ? "1" : "");
		return xoauthParams;
	}
	
	private enum State {
		DEFAULT,
		ON_LOGIN,
		ON_LAUNCH,
		ON_RESUME,
		ON_INIT,
	};
}

class AppInfo
{
	public string nobackground_updates = "";
}  

/*!
 * @Class for save response info after login complete
 */
class ResponseInfo
{
	 public int oauth_expires_in = 0;
	 public string oauth_token = "";
	 public string user_id = "";
	 public AppInfo app_info = null;
	 public string oauth_token_secret = ""; 
}
