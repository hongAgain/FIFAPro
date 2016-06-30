using System;
using System.Collections.Generic;

public class BalanceImpl
{
	private static BalanceImpl mInstance = null;
	/*!
	 * @Return instance of BalanceImpl.
	 */
	public static BalanceImpl GetInstance()
	{
		if(mInstance == null)
		{
			mInstance = new BalanceImpl();
		}
		return mInstance;
	}
	
		
	/*!
	 * @DeleteEntries
	 * @discussion 
	 * @param {string} appid  The id of application
	 * @param {function} onComplete The callback function that handles request complete.
	 */
	public void GetBalance(SocialPFRequest.CallBackOnComplete onComplete) 
	{
		bool isIOS = (PFConfig.GetPFType() == PFConfig.PFTYPE.IOS);
		SortedDictionary<string, object> parameters = new SortedDictionary<string, object>();
		string currencyId = (isIOS )?"virtual_currency":"coin";
		parameters.Add("appId", GlobalVar.GetInstance().AppID);
		parameters.Add("currencyId", currencyId);
		(new SocialPFRequest()).RequestWithInfo(parameters, "bankbalance.get", onComplete);
	}


}
