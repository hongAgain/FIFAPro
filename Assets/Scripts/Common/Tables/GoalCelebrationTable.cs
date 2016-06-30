
using Common.Log;
using System;
using System.Collections.Generic;

////////
///AI配置中，关于传球球员的相关规则数据
////////
namespace Common.Tables
{

    public class GoalPlayerData
    {
        public Vector3D m_pos;
        public double m_rorateAngle;
        public int m_sAniId;
        public AniClipData m_clipData;
    }
    public class GoalCelebrationData
    {
        public int m_id;
        public string m_resName;
        public float m_AniTime;
        public int m_playerCount;
        public Vector3D m_parentPosition;
        public Vector3D m_kCameraPosition;
        public Vector3D m_fCameraAngle;
        public List<GoalPlayerData> m_kPlyData;
        public string m_cameAniName;
    }
    public class GoalCelebrationTable
    {
        public GoalCelebrationTable()
        {
        }

        public bool InitTable()
        {
            JsonTable kTable = DataManager.Instance.ReadJsonTable("Tables/Battle/GoalCelebration") as JsonTable;
            if (null == kTable)
                return false;

            foreach (var kItem in kTable.ItemList)
            {
                GoalCelebrationData _data = new GoalCelebrationData();
                string strVal = "";
                kItem.Value.TryGetValue("id", out strVal);
                if (string.IsNullOrEmpty(strVal))
                {
                    _data.m_id = 0;

                }
                else
                {
                    _data.m_id = int.Parse(strVal);
                }

                kItem.Value.TryGetValue("name", out strVal);
                if (string.IsNullOrEmpty(strVal))
                {
                    _data.m_resName = "";

                }
                else
                {
                    _data.m_resName = strVal;
                }
                kItem.Value.TryGetValue("all_frame", out strVal);
                if (string.IsNullOrEmpty(strVal))
                {
                    _data.m_AniTime = 0;

                }
                else
                {
                    _data.m_AniTime = int.Parse(strVal) / 30f;
                }

                kItem.Value.TryGetValue("parentPos", out strVal);
                if (string.IsNullOrEmpty(strVal))
                {
                    _data.m_parentPosition = Vector3D.zero;

                }
                else
                {
                    string[] _strs = strVal.Split(',');
                    _data.m_parentPosition = new Vector3D(double.Parse(_strs[0]), double.Parse(_strs[1]), double.Parse(_strs[2]));
                }
                kItem.Value.TryGetValue("number", out strVal);
                if (string.IsNullOrEmpty(strVal))
                {
                    _data.m_playerCount = 0;

                }
                else
                {
                    _data.m_playerCount = int.Parse(strVal);
                }

                kItem.Value.TryGetValue("cameraAniName", out strVal);
                if (string.IsNullOrEmpty(strVal))
                {
                    _data.m_cameAniName = "";

                }
                else
                {
                    _data.m_cameAniName = strVal;
                }
                kItem.Value.TryGetValue("cameraPosition",out strVal);
                if(string.IsNullOrEmpty(strVal))
                {
                    _data.m_kCameraPosition = Vector3D.zero;
                    _data.m_fCameraAngle = Vector3D.zero;

                }
                else
                {
                    string[] _strs = strVal.Split(',');
                    _data.m_kCameraPosition = new Vector3D(double.Parse(_strs[0]), double.Parse(_strs[1]), double.Parse(_strs[2]));
                    _data.m_fCameraAngle = new Vector3D(double.Parse(_strs[3]), double.Parse(_strs[4]), double.Parse(_strs[5]));
                }

                //重构进球之后参与动画的数据//
                _data.m_kPlyData = ResetVector3d(kItem, _data.m_playerCount);
                m_configs.Add(_data);
            }
            return true;
        }


        private List<GoalPlayerData> ResetVector3d(KeyValuePair<string, Dictionary<string, string>> _kvc, int _count)
        {
            List<GoalPlayerData> _returns = new List<GoalPlayerData>();
            string _cName1 = "coordinates";
            string _cName2 = "animationId";
            string _strVal = "";
            for (int i = 1; i <= _count; ++i)
            {
                GoalPlayerData _data = new GoalPlayerData();
                _kvc.Value.TryGetValue(_cName1+i,out _strVal);
                if(string.IsNullOrEmpty(_strVal))
                {
                    LogManager.Instance.RedLog("GoalCelebrationTable is wrong,this position is null,you check it");
                }
                else
                {
                    string[] _strs = _strVal.Split(',');
                    _data.m_pos = new Vector3D(double.Parse(_strs[0]), double.Parse(_strs[1]), double.Parse(_strs[2]));
                    _data.m_rorateAngle = double.Parse(_strs[3]);
                }
                _kvc.Value.TryGetValue(_cName2 + i, out _strVal);
                if (string.IsNullOrEmpty(_strVal))
                {
                    LogManager.Instance.RedLog("GoalCelebrationTable is wrong,this is Player animation is null,you check it");
                }
                else
                {
                    _data.m_sAniId = int.Parse(_strVal);
                    AniData _aniData = MatchAniHelper.Instance.GetAniDataById(_data.m_sAniId);
                    _data.m_clipData = MatchAniHelper.Instance.ResetClipData(_aniData);
                }
                _returns.Add(_data);
            }
            return _returns;
        }

        public GoalCelebrationData GetGoalCelebrationDataById(int _id)
        {
            for(int i = 0;i<m_configs.Count;i++)
            {
                if (m_configs[i].m_id == _id)
                    return m_configs[i];
            }
            return null;
        }
        public List<GoalCelebrationData> m_configs = new List<GoalCelebrationData>();
    }
}