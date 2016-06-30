using System.IO;
using Common.Log;
using LitJson;
using System.Collections.Generic;

namespace Common.Tables
{
    public class TableUtil
    {
        public static Vector3D GetVector3(string strInVal, char splitChar)
        {
            string[] strList = strInVal.Split(splitChar);
            if( 3 != strList.Length )
                return Vector3D.zero;
            return new Vector3D(float.Parse(strList[0]),float.Parse(strList[1]),float.Parse(strList[2]));
        }

        public static bool GetBoolean(string strInVal)
        {

            int iVal = int.Parse(strInVal);
            if (iVal != 0)
                return true;
            return false;
        }

#if FIFA_CLIENT
        public static void GetColor(string strInVal, char splitChar,ref UnityEngine.Color kColor)
        {
            string[] strList = strInVal.Split(splitChar);
            if( 3 != strList.Length )
            {
                kColor = UnityEngine.Color.white*210/255;
            }
            else    
                kColor  = new UnityEngine.Color(float.Parse(strList[0]),float.Parse(strList[1]),float.Parse(strList[2]))/255;
        }
#endif
    }

    public interface Table
    {
        bool BuildTable(string strContent);
        Dictionary<string,string> GetItem(string strKey);
        string GetValue(string strItemKey,string strValKey);
        Vector3D GetVector3D(string strItemKey, string strValKey);

        Dictionary<string, Dictionary<string, string>> ItemList
        {
            get;
        }
        
        
    }
    
    public class JsonTable : Table
    {
        public bool BuildTable(string strContent)
        {
            JsonData kJsonData = JsonMapper.ToObject(strContent);
            foreach (var kKey in kJsonData.Keys)
            {
                string strKey = kKey.ToString();
                JsonData kChildData = kJsonData[strKey];
                Dictionary<string,string> kDataList = new Dictionary<string,string>();
                foreach (var kChildKey in kChildData.Keys)
                {
                    string strChildKey = kChildKey.ToString();
                    JsonData kData = kChildData[strChildKey];
                    if (null == kData)
                        continue;
                    if (kData.IsArray)
                    {
                        string strArray = "";
                        for(int i = 0;i < kData.Count;i++)
                        {
                            string strVal = kData[i].ToString();
                            if (string.IsNullOrEmpty(strVal))
                            {
                                strArray += "null";
                            }
                            else
                            {
                                strArray += strVal;
                            }
                            if(i < kData.Count-1)
                                strArray += " ";
                        }
                        kDataList.Add(strChildKey, strArray);
                    }
                    else
                        kDataList.Add(strChildKey,kData.ToString());
                }
                m_kDataList.Add(strKey,kDataList);
            }
            return true;
        }
        
        public Dictionary<string,string> GetItem(string strKey)
        {
            if(m_kDataList.ContainsKey(strKey))
                return m_kDataList[strKey];
            return null;      
        }
        
        public string GetValue(string strItemKey,string strValKey)
        {
            if(m_kDataList.ContainsKey(strItemKey))
            {
                 Dictionary<string,string> kDict = m_kDataList[strItemKey];
                 if(kDict.ContainsKey(strValKey))
                    return kDict[strValKey];
                 return null;
            }
                
            return null;
        }
        public Vector3D GetVector3D(string strItemKey, string strValKey)
        {
            Vector3D kRetVal = new Vector3D();
            if (m_kDataList.ContainsKey(strItemKey))
            {
                Dictionary<string, string> kDict = m_kDataList[strItemKey];
                if (kDict.ContainsKey(strValKey))
                {
                    kRetVal = TableUtil.GetVector3(kDict[strValKey],' ');
                }
            }
            return kRetVal;
        }

        public Dictionary<string, Dictionary<string, string>> ItemList
        {
            get { return m_kDataList; }
        }

        private Dictionary<string,Dictionary<string,string>> m_kDataList = new  Dictionary<string,Dictionary<string,string>>();
    }

    public class DataManager
    {
        public static readonly DataManager Instance = new DataManager();
        private DataManager() { }
        public Table ReadJsonTable(string strName)
        {
            string strContent;
#if FIFA_CLIENT
            strContent = ResourceManager.Instance.LoadText(strName);
#else
            strContent = File.ReadAllText(strName);
#endif

            if (null == strContent)
            {
                LogManager.Instance.LogError("Read Table Error ,No This Table: " + strName);
                return null;
            }

            JsonTable kTable = new JsonTable();
            if (!kTable.BuildTable(strContent))
            {
                return null;
            }

            return kTable;
        }
        
        public Table ReadCVSTable(string strName)
        {
            return null;
        }
    }
}