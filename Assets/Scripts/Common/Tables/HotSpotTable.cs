using Common.Tables;
using System.Collections.Generic;

public class HotSpotTable
{
    private Dictionary<string, Dictionary<string, int>> mData = new Dictionary<string, Dictionary<string, int>>();

    public bool InitTable()
    {
        JsonTable kTable = DataManager.Instance.ReadJsonTable("Tables/Battle/HotSpot") as JsonTable;
        if (null == kTable)
            return false;

        foreach (var kItem in kTable.ItemList)
        {
            var dic = new Dictionary<string, int>();
            mData.Add(kItem.Key, dic);

            foreach (var value in kItem.Value)
            {
                dic.Add(value.Key, int.Parse(value.Value));
            }
        }

        return true;
    }

    public int GetValue(string key1, string key2)
    {
        if (mData.ContainsKey(key1))
        {
            if (mData[key1].ContainsKey(key2))
            {
                return mData[key1][key2];
            }
        }
        return 0;
    }
}