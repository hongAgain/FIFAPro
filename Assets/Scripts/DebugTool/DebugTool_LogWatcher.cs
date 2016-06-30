using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DebugTool_LogWatcher : MonoBehaviour {

	public static DebugTool_LogWatcher logWatcher = null;
	
//	List<System.Action<string,string,LogType>> delegates = new List<System.Action> ();
	System.Action<string,string,LogType> delegates = null;

	public static void RegisterLogWatcherDelegate(System.Action<string,string,LogType> delegateToRegister)
	{
		if(logWatcher!=null)
			logWatcher.delegates += delegateToRegister;
	}

	public static void UnregisterLogWatcherDelegate(System.Action<string,string,LogType> delegateToRegister)
	{
		if(logWatcher!=null)
			logWatcher.delegates -= delegateToRegister;		
	}

	void Awake () 
	{
		logWatcher = this;
		Application.RegisterLogCallback(HandleLog);
	}

	void HandleLog(string logString, string stackTrace, LogType type) 
	{
		if (delegates != null)
			delegates(logString, stackTrace, type);
	}

	void OnDestroy () 
	{
		Application.RegisterLogCallback(null);
	}
}
