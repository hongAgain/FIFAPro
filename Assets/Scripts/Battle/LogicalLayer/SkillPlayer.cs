using BehaviourTree;
using Common;
using Common.Log;
using Common.Tables;
using System;
using System.Collections.Generic;

class SkillPlayer
{
    class SSkillItem
    {
        public SSkillItem(SkillAppearItem kItem)
        {
            Item = kItem;
            ElapseTime = 0;
        }

        public double ElapseTime;
        public SkillAppearItem Item;
    }

    public SkillPlayer(LLUnit kUnit)
    {
        m_kOwner = kUnit;
        m_kSkillAppearTable = TableManager.Instance.SkillAppearTbl;
    }


    private void UpdateSkill(double dTime)
    {
        for (int i = m_kSkillList.Count - 1; i >= 0; i--)
        {
            SSkillItem kItem = m_kSkillList[i];
            if (kItem.ElapseTime > kItem.Item.DurationTime)
            {
                EndSkill(kItem);
            }
            else
                kItem.ElapseTime += dTime;
        }
    }

    public void Update(double dTime)
    {
        UpdateSkill(dTime);
    }

    public void CastSkill(EEventType kEvtType)
    {
        int iSkillID = -1;
        bool bRet = CanCastSkill(kEvtType, ref iSkillID);
        if (false == bRet)
            return;
        BeginSkill(iSkillID);
    }

    private bool CanCastSkill(EEventType kEvtType,ref int iSkillID)
    {
        if (null == m_kOwner)
            return false;
        //if (false == CheckCondition(kEvtType))
        //    return false; 
        PlayerInfo kInfo = m_kOwner.PlayerBaseInfo;
        List<SkillItem> kSkillItemList = new List<SkillItem>();
        for (int i = 0; i < kInfo.SkillList.Count; i++)
        {
            SkillItem kItem = TableManager.Instance.SkillTbl.GetItem(kInfo.SkillList[i].ID);
            if (null == kItem)
                continue;
            if (kItem.SkillType == kEvtType)
                kSkillItemList.Add(kItem);
        }

        if (kSkillItemList.Count == 0)
            return false;
        //技能i的触发概率=pi/(p1+p2+…+pi)*min(1,p1+p2+p3+…+pi)
        double dValPr = 0;
        for (int i = 0; i < kSkillItemList.Count; i++)
        {
            dValPr += (kSkillItemList[i].SkillRate + kSkillItemList[i].SkillRateStep*m_kOwner.PlayerBaseInfo.Attri.lv);
        }
        dValPr = dValPr * Math.Min(1, dValPr);
        double dRandVal = FIFARandom.GetRandomValue(0, 1);
        double dCurPr = 0;
        for (int i = 0; i < kSkillItemList.Count; i++)
        {
            dCurPr += (kSkillItemList[i].SkillRate + kSkillItemList[i].SkillRateStep * m_kOwner.PlayerBaseInfo.Attri.lv) / dValPr;
            if (dRandVal < dCurPr)
            {
                iSkillID = kSkillItemList[i].ID;
                return true;
            }
        }
        return true;
    }
    /// <summary>
    /// 判断技能释放条件是否满足
    /// </summary>
    /// <returns></returns>
    private bool CheckCondition(EEventType kEvtType)
    {
        switch(kEvtType)
        {
            case EEventType.ET_All:
            case EEventType.ET_Shoot:
            case EEventType.ET_HeadShoot:
            case EEventType.ET_CatchBall:
            case EEventType.ET_ShootSuccess:
            case EEventType.ET_LongPassBall:
            case EEventType.ET_BreakThrough:
         //   case ESkillType.ST_Tackle:
            case EEventType.ET_Block:
            case EEventType.ET_Mark:
            case EEventType.ET_FarShoot:
                return true;
            case EEventType.ET_OpCatchBall:   //对方接球事件
                return true;
        }
        return true;
    }

    // 开始释放技能
    public void BeginSkill(int iSkillID)
    {
        SkillItem kSkillItem = TableManager.Instance.SkillTbl.GetItem(iSkillID);
        if (null == kSkillItem)
            return;
        SkillAppearItem kSAItem = m_kSkillAppearTable.GetItem(kSkillItem.EffectID);
        m_kSkillList.Add(new SSkillItem(kSAItem));
        if (kSAItem.CameraFxIDList.Count > 0)
        {
            CameraFxBeginMessage kMsg = new CameraFxBeginMessage(kSAItem.CameraFxIDList, m_kOwner,kSAItem.DurationTime, iSkillID);
            MessageDispatcher.Instance.SendMessage(kMsg);
        }

        for (int i = 0; i < kSAItem.FXIDList.Count; i++)
        {
            BaseFxBeginMessage kMsg = new BaseFxBeginMessage(m_kOwner, kSAItem.FXIDList[i], iSkillID);
            MessageDispatcher.Instance.SendMessage(kMsg);
        }

        if(null != m_kOwner)
        {
            if (kSAItem.AnimIDList.Count > 0)
            {
                m_kOwner.IsChangeAniID = true;
                m_kOwner.SkillAniItem = kSAItem;
            }
        }
    }

    // 结束释放技能
    private void EndSkill(SSkillItem kItem)
    {
        SkillAppearItem kSAItem = kItem.Item;
        if (kSAItem.CameraFxIDList.Count > 0)
        {
            CameraFxEndMessage kMsg = new CameraFxEndMessage();
            MessageDispatcher.Instance.PostMessage(kMsg);
        }

        for (int i = 0; i < kSAItem.FXIDList.Count; i++)
        {
            BaseFxEndMessage kMsg = new BaseFxEndMessage(m_kOwner, kSAItem.FXIDList[i]);
            MessageDispatcher.Instance.PostMessage(kMsg);
        }
        m_kSkillList.Remove(kItem);
        if (null != m_kOwner)
        {
            if (kSAItem.AnimIDList.Count>0)
            {
                m_kOwner.IsChangeAniID = false;
                m_kOwner.SkillAniItem = null;
            }
                
        }
    }

    private bool CanCastSkill(int iSkillID, string strID)
    {

        if (false == m_kOwner.ContainSkill(iSkillID))
        {
            LogManager.Instance.Log("Unit not support skill ID = {0}", iSkillID);
            return false;
        }

        if (null == m_kSkillAppearTable)
        {
            LogManager.Instance.Log("Skill Table is null");
            return false;
        }

        SkillItem kSkillItem = TableManager.Instance.SkillTbl.GetItem(iSkillID);
        if (null == kSkillItem)
            return false;
        SkillAppearItem kSAItem = m_kSkillAppearTable.GetItem(kSkillItem.EffectID);
        if (null == kSAItem)
        {
            LogManager.Instance.Log("Skill ID = {0} not exisit", iSkillID);
            return false;
        }

        return false;
    }

    private SkillAppearTable m_kSkillAppearTable = null;
    private LLUnit m_kOwner = null;
    private List<SSkillItem> m_kSkillList = new List<SSkillItem>();
    
}