using UnityEngine;
using System.Collections;

public class SDKMgr
{
    private static ISDK sdk = null;

	// Use this for initialization
    public static void Start()
    {
#if UNITY_EDITOR
        sdk = new NoSDK();
#elif Enable_NoSDK
        sdk = new NoSDK();
#elif Enable_Mobage
        var prefab = Resources.Load("MobageCallback") as GameObject;
        var clone = (GameObject)Object.Instantiate(prefab);
        clone.name = clone.name.Replace("(Clone)", "");
        sdk = clone.AddComponent<SDKAdapter_Mobage>();
#else
        sdk = new NoSDK();
#endif
	}

    public static void CallSDK(Hashtable kv, OnResponse response)
    {
        sdk.DispatchEvent(kv, response);
    }

    public static void CallSDK(Hashtable kv)
    {
        CallSDK(kv, null);
    }
}