using System;
using System.Collections.Generic;

namespace Common.Tables
{
    public class ShootTimeItem
    {
        public double Distance;
        public double Time;
        public double MaxHight;
    }

    public class ShootTimeTable
    {
        public bool InitTable()
        {
            JsonTable kTable = DataManager.Instance.ReadJsonTable("Tables/Battle/PassTime") as JsonTable;
            if (null == kTable)
                return false;


            foreach (var kItem in kTable.ItemList)
            {
                ShootTimeItem kPassTime = new ShootTimeItem();
                kPassTime.Distance = double.Parse(kItem.Key);
                kPassTime.Time = double.Parse(kItem.Value["time"]);
                kPassTime.MaxHight = double.Parse(kItem.Value["height"]);
                m_kItemList.Add(kPassTime);
            }

            m_kItemList.Sort(delegate (ShootTimeItem kItem1, ShootTimeItem kItem2)
            {
                if (kItem1.Distance < kItem2.Distance)
                    return -1;
                return 1;
            });
            return true;
        }

        public ShootTimeItem GetItem(double dDist)
        {
            for (int i = 0; i < m_kItemList.Count; i++)
            {
                if (m_kItemList[i].Distance > dDist)
                {
                    return m_kItemList[i];
                }
            }
            return m_kItemList[m_kItemList.Count-1];
        }

        private List<ShootTimeItem> m_kItemList = new List<ShootTimeItem>();
    }
}