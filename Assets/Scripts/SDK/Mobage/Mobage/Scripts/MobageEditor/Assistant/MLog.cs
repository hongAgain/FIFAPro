using System;
using UnityEngine;

public class MLog:MonoBehaviour
{
	//private static string TAG = "MLog";
	public static string mSpace = "  ";
	//public static bool VERBOSE = false;
	//public static bool DEBUG   = false;
	public static bool VERBOSE = true;
	public static bool DEBUG   = true;
	public static bool INFO    = true;
	public static bool WARN    = true;
	public static bool ERR     = true;
	
	
	public static void v(string t, string m) {
		if (VERBOSE) print(t + mSpace + m);
	}
	public static void v(string t, string m, Exception w) {
		if (VERBOSE) print(t + mSpace + m + mSpace + w);
	}

	public static void d(string t, string m) {
		if (DEBUG) print(t + mSpace + m);
	}
	public static void d(string t, string m, Exception w) {
		if (DEBUG) print(t + mSpace + m + mSpace + w);
	}

	public static void i(string t, string m) {
		if (INFO) print(t + mSpace + m);
	}
	public static void i(string t, string m, Exception w) {
		if (INFO) print(t + mSpace + m + mSpace + w);
	}
	
	public static void w(string t, string m) {
		if (WARN) print(t + mSpace + m);
	}
	public static void w(string t, string m, Exception w) {
		if (WARN) print(t + mSpace + m + mSpace + w);
	}
	
	public static void e(string t, string m) {
		if (ERR) print(t + mSpace + m);
	}
	public static void e(string t, string m, Exception w) {
		if (ERR) print(t + mSpace + m + mSpace + w);
	}

	// internal use only
	private static void setDebugLogEnabled(bool enable) {
		DEBUG = enable;
	}

	// internal use only
	private static void setVerboseLogEnabled(bool enable) {
		VERBOSE = enable;
	}
}

