using UnityEngine;
using System.Collections;

public static class EditorConf{

	public enum PlatformEnv{
		SANDBOX = 0,
		PRODUCTION = 1,
	}

	public static string affcode = "00000000";
	public static PlatformEnv platformEnv = PlatformEnv.SANDBOX; 
}
