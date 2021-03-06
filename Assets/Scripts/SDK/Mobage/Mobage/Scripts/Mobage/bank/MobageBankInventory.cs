/**
 * MobageUnityBankInventory
 */


using System.Collections;



/*!
 * @abstract Provides an interface to retrieve items from inventory. 
 * @discussion The Mobage platform server is responsible for managing a game's item inventory.
 */
public class MobageBankInventory {

	/*!
	 * @abstract Retrieves the item identified by its product ID from inventory on the Mobage platform server.
	 * @param itemId The product ID for the item.
	 * @param onComplete Retrieves the item from inventory on the Mobage platform server.
	 * @param onError Callback interface that handles errors.
	 *
	 */
	public static void getItem( string itemId, 
	                            BankInventryCallBackLib.OnSuccess onSuccess, 
	                            BankInventryCallBackLib.OnError onError )
	{
		MobageManager.getItem(itemId, onSuccess, onError);
		return;
	}


}


