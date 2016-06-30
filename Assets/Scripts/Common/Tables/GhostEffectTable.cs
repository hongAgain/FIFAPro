using System;
using System.Collections.Generic;

namespace Common.Tables
{
    public class GhostEffectItem
    {
        public int ID;
        public string Name;
        public int Count;
        public double StartTime;
        public double DeltaTime;
        public double LiveTime;
        public double AllPowerBegin;
        public double AllPowerEnd;
    }
    public class GhostEffectTable
    {
        public GhostEffectTable()
        {

        }

        public bool InitTable()
        {
            JsonTable kTable = DataManager.Instance.ReadJsonTable("Tables/Battle/GhostEffect") as JsonTable;
            if (null == kTable)
                return false;

            foreach (var kItem in kTable.ItemList)
            {
                GhostEffectItem kEffectItem = new GhostEffectItem();
                kEffectItem.ID = int.Parse(kItem.Key);
                string strVal;
                kItem.Value.TryGetValue("name",out strVal);
                if (string.IsNullOrEmpty(strVal))
                    kEffectItem.Name = "";
                else
                    kEffectItem.Name = strVal;

                kItem.Value.TryGetValue("num", out strVal);
                if (string.IsNullOrEmpty(strVal))
                    kEffectItem.Count = 0;
                else
                    kEffectItem.Count = int.Parse(strVal);

                kItem.Value.TryGetValue("anima_start_time", out strVal);
                if (string.IsNullOrEmpty(strVal))
                    kEffectItem.StartTime = 0;
                else
                    kEffectItem.StartTime = double.Parse(strVal);

                kItem.Value.TryGetValue("delta_time", out strVal);
                if (string.IsNullOrEmpty(strVal))
                    kEffectItem.DeltaTime = 0;
                else
                    kEffectItem.DeltaTime = double.Parse(strVal);

                kItem.Value.TryGetValue("live_time", out strVal);
                if (string.IsNullOrEmpty(strVal))
                    kEffectItem.LiveTime = 0;
                else
                    kEffectItem.LiveTime = double.Parse(strVal);

                kItem.Value.TryGetValue("all_pow_begin", out strVal);
                if (string.IsNullOrEmpty(strVal))
                    kEffectItem.AllPowerBegin = 0;
                else
                    kEffectItem.AllPowerBegin = double.Parse(strVal);

                kItem.Value.TryGetValue("all_pow_end", out strVal);
                if (string.IsNullOrEmpty(strVal))
                    kEffectItem.AllPowerEnd = 0;
                else
                    kEffectItem.AllPowerEnd = double.Parse(strVal);
                m_kItemList.Add(kEffectItem.ID, kEffectItem);
            }
            return true;
        }


        public GhostEffectItem GetItem(int iID)
        {
            GhostEffectItem kItem;
            m_kItemList.TryGetValue(iID, out kItem);
            return kItem;
        }


        protected Dictionary<int, GhostEffectItem> m_kItemList = new Dictionary<int, GhostEffectItem>();
    }
}
