using System;
using System.Collections.Generic;

namespace Common.Tables
{
    public class HeroItem
    {
        public HeroItem()
        {
            GroupIDList = new List<int>();
            PotentialityList = new List<float>();
            SkillList = new List<int>();
        }
        public int ID;
        public string Name;
        public int Rating;
        public int SeasonID;
        public int SignID;
        public List<int> GroupIDList;
        public int ClubID;                      // 
        public int CountryID;                   // 国家ID
        public int StarLevel;                   // 初始星级
        public int SoulID;                      // 球员碎片
        public int AdvModID;                    // 进阶模板
        public int Pos;                         // 位置
        public int PotentialModelID;            // 潜力模板
        public List<float> PotentialityList;    // 潜力成长系数
        public List<int> SkillList;             // 技能列表

#if FIFA_CLIENT
        public string Desc;
        public int ModelID;
        public int HeadID;
        public UnityEngine.Color SkinColor;
        public string ShoesID;
        public int PlayerIconID;
        public string number;
#endif
    }
    public class HeroTable
    {
        public HeroTable()
        {
            
        }

        public bool InitTable()
        {
            JsonTable kTable = DataManager.Instance.ReadJsonTable("Tables/Battle/hero") as JsonTable;
            if (null == kTable )
                return false;

            foreach(var kItem in kTable.ItemList)
            {
                HeroItem kHeroItem = new HeroItem();
                kHeroItem.ID = int.Parse(kItem.Key);
                kHeroItem.Name = kItem.Value["name"];
                kHeroItem.Rating = int.Parse(kItem.Value["rating"]);
                kHeroItem.SeasonID = int.Parse(kItem.Value["season"]);
                kHeroItem.SignID = int.Parse(kItem.Value["sign"]);
                kHeroItem.ClubID = int.Parse(kItem.Value["club"]);
                kHeroItem.CountryID = int.Parse(kItem.Value["country"]);
                kHeroItem.StarLevel = int.Parse(kItem.Value["islv"]);
                kHeroItem.SoulID = int.Parse(kItem.Value["soul"]);
                kHeroItem.AdvModID = int.Parse(kItem.Value["advMod"]);
                kHeroItem.Pos = int.Parse(kItem.Value["ipos"]);
                kHeroItem.PotentialModelID = int.Parse(kItem.Value["potential_model"]);
                string strVal = kItem.Value["group"];
                string[] strList = strVal.Split(' ');
                for (int i = 0; i < strList.Length; i++)
                {
                    int iVal = -1;
                    if ("null" != strList[i])
                        iVal = int.Parse(strList[i]);
                    kHeroItem.GroupIDList.Add(iVal);
                }

                strVal = kItem.Value["potentiality"];
                strList = strVal.Split(' ');
                for (int i = 0; i < strList.Length; i++)
                {
                    if ("null" == strList[i])
                    {
                        kHeroItem.PotentialityList.Add(-1);
                    }
                    else
                        kHeroItem.PotentialityList.Add(float.Parse(strList[i]));
                }

                strVal = kItem.Value["skill"];
                strList = strVal.Split(' ');
                for (int i = 0; i < strList.Length; i+=3)
                {
                    if ("null" == strList[i])
                    {
                        kHeroItem.SkillList.Add(-1);
                    }
                    else
                        kHeroItem.SkillList.Add(int.Parse(strList[i]));
                }
#if FIFA_CLIENT
                kHeroItem.ModelID = int.Parse(kItem.Value["modelId"]);
                kHeroItem.HeadID = int.Parse(kItem.Value["headId"]);
                kHeroItem.PlayerIconID = int.Parse(kItem.Value["playericon"]);
                kHeroItem.ShoesID = kItem.Value["shoes"];
                kHeroItem.Desc = kItem.Value["desc"];
                TableUtil.GetColor(kItem.Value["skin"], '.',ref kHeroItem.SkinColor);
                kHeroItem.number = kItem.Value["number"];
#endif
                m_kItemList.Add(kHeroItem.ID, kHeroItem);
            }
            return true;
        }

        public HeroItem GetItem(int iID)
        {
            HeroItem kItem;
            m_kItemList.TryGetValue(iID, out kItem);
            return kItem;
        }

        protected Dictionary<int, HeroItem> m_kItemList = new Dictionary<int, HeroItem>();
    }
}
