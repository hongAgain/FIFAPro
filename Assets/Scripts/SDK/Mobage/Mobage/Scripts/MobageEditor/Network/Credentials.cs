using System;
public class Credentials
{
	private string consumerKey = "";
	private string consumerSecret = "";
	private string token = "";
	private string tokenSecret = "";
	private  static Credentials mInstance = null;
	public static Credentials GetInstance()
	{
		if(mInstance == null)
		{
			mInstance = new Credentials();
		}
		return mInstance;
	}

	public string getConsumerKey() {
		return consumerKey;
	}
	
	public void setConsumerKey(string consumerKey) {
		this.consumerKey = consumerKey;
	}
	
	public string getConsumerSecret() {
		return consumerSecret;
	}
	
	public void setConsumerSecret(string consumerSecret) {
		this.consumerSecret = consumerSecret;
	}
	
	public string getToken() {
		return token;
	}
	
	public void setToken(string token) {
		this.token = token;
	}
	
	public string getTokenSecret() {
		return tokenSecret;
	}
	
	public void setTokenSecret(string tokenSecret) {
		this.tokenSecret = tokenSecret;
	}
}


