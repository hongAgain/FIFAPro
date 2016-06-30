/*
 *  UnityEditorCallback.cs
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

public class EditorCallback {
	
	private string TAG = "EditorCallback";
	private static MobageCallback mCallback = null;
	private const string RESULT = "\"result\":";
	private const string ERROR = "\"error\":";
	private const string ENTRY = "\"entry\":";
	private  static EditorCallback mInstance = null;
	/*!
	 * @Return instance of EditorCallback.
	 */
	public static EditorCallback GetInstance()
	{
		if(mInstance == null)
		{
			mInstance = new EditorCallback();
		}
		return mInstance;
	}
	public void SetCallback(MobageCallback callback)
	{
		 mCallback = callback;
	}
	
	//login
    public void OnLoginComp(string message)
	{
		MLog.d(TAG, "OnLoginComp:"+message);
		if(message == null)
		{
			MobageError error = new MobageError();
			error.description = "login failed!";
			error.code = 400;
			string val = JsonMapper.ToJson(error);
			mCallback.addLoginListenerError(val);
		}
		else mCallback.addLoginListenerComp(message);
		return;
	}


	// Logout
    public void LogoutComp(string selection)
	{
		switch (selection)
		{
			case "ok":mCallback.LogoutComp("LOGOUTED.");break;
			case "cancel":mCallback.LogoutCancel("LOGOUT CANCELED.");break;
			default :break;
		}
		return;
	}


	// People
    public  void OnGetUserComplete(string message) 
	{
		MLog.d(TAG, "OnGetUserComplete:" + message);
		JsonData  data = JsonMapper.ToObject( message);
		if(message.Contains(RESULT))
		{
			string val = JsonMapper.ToJson(data["result"]);
	   		mCallback.OnGetUserCompleteSuccess(val);
		}
		else if(message.Contains(ERROR))
		{
			string val = JsonMapper.ToJson(data["error"]);			
			mCallback.OnGetUserCompleteError(val.Replace("message", "description"));
		}
		else 
		{
			MLog.e(TAG,"error");	
		}
		return;
	}

	public  void OnGetCurrentUserComplete(string message) 
	{
		MLog.d(TAG, "OnGetCurrentUserComplete:" + message);
		JsonData  data = JsonMapper.ToObject( message);
		if(message.Contains(RESULT))
		{
			string val = JsonMapper.ToJson(data["result"]);
			mCallback.OnGetCurrentUserSuccess(val);
		}
		else if(message.Contains(ERROR))
		{
			string val = JsonMapper.ToJson(data["error"]);			
			mCallback.OnGetCurrentUserError(val.Replace("message", "description"));
		}
		else 
		{
			MLog.e(TAG,"error");	
		}
		return;
	}
	
    public void OnGetUsersComplete(string message)
	{
		MLog.d(TAG, "OnGetUsersComplete:" + message);
		JsonData  data = JsonMapper.ToObject(message);
		if(message.Contains(RESULT))
		{
			JsonData info = data["result"]; 
			string val = "{";
			val += "{Users::";
			if(info["entry"] != null)
			{
				JsonData users = info["entry"];
				for(int i = 0; i < users.Count; i++)
				{
					val += JsonMapper.ToJson(users[i]);
					if( i < users.Count - 1) val += ",";
				}	
			}
			val += "}";
			val += ",";
			val += "{start::";
			val += info["startIndex"].ToString();
			val += "}";
			val += "{count::";
			val += info["itemsPerPage"].ToString();
			val += "}";
			val += "{total::";
			val += info["totalResults"].ToString();
			val += "}";
			val += "}";
			mCallback.OnGetUsersCompleteSuccess(val);
		}
		else if(message.Contains(ERROR))
		{
			string val = JsonMapper.ToJson(data["error"]);
			mCallback.OnGetUsersCompleteError(val.Replace("message", "description"));
		}
		else 
		{
			MLog.e(TAG,"error");
		}
		return;
	}
	
	// BlackList
    public  void OnCheckBlacklistComplete(string message)
	{
	    MLog.d(TAG, "OnCheckBlacklistComplete:" + message);
		JsonData  data = JsonMapper.ToObject(message);
		if(message.Contains(RESULT))
		{
			string val = "{{";
			JsonData info = data["result"]; 
			if(JsonMapper.ToJson(info).Contains(ENTRY))
			{
				
				if(info["entry"].Count > 0)
				{
					JsonData users = info["entry"];
					for(int i = 0; i < users.Count; i++)
					{
						val += JsonMapper.ToJson(users[i]);
						if(i < users.Count - 1) val += ",";
					}
				}
				val += "}";
				val += ",";
				val += "{start::";
				val += info["startIndex"].ToString();
				val += "}";
				val += "{count::";
				val += info["itemsPerPage"].ToString();
				val += "}";
				val += "{total::";
				val += info["totalResults"].ToString();
				val += "}";
				val += "}";
			}
			else
			{
				
				val += "targetId:";
				val += info["targetId"];
				val += "}";
				val += ",";
				val += "{start::";
				val += "0";
				val += "}";
				val += "{count::";
				val += "1";
				val += "}";
				val += "{total::";
				val += "1";
				val += "}";
				val += "}";
				
			}
			mCallback.OnCheckBlacklistCompleteSuccess(val);
		}
		else if(message.Contains(ERROR))
		{
			string val = JsonMapper.ToJson(data["error"]);
			mCallback.OnCheckBlacklistCompleteError(val.Replace("message", "description"));
		}
		else 
		{
			MLog.e(TAG,"error");
		}
		return;
	}
	
    public void OnDialogComplete(string message) {
	    MLog.d(TAG, "OnDialogComplete:" + message);
		mCallback.OnDialogComplete(message);			
		return;
	}
	
	// Auth
    public  void AuthorizeToken(string message)
	{
		MLog.d(TAG, "AuthorizeToken:" + message);
		JsonData  data = JsonMapper.ToObject(message);	
		if(message.Contains(RESULT))
		{
			string val = JsonMapper.ToJson(data["result"]["verifier"]);
			val = val.Trim("\"\'".ToCharArray());
	   		mCallback.AuthorizeTokenSuccess(val);
		}
		else if(message.Contains(ERROR))
		{
			string val = JsonMapper.ToJson(data["error"]);
			mCallback.AuthorizeTokenError(val.Replace("message", "description"));
		}	
		else 
		{
			MLog.e(TAG,"error");	
		}
		return;
	}
	
	// Bank
    public void CreateTransactionWithDialogComplete(string message)
	{
		MLog.d(TAG, "CreateTransactionWithDialogComplete:" + message);
		JsonData  data = JsonMapper.ToObject(message);
		if(message.Contains(RESULT))
		{
			string id = data["result"].ToString();
			JsonData transactionId = new JsonData();
			transactionId["id"] = id;
			string val =JsonMapper.ToJson(transactionId);
	   		mCallback.TransactionWithDialogCompleteSuccess(val);
		}
		else if(message.Contains(ERROR))
		{
			string val = JsonMapper.ToJson(data["error"]);
			mCallback.TransactionWithDialogCompleteError(val.Replace("message", "description"));
		}		
		else 
		{
			MLog.e(TAG,"error");
		}	
		return;
	}
	
	public void ContinueTransactionWithDialogComplete(string message)
	{
		MLog.d(TAG, "ContinueTransactionWithDialogComplete:" + message);
		JsonData  data = JsonMapper.ToObject(message);
		if(message.Contains(RESULT))
		{
			JsonData info = data["result"];
			JsonData items = info["items"];
			for(int i = 0; i< items.Count; i++)
			{
				if(items[i]["item"]["imageUrl"] == null)
				items[i]["item"]["imageUrl"] = "";
			}
			string val = JsonMapper.ToJson(info);
	   		mCallback.TransactionWithDialogCompleteSuccess(val);
		}
		else if(message.Contains(ERROR))
		{
			string val = JsonMapper.ToJson(data["error"]);
			mCallback.TransactionWithDialogCompleteError(val.Replace("message", "description"));
		}	
		else 
		{
			MLog.e(TAG,"error");	
		}
		
		return;
	}

	//void TransactionWithDialogCompleteCancel(string message) {
	//	MLog.d(TAG, "TransactionWithDialogCompleteCancel:" + message);
	//	mCallback.TransactionWithDialogCompleteCancel(message);
	//	return;
	//}
		
	public void OpenTransactionComplete(string message)
	{
		MLog.d(TAG, "OpenTransactionComplete:" + message);
		JsonData  data = JsonMapper.ToObject(message);
		if(message.Contains(RESULT))
		{
			JsonData info = data["result"];
			JsonData items = info["items"];
			for(int i = 0; i< items.Count; i++)
			{
				if(items[i]["item"]["imageUrl"] == null)
				items[i]["item"]["imageUrl"] = "";
			}
			string val = JsonMapper.ToJson(info);
	   		mCallback.TransactionCompleteSuccess(val);
		}
		else if(message.Contains(ERROR))
		{
			string val = JsonMapper.ToJson(data["error"]);
			mCallback.TransactionCompleteError(val.Replace("message", "description"));
		}	
		else 
		{
			MLog.e(TAG,"error");
		}
		return;
	}
	
	public void CancelTransactionComplete(string message) 
	{
		MLog.d(TAG, "CancelTransactionComplete:" + message);
		JsonData  data = JsonMapper.ToObject(message);
		if(message.Contains(RESULT))
		{
			JsonData info = data["result"];
			JsonData items = info["items"];
			for(int i = 0; i< items.Count; i++)
			{
				if(items[i]["item"]["imageUrl"] == null)
				items[i]["item"]["imageUrl"] = "";
			}
			string val = JsonMapper.ToJson(info);
	   		mCallback.TransactionWithDialogCompleteCancel(val);
		}
		else if(message.Contains(ERROR))
		{
			string val = JsonMapper.ToJson(data["error"]);
			mCallback.TransactionCompleteError(val.Replace("message", "description"));
		}	
		else 
		{
			MLog.e(TAG,"error");
		}
		return;
	}
			
	public void CloseTransactionComplete(string message)
	{
		MLog.d(TAG, "CloseTransactionComplete:" + message);
		JsonData  data = JsonMapper.ToObject(message);
		if(message.Contains(RESULT))
		{
			JsonData info = data["result"];
			JsonData items = info["items"];
			for(int i = 0; i< items.Count; i++)
			{
				if(items[i]["item"]["imageUrl"] == null)
				items[i]["item"]["imageUrl"] = "";
			}
			string val = JsonMapper.ToJson(info);
	   		mCallback.TransactionCompleteSuccess(val);
		}
		else if(message.Contains(ERROR))
		{
			string val = JsonMapper.ToJson(data["error"]);
			mCallback.TransactionCompleteError(val.Replace("message", "description"));
		}	
		else 
		{
			MLog.e(TAG,"error");
		}
		return;
	}
	
	public void GetTransactionComplete(string message)
	{
		MLog.d(TAG, "GetTransactionComplete:" + message);
		JsonData  data = JsonMapper.ToObject(message);
		if(message.Contains(RESULT))
		{
			JsonData info = data["result"];
			JsonData items = info["items"];
			for(int i = 0; i< items.Count; i++)
			{
				if(items[i]["item"]["imageUrl"] == null)
				items[i]["item"]["imageUrl"] = "";
			}
			string val = JsonMapper.ToJson(info);
	   		mCallback.TransactionCompleteSuccess(val);
		}
		else if(message.Contains(ERROR))
		{
			string val = JsonMapper.ToJson(data["error"]);
			mCallback.TransactionCompleteError(val.Replace("message", "description"));
		}	
		else 
		{
			MLog.e(TAG,"error");	
		}
		return;
	}
	
	public void GetPendingTransactionComplete(string message) 
	{
		MLog.d(TAG, "GetPendingTransactionComplete:" + message);
		JsonData  data = JsonMapper.ToObject(message);
		if(message.Contains(RESULT))
		{
			JsonData info = data["result"];
			JsonData items = info["items"];
			for(int i = 0; i< items.Count; i++)
			{
				if(items[i]["item"]["imageUrl"] == null)
				items[i]["item"]["imageUrl"] = "";
			}
			string val = JsonMapper.ToJson(info);
	   		mCallback.TransactionCompleteSuccess(val);
		}
		else if(message.Contains(ERROR))
		{
			string val = JsonMapper.ToJson(data["error"]);
			mCallback.TransactionCompleteError(val.Replace("message", "description"));
		}	
		else 
		{
			MLog.e(TAG,"error");
		}
		return;
	}
	
	public void GetItemComplete(string message)
	{
		MLog.d(TAG, "GetItemComplete:" + message);
		JsonData  data = JsonMapper.ToObject(message);
		if(message.Contains(RESULT))
		{			
			string val = JsonMapper.ToJson(data["result"]);
	   		mCallback.GetItemCompleteSuccess(val);
		}
		else if(message.Contains(ERROR))
		{
			string val = JsonMapper.ToJson(data["error"]);
			mCallback.GetItemCompleteError(val.Replace("message", "description"));
		}		
		else 
		{
			MLog.e(TAG,"error");	
		}
		return;
	}
			
	public void  OnGetBalanceComplete(string message) 
	{
		MLog.d(TAG, "OnGetBalanceCompleteSuccess:" + message);
		JsonData  data = JsonMapper.ToObject(message);
		if(message.Contains(RESULT))
		{
			JsonData res = data["result"];
			string val = res["balance"].ToString();
			string limitation = res["limitation"].ToString();
			string state = res["state"].ToString();
			string ret = val + "," + limitation + "," + state;
			mCallback.OnGetBalanceCompleteSuccess(ret);
		}
		else if(message.Contains(ERROR))
		{
			string val = JsonMapper.ToJson(data["error"]);
			mCallback.OnGetBalanceCompleteError(val.Replace("message", "description"));
		}		
		else 
		{
			MLog.e(TAG,"error");
		}
		
		return;
	}
	// JPSocial
	public void  GetvcNameStr(string message)
	{
		MLog.d(TAG, "GetvcNameStr:" + message);
		mCallback.GetvcNameStr(message);
		return;
	}

	public void  GetMarketCode(string message) 
	{
		MLog.d(TAG, "GetMarketCode:" + message);
		mCallback.GetMarketCode(message);
		return;
	}

	//Notification
	public void OnPushSendComplete(string message)
	{
		MLog.d(TAG, "OnPushSendComplete:" + message);
		JsonData  data = JsonMapper.ToObject(message);
		if(message.Contains(RESULT))
		{
			JsonData info = data["result"];
			
			JsonData response = new JsonData();
			response["payload"] = info["payload"];
			response["published"] = info["published"];
			response["senderId"] = info["senderId"];
		    string val = JsonMapper.ToJson(response);
			mCallback.OnPushSendCompleteSuccess(val);
		}
		else if(message.Contains(ERROR))
		{
			string val = JsonMapper.ToJson(data["error"]);
			mCallback.OnPushSendCompleteError(val.Replace("message", "description"));
		}		
		else 
		{
			MLog.e(TAG,"error");
		}
		return;
	}
	
    public void OnPushSetEnabledComplete(string message)
	{
		MLog.d(TAG, "OnPushSetEnabledComplete:" + message);
		JsonData  data = JsonMapper.ToObject(message);
		if(message.Contains(RESULT))
		{
			mCallback.OnPushSetEnableCompleteSuccess("");
		}
		else if(message.Contains(ERROR))
		{
			string val = JsonMapper.ToJson(data["error"]);
			mCallback.OnPushSetEnableCompleteError(val.Replace("message", "description"));
		}		
		else 
		{
			MLog.e(TAG,"error");
		}
		return;
	}
	
	public void OnPushGetEnabledComplete(string message)
	{
		MLog.d(TAG, "OnPushGetEnabledComplete:" + message);
		JsonData  data = JsonMapper.ToObject(message);
		
		if(message.Contains(RESULT))
		{
			JsonData result = data["result"];
			string val = JsonMapper.ToJson(result["state"]);
			mCallback.OnPushGetEnableCompleteSuccess(val);
		}
		else if(message.Contains(ERROR))
		{
			string val = JsonMapper.ToJson(data["error"]);
			mCallback.OnPushSetEnableCompleteError(val.Replace("message", "description"));
		}	
		else 
		{
			MLog.e(TAG,"error");	
		}
		return;
	}
	
	public void OnHandleListenerComplete(string message)
	{
		MLog.d(TAG, "OnHandleListenerComplete:" + message);
		JsonData  data = JsonMapper.ToObject(message);
		
		if(message.Contains(RESULT))
		{
			string val = JsonMapper.ToJson(data["result"]);
			mCallback.OnPushHandleReceivedComplete(val);
		}
		else 
		{
			MLog.e(TAG,"error");
		}
		return;
	}
}






