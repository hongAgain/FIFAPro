using Common.Tables;
using Common;
using System.Collections.Generic;
using BehaviourTree;
using Common.Log;

/// <summary>
/// 动画查询数据管理
/// Author:Cepheus
/// </summary>
public class MatchAniHelper
{
    private Dictionary<int, AniBeahavior> m_aniBeahaviors = new Dictionary<int, AniBeahavior>();
    private Dictionary<int, AniCombine> m_aniCombines = new Dictionary<int, AniCombine>();
    private Dictionary<int, AniData> m_aniDatas = new Dictionary<int, AniData>();
    private Dictionary<string, AniStateLayer> m_layers = new Dictionary<string, AniStateLayer>();
    private static MatchAniHelper m_instance;

    private MatchAniHelper()
    {
        if (m_aniBeahaviors.Count == 0)
            m_aniBeahaviors = TableManager.Instance.AniBeahaviorConfig.Datas;
        if (m_aniCombines.Count == 0)
            m_aniCombines = TableManager.Instance.AniCombineConfig.Datas;
        if (m_aniDatas.Count == 0)
            m_aniDatas = TableManager.Instance.AniDataConfig.Datas;
        if (m_layers.Count == 0)
        {
            m_layers = TableManager.Instance.AniStateLayerConfig.Datas;
        }
    }

    public static MatchAniHelper Instance
    {
        get
        {
            if (m_instance == null)
                m_instance = new MatchAniHelper();
            return m_instance;
        }
    }

    /// <summary>
    ///通过动画类型来获取相关的动画数据
    /// </summary>
    /// <param name="_type"></param>
    /// <param name="_llcore"></param>
    /// <returns></returns>
    public List<AniData> GetAnimationDataByAniType(int _id, out AniCombineData _data)
    {
        _data = null;
        AniBeahavior _b;
        List<int> _animationIds = new List<int>();
        List<AniData> _anidatas = new List<AniData>();
        m_aniBeahaviors.TryGetValue(_id, out _b);
        if (_b != null)
        {
            AniCombine _c = GetAnimationCombineByID(_b.m_combineId);
            if (_c != null && _c.m_AniCombineData != null && _c.m_AniCombineData.Count > 0)
            {
                int _random = (int)FIFARandom.GetRandomValue(0, 100);
                for (int i = 0; i < _c.m_AniCombineData.Count; ++i)
                {
                    if (_random >= _c.m_AniCombineData[i].m_MinRate && _random <= _c.m_AniCombineData[i].m_MaxRate)
                    {
                        _data = _c.m_AniCombineData[i];
                        _animationIds = _c.m_AniCombineData[i].m_AnimationIds;
                        break;
                    }
                }
            }
            else
            {
                LogManager.Instance.RedLog("MatchAniHelper===>GetAnimationDataByAniType.Not can find this animation combines data,_animationType===" + (EAniState)_id);
                return null;
            }
        }

        if (_animationIds != null && _animationIds.Count > 0)
        {
            for (int i = 0; i < _animationIds.Count; i++)
            {
                AniData _d;
                m_aniDatas.TryGetValue(_animationIds[i], out _d);
                if (_d != null)
                {
                    _anidatas.Add(_d);
                }
                else
                {
                    LogManager.Instance.RedLog(" This state animation data,AnimationId===" + _animationIds[i]);
                }
            }
        }

        return _anidatas;
    }

    private AniCombine GetAnimationCombineByID(int _id)
    {

        foreach (KeyValuePair<int, AniCombine> _item in m_aniCombines)
        {
            if (_item.Key == _id)
            {
                return _item.Value;
            }
        }
        return null;
    }

    public AniStateLayer GetAniIdByStateName(string _stateName)
    {
        AniStateLayer _layer;
        m_layers.TryGetValue(_stateName, out _layer);
        return _layer;
    }


    public AniData GetAniDataByName(string _name)
    {
        foreach (KeyValuePair<int, AniData> kv in m_aniDatas)
        {
            if (kv.Value.m_aniName.Equals(_name))
            {
                return kv.Value;
            }
        }
        return null;
    }
    public AniData GetAniDataById(int id)
    {
        foreach (KeyValuePair<int, AniData> kv in m_aniDatas)
        {
            if (kv.Value.m_aniId.Equals(id))
            {
                return kv.Value;
            }
        }
        return null;
    }
    public AniClipData ResetClipData(AniData _Adata)
    {
        AniClipData _data = new AniClipData();
        _data.AniName = _Adata.m_aniName;
        _data.TurnBack = false;
        _data.BallInTime = _Adata.m_aniBallInTime;
        _data.BallInOffset = _Adata.m_aniballInOffsetPos;
        _data.BallOutTime = _Adata.m_aniBallOutTime;
        _data.BallOutOffset = _Adata.m_aniballOutOffsetPos;
        _data.DelayTime = _Adata.m_aniBlendTime;
        _data.Mirror = false;
        _data.Loop = _Adata.m_aniLoop;
        _data.AllFrameTime = _Adata.m_aniAllFrameTime;
        _data.TargetSpeed = _Adata.m_aniMoveSpeed;
        _data.StartFrame = _Adata.m_aniStartFrame;
        _data.EndFrame = _Adata.m_aniEndFrame;
        _data.AniAngle = _Adata.m_aniAngle;
        _data.AtionSide = _Adata.m_anActionSide;
        _data.AniRoundAngle = _Adata.m_aniRoungAngle;
        return _data;
    }
}
