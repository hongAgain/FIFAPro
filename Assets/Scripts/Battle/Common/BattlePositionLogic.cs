using BehaviourTree;
using Common;
using Common.Log;
using Common.Tables;
using System;
using System.Collections.Generic;

/// <summary>
/// 球员球场站位
/// </summary>
public class BattlePositionLogic
{


    public class PlayerPositionData
    {
        public bool IsGoalKeeper = false;
        public int m_playerPos = 0;
        public Vector3D m_playerPostion = Vector3D.zero;
    }

    public class TeamBattleKeyData
    {
        public ETeamColor m_color;
        public int m_formationId;
        public int m_kickOffIndex;
        public List<PlayerPositionData> m_playerDatas = new List<PlayerPositionData>();

        public PlayerPositionData GetPlayerPositionByPosIndex(int _index,int _posIndex)
        {
           
            for(int i = 0;i<m_playerDatas.Count;++i)
            {
                if (_index == i&&_posIndex==m_playerDatas[i].m_playerPos)
                {
                    return m_playerDatas[i];
                }
               
            }
            return null;
        }
    }

    public void initBattlePlayerLogic(LLTeam _team)
    {
        m_TeamData = InitTeamData(_team);
        m_midKickControlTeamData = TableManager.Instance.BattlePosTbl.GetFormationTable(_team.TeamInfo.ForamtionID, StandType.MidKick_Control);
        m_BattleRunControlTeamData = TableManager.Instance.BattlePosTbl.GetFormationTable(_team.TeamInfo.ForamtionID, StandType.BattleRun_Control);
        m_midKickNoControlTeamData = TableManager.Instance.BattlePosTbl.GetFormationTable(_team.TeamInfo.ForamtionID, StandType.MidKick_NoControl);
        m_BattleRunNoControlTeamData = TableManager.Instance.BattlePosTbl.GetFormationTable(_team.TeamInfo.ForamtionID, StandType.BattleRun_NoControl);
        m_BattleTables.Add(m_midKickControlTeamData);
        m_BattleTables.Add(m_midKickNoControlTeamData);
        m_BattleTables.Add(m_BattleRunControlTeamData);
        m_BattleTables.Add(m_BattleRunNoControlTeamData);


        m_InsideX = TableManager.Instance.BattleInfoTable.GetItem("XWidth").Value;
        m_InsideZ = TableManager.Instance.BattleInfoTable.GetItem("ZWidth").Value;
        m_WideX = TableManager.Instance.BattleInfoTable.GetItem("GroundWidth").Value;
        m_WideZ = TableManager.Instance.BattleInfoTable.GetItem("GroundLength").Value;
        m_radiusHomeposition = TableManager.Instance.AIConfig.GetItem("homeposition_random_radius").Value;
        
    }

    public void ResetBaseHomepositionDeltx()
    {
        double _deltx = FIFARandom.GetRandomValue(-m_radiusHomeposition, m_radiusHomeposition);
        double _deltz = FIFARandom.GetRandomValue(-m_radiusHomeposition, m_radiusHomeposition);
        m_VBaseHomepositionDeltx = new Vector3D(_deltx,0,_deltz);
    }

    public void RemoveBaseHomepositionDeltx()
    {
        m_VBaseHomepositionDeltx = Vector3D.zero;
    }
    private TeamBattleKeyData InitTeamData(LLTeam _team)
    {
        TeamBattleKeyData _teamData = new TeamBattleKeyData();
        _teamData.m_color = _team.TeamColor;
        _teamData.m_formationId = _team.TeamInfo.ForamtionID;
        List<LLUnit> _teamUints = new List<LLUnit>();
        _teamUints.Add(_team.GoalKeeper as LLUnit);
        foreach (LLPlayer _p in _team.PlayerList)
        {
            _teamUints.Add(_p as LLUnit);
        }

        for (int i = 0; i < _teamUints.Count; ++i)
        {
            PlayerPositionData _pData = new PlayerPositionData();
            if (_teamUints[i] is LLGoalKeeper)
                _pData.IsGoalKeeper = true;
            else
                _pData.IsGoalKeeper = false;
            _pData.m_playerPos = _teamUints[i].PlayerBaseInfo.PosID;
            _pData.m_playerPostion = Vector3D.zero;
            _teamData.m_playerDatas.Add(_pData);
        }
        return _teamData;
    }


    public TeamBattleKeyData UpdateTeamBaseHomePosition(LLTeam _team, StandType _sType, Vector3D _ballPosition)
    {
        BattlePosItem _userTable = new BattlePosItem();
        List<BattlePosItem> _useList = new List<BattlePosItem>();
        TeamBattleKeyData _keyData = new TeamBattleKeyData();
        _useList = m_BattleTables;
        _keyData = m_TeamData;
        _userTable = _useList[(int)_sType - 1];
        //确定球位置比例//
        double _xPercent = (_ballPosition.X + m_InsideX / 2) / m_InsideX;
        double _zPercent = (_ballPosition.Z + m_InsideZ / 2) / m_InsideZ;
        _keyData.m_kickOffIndex = _userTable.m_MidlleKickIndex;
        for (int i = 0; i < _keyData.m_playerDatas.Count; i++)
        {
            BattlePostionData _bPdata =GetPositionData(_keyData.m_playerDatas[i].m_playerPos, _userTable,i);
            if (_bPdata != null)
            {
                _keyData.m_playerDatas[i] = ResetPlayerPosition(_xPercent, _zPercent, _bPdata, _keyData.m_playerDatas[i]);
            }
            else
            {
                LogManager.Instance.RedLog("_keyData.m_playerDatas[i].m_playerPos===" + _keyData.m_playerDatas[i].m_playerPos);
            }
        }


        return _keyData;
    }

    private PlayerPositionData ResetPlayerPosition(double _xP, double _zP, BattlePostionData _pData, PlayerPositionData _data)
    {
        double _insideHalfLength = _pData.m_lengthLeft / 2;
        double _insideHalfWidth = _pData.m_lengthRight / 2;
        double _x = 0d;
        double _z = 0d;
        if (m_TeamData.m_color == ETeamColor.Team_Red)
        {
            _x = _pData.m_pos.X;
            _z = _pData.m_pos.Z;
        }
        else
        {
            _x = -_pData.m_pos.X;
            _z = -_pData.m_pos.Z;
        }

        if (_zP * _pData.m_lengthLeft >= _insideHalfLength)
        {
            _z += (_zP * _pData.m_lengthLeft - _insideHalfLength);
        }
        else
            _z -= (_insideHalfLength - _zP * _pData.m_lengthLeft);

        if (_xP * _pData.m_lengthRight >= _insideHalfWidth)
        {
            _x += (_xP * _pData.m_lengthRight - _insideHalfWidth);
        }
        else
            _x -= (_insideHalfWidth - _xP * _pData.m_lengthRight);
        if(false == _data.IsGoalKeeper)
        {
            _x += m_VBaseHomepositionDeltx.X;
            _z += m_VBaseHomepositionDeltx.Z;
        }
        if (Math.Abs(_x) > m_WideX / 2)
        {
            if (_x > 0)
            {
                _x = m_WideX / 2;
            }
            else
                _x = -m_WideX / 2;
        }
        if (Math.Abs(_z) > m_WideZ / 2)
        {
            if (_z > 0)
            {
                _z = m_WideZ / 2;
            }
            else
                _z = -m_WideZ / 2;
        }

        _data.m_playerPostion = new Vector3D(_x, 0, _z);
        return _data;
    }
    private BattlePostionData GetPositionData(int _posIndex, BattlePosItem _table, int _index)
    {
        if (_table != null)
        {
            for (int i = 0; i < _table.m_posDatats.Count; i++)
            {
                if (_table.m_posDatats[i].m_posIndex == _posIndex && _index == i)
                    return _table.m_posDatats[i];
            }
        }
        return null;
    }
    public BattlePosItem GetMatchBPTable(StandType _sType)
    {
        BattlePosItem _table = m_BattleTables[(int)_sType - 1];
        if (_table != null)
        {
            return _table;
        }
        else
        {
            LogManager.Instance.RedLog("This data not contain kickoffopsition");
        }
        return null;
    }
    public List<int> GetSeleckPlayerMiddle(StandType _sType)
    {
        BattlePosItem _table = m_BattleTables[(int)_sType - 1];
        if(_table!=null)
        {
            return _table.m_MiddleKickList;
        }
        else
        {
            LogManager.Instance.RedLog("This data not contain kickoffopsition");
        }
        return null;
    }


    private double m_InsideX = 0d;
    private double m_InsideZ = 0d;
    private double m_WideX = 0d;
    private double m_WideZ = 0d;
    private double m_radiusHomeposition = 0d;
    private Vector3D m_VBaseHomepositionDeltx = Vector3D.zero;
    // 红队 //
    private TeamBattleKeyData m_TeamData = new TeamBattleKeyData();

    private List<BattlePosItem> m_BattleTables = new List<BattlePosItem>();

    private BattlePosItem m_midKickControlTeamData = new BattlePosItem();
    private BattlePosItem m_BattleRunControlTeamData = new BattlePosItem();
    private BattlePosItem m_midKickNoControlTeamData = new BattlePosItem();
    private BattlePosItem m_BattleRunNoControlTeamData = new BattlePosItem();
}
