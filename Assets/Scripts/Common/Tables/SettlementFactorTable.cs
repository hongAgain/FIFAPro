using System.Collections.Generic;

namespace Common.Tables
{
    public class SettlementFactorItem
    {
        public string ID;
        public double SponsorParam1;            // 发起方结算系数1
        public double SponsorParam2;            // 发起方结算系数2
        public double ReceiverParam1;           // 对抗方结算系数1
        public double ReceiverParam2;           // 对抗方结算系数2
        public double BasicPr;                  // 事件基础概率
        public double Distance;                 // 基础距离数
        public double DefenceNum;               // 防守基础人数参数
    }

    public class SettlementFactorTable
    {
        public SettlementFactorTable()
        {
        }
        public bool InitTable()
        {
            JsonTable kTable = DataManager.Instance.ReadJsonTable("Tables/Battle/SettlementCoefficient") as JsonTable;
            if (null == kTable)
                return false;

            foreach (var kItem in kTable.ItemList)
            {
                SettlementFactorItem kSFItem = new SettlementFactorItem();
                kSFItem.ID = kItem.Key;

                string strVal;
                kItem.Value.TryGetValue("settlement_coefficient1", out strVal);
                if (string.IsNullOrEmpty(strVal))
                    kSFItem.SponsorParam1 = 1;
                else
                    kSFItem.SponsorParam1 = double.Parse(strVal);

                kItem.Value.TryGetValue("settlement_coefficient2", out strVal);
                if (string.IsNullOrEmpty(strVal))
                    kSFItem.SponsorParam2 = 1;
                else
                    kSFItem.SponsorParam2 = double.Parse(strVal);

                kItem.Value.TryGetValue("settlement_coefficient3", out strVal);
                if (string.IsNullOrEmpty(strVal))
                    kSFItem.ReceiverParam1 = 1;
                else
                    kSFItem.ReceiverParam1 = double.Parse(strVal);

                kItem.Value.TryGetValue("settlement_coefficient4", out strVal);
                if (string.IsNullOrEmpty(strVal))
                    kSFItem.ReceiverParam2 = 1;
                else
                    kSFItem.ReceiverParam2 = double.Parse(strVal);

                kItem.Value.TryGetValue("basic_value", out strVal);
                if (string.IsNullOrEmpty(strVal))
                    kSFItem.BasicPr = 1;
                else
                    kSFItem.BasicPr = double.Parse(strVal);

                kItem.Value.TryGetValue("basic_distance", out strVal);
                if (string.IsNullOrEmpty(strVal))
                    kSFItem.Distance = 1;
                else
                    kSFItem.Distance = double.Parse(strVal);

                kItem.Value.TryGetValue("defence_num", out strVal);
                if (string.IsNullOrEmpty(strVal))
                    kSFItem.DefenceNum = 1;
                else
                    kSFItem.DefenceNum = double.Parse(strVal);

                m_kItemList.Add(kSFItem.ID, kSFItem);
            }
            return true;
        }
        public SettlementFactorItem GetItem(string strID)
        {
            SettlementFactorItem kItem;
            m_kItemList.TryGetValue(strID, out kItem);
            return kItem;
        }
        private Dictionary<string, SettlementFactorItem> m_kItemList = new Dictionary<string, SettlementFactorItem>();
    }
}