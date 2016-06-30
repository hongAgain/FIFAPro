
using System.Collections.Generic;

namespace Common.Tables
{
    public enum EBattleTextType
    {
        Unknown = 0,
        PlayerName = 1,
        TeamName = 2,
        SkillName = 3
    }
    public class BattleTextItem
    {
        public int ID;
        public string Text;
        public EBattleTextType TextType;  
    }
    public class BattleTextTable
    {
        
        public BattleTextTable()
        {
            
        }
        
        public bool InitTable()
        {
            JsonTable kTable = DataManager.Instance.ReadJsonTable("Tables/Common/BattleText") as JsonTable;
            foreach(var kItem in kTable.ItemList)
            {
                BattleTextItem kTextItem = new BattleTextItem();
                kTextItem.ID = int.Parse(kItem.Key);
                
                string strVal;
                kItem.Value.TryGetValue("text",out strVal);
                if(string.IsNullOrEmpty(strVal))
                    kTextItem.Text = "";
                else
                    kTextItem.Text = strVal;
                kItem.Value.TryGetValue("type",out strVal);
                if(string.IsNullOrEmpty(strVal))
                    kTextItem.TextType = EBattleTextType.Unknown;
                else
                    kTextItem.TextType = (EBattleTextType)(int.Parse(strVal));
                    
                m_kItemList.Add(kTextItem.ID, kTextItem);
            }
            return true;
        }
        public BattleTextItem GetItem(int iID)
        {
            BattleTextItem kItem;
            m_kItemList.TryGetValue(iID, out kItem);
            return kItem;            
        }

        private Dictionary<int,BattleTextItem> m_kItemList = new Dictionary<int, BattleTextItem>();
    }
}