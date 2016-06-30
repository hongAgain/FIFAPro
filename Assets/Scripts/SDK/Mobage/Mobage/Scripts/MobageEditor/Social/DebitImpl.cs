using System;
using System.Collections.Generic;
using UnityEngine;
using MobageLitJson;
public class DebitImpl
{
	private string TAG = "DebitImpl";
	private static DebitImpl mInstance = null;
	private const string RESULT = "\"result\":";
	private const string ERROR = "\"error\":";
	/*!
	 * @Return instance of DebitImpl.
	 */
	public static DebitImpl GetInstance()
	{
		if(mInstance == null)
		{
			mInstance = new DebitImpl();
		}
		return mInstance;
	}
	
	/*!
	 * @GetPendingTransactions
	 * @discussion 
	 * @param {function} onComplete The callback function  that handles request complete.
	 */
	public void GetPendingTransactions(SocialPFRequest.CallBackOnComplete onComplete)
	{
		return;
	}
	
	/*!
	 * @GetTransaction
	 * @discussion 
	 * @param {string} transactionId The id of this transaction.
	 * @param {function} onComplete The callback function  that handles request complete.
	 */
	public void GetTransaction(string transactionId,  SocialPFRequest.CallBackOnComplete onComplete)
	{
		SortedDictionary<string, object> parameters = new SortedDictionary<string, object>();
		parameters.Add("transactionId", transactionId); 
		SortedDictionary<string, object> transaction = new SortedDictionary<string, object>();
		transaction.Add("state", "open");	
		parameters.Add("transaction", transaction);
		(new SocialPFRequest()).RequestWithInfo(parameters, "bankdebit.get", onComplete);
	}
	
	
	/*!
	 * @OpenTransaction
	 * @discussion 
	 * @param {string} transactionId The id of this transaction.
	 * @param {function} onComplete The callback function that handles request complete.
	 */
	public void OpenTransaction(string transactionId,  SocialPFRequest.CallBackOnComplete onComplete)
	{
		SortedDictionary<string, object> parameters = new SortedDictionary<string, object>();	
		parameters.Add("transactionId", transactionId); 
		SortedDictionary<string, object> transaction = new SortedDictionary<string, object>();
		transaction.Add("state", "open");	
		parameters.Add("transaction", transaction);
		(new SocialPFRequest()).RequestWithInfo(parameters, "bankdebit.update", onComplete);
	}
	
	/*!
	 * @CloseTransaction
	 * @discussion 
	 * @param {string} transactionId The id of this transaction.
	 * @param {function} onComplete The callback function that handles request complete.
	 */
	public void CloseTransaction(string transactionId,  SocialPFRequest.CallBackOnComplete onComplete)
	{
		SortedDictionary<string, object> parameters = new SortedDictionary<string, object>();
		
		parameters.Add("transactionId", transactionId); 
		
		SortedDictionary<string, object> transaction = new SortedDictionary<string, object>();
		transaction.Add("state", "closed");	
		parameters.Add("transaction", transaction);
		(new SocialPFRequest()).RequestWithInfo(parameters, "bankdebit.update", onComplete);
	}
	
	/*!
	 * @CancelTransaction
	 * @discussion 
	 * @param {string} transactionId The id of this transaction.
	 * @param {function} onComplete The callback function that handles request complete.
	 */
	public void CancelTransaction(string transactionId,  SocialPFRequest.CallBackOnComplete onComplete)
	{
		SortedDictionary<string, object> parameters = new SortedDictionary<string, object>();
		parameters.Add("transactionId", transactionId); 
		SortedDictionary<string, object> transaction = new SortedDictionary<string, object>();
		transaction.Add("state", "canceled");	
		parameters.Add("transaction", transaction);
		(new SocialPFRequest()).RequestWithInfo(parameters, "bankdebit.update", onComplete);
	}
	
	private string mTransactionId = "";
	private string mPrice = "0";
	private SocialPFRequest.CallBackOnComplete OnComplete = null;
	/*!
	 * @OnContinueTransactionDlgComplete
	 * @discussion 
	 * @param {string} selection ok or cancel.
	 */
	private void OnContinueTransactionDlgComplete(string selection)
	{
		switch (selection)
		{
			case "ok":ContinueTransaction();break;
			case "cancel":CancelTransaction(mTransactionId,EditorCallback.GetInstance().CancelTransactionComplete);break;
			default :break;
		}
	}
	
	/*!
	 * @OnRechargeDlgComplete
	 * @discussion 
	 * @param {string} selection ok or cancel.
	 */
	private void OnRechargeDlgComplete(string selection)
	{
		switch (selection)
		{
			case "ok":ServiceImpl.GetInstance().ShowBankUI(EditorCallback.GetInstance().OnDialogComplete);break;
			case "cancel":CancelTransaction(mTransactionId,EditorCallback.GetInstance().CancelTransactionComplete);break;
			default :break;
		}
	}

	
	private void OnCheckBalance(string message)
	{		
		int balance = 0;
		JsonData  data = JsonMapper.ToObject(message);
		if(message.Contains(RESULT))
		{
			JsonData res = data["result"];
			balance = Convert.ToInt32(res["balance"].ToString());
		}
		else 
		{
			if(message.Contains(ERROR))
			{
				string val = JsonMapper.ToJson(data["error"]);
				MLog.e(TAG, val);
				return;
			}		
		}
		int price = Convert.ToInt32(mPrice);
		string intro = "Balance is " + balance + "\nPrice is " + price;
		if(balance < price)
		{
			
			MobageDialog.ShowDialog(intro + "\nGo to recharge page?", OnRechargeDlgComplete);	
		}
		else
		{
			MobageDialog.ShowDialog(intro + "\nAre you sure to buy it?", OnContinueTransactionDlgComplete);	
		}
	}
	
	private void ContinueTransaction()
	{
		SortedDictionary<string, object> parameters = new SortedDictionary<string, object>();	
		parameters.Add("transactionId", mTransactionId); 
		SortedDictionary<string, object> transaction = new SortedDictionary<string, object>();
		transaction.Add("state", "authorized");	
		parameters.Add("transaction", transaction);
		(new SocialPFRequest()).RequestWithInfo(parameters, "bankdebit.get", OnComplete);
	}
	
	/*!
	 * @ContinueTransaction,get and check balance.
	 * @discussion 
	 * @param {string} transactionId The id of this transaction.
	 * @param {string} appId The id of the application.
	 * @param {function} onComplete The callback function that handles request complete.
	 */
	public void ContinueTransaction(string transactionId, SocialPFRequest.CallBackOnComplete onComplete)
	{
		//get balance
		BalanceImpl.GetInstance().GetBalance(OnCheckBalance);
		//save call back function
		OnComplete = onComplete;
		//save transactionId
		mTransactionId = transactionId;
	}

	/*!
	 * @ContinueTransaction
	 * @discussion 
	 * @param {string} keys The params to create transaction.
	 * @param {string} appId The id of the application.
	 * @param {function} onComplete The callback function that handles request complete.
	 */
	public void CreateTransaction(string keys,  SocialPFRequest.CallBackOnComplete onComplete)
	{
		Dictionary<string, object> info = MobageSerializer.DeSerialize(keys);				
		SortedDictionary<string, object> item = new SortedDictionary<string, object>();
		
		//save price
		if(info.ContainsKey("price"))
        mPrice = info["price"].ToString();
		
		item.Add("id", info["id"]);		
		SortedDictionary<string, object> items = new SortedDictionary<string, object>();
		items.Add("item", item);
		items.Add("quantity", Convert.ToInt32(info["quantity"]));
		
		SortedDictionary<string, object> parameters = new SortedDictionary<string, object>();
		parameters.Add("comment", info["Comment"]); 
		parameters.Add("items", items); 
		parameters.Add("state", "authorized"); 
		SortedDictionary<string, object> transaction = new SortedDictionary<string, object>();
		transaction.Add("transaction", parameters);
		(new SocialPFRequest()).RequestWithInfo(transaction, "bankdebit.create", onComplete);
	}
	
}

