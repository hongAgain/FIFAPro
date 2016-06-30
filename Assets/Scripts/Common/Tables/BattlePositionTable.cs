using Common.Log;
using System.Collections.Generic;
namespace Common.Tables
{
    public enum StandType
    {
        MidKick_Control=1,      // 中场开球控球方
        MidKick_NoControl,
        BattleRun_Control,
        BattleRun_NoControl
    }
    public class BattlePostionData
    {
        public int m_posIndex = 0;
        public Vector3D m_pos = Vector3D.zero;
        public double m_lengthLeft = 0d;
        public double m_lengthRight = 0d;
    }
    public class BattlePosItem
    {
        public int m_Id = 0;
        public int m_type = 0;
        public int m_formation = 0;
        public int m_MidlleKickIndex = 0;
        public List<int> m_MiddleKickList = new List<int>(); //不包含开球球员//
        public List<BattlePostionData> m_posDatats = new List<BattlePostionData>();
    }

    public class BattlePosTable
    {
        public BattlePosTable() { }

        public bool InitTable()
        {
            JsonTable kTable = DataManager.Instance.ReadJsonTable("Tables/Battle/BattlePlayerPosition") as JsonTable;
            if (null == kTable)
                return false;
            foreach (var kItem in kTable.ItemList)
            {
                BattlePosItem _data = new BattlePosItem();
                string strVal = "";
                kItem.Value.TryGetValue("id", out strVal);
                if (string.IsNullOrEmpty(strVal))
                    _data.m_Id = 0;
                else
                    _data.m_Id = int.Parse(strVal);
                kItem.Value.TryGetValue("type", out strVal);
                if (string.IsNullOrEmpty(strVal))
                    _data.m_type = 0;
                else
                    _data.m_type = int.Parse(strVal);

                kItem.Value.TryGetValue("formation", out strVal);
                
                if (string.IsNullOrEmpty(strVal))
                    _data.m_formation = 0;
                else
                    _data.m_formation = int.Parse(strVal);

                kItem.Value.TryGetValue("kickoffposition", out strVal);
                SetKickMiddleData(strVal, _data);
//                 if (string.IsNullOrEmpty(strVal))
//                     _data.m_MidlleKickIndex = 0;
//                 else
//                     _data.m_MidlleKickIndex = int.Parse(strVal);
               _data.m_posDatats =  SetBattleData(kItem);


               m_configs.Add(_data.m_Id, _data);
            }
            return true;
        }

        private void SetKickMiddleData(string _str,BattlePosItem _table)
        {
            if (!string.IsNullOrEmpty(_str))
            {
                if (_str.Contains("&"))
                {
                    string[] _strs = _str.Split('&');
                    _table.m_MidlleKickIndex = int.Parse(_strs[0])-2;
                    for(int i = 1;i<_strs.Length;++i)
                    {
                        int _i = int.Parse(_strs[i])-2;
                        _table.m_MiddleKickList.Add(_i);
                    }
                }
                else
                {
                    _table.m_MidlleKickIndex = int.Parse(_str)-2;
                }
            }


        }

        private List<BattlePostionData> SetBattleData(KeyValuePair<string, Dictionary<string, string>> kItem)
        {
            List<BattlePostionData> _battleDatas = new List<BattlePostionData>();
            string _firstName = "pos";
            string _secondName = "data";
            string _thirdName = "scale";
            int _index = 0;

            foreach (var item in kItem.Value)
            {
                
                _index++;
                if(kItem.Value.Count/3<=_index)
                {
                    break;
                }
                BattlePostionData _data = new BattlePostionData();
                string _valueName1 = "";
                string strVal = "";
                _valueName1 = _firstName + _index;
                kItem.Value.TryGetValue(_valueName1, out strVal);
                if (string.IsNullOrEmpty(strVal))
                    _data.m_posIndex = 0;
                else
                    _data.m_posIndex = int.Parse(strVal);

                string _valueName2 = "";
                _valueName2 = _valueName1 + _secondName;
                kItem.Value.TryGetValue(_valueName2, out strVal);
                if (string.IsNullOrEmpty(strVal))
                    _data.m_pos = Vector3D.zero;
                else
                {
                    Vector3D _v11 = StringToVector3D(strVal);
                    _data.m_pos = new Vector3D(_v11.Z, _v11.Y, _v11.X);
                }
                string _valueName3 = "";
                _valueName3 = _valueName1 + _thirdName;
                kItem.Value.TryGetValue(_valueName3, out strVal);
                if (string.IsNullOrEmpty(strVal))
                {
                    _data.m_lengthLeft = 0;
                    _data.m_lengthRight = 0;
                }
                else
                {
                    Vector3D _v = StringToVector3D(strVal);
                    _data.m_lengthLeft = _v.X;
                    _data.m_lengthRight = _v.Z;
                }
                _battleDatas.Add(_data);
            }
            return _battleDatas;
        }

        public BattlePosItem GetFormationTable(int _fId,StandType _sType)
        {
            foreach (KeyValuePair<int ,BattlePosItem> _t in m_configs)
            {
                if (_t.Value.m_formation == _fId && _t.Value.m_type == (int)_sType)
                    return _t.Value;
            }
            return null;
        }

        private Vector3D StringToVector3D(string _str)
        {
            if (!string.IsNullOrEmpty(_str))
            {
                char _split = '&';
                string[] _strs = _str.Split(_split);
                if (_strs.Length < 2)
                    return Vector3D.zero;
                return new Vector3D(double.Parse(_strs[0]), 0, double.Parse(_strs[1]));
            }
            return Vector3D.zero;
        }

        public Dictionary<int, BattlePosItem> Datas
        {
            get
            {
                return m_configs;
            }
        }
        public Dictionary<int, BattlePosItem> m_configs = new Dictionary<int, BattlePosItem>();
    }
}