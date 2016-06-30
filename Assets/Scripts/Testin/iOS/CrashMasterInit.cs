using UnityEngine;
using System.Collections;

public class CrashMasterInit : MonoBehaviour {

	/// <summary>
	/// Your CrashMaster Appkey.  Every app has a special identifier that allows CrashMaster
	/// to associate error monitoring data with your app.  Your Appkey can be found on the
	/// "App Settings" page of the app you are trying to monitor.
	/// </summary>
	/// <example>A real Appkey looks like this:  a0d49052a0853862a7c18a77d2a39838</example>
	
	void Awake ()
	{
		CrashMasterHelper.Init ();
		Destroy (this);
	}
	
}
