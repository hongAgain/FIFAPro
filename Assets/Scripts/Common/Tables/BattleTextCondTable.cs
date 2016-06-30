using Common.Log;
using System.Collections.Generic;

namespace Common.Tables
{
    public class BattleTextCondItem
    {
        public string ID;
        public List<int> CombineList = new List<int>();
        public List<int> RateList = new List<int>();
    }
    
    public class BattleTextCondTable
    {
        public BattleTextCondTable()
        {
            
        }
        
        public bool InitTable()
        {
            JsonTable kTable = DataManager.Instance.ReadJsonTable("Tables/Common/BattleTextCondition") as JsonTable;
            foreach(var kItem in kTable.ItemList)
            {
                BattleTextCondItem kCondItem = new BattleTextCondItem();
                kCondItem.ID = kItem.Key;
                
                string strVal;
                kItem.Value.TryGetValue("combine",out strVal);
                if(string.IsNullOrEmpty(strVal))
                {
                    LogManager.Instance.Log(string.Format("BattleText : ID = {0} Combine Count is zero",kCondItem.ID));
                    continue;
                }                   
                else
                {
                    string[] strList = strVal.Split(';');
                    for(int i = 0;i < strList.Length;i++)
                    {
                        int iVal = -1;
                        if ("null" != strList[i])
                            iVal = int.Parse(strList[i]);
                        kCondItem.CombineList.Add(iVal);
                    }
                }    
                kItem.Value.TryGetValue("rate",out strVal);
                if(string.IsNullOrEmpty(strVal))
                {
                    LogManager.Instance.Log(string.Format("BattleText : ID = {0} Rate Count is zero",kCondItem.ID));
                    continue;
                }
                    
                else
                {
                    string[] strList = strVal.Split(';');
                    for(int i = 0;i < strList.Length;i++)
                    {
                        int iVal = -1;
                        if ("null" != strList[i])
                            iVal = int.Parse(strList[i]);
                        kCondItem.RateList.Add(iVal);
                    }
                }

                //  数据个数不匹配 表明数据无效
                if (kCondItem.CombineList.Count != kCondItem.RateList.Count)
                {
                    LogManager.Instance.Log(string.Format("BattleText : ID = {0} Combine Count unequal to Rate Count", kCondItem.ID));
                    continue;
                }
                m_kItemList.Add(kCondItem.ID, kCondItem);
            }
            return true;
        }
        public BattleTextCondItem GetItem(string strID)
        {
            BattleTextCondItem kItem;
            m_kItemList.TryGetValue(strID, out kItem);
            return kItem;            
        }

        private Dictionary<string,BattleTextCondItem> m_kItemList = new Dictionary<string, BattleTextCondItem>();
    }
}