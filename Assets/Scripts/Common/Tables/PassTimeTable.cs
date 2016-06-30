using System.Collections.Generic;

namespace Common.Tables
{

    public class PassTimeItem
    {
        public double Distance;
        public double Time;
        public double MaxHight;
    }

    public class PassTimeTable
    {
        public PassTimeTable()
        {
            
        }

        public bool InitTable()
        {
            JsonTable kTable = DataManager.Instance.ReadJsonTable("Tables/Battle/PassTime") as JsonTable;
            if (null == kTable)
                return false;

            
            foreach (var kItem in kTable.ItemList)
            {
                PassTimeItem kPassTime = new PassTimeItem();
                kPassTime.Distance = double.Parse(kItem.Key);
                kPassTime.Time = double.Parse(kItem.Value["time"]);
                kPassTime.MaxHight = double.Parse(kItem.Value["height"]); 
                m_kItemList.Add(kPassTime);
            }

            m_kItemList.Sort(delegate(PassTimeItem kItem1, PassTimeItem kItem2)
            {
                if (kItem1.Distance < kItem2.Distance)
                    return -1;
                return 1;
            });
            return true;
        }

        public PassTimeItem GetItem(double dDist)
        {
            for (int i = 0; i < m_kItemList.Count;i++ )
            {
                if (m_kItemList[i].Distance > dDist)
                {
                    return m_kItemList[i];
                }
            }
            return m_kItemList[m_kItemList.Count-1];
        }

        private List<PassTimeItem> m_kItemList = new List<PassTimeItem>(); 
    }
}

