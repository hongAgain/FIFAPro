/*
 * This Class is used by PC and MAC StandAlone
 * for In-Editor function.
 *
 */
using UnityEngine;
using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Net;
using System.IO;
using System.Text;
using System.IO.IsolatedStorage;
using System.Security.Cryptography;
using System.Reflection;
using Common.Log;

public class SDKForIDEEditor 
{	
	    private static string TAG = "SDKForIDEEditor";
		public static void MobageaddLoginListener( ){
		    int region = HostConfig.GetInstance().REGION;
			int serverMode = HostConfig.GetInstance().SERVER_MODE;
			
			switch(region)
			{
				case (int)MBG_REGION.MBG_REGION_CN:
				{
			        if(serverMode == (int)MBG_SERVER_TYPE.MBG_SANDBOX)
					{
						//Please set your username and password
						SocialPFLogin.GetInstance().LoginWithInfo("zhiyuan.peng@dena.com", "19911118", EditorCallback.GetInstance().OnLoginComp);//SandBox
					}
					else if(serverMode == (int)MBG_SERVER_TYPE.MBG_PRODUCTION)
					{  
						//Please set your username and password
						SocialPFLogin.GetInstance().LoginWithInfo("15000571483", "zlsky123", EditorCallback.GetInstance().OnLoginComp);//Production
					}
					break;
				}
				case (int)MBG_REGION.MBG_REGION_TW:
				{
					//Please set your username and password
					SocialPFLogin.GetInstance().LoginWithInfo("15221254245", "test123456", EditorCallback.GetInstance().OnLoginComp);
					break;
				}
				default:MLog.e("HelloMobage", "Region is not support!");break;
			}
			return;
		}
	

	
		public static void MobageSocialRequestDispatcherGetCurrentUser(string keys){
			MLog.d(TAG, "keys:" + keys);
	    	//set post body and callback function to call api		    
			PeopleImpl.GetInstance().GetCurrentUser(keys, EditorCallback.GetInstance().OnGetCurrentUserComplete);
			return;
		}
	
		public static void MobageSocialRequestDispatcherGetFriends(string keys){
			MLog.d(TAG, "keys:" + keys);
		    //set post body and callback function to call api
			PeopleImpl.GetInstance().GetFriends(keys, EditorCallback.GetInstance().OnGetUsersComplete);
			return;
		}
	
		public static void MobageSocialRequestDispatcherGetFriendsWithGame(string keys){
			MLog.d(TAG, "keys:" + keys);
		 	//set post body and callback function to call api
			PeopleImpl.GetInstance().GetFriendsWithGame(keys, EditorCallback.GetInstance().OnGetUsersComplete);
			return;
		}
	
		public static void MobageSocialRequestDispatcherGetUser(string keys){
			MLog.d(TAG, "keys:" + keys);
			//set post body and callback function to call api
			PeopleImpl.GetInstance().GetUser(keys, EditorCallback.GetInstance().OnGetUserComplete);
			return;
		}
	
		public static void MobageSocialRequestDispatcherGetUsers(string keys){
			MLog.d(TAG, "keys:" + keys);
			//set post body and callback function to call api
			PeopleImpl.GetInstance().GetUsers(keys, EditorCallback.GetInstance().OnGetUsersComplete);
			return;
		}
	    
		public static void MobageSocialRequestDispatcherCheckBlackList(string keys){
			MLog.d(TAG, "keys:" + keys);
		    //set post body and callback function to call api
		    BlackListImpl.GetInstance().CheckBlackList(keys, EditorCallback.GetInstance().OnCheckBlacklistComplete);
			return;
		}

		public static void MobageSocialRequestDispatcheropenUserProfile(string keys){
			MLog.d(TAG, "keys:" + keys);
			ServiceImpl.GetInstance().OpenUserProfile(keys, EditorCallback.GetInstance().OnDialogComplete);
			return;
		}

		public static void MobageSocialRequestDispatchershowBankUI(){
			ServiceImpl.GetInstance().ShowBankUI(EditorCallback.GetInstance().OnDialogComplete);
			return;
		}
	
		public static void MobageSocialRequestDispatcheropenshowBalanceDialog(){
		    ServiceImpl.GetInstance().ShowBalanceDialog(EditorCallback.GetInstance().OnDialogComplete);
			return;
		}
	    
		public static void MobageSocialRequestDispatcherlaunchPortalApp(){
			EditorCallback.GetInstance().OnDialogComplete("");
			return;
		}
	
		public static void MobageSocialRequestDispatcherOpenHomePage(){
			ServiceImpl.GetInstance().OpenHomePage(EditorCallback.GetInstance().OnDialogComplete);
			return;
		}
	
		public static void MobageSocialRequestDispatcherAuth(string keys){
			MLog.d(TAG, "keys:" + keys);
			PTools.SetMLogDebug (true);
		    AuthImpl.GetInstance().AuthorizeToken(keys, EditorCallback.GetInstance().AuthorizeToken);
			return;
		}
	
		public static void MobageSocialRequestDispatcheropenDocument(string keys){
			MLog.d(TAG, "keys:" + keys);
			ServiceImpl.GetInstance().OpenDocument(keys, EditorCallback.GetInstance().OnDialogComplete);
			return;
		}
	
		public static void MobageSocialRequestDispatcherBankInventorygetItem(string keys){
			MLog.d(TAG, "keys:" + keys);
		    InventoryImpl.GetInstance().GetItem(keys, EditorCallback.GetInstance().GetItemComplete);
			return;
		}
	
		public static void MobageSocialRequestDispatchercreateTransaction(string keys){
			MLog.d(TAG, "keys:" + keys);
		    //set post body and callback function to call api
			DebitImpl.GetInstance().CreateTransaction(keys, EditorCallback.GetInstance().CreateTransactionWithDialogComplete);
			return;
		}
	
		public static void MobageSocialRequestDispatchercontinueTransaction(string keys){
			MLog.d(TAG, "keys:" + keys);
			 //set post body and callback function to call api
			DebitImpl.GetInstance().ContinueTransaction(keys, EditorCallback.GetInstance().ContinueTransactionWithDialogComplete);
			return;
		}
	
		public static void MobageSocialRequestDispatchercancelTransaction(string keys){
			MLog.d(TAG, "keys:" + keys);
			//set post body and callback function to call api
			DebitImpl.GetInstance().CancelTransaction(keys, EditorCallback.GetInstance().CancelTransactionComplete);
			return;
		}
	
		public static void MobageSocialRequestDispatchercloseTransaction(string keys){
			MLog.d(TAG, "keys:" + keys);
		    //set post body and callback function to call api
			DebitImpl.GetInstance().CloseTransaction(keys, EditorCallback.GetInstance().CloseTransactionComplete);
			return;
		}
	
		public static void MobageSocialRequestDispatchergetTransaction(string keys){
			MLog.d(TAG, "keys:" + keys);
		    //set post body and callback function to call api
		    DebitImpl.GetInstance().GetTransaction(keys, EditorCallback.GetInstance().GetTransactionComplete);
			return;
		}
	
		public static void MobageSocialRequestDispatchergetPendingTransactions(){
			 //set post body and callback function to call api
			DebitImpl.GetInstance().GetPendingTransactions(EditorCallback.GetInstance().GetPendingTransactionComplete);
			return;
		}
	
		public static void MobageSocialRequestDispatcheropenTransaction(string keys){
			MLog.d(TAG, "keys:" + keys);
		    //set post body and callback function to call api
			DebitImpl.GetInstance().OpenTransaction(keys, EditorCallback.GetInstance().OpenTransactionComplete);
			return;
		}
	
		public static void MobageSocialRequestDispatchergetBalance(){
			BalanceImpl.GetInstance().GetBalance(EditorCallback.GetInstance().OnGetBalanceComplete);
			return;
		}
	
		public static void Mobageinitialization(int region, 
												int serverMode, 
												string consumerKey, 
												string consumerSecret, 
												string appId){

            LogManager.Instance.Log("region:" + region + ", serverMode:" + serverMode + ", consumerKey:" + consumerKey + ", consumerSecret: " + consumerSecret + ", appId:" + appId);

			GlobalVar.GetInstance().AppID = appId;
			//Init Credential
			Credentials.GetInstance().setConsumerKey(consumerKey);
			Credentials.GetInstance().setConsumerSecret(consumerSecret);
			//Init Config Info
		    PFConfig.SetPFType(PFConfig.PFTYPE.ANDROID);
			//HostConfig.GetInstance().Init(serverMode, region);
		
			EditorCallback.GetInstance().SetCallback((MobageCallback)GameObject.FindObjectOfType(typeof(MobageCallback)));
			return;
		}

	
		public static void MobageshowLoginDialog( ){
	//		string response = "";

			return;
		}
	
		public static void MobagecheckLoginStatus( ){
	//		string response = "";
	
			return;
		}

		public static void Mobagetick( ){
	//		string response = "";
			return;
		}
	
		public static void MobageshowLogoutDialog(){
			MobageDialog.ShowDialog("Are you sure log out?", EditorCallback.GetInstance().LogoutComp);
			return;
		}

		public static void GetvcNameStr(){
		 	EditorCallback.GetInstance().GetvcNameStr("MobaCoin");
			return;
		}
	
		public static void GetMarketCode(){
		    EditorCallback.GetInstance().GetMarketCode("amkt");
			return;
		}
	
		public static void PushSend(string keys){
			MLog.d(TAG, "keys:"+keys);
		    NotificationImpl.GetInstance().pushSend(keys, EditorCallback.GetInstance().OnPushSendComplete);
			return;
		}
	
		public static void PushGetEnabled(){
		    NotificationImpl.GetInstance().getPushEnabled( EditorCallback.GetInstance().OnPushGetEnabledComplete);
			return;
		}
		
		public static void PushSetEnabled(string keys){
			MLog.d(TAG, "keys:"+keys);
		    NotificationImpl.GetInstance().setPushEnabled(keys, EditorCallback.GetInstance().OnPushSetEnabledComplete);
			return;
		}
		
		public static void HandleListener(){
		    NotificationImpl.GetInstance().handleListener(EditorCallback.GetInstance().OnHandleListenerComplete);
			return;
		}

	   
}



