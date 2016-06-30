using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Threading;
using UnityEngine;
public class SocialPFRequest
{
	private string mPostBody = null;
	private string TAG = "SocialPFRequest";
	private static SocialPFRequest mInstance = null;
    public delegate void CallBackOnComplete(string response);
	private CallBackOnComplete OnComplete = null;
	private int mFlag = 0;
	
	/*!
	 * @Return instance of SocialPFRequest.
	 */
	public static SocialPFRequest GetInstance()
	{
		if(mInstance == null) mInstance = new SocialPFRequest();
		return mInstance;
	}
	
	private bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
    {  
	   // always accept   
       return true;
    }
		
	/*!
	 * @Request with url.Create OAuth and use http post method
	 * @discussion 
	 * @param {string} request url.
	 */
	private  string GetResponse(string url) 
	{
		OAuth oauth = new OAuth();
	    SortedDictionary<string, string> parameters = new SortedDictionary<string, string>();
		parameters.Add("xoauth_mobile_carrier", "CMCC");
		parameters.Add("xoauth_requestor_id", GlobalVar.GetInstance().UserID);
	    oauth.CompleteRequestWithPostBody ("POST", url, parameters, mPostBody );
		string header = oauth.GetAuthorizationHeader();
		MLog.d(TAG, "header:" + header);
	    ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(CheckValidationResult);
		HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url); 
		request.Method = "POST";
		request.UserAgent = "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_8_0) AppleWebKit/537.1 (KHTML, like Gecko) Chrome/21.0.1180.89 Safari/537.1";
   		request.ContentType = "application/json; charset=utf8";
   	 	request.Credentials = CredentialCache.DefaultCredentials;
        request.Headers.Set("Authorization", header);
	    // set postbody
		if(mPostBody != null)
		{
			try
			{
			byte[] buffer = Encoding.UTF8.GetBytes(mPostBody);
            request.ContentLength = buffer.Length;
            request.GetRequestStream().Write(buffer, 0, buffer.Length);
			}
			catch(Exception e)
			{
				MLog.e(TAG, "error", e);
			}
		}
		try 
		{       
			// Request HTTP Post Request
			HttpWebResponse wrep = (HttpWebResponse)request.GetResponse();
			if(wrep.StatusCode == HttpStatusCode.OK)
			{
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
			    return response;
			}
		} 
		catch (Exception e) 
		{
			MLog.e(TAG, "error", e);
		}
		return null;
	}
	
	
	/*!
	 * @Get random value for each request 
	 */
	private string GetUnixEpoc() {
		
		TimeSpan ts = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc));
		long tick = (long)ts.TotalMilliseconds;
		return (tick / 1000L).ToString();
	}
	
	
	/*!
	 * @Execute request and callback.
	 */
	public void Request()
	{
		string url = HostConfig.GetInstance().GetPFRequestURL(mFlag);
		string jsonString = GetResponse(url);
		if(jsonString == null)
		{
			MLog.e (TAG, "Request failed!");
		}
		else OnComplete(jsonString);
	}
	
	
	/*!
	 * @Set info and create threading to request with default flag.
	 * @discussion 
	 * @param {SortedDictionary<string, object>} request parameters .
	 * @param {string} request method.
	 * @param {function} onComplete The callback function that handles request complete.
	 */
	public void RequestWithInfo(SortedDictionary<string, object> parameters, string method, CallBackOnComplete onComplete)
	{
		RequestWithInfo(parameters, method, onComplete, 0);
	}
	
	/*!
	 * @Set info and create threading to request.
	 * @discussion 
	 * @param {SortedDictionary<string, object>} request parameters .
	 * @param {string} request method.
	 * @param {function} onComplete The callback function that handles request complete.
	 * @param {int} flag identification whether it is new api.
	 */
	public void RequestWithInfo(SortedDictionary<string, object> parameters, string method, CallBackOnComplete onComplete, int flag)
	{
		SortedDictionary<string, object> postBody = new SortedDictionary<string, object>();
		postBody.Add("jsonrpc", "2.0");
		postBody.Add("method", method);
		postBody.Add("id", GetUnixEpoc());
		postBody.Add("params",  parameters);
		mPostBody =  MobageSerializer.Serialize(postBody);
		OnComplete = onComplete;
		mFlag = flag;
		MLog.d(TAG, "PostBodyString:" + mPostBody);
		Thread requestThread = new Thread(Request);
	    requestThread.Start();	
	}
}
