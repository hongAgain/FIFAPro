//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2015 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;
using LuaInterface;

/// <summary>
/// Tween the widget's size.
/// </summary>

[AddComponentMenu("NGUI/Tween/UILuaTween")]
public class UILuaTween : UITweener
{
    private LuaFunction func;

	protected override void OnUpdate (float factor, bool isFinished)
	{
	    func.Call(factor);
	}

	/// <summary>
	/// Start the tweening operation.
	/// </summary>

    static public UILuaTween Begin(GameObject obj, float duration, LuaFunction luaFunc, LuaFunction onDone)
	{
        UILuaTween comp = UITweener.Begin<UILuaTween>(obj, duration);
	    comp.func = luaFunc;
        comp.onFinished.Clear();
	    EventDelegate.Callback done = delegate()
	    {
	        onDone.Call();
	    };
        comp.onFinished.Add(new EventDelegate(done));

		if (duration <= 0f)
		{
			comp.Sample(1f, true);
			comp.enabled = false;
		}
		return comp;
	}
}
