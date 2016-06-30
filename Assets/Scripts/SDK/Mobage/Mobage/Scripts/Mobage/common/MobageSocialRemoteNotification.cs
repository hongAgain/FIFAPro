/**
 * MobageUnitySocialRemoteNotification
 */

using System.Runtime.InteropServices;
using System.Collections;
using System.Collections.Generic;


/*!
 * @abstract 
 * Mobage provides a Common API for the sending of remote notifications to others within the Mobage Service.
 * The messages are sent and received by games using NativeSDK's network-based notification capabilities.
 * The Mobage platform service acts an intermediary to enqueue and send remote messages.
 * <br />
 * <code>RemoteNotification</code> Sends a remote notification (push message) to a user's device.
 * This API can be used to send a message to a single user within a given application.  Broadcast notifications
 * can be sent via the server's HTTP REST endpoints, but only when game server's OAuth tokens.
 */
public class MobageSocialRemoteNotification {

	
	/*!
	* @abstract 
	* Sends a remote notification to another user who is also playing a Mobage-enabled game. This API covers only
	* the "user to user" remote notification. This API is not currently rate limited, but Mobage reserves the
	* right to throttle or suspend accounts sending excessive remote notifications.
	* @param recipientId Recipient user's identifier.  This can be either a user's gamertag or user ID. 
	* @param data Additional parameters passed to the device. 
	* @param onSuccess Retrieves the remote notification send response.
	*        public delegate void OnSuccess(MobageRemoteNotificationResponse response);
	* @param onError Callback interface that handles errors.
	*        public delegate void OnError(MobageError err);
	*/
	public static void send( string recipientId, 
	                        MobageRemoteNotificationPayload data,
	                        RemoteNotificationSendCallBackLib.OnSuccess onSuccess, 
	                        RemoteNotificationSendCallBackLib.OnError onError ) {
		
		MobageManager.Pushsend(recipientId,
		                       data,
		                       onSuccess,
		                       onError);
		
		return;
	}

	/*!
	 * @abstract 
     * Returns true if the current logged in user can receive remote notifications for the current running application.
     * This can be called from within each application to determine if it is capable of receiving remote notifications.
	 * @param onSuccess Retrives whether or not the currently logged in user can recieve remote notifications
	 *        public delegate void OnSuccess(bool canBeNotified);
	 * @param onError The callback interface that handles errors.
	 *        public delegate void OnError(MobageError err);
	 */
	public static void getRemoteNotificationsEnabled( 
	                              RemoteNotificationGetEnableCallBackLib.OnSuccess onSuccess, 
	                              RemoteNotificationGetEnableCallBackLib.OnError onError ) {

		MobageManager.getRemoteNotificationsEnabled(onSuccess, onError);

		return;
	}

	/*!
	 * @abstract 
     * Boolean set value for whether or not the current logged in user may receive remote notifications for the current running application.
     * This can be called from within each application to set the remote notification receipt capability.
     * @param enabled Allows or disallows the current user to receive remote notifications.
	 * @param onSuccess Indicates the ability of a currently logged in user to recieve remote notifications was successfully set
	 *        public delegate void OnSuccess();
	 * @param onError The callback interface that handles errors.
	 *        public delegate void OnError(MobageError err);
	 */
	public static void setRemoteNotificationsEnabled( bool enabled, 
	                              RemoteNotificationSetEnableCallBackLib.OnSuccess onSuccess, 
	                              RemoteNotificationSetEnableCallBackLib.OnError onError ) {

		MobageManager.setRemoteNotificationsEnabled(enabled, onSuccess, onError);

		return;
	}



	#if UNITY_IPHONE || (!UNITY_ANDROID && !UNITY_IPHONE)
	/*!
	 * @abstract 
     * Callback listener interface for handle a remote notification listener.
     * @param onListener Handler The listener receiving incoming remote notifications
     *        public delegate void OnSuccess(MobageRemoteNotificationPayload payload);
	 */
	public static void setListener( RemoteNotificationHandlerCallBackLib.OnSuccess onListener ) {
		MobageManager.remoteNotificationListener(onListener);
		return;
	}
	#endif


}
