
using System.Collections.Generic;

namespace Common.Tables
{
    public class ProfessionItem
    {
        public int ID;
        public int Group;
        public string Name = "";
    };

    public class ProfessionTable
    {
        public ProfessionTable()
        {

        }

        public bool InitTable()
        {
            JsonTable kTable = DataManager.Instance.ReadJsonTable("Tables/Battle/profession") as JsonTable;
            if (null == kTable)
                return false;

            foreach (var kVal in kTable.ItemList)
            {
                ProfessionItem kItem = new ProfessionItem();
                kItem.ID = int.Parse(kVal.Key);
                string strVal;
                kVal.Value.TryGetValue("group", out strVal);
                if (string.IsNullOrEmpty(strVal))
                    kItem.Group = -1;
                else
                    kItem.Group = int.Parse(strVal);
                kVal.Value.TryGetValue("shortName", out strVal);
                if (false == string.IsNullOrEmpty(strVal))
                    kItem.Name = strVal;
            }


            return true;
        }


        public ProfessionItem GetItem(int iID)
        {
            ProfessionItem kItem;
            m_kItemList.TryGetValue(iID, out kItem);
            return kItem;
        }

        protected Dictionary<int, ProfessionItem> m_kItemList = new Dictionary<int, ProfessionItem>();
    }
}