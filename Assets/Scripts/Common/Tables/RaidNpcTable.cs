using System;
using System.Collections.Generic;

namespace Common.Tables
{
    public class RaidNPCItem
    {
        public int ID;
        public string Name;
        public int Pos;
        public int PlayerIconID;
        public int PlayerID;
        public List<int> SkillList = new List<int>();
    }
    public class RaidNPCTable
    {
        public RaidNPCTable()
        {

        }

        public bool InitTable()
        {
            JsonTable kTable = DataManager.Instance.ReadJsonTable("Tables/Common/raid_npc") as JsonTable;
            if (null == kTable)
                return false;

            foreach (var kItem in kTable.ItemList)
            {
                RaidNPCItem kRaidItem = new RaidNPCItem();
                kRaidItem.ID = int.Parse(kItem.Key);
                string strVal;
                kItem.Value.TryGetValue("name", out strVal);
                if (string.IsNullOrEmpty(strVal))
                    kRaidItem.Name = "";
                else
                    kRaidItem.Name = strVal;
                kItem.Value.TryGetValue("ipos", out strVal);
                if (string.IsNullOrEmpty(strVal))
                    kRaidItem.Pos = 0;
                else
                    kRaidItem.Pos = int.Parse(strVal);
                kItem.Value.TryGetValue("player_id", out strVal);
                if (string.IsNullOrEmpty(strVal))
                    kRaidItem.PlayerID = 0;
                else
                    kRaidItem.PlayerID = int.Parse(strVal);
                kItem.Value.TryGetValue("playericon", out strVal);
                if (string.IsNullOrEmpty(strVal))
                    kRaidItem.PlayerIconID = 0;
                else
                    kRaidItem.PlayerIconID = int.Parse(strVal);
                kItem.Value.TryGetValue("skill", out strVal);
                if (false == string.IsNullOrEmpty(strVal))
                {
                    string[] strList = strVal.Split(' ');
                    for(int i = 0;i < strList.Length;i++)
                    {
                        if ("null" != strList[i])
                            kRaidItem.SkillList.Add(int.Parse(strList[i]));
                    }
                }

                m_kItemList.Add(kRaidItem.ID, kRaidItem);
            }
            return true;
        }

        public RaidNPCItem GetItem(string strKey)
        {
            return GetItem(int.Parse(strKey));
        }

        public RaidNPCItem GetItem(int iID)
        {
            RaidNPCItem kItem;
            m_kItemList.TryGetValue(iID, out kItem);
            return kItem;
        }

        protected Dictionary<int, RaidNPCItem> m_kItemList = new Dictionary<int, RaidNPCItem>();
    }
}
