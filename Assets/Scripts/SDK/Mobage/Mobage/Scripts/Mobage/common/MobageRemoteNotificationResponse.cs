/**
 * MobageUnityRemoteNotificationResponse
 */

using System.Runtime.InteropServices;
using System.Collections;
using System.Collections.Generic;


/*!
 * @abstract Information about the response from server.
 */
public class MobageRemoteNotificationResponse {

	private string senderId_;
	private MobageRemoteNotificationPayload payload_;
	private string published_;

	/*!
	 * @abstract Unique identifier for the remote notification.
	 */
	public string senderId
	{
		set{this.senderId_ = value;}
		get{return this.senderId_;}
	}

	/*!
	 * @abstract payload value specified in the send() method.
	 */
	public MobageRemoteNotificationPayload payload
	{
		set{this.payload_ = value;}
		get{return this.payload_;}
	}

	/*!
	 * @abstract ISO 8601 Timestamp when the Mobage API server received the request.
	 */
	public string published
	{
		set{this.published_ = value;}
		get{return this.published_;}
	}

}
