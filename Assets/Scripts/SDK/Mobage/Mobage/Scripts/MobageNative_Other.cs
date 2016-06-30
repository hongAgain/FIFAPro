#if !UNITY_IPHONE && !UNITY_ANDROID
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
	public static bool SetOnce(string json){ return false;}
	public static bool SetRepeatingEveryDay(string json){return false;}
	public static bool SetRepeatingEveryDay2(string json){return false;}
	public static bool SetRepeatingDayOfWeek(string json){return false;}
	public static bool RemoveLocalNotification(string json){return false;}
	public static int RemoveAllNotification(){return 0;}

	public static void Logout(){
	}

	public static void SetMenubarVisibility(string keys)
	{
	}

	public static void SetMenubarPosition(string keys)
	{
	}

	public static void GetMobageVendorId()
	{
	}

	public static void setExtraData(String ext)
	{
	}
	
	public static bool HasCustomerServicePage()
	{
		return false;
	}

	public static void QuitSDK()
	{	
	}

	public static void AddLoginListener( ){
		SDKForIDEEditor.MobageaddLoginListener();
		return;
	}
	
	public static void SocialRequestDispatcherGetCurrentUser(string keys){
		SDKForIDEEditor.MobageSocialRequestDispatcherGetCurrentUser(keys);
		return;
	}
	
	public static void SocialRequestDispatcherGetFriends(string keys){
		SDKForIDEEditor.MobageSocialRequestDispatcherGetFriends(keys);
		return;
	}
	
	public static void SocialRequestDispatcherGetFriendsWithGame(string keys){
		SDKForIDEEditor.MobageSocialRequestDispatcherGetFriendsWithGame(keys);
		return;
	}
	
	public static void SocialRequestDispatcherGetUser(string keys){
		SDKForIDEEditor.MobageSocialRequestDispatcherGetUser(keys);
		return;
	}
	
	public static void SocialRequestDispatcherGetUsers(string keys){
		SDKForIDEEditor.MobageSocialRequestDispatcherGetUsers(keys);
		return;
	}
	
	public static void SocialRequestDispatcherCheckBlackList(string keys){
		SDKForIDEEditor.MobageSocialRequestDispatcherCheckBlackList(keys);
		return;
	}
	
	public static void SocialRequestDispatchershowBankUI(){
		SDKForIDEEditor.MobageSocialRequestDispatchershowBankUI();
		return;
	}
	
	public static void SocialRequestDispatcheropenshowBalanceDialog(){
		SDKForIDEEditor.MobageSocialRequestDispatcheropenshowBalanceDialog();
		return;
	}
	
	public static void LaunchDashboardWithHomePage(){
		SDKForIDEEditor.MobageSocialRequestDispatcherOpenHomePage();
		return;
	}
	
	public static void SocialRequestDispatcherAuth(string keys){
		SDKForIDEEditor.MobageSocialRequestDispatcherAuth(keys);
		return;
	}
	
	
	public static void SocialRequestDispatcherBankInventorygetItem(string keys){
		SDKForIDEEditor.MobageSocialRequestDispatcherBankInventorygetItem(keys);
		return;
	}
	
	public static void SocialRequestDispatchercreateTransaction(string keys){
		SDKForIDEEditor.MobageSocialRequestDispatchercreateTransaction(keys);
		return;
	}
	
	public static void SocialRequestDispatchercontinueTransaction(string keys){
		SDKForIDEEditor.MobageSocialRequestDispatchercontinueTransaction(keys);
		return;
	}
	
	public static void SocialRequestDispatchercancelTransaction(string keys){
		SDKForIDEEditor.MobageSocialRequestDispatchercancelTransaction(keys);
		return;
	}
	
	public static void SocialRequestDispatchercloseTransaction(string keys){
		SDKForIDEEditor.MobageSocialRequestDispatchercloseTransaction(keys);
		return;
	}
	
	public static void SocialRequestDispatchergetTransaction(string keys){
		SDKForIDEEditor.MobageSocialRequestDispatchergetTransaction(keys);
		return;
	}
	
	public static void SocialRequestDispatchergetPendingTransactions(){
		SDKForIDEEditor.MobageSocialRequestDispatchergetPendingTransactions();
		return;
	}
	
	public static void SocialRequestDispatcheropenTransaction(string keys){
		SDKForIDEEditor.MobageSocialRequestDispatcheropenTransaction(keys);
		return;
	}
	
	public static void SocialRequestDispatchergetBalance(){
		SDKForIDEEditor.MobageSocialRequestDispatchergetBalance();
		return;
	}
	
	public static void Initialization(int region, 
	                                        int serverMode, 
	                                        string consumerKey, 
	                                        string consumerSecret, 
	                                        string appId){
		SDKForIDEEditor.Mobageinitialization(region,serverMode,consumerKey,consumerSecret,appId);
		return;
	}
	
	public static void CheckLoginStatus( ){
		SDKForIDEEditor.MobagecheckLoginStatus();
		return;
	}
	
	public static void Tick( ){
		SDKForIDEEditor.Mobagetick();
		return;
	}
	
	public static void ShowLogoutDialog(){
		SDKForIDEEditor.MobageshowLogoutDialog();
		return;
	}
	
	public static void SocialRequestDispatcherPushSend(string keys){
		SDKForIDEEditor.PushSend(keys);
		return;
	}
	
	public static void SocialRequestDispatchergetPushEnabled( ){
		SDKForIDEEditor.PushGetEnabled();
		return;
	}
	
	public static void SocialRequestDispatchersetPushEnabled(string keys){
		SDKForIDEEditor.PushSetEnabled(keys);
		return;
	}
	
	public static void SocialRequestDispatcherNotificationListener( ){
		SDKForIDEEditor.HandleListener();
		return;
	}
	
	public static void GetvcNameStr(){	
		SDKForIDEEditor.GetvcNameStr();
		return;
	}
	
	public static void GetMarketCode(){
		SDKForIDEEditor.GetMarketCode();
		return;
	}
	
	//get server environment (sandbox : 0, production : 1)
	public static int GetServerModeFromConfig() {
		return (int)EditorConf.platformEnv;
	}
	
	public static void SocialRequestDispatcherLBSConfirmDialog() {
		return;
	}
	
	public static string GetAffcode()
	{
		return EditorConf.affcode;
	}

	public static void OpenCustomerServicePage()
	{

	}

	public static void OpenAccountRechargePage()
	{
	}

	public static bool HasForum()
	{
		return false;
	}

	public static void ShowForum()
	{
	
	}

	public static void GetFacebookUser()
	{
	}
}

#endif

