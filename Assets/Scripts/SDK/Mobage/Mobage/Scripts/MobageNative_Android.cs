#if UNITY_ANDROID 
using UnityEngine;
using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Runtime.InteropServices;
using System.Threading;
using MobageLitJson;

public partial class MobageNative
{

	public static AndroidJavaClass native { get; set;}
	
	static MobageNative()
	{
		#if UNITY_EDITOR

		#else
		if(MobageNative.native == null)
		{
			MobageNative.native = new AndroidJavaClass ("com.mobage.android.unity3d.UnityProxy");
		}
		#endif
	}
	//------------------------------------------
	public static bool SetOnce(string json){
		#if UNITY_EDITOR
		return true;
		#else
		return MobageNative.native.CallStatic<bool>("setOnce",json );
		#endif
	}

	public static bool SetRepeatingEveryDay(string json){
		#if UNITY_EDITOR
		return true;
		#else
		return MobageNative.native.CallStatic<bool>("setRepeatingEveryDay",json );
		#endif
	}

	public static bool SetRepeatingEveryDay2(string json){
		#if UNITY_EDITOR
		return true;
		#else
		return MobageNative.native.CallStatic<bool>("setRepeatingEveryDay2",json );
		#endif
	}

	public static bool SetRepeatingDayOfWeek(string json){
		#if UNITY_EDITOR
		return true;
		#else
		return MobageNative.native.CallStatic<bool>("setRepeatingDayOfWeek",json );
		#endif
	}

	public static bool RemoveLocalNotification(string json){
		#if UNITY_EDITOR
		return true;
		#else
		return MobageNative.native.CallStatic<bool>("removeLocalNotification",json );
		#endif
	}

	public static int RemoveAllNotification(){
		#if UNITY_EDITOR
		return 1;
		#else
		return MobageNative.native.CallStatic<int>("removeAllNotification");
		#endif
	}

	//---------------------------------------------
	public static bool HasCustomerServicePage()
	{
		#if UNITY_EDITOR
		return false;
		#else
		return MobageNative.native.CallStatic<bool>("hasCustomerService");
		#endif
	}

	public static bool HasForum()
	{
		#if UNITY_EDITOR
		return false;
		#else
		return MobageNative.native.CallStatic<bool>("hasForum");
		#endif
	}

	public static void ShowForum()
	{
		#if UNITY_EDITOR
		return;
		#else
		MobageNative.native.CallStatic("showForum");
		#endif
	}

	public static void setExtraData(String ext)
	{
		#if UNITY_EDITOR

		#else
		MobageNative.native.CallStatic("setExtraData", ext);
		#endif
	}

	public static void QuitSDK()
	{
		#if UNITY_EDITOR
		
		#else
			MobageNative.native.CallStatic("quitSdk");
		#endif
	}

	//1.3.9 new
	public static void TestPhotoUpload()
	{
		#if UNITY_EDITOR

		#else
			MobageNative.native.CallStatic("testPhotoUpload");
		#endif
	}

	public static void LaunchDashboardWithHomePage()
	{
		#if UNITY_EDITOR
			SDKForIDEEditor.MobageSocialRequestDispatcherOpenHomePage();
		#else
			MobageNative.native.CallStatic("launchDashboardWithHomePage");
		#endif
	}

	public static void SocialRequestDispatcherGetCurrentUser(string keys)
	{
		#if UNITY_EDITOR
			SDKForIDEEditor.MobageSocialRequestDispatcherGetCurrentUser (keys);
		#else
			MobageNative.native.CallStatic ("GetCurrentUser", keys);
		#endif
	}
	
	public static void SocialRequestDispatcherGetFriends(string keys)
	{
		#if UNITY_EDITOR
			SDKForIDEEditor.MobageSocialRequestDispatcherGetFriends (keys);
		#else
			MobageNative.native.CallStatic("GetFriends",keys);
		#endif
	}
	public static void SocialRequestDispatcherGetFriendsWithGame(string keys)
	{
		#if UNITY_EDITOR
		SDKForIDEEditor.MobageSocialRequestDispatcherGetFriendsWithGame(keys);
		#else
			MobageNative.native.CallStatic("GetFriendsWithGame",keys);
		#endif

	}
	public static void SocialRequestDispatcherGetUser(string keys)
	{
		#if UNITY_EDITOR
			SDKForIDEEditor.MobageSocialRequestDispatcherGetUser(keys);
		#else
			MobageNative.native.CallStatic("GetUser",keys);
		#endif

	}	
	public static void SocialRequestDispatcherGetUsers(string keys)
	{
		#if UNITY_EDITOR
		SDKForIDEEditor.MobageSocialRequestDispatcherGetUsers(keys);
		#else
			MobageNative.native.CallStatic("GetUsers",keys);
		#endif

	}
	public static void SocialRequestDispatcherCheckBlackList(string keys)
	{
		#if UNITY_EDITOR
		SDKForIDEEditor.MobageSocialRequestDispatcherCheckBlackList(keys);
		#else
			MobageNative.native.CallStatic("CheckBlacklist",keys);
		#endif

	}
	
	public static void SocialRequestDispatchershowBankUI()
	{
		#if UNITY_EDITOR
		SDKForIDEEditor.MobageSocialRequestDispatchershowBankUI();
		#else
			MobageNative.native.CallStatic("showBankUi");
		#endif

	}

	public static void SocialRequestDispatcherAuth(string keys)
	{
		#if UNITY_EDITOR
		SDKForIDEEditor.MobageSocialRequestDispatcherAuth(keys);
		#else
			MobageNative.native.CallStatic("AuthorizeToken",keys);
		#endif

	}
	public static void MobageSocialRequestDispatcheropenDocument(string keys)
	{
		#if UNITY_EDITOR
		SDKForIDEEditor.MobageSocialRequestDispatcheropenDocument(keys);
		#else
			MobageNative.native.CallStatic("openDocument",keys);
		#endif

	}
	public static void SocialRequestDispatcherBankInventorygetItem(string keys)
	{
		#if UNITY_EDITOR
		SDKForIDEEditor.MobageSocialRequestDispatcherBankInventorygetItem(keys);
		#else
			MobageNative.native.CallStatic("GetItem",keys);
		#endif

	}
	public static void SocialRequestDispatchercreateTransaction(string keys)
	{
		#if UNITY_EDITOR
			SDKForIDEEditor.MobageSocialRequestDispatchercreateTransaction(keys);
		#else
			MobageNative.native.CallStatic("CreateTransaction",keys);
		#endif

	}
	public static void SocialRequestDispatchercontinueTransaction(string keys)
	{
		#if UNITY_EDITOR
			SDKForIDEEditor.MobageSocialRequestDispatchercontinueTransaction(keys);
		#else
			MobageNative.native.CallStatic("ContinueTransaction",keys);
		#endif

	}
	public static void SocialRequestDispatchercancelTransaction(string keys)
	{
		#if UNITY_EDITOR
			SDKForIDEEditor.MobageSocialRequestDispatchercancelTransaction(keys);
		#else
			MobageNative.native.CallStatic("CancelTransaction",keys);
		#endif

	}
	public static void SocialRequestDispatchercloseTransaction(string keys)
	{
		#if UNITY_EDITOR
			SDKForIDEEditor.MobageSocialRequestDispatchercloseTransaction(keys);
		#else
			MobageNative.native.CallStatic("CloseTransaction",keys);
		#endif

	}	
	public static void SocialRequestDispatchergetTransaction(string keys)
	{
		#if UNITY_EDITOR
			SDKForIDEEditor.MobageSocialRequestDispatchergetTransaction(keys);
		#else
			MobageNative.native.CallStatic("GetTransaction",keys);
		#endif

	}
	public static void SocialRequestDispatchergetPendingTransactions()
	{
		#if UNITY_EDITOR
			SDKForIDEEditor.MobageSocialRequestDispatchergetPendingTransactions();
		#else
			MobageNative.native.CallStatic("GetPendingTransaction");
		#endif
	}
	public static void SocialRequestDispatcheropenTransaction(string keys)
	{
		#if UNITY_EDITOR
			SDKForIDEEditor.MobageSocialRequestDispatcheropenTransaction(keys);
		#else
			MobageNative.native.CallStatic("OpenTransaction",keys);
		#endif

	}
	public static void SocialRequestDispatchergetBalance()
	{
		#if UNITY_EDITOR
			SDKForIDEEditor.MobageSocialRequestDispatchergetBalance();
		#else
			MobageNative.native.CallStatic("getBalance");
		#endif

	}
	
	public static void SocialRequestDispatcherPushSend(string keys)
	{
		#if UNITY_EDITOR
			SDKForIDEEditor.PushSend(keys);
		#else
			MobageNative.native.CallStatic("send",keys);
		#endif

	}
	public static void SocialRequestDispatchergetPushEnabled( )
	{
		#if UNITY_EDITOR
			SDKForIDEEditor.PushGetEnabled();
		#else
			MobageNative.native.CallStatic("getRemoteNotificationsEnabled");
		#endif

	}
	public static void SocialRequestDispatchersetPushEnabled(string keys)
	{
		#if UNITY_EDITOR
			SDKForIDEEditor.PushSetEnabled(keys);
		#else
			MobageNative.native.CallStatic("setRemoteNotificationsEnabled",keys);
		#endif

	}
	
	// not used on Android ture machione
	public static void Initialization(int region, int serverMode, string consumerKey, string consumerSecret, string appId)
	{
		#if UNITY_EDITOR
			SDKForIDEEditor.Mobageinitialization(region, serverMode, consumerKey, consumerSecret, appId);
		#else

		#endif
	}

	// only used in Andorid ture machione
	public static void Initialization(string keys)
	{
		native.CallStatic("initialization", keys);
	}

	public static void MobageshowLoginDialog()
	{
		#if UNITY_EDITOR
			SDKForIDEEditor.MobageshowLoginDialog();
		#else
			MobageNative.native.CallStatic("showLoginDialog");
		#endif

	}
	public static void Tick()
	{
		#if UNITY_EDITOR
			SDKForIDEEditor.Mobagetick();
		#else
			//MobageNative.native.CallStatic("tick");
		#endif

	}

	//not work in IDE
	public static void Logout()
	{
		#if UNITY_EDITOR
			SDKForIDEEditor.MobageshowLogoutDialog();
		#else
			MobageNative.native.CallStatic("Logout");
		#endif
			
	}
	// do not exist in cpp file, and is not called by unity
	public static void CheckLoginStatus()
	{
		#if UNITY_EDITOR
			SDKForIDEEditor.MobagecheckLoginStatus();
		#else
			MobageNative.native.CallStatic("checkLoginStatus");
		#endif

	}
	public static void AddLoginListener()
	{
		#if UNITY_EDITOR
			SDKForIDEEditor.MobageaddLoginListener();
		#else
			
		#endif
		//MobageManager.native.CallStatic("LoginListener");
	}
	//CN
	
	//menubar
	public static void SetMenubarVisibility(string keys)
	{
		#if UNITY_EDITOR
			//no menu bar in other implement
		#else
			MobageNative.native.CallStatic ("setMobageToolBarVisibility",keys);
		#endif

	}
	public static void SetMenubarPosition(string keys)
	{
		#if UNITY_EDITOR
			//no menu bar in other implement
		#else
			MobageNative.native.CallStatic ("setMobageToolBarLocation",keys);
		#endif

	}

	//GetMobageVendorId
	public static void GetMobageVendorId()
	{
		#if UNITY_EDITOR
			//not implement in other platform
		#else
			MobageNative.native.CallStatic("getMobageVendorId");
		#endif

	}
	
	public static void  SetLocalNotificationOnce(string keys) 
	{
		#if UNITY_EDITOR
			//not implement in other platform
		#else
			MobageNative.native.CallStatic("setLocalNotificationOnce",keys);
		#endif

	}
	
	public static void SetLocalNotificationRepeatingDayofWeek(string keys)
	{
		#if UNITY_EDITOR
			//not implement in other platform
		#else
			MobageNative.native.CallStatic("setLocalNotificationRepeatingDayofWeek",keys);
		#endif

	}
	public static void SetLocalNotificationRepeatingEveryDay(string keys)
	{
		#if UNITY_EDITOR
			//not implement in other platform
		#else
			MobageNative.native.CallStatic("setLocalNotificationRepeatingEveryDay",keys);
		#endif

	}
	/*(
	public static void RemoveLocalNotification(string keys)
	{
		#if UNITY_EDITOR
				//not implement in other platform
		#else
			MobageNative.native.CallStatic("removeLocalNotification",keys);
		#endif

	}*/
	public static void RemoveAllLocalNotification()
	{
		#if UNITY_EDITOR
			//not implement in other platform
		#else
			MobageNative.native.CallStatic("removeAllLocalNotification");
		#endif

	}

	//get server environment (sandbox : 0, production : 1)
	public static int GetServerModeFromConfig() {	
		#if UNITY_EDITOR
			return (int)EditorConf.platformEnv;
		#else
			return MobageNative.native.CallStatic<int>("GetServerModeFromConfig");
		#endif

	}

	public static String GetAffcode(){
		#if UNITY_EDITOR
		//always be sandbox in editor
		return EditorConf.affcode;
		#else
		return MobageNative.native.CallStatic<String>("getAffcode");
		#endif
	}

	public static void OpenAccountRechargePage(){
		#if UNITY_EDITOR

		#else
		MobageNative.native.CallStatic("openAccountRechargePage");
		#endif
	}

	public static void OpenCustomerServicePage(){
		#if UNITY_EDITOR

		#else
		MobageNative.native.CallStatic("openCustomerServicePage");
		#endif
	}

	public static void GetFacebookUser(){
		#if UNITY_EDITOR
		
		#else
		MobageNative.native.CallStatic("getFacebookUser");
		#endif
	}
}

#endif
