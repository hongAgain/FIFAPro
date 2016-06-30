using System;
public class HostConfig
{
	//HostJP
	private  string kJPPFApiDomainSandbox = "sb.sp.mbga-platform.jp";
	private  string kJPPFApiDomainProduction = "sp.mbga-platform.jp";
	
	private  string kJPSPWebBaseHostSandbox = "sb-sp.mbga.jp";
	private  string kJPSPWebBaseHostProduction = "sp.mbga.jp";

	//HostCN
	private string  kCNPFApiDomainSandbox = "sp.sb.mobage-platform.cn";
	private string  kCNPFApiDomainProduction = "sp.mobage-platform.cn";
	
	private string  kCNSPWebBaseHostSandbox = "sb.mobage.cn";
	private string  kCNSPWebBaseHostProduction = "mobage.cn";
	
	//HostTW
	private string  kTWPFApiDomainSandbox = "sp.sb.mobage-platform.tw";
	private string  kTWPFApiDomainProduction = "sp.mobage-platform.tw";
	
	private string  kTWSPWebBaseHostSandbox = "sb.mobage.tw";
	private string  kTWSPWebBaseHostProduction = "mobage.tw";
	
	private string	pfApiDomain_;
	private string  hostUrl;
	
	private string	spWebBaseUrl_;
	private string  browserLoginURL;
	
	private int  mRegion;
	private int  mServerMode;
	
	private  static HostConfig mInstance = null;
	public static HostConfig GetInstance()
	{
		if(mInstance == null)
		{
			mInstance = new HostConfig();
		}
		return mInstance;
	}
	
	/*!
	 * @Init url value
	 * @discussion 
	 * @param {int} serverMode.
	 * @param {int} region.
	 */
	public void Init(int serverMode, int region){
		mServerMode = serverMode;
		mRegion = region;
		switch(region)
		{
			//case 0:  SetURLUS(serverType);break;    //MBG_REGION.MBG_REGION_US
			case 1:  SetURLJP(serverMode);break;	  //MBG_REGION.MBG_REGION_JP
			case 2:  SetURLCN(serverMode);break;      //MBG_REGION.MBG_REGION_CN
			case 3:  SetURLTW(serverMode);break;      //MBG_REGION.MBG_REGION_TW
			default: break;
		}
	}

	/*!
	 * @Get Region
	 */
	public int REGION
	{
		get{return mRegion;}
	}
	
	/*!
	 * @Get ServerMode
	 */
	public int SERVER_MODE
	{
		get{return mServerMode;}
	}
	
	/*!
	 * @Get value of PFAPIDomain
	 */
	public string PFAPIDomain
	{
		get{return pfApiDomain_;}
	}
	
	/*!
	 * @Get base host url
	 */
	public string HostURL
	{
		get
		{ 
			return hostUrl;
		}
	}
	
	/*!
	 * @Get url for login in unity editor
	 */
	public string GetPFLoginURL()
	{
		string url = spWebBaseUrl_;
		url += "/_sdk_debug_auth";
		return url;
	}
	
	/*!
	 * @Get url for request in unity editor
	 */
	public string GetPFRequestURL(int flag)
	{
		string url = "http://";
		url += pfApiDomain_;
		if(flag == 0) url += "/social/api/jsonrpc/v2";
		else url += "/social/api/jsonrpc/v2/cn";
		return url;
	}

	/*!
	 * @URL for login in browser
	 */
	public string LoginURL
	{
		get
		{
			string url = browserLoginURL.Replace("https", "http");
			return mRegion == 2?url.Replace("ssl", "m"):url;//2 is MBG_REGION.MBG_REGION_CN
		}
		set{ browserLoginURL = value;}
	}
	
	/*!
	 * @Get base url for load in browser
	 */
	public string BASE_URL
	{
		get
		{ 
			string url = spWebBaseUrl_.Replace("https", "http");
			return mRegion == 2?url.Replace("ssl", "m"):url;//2 is MBG_REGION.MBG_REGION_CN
		}
	}
	
	
	/*!
	 * @Init  url for JP
	 */
	private  void SetURLJP(int serverMode)
	{
		
		switch(serverMode) {
		case 0://MBG_SERVER_TYPE.MBG_SANDBOX
			spWebBaseUrl_ = "http://";
			spWebBaseUrl_ += kJPSPWebBaseHostSandbox;
			hostUrl = kJPSPWebBaseHostSandbox;
			pfApiDomain_ = kJPPFApiDomainSandbox;
			break;
		case 1://MBG_SERVER_TYPE.MBG_PRODUCTION
			spWebBaseUrl_ = "http://";
			spWebBaseUrl_ += kJPSPWebBaseHostProduction;
			hostUrl = kJPSPWebBaseHostProduction;
			pfApiDomain_ = kJPPFApiDomainProduction;
			break;
		default:
			break;	
		}
	}
	
	/*!
	 * @Init  url for CN
	 */
	private void SetURLCN(int serverMode)
	{
		switch(serverMode) {
		case 0://MBG_SERVER_TYPE.MBG_SANDBOX
			spWebBaseUrl_ = "https://";
			spWebBaseUrl_ += kCNSPWebBaseHostSandbox;
			hostUrl = kCNSPWebBaseHostSandbox;
			pfApiDomain_ = kCNPFApiDomainSandbox;
			break;
		case 1://MBG_SERVER_TYPE.MBG_PRODUCTION
			spWebBaseUrl_ = "https://ssl.";
			spWebBaseUrl_ += kCNSPWebBaseHostProduction;
			hostUrl = kCNSPWebBaseHostProduction;
			pfApiDomain_ = kCNPFApiDomainProduction;
			break;
		default:
			break;
		}
	}
	
	/*!
	 * @Init  url for TW
	 */
	private void SetURLTW(int serverMode)
	{
		switch(serverMode) {
		case 0://MBG_SERVER_TYPE.MBG_SANDBOX
			spWebBaseUrl_ = "https://";
			spWebBaseUrl_ += kTWSPWebBaseHostSandbox;
			hostUrl = kTWSPWebBaseHostSandbox;
			pfApiDomain_ = kTWPFApiDomainSandbox;
			break;
		case 1://MBG_SERVER_TYPE.MBG_PRODUCTION
			spWebBaseUrl_ = "https://ssl.";
			spWebBaseUrl_ += kTWSPWebBaseHostProduction;
			hostUrl = kTWSPWebBaseHostProduction;
			pfApiDomain_ = kTWPFApiDomainProduction;
			break;
		default:
			break;
		}
	}
}








