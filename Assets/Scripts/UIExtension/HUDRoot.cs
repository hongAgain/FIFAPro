//--------------------------------------------
//            NGUI: HUD Text
// Copyright © 2012 Tasharen Entertainment
//--------------------------------------------

using UnityEngine;

[AddComponentMenu("NGUI/Examples/HUD Root")]
public class HUDRoot : MonoBehaviour
{
	static public GameObject Go;
	void Awake () 
    {
        Go = gameObject; 
    }
}