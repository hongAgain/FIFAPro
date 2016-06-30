using UnityEngine;
using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using LuaInterface;
using Common;
using Common.Log;

public class Util 
{
	public static System.Type GetType(string classname) {
		System.Reflection.Assembly assb = System.Reflection.Assembly.GetExecutingAssembly();
		System.Type t = null;
		t = assb.GetType(classname); ;
		if (t == null) {
			t = assb.GetType(classname);
		}
		return t;
	}
	
	public static Component GetComponentInChildren(GameObject obj, string classname) 
	{
		System.Type t = GetType(classname);
		Component comp = null;
		if (t != null && obj != null) comp = obj.GetComponentInChildren(t);
		return comp;
	}
	
	public static Component GetComponent(GameObject obj, string classname) 
	{
		if (obj == null) return null; 
		return obj.GetComponent(classname);
	}

	public static Component GetMissingComponent(GameObject obj, string classname) 
	{
		if (obj == null) return null; 
		Component temp = obj.GetComponent(classname);
		if(temp == null)
		{
			return obj.AddComponent(classname);
		}
		else
		{
			return temp;
		}
	}

	public static Component[] GetComponentsInChildren(GameObject obj, string classname) 
	{
		System.Type t = GetType(classname);
		if (t != null && obj != null) return obj.transform.GetComponentsInChildren(t);
		return null;
	}
	
	public static Transform[] GetAllChild(GameObject obj) 
	{
		Transform[] child = null;
		int count = obj.transform.childCount;
		child = new Transform[count];
		for (int i = 0; i < count; i++) {
			child[i] = obj.transform.GetChild(i);
		}
		return child;
	}

    public static int Int(float o) 
	{
        int factor = o >= 0 ? 1 : -1;
        return (int)(Math.Abs(o) + 0.5f) * factor;
    }

    public static float Float(object o) 
	{
        return (float)Math.Round(Convert.ToSingle(o), 2);
    }

    public static long Long(object o) 
	{
        return Convert.ToInt64(o);
    }

    public static int Random(int min, int max) 
	{
        return UnityEngine.Random.Range(min, max);
    }

    public static float Random(float min, float max) 
	{
        return UnityEngine.Random.Range(min, max);
    }

    public static string Uid(string uid) 
	{
        int position = uid.LastIndexOf('_');
        return uid.Remove(0, position + 1);
    }

    public static long GetTime() 
	{ 
        //TimeSpan ts = new TimeSpan(DateTime.UtcNow.Ticks - new DateTime(1970, 1, 1, 0, 0, 0).Ticks);
        TimeSpan ts = new TimeSpan(LuaServerTime.Now.Ticks - new DateTime(1970, 1, 1, 0, 0, 0).ToLocalTime().Ticks);
        return (long)ts.TotalMilliseconds;
    }

	public static string GetTimeToString(double targetTimeStamp)
	{
		DateTime date = (new DateTime (1970,1,1,0,0,0,0).ToLocalTime().AddMilliseconds(targetTimeStamp));
		return date.Year+"-"+date.Month+"-"+date.Day;
	}

    public static double GetTotalSeconds(long time_)
    {
        //TimeSpan ts = DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0, 0).ToLocalTime().AddMilliseconds(time_);
        TimeSpan ts = LuaServerTime.Now - new DateTime(1970, 1, 1, 0, 0, 0, 0).ToLocalTime().AddMilliseconds(time_);
        return ts.TotalSeconds;
    }

    public static void ShowTestTime(long time_)
    {
        //Debug.Log("Server Time: " + new DateTime(1970, 1, 1, 0, 0, 0, 0).ToLocalTime().AddMilliseconds(time_).ToString());
        //Debug.Log("Date.Now Time: " + DateTime.Now.ToString());
        //Debug.Log("Server Date.Now Time: " + LuaServerTime.Now.ToString());
    }
	//not safe
	public static int GetLocalYear()
	{
		return DateTime.Now.Year;
	}

	//not safe
	public static int GetLocalMonth()
	{
		return DateTime.Now.Month;
	}

	//0-6 :sunday - saturday
	public static int GetLocalDayInWeek()
	{
		return (int)DateTime.Now.DayOfWeek;
	}

	//not safe
	public static int GetLocalDayInMonth()
	{
		return DateTime.Now.Day;
	}

	public static int GetLocalHourInDay()
	{
		return DateTime.Now.Hour;
	}

	public static int GetYearFromTimeStamp(double targetTimeStamp)
	{
		return (new DateTime (1970,1,1,0,0,0,0).ToLocalTime().AddMilliseconds(targetTimeStamp)).Year;
	}
		
	public static int GetMonthFromTimeStamp(double targetTimeStamp)
	{
		return (new DateTime (1970,1,1,0,0,0,0).ToLocalTime().AddMilliseconds(targetTimeStamp)).Month;
	}
//	0-6 : sunday - saturday
	public static int GetDayInWeekFromTimeStamp(double targetTimeStamp)
	{
		return (int)(new DateTime (1970,1,1,0,0,0,0).ToLocalTime().AddMilliseconds(targetTimeStamp)).DayOfWeek;
	}

	public static int GetDayInMonthFromTimeStamp(double targetTimeStamp)
	{
		return (new DateTime (1970,1,1,0,0,0,0).ToLocalTime().AddMilliseconds(targetTimeStamp)).Day;
	}
	
	public static int GetYearAfterAddDay(double targetTimeStamp,int daysToAdd)
	{
		return (new DateTime (1970,1,1,0,0,0,0).ToLocalTime().AddMilliseconds(targetTimeStamp)).AddDays(daysToAdd).Year;
	}

	public static int GetMonthAfterAddDay(double targetTimeStamp,int daysToAdd)
	{
		return (new DateTime (1970,1,1,0,0,0,0).ToLocalTime().AddMilliseconds(targetTimeStamp)).AddDays(daysToAdd).Month;
	}

	public static int GetDayInMonthAfterAddDay(double targetTimeStamp,int daysToAdd)
	{
		return (new DateTime (1970,1,1,0,0,0,0).ToLocalTime().AddMilliseconds(targetTimeStamp)).AddDays(daysToAdd).Day;
	}

	public static int GetDayInYearAfterAddDay(double targetTimeStamp,int daysToAdd)
	{
		return (new DateTime (1970,1,1,0,0,0,0).ToLocalTime().AddMilliseconds(targetTimeStamp)).AddDays(daysToAdd).DayOfYear;
	}

	public static string GetPassedTimeMeasurement(double targetTimeStamp)
	{
		TimeSpan delta = System.DateTime.Now - (new DateTime (1970,1,1,0,0,0,0).ToLocalTime().AddMilliseconds(targetTimeStamp));
		if(delta.Days > 0)
		{
			return string.Format(LocalizeString("DaysAgo"),delta.Days);
		}
		else if(delta.Hours > 0)
		{			
			return string.Format(LocalizeString("HoursAgo"),delta.Hours);
		}
		else
		{
			return LocalizeString("JustNow");
		}
	}

//	private static int GetTargetDayInMonth(DateTime targetDate)
//	{
//		int thisYear = targetDate.Year;
//		int thisMonth = targetDate.Month;
//		int daysInPassedMonthes=0;
//		for (int i=1;i < thisMonth;i++)
//			daysInPassedMonthes += DateTime.DaysInMonth(thisYear,i);
//		return targetDate.DayOfYear - daysInPassedMonthes;
//	}

    public static T Get<T>(GameObject go, string subnode) where T : Component 
	{
        if (go != null) {
            Transform sub = go.transform.FindChild(subnode);
            if (sub != null) return sub.GetComponent<T>();
        }
        return null;
    }


    public static T Get<T>(Transform go, string subnode) where T : Component 
	{
        if (go != null) {
            Transform sub = go.FindChild(subnode);
            if (sub != null) return sub.GetComponent<T>();
        }
        return null;
    }


    public static T Get<T>(Component go, string subnode) where T : Component
	{
        return go.transform.FindChild(subnode).GetComponent<T>();
    }


    public static T Add<T>(GameObject go) where T : Component 
	{
        if (go != null) {
            T[] ts = go.GetComponents<T>();
            for (int i = 0; i < ts.Length; i++ ) {
                if (ts[i] != null) Component.Destroy(ts[i]);
            }
            return go.gameObject.AddComponent<T>();
        }
        return null;
    }

    public static T Add<T>(Transform go) where T : Component 
	{
        return Add<T>(go.gameObject);
    }

    public static GameObject Child(GameObject go, string subnode) 
	{
        return Child(go.transform, subnode);
    }



    public static GameObject Child(Transform go, string subnode) 
	{
        Transform tran = go.FindChild(subnode);
        if (tran == null) return null;
        return tran.gameObject;
    }



    public static GameObject Peer(GameObject go, string subnode) 
	{
        return Peer(go.transform, subnode);
    }

    public static GameObject Peer(Transform go, string subnode) 
	{
        Transform tran = go.parent.FindChild(subnode);
        if (tran == null) return null;
        return tran.gameObject;
    }



    public static string Encode(string message) 
	{
        byte[] bytes = Encoding.GetEncoding("utf-8").GetBytes(message);
        return Convert.ToBase64String(bytes);
    }


    public static string Decode(string message) 
	{
        byte[] bytes = Convert.FromBase64String(message);
        return Encoding.GetEncoding("utf-8").GetString(bytes);
    }

    public static bool IsNumeric(string str) 
	{
        if (str == null || str.Length == 0) return false;
        for (int i = 0; i < str.Length; i++ ) {
            if (!Char.IsNumber(str[i])) { return false; }
        }
        return true;
    }

    public static string HashToMD5Hex(string sourceStr) 
	{
        byte[] Bytes = Encoding.UTF8.GetBytes(sourceStr);
        using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider()) {
            byte[] result = md5.ComputeHash(Bytes);
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
                builder.Append(result[i].ToString("x2"));
            return builder.ToString();
        }
    }

    public static string md5(string source) 
	{
        MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
        byte[] data = System.Text.Encoding.UTF8.GetBytes(source);
        byte[] md5Data = md5.ComputeHash(data, 0, data.Length);
        md5.Clear();
        string destString = "";
        for (int i = 0; i < md5Data.Length; i++)
        {
            destString += System.Convert.ToString(md5Data[i], 16).PadLeft(2, '0');
        }
        destString = destString.PadLeft(32, '0');
        return destString;
    }

    public static string md5file(string file) 
	{
        try {
            FileStream fs = new FileStream(file, FileMode.Open);
            System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] retVal = md5.ComputeHash(fs);
            fs.Close();

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < retVal.Length; i++) {
                sb.Append(retVal[i].ToString("x2"));
            }
            return sb.ToString();
        } catch (Exception ex) {
            throw new Exception("md5file() fail, error:" + ex.Message);
        }
    }

    public static void CompressFile(string infile, string outfile) 
	{
        //Stream gs = new GZipOutputStream(File.Create(outfile));
        //FileStream fs = File.OpenRead(infile);
        //byte[] writeData = new byte[fs.Length];
        //fs.Read(writeData, 0, (int)fs.Length);
        //gs.Write(writeData, 0, writeData.Length);
        //gs.Close(); fs.Close();
    }


    public static string DecompressFile(string infile) 
	{
        //string result = string.Empty;
        //Stream gs = new GZipInputStream(File.OpenRead(infile));
        //MemoryStream ms = new MemoryStream();
        //int size = 2048;
        //byte[] writeData = new byte[size]; 
        //while (true) {
        //    size = gs.Read(writeData, 0, size); 
        //    if (size > 0) {
        //        ms.Write(writeData, 0, size); 
        //    } else {
        //        break; 
        //    }
        //}
        //result = new UTF8Encoding().GetString(ms.ToArray());
        //gs.Close(); ms.Close();
        //return result;
        return "";
    }


    public static string Compress(string source) 
	{
        //byte[] data = Encoding.UTF8.GetBytes(source);
        //MemoryStream ms = null;
        //using (ms = new MemoryStream())
        //{
        //    using (Stream stream = new GZipOutputStream(ms))
        //    {
        //        try
        //        {
        //            stream.Write(data, 0, data.Length);
        //        }
        //        finally
        //        {
        //            stream.Close();
        //            ms.Close();
        //        }
        //    }
        //}
        //return Convert.ToBase64String(ms.ToArray());
        return null;
    }


    public static string Decompress(string source) 
	{
        //string result = string.Empty;
        //byte[] buffer = null;
        //try {
        //    buffer = Convert.FromBase64String(source);
        //} catch {
        //}
        //using (MemoryStream ms = new MemoryStream(buffer)) {
        //    using (Stream sm = new GZipInputStream(ms)) {
        //        StreamReader reader = new StreamReader(sm, Encoding.UTF8);
        //        try {
        //            result = reader.ReadToEnd();
        //        } finally {
        //            sm.Close();
        //            ms.Close();
        //        }
        //    }
        //}
        //return result;
        return "";
    }

    public static void ClearChild(Transform go) 
	{
        if (go == null) return;
        for (int i = go.childCount - 1; i >= 0; i--) 
		{
            UnityEngine.GameObject.Destroy(go.GetChild(i).gameObject);
        }
    }


    public static int GetInt(string key) 
	{
		return PlayerPrefs.GetInt(key);
    }

 
    public static bool HasKey(string key) 
	{
		return PlayerPrefs.HasKey(key);
    }


    public static void SetInt(string key, int value) 
	{
		PlayerPrefs.SetInt(key, value);
    }


    public static string GetString(string key) 
	{
		return PlayerPrefs.GetString(key);
    }

    public static void SetString(string key, string value) 
	{
		PlayerPrefs.SetString(key, value);
    }

 
    public static void RemoveData(string key) 
	{
		PlayerPrefs.DeleteKey(key);
    }


	public static void CallGC()
	{
		GC.Collect(); 
	}

	public static void CallUnloadUnusedAssets()
	{
		Resources.UnloadUnusedAssets();
	}


    public static bool IsNumber(string strNumber) {
        Regex regex = new Regex("[^0-9]");
        return !regex.IsMatch(strNumber);
    }
	
    public static Uri AppContentDataUri {
        get {
            string dataPath = Application.dataPath;
            if (Application.platform == RuntimePlatform.IPhonePlayer) {
                var uriBuilder = new UriBuilder();
                uriBuilder.Scheme = "file";
                uriBuilder.Path = Path.Combine(dataPath, "Raw");
                return uriBuilder.Uri;
            } else if (Application.platform == RuntimePlatform.Android) {
                return new Uri("jar:file://" + dataPath + "!/assets");
            } else {
                var uriBuilder = new UriBuilder();
                uriBuilder.Scheme = "file";
                uriBuilder.Path = Path.Combine(dataPath, "StreamingAssets");
                return uriBuilder.Uri;
            }
        }
    }

    public static string GetFileText(string path) 
	{
        return File.ReadAllText(path);
    }


    public static bool NetAvailable 
	{
        get {
            return Application.internetReachability != NetworkReachability.NotReachable;
        }
    }

    public static bool IsWifi 
	{
        get {
            return Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork;
        }
    }

    public static string AppContentPath() 
	{
        string path = string.Empty;
        switch (Application.platform) {
            case RuntimePlatform.Android:
                path = "jar:file://" + Application.dataPath + "!/assets/";
            break;
            case RuntimePlatform.IPhonePlayer:
                path = Application.dataPath + "/Raw/";
            break;
            default:
                path = "file://" + Application.dataPath + "/StreamingAssets/";
            break;
        }
        return path;
    }

	public static void EnableScript(GameObject go, string classname, bool willEnable)
	{
		if (go == null) return;
		MonoBehaviour mb = (go.GetComponent(classname) as MonoBehaviour);
		if(mb!=null)
			mb.enabled = willEnable;
	}

    public static UIEventListener AddClick(GameObject go, System.Object luafuc) 
	{
        UIEventListener listener = UIEventListener.Get(go);
        listener.onClick += delegate(GameObject o)
        {
            LuaInterface.LuaFunction func = (LuaInterface.LuaFunction)luafuc;
            func.Call(go);
        };
        return listener;
    }

    public static UIEventListener AddPress(GameObject go, System.Object luafuc)
    {
        UIEventListener listener = UIEventListener.Get(go);
        listener.onPress += delegate(GameObject o, bool isPressed)
        {
            LuaInterface.LuaFunction func = (LuaInterface.LuaFunction)luafuc;
            func.Call(go,isPressed);
        };
        return listener;
    }

    public static UIEventListener ChangeClick(GameObject go, System.Object luafunc)
    {
        UIEventListener listener = UIEventListener.Get(go);
        listener.onClick = delegate(GameObject o)
        {
            LuaInterface.LuaFunction func = (LuaInterface.LuaFunction)luafunc;
            func.Call(go);
        };
        return listener;
    }

    public static UIEventListener AddDragDrop(GameObject go, System.Object onDragStart, System.Object onDrag, System.Object onDrop, System.Object onDragEnd, System.Object param)
    {
        var old = go.GetComponent<UIEventListener>();
        if (old != null) UnityEngine.Object.Destroy(old);

        var listener = go.AddComponent<UIEventListener>();
        listener.onDragStart += delegate(GameObject o)
        {
            LuaInterface.LuaFunction func = (LuaInterface.LuaFunction)onDragStart;
            func.Call(o);
        };
        listener.onDrag += delegate(GameObject o, Vector2 delta)
        {
            LuaInterface.LuaFunction func = (LuaInterface.LuaFunction)onDrag;
            var root = NGUITools.FindInParents<UIRoot>(go);
            func.Call(o, delta * root.pixelSizeAdjustment);
        };
        listener.onDrop += delegate(GameObject dragObj, GameObject dropOnObj)
        {
            LuaInterface.LuaFunction func = (LuaInterface.LuaFunction)onDrop;
            func.Call(dragObj, dropOnObj);
        };
        listener.onDragEnd += delegate(GameObject o)
        {
            LuaInterface.LuaFunction func = (LuaInterface.LuaFunction)onDragEnd;
            func.Call(o);
        };
        listener.parameter = param;
        return listener;
    }

	public static void RmvEventListener(GameObject go) 
	{
		UIEventListener listener = go.GetComponent<UIEventListener>();
		if(listener!=null)
		{
			GameObject.Destroy(listener);
		}
	}

    public static bool isLogin 
	{
        get { return Application.loadedLevelName.CompareTo("login") == 0; }
    }

	
    public static string LuaPath(string name) 
	{
        if (name.EndsWith(".lua")) {
            return Application.dataPath + "/lua/" + name;
        }
        return Application.dataPath + "/lua/" + name + ".lua";
    }

    public static string GetLuaScriptModuleName(string scriptname)
    {
        int iIdx = scriptname.LastIndexOf('/');
        if(-1 == iIdx)
            return scriptname + ".";
        String strRet = scriptname.Substring(iIdx+1);
        return strRet + ".";
    }

    public static string GetLuaFilePath(string scriptname)
    {
        return scriptname;
        //if (scriptname.EndsWith(".lua"))
        //{
        //    return scriptname;
        //}
        //else
        //{
        //    return scriptname + ".lua";
        //}
    }

	public static Vector3 WorldToNGUIPixel(Camera camera, Vector3 pos)
	{
		Vector3 screenPos = camera.WorldToScreenPoint(pos);
		float x = screenPos.x - Screen.width * 0.5f;
		float y = screenPos.y - Screen.height * 0.5f;
		return new Vector3(x, y, 0.0f);
	}
	
	public static Vector3 WorldToNGUIScale(Camera camera, Vector3 pos)
	{
		Vector3 screenPos = camera.WorldToScreenPoint(pos);
		float horizScale = (float)Screen.width / (float)Screen.height;
		float x = (1.0f + (screenPos.x - Screen.width) / Screen.width * 2) * horizScale;
		float y = 1.0f + (screenPos.y - Screen.height) / Screen.height * 2;
		return new Vector3(x, y, 0.0f);
	}
	
	public static Vector3 ScreenToNGUIScale(Vector3 screenPos)
	{
		float horizScale = (float)Screen.width / (float)Screen.height;
		float x = (1.0f + (screenPos.x - Screen.width) / Screen.width * 2) * horizScale;
		float y = 1.0f + (screenPos.y - Screen.height) / Screen.height * 2;
		return new Vector3(x, y, 0.0f);
    }

    public static Hashtable GetHashTable()
    {
        return new Hashtable();
    }

    public static void FillHashTable(Hashtable hashtable, object key, object value)
    {
        hashtable.Add(key, value);
    }

    public static bool MatchURL(string s)
    {
        string Pattern = @"^(http|https|ftp)\://[a-zA-Z0-9\-\.]+\.[a-zA-Z]{2,3}(:[a-zA-Z0-9]*)?/?([a-zA-Z0-9\-\._\?\,\'/\\\+&$%\$#\=~])*$";
        Regex urlRegex = new Regex(Pattern);

        return urlRegex.IsMatch(s);
    }

    public static string UriEscapeDataString(string data)
    {
        return System.Uri.EscapeDataString(data);
    }

    public static void Log(object o)
    {
        if (o != null)
        {
            LogManager.Instance.Log(o.ToString());
            //LogView.Log(o.ToString());
        }
    }

    public static void LogWarning(object o)
    {
        if (o != null)
        {
            LogManager.Instance.LogWarning(o.ToString());
            //LogView.Warn(o.ToString());
        }
    }

    public static void LogError(object o)
    {
		if (o != null)
        {
            LogManager.Instance.LogError(o.ToString());
            //LogView.Error(o.ToString());
        }
    }

    

    public static void MarkAsChanged(GameObject go)
    {
        UIRect[] rects = go.GetComponentsInChildren<UIRect>();
        for (int i = 0, imax = rects.Length; i < imax; ++i)
            rects[i].ParentHasChanged();
    }

    public static void ForceQuitGame()
    {
        Application.Quit();
    }

    public static string LocalizeString(string strKey_)
    {
        return Localization.Get(strKey_);
    }

    public static void CreateLobbyPlayer(string teamId, LuaTable heroDatas)
    {
        AvatarManager.Instance.CreateLobbyPlayers(teamId, heroDatas);
	}

    public static void CreatePerson(string modelId, string headId, string teamId, string shoesId,
        Color skin, int actionCollection, string defaultAnim, LuaFunction onCreate,String strPerfab)
    {
        //commented by dracula_jin 
        //var clone = AvatarManager.Instance.CreateCRonaldo(modelId, headId, headId,
        //    string.Format("hose_{0}1", teamId),
        //    string.Format("shirt_{0}1", teamId),
        //    shoesId, alpha, skin, actionCollection, defaultAnim);

        GameObject kGameObj = AvatarManager.Instance.CreateRoleObject(System.Convert.ToInt32(headId), strPerfab, System.Convert.ToInt32(teamId),true);
        Animation kAnimation = kGameObj.GetComponentInChildren<Animation>();
        if (null != kAnimation)
        {
            kAnimation.clip = kAnimation.GetClip(defaultAnim);
            kAnimation.Play();
        }
        if (onCreate != null)
        {
            onCreate.Call(kGameObj);
        }
        //return;

        //GameObject bodyPrefab = null;
        //AssetMgr.OnGetRes onLoadBody = delegate(AssetBundle ab)
        //{
        //    bodyPrefab = ab.Load(modelId, typeof(GameObject)) as GameObject;
        //};
        //AssetMgr.LoadAsset("Body.assetbundle", onLoadBody);

        //GameObject headPrefab = null;
        //Texture headTex = null;
        //AssetMgr.OnGetRes onLoadHead = delegate(AssetBundle ab)
        //{
        //    headPrefab = ab.Load("Head", typeof(GameObject)) as GameObject;
        //    headTex = ab.Load(headId, typeof(Texture)) as Texture;
        //};
        //AssetMgr.LoadAsset(string.Format("Head/{0}.assetbundle", headId), onLoadHead);

        //Texture alpha = null;
        //AssetMgr.OnGetRes onLoadAlpha = delegate(AssetBundle ab)
        //{
        //    alpha = ab.Load("alpha01", typeof(Texture)) as Texture;
        //};
        //AssetMgr.LoadAsset("Uniform/alpha.assetbundle", onLoadAlpha);

        //AssetMgr.OnGetRes onLoadTexture = delegate(AssetBundle ab)
        //{
        //    Texture shirt = ab.Load(string.Format("shirt_{0}1", teamId)) as Texture;
        //    Texture shoes = ab.Load(string.Format("shoes_{0}1", teamId)) as Texture;
        //    Texture hose = ab.Load(string.Format("hose_{0}1", teamId)) as Texture;

        //    GameObject person = PrepareMatch.CreatePerson(bodyPrefab, headPrefab, shirt, shoes, hose, headTex, alpha);
        //    person.transform.position = new Vector3(-6.57f, -0.79f, -0.13f);
        //    person.transform.localEulerAngles = new Vector3(0, 276.11f, 0);
        //    if (onCreate != null)
        //        onCreate.Call(person);
        //};
        //AssetMgr.LoadAsset(string.Format("Uniform/{0}.assetbundle", teamId), onLoadTexture);
    }

	public static void CreateCoach(string headId, LuaFunction onCreate, string defaultName, String animePerfab)
	{
		GameObject kGameObj = AvatarManager.Instance.CreateCoachObject(System.Convert.ToInt32(headId), animePerfab);
		Animation kAnimation = kGameObj.GetComponentInChildren<Animation>();
	    if (null != kAnimation)
	    {
	        kAnimation.clip = kAnimation.GetClip(defaultName);
            kAnimation.Play();   
	    }
		if (onCreate != null)
		{
			onCreate.Call(kGameObj);
		}
	}

	public static void CombineSkinnedMeshRenderer(Transform targetTransform,LuaInterface.LuaFunction luafuc = null)
	{
//        SkinnedMeshCombiner combiner = targetTransform.gameObject.AddComponent<SkinnedMeshCombiner>();
////		SkinnedMeshCombiner_SG combiner = targetTransform.gameObject.AddComponent<SkinnedMeshCombiner_SG>();
//        combiner.Initialize();
//        combiner.CombineSkinnedMesh((transNode,animNode)=>{luafuc.Call(transNode,animNode);});

        CombineLobbyPlayer.Combine(targetTransform);
        var anim = targetTransform.gameObject.GetComponentInChildren<Animation>();
        luafuc.Call(targetTransform, anim);
	}

	public static void PlayAnimation(Transform animationTrans,string animationName,bool isLoop)
	{
		Animation ani = animationTrans.GetComponent<Animation>();
		if(ani!=null)
		{
			string previewsAnimName = "";
			if(!isLoop)
			{
				if(ani.isPlaying)
				{
					float currentAnimWeight = -1f;
					//find out the heaviest played anim clip
					foreach(AnimationState a in ani)
					{
						if(a.enabled && a.weight > currentAnimWeight)
						{
							currentAnimWeight = a.weight;
							previewsAnimName = a.name;
						}
					}
				}
			}

//			ani.wrapMode = isLoop?WrapMode.Loop:WrapMode.Once;
			ani.CrossFade(animationName);
			if(previewsAnimName!="" && !isLoop)
			{
				ani.CrossFadeQueued(previewsAnimName);
			}
		}
	}

	public static void StartAnimAtRandomTime(Transform animationTrans,string animationName,bool isLoop)
	{
		Animation ani = animationTrans.GetComponent<Animation>();
		if(ani!=null)
		{
			

			AnimationState state = ani[animationName];
            if (null == state)
                return;
            ani.Play(animationName);
			state.enabled = true;
			state.weight = 1;
			state.speed = Random(0.9f,1.1f);
			state.normalizedTime = Random(0f,1f);
			
			ani.Sample();
		}
	}

	public static void InitializeUICircleListItem(Transform trans)
	{
		UICircleListItem item = trans.GetComponent<UICircleListItem>();
		if(item!=null)
			item.Initialize();
	}

    public static void SetUITexture(Transform transform, string texLoader, string texName, bool pixelPerfect)
    {
        var uitexture = transform.GetComponent<UITexture>();
        if (uitexture != null)
        {
            var obj = ResourceManager.Instance.LoadTexture("Textures/ScatteredImg/" + texLoader + "/" + texName);

            if (obj != null)
            {
                uitexture.mainTexture = obj as Texture;
                if (pixelPerfect)
                {
                    uitexture.MakePixelPerfect();
                }
            }
        }
    }

    public static GameObject GetGameObject(string name)
    {
        return ResourceManager.Instance.Load("UI/UICommon/" + name) as GameObject;
    }

    public static void ChangeUniform(GameObject go, string teamId)
    {
        go.SendMessage("ChangeUniform", teamId);
    }

    public static void ChangeUniformImmidiately(GameObject go, string teamId)
    {
        go.SendMessage("ChangeUniformImmidiately", teamId);
    }

	public static void SetLight(Transform lightTrans,Color lightColor,float lightIntensity)
	{
		Light l = lightTrans.GetComponent<Light>();
		if(l!=null)
		{
			l.color = lightColor;
			l.intensity = lightIntensity;
		}
	}

	public static void SetAmbientLight(Color lightColor)
	{
		RenderSettings.ambientLight = lightColor;
	}

    public static void SetFog(bool willUse)
    {
        RenderSettings.fog = willUse;
    }

    public static void DisableLightMap()
    {
        LightmapSettings.lightmaps = null;
    }

    public static void LoadLevel(string levelName)
    {
        SceneMgr.LoadLevel(levelName);
    }

    public static GameObject ChangeLevelState(string levelStateName, Vector3 atPos)
    {
        return SceneMgr.Instance.ChangeLevelState(levelStateName, atPos);
    }

    public static GameObject CreateLevelState(string levelStateName)
    {
        return SceneMgr.Instance.CreateLevelState(levelStateName);
    }

    public static void ClearLevelState()
    {
        SceneMgr.Instance.ClearLevelState();
    }

    public static void SyncObjPos(GameObject lookAtTarget, GameObject syncTarget,
                                    Camera shotLookAtCamera, Camera shotSyncCamera)
    {
        SyncObj.Sync(lookAtTarget, syncTarget, shotLookAtCamera, shotSyncCamera);
    }

    public static void SetJinJieLevel(GameObject go, int stage)
    {
        var jinjie = go.GetComponent<JinJie>();
        jinjie.stage = stage;
    }

	public static int GetLuaBehaviourParamNum(Transform hintTrans)
	{
		LuaBehaviour lb = hintTrans.GetComponent<LuaBehaviour>();
		return lb.luaScriptParams.Count;
	}

    public static void SetAutoActive(Transform tf_, int activeNum_)
    {
        var auto = tf_.GetComponent<AutoActive>();
        if (auto != null)
        {
            auto.activeCount = activeNum_;
        }
    }

    public static GameObject AddChild(GameObject parent, GameObject prefab)
    {
        GameObject go = prefab;
        if (go != null && parent != null)
        {
            Transform t = go.transform;

            Vector3 lp = t.localPosition;
            Quaternion lr = t.localRotation;
            Vector3 ls = t.localScale;

            t.parent = parent.transform;
            t.localPosition = lp;
            t.localRotation = lr;
            t.localScale = ls;
            //go.layer = parent.layer;
        }
        return go;
    }

    public static void DetachHUD(GameObject kObj)
    {
        if (null != kObj)
            GameObject.Destroy(kObj);
    }

    public static GameObject AttachHUD(string strPrefab, GameObject kTarget)
    {
        GameObject kHUDObj = Util.AddChild(HUDRoot.Go, ResourceManager.Instance.Load(strPrefab, true) as GameObject);

        UIFollowTarget kFollowTarget = kHUDObj.GetComponent<UIFollowTarget>();
        if (kFollowTarget == null)
        {
            kFollowTarget = kHUDObj.AddComponent<UIFollowTarget>();
        }
        kFollowTarget.target = kTarget.transform;

        return kHUDObj;
    }

    public static GameObject Attach2D(string strPrefab, GameObject kTarget)
    {
        if (null == HUDRoot.Go)
            return null;
        Transform kTransform = HUDRoot.Go.transform.FindChild("Match2D");
        if (null == kTransform)
            return null;
        GameObject kHUDObj = Util.AddChild(kTransform.gameObject, ResourceManager.Instance.Load(strPrefab, true) as GameObject);

        UI3Dto2D kFollowTarget = kHUDObj.GetComponent<UI3Dto2D>();
        if (kFollowTarget == null)
        {
            kFollowTarget = kHUDObj.AddComponent<UI3Dto2D>();
        }
        kFollowTarget.isBall = false;
        kFollowTarget.target = kTarget.transform;
        kFollowTarget.tfArrow = kHUDObj.transform.FindChild("Arrow");

        return kHUDObj;
    }

    public static GameObject Attach2DBall(string strPrefab, GameObject kTarget)
    {
        if (null == HUDRoot.Go)
            return null;
        Transform kTransform = HUDRoot.Go.transform.FindChild("Match2D");
        if (null == kTransform)
            return null;
        GameObject clone = Util.AddChild(kTransform.gameObject, ResourceManager.Instance.Load(strPrefab, true) as GameObject);

        UI3Dto2D kFollowTarget = clone.GetComponent<UI3Dto2D>();
        if (kFollowTarget == null)
        {
            kFollowTarget = clone.AddComponent<UI3Dto2D>();
        }
        kFollowTarget.target = kTarget.transform;
        kFollowTarget.isBall = true;

        return clone;
    }

    public static void TestinExceptionLog(string strLog_)
    {
        TestinMgr.Instance.TestinExceptionLog(new System.Exception(strLog_));
    }

    public static void SetShaderPropertiesInt(GameObject objGame,string path,string strVar,int iValue)
    {
        var obj = objGame.GetComponent<UITexture>();
        if (obj != null)
        {
            obj.material.SetInt(strVar, iValue);
        }
    }

    public static void SetParentAndAllChildrenToLayer(Transform kTransform, string strLayerName)
    {
        SetParentAndAllChildrenToLayer(kTransform, LayerMask.NameToLayer(strLayerName));
    }
    public static void SetParentAndAllChildrenToLayer(Transform kTransform, int iLayer)
    {
        kTransform.gameObject.layer = iLayer;
        SetAllChildrenToLayer(kTransform, iLayer);
    }
    public static void SetAllChildrenToLayer(Transform kTransform, int iLayer)
    {
        for (int i = 0; i < kTransform.childCount; ++i)
        {
            GameObject child = kTransform.GetChild(i).gameObject;
            child.layer = iLayer;
            SetAllChildrenToLayer(child.transform, iLayer);
        }
    }
    public static string FormatString(string str, params string[] args )
    {
        return string.Format(str, args);
    }
    /*
     * 
     */

}