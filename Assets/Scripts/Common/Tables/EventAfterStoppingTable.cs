using System.Collections.Generic;

namespace Common.Tables
{
    public class EventAfterStoppingItem
    {
        public int ID;
        public int DribblePr;   // 带球概率
        public int PassPr;      // 传球概率
        public int ShootPr;     // 射门概率         
    }
    public class EventAfterStoppingTable
    {
        public EventAfterStoppingTable()
        {
            
        }

        public bool InitTable()
        {
            JsonTable kTable = DataManager.Instance.ReadJsonTable("Tables/Battle/EventAfterStopping") as JsonTable;
            if (null == kTable )
                return false;

            foreach(var kItem in kTable.ItemList)
            {
                EventAfterStoppingItem kEASItem = new EventAfterStoppingItem();
                kEASItem.ID = int.Parse(kItem.Key);
                kEASItem.DribblePr = int.Parse(kItem.Value["dribble"]);
                kEASItem.PassPr = int.Parse(kItem.Value["pass"]);
                kEASItem.ShootPr = int.Parse(kItem.Value["shoot"]);
                m_kItemList.Add(kEASItem.ID, kEASItem);
            }
        
            return true;
        }


        public EventAfterStoppingItem GetItem(int iID)
        {
            if (m_kItemList.ContainsKey(iID))
                return m_kItemList[iID];
            return null;
        }

        protected Dictionary<int, EventAfterStoppingItem> m_kItemList = new Dictionary<int, EventAfterStoppingItem>();
    }
}
