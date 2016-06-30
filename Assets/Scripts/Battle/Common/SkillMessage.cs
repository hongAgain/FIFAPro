using System.Collections.Generic;

namespace Common
{
    public class SkillBeginMessage : Message
    {
        public SkillBeginMessage(LLUnit kUnit) :
            base(MessageType.SkillBegin)
        {
            m_kUnit = kUnit;
        }

        public LLUnit Unit
        {
            get { return m_kUnit; }
        }

        public int SkillID
        {
            get { return m_iSkillID; }
        }

        private int m_iSkillID;
        private LLUnit m_kUnit;
    }

    public class SkillEndMessage : Message
    {
        public SkillEndMessage(LLUnit kUnit) :
            base(MessageType.SkillEnd)
        {
            m_kUnit = kUnit;
        }

        public LLUnit Unit
        {
            get { return m_kUnit; }
        }
        public int SkillID
        {
            get { return m_iSkillID; }
        }

        private int m_iSkillID;
        private LLUnit m_kUnit;
    }

    public class CameraFxBeginMessage : Message
    {
        public CameraFxBeginMessage(List<int> iID,LLUnit kUnit,double dSkillTime,int iSkillID) 
            : base(MessageType.CameraFxBegin)
        {
            m_iSkillID = iSkillID;
            m_kFxIDList = iID;
            m_kUnit = kUnit;
            m_dSkillTime = dSkillTime;
        }

        public int SkillID
        {
            get { return m_iSkillID; }
        }

        public List<int> IDList
        {
            get { return m_kFxIDList; }
            set { m_kFxIDList = value; }
        }

        public LLUnit Unit
        {
            get { return m_kUnit; }
            set { m_kUnit = value; }
        }

        public double SkillTime
        {
            get { return m_dSkillTime; }
            set { m_dSkillTime = value; }
        }

        private int m_iSkillID = -1;
        private LLUnit m_kUnit = null;
        private List<int> m_kFxIDList;
        private double m_dSkillTime;
    }

    public class CameraFxEndMessage : Message
    {
        public CameraFxEndMessage()
            : base(MessageType.CameraFxEnd)
        {
        }
    }


    public class GhostFxBeginMessage : Message
    {
        public GhostFxBeginMessage(int iID,LLUnit kUnit)
            : base(MessageType.GhostFxBegin)
       {
            m_kFxID = iID;
            m_kUnit = kUnit;
        }
        public int ID
        {
            get { return m_kFxID; }
            set { m_kFxID = value; }
        }

        public LLUnit Unit
        {
            get { return m_kUnit; }
            set { m_kUnit = value; }
        }
        private int m_kFxID;
        private LLUnit m_kUnit;
    }

    public class GhostFxEndMessage : Message
    {
        public GhostFxEndMessage(LLUnit kUnit)
            : base(MessageType.GhostFxEnd)
        {
            m_kUnit = kUnit;
        }
       
        public LLUnit Unit
        {
            get { return m_kUnit; }
            set { m_kUnit = value; }
        }
        private LLUnit m_kUnit;
    }
    public class BaseFxEndMessage : Message
    {
        public BaseFxEndMessage(LLUnit kUnit, int iID)
            : base(MessageType.BaseFxEnd)
        {
            m_kUnit = kUnit;
            m_iID = iID;
        }

        public int ID
        {
            get { return m_iID; }
            set { m_iID = value; }
        }

        public LLUnit Unit
        {
            get { return m_kUnit; }
            set { m_kUnit = value; }
        }

        private int m_iID;   // 特效的ID
        private LLUnit m_kUnit;     // 球员对象
    }

    public class BaseFxBeginMessage : Message
    {
        public BaseFxBeginMessage(LLUnit kUnit ,int iID,int iSkillID)
            : base(MessageType.BaseFxBegin)
        {
            m_kUnit = kUnit;
            m_iID = iID;
            m_iSkillID = iSkillID;
        }

        public int ID
        {
            get { return m_iID; }
            set { m_iID = value; }
        }

        public LLUnit Unit
        {
            get { return m_kUnit; }
            set { m_kUnit = value; }
        }

        public int SkillID
        {
            get { return m_iSkillID; }
        }
        
        private int m_iID;   // 特效的ID
        private LLUnit m_kUnit;         // 球员对象
        private int m_iSkillID;  // 技能ID
    }

    public class FrameFrozenBeginMessage : Message
    {
        public FrameFrozenBeginMessage(double dScaleTime)
            :base(MessageType.FrameFrozenBegin)
        {
            m_dScaleTime = dScaleTime;
        }

        public void AddUnit(LLUnit kUnit)
        {
            m_kUnitList.Add(kUnit);
        }
        public List<LLUnit> UnitList
        {
            get { return m_kUnitList; }
            set { m_kUnitList = value; }
        }

        public double ScaleTime
        {
            get { return m_dScaleTime; }
            set { m_dScaleTime = value; }
        }
       

        private double m_dScaleTime;                                // 帧冻结时间缩放比
        private List<LLUnit> m_kUnitList = new List<LLUnit>();
    }


    public class FrameFrozenEndMessage : Message
    {
        public FrameFrozenEndMessage(double dScaleTime)
            : base(MessageType.FrameFrozenEnd)
        {
            m_dScaleTime = dScaleTime;
        }

        public void AddUnit(LLUnit kUnit)
        {
            m_kUnitList.Add(kUnit);
        }
        public List<LLUnit> UnitList
        {
            get { return m_kUnitList; }
            set { m_kUnitList = value; }
        }

        public double ScaleTime
        {
            get { return m_dScaleTime; }
            set { m_dScaleTime = value; }
        }

        private double m_dScaleTime;                                // 帧冻结时间缩放比
        private List<LLUnit> m_kUnitList = new List<LLUnit>();
    }



}
