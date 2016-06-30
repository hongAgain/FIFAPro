/**
 * MobageUnityInitialization
 */


using System.Collections;


/*!
 * @abstract Region Type
 */
public enum MBG_REGION {
	MBG_REGION_US, //unsupported for now
	MBG_REGION_JP, //unsupported for now 
	MBG_REGION_CN,
	MBG_REGION_TW
};

/*!
 * @abstract Server Type
 */
public enum MBG_SERVER_TYPE {
	MBG_SANDBOX,
	MBG_PRODUCTION
};


/*!
 * @abstract MarketCode Type
 */
public class MobageMarketCode {

	/*!
	 * @abstract MarketCode Type
	 */
	enum MarketCode { 
		MOBAGE, 
		GOOGLE_ANDROID_MARKET };

	/*!
	 * @abstract Get a MarketCode Type from MarketCode string
	 * @param value MarketCode character
	 */
	public static int fromString(string value) {
		if ((value == null) || (value.Length == 0)) {
			return (int)MarketCode.MOBAGE;
		}

		string v = value.ToLower();

		if(string.Compare("amkt", v)==0) {
			return (int)MarketCode.GOOGLE_ANDROID_MARKET;
		}else if(string.Compare("mbga", v)==0) {
			return (int)MarketCode.MOBAGE;
		}else {
			return (int)MarketCode.MOBAGE;
		}
	}

	/*!
	 * @abstract Get a MarketCode string from MarketCode
	 * @param value MarketCode type
	 */
	public string toString(int value) {
		switch(value) {
		case (int)MarketCode.MOBAGE:
			return "mbga";
		case (int)MarketCode.GOOGLE_ANDROID_MARKET:
			return "amkt";
		default:
			return "mbga";
		}
	}
}

/*!
 * @abstract Main class for controlling the mobage SDK. Includes:
 * <br /> - Login 
 * <br /> - Tick(asynchronous operation handle) control 
 */
public class MobagePlatform {


	/*!
	 * @abstract regist a Login Listener. 
	 *  This API is able to use at iOS side. At Android side, this function may use a NativeSDK API at JAVA.
	 */
	public static void addLoginListener(AddLoginCallBackLib.onLoginComplete onComp, 
	                                    AddLoginCallBackLib.onLoginRequired onRequired, 
	                                    AddLoginCallBackLib.onCancel onCancel, 
	                                    AddLoginCallBackLib.onError onError) {
		MobageManager.addLoginListener(onComp, onRequired, onCancel, onError);
	}

	//!!
	public static void addSwitchAccountListenner(SwitchAccountProxy.OnSwitchAccount onSwitchAccount)
	{
		MobageManager.addSwitchAccountListenner(onSwitchAccount);
	}


	public static void addLogoutListenner(LogoutListener.OnLogoutComplete onLogoutComplete){
		MobageManager.addLogoutListenner(onLogoutComplete);
	}


	/*!
	 * @abstract regist a  Listener for DashBoard 
	 *  This API is able to use at iOS side. At Android side, this function may use a NativeSDK API at JAVA.
	 */
	public static void addDashBoardListener(AddDashBoardCallBackLib.OnDis onDis) {
		MobageManager.addDashBoardListener(onDis);
	}

	/*!
	 * @abstract initializes the mobage platform sdk. 
	 *  This API is able to use at iOS side. At Android side, this function may use a NativeSDK API at JAVA.
	 */
	public static void Initialize (int region, int serverMode, string consumerKey, string consumerSecret, string appId) {
		MobageManager.initialize(region,  serverMode,  consumerKey,  consumerSecret,  appId);
		return;
	}
	

	/*!
	 * @abstract logout from mobage service. 
	 */
	public static void Logout() {
		MobageManager.Logout();
	}

	//这个方法需要在游戏退出时被调用.
	public static void QuitSDK()
	{
		MobageManager.QuitSDK ();
	}

	//!!
	public static void addXPromotionListenner(XPromotionListener.DidShow didShow, XPromotionListener.DidClose didClose)
	{
		MobageManager.addXPromotionListenner (didShow, didClose);
	}
}

