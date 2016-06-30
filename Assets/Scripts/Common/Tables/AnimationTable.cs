using Common.Log;
using System.Collections.Generic;
namespace Common.Tables
{

    #region  动画表

    /// <summary>
    /// 详细的动画数据
    /// </summary>
    public class AniData
    {
        public int m_aniId;
        public string m_aniName;
        public int m_isMirror;
        public float m_aniBallInTime;
        public float m_aniBallOutTime;
        public float m_aniAllFrameTime;
        public float m_aniBasePlaySpeed;
        public Vector3D m_aniballOutOffsetPos;
        public Vector3D m_aniballInOffsetPos;
        public float m_aniBlendTime;
        public float m_aniMoveSpeed;
        public float m_aniAngle;
        public float m_aniStartFrame;
        public float m_aniEndFrame;
        public bool m_aniLoop;
        public int m_anActionSide; //0:左边，1：右边
        public int m_aniRoungAngle = 0;
    }

    /// <summary>
    /// 动画数据配置表
    /// </summary>
    public class AniDataConfig
    {

        private readonly float m_posOffset = 0.025f;
        public AniDataConfig()
        {

        }

        public bool InitTable()
        {
            JsonTable kTable = DataManager.Instance.ReadJsonTable("Tables/Battle/AnimData") as JsonTable;
            if (null == kTable)
                return false;
            foreach (var kItem in kTable.ItemList)
            {
                AniData _data = new AniData();
                _data.m_aniId = int.Parse(kItem.Value["id"]);
                _data.m_aniName = kItem.Value["name"];
                string strVal = "";
                kItem.Value.TryGetValue("mirror", out strVal);
                if (string.IsNullOrEmpty(strVal))
                    _data.m_isMirror = 0;
                else
                    _data.m_isMirror = int.Parse(strVal);

                kItem.Value.TryGetValue("basic_play_speed", out strVal);
                if (string.IsNullOrEmpty(strVal))
                    _data.m_aniBasePlaySpeed = 0;
                else
                    _data.m_aniBasePlaySpeed = float.Parse(strVal);

                kItem.Value.TryGetValue("ball_in_frame", out strVal);
                if (string.IsNullOrEmpty(strVal))
                    _data.m_aniBallInTime = 0;
                else
                    _data.m_aniBallInTime = (float.Parse(strVal)) / 30f;

                kItem.Value.TryGetValue("ball_out_frame", out strVal);
                if (string.IsNullOrEmpty(strVal))
                    _data.m_aniBallOutTime = 0;
                else
                    _data.m_aniBallOutTime = (float.Parse(strVal)) / 30f;

                kItem.Value.TryGetValue("all_frame", out strVal);
                if (string.IsNullOrEmpty(strVal))
                    _data.m_aniAllFrameTime = 0;
                else
                    _data.m_aniAllFrameTime = float.Parse(strVal) / 30f;

                kItem.Value.TryGetValue("blend_time", out strVal);
                if (string.IsNullOrEmpty(strVal))
                    _data.m_aniBlendTime = _data.m_aniAllFrameTime;
                else
                    _data.m_aniBlendTime = _data.m_aniAllFrameTime - float.Parse(strVal) / 30f;
                
                _data.m_aniballInOffsetPos = StringToVector3D(kItem.Value["delet_coordinate"]);
                _data.m_aniballOutOffsetPos = StringToVector3D(kItem.Value["creat_coordinate"]);

                kItem.Value.TryGetValue("anispeed", out strVal);
                if (string.IsNullOrEmpty(strVal))
                    _data.m_aniMoveSpeed = 0;
                else
                    _data.m_aniMoveSpeed = float.Parse(strVal);

                kItem.Value.TryGetValue("angle", out strVal);
                if (string.IsNullOrEmpty(strVal))
                    _data.m_aniAngle = 0;
                else
                    _data.m_aniAngle = float.Parse(strVal);

                kItem.Value.TryGetValue("start_move_frame", out strVal);
                if (string.IsNullOrEmpty(strVal))
                    _data.m_aniStartFrame = 0;
                else
                    _data.m_aniStartFrame = float.Parse(strVal) / 30f;

                kItem.Value.TryGetValue("end_move_frame", out strVal);
                if (string.IsNullOrEmpty(strVal))
                    _data.m_aniEndFrame = 0;
                else
                    _data.m_aniEndFrame = float.Parse(strVal) / 30f;

                kItem.Value.TryGetValue("loop", out strVal);
                if (string.IsNullOrEmpty(strVal))
                    _data.m_aniLoop = false;
                else
                    _data.m_aniLoop = int.Parse(strVal) == 1 ? true : false;

                kItem.Value.TryGetValue("actionside", out strVal);
                if (string.IsNullOrEmpty(strVal))
                    _data.m_anActionSide = 0;
                else
                    _data.m_anActionSide = int.Parse(strVal);

                kItem.Value.TryGetValue("roundAngle", out strVal);
                if (string.IsNullOrEmpty(strVal))
                    _data.m_aniRoungAngle = 0;
                else
                    _data.m_aniRoungAngle = int.Parse(strVal);

                m_configs.Add(_data.m_aniId, _data);
            }
            return true;
        }

        private Vector3D StringToVector3D(string _str)
        {
            if (!string.IsNullOrEmpty(_str))
            {
                char _split = ',';
                string[] _strs = _str.Split(_split);
                if (_strs.Length < 3)
                    return Vector3D.zero;
                return new Vector3D(double.Parse(_strs[0]) * m_posOffset, double.Parse(_strs[2]) * m_posOffset, -double.Parse(_strs[1]) * m_posOffset);
            }
            return Vector3D.zero;
        }
        public AniData GetAniDataByName(int _id)
        {
            AniData _data;
            m_configs.TryGetValue(_id, out _data);
            return _data;
        }

        public Dictionary<int, AniData> Datas
        {
            get
            {
                return m_configs;
            }
        }
        public Dictionary<int, AniData> m_configs = new Dictionary<int, AniData>();
    }
    #endregion

    #region 行为表
    public class AniBeahavior
    {
        public int m_typeId;
        public int m_combineId;
       
    }

    public class AniBeahaviorConfig
    {
        public AniBeahaviorConfig()
        {

        }

        public bool InitTable()
        {
            JsonTable kTable = DataManager.Instance.ReadJsonTable("Tables/Battle/AnimState") as JsonTable;
            if (null == kTable)
                return false;
            foreach (var kItem in kTable.ItemList)
            {
                AniBeahavior _data = new AniBeahavior();
                _data.m_typeId = int.Parse(kItem.Value["id"]);
                if (kItem.Value["anim_combine"] != string.Empty)
                    _data.m_combineId = int.Parse(kItem.Value["anim_combine"]);
                m_configs.Add(_data.m_typeId, _data);
            }
            return true;
        }
        public AniBeahavior GetAniBeahaviorByAnimationType(int _id)
        {
            AniBeahavior _h;
            m_configs.TryGetValue(_id, out _h);
            return _h;
        }
        public Dictionary<int, AniBeahavior> Datas
        {
            get
            {
                return m_configs;
            }
        }
        private Dictionary<int, AniBeahavior> m_configs = new Dictionary<int, AniBeahavior>();
    }
    #endregion

    #region 行为动画组合表
    public class AniCombine
    {
        public int m_aniCId;
        public List<AniCombineData> m_AniCombineData;
    }

    public class AniCombineData
    {
        public List<int> m_AnimationIds;
        public double m_rate;
        public double m_MinRate;
        public double m_MaxRate;
        public int m_ballInIndex;
        public int m_ballOutIndex;
        public int m_skillIndex;
    }

    public class AniCombineConfig
    {
        private readonly char _point1 = ',';
        private readonly char _point2 = ';';
        private Dictionary<int, AniCombine> m_configs = new Dictionary<int, AniCombine>();
        public AniCombineConfig()
        {

        }

        public bool InitTable()
        {
            JsonTable kTable = DataManager.Instance.ReadJsonTable("Tables/Battle/AnimCombine") as JsonTable;
            if (kTable == null)
                return false;
            foreach (var kItem in kTable.ItemList)
            {
                AniCombine _data = new AniCombine();
                string strCombine, strRate, strKeyPos;
                kItem.Value.TryGetValue("combine", out strCombine);
                kItem.Value.TryGetValue("rate", out strRate);
                kItem.Value.TryGetValue("keyPos", out strKeyPos);
                if (string.IsNullOrEmpty(strCombine) || string.IsNullOrEmpty(strRate))
                    continue;
                _data.m_AniCombineData = ResetAniCombine(strCombine, strRate, strKeyPos);
                _data.m_aniCId = int.Parse(kItem.Value["id"]);
                string _str;
                int _pos = 0;
                kItem.Value.TryGetValue("skillsChangePos", out _str);
                if (string.IsNullOrEmpty(_str))
                {
                    _pos = -1;
                }
                else
                    _pos = int.Parse(_str);
                foreach(var item in _data.m_AniCombineData)
                {
                    item.m_skillIndex = _pos;
                }
                m_configs.Add(_data.m_aniCId, _data);
            }
            return true;
        }


        private List<AniCombineData> ResetAniCombine(string _Idsstr, string _rateStr, string _indexStr)
        {
            List<AniCombineData> _datas = new List<AniCombineData>();

            string[] _sIds = _Idsstr.Split(';');
            if(_sIds.Length > 0)
            {
                string[] _sRates = _rateStr.Split(';');
                double _tmp = 0;
                for (int i = 0; i < _sIds.Length; i++)
                {
                    AniCombineData _data = new AniCombineData();
                    _data.m_AnimationIds = new List<int>();
                    _data.m_rate = double.Parse(_sRates[i]);
                    _data.m_MinRate = _tmp;
                    _data.m_MaxRate = _tmp + _data.m_rate;
                    _tmp += _data.m_rate;
                    _data.m_ballInIndex = 0;
                    _data.m_ballOutIndex = 0;
                    if (_sIds[i].Contains(_point1.ToString()))
                    {
                        string[] _IdItems = _sIds[i].Split(_point1);
                        for (int j = 0; j < _IdItems.Length; j++)
                        {
                            _data.m_AnimationIds.Add(int.Parse(_IdItems[j]));
                        }
                    }
                    else
                    {
                        _data.m_AnimationIds.Add(int.Parse(_sIds[i]));
                    }

                    _datas.Add(_data);
                }
            }
            else
            {
                AniCombineData _data = new AniCombineData();
                _data.m_AnimationIds = new List<int>();
                _data.m_rate = double.Parse(_rateStr);
                double _tmp = 0;
                _data.m_MinRate = _tmp;
                _data.m_MaxRate = _tmp + _data.m_rate;
                _tmp += _data.m_rate;
                if (_Idsstr.Contains(_point1.ToString()))
                {
                    string[] _IdItems = _Idsstr.Split(_point1);
                    for (int j = 0; j < _IdItems.Length; j++)
                    {
                        _data.m_AnimationIds.Add(int.Parse(_IdItems[j]));
                    }
                }
                else
                {
                    _data.m_AnimationIds.Add(int.Parse(_Idsstr));
                }
                _datas.Add(_data);
            }

            ///是否存在关键动画位置数据///
            if (false == string.IsNullOrEmpty(_indexStr))
            {
                string[] _indexs = _indexStr.Split(_point2);
                if(_indexs.Length > 0)
                {
                    if (_indexs.Length > _datas.Count)
                    {
                        LogManager.Instance.RedLog("This combine keypos is error,_Index===" + _indexStr);
                    }
                    else
                    {
                        for (int i = 0; i < _indexs.Length; ++i)
                        {
                            AniCombineData _data = _datas[i];
                            string[] _is = _indexs[i].Split(',');
                            _data.m_ballInIndex = int.Parse(_is[0]);
                            _data.m_ballOutIndex = int.Parse(_is[1]);
                        }
                    }
                }
                
                else
                {
                    if (_datas.Count == 0)
                    {
                        LogManager.Instance.RedLog("This combine keypos is error,_Index===" + _indexStr);
                    }
                    else
                    {
                        string[] _is = _indexStr.Split(',');
                        _datas[0].m_ballInIndex = int.Parse(_is[0]);
                        _datas[0].m_ballOutIndex = int.Parse(_is[1]);
                    }
                }
            }
            return _datas;
        }
        public Dictionary<int, AniCombine> Datas
        {
            get
            {
                return m_configs;
            }
        }
    }
    #endregion

    #region 状态翻译表
    public class AniStateLayer
    {
        public string m_stateName;
        public int m_stateId;
        public int m_stateIndex;
        public List<string> m_preStateIds = new List<string>();
    }


    public class AniStateLayerConfig
    {
        private Dictionary<string, AniStateLayer> m_configs = new Dictionary<string, AniStateLayer>();
        public AniStateLayerConfig()
        {

        }
        public bool InitTable()
        {
            JsonTable kTable = DataManager.Instance.ReadJsonTable("Tables/Battle/AnimStateLayer") as JsonTable;
            if (kTable == null)
                return false;
            foreach (var kItem in kTable.ItemList)
            {
                AniStateLayer _layer = new AniStateLayer();
                string _str;
                kItem.Value.TryGetValue("id", out _str);
                if (string.IsNullOrEmpty(_str))
                {
                    _layer.m_stateName = "";
                }
                else
                {
                    _layer.m_stateName = _str;
                }
                kItem.Value.TryGetValue("type", out _str);
                if (string.IsNullOrEmpty(_str))
                {
                    _layer.m_stateId = 0;
                }
                else
                {
                    _layer.m_stateId = int.Parse(_str);
                }
                kItem.Value.TryGetValue("stateQueue.", out _str);
                if (string.IsNullOrEmpty(_str))
                {
                    _layer.m_stateIndex = 0;
                }
                else
                {
                    _layer.m_stateIndex = int.Parse(_str);
                }

                m_configs.Add(_layer.m_stateName, _layer);
            }
            return true;
        }

        public Dictionary<string, AniStateLayer> Datas
        {
            get
            {
                return m_configs;
            }
        }
    }
    #endregion
}
