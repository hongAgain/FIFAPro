using System;
using System.Collections.Generic;
using Common.Log;
namespace BehaviourTree
{
    public class BTDatabase 
    {
        public BTDatabase()
        {
        }
		
		public T GetData<T> (String strName) 
        {
			int iDataID = IndexOfDataId(strName);
            if (iDataID == -1)
            {
                LogManager.Instance.Log("BTDatabase: Data for " + strName + " does not exist!");
            }
			return (T) m_kDataList[iDataID];
		}
		
		public T GetData<T> (int uiDataID) 
        {
            //if (BTConfiguration.ENABLE_DATABASE_LOG) {
            //    Debug.Log("BTDatabase: getting kData for " + _dataNames[uiDataID]);
            //}
			return (T) m_kDataList[uiDataID];
		}
		
		public void SetData<T> (String strName, T kData) 
        {
			int uiDataID = GetDataID(strName);
			m_kDataList[uiDataID] = (object) kData;
		}
		
		public void SetData<T> (int uiDataID, T kData)
        {
			m_kDataList[uiDataID] = (object) kData;
		}

		public bool CheckDataNull (String strName) {
			int uiDataID = IndexOfDataId(strName);
            if (uiDataID == -1) 
                return true;

			return CheckDataNull(uiDataID);
		}

		public bool CheckDataNull (int uiDataID) 
        {
			// Despite == test, Equal test helps the case that the reference is Monobahvior and is destroyed.
			return m_kDataList[uiDataID] == null || m_kDataList[uiDataID].Equals(null);
		}
		
		public int GetDataID (String strName) 
        {
			int uiDataID = IndexOfDataId(strName);
            if (uiDataID == -1)
            {
				m_kDataNames.Add(strName);
				m_kDataList.Add(null);
				uiDataID = m_kDataNames.Count - 1;
			}
			
			return uiDataID;
		}
		
		private int IndexOfDataId (String strName) 
        {
			for (int i=0; i<m_kDataNames.Count; i++) 
            {
				if (m_kDataNames[i].Equals(strName))
                {
                    return i;
                }
			}
			return -1;
		}
		
		public bool ContainsData (String strName)
        {
            return IndexOfDataId(strName) != -1;
		}

        // m_kDataList & m_kDataNames 一对一的关系
        private List<object> m_kDataList = new List<object>();
        private List<String> m_kDataNames = new List<String>();

	}

}