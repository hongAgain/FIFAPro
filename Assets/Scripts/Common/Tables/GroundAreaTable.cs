using System;
using System.Collections.Generic;

namespace Common.Tables
{
    /// <summary>
    /// only affective for continuoes regions
    /// </summary>
    public class GroundAreaItem
    {
        public GroundAreaItem()
        {
            Area = new Point2D();
        }
        public String ID;
        public Point2D Area;

        public bool IsInside(int regionid)
        {
            return regionid >= Area.X && regionid <= Area.Y;
        }
    }
    public class GroundAreaTable
    {
        public GroundAreaTable()
        {
        }
        public bool InitTable()
        {
            JsonTable kTable = DataManager.Instance.ReadJsonTable("Tables/Battle/GroundArea") as JsonTable;
            if (null == kTable)
                return false;

            foreach (var kItem in kTable.ItemList)
            {
                GroundAreaItem kGroundAreaItem = new GroundAreaItem();
                kGroundAreaItem.ID = kItem.Key;
                kGroundAreaItem.Area.X = int.Parse(kItem.Value["left_top_id"]);
                kGroundAreaItem.Area.Y = int.Parse(kItem.Value["right_bottom"]);
                m_kItemList.Add(kItem.Key, kGroundAreaItem);
            }
            return true;
        }
        public GroundAreaItem GetItem(String strKey)
        {
            if (m_kItemList.ContainsKey(strKey))
                return m_kItemList[strKey];
            return null;
        }

        protected Dictionary<String, GroundAreaItem> m_kItemList = new Dictionary<String, GroundAreaItem>();
    }
    
}
