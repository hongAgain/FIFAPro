using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class Global 
{
    public static Hashtable ShareVars = new Hashtable();  

    public static void AddValue(string name, string value) 
	{
        ShareVars.Add(name, value);
    }

    public static object GetValue(string name) 
	{
        return ShareVars[name];
    }

    public static void RemoveValue(string name) 
	{
        ShareVars.Remove(name);
    }

    public static void ClearShareVars() 
	{
        ShareVars.Clear();
    }
}
