#if UNITY_IOS
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

	public static bool SetOnce(string json){return false;}
	public static bool SetRepeatingEveryDay(string json){ return false;}
	public static bool SetRepeatingDayOfWeek(string json){ return false;}
	public static bool RemoveLocalNotification(string json){ return false;}
	public static int RemoveAllNotification(){ return 0;}


	// Callback Function
	[DllImport ("__Internal")]
	public static extern void MobageLaunchDashboardWithHomePage();
	public static void LaunchDashboardWithHomePage()
	{
	#if UNITY_EDITOR
		SDKForIDEEditor.MobageSocialRequestDispatcherOpenHomePage();
	#else
		MobageNative.MobageLaunchDashboardWithHomePage();
	#endif
	}

	//only useed in editor
	public static void AddLoginListener()
	{
		#if UNITY_EDITOR
		SDKForIDEEditor.MobageaddLoginListener();
		#else
		
		#endif
	}

	public static void setExtraData(String ext)
	{
		#if UNITY_EDITOR
		
		#else

		#endif
	}

	public static void QuitSDK()
	{
	}

	[DllImport ("__Internal", CallingConvention = CallingConvention.Cdecl)]
	public static extern void MobageSocialRequestDispatcherGetCurrentUser(string keys);
	public static void SocialRequestDispatcherGetCurrentUser(string keys)
	{
	#if UNITY_EDITOR
			SDKForIDEEditor.MobageSocialRequestDispatcherGetCurrentUser(keys);
	#else
			MobageNative.MobageSocialRequestDispatcherGetCurrentUser (keys);
	#endif

	}

	[DllImport ("__Internal", CallingConvention = CallingConvention.Cdecl)]
	public static extern void MobageSocialRequestDispatcherGetFriends(string keys);
	public static void SocialRequestDispatcherGetFriends(string keys)
	{
	#if UNITY_EDITOR
			SDKForIDEEditor.MobageSocialRequestDispatcherGetFriends(keys);
	#else
			MobageNative.MobageSocialRequestDispatcherGetFriends (keys);
	#endif

	}

	[DllImport ("__Internal", CallingConvention = CallingConvention.Cdecl)]
	public static extern void MobageSocialRequestDispatcherGetFriendsWithGame(string keys);
	public static void SocialRequestDispatcherGetFriendsWithGame(string keys)
	{
	#if UNITY_EDITOR
			SDKForIDEEditor.MobageSocialRequestDispatcherGetFriendsWithGame(keys);
	#else
			MobageNative.MobageSocialRequestDispatcherGetFriendsWithGame (keys);
	#endif

	}

	[DllImport ("__Internal", CallingConvention = CallingConvention.Cdecl)]
	public static extern void MobageSocialRequestDispatcherGetUser(string keys);
	public static void SocialRequestDispatcherGetUser(string keys)
	{
	#if UNITY_EDITOR
			SDKForIDEEditor.MobageSocialRequestDispatcherGetUser(keys);
	#else
			MobageNative.MobageSocialRequestDispatcherGetUser (keys);
	#endif

	}

	[DllImport ("__Internal", CallingConvention = CallingConvention.Cdecl)]
	public static extern void MobageSocialRequestDispatcherGetUsers(string keys);
	public static void SocialRequestDispatcherGetUsers(string keys)
	{
	#if UNITY_EDITOR
			SDKForIDEEditor.MobageSocialRequestDispatcherGetUsers(keys);
	#else
			MobageNative.MobageSocialRequestDispatcherGetUsers (keys);
	#endif

	}

	[DllImport ("__Internal", CallingConvention = CallingConvention.Cdecl)]
	public static extern void MobageSocialRequestDispatcherCheckBlackList(string keys);
	public static void SocialRequestDispatcherCheckBlackList(string keys)
	{
	#if UNITY_EDITOR
			SDKForIDEEditor.MobageSocialRequestDispatcherCheckBlackList(keys);
	#else
			MobageNative.MobageSocialRequestDispatcherCheckBlackList(keys);
	#endif

	}

	[DllImport ("__Internal", CallingConvention = CallingConvention.Cdecl)]
	public static extern void MobageSocialRequestDispatchershowBankUI();
	public static void SocialRequestDispatchershowBankUI()
	{
	#if UNITY_EDITOR
			SDKForIDEEditor.MobageSocialRequestDispatchershowBankUI();
	#else
			MobageNative.MobageSocialRequestDispatchershowBankUI ();
	#endif

	}
	

	[DllImport ("__Internal", CallingConvention = CallingConvention.Cdecl)]
	public static extern void MobageSocialRequestDispatcherAuth(string keys);
	public static void SocialRequestDispatcherAuth(string keys)
	{
	#if UNITY_EDITOR
			SDKForIDEEditor.MobageSocialRequestDispatcherAuth(keys);
	#else
			MobageNative.MobageSocialRequestDispatcherAuth (keys);
	#endif

	}
	

	[DllImport ("__Internal", CallingConvention = CallingConvention.Cdecl)]
	public static extern void MobageSocialRequestDispatcherBankInventorygetItem(string keys);
	public static void SocialRequestDispatcherBankInventorygetItem(string keys)
	{
	#if UNITY_EDITOR
			SDKForIDEEditor.MobageSocialRequestDispatcherBankInventorygetItem(keys);
	#else
			MobageNative.MobageSocialRequestDispatcherBankInventorygetItem (keys);
	#endif

	}


	[DllImport ("__Internal", CallingConvention = CallingConvention.Cdecl)]
	public static extern void MobageSocialRequestDispatchercreateTransaction(string keys);
	public static void SocialRequestDispatchercreateTransaction(string keys)
	{
	#if UNITY_EDITOR
			SDKForIDEEditor.MobageSocialRequestDispatchercreateTransaction(keys);
	#else
			MobageNative.MobageSocialRequestDispatchercreateTransaction (keys);
	#endif

	}

	[DllImport ("__Internal", CallingConvention = CallingConvention.Cdecl)]
	public static extern void MobageSocialRequestDispatchercontinueTransaction(string keys);
	public static void SocialRequestDispatchercontinueTransaction(string keys)
	{
	#if UNITY_EDITOR
			SDKForIDEEditor.MobageSocialRequestDispatchercontinueTransaction(keys);
	#else
			MobageNative.MobageSocialRequestDispatchercontinueTransaction (keys);
	#endif

	}

	[DllImport ("__Internal", CallingConvention = CallingConvention.Cdecl)]
	public static extern void MobageSocialRequestDispatchercancelTransaction(string keys);
	public static void SocialRequestDispatchercancelTransaction(string keys)
	{
	#if UNITY_EDITOR
			SDKForIDEEditor.MobageSocialRequestDispatchercancelTransaction(keys);
	#else
			MobageNative.MobageSocialRequestDispatchercancelTransaction (keys);
	#endif

	}

	[DllImport ("__Internal", CallingConvention = CallingConvention.Cdecl)]
	public static extern void MobageSocialRequestDispatchercloseTransaction(string keys);
	public static void SocialRequestDispatchercloseTransaction(string keys)
	{
	#if UNITY_EDITOR
			SDKForIDEEditor.MobageSocialRequestDispatchercloseTransaction(keys);
	#else
			MobageNative.MobageSocialRequestDispatchercloseTransaction (keys);
	#endif

	}

	[DllImport ("__Internal", CallingConvention = CallingConvention.Cdecl)]
	public static extern void MobageSocialRequestDispatchergetTransaction(string keys);
	public static void SocialRequestDispatchergetTransaction(string keys)
	{
	#if UNITY_EDITOR
			SDKForIDEEditor.MobageSocialRequestDispatchergetTransaction(keys);
	#else
			MobageNative.MobageSocialRequestDispatchergetTransaction (keys);
	#endif

	}

	[DllImport ("__Internal", CallingConvention = CallingConvention.Cdecl)]
	public static extern void MobageSocialRequestDispatchergetPendingTransactions();
	public static void SocialRequestDispatchergetPendingTransactions()
	{
	#if UNITY_EDITOR
			SDKForIDEEditor.MobageSocialRequestDispatchergetPendingTransactions();
	#else
			MobageNative.MobageSocialRequestDispatchergetPendingTransactions ();
	#endif

	}

	[DllImport ("__Internal", CallingConvention = CallingConvention.Cdecl)]
	public static extern void MobageSocialRequestDispatcheropenTransaction(string keys);
	public static void SocialRequestDispatcheropenTransaction(string keys)
	{
	#if UNITY_EDITOR
			SDKForIDEEditor.MobageSocialRequestDispatcheropenTransaction(keys);
	#else
			MobageNative.MobageSocialRequestDispatcheropenTransaction (keys);
	#endif

	}

	[DllImport ("__Internal", CallingConvention = CallingConvention.Cdecl)]
	public static extern void MobageSocialRequestDispatchergetBalance();
	public static void SocialRequestDispatchergetBalance()
	{
	#if UNITY_EDITOR
			SDKForIDEEditor.MobageSocialRequestDispatchergetBalance();
	#else
			MobageNative.MobageSocialRequestDispatchergetBalance ();
	#endif

	}
	
	
	[DllImport ("__Internal", CallingConvention = CallingConvention.Cdecl)]
	public static extern void MobageSocialRequestDispatcherPushSend(string keys);
	public static void SocialRequestDispatcherPushSend(string keys)
	{
	#if UNITY_EDITOR
			SDKForIDEEditor.PushSend(keys);
	#else
			MobageNative.MobageSocialRequestDispatcherPushSend (keys);
	#endif

	}
	
	[DllImport ("__Internal", CallingConvention = CallingConvention.Cdecl)]
	public static extern void MobageSocialRequestDispatchergetPushEnabled();
	public static void SocialRequestDispatchergetPushEnabled()
	{
	#if UNITY_EDITOR
			SDKForIDEEditor.PushGetEnabled();
	#else
			MobageNative.MobageSocialRequestDispatchergetPushEnabled ();
	#endif

	}


	[DllImport ("__Internal", CallingConvention = CallingConvention.Cdecl)]
	public static extern void MobageSocialRequestDispatchersetPushEnabled(string keys);
	public static void SocialRequestDispatchersetPushEnabled(string keys)
	{
	#if UNITY_EDITOR
			SDKForIDEEditor.PushSetEnabled(keys);
	#else
			MobageNative.MobageSocialRequestDispatchersetPushEnabled (keys);
	#endif

	}


	[DllImport ("__Internal", CallingConvention = CallingConvention.Cdecl)]
	public static extern void MobageSocialRequestDispatcherNotificationListener();
	public static void SocialRequestDispatcherNotificationListener()
	{
	#if UNITY_EDITOR
			SDKForIDEEditor.HandleListener();
	#else
			MobageNative.MobageSocialRequestDispatcherNotificationListener ();
	#endif

	}
	
	// Native request
	[DllImport ("__Internal", CallingConvention = CallingConvention.Cdecl)]
	public static extern void Mobageinitialization( int region, int serverMode, string consumerKey, string consumerSecret, string appId);
	public static void Initialization( int region, int serverMode, string consumerKey, string consumerSecret, string appId)
	{
	#if UNITY_EDITOR
			SDKForIDEEditor.Mobageinitialization(region, serverMode, consumerKey, consumerSecret, appId);
	#else
			MobageNative.Mobageinitialization (region, serverMode, consumerKey, consumerSecret, appId);
	#endif

	}


	[DllImport ("__Internal", CallingConvention = CallingConvention.Cdecl)]
	public static extern void MobagecheckLoginStatus();
	public static void CheckLoginStatus()
	{
	#if UNITY_EDITOR
			SDKForIDEEditor.MobagecheckLoginStatus();
	#else
			MobageNative.MobagecheckLoginStatus();
	#endif

	}

	[DllImport ("__Internal", CallingConvention = CallingConvention.Cdecl)]
	public static extern void Mobagetick();
	public static void Tick()
	{
	#if UNITY_EDITOR
			SDKForIDEEditor.Mobagetick();
	#else
			MobageNative.Mobagetick();
	#endif

	}

	[DllImport ("__Internal", CallingConvention = CallingConvention.Cdecl)]	
	public static extern void MobageshowLogoutDialog();
	public static void ShowLogoutDialog()
	{
	#if UNITY_EDITOR
			SDKForIDEEditor.MobageshowLogoutDialog();
	#else
			MobageNative.MobageshowLogoutDialog();
	#endif

	}

	public static void Logout()
	{
		ShowLogoutDialog();
		
	}
	
	// Blocking Data API
	[DllImport ("__Internal", CallingConvention = CallingConvention.Cdecl)]	
	public static extern void MobageGetvcNameStr();
	public static void GetvcNameStr()
	{
	#if UNITY_EDITOR
			SDKForIDEEditor.GetvcNameStr();
	#else
			MobageNative.MobageGetvcNameStr();
	#endif

	}
	
	//CN
	
	//menubar
	[DllImport ("__Internal", CallingConvention = CallingConvention.Cdecl)]	
	public static extern void MobageSetMenubarVisibility(string keys);
	public static void SetMenubarVisibility(string keys)
	{
	#if UNITY_EDITOR
			//no menubar in editor
	#else
			MobageNative.MobageSetMenubarVisibility (keys);
	#endif

	}

	[DllImport ("__Internal", CallingConvention = CallingConvention.Cdecl)]	
	public static extern void MobageSetMenubarPosition(string keys);
	public static void SetMenubarPosition(string keys)
	{
	#if UNITY_EDITOR
			//no menubar in editor
	#else
			MobageNative.MobageSetMenubarPosition (keys);
	#endif

	}

	//get server environment (sandbox : 0, production : 1)
	[DllImport ("__Internal", CallingConvention = CallingConvention.Cdecl)]	
	public static extern int MobagegetServerModeFromConfig();
	public static int GetServerModeFromConfig()
	{
	#if UNITY_EDITOR
			return (int)EditorConf.platformEnv;
	#else
			return MobageNative.MobagegetServerModeFromConfig ();
	#endif

	}
	
	//GetMobageVendorId
	[DllImport ("__Internal", CallingConvention = CallingConvention.Cdecl)]
	public static extern void MobageGetMobageVendorId();
	public static void GetMobageVendorId()
	{
	#if UNITY_EDITOR
			//not implement in editor
	#else
			MobageNative.MobageGetMobageVendorId ();
	#endif

	}
	
	[DllImport ("__Internal", CallingConvention = CallingConvention.Cdecl)]
	public static extern string MobageGetAffcode();
	public static string GetAffcode()
	{
		#if UNITY_EDITOR
		//not implement in editor
		return EditorConf.affcode;
		#else
		return MobageNative.MobageGetAffcode ();
		#endif
		
	}


	[DllImport ("__Internal", CallingConvention = CallingConvention.Cdecl)]
	public static extern string MobageOpenCustomerServicePage();
	public static void OpenCustomerServicePage()
	{
		#if UNITY_EDITOR
		//not imlement in editor
		#else
		//not used
		//MobageNative.MobageOpenCustomerServicePage ();
		#endif
		
	}


	[DllImport ("__Internal", CallingConvention = CallingConvention.Cdecl)]
	public static extern string MobageOpenChargePage();
	public static void OpenAccountRechargePage()
	{
		#if UNITY_EDITOR
		//not implement in editor
		#else
		//not used
		//MobageNative.MobageOpenChargePage ();
		#endif
		
	}


	public static bool HasCustomerServicePage()
	{
		#if UNITY_EDITOR
		return false;
		#else
		return false;
		#endif
	}

	[DllImport ("__Internal", CallingConvention = CallingConvention.Cdecl)]
	public static extern bool MobageHasForum();
	public static bool HasForum()
	{
		#if UNITY_EDITOR
		return false;
		#else
		return MobageNative.MobageHasForum();
		#endif
	}

	[DllImport ("__Internal", CallingConvention = CallingConvention.Cdecl)]
	public static extern string MobageShowForum();
	public static void ShowForum()
	{
		#if UNITY_EDITOR
		return;
		#else
		MobageNative.MobageShowForum();
		#endif
	}

	[DllImport ("__Internal", CallingConvention = CallingConvention.Cdecl)]
	public static extern string MobageGetFacebookUser();
	public static void GetFacebookUser()
	{
		#if UNITY_EDITOR
		return;
		#else
		MobageNative.MobageGetFacebookUser();
		#endif
	}

}

#endif
