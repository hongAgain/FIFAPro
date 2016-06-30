using System.Collections.Generic;

namespace Common.Tables
{
    /// <summary>
    /// 技能事件类型
    /// </summary>
    public enum EEventType
    {
        ET_All = 0,        // 所有事件
        ET_Trigger_Begin,
        // 触发类技能
        ET_Shoot = ET_Trigger_Begin,    // 我方脚下射门事件    done                                     
        ET_FarShoot,                    // 我方脚下远射门事件  done
        ET_HeadShoot,                   // 我方头球射门事件    done
        ET_CatchBall,                   // 我方接球事件    
        ET_ShootSuccess,                // 我方进球事件
        ET_OpCatchBall,                 // 对方接球事件
        ET_LongPassBall,                // 我方长传球事件   done
        ET_ShortPassBall,               // 我方短传球事件   done
        ET_BreakThrough,                // 我方突破事件     done        
        ET_Block,                       // 我方抢断事件     done
        ET_Intercept,                   // 我方拦截事件     done
        ET_Mark,                        // 我方盯防事件  
        ET_Snatch,                      // 铲球事件
        ET_HeadRob,                     // 头球争顶
        ET_GKSave,                      // 门将扑球         done
        ET_Trigger_End = ET_GKSave,

        // buff类技能
        ET_Buff_Begin,
        ET_Buff_End
        // 天赋类技能
    }
   
    // 技能作用对象
    public enum ESkillTarget
    {
        ST_Self = 1,        // 自身
        ST_Self_Forward,    // 我方所有前锋
        ST_Self_Middle,     // 我方所有中场
        ST_Self_Back,       // 我方所有后卫
        ST_Self_All,        // 所有我方球员
        ST_Forward,         // 对方所有前锋
        ST_Middle,          // 对方所有中场
        ST_Back,            // 对方所有后卫
        ST_All,             // 对方所有球员
        ST_Self_GK,         // 我方门将
        ST_GK               // 对方门将
    }
    /// <summary>
    /// 技能类型分类
    /// </summary>
    public enum ESkillType
    {
        Buff = 0,
        Talent,
        Team,       
        Trigger
    }

    public enum EActiveObject
    {
        ActiveObject_Invalid = 0
    }

    public class SkillItem
    {   
        public int ID=0;                                            // ID
        public string IconName="";                                  // 图片名
        public string Name = "";                                    // 技能名
        public int Quality = 1;                                     // 品质
        public int SkillSetID;                                      // 技能族ID
        public string Desc = "";                                    // 描述
        public int EffectID;                                        // 技能效果类型
        public double SkillRate = 0;                                // 技能初始概率
        public double SkillRateStep = 0;                            // 技能概率步长
        public EEventType SkillType = EEventType.ET_All;            // 事件类型
        public ESkillType EffectType = ESkillType.Buff;             // 技能类型
        public ESkillTarget SkillTarget = ESkillTarget.ST_Self;     // 作用对象类型
        public List<double> AttriList = new List<double>();         // 系数
    }

    public class SkillTable
    {
        public SkillTable()
        {
            
        }

        public bool InitTable()
        {
            JsonTable kTable = DataManager.Instance.ReadJsonTable("Tables/Common/skill") as JsonTable;
            if (null == kTable )
                return false;

            foreach(var kItem in kTable.ItemList)
            {
                SkillItem kSkillItem = new SkillItem();
                kSkillItem.ID = int.Parse(kItem.Key);

                string strVal = null;
                kItem.Value.TryGetValue("name", out strVal);
                if (string.IsNullOrEmpty(strVal))
                    kSkillItem.Name = "";
                else
                    kSkillItem.Name = strVal;

                kItem.Value.TryGetValue("quality", out strVal);
                if (string.IsNullOrEmpty(strVal))
                    kSkillItem.Quality = 0;
                else
                    kSkillItem.Quality = int.Parse(strVal);

                kItem.Value.TryGetValue("skill_set", out strVal);
                if (false == string.IsNullOrEmpty(strVal))
                    kSkillItem.SkillSetID = int.Parse(strVal);

                kItem.Value.TryGetValue("desc", out strVal);
                if (false == string.IsNullOrEmpty(strVal))
                {
                    int iIdx = strVal.IndexOf('$');
                    while (iIdx > 0)
                    {
                        string strSub = strVal.Substring(iIdx, 2);
                        strVal = strVal.Replace(strSub, "{0}");
                        iIdx = strVal.IndexOf('$');
                    }
                    kSkillItem.Desc = strVal;
                }
                    

                kItem.Value.TryGetValue("effect_id", out strVal);
                if (false == string.IsNullOrEmpty(strVal))
                    kSkillItem.EffectID = int.Parse(strVal);

                kItem.Value.TryGetValue("event_type", out strVal);
                if (false == string.IsNullOrEmpty(strVal))
                    kSkillItem.SkillType = (EEventType)(int.Parse(strVal));

                kItem.Value.TryGetValue("skill_rate_ths", out strVal);
                if (false == string.IsNullOrEmpty(strVal))
                    kSkillItem.SkillRate = double.Parse(strVal)/1000;

                kItem.Value.TryGetValue("skill_rate", out strVal);
                if (false == string.IsNullOrEmpty(strVal))
                    kSkillItem.SkillRateStep = double.Parse(strVal) / 1000;

                kItem.Value.TryGetValue("skill_type", out strVal);
                if (string.IsNullOrEmpty(strVal))
                    kSkillItem.EffectType = ESkillType.Buff;
                else
                    kSkillItem.EffectType = (ESkillType)(int.Parse(strVal));

                kItem.Value.TryGetValue("skill_target", out strVal);
                if (string.IsNullOrEmpty(strVal))
                    kSkillItem.SkillTarget = ESkillTarget.ST_Self;
                else
                    kSkillItem.SkillTarget = (ESkillTarget)(int.Parse(strVal));

                kItem.Value.TryGetValue("icon", out strVal);
                if (false == string.IsNullOrEmpty(strVal))
                    kSkillItem.IconName = strVal;

                kItem.Value.TryGetValue("attr", out strVal);
                if (false == string.IsNullOrEmpty(strVal))
                {
                    string[] strList = strVal.Split(' ');
                    for (int i = 0; i < strList.Length; i++)
                    {
                        double dVal = -1;
                        if ("null" != strList[i])
                            dVal = double.Parse(strList[i]);
                        kSkillItem.AttriList.Add(dVal);
                    }
                }
                m_kItemList.Add(kSkillItem.ID, kSkillItem);
            }

            return true;
        }


        public SkillItem GetItem(int iID)
        {
            SkillItem kItem;
            m_kItemList.TryGetValue(iID, out kItem);
            return kItem;
        }

        protected Dictionary<int, SkillItem> m_kItemList = new Dictionary<int, SkillItem>();
    }
    public struct SAminID
    {
        public int ID;
        public double Angle;
    }
    public class SkillAppearItem
    {
        public struct SAminIDInfo
        {
            public int ID;
            public int AngleType;
        }
        public int ID = 0;                                              // 技能表现ID 
        public double CoolTime;                                         // CD时间
        public double DurationTime;                                     // 技能持续时间
        public double TriggerProbability;                               // 触发概率
        public double Angle = 0;                                        // 旋转角度
        public List<int> FXIDList = new List<int>();                    // 特效ID列表
        public List<int> CameraFxIDList = new List<int>();              // 相机特效ID列表 
        public List<SAminIDInfo> AnimIDList = new List<SAminIDInfo>();  // 动画列表
    }


    public class SkillAppearTable
    {
        public SkillAppearTable()
        {

        }

        public bool InitTable()
        {
            JsonTable kTable = DataManager.Instance.ReadJsonTable("Tables/Battle/skill_appear") as JsonTable;
            if (null == kTable)
                return false;

            foreach (var kItem in kTable.ItemList)
            {
                SkillAppearItem kSkillItem = new SkillAppearItem();
                kSkillItem.ID = int.Parse(kItem.Key);

                string strVal = null;


                kItem.Value.TryGetValue("cd", out strVal);
                if (false == string.IsNullOrEmpty(strVal))
                    kSkillItem.CoolTime = double.Parse(strVal);

                kItem.Value.TryGetValue("duration", out strVal);
                if (false == string.IsNullOrEmpty(strVal))
                    kSkillItem.DurationTime = double.Parse(strVal);

                kItem.Value.TryGetValue("angle", out strVal);
                if (false == string.IsNullOrEmpty(strVal))
                    kSkillItem.Angle = double.Parse(strVal);
                

                kItem.Value.TryGetValue("fx_id", out strVal);
                if (false == string.IsNullOrEmpty(strVal))
                {
                    string[] strList = strVal.Split(',');
                    for (int i = 0; i < strList.Length; i++)
                    {
                        int iVal = -1;
                        iVal = int.Parse(strList[i]);
                        kSkillItem.FXIDList.Add(iVal);
                    }
                }

                kItem.Value.TryGetValue("camera_fx_id", out strVal);
                if (false == string.IsNullOrEmpty(strVal))
                {
                    string[] strList = strVal.Split(',');
                    for (int i = 0; i < strList.Length; i++)
                    {
                        int iVal = -1;
                        iVal = int.Parse(strList[i]);
                        kSkillItem.CameraFxIDList.Add(iVal);
                    }
                }

                kItem.Value.TryGetValue("amin_ids", out strVal);
                if (false == string.IsNullOrEmpty(strVal))
                {
                    string[] strList = strVal.Split(';');
                    for (int i = 0; i < strList.Length; i++)
                    {
                        string[] strValList = strList[i].Split(',');
                        if(2 == strValList.Length)
                        {
                            SkillAppearItem.SAminIDInfo kAnimID = new SkillAppearItem.SAminIDInfo();
                            kAnimID.AngleType = int.Parse(strValList[0]);
                            kAnimID.ID = int.Parse(strValList[1]);
                            kSkillItem.AnimIDList.Add(kAnimID);
                        }
                    }
                }

                m_kItemList.Add(kSkillItem.ID, kSkillItem);
            }

            return true;
        }


        public SkillAppearItem GetItem(int iID)
        {
            SkillAppearItem kItem;
            m_kItemList.TryGetValue(iID, out kItem);
            return kItem;
        }
        protected Dictionary<int, SkillAppearItem> m_kItemList = new Dictionary<int, SkillAppearItem>();
    }

}
