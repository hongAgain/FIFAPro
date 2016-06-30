using System;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

public class OAuth
{
	private static string TAG = "OAuth";
	private Credentials mCredentials;
	private SortedDictionary<string, string> mParams = new SortedDictionary<string, string>();
	
	/*!
	 * @OAuth constructor,set credentials with default
	 */
	public OAuth()
	{
		mCredentials = Credentials.GetInstance();
	}
	
	/*!
	 * @OAuth constructor,set credentials with new one
	 */
	public OAuth(Credentials credentials)
	{
		mCredentials = credentials;
	}
	
	/*!
	 * @get Query String from parameter dictionary.
	 * @param params
	 * @return QueryStrings
	 */
	public string getQueryStrings(){
		string queryStrings = "";
		IEnumerator<string> it = mParams.Keys.GetEnumerator();
		int index = 0;
		while(it.MoveNext()){
			if(index != 0){ queryStrings += "&"; }
			string key = it.Current.ToString();
			queryStrings += key + "=" + mParams[key];
			index++;
		}
		return queryStrings;
	}
	
	/**
	 * make OAuth header from parameter dictionary. 
	 * @param params
	 * @return OAuth header string.
	 */
	public  string GetAuthorizationHeader() 
	{
		string header = "OAuth ";
		int i = 1;
		foreach (string key in mParams.Keys) 
		{
			string val = mParams[key];
			header += key + "=\"" + val + "\"";
			if (i < mParams.Count)
			{
				header += ",";
			}
			i++;
		}
		MLog.d(TAG, "Authorization:" + header);
		return header;
	}
	
	/*!
	 * @generate parameter dictionary.
	 * @param {string} method
	 * @param {string} url
	 * @param {SortedDictionary<string, string>} parameters
	 */
	public void completeRequest(string method, string url, SortedDictionary<string, string> parameters){
		try {
			IEnumerator<string> it = parameters.Keys.GetEnumerator();

			while(it.MoveNext()){
				string key = it.Current.ToString();
				string val = parameters[key];
				mParams.Add(URLEncoder.Encode(key), URLEncoder.Encode(val));
			}

			mParams.Add("oauth_consumer_key", URLEncoder.Encode(mCredentials.getConsumerKey()));
			mParams.Add("oauth_nonce", URLEncoder.Encode(this.getNonce()));
			mParams.Add("oauth_signature_method", URLEncoder.Encode("HMAC-SHA1"));

			mParams.Add("oauth_timestamp", getUnixEpoc());
			mParams.Add("oauth_token", URLEncoder.Encode(mCredentials.getToken()));
			mParams.Add("oauth_version", URLEncoder.Encode("1.0"));
			string baseString = getBaseString(method, url , mParams);
			string signature = getSignature(baseString);
			mParams.Add("oauth_signature", URLEncoder.Encode(signature));
			
		} catch (Exception e) {
			MLog.e(TAG, "error", e);
		}
	}

	/*!
	 * @generate parameter dictionary.
	 * @param {string} method
	 * @param {string} url
	 * @param {SortedDictionary<string, string>} parameters
	 */
	public void CompleteRequestWithPostBody(string method, string url, SortedDictionary<string, string> parameters,string postBody)
	{
		try 
		{
			IEnumerator<string> it = parameters.Keys.GetEnumerator();
			while(it.MoveNext())
			{
				string key = it.Current.ToString();
				string val = parameters[key];
				mParams.Add(URLEncoder.Encode(key), URLEncoder.Encode(val));
			}
			if(postBody != null)
			{
			 	string bodyHash = Base64_sha1(postBody);
				mParams.Add("oauth_body_hash", URLEncoder.Encode(bodyHash));
			}
			
			mParams.Add("oauth_consumer_key", URLEncoder.Encode(mCredentials.getConsumerKey()));

			mParams.Add("oauth_nonce",  getNonce());
			mParams.Add("oauth_signature_method", URLEncoder.Encode("HMAC-SHA1"));
			mParams.Add("oauth_timestamp", getUnixEpoc());

       	 	if(mCredentials.getToken() != "")
				mParams.Add("oauth_token", URLEncoder.Encode(mCredentials.getToken()));

			mParams.Add("oauth_version",URLEncoder.Encode("1.0"));
			string baseString = getBaseString(method, url , mParams);
			string signature = getSignature(baseString);
			mParams.Add("oauth_signature", URLEncoder.Encode(signature));
			
		} catch (Exception e) {
			// TODO Auto-generated catch block
			MLog.e(TAG, "error", e);

		}
	}
	
	/*!
	 * @generate base string for calculate signature.
	 * @param {string} method
	 * @param {string} url
	 * @param {SortedDictionary<string, string>} parameters
	 * @return {string} baseString
	 */
	private string getBaseString(string method, string url, SortedDictionary<string, string> parameters){
		
		string queryStrings = "";
		IEnumerator<string> it = parameters.Keys.GetEnumerator();
		int index = 0;
		
		while(it.MoveNext()){
			if(index != 0){ queryStrings += "&"; }
			string key = it.Current.ToString();
			queryStrings += key + "=" + parameters[key];
			index++;
		}
		string baseString = "";
		try {
			baseString = URLEncoder.Encode(method) + "&";
			baseString += URLEncoder.Encode(url) + "&";
			baseString += URLEncoder.Encode(queryStrings);
		} catch (Exception e) {
			MLog.e(TAG, "error", e);	
		}
		MLog.d(TAG, "baseString:"+baseString);
		return baseString;
	}
	
	
	/*!
	 * @Calculate signature with base string.
	 * @param {string} baseString
	 * @return {string} signature
	 */
	private string getSignature(string baseString){
		string key = mCredentials.getConsumerSecret() + "&" + mCredentials.getTokenSecret();
		string signature="";
		try 
		{
			MLog.d(TAG,"yxx====>"+key);
			System.Security.Cryptography.HMACSHA1 hmacsha1 = new System.Security.Cryptography.HMACSHA1();
            hmacsha1.Key = Encoding.UTF8.GetBytes(key);
			System.Text.StringBuilder res = new System.Text.StringBuilder();
			foreach (byte b in hmacsha1.Key)
			{
		    	res.Append(b.ToString("x2"));
			}

            byte[] result = hmacsha1.ComputeHash(Encoding.UTF8.GetBytes(baseString));
			
			signature = Convert.ToBase64String(result).Trim();

			} 
			catch (Exception ex) 
			{
				MLog.e(TAG, "error", ex);
			} 
			
			return signature;
		
	}
	
	/*!
	 * @Get random value.
	 */
	private string getNonce() {
		int RandomNum = 6;
		string a = "abcdefghijklmnopqrstuvwxyz"
				 + "ABCDEFGHIJKLMNOPQRSTUVWXYZ"
				 + "0123456789";
		StringBuilder s = new StringBuilder(6);
		Random rnd = new Random();
		for (int i = 0; i < RandomNum; i++) {
			s.Append(a[Math.Abs(rnd.Next() % a.Length)]);
		}
		return s.ToString();
	}
	
	/*!
	 * @Get random value.
	 */
	private string getUnixEpoc() {
		
		TimeSpan ts = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc));
		long tick = (long)ts.TotalMilliseconds;
		return (tick / 1000L).ToString();		
	}
	
	
	/*!
	 * @Calculate hash value with postBody.
	 * @param {string} postBody
	 * @return {string} hash
	 */
	private string Base64_sha1(string postBody)
	{
		string hash = "";
		if(postBody != null)
		{
			try
			{
				HashAlgorithm sha = new SHA1CryptoServiceProvider();
				byte[] result = sha.ComputeHash(Encoding.UTF8.GetBytes(postBody));
	            hash = Convert.ToBase64String(result).Trim();
			} 
		
			catch (Exception e) 
			{
				MLog.e(TAG, "error", e);
			} 
		}
		MLog.d(TAG, "hash:" + hash);
		
		return hash;
	}
	
}


