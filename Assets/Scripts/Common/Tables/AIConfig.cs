using System;
using System.Collections.Generic;

namespace Common.Tables
{
    public class AICfgItem
    {
        public string ID;
        public double Value;
    }
    public class AIConfig
    {
        public AIConfig()
        {
            
        }

        public bool InitTable()
        {
            JsonTable kTable = DataManager.Instance.ReadJsonTable("Tables/Battle/AIConfig") as JsonTable;
            if (null == kTable )
                return false;

            foreach(var kItem in kTable.ItemList)
            {
                AICfgItem kAICfgItem = new AICfgItem();
                kAICfgItem.ID = kItem.Key;
                string strVal = null;
                kItem.Value.TryGetValue("val", out strVal);
                if(null != strVal)
                {
                    kAICfgItem.Value = double.Parse(strVal);
                    m_kItemList.Add(kItem.Key, kAICfgItem);
                }
            }
            runback_coefficients.Add(GetItem("runback_coefficient1").Value);
            runback_coefficients.Add(GetItem("runback_coefficient2").Value);
            runback_coefficients.Add(GetItem("runback_coefficient3").Value);
            press_coefficients.Add(GetItem("press_coefficient1").Value);
            press_coefficients.Add(GetItem("press_coefficient2").Value);
            press_coefficients.Add(GetItem("press_coefficient3").Value);

            return true;
        }


        public AICfgItem GetItem(string strKey)
        {
            AICfgItem kItem;
            m_kItemList.TryGetValue(strKey, out kItem);
            return kItem;
        }
        public List<double> DeltaZ
        {
            get { return delta_Z; }
            private set{}
        }
        public List<double> DeltaX
        {
            get { return delta_X; }
            
        }

        public List<double> PressCoeff
        {
            get { return press_coefficients; }
            
        }


        /// <summary>
        /// 控制阵型调整
        /// </summary>
        private List<double> delta_z_level = new List<double>();
        private List<double> delta_x_level = new List<double>();

        protected List<double> delta_Z = new List<double>();
        protected List<double> delta_X = new List<double>();

        /// <summary>
        ///  控制防守策略
        /// </summary>
        protected List<double> runback_coefficients = new List<double>();         //回防系数
        /// <summary>
        /// 控制进攻倾向
        /// </summary>
        protected List<double> press_coefficients = new List<double>();           //前压系数

        protected Dictionary<string, AICfgItem> m_kItemList = new Dictionary<string, AICfgItem>();
    }
}
