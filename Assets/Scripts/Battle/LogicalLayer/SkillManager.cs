using BehaviourTree;
using Common;
using Common.Log;
using Common.Tables;
using System;
using System.Collections.Generic;

class SkillManager
{

    //public void CastTriggerSkill(LLUnit kUnit,ESkillType kSkillType)
    //{
    //    if (kSkillType <= ESkillType.ST_Trigger_End && kSkillType >= ESkillType.ST_Trigger_Begin)
    //        CastTriggerSkill(kUnit, kSkillType);
    //    else if (kSkillType >= ESkillType.ST_Buff_Begin && kSkillType <= ESkillType.ST_Buff_End)
    //        CastBuffSkill(kUnit, kSkillType);
    //    //SkillAppearTable kSkillAppearTbl = TableManager.Instance.SkillAppearTbl;
    //    //SkillAppearItem kSkillItem = kSkillAppearTbl.GetItem(iSkillID);
    //    //if (null == kSkillItem)
    //    //    return;
    //    //if (ESkillEffectType.Trigger == kSkillItem.EffectType)
    //    //    return;
    //    //switch (kSkillItem.SkillTarget)
    //    //{
    //    //    case ESkillTarget.ST_Self_Forward:      // 我方所有前锋
    //    //        break;
    //    //    case ESkillTarget.ST_Self_Middle:       // 我方所有中场
    //    //        break;
    //    //    case ESkillTarget.ST_Self_Back:         // 我方所有后卫
    //    //        break;
    //    //    case ESkillTarget.ST_Self_All:          // 所有我方球员
    //    //        break;
    //    //    case ESkillTarget.ST_Forward:           // 所有我方球员
    //    //        break;
    //    //    case ESkillTarget.ST_Middle:            // 对方所有中场
    //    //        break;
    //    //    case ESkillTarget.ST_Back:              // 对方所有后卫
    //    //        break;
    //    //    case ESkillTarget.ST_All:               // 对方所有球员
    //    //        break;
    //    //    case ESkillTarget.ST_Self_GK:           // 我方门将
    //    //        break;
    //    //    case ESkillTarget.ST_GK:                // 对方门将
    //    //        break;
    //    //    default:
    //    //        break;
    //    //}
    //}

    public void CastTriggerSkill(LLUnit kUnit, EEventType kSkillType)
    {
        if (null == kUnit)
            return;
        kUnit.CastSkill(kSkillType);
    }

    public void CastBuffSkill(EEventType kSkillType )
    {

    }
}