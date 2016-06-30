using System;
using System.Collections.Generic;

namespace Common.Tables
{
    public class PosCfgItem
    {
        public string ID;
        public List<Vector3D> PosList = new List<Vector3D>();
    }
    public class PosCfgTable
    {
        public PosCfgTable()
        {
            
        }

        public bool InitTable()
        {
            JsonTable kTable = DataManager.Instance.ReadJsonTable("Tables/Battle/PosCfg") as JsonTable;
            if (null == kTable )
                return false;

            foreach(var kItem in kTable.ItemList)
            {
                PosCfgItem kKickOffItem = new PosCfgItem();
                kKickOffItem.ID = kItem.Key;
                string strVal;
                kItem.Value.TryGetValue("value", out strVal);
                if (true == string.IsNullOrEmpty(strVal))
                    continue;
                string[] strList = strVal.Split(';');
                for(int i = 0;i < strList.Length;i++)
                {
                    if (string.IsNullOrEmpty(strList[i]))
                        continue;
                    kKickOffItem.PosList.Add(TableUtil.GetVector3(strList[i], ','));
                }
                m_kItemList.Add(kItem.Key, kKickOffItem);
            }
        
            return true;
        }


        public PosCfgItem GetItem(string strKey)
        {
            PosCfgItem kItem;
            m_kItemList.TryGetValue(strKey, out kItem);
            return kItem;
        }

        protected Dictionary<string, PosCfgItem> m_kItemList = new Dictionary<string, PosCfgItem>();
    }
}
