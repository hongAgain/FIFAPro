using System.Collections.Generic;

namespace Common.Tables
{

    public class SensitivityFactorItem
    {
        public int ID;
        public int LongPass;                    // 长传传出
        public int ShortPass;                   // 短传传出
        public int ShortPassIntercept;          // 短传拦截
        public int Break;                       // 突破成功率
        public int Tackle;                      // 抢断成功率
        public int Shoot;                       // 射门
        public int LongShoot;                   // 远射
        public int HeadShoot;                   // 头球射门
        public int ShootInsidePr;               // 射正
        public int SlideTackle;                 // 铲球
        public int Head;                        // 争顶
        public int Freekick;                    // 任意球
        public int Penalty;                     // 点球
        public int Save;                        // 扑住
        public int GKAttack;                    // 出击
    }

    public class SensitivityFactorTable
    {
        public SensitivityFactorTable()
        {
            
        }

        public bool InitTable()
        {
            JsonTable kTable = DataManager.Instance.ReadJsonTable("Tables/Battle/SensitivityCoefficient") as JsonTable;
            if (null == kTable )
                return false;

            foreach (var kItem in kTable.ItemList)
            {
                SensitivityFactorItem kSFItem = new SensitivityFactorItem();
                kSFItem.ID = int.Parse(kItem.Key);
                string strVal;

                kItem.Value.TryGetValue("long_pass", out strVal);
                if (string.IsNullOrEmpty(strVal))
                    kSFItem.LongPass = 1;
                else
                    kSFItem.LongPass = int.Parse(strVal);

                kItem.Value.TryGetValue("short_pass", out strVal);
                if (string.IsNullOrEmpty(strVal))
                    kSFItem.ShortPass = 1;
                else
                    kSFItem.ShortPass = int.Parse(strVal);

                kItem.Value.TryGetValue("short_pass_intercept", out strVal);
                if (string.IsNullOrEmpty(strVal))
                    kSFItem.ShortPassIntercept = 1;
                else
                    kSFItem.ShortPassIntercept = int.Parse(strVal);

                kItem.Value.TryGetValue("break", out strVal);
                if (string.IsNullOrEmpty(strVal))
                    kSFItem.Break = 1;
                else
                    kSFItem.Break = int.Parse(strVal);

                kItem.Value.TryGetValue("tackle", out strVal);
                if (string.IsNullOrEmpty(strVal))
                    kSFItem.Tackle = 1;
                else
                    kSFItem.Tackle = int.Parse(strVal);

                kItem.Value.TryGetValue("shot", out strVal);
                if (string.IsNullOrEmpty(strVal))
                    kSFItem.Shoot = 1;
                else
                    kSFItem.Shoot = int.Parse(strVal);

                kItem.Value.TryGetValue("head", out strVal);
                if (string.IsNullOrEmpty(strVal))
                    kSFItem.HeadShoot = 1;
                else
                    kSFItem.HeadShoot = int.Parse(strVal);

                kItem.Value.TryGetValue("shot_inthedoor", out strVal);
                if (string.IsNullOrEmpty(strVal))
                    kSFItem.ShootInsidePr = 1;
                else
                    kSFItem.ShootInsidePr = int.Parse(strVal);

                kItem.Value.TryGetValue("slide_tackle", out strVal);
                if (string.IsNullOrEmpty(strVal))
                    kSFItem.SlideTackle = 1;
                else
                    kSFItem.SlideTackle = int.Parse(strVal);

                kItem.Value.TryGetValue("head", out strVal);
                if (string.IsNullOrEmpty(strVal))
                    kSFItem.Head = 1;
                else
                    kSFItem.Head = int.Parse(strVal);

                kItem.Value.TryGetValue("long_shot", out strVal);
                if (string.IsNullOrEmpty(strVal))
                    kSFItem.LongShoot = 1;
                else
                    kSFItem.LongShoot = int.Parse(strVal);

                kItem.Value.TryGetValue("freekick", out strVal);
                if (string.IsNullOrEmpty(strVal))
                    kSFItem.Freekick = 1;
                else
                    kSFItem.Freekick = int.Parse(strVal);

                kItem.Value.TryGetValue("penalty", out strVal);
                if (string.IsNullOrEmpty(strVal))
                    kSFItem.Penalty = 1;
                else
                    kSFItem.Penalty = int.Parse(strVal);

                kItem.Value.TryGetValue("save", out strVal);
                if (string.IsNullOrEmpty(strVal))
                    kSFItem.Save = 1;
                else
                    kSFItem.Save = int.Parse(strVal);

                kItem.Value.TryGetValue("goalkeeper_attack", out strVal);
                if (string.IsNullOrEmpty(strVal))
                    kSFItem.GKAttack = 1;
                else
                    kSFItem.GKAttack = int.Parse(strVal);

                m_kItemList.Add(kSFItem.ID, kSFItem);
            }

            return true;
        }
        public SensitivityFactorItem GetItem(int iID)
        {
            SensitivityFactorItem kItem;

            m_kItemList.TryGetValue(iID, out kItem);
            return kItem;
        }

        private Dictionary<int, SensitivityFactorItem> m_kItemList = new Dictionary<int, SensitivityFactorItem>(); 
    }
}

