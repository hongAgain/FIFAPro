/**
 * MobageUnityRemoteNotificationPayload
 */

using System.Runtime.InteropServices;
using System.Collections;
using System.Collections.Generic;


/*!
 * @abstract parameters passed to the device.
 */
public class MobageRemoteNotificationPayload {

	
	/*!
	 * @abstract If the notification is sent to an iOS device, this number is used
	 *      to badge the application icon.  If no badge is included, no numeric badge will be sent.  Including this
	 *      field for target Android devices has no affect and does not count toward payload size constraints.
	 */
	public int badge;

	/*!
	 * @abstract UTF-8 message body
	 */
	public string message;

	/*!
	 * @abstract If the notification is sent to an iOS device, this is the name of
	 *      the sound file in the application bundle.  If the sound file doesn't exist, or "default" is specified as
	 *      the value, the default alert sound is played.  Including this field for target Android devices has no
	 *      affect and does not count toward payload size constraints.
	 */
	public string sound;

	/*!
	 * @abstract If the notification is sent to an Android device, this is
	 *      the collapse key to coalesce multiple messages into a single group, so that only the last received
	 *      notification is visible.  If no key is specified, all notifications will use the same collapseKey.
	 *      Including this field for target iOS devices has no affect and doesn't count toward payload size constraints.
	 */
	public string collapseKey;

	/*!
	 * @abstract Android Device
	 */
	public string style;

	/*!
	 * @abstract Android Device
	 */
	public string iconUrl;

	/*!
	 * @abstract 
	 *		<br/>Custom 1 level hash of key/value parameters defined by the developer.
	 *      <br/>Note; the string key for each item in this section cannot contain any of the payload's defined key values.
	 */
	public Dictionary<string, string> extras;

	public PJson getPJson(){
		PJson ret = new PJson ();
		ret ["badge"] = this.badge;
		ret ["message"] = this.message;
		ret ["sound"] = this.sound;
		ret ["collapseKey"] = this.collapseKey;
		ret ["style"] = this.style;
		ret ["iconUrl"] = this.iconUrl;
		return ret;
	}


}
