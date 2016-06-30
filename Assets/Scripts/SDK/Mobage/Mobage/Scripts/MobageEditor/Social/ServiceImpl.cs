using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Text.RegularExpressions;


public class ServiceImpl
{
	private static  ServiceImpl mInstance = null;
	/*!
	 * @Login in browser,Return instance of ServiceImpl.
	 */
	public static  ServiceImpl GetInstance()
	{
		if(mInstance == null)
		{
			Application.OpenURL(HostConfig.GetInstance().LoginURL);
			Thread.Sleep(3000);//wait for login in browser
			mInstance = new  ServiceImpl();
		}
		return mInstance;
	}
	
	/*!
	 * @OpenUserProfile,load in browser.
	 * @discussion 
	 * @param {string} userId.
	 * @param {function} onComplete The callback function that handles request complete.
	 */
	public void OpenUserProfile(string userId,  SocialPFRequest.CallBackOnComplete onComplete)
	{
		string url= HostConfig.GetInstance().BASE_URL;
		url += "/_u?u=";
		url += userId;
		Application.OpenURL(url);
	}
	
	/*!
	 * @OpenUserProfile,load in browser.
	 * @discussion 
	 * @param {string} keys.
	 * @param {string} appId.
	 * @param {function} onComplete The callback function that handles request complete.
	 */
	public void OpenFriendPicker(string keys, SocialPFRequest.CallBackOnComplete onComplete)
	{
		string url= HostConfig.GetInstance().BASE_URL;
		url += "/_pf_sdk_pick_friend_list?gid=";
		url += GlobalVar.GetInstance().AppID;
		url += "&maxFriendsToSelect=";
		url += keys;
		url += "&_sdk_api=1";
		Application.OpenURL(url);
	}
	
	/*!
	 * @ShowBalanceDialog,load in browser.
	 * @discussion 
	 * @param {string} appId.
	 * @param {function} onComplete The callback function that handles request complete.
	 */
	public void ShowBalanceDialog(SocialPFRequest.CallBackOnComplete onComplete)
	{
		bool isIOS = (PFConfig.GetPFType() == PFConfig.PFTYPE.IOS);
		string url= HostConfig.GetInstance().BASE_URL;
		url += "/_pf_sdk_pick_friend_list?gid=";
		url += (isIOS) ? "/_coin_vc_t" :"/_coin_hist";
		url += "?app_id=";
		url += GlobalVar.GetInstance().AppID;
		Application.OpenURL(url);
	}
	
	private string kDocumentAgreement = "/thisgame/agreement";
	private string kDocumentLegal = "/thisgame/tokushoho";
	private string  kDocumentContact = "/thisgame/inquiry";
    /*!
	 * @OpenDocument,load in browser.
	 * @discussion 
	 * @param {string} appId.
	 * @param {string} keys.
	 * @param {function} onComplete The callback function that handles request complete.
	 */
	public void OpenDocument(string keys, SocialPFRequest.CallBackOnComplete onComplete)
	{
		string path = "";
		int documentType = Convert.ToInt32(keys);
		switch(documentType)
		{
			case 0:path = kDocumentAgreement;break;
			case 1:path = kDocumentLegal;break;
			case 2:path = kDocumentContact;break;
			default: break;
		}
		if(path != "")
		{
			string url= HostConfig.GetInstance().BASE_URL;
			url += "/_sdk_page_redirect?path=";
			url += URLEncoder.Encode(path);
			url += "&app_id=";
			url += GlobalVar.GetInstance().AppID;
			Application.OpenURL(url);		
		}
	}
	
	/*!
	 * @ShowBankUI,load in browser.
	 * @param {function} onComplete The callback function that handles request complete.
	 */
	public void ShowBankUI(SocialPFRequest.CallBackOnComplete onComplete)
	{
		bool isIOS = (PFConfig.GetPFType() == PFConfig.PFTYPE.IOS);
		string url= HostConfig.GetInstance().BASE_URL;
		url += (isIOS) ? "/_coin_vc_t" :"/_coin_hist";
		Application.OpenURL(url);
	}
	
	/*!
	 * @OpenHomePage,load in browser.
	 * @param {function} onComplete The callback function that handles request complete.
	 */
	public void OpenHomePage(SocialPFRequest.CallBackOnComplete onComplete)
	{
		string url= HostConfig.GetInstance().BASE_URL;
		Application.OpenURL(url);
	}

}

