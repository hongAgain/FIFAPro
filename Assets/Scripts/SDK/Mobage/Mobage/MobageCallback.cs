/*
 *  MobageUnityCallback.cs
 * 
 *  sample application for MobageUnitySDK iOS version
 *  This will be used for callback method.
 * 
 */

using UnityEngine;
using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Runtime.InteropServices;
using MobageLitJson;

public class MobageCallback : MonoBehaviour ,IMobageCallback{

	// Login Listener
	private string TAG = "MobageCallback";
    public void addLoginListenerComp(string message) {
		MLog.i(TAG, "addLoginListenerComp:" + message);
		MobageCallbackManager.loginLib.GetonComp(message);
		return;
	}

    public void addLoginListenerRequied(string message) {
		MLog.i(TAG, "addLoginListenerRequied:" + message);
		MobageCallbackManager.loginLib.GetonReqed(message);
		return;
	}

    public void addLoginListenerError(string message) {
		MLog.i(TAG, "addLoginListenerError:" + message);
		MobageCallbackManager.loginLib.GetonError(message);
		return;
	}

    public void addLoginListenerCancel(string message) {
		MLog.i(TAG, "addLoginListenerCancel:" + message);
		MobageCallbackManager.loginLib.GetonCancel(message);
		return;
	}

	public void onLogoutProxyLogoutComplete(string param){
		MLog.i(TAG, "onLogoutProxyLogoutComplete:" + param);
		MobageCallbackManager.logoutListenner.onNativeLogoutComplete ();
	}

	// Logout
    public void LogoutComp(string message) {
		MLog.i(TAG, "LogoutComp:" + message);
		//MobageCallbackManager.logoutLib.GetonSuccess(message);
		return;
	}

    public void LogoutCancel(string message) {
		MLog.i(TAG, "LogoutCancel:" + message);
		//MobageCallbackManager.logoutLib.GetonCancel(message);
		return;
	}

	//!!
	//switchAccount
	public void SwitchAccount(string message) {
		MLog.i(TAG, "addLoginListenerComp:" + message);
		SwitchAccountProxy.onNativeSwitchAccount (message);
		return;
	}

	//XPromotion
	public void XPromotionDidShow(string message) {
		MLog.i(TAG, "XPromotionDidShow:" + message);
		XPromotionListener.onNative_DidShow ();
		return;
	}

	public void XPromotionDidClose(string message) {
		MLog.i(TAG, "XPromotionDidClose:" + message);
		XPromotionListener.onNative_DidClose ();
		return;
	}

	public void XPromotionDidClick(string message) {
		MLog.i(TAG, "XPromotionDidClick:" + message);
		XPromotionListener.onNative_DidClick ();
		return;
	}


	// People
    public void OnGetUserCompleteSuccess(string message) {
		MLog.i(TAG, "OnGetUserCompleteSuccess:" + message);
		Proxy.GetUser.onNativeSuccess (message);
	}

    public void OnGetUserCompleteError(string message) {
		MLog.i(TAG, "OnGetUserCompleteError:" + message);
		Proxy.GetUser.onNativeSuccess (message);
	}

	public void OnGetCurrentUserSuccess(string message) {
		MLog.i(TAG, "OnGetCurrentUserSuccess:" + message);
		Proxy.GetCurrentUser.onNativeSuccess (message);
	}

	public void OnGetCurrentUserError(string message) {
		MLog.i(TAG, "OnGetCurrentUserError:" + message);
		Proxy.GetCurrentUser.onNativeError (message);
	}

    public void OnGetUsersCompleteSuccess(string message) {
		MLog.i(TAG, "OnGetUsersCompleteSuccess:" + message);
		MobageCallbackManager.usrsLib.GetonSuccess(message);
		return;
	}
	
    public void OnGetUsersCompleteError(string message) {
		MLog.i(TAG, "OnGetUsersCompleteError:" + message);
		MobageCallbackManager.usrsLib.GetonError(message);
		return;
	}
		
	// BlackList
    public void OnCheckBlacklistCompleteSuccess(string message) {
	    MLog.i(TAG, "OnCheckBlacklistCompleteSuccess:" + message);
		MobageCallbackManager.blLib.GetonSuccess(message);
		return;
	}
	
    public void OnCheckBlacklistCompleteError(string message) {
	    MLog.i(TAG, "OnCheckBlacklistCompleteError:" + message);
		MobageCallbackManager.blLib.GetonError(message);
		return;
	}

	public void  OnDialogComplete(string message) {
		PTools.SetMLogDebug (true);
	    MLog.i(TAG, "OnDialogComplete:" + message);
		MobageCallbackManager.dlgdisLib.GetonSuccess();			
		return;
	}
	
	public void  OnDashBoardComplete(string message) {
	    MLog.i(TAG, "OnDashBoardComplete:" + message);
		MobageCallbackManager.dashBoardLib.GetonDismiss(message);			
		return;
	}

	// Auth
    public void AuthorizeTokenSuccess(string message) {
		MLog.i(TAG, "AuthorizeTokenSuccess:" + message);
		MobageCallbackManager.authLib.GetonSuccess(message);
		return;
	}
	
    public void AuthorizeTokenError(string message) {
		MLog.i(TAG, "AuthorizeTokenError:" + message);
		MobageCallbackManager.authLib.GetonError(message);
		return;
	}
	
	// Bank
	public void TransactionWithDialogCompleteSuccess(string message) {
		MLog.i(TAG, "TransactionWithDialogCompleteSuccess:" + message);
		MobageCallbackManager.bnktrncmpLib.GetonSuccess(message);
		return;
	}
	
	public void TransactionWithDialogCompleteError(string message) {
		MLog.i(TAG, "TransactionWithDialogCompleteError:" + message);
		MobageCallbackManager.bnktrncmpLib.GetonError(message);
		return;
	}
	
	public void TransactionWithDialogCompleteCancel(string message) {
		MLog.i(TAG, "TransactionWithDialogCompleteCancel:" + message);
		MobageCallbackManager.bnktrncmpLib.GetonCancel(message);
		return;
	}
		
	public void TransactionCompleteSuccess(string message) {
		MLog.i(TAG, "TransactionCompleteSuccess:" + message);
		MobageCallbackManager.bnktrnLib.GetonSuccess(message);
		return;
	}
	
	public void TransactionCompleteError(string message) {
		MLog.i(TAG, "TransactionCompleteError:" + message);
		MobageCallbackManager.bnktrnLib.GetonError(message);
		return;
	}
			
	public void GetItemCompleteSuccess(string message) {
		MLog.i(TAG, "GetItemCompleteSuccess:" + message);
		MobageCallbackManager.bnkinvLib.GetonSuccess(message);
		return;
	}
	
	public void GetItemCompleteError(string message) {
		MLog.i(TAG, "GetItemCompleteError:" + message);
		MobageCallbackManager.bnkinvLib.GetonError(message);
		return;
	}
			
	public void  OnGetBalanceCompleteSuccess(string message) {
		MLog.i(TAG, "OnGetBalanceCompleteSuccess:" + message);
		MobageCallbackManager.balancebtnLib.GetonSuccess(message);
		return;
	}

	public void  OnGetBalanceCompleteError(string message) {
		MLog.i(TAG, "OnGetBalanceCompleteError:" + message);
		MobageCallbackManager.balancebtnLib.GetonError(message);
		return;
	}

	public void  GetvcNameStr(string message) {
		MLog.i(TAG, "GetvcNameStr:" + message);
		return;
	}

	public void  GetMarketCode(string message) {
		MLog.i(TAG, "GetMarketCode:" + message);
		MobageCallbackManager.marketLib.GetMarketCode(MobageMarketCode.fromString(message));
		return;
	}

	// Remote Notification
    public void OnPushSendCompleteSuccess(string message) {
		MLog.i(TAG, "OnPushSendCompleteSuccess:" + message);
		MobageCallbackManager.pushsendLib.GetonSuccess(message);
		return;
	}

    public void OnPushSendCompleteError(string message) {
		MLog.i(TAG, "OnPushSendCompleteError:" + message);
		MobageCallbackManager.pushsendLib.GetonError(message);
		return;
	}
	
    public void OnPushGetEnableCompleteSuccess(string message) {
		MLog.i(TAG, "OnPushGetEnableCompleteSuccess:" + message);
		MobageCallbackManager.pushgetLib.GetonSuccess(message);
		return;
	}

    public void OnPushGetEnableCompleteError(string message) {
		MLog.i(TAG, "OnPushGetEnableCompleteError:" + message);
		MobageCallbackManager.pushgetLib.GetonError(message);
		return;
	}

    public void OnPushSetEnableCompleteSuccess(string message) {
		MLog.i(TAG, "OnPushSetEnableCompleteSuccess:" + message);
		MobageCallbackManager.pushsetLib.GetonSuccess(message);
		return;
	}

    public void OnPushSetEnableCompleteError(string message) {
		MLog.i(TAG, "OnPushSetEnableCompleteError:" + message);
		MobageCallbackManager.pushsetLib.GetonError(message);
		return;
	}

    public void OnPushHandleReceivedComplete(string message) {
		MLog.i(TAG, "OnPushHandleReceivedComplete:" + message);
		MobageCallbackManager.handlerLib.GetonSuccess(message);
		return;
	}
	

	public void OnGetMobageVendorIdComplete(string message)
	{
		MobageCallbackManager.VendorIdLib.GetonSuccess(message);
	}

	public void GetFacebookUserComplete(string message)
	{
		Proxy.GetFacebookUser.onNativeCallback(message);
	}
		
}



