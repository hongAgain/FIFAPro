using System;
using System.Collections.Generic;

namespace Common.Tables
{
    public class FormationItem
    {
        public int ID;
        public string Name;
#if (UNITY_ANDROID || UNITY_IOS || UNITY_EDITOR || UNITY_IPHONE || UNITY_STANDALONE)
        public string DetailName;   // 阵型细分
        public List<int> ProList = new List<int>();   // 职业ID 
#endif
    }
    public class FormationTable
    {
        public FormationTable()
        {
            
        }

        public bool InitTable()
        {
            JsonTable kTable = DataManager.Instance.ReadJsonTable("Tables/Common/formation") as JsonTable;
            if (null == kTable )
                return false;

            foreach(var kItem in kTable.ItemList)
            {
                FormationItem kFormationItem = new FormationItem();
                kFormationItem.ID = int.Parse(kItem.Key);
                kFormationItem.Name = kItem.Value["name"];
#if (UNITY_ANDROID || UNITY_IOS || UNITY_EDITOR || UNITY_IPHONE || UNITY_STANDALONE)
                kFormationItem.DetailName = kItem.Value["name2"];
                string[] strProList = kItem.Value["pro"].Split(' ');
                for(int iIdx = 0; iIdx < strProList.Length;iIdx++)
                {
                    int iVal = -1;
                    if ("null" != strProList[iIdx])
                        iVal = int.Parse(strProList[iIdx]);
                    kFormationItem.ProList.Add(iVal);
                }
#endif
                m_kItemList.Add(kFormationItem.ID, kFormationItem);
            }

            return true;
        }


        public FormationItem GetItem(int iID)
        {
            if (m_kItemList.ContainsKey(iID))
                return m_kItemList[iID];
            return null;
        }

        public Dictionary<int, FormationItem> GetItemList()
        {
            return m_kItemList;
        }


        protected Dictionary<int, FormationItem> m_kItemList = new Dictionary<int, FormationItem>();
    }
}
