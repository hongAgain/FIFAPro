using Common;
using Common.Log;
using Common.Tables;
using System;
using System.Collections.Generic;

/// <summary>
/// 技能角度类型
/// 1:正向（0-15）
/// 2：斜前（15-75）
/// 3：侧（75-105）
/// 4：斜后（105-165）
/// 5：正后（165-180）
/// </summary>
public enum SkillAngleType
{
    NULL,
    AngleType_Front = 1,
    AngleType_SlantFront,
    AngleType_Slant,
    AngleType_SlantBack,
    AngleType_Back,

}
public class NetAniSkillLogic
{
    /// <summary>
    /// 对于角度的动画Ids
    /// </summary>
    public List<int> m_skillAniIds = new List<int>();

    /// <summary>
    /// 针对动画和目标点的角度控制
    /// </summary>
    public double m_angle = 0d;

    /// <summary>
    /// 启动技能的球员Player
    /// </summary>
    public LLUnit m_kLLUint = null;


    private SkillAngleType m_skillType = SkillAngleType.AngleType_Front;

    /// <summary>
    /// 获取做技能动画，是否需要角度动画旋转
    /// </summary>
    /// <param name="_kplayer"></param>
    /// <param name="_AniId"></param>
    /// <returns></returns>
    public double OnSkillRotateAngle(LLUnit _kplayer, int _index,out int _AniId)
    {
        _AniId = 0;
        m_kLLUint = _kplayer;
        if (m_kLLUint.SkillAniItem != null)
        {
            if (m_kLLUint.SkillAniItem.AnimIDList != null && m_kLLUint.SkillAniItem.AnimIDList.Count > 0)
            {
                if (m_kLLUint.SkillChangeAngle >= 0)
                {
                    _AniId = m_kLLUint.SkillAniItem.AnimIDList[_index].ID;
                    return m_kLLUint.SkillChangeAngle;
                }
                else
                {
                    _AniId = m_kLLUint.SkillAniItem.AnimIDList[_index].ID;
                }
        
            }
            else
            {
                LogManager.Instance.RedLog("OnSkillRotateAngle.....m_kLLUint.SkillAniItem.AnimIDList is null");
            }
        }
        else
        {
            LogManager.Instance.RedLog("OnSkillRotateAngle.....m_kLLUint.SkillAniItem is null");
        }
        return 0d;
    }


    private bool HaveSkillAnimationId(SkillAngleType _type,out  int _index)
    {
        _index = -1;
        for (int i = 0; i < m_kLLUint.SkillAniItem.AnimIDList.Count; ++i)
        {
            if (m_kLLUint.SkillAniItem.AnimIDList[i].AngleType == (int)_type)
            {
                _index = i;
                return true;
            }
        }

            return false;
    }

}
