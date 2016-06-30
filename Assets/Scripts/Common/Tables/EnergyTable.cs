using System.Collections.Generic;

namespace Common.Tables
{
    public class EnergyItem
    {
        public int ID;
        public double Value;
    }

    public class EnergyTable
    {
        public EnergyTable()
        {

        }

        public bool InitTable()
        {
            JsonTable kTable = DataManager.Instance.ReadJsonTable("Tables/Battle/Energy") as JsonTable;
            if (null == kTable)
                return false;

            foreach (var kItem in kTable.ItemList)
            {
                EnergyItem kEnergyItem = new EnergyItem();
                kEnergyItem.ID = int.Parse(kItem.Key);
                string strVal = null;
                kItem.Value.TryGetValue("rate", out strVal);
                if (string.IsNullOrEmpty(strVal))
                    kEnergyItem.Value = 0;
                else
                    kEnergyItem.Value = double.Parse(strVal);
                m_kItemList.Add(kEnergyItem);
            }
            m_kItemList.Sort(delegate (EnergyItem kLItem, EnergyItem kRItem)
            {
                // 由大到小排序
                if (kLItem.ID < kRItem.ID)
                    return 1;
                return 0;
            });
            
            return true;
        }
        public EnergyItem GetItem(int iID)
        {
            if (0 == m_kItemList.Count)
                return null;
            for(int i = 0;i < m_kItemList.Count;i++)
            {
                if (iID >= m_kItemList[i].ID)
                    return m_kItemList[i];
            }
            return m_kItemList[m_kItemList.Count-1];
        }


        protected List<EnergyItem> m_kItemList = new List<EnergyItem>();
    }
}