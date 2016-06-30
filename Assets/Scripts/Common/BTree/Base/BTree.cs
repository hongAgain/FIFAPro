using Common;
using LitJson;
using System.IO;

namespace BehaviourTree
{
    public class BTree
    {
        public BTree()
        {
            m_kDatabase = new BTDatabase();
            m_kRoot = new BTSelector();
        }

        public void EditorLoad(string strFileName)
        {
            string strContent = File.ReadAllText(strFileName);
            m_kRoot = new BTSelector();
            m_kRoot.DisplayName = "Root";
            JsonData kData = JsonMapper.ToObject(strContent);
            m_kRoot.Load(kData);
        }

        public void Load(string strFileName)
        {
#if FIFA_CLIENT
            string strContent = ResourceManager.Instance.LoadText(strFileName);
#else
            string strContent = File.ReadAllText(strFileName); 
#endif
            m_kRoot = new BTSelector();
            m_kRoot.DisplayName = "Root";
            JsonData kData = JsonMapper.ToObject(strContent);
            m_kRoot.Load(kData);
            m_kRoot.Activate(m_kDatabase);
        }

        public void Save(string strFileName)
        {
            JsonData kJson = new JsonData();
            m_kRoot.Save(kJson);
            File.WriteAllText(strFileName, kJson.ToJson());
        }

        public void Update(double fTime)
        {
            if (null == m_kRoot)
                return;
            m_kRoot.Tick(fTime);
        }

        public void Reset()
        {
            if (null != m_kRoot)
                m_kRoot.Clear();
        }
        public BTDatabase Database
        {
            get { return m_kDatabase; }
        }

        public BTSelector Root      // 选择节点，根据节点的情况来决定运行哪个子节点
        {
            get { return m_kRoot; }
        }
        protected BTSelector m_kRoot = null;
        protected BTDatabase m_kDatabase = null;
    }
}
