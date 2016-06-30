using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIEventHandlerFlags
{
	public bool isEventLogicTriggered = false;
	public bool isUIEventMgrSelf = false;
}

public enum UIEventType
{
	onHoverOver,
	onHoverOut,
	onPress,
	onRelease,
	onSelect,
	onDeselect,
	onClick,
	onDoubleClick,
	onDragOver,
	onDragOut
}

public class UIEventMgr : MonoBehaviour {

	public static UIEventMgr uiEventManager
	{
		get
		{
			return _manager;
		}
	}
	private static UIEventMgr _manager = null;

	private System.Action<UIEventHandlerFlags> delegateOnHoverOver = null;
	private System.Action<UIEventHandlerFlags> delegateOnHoverOut = null;
	private System.Action<UIEventHandlerFlags> delegateOnPress = null;
	private System.Action<UIEventHandlerFlags> delegateOnRelease = null;
	private System.Action<UIEventHandlerFlags> delegateOnSelect = null;
	private System.Action<UIEventHandlerFlags> delegateOnDeselect = null;
	private System.Action<UIEventHandlerFlags> delegateOnClick = null;
	private System.Action<UIEventHandlerFlags> delegateOnDoubleClick = null;
	private System.Action<UIEventHandlerFlags> delegateOnDragOver = null;
	private System.Action<UIEventHandlerFlags> delegateOnDragOut = null;

	private Dictionary<UIEventType,System.Action<UIEventHandlerFlags>> delegateTypeMap = new Dictionary<UIEventType, System.Action<UIEventHandlerFlags>>();
	
	bool initDone = false;

	void Awake()
	{
		_manager = this;
	}

	public void Init()
	{
		if(initDone)
			return;

		delegateTypeMap[UIEventType.onHoverOver]	= delegateOnHoverOver;
		delegateTypeMap[UIEventType.onHoverOut]		= delegateOnHoverOut;
		delegateTypeMap[UIEventType.onPress]		= delegateOnPress;
		delegateTypeMap[UIEventType.onRelease] 		= delegateOnRelease;
		delegateTypeMap[UIEventType.onSelect] 		= delegateOnSelect;
		delegateTypeMap[UIEventType.onDeselect]		= delegateOnDeselect;
		delegateTypeMap[UIEventType.onClick]		= delegateOnClick;
		delegateTypeMap[UIEventType.onDoubleClick] 	= delegateOnDoubleClick;
		delegateTypeMap[UIEventType.onDragOver]		= delegateOnDragOver;
		delegateTypeMap[UIEventType.onDragOut] 		= delegateOnDragOut;

		UICamera.genericEventHandler = this.gameObject;
		initDone = true;
	}

	/// <summary>
	/// call by Singlton.getInstance<NGUIEventHandler>().Register(target,eType);
	/// </summary>
	/// <param name="target">Target.</param>
	/// <param name="eType">E type.</param>
	public void Register(System.Action<UIEventHandlerFlags> target , UIEventType eType , bool instantRegister = false)
	{
		Init();
		if(instantRegister)
		{
			RealRegister(target,eType);
		}
		else
		{
			StartCoroutine(WaitToRegister(target,eType));
		}
	}
	
	IEnumerator WaitToRegister(System.Action<UIEventHandlerFlags> target , UIEventType eType)
	{
		yield return 0;
		RealRegister(target,eType);
	}
	
	private void RealRegister(System.Action<UIEventHandlerFlags> target , UIEventType eType)
	{
		delegateTypeMap[eType] += target;
	}
	
	bool isSendingMsg = false;
	
	public void UnRegister(System.Action<UIEventHandlerFlags> target , UIEventType eType)
	{
		StartCoroutine(WaitToUnRegister(target, eType));
	}
	
	IEnumerator WaitToUnRegister(System.Action<UIEventHandlerFlags> target , UIEventType eType)
	{
		while(isSendingMsg)
		{
			yield return 0;
		}
		RealUnRegister(target,eType);
	}
	
	private void RealUnRegister(System.Action<UIEventHandlerFlags> target , UIEventType eType)
	{
		delegateTypeMap[eType] -= target;
	}
	
	#region ==================== Listened events ====================
	
	void OnHover (bool isOver)
	{
		if(HasNoListener(isOver?UIEventType.onHoverOver:UIEventType.onHoverOut))
		{
			return;
		}
		UIEventHandlerFlags flags = GetFlags(UICamera.hoveredObject,"OnHover");
		if(isOver)
		{
			delegateTypeMap[UIEventType.onHoverOver](flags);
		}
		else
		{
			delegateTypeMap[UIEventType.onHoverOut](flags);
		}
	}
	
	void OnPress (bool isDown)
	{
		if(HasNoListener(isDown?UIEventType.onPress:UIEventType.onRelease))
		{
			return;
		}
		UIEventHandlerFlags flags = GetFlags(UICamera.currentTouch.pressed,"OnPress");
		if(isDown)
		{
			delegateTypeMap[UIEventType.onPress](flags);
		}
		else
		{
			delegateTypeMap[UIEventType.onRelease](flags);
		}
	}
	
	void OnSelect (bool selected)
	{
		if(HasNoListener(selected?UIEventType.onSelect:UIEventType.onDeselect))
		{
			return;
		}
		UIEventHandlerFlags flags = GetFlags(UICamera.currentTouch.pressed,"OnSelect");
		if(selected)
		{
			delegateTypeMap[UIEventType.onSelect](flags);
		}
		else
		{
			delegateTypeMap[UIEventType.onDeselect](flags);
		}
	}
	
	void OnClick ()
	{
		if(HasNoListener(UIEventType.onClick))
		{
			return;
		}
		UIEventHandlerFlags flags = GetFlags(UICamera.currentTouch.pressed,"OnClick");
		delegateTypeMap[UIEventType.onClick](flags);
	}
	
	void OnDoubleClick()
	{		
		if(HasNoListener(UIEventType.onDoubleClick))
		{
			return;
		}
		UIEventHandlerFlags flags = GetFlags(UICamera.currentTouch.pressed,"OnDoubleClick");
		delegateTypeMap[UIEventType.onDoubleClick](flags);
	}
	
	void OnDragOver (GameObject draggedObject)
	{		
		if(HasNoListener(UIEventType.onDragOver))
		{
			return;
		}
		UIEventHandlerFlags flags = GetFlags(draggedObject,"OnDragOver");
		delegateTypeMap[UIEventType.onDragOver](flags);
	}
	
	void OnDragOut (GameObject draggedObject)
	{		
		if(HasNoListener(UIEventType.onDragOut))
		{
			return;
		}
		UIEventHandlerFlags flags = GetFlags(draggedObject,"OnDragOut");
		delegateTypeMap[UIEventType.onDragOut](flags);
	}
	
	#endregion
		
	private UIEventHandlerFlags GetFlags(GameObject target , string functionName)
	{
		UIEventHandlerFlags flags = new UIEventHandlerFlags ();
		if(target == null)
		{
			flags.isUIEventMgrSelf = false;
			flags.isEventLogicTriggered = false;
			return flags;
		}
		Component[] components = target.GetComponentsInChildren<Component>(false);
		foreach(Component c in components)
		{
			if(c.GetType().GetMethod(functionName,
			                         System.Reflection.BindingFlags.Instance
			                         |System.Reflection.BindingFlags.NonPublic)!=null)
			{
				if(c is UIEventHandlerFlags)
					flags.isUIEventMgrSelf = true;
				else
					flags.isUIEventMgrSelf = false;
				flags.isEventLogicTriggered = true;
				return flags;
			}
		}
		flags.isUIEventMgrSelf = false;
		flags.isEventLogicTriggered = false;
		return flags;
	}
	
	private bool HasNoListener(UIEventType listenerType)
	{
		return (delegateTypeMap[listenerType] == null);
	}
}
