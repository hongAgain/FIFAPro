using System.Collections.Generic;

namespace Common.Tables
{
    public enum ECameraType
    {
        Play_Goal_Line = 1,
        Camera_Goal_Line,
    }
    public enum ECameraStartType
    {
        Event_Start = 1,
        Ani_Ball_In,
        Ani_Ball_Out
    }
    public enum ECameraEndType
    {
        Event_End = 1,
        Ani_Ball_Out,
        Ani_Ball_Finished
    }
    public class CameraEffectItem
    {
        public int ID;
        public string Name;
        public ECameraType AngleType;      // 镜头夹角类型
        public ECameraStartType StartType;      // 镜头特效开始方式
        public ECameraEndType EndType;          // 镜头特效结束方式
        public double StartLen;                 // 开始长度
        public double EndLen;                   // 结束长度   
        public double LenTime;                  // 长度缩进时间 
        public double StartAzimuthAngle;
        public double EndAzimuthAngle;
        public double AngleTime;                // 角度变化所需要时间
        public double LenDelayTime;             // 距离延时
        public double AngleDelayTime;           // 角度变换延时
        public bool BlackScreen;                // 是否黑屏
        public double CameraSpeed;              // 相机速度
        public double ElevationAngle;             // 仰角
        public List<int> FxIDList = new List<int>();            // 特效列表
        public int GhostFxID;                   // 残影特效ID
        public double ScaleTime;                // 帧冻结时间缩放比
        public double ScaleTimeStart;           // 帧冻结开始时间
        public double ScaleTimeDuration;        // 帧冻结持续时间
        public double TargetHeight;             // 目标点高度
    }
    public class CameraEffectTable
    {
        public CameraEffectTable()
        {

        }

        public bool InitTable()
        {
            JsonTable kTable = DataManager.Instance.ReadJsonTable("Tables/Battle/CameraEffect") as JsonTable;
            if (null == kTable)
                return false;

            foreach (var kItem in kTable.ItemList)
            {
                CameraEffectItem kEffectItem = new CameraEffectItem();
                kEffectItem.ID = int.Parse(kItem.Key);
                kEffectItem.Name = kItem.Value["name"];
               
                string strVal = null;

                kItem.Value.TryGetValue("angle_type", out strVal);
                if (string.IsNullOrEmpty(strVal))
                    kEffectItem.AngleType = ECameraType.Play_Goal_Line;
                else
                    kEffectItem.AngleType = (ECameraType)(int.Parse(strVal));

                kItem.Value.TryGetValue("start_type", out strVal);
                if (string.IsNullOrEmpty(strVal))
                    kEffectItem.StartType = ECameraStartType.Event_Start;
                else
                    kEffectItem.StartType = (ECameraStartType)(int.Parse(strVal));

                kItem.Value.TryGetValue("end_type", out strVal);
                if (string.IsNullOrEmpty(strVal))
                    kEffectItem.EndType = ECameraEndType.Event_End;
                else
                    kEffectItem.EndType = (ECameraEndType)(int.Parse(strVal));

                kItem.Value.TryGetValue("start_len", out strVal);
                if (string.IsNullOrEmpty(strVal))
                    kEffectItem.StartLen = 0;
                else
                    kEffectItem.StartLen = double.Parse(strVal);

                kItem.Value.TryGetValue("end_len", out strVal);
                if (string.IsNullOrEmpty(strVal))
                    kEffectItem.EndLen = 0;
                else
                    kEffectItem.EndLen = double.Parse(strVal);

                kItem.Value.TryGetValue("start_azimuth_angle", out strVal);
                if (string.IsNullOrEmpty(strVal))
                    kEffectItem.StartAzimuthAngle = 0;
                else
                    kEffectItem.StartAzimuthAngle = double.Parse(strVal);

                kItem.Value.TryGetValue("end_azimuth_angle", out strVal);
                if (string.IsNullOrEmpty(strVal))
                    kEffectItem.EndAzimuthAngle = 0;
                else
                    kEffectItem.EndAzimuthAngle = double.Parse(strVal);

                kItem.Value.TryGetValue("pos_time", out strVal);
                if (string.IsNullOrEmpty(strVal))
                    kEffectItem.LenTime = 0;
                else
                    kEffectItem.LenTime = double.Parse(strVal);

                kItem.Value.TryGetValue("angle_time", out strVal);
                if (string.IsNullOrEmpty(strVal))
                    kEffectItem.AngleTime = 0;
                else
                    kEffectItem.AngleTime = double.Parse(strVal);

                kItem.Value.TryGetValue("pos_delay_time", out strVal);
                if (string.IsNullOrEmpty(strVal))
                    kEffectItem.LenDelayTime = 0;
                else
                    kEffectItem.LenDelayTime = double.Parse(strVal);

                kItem.Value.TryGetValue("angle_delay_time", out strVal);
                if (string.IsNullOrEmpty(strVal))
                    kEffectItem.AngleDelayTime = 0;
                else
                    kEffectItem.AngleDelayTime = double.Parse(strVal);

                kItem.Value.TryGetValue("black_screen", out strVal);
                if (string.IsNullOrEmpty(strVal))
                    kEffectItem.BlackScreen = false;
                else
                    kEffectItem.BlackScreen = TableUtil.GetBoolean(strVal);
               
                kItem.Value.TryGetValue("scale_time", out strVal);
                if (string.IsNullOrEmpty(strVal))
                    kEffectItem.ScaleTime = 1;
                else
                    kEffectItem.ScaleTime = double.Parse(strVal);

                kItem.Value.TryGetValue("scale_time_start", out strVal);
                if (string.IsNullOrEmpty(strVal))
                    kEffectItem.ScaleTimeStart = 0;
                else
                    kEffectItem.ScaleTimeStart = double.Parse(strVal);

                kItem.Value.TryGetValue("scale_time_duration", out strVal);
                if (string.IsNullOrEmpty(strVal))
                    kEffectItem.ScaleTimeDuration = 0;
                else
                    kEffectItem.ScaleTimeDuration = double.Parse(strVal);

                kItem.Value.TryGetValue("ghost_fx_id", out strVal);
                if (string.IsNullOrEmpty(strVal))
                    kEffectItem.GhostFxID = 0;
                else
                    kEffectItem.GhostFxID = int.Parse(strVal);

                kItem.Value.TryGetValue("camera_speed", out strVal);
                if (string.IsNullOrEmpty(strVal))
                    kEffectItem.CameraSpeed = 0;
                else
                    kEffectItem.CameraSpeed = double.Parse(strVal);

                kItem.Value.TryGetValue("elevation_angle", out strVal);
                if (string.IsNullOrEmpty(strVal))
                    kEffectItem.ElevationAngle = 0;
                else
                    kEffectItem.ElevationAngle = double.Parse(strVal);

                kItem.Value.TryGetValue("elevation_height_angle", out strVal);
                if (string.IsNullOrEmpty(strVal))
                    kEffectItem.TargetHeight = 0;
                else
                    kEffectItem.TargetHeight = double.Parse(strVal);

                

                kItem.Value.TryGetValue("fx_id", out strVal);
                if (false == string.IsNullOrEmpty(strVal))
                {
                    string[] strList = strVal.Split(' ');
                    for (int i = 0; i < strList.Length; i++)
                    {
                        int iVal = -1;
                        if ("null" != strList[i])
                            iVal = int.Parse(strList[i]);
                        kEffectItem.FxIDList.Add(iVal);
                    }
                }

                m_kItemList.Add(kEffectItem.ID, kEffectItem);
            }
            return true;
        }
        
        public CameraEffectItem GetItem(int iID)
        {
            CameraEffectItem kItem;
            m_kItemList.TryGetValue(iID, out kItem);
            return kItem;
        }


        protected Dictionary<int, CameraEffectItem> m_kItemList = new Dictionary<int, CameraEffectItem>();
    }
}
