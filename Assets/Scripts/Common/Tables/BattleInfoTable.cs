using System;
using System.Collections.Generic;

namespace Common.Tables
{
    public class BattleInfoItem
    {
        public string ID;
        public double Value;
    }
    public class BattleInfoTable
    {
        public BattleInfoTable()
        {
            
        }

        public bool InitTable()
        {
            JsonTable kTable = DataManager.Instance.ReadJsonTable("Tables/Battle/battleinfo") as JsonTable;
            if (null == kTable )
                return false;

            foreach(var kItem in kTable.ItemList)
            {
                BattleInfoItem kBattleInfoItem = new BattleInfoItem();
                kBattleInfoItem.ID = kItem.Key;
                kBattleInfoItem.Value = double.Parse(kItem.Value["value"]);
                m_kItemList.Add(kItem.Key, kBattleInfoItem);
            }

            return true;
        }

        public BattleInfoItem GetItem(string strKey)
        {
            BattleInfoItem kItem;
            m_kItemList.TryGetValue(strKey,out kItem);
            return kItem;
        }

        protected Dictionary<string, BattleInfoItem> m_kItemList = new Dictionary<string, BattleInfoItem>();
    }
}
