
using System.Collections;
using System.Collections.Generic;
public class MobageSerializer {
	private static string TAG = "MobageSerializer";
	/*!
	 * @DeSerialize chunk string to Dictionary
	 * @discussion 
	 * @param {string} chunk.
	 * return {Dictionary<string, object>} paramsDic
	 */
	static public Dictionary<string, object>  DeSerialize(string chunk)
    {
		if(chunk == null) return null;
        Dictionary<string, object> paramsDic = new Dictionary<string, object>();
        List<string> info = ToList(chunk);
		//If info is only one item(single value of list),set default key value with "keys"
		string nextName = "keys";
        for(int i = 0; i < info.Count; i++)
        {
             List<string>  strList = ArrayToList(info[i].Split(','));
             string curName= nextName;  
             if (i < info.Count - 1)//The last item hasn't next name
             {
				 nextName = strList[strList.Count - 1];//save next name
                 strList.RemoveAt(strList.Count - 1);//delete next name to get param list.
             }
             //if strList.Count == 0,needn't add it   
             if(strList.Count == 1)
             {
                paramsDic.Add(curName, strList[0]);// add single value
             }
             else if(strList.Count > 1) paramsDic.Add(curName, strList);//add list
        }
		//foreach(string key in paramsDic.Keys)
		//MLog.d(TAG, "DeSerialize:" + key + ":" +paramsDic[key]);
        return paramsDic;
    }
	
	/*!
	 * @Serialize object to jsonString
	 * @discussion 
	 * @param {object} param.
	 * return {string} jsonString
	 */
	static public string Serialize(object param)
	{
		if(param is List<string>) return Serialize(param as List<string>);
		if(param is SortedDictionary<string, object>) return Serialize(param as SortedDictionary<string, object>);
		if(param is string) return Serialize(param as string);
		if(param is int) return param.ToString();
		if(param is float) return param.ToString();
		if(param is double) return param.ToString();
		if(param is bool) return param.ToString();
		else
		{
			MLog.e(TAG, "Can't Serialize :" + param.ToString());
			return null;	
		}
	}
	
	/*!
	 * @Serialize object to jsonString
	 * @discussion 
	 * @param {object} param.
	 * return {string} jsonString
	 */
	static public string RevertToString(object strings)
	{
		
		if(strings is string) return strings as string;
		else if(strings is List<string>)
		{
			string revString = "";
			List<string> parameters = strings as List<string>;
			int count = 0;
			foreach(string s in parameters)
			{
				revString += s;
				if (++count < parameters.Count) revString += ",";
			}
			return revString;
		}
		else 
		{
			MLog.e(TAG, "Can't revert to string!");
			return "";
		}
	}
	
	/*!
	 * @DeSerialize chunk string to  List
	 * @discussion 
	 * @param {string} chunk.
	 * return {List<string>} infoList
	 */
    static private List<string> ToList(string chunk)
    {
    	chunk = chunk.Replace("}{", "},{").Replace("{", "").Replace("}", "").Replace("\"", "");
        List<string> infoList;
		if(chunk.Contains("::"))
		{
			infoList = ArrayToList(chunk.Split(new string[]{"::"}, System.StringSplitOptions.None));
		}
		else if(chunk.Contains(":")) 
		{
			infoList = ArrayToList(chunk.Split(':'));
		}
        else  infoList = ArrayToList(new string[]{chunk});
        return infoList;
    }
 
    
	/*!
	 * @Convert Array to  List
	 */
	static private List<string> ArrayToList(string[] infoArray)
	{
		List<string> infoList = new List<string>();
		foreach(string info in infoArray) infoList.Add(info);
		return infoList;
	}
	
	/*!
	 * @Serialize string
	 */
	static private string Serialize(string key)
    {
        string result = "\"" + key + "\"";
        return result;  
    }
	
	/*!
	 * @Serialize List
	 */	
    static private string Serialize(List<string> fields)
    {
        string keys ="[";
		int count = 0;
        foreach (string key in fields)
        {
            keys += Serialize(key);
			if (++count < fields.Count)  keys += ",";
        }
        keys += "]";
        return keys;
    }
	
	/*!
	 * @Serialize SortedDictionary
	 */	
    static private string Serialize(SortedDictionary<string, object> parameters)
    {
        string param = "{";
        int count = 0;
        foreach (string key in parameters.Keys)
        {
            param += Serialize(key);
            param += ":";
            param += Serialize(parameters[key]);
            if (++count < parameters.Count) param += ",";
        }
        param += "}";
        return param;
    }
}



