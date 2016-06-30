
using Common.Log;
using System;
using System.Collections.Generic;

////////
///AI配置中，关于传球球员的相关规则数据
////////
namespace Common.Tables
{

    /// <summary>
    /// 战术配置的接球区域系数
    /// </summary>
    public class AttackTacticalData
    {
        public int ID;
        public int m_etType;
        public double forwardparam;
        public double backwardparam;
        public double leftparam;
        public double rightparam;
        public int[] mEightRate;
    }

    /// <summary>
    /// 距离配置接球属性数据
    /// </summary>
    public class DistanceDecayItem
    {
        public String m_id;
        public double m_dis;
        public double m_percent;
        public double m_predis;
        public double m_prepercent;
        public bool InCheck(double _distance)
        {
            if (_distance >= m_predis && _distance < m_dis)
                return true;
            return false;
        }

        public double GetDistancepercent()
        {
            return m_prepercent;
        }
    }

    /// <summary>
    /// 传球拦截相关系数
    /// </summary>
    public class InterceptCoefficientData
    {
        public int ID;
        public double Percent;
    }

    /// <summary>
    /// 根据区域ID，得出接球者的得分系数
    /// </summary>
    public class TacticalPosCoefficientData
    {
        public int ID;
        public int m_reginId;
        public double m_forward;
        public double m_backward;
        public double m_minfield;
        public double m_goalkeeper;

        public double GetFieldPercent(int _field)
        {
            switch (_field)
            {
                case 0:
                    return m_forward;
                case 1:
                    return m_minfield;
                case 2:
                    return m_backward;
                case 3:
                    return m_goalkeeper;
            }
            return 0;
        }
    }


   public class PlayerPositionData{
       public double m_startx;
       public double m_startz;
       public double m_regionIdX;
       public double m_regionIdZ;

       public PlayerPositionData(double _startX,double _startZ,double _regionX,double _regionZ)
       {
           m_startx = _startX;
           m_startz = _startZ;
           m_regionIdX = _regionX;
           m_regionIdZ = _regionZ;
       }
   }
   public class MidKickOffPosItem
    {
        public int ID;
        public List<PlayerPositionData> m_datas;

        public MidKickOffPosItem()
        {
            m_datas = new List<PlayerPositionData>();
        }
       
    }


   public class HomePositionZData
   {
       public int m_zId;
       public double  m_attackMin;
       public double m_attackMax;
       public double m_defineMin;
       public double m_defineMax;
   }

    /// <summary>
    /// 存储战术配置的相关参数
    /// </summary>

    public class AttackTacticalConfig
    {
        private Dictionary<int,AttackTacticalData> m_configs = new Dictionary<int, AttackTacticalData>();

        public AttackTacticalConfig()
        {
        }

        public bool InitTable()
        {
            JsonTable kTable = DataManager.Instance.ReadJsonTable("Tables/Battle/AttackTacticalType") as JsonTable;
            if (null == kTable)
                return false;

            foreach (var kItem in kTable.ItemList)
            {
                AttackTacticalData kData = new AttackTacticalData();
                kData.ID = int.Parse(kItem.Key);
                kData.m_etType = int.Parse(kItem.Value["type"]);
                kData.forwardparam = double.Parse(kItem.Value["forward_val"]);
                kData.backwardparam = double.Parse(kItem.Value["backward_val"]);
                kData.leftparam = double.Parse(kItem.Value["leftward_val"]);
                kData.rightparam = double.Parse(kItem.Value["right_val"]);

                kData.mEightRate = new int[8];
                for (int i = 0; i < kData.mEightRate.Length; ++i)
                {
                    kData.mEightRate[i] = int.Parse(kItem.Value[string.Format("{0}_dir", i + 1)]);
                }
                m_configs.Add(kData.ID,kData);
            }
            return true;
        }

        /// <summary>
        /// 根据战术ID，查找响应的战术配置数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public AttackTacticalData GetDataById(int id)
        {
            AttackTacticalData kData;
            m_configs.TryGetValue(id, out kData);
            return kData;
        }

    }

    /// <summary>
    /// 根据接球者和持球者距离得到接球效果百分比配置
    /// </summary>
    public class DistanceDecayTable
    {
        private List<DistanceDecayItem> m_configs = new List<DistanceDecayItem>();
        public DistanceDecayTable()
        {
            
        }
        public bool InitTable()
        {
            JsonTable kTable = DataManager.Instance.ReadJsonTable("Tables/Battle/DistanceDecay") as JsonTable;
            if (null == kTable)
                return false;

            foreach (var kItem in kTable.ItemList)
            {
                DistanceDecayItem kData = new DistanceDecayItem();
                kData.m_id = kItem.Key;
                kData.m_dis = double.Parse(kData.m_id);
                kData.m_percent = double.Parse(kItem.Value["effect_percentage"]);
                m_configs.Add(kData);
            }

            m_configs.Sort(delegate(DistanceDecayItem kItem1, DistanceDecayItem kItem2)
            {
                if (kItem1.m_dis < kItem2.m_dis)
                    return -1;
                return 1;
            });

            ResetConfig();
            return true;
        }
        
        private void ResetConfig()
        {
            for (int i = 0; i < m_configs.Count; i++)
            {
                if (i == 0)
                {
                    m_configs[i].m_predis = m_configs[i].m_dis;
                    m_configs[i].m_prepercent = m_configs[i].m_percent;
                }
                else
                {
                    m_configs[i].m_predis = m_configs[i - 1].m_dis;
                    m_configs[i].m_prepercent = m_configs[i - 1].m_percent;
                }
            }
        }

        public double GetCurrentPercentByDistance(double _dis)
        {
            for (int i = 0; i < m_configs.Count; i++)
            {
                if (m_configs[i].InCheck(_dis))
                    return m_configs[i].m_prepercent;
            }
            return 0;
        }
    }

    /// <summary>
    /// 根据接球者和持球者距离得到接球效果百分比配置
    /// </summary>
    public class DistanceCoefficientTableConfig
    {
        private List<DistanceDecayItem> m_configs = new List<DistanceDecayItem>();
        public DistanceCoefficientTableConfig()
        {

        }
        public bool InitTable(string _path)
        {
            JsonTable kTable = DataManager.Instance.ReadJsonTable(_path) as JsonTable;
            if (null == kTable)
                return false;

            foreach (var kItem in kTable.ItemList)
            {
                DistanceDecayItem kData = new DistanceDecayItem();
                kData.m_id = kItem.Key;
                kData.m_dis = double.Parse(kData.m_id);
                kData.m_percent = double.Parse(kItem.Value["effect_percentage"]);
                m_configs.Add(kData);
            }

            m_configs.Sort(delegate(DistanceDecayItem kItem1, DistanceDecayItem kItem2)
            {
                if (kItem1.m_dis < kItem2.m_dis)
                    return -1;
                return 1;
            });

            ResetConfig();
            return true;
        }

        private void ResetConfig()
        {
            for (int i = 0; i < m_configs.Count; i++)
            {
                if (i == 0)
                {
                    m_configs[i].m_predis = m_configs[i].m_dis;
                    m_configs[i].m_prepercent = m_configs[i].m_percent;
                }
                else
                {
                    m_configs[i].m_predis = m_configs[i - 1].m_dis;
                    m_configs[i].m_prepercent = m_configs[i - 1].m_percent;
                }
            }
        }

        public double GetCurrentPercentByDistance(double _dis)
        {
            for (int i = 0; i < m_configs.Count; i++)
            {
                if (m_configs[i].InCheck(_dis))
                    return m_configs[i].m_prepercent;
            }
            return 0;
        }
    }

    /// <summary>
    /// 传球拦截人数相关配置集合
    /// </summary>
    public class InterceptCoefficientDataConfig
    {
        private Dictionary<int, InterceptCoefficientData> m_kItemList = new Dictionary<int, InterceptCoefficientData>();
        public InterceptCoefficientDataConfig()
        {
           
        }
        public bool InitTable()
        {
            JsonTable kTable = DataManager.Instance.ReadJsonTable("Tables/Battle/InterceptCoefficient") as JsonTable;
            if (null == kTable)
                return false;

            foreach (var kItem in kTable.ItemList)
            {
                InterceptCoefficientData kData = new InterceptCoefficientData();
                kData.ID = int.Parse(kItem.Key);
                kData.Percent = double.Parse(kItem.Value["effect_percentage"]);
                m_kItemList.Add(kData.ID, kData);
            }
            return true;
        }
        //InterceptCoefficient
        public double GetItem(int iID)
        {
            if (m_kItemList.ContainsKey(iID))
                return m_kItemList[iID].Percent;
            return 0;
        }
    }

    /// <summary>
    /// 接球拦截人数相关配置集合
    /// </summary>
    public class MarkCoefficientDataConfig
    {
        private Dictionary<int, InterceptCoefficientData> m_kItemList = new Dictionary<int, InterceptCoefficientData>();
        public MarkCoefficientDataConfig()
        {
        }

        public bool InitTable()
        {
            JsonTable kTable = DataManager.Instance.ReadJsonTable("Tables/Battle/MarkCoefficient") as JsonTable;
            if (null == kTable)
                return false;

            foreach (var kItem in kTable.ItemList)
            {
                InterceptCoefficientData kData = new InterceptCoefficientData();
                kData.ID = int.Parse(kItem.Key);
                kData.Percent = double.Parse(kItem.Value["effect_percentage"]);
                m_kItemList.Add(kData.ID, kData);
            }
            return true;
        }

        public double GetItem(int iID)
        {
            if (m_kItemList.ContainsKey(iID))
                return m_kItemList[iID].Percent;
            return 0;
        }
    }

    /// <summary>
    /// 战术位置匹配系数集合
    /// </summary>
    public class TacticalPosCoefficientDataConfig
    {
        private Dictionary<int, TacticalPosCoefficientData> m_kItemList = new Dictionary<int, TacticalPosCoefficientData>();
        public TacticalPosCoefficientDataConfig()
        {
            
        }

        public bool InitTable()
        {
            JsonTable kTable = DataManager.Instance.ReadJsonTable("Tables/Battle/TacticalCoefficient") as JsonTable;
            if (null == kTable)
                return false;

            foreach (var kItem in kTable.ItemList)
            {
                TacticalPosCoefficientData kData = new TacticalPosCoefficientData();
                kData.ID = int.Parse(kItem.Key);
                kData.m_forward = double.Parse(kItem.Value["forward"]);
                kData.m_backward = double.Parse(kItem.Value["backward"]);
                kData.m_minfield = double.Parse(kItem.Value["midfield"]);
                kData.m_goalkeeper = double.Parse(kItem.Value["goalkeeper"]);
                m_kItemList.Add(kData.ID, kData);
            }
            return true;
        }
        //
        public TacticalPosCoefficientData GetItem(int iID)
        {
            TacticalPosCoefficientData kData;
            m_kItemList.TryGetValue(iID, out kData);
            return kData;
        }
    }

    public class MidKickOffPosTableConfig
    {
        public Dictionary<int, MidKickOffPosItem> m_kItemList = new Dictionary<int, MidKickOffPosItem>();

        public MidKickOffPosTableConfig()
        {
            
            
        }
        public bool InitTable()
        {
            JsonTable kTable = DataManager.Instance.ReadJsonTable("Tables/Battle/MidKickOffPosTable") as JsonTable;
            if (null == kTable)
                return false;

            foreach (var kItem in kTable.ItemList)
            {
                MidKickOffPosItem kData = new MidKickOffPosItem();
                kData.ID = int.Parse(kItem.Key);
                String[] strCourtPosX = kItem.Value["courtPos_x"].Split(' ');
                String[] strCourtPosZ = kItem.Value["courtPos_z"].Split(' ');
                String[] strHomePosZ = kItem.Value["homepose_z_id"].Split(' ');
                String[] strHomePosX = kItem.Value["homepose_x_id"].Split(' ');
                for (int i = 0; i < strCourtPosX.Length;i++ )
                {
                    double dStartX,dStartZ,dRegionX,dRegionZ;
                    if ("null" == strCourtPosX[i])
                        dStartX = -1;
                    else
                        dStartX = double.Parse(strCourtPosX[i]);
                    
                    if ("null" == strCourtPosZ[i])
                        dStartZ = -1;
                    else
                        dStartZ = double.Parse(strCourtPosZ[i]);

                    if ("null" == strHomePosZ[i])
                        dRegionZ= -1;
                    else
                        dRegionZ = double.Parse(strHomePosZ[i]);

                    if ("null" == strHomePosX[i])
                        dRegionX = -1;
                    else
                        dRegionX = double.Parse(strHomePosX[i]);

                    kData.m_datas.Add(new PlayerPositionData(dStartX,dStartZ,dRegionX,dRegionZ));
                }
                    m_kItemList.Add(kData.ID, kData);
            }
            return true;
        }
        //

        public MidKickOffPosItem GetItem(int iID)
        {
            MidKickOffPosItem kItem;
            m_kItemList.TryGetValue(iID, out kItem);
            return kItem;
        }
    }

    public class HomePositionZDataConfig
    {
        public Dictionary<int, HomePositionZData> m_configs = new Dictionary<int, HomePositionZData>();
        public HomePositionZDataConfig()
        {

        }

        public bool InitTable()
        {
              JsonTable kTable = DataManager.Instance.ReadJsonTable("Tables/Battle/HomePosition") as JsonTable;
            if (null == kTable)
                return false;

            foreach (var kItem in kTable.ItemList)
            {
                HomePositionZData _data = new HomePositionZData();
                _data.m_zId = int.Parse(kItem.Value["id"]);
                _data.m_attackMin = double.Parse(kItem.Value["attack_min"]);
                _data.m_attackMax = double.Parse(kItem.Value["attack_max"]);
                _data.m_defineMin = double.Parse(kItem.Value["defence_min"]);
                _data.m_defineMax = double.Parse(kItem.Value["defence_max"]);
                m_configs.Add(_data.m_zId,_data);
            }

            return true;
        }


        public HomePositionZData GetHomePositionZDataByLineId(int lineId)
        {
            HomePositionZData _data;
            m_configs.TryGetValue(lineId,out _data);
            return _data;
        }
    }
}
