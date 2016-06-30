using System;
using System.Collections.Generic;
public class InventoryImpl
{
	private static InventoryImpl mInstance = null;
	/*!
	 * @Return instance of InventoryImpl.
	 */
	public static InventoryImpl GetInstance()
	{
		if(mInstance == null)
		{
			mInstance = new InventoryImpl();
		}
		return mInstance;
	}
	
	/*!
	 * @GetItem
	 * @discussion 
	 * @param {string} itemId The id of item.
	 * @param {function} onComplete The callback function that handles request complete.
	 */
	public void GetItem(string itemId, SocialPFRequest.CallBackOnComplete onComplete)
	{
		SortedDictionary<string, object> parameters = new SortedDictionary<string, object>();
		List<string> fields = new List<string>();
		fields.Add("id");
		fields.Add("price");
		fields.Add("imageUrl");
		fields.Add("name");
		fields.Add("description");
		parameters.Add("fields", fields); 
		if(itemId != "")
		{
			parameters.Add("itemId", itemId);
		}
		(new SocialPFRequest()).RequestWithInfo(parameters, "bankinventory.get", onComplete);
	}
}


