using System;
using System.Reflection;

public class PTools
{
	public static void SetMLogDebug(bool b)
	{
		BindingFlags flag = BindingFlags.Static | BindingFlags.Public;
		FieldInfo f_key = typeof(MLog).GetField("DEBUG", flag);
		f_key.SetValue(null, b);
	}
}


