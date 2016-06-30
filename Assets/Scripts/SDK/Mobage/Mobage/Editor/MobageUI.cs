using UnityEngine;
using UnityEditor;
using System.Collections;

public class MobageUI {

	[MenuItem("Mobage/Online Document")]
	public static void Document()
	{
		Application.OpenURL("https://wiki.dena.jp/display/SDKCH/Unity");
	}
}
