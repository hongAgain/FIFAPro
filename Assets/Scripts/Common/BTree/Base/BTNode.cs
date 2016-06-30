using LitJson;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace BehaviourTree
{
    public abstract class BTNode
    {
        public virtual void Activate(BTDatabase kDatabase)
        {
            m_kDatabase = kDatabase;
            for(int i=0;i< m_kChildList.Count;i++)
            {
                m_kChildList[i].Activate(m_kDatabase);
            }
        }

        public virtual BTResult Tick(double fTime) 
        { 
            return BTResult.Failed;
        }

        public virtual void Clear()
        {
            for (int i = 0; i < m_kChildList.Count; i++)
            {
                m_kChildList[i].Clear();
            }
            IsRunnint = false;
        }


        public void AddChild(BTNode kNode)
        {
            kNode.Parent = this;
            m_kChildList.Add(kNode);
        }

        public void RemoveChild(BTNode kNode)
        {
            if(null != kNode)
                kNode.Parent = null;
            m_kChildList.Remove(kNode);
        }

        public virtual void Load(JsonData kInData)
        {
            NodeType = kInData[BTConstant.Type].ToString();
            Name = kInData[BTConstant.Name].ToString();
            DisplayName = kInData[BTConstant.DisplayName].ToString();

            JsonData kArg = kInData[BTConstant.Argument];
            FieldInfo[] kInfoList = GetType().GetFields();
            for(int iIdx = 0;iIdx < kInfoList.Length;iIdx++)
            {
                FieldInfo kInfo = kInfoList[iIdx];
                if(kArg.Keys.Contains(kInfo.Name))
                {
                    string strName = kArg[kInfo.Name].ToString();
                    object kObj = null;
                    if(typeof(int) == kInfo.FieldType)
                    {
                        kObj = int.Parse(strName);
                    }
                    else if (typeof(float) == kInfo.FieldType)
                    {
                        kObj = float.Parse(strName);
                    }
                    else if (typeof(bool) == kInfo.FieldType)
                    {
                        kObj = bool.Parse(strName);
                    }
                    else if (typeof(string) == kInfo.FieldType)
                    {
                        kObj = strName;
                    }

                    kInfo.SetValue(this,kObj);
                }
            }

            for(int iIdx = 0;iIdx < kInData[BTConstant.Child].Count;iIdx++)
            {
                string strTypeName = kInData[BTConstant.Child][iIdx][BTConstant.Type].ToString();
                BTNode kChildNode = ClassFactory.Instance.CreateClass("BehaviourTree." + strTypeName) as BTNode;
                if(null != kChildNode)
                {
                    kChildNode.Load(kInData[BTConstant.Child][iIdx]);
                    AddChild(kChildNode);
                }
            }
        }
        
        public virtual void Save(JsonData kInData)
        {
            kInData[BTConstant.Type] = NodeType;
            kInData[BTConstant.Name] = Name;
            kInData[BTConstant.DisplayName] = DisplayName;
            kInData[BTConstant.Argument] = new JsonData();
            kInData[BTConstant.Argument].SetJsonType(JsonType.Object);
            FieldInfo[] kInfoList = GetType().GetFields();
            for(int iIdx = 0;iIdx < kInfoList.Length;iIdx++)
            {
                FieldInfo kInfo = kInfoList[iIdx];
                kInData[BTConstant.Argument][kInfo.Name] = kInfo.GetValue(this).ToString();
            }

            kInData[BTConstant.Child] = new JsonData();
            kInData[BTConstant.Child].SetJsonType(JsonType.Array);
            for(int iIdx = 0;iIdx < m_kChildList.Count;iIdx++)
            {
                JsonData kData = new JsonData();
                m_kChildList[iIdx].Save(kData);
                kInData[BTConstant.Child].Add(kData);
            }
        }

        public string NodeType
        {
            get { return m_strNodeType; }
            set { m_strNodeType = value; }
        }
        public string Name
        {
            get { return m_strName; }
            set { m_strName = value; }
        }
        public string DisplayName
        {
            get { return m_strDisplayName; }
            set { m_strDisplayName = value; }
        }
        public string Details
        {
            get { return m_strDetail; }
            set { m_strDetail = value; }
        }
        public bool IsRunnint
        {
            get { return m_bIsRunning; }
            set { m_bIsRunning = value; }
        }

        public BTNode Parent
        {
            get { return m_kParent; }
            set { m_kParent = value; }
        }
        public List<BTNode> Children
        {
            get { return m_kChildList; }
            
        }

        public bool IsClientNode
        {
            get { return m_bClientNode; }
            
        }

        protected BTDatabase m_kDatabase;
        protected string m_strName = "Node";
        protected string m_strDisplayName = "Node";
        protected string m_strNodeType = "BTNode";
        protected string m_strDetail = "";
        protected bool m_bIsRunning = false;
        protected BTNode m_kParent = null;
        protected bool m_bClientNode = false;
        protected List<BTNode> m_kChildList = new List<BTNode>();
    }

}