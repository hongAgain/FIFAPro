using Common.Tables;
using System.Collections.Generic;

public class DefenceDensityTable
{
    private Dictionary<string, double> mData = new Dictionary<string, double>();

    public bool InitTable()
    {
        JsonTable kTable = DataManager.Instance.ReadJsonTable("Tables/Battle/DefenceDensity") as JsonTable;
        if (null == kTable)
            return false;

        foreach (var kItem in kTable.ItemList)
        {
            mData.Add(kItem.Key, double.Parse(kItem.Value["defence_coefficient"]));
        }

        return true;
    }

    public double GetValue(string key)
    {
        if (mData.ContainsKey(key))
        {
            return mData[key];
        }
        else
        {
            return 0;
        }
    }
}