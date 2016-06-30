using System.Collections.Generic;

namespace Common.Tables
{
    public class ConfrontationBasicItem
    {
		public string ID;
		public double Value;
	}

	public class ConfrontationBasicTable
    {
		public ConfrontationBasicTable()
		{
			
		}
	
		public bool InitTable()
		{
			JsonTable kTable = DataManager.Instance.ReadJsonTable("Tables/Battle/ConfrontationBasic") as JsonTable;
			if (null == kTable )
				return false;
			
			foreach(var kVal in kTable.ItemList)
			{
                ConfrontationBasicItem kItem = new ConfrontationBasicItem();
                kItem.ID = kVal.Key;
                kItem.Value = double.Parse(kVal.Value["value"]);
				m_kItemList.Add(kItem.ID, kItem);
			}
			return true;
		}
		
		public ConfrontationBasicItem GetItem(string strID)
		{
            ConfrontationBasicItem kItem;
			m_kItemList.TryGetValue(strID, out kItem);
			return kItem;
		}
		
		protected Dictionary<string, ConfrontationBasicItem> m_kItemList = new Dictionary<string, ConfrontationBasicItem>();
	}
}
