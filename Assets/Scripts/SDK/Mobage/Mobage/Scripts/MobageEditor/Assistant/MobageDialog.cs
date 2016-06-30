using UnityEngine;
using System.Collections;

public class MobageDialog: MonoBehaviour{
	static bool WindowSwitch = false;
	int width = 200;
	int height = 80;
	static string mTitle;
	public delegate void CallBackOnDlgComplete(string response);
	private static CallBackOnDlgComplete OnDlgComplete = null;
	private Rect windowRect;
	
	void OnGUI ()
	{
	   if(WindowSwitch)
	   {
			windowRect = new Rect((Screen.width-width)/2, (Screen.height-height)/3, width, height);
	  		windowRect = GUI.Window (-1,  windowRect, WindowFunction, mTitle);
			GUI.depth = 0;
	   }
	}
	public static bool FocusDialog
	{
		get{return WindowSwitch;}
	}
		
	public static void ShowDialog (string message, CallBackOnDlgComplete onDlgComplete)
	{
	   WindowSwitch = true;
	   mTitle = message;
	   OnDlgComplete = onDlgComplete;
	}
	
	void  WindowFunction (int windowID)
	{
	   int btWidth = 80;
	   int btHeight = 20;
	   if (GUI.Button (new Rect (10,height - btHeight - 8, btWidth, btHeight), "Ok"))
	   {
	      WindowSwitch = false;
		  if(OnDlgComplete != null) OnDlgComplete("ok");
	   }
	   if (GUI.Button (new Rect (width - btWidth - 10 ,height - btHeight - 8, btWidth, btHeight), "Cancel"))
	   {
	      WindowSwitch = false;
		  if(OnDlgComplete != null) OnDlgComplete("cancel");
	   }
	    GUI.BringWindowToFront(-1);
		GUI.FocusWindow(-1);
	}
	
	void OnMouseEnter ()
	{
	    renderer.material.color = Color.blue;
	}
	
	//void OnMouseDown ()
	//{
	//   WindowSwitch = true;
	//}
	
	void OnMouseExit ()
	{
	    renderer.material.color = Color.white;
	}
}