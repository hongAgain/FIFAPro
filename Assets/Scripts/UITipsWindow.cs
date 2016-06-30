using UnityEngine;
using System.Collections;

public class UITipsWindow : MonoBehaviour {

	// Use this for initialization
//	void Start () 
//	{
//		Register();
//	}

	void OnEnable()
	{
		SetTipsActive(true);
	}

	bool isTipsActive = true;
	
	private void OnUIEventTriggered(UIEventHandlerFlags flags)
	{
//		Debug.LogError("isTipsActive "+isTipsActive);
//		Debug.LogError("flags.isEventLogicTriggered "+flags.isEventLogicTriggered);
//		Debug.LogError("flags.isUIEventMgrSelf "+flags.isUIEventMgrSelf);

		if(isTipsActive&&/*flags.isEventLogicTriggered&&*/!flags.isUIEventMgrSelf)
		{
			SetTipsActive(false);
			gameObject.SetActive(false);
		}
	}

	public void SetTipsActive(bool willActive)
	{
		isTipsActive = willActive;		
//		Debug.LogError("SetTipsActive "+isTipsActive);
		if(isTipsActive)
			Register();
		else
			Unregister();
	}

	public void Register()
	{		
//		Debug.LogError("Register");
//		UIEventMgr.uiEventManager.Register(OnUIEventTriggered,UIEventType.onPress);
		UIEventMgr.uiEventManager.Register(OnUIEventTriggered,UIEventType.onClick);
		UIEventMgr.uiEventManager.Register(OnUIEventTriggered,UIEventType.onDoubleClick);
		UIEventMgr.uiEventManager.Register(OnUIEventTriggered,UIEventType.onSelect);
		UIEventMgr.uiEventManager.Register(OnUIEventTriggered,UIEventType.onDragOver);
		UIEventMgr.uiEventManager.Register(OnUIEventTriggered,UIEventType.onDragOut);
	}

	public void Unregister()
	{
//		Debug.LogError("Unregister");
//		UIEventMgr.uiEventManager.UnRegister(OnUIEventTriggered,UIEventType.onPress);
		UIEventMgr.uiEventManager.UnRegister(OnUIEventTriggered,UIEventType.onClick);
		UIEventMgr.uiEventManager.UnRegister(OnUIEventTriggered,UIEventType.onDoubleClick);
		UIEventMgr.uiEventManager.UnRegister(OnUIEventTriggered,UIEventType.onSelect);
		UIEventMgr.uiEventManager.UnRegister(OnUIEventTriggered,UIEventType.onDragOver);
		UIEventMgr.uiEventManager.UnRegister(OnUIEventTriggered,UIEventType.onDragOut);
	}


	void OnDestroy()
	{		
		SetTipsActive(false);
	}
}