using System.Collections.Generic;

namespace Common.Tables
{
    public class CoachItem
	{
		public CoachItem()
		{
			AbilityList = new List<float>();
			SkillList = new List<int>();
		}
		public int ID;
		public string Name;

	#if FIFA_CLIENT
		public int HeadID;
		public int BodyID;
		public int ShirtID;
		public UnityEngine.Color SkinColor;
	#endif

		public string Rating;
		public int SoulID;
		public int ComposeNum;
		public int RecycleNum;

		public int AdvModID;
		public List<float> AbilityList;
		public List<int> SkillList;

		public string Reputation;
	}

	public class CoachTable 
	{
		public CoachTable()
		{
			
		}
	
		public bool InitTable()
		{
			JsonTable kTable = DataManager.Instance.ReadJsonTable("Tables/Battle/coach") as JsonTable;
			if (null == kTable )
				return false;
			
			foreach(var kItem in kTable.ItemList)
			{
				CoachItem kCoachItem = new CoachItem();
				kCoachItem.ID = int.Parse(kItem.Key);
				kCoachItem.Name = kItem.Value["name"];
			#if FIFA_CLIENT
				kCoachItem.HeadID = int.Parse(kItem.Value["head"]);
				kCoachItem.BodyID = int.Parse(kItem.Value["body"]);
				kCoachItem.ShirtID = int.Parse(kItem.Value["shirt"]);
				TableUtil.GetColor(kItem.Value["skinColor"], '.',ref kCoachItem.SkinColor);
			#endif
				kCoachItem.Rating = kItem.Value["rating"];
				kCoachItem.SoulID = int.Parse(kItem.Value["soul"]);
				kCoachItem.ComposeNum = int.Parse(kItem.Value["compose"]);
				kCoachItem.RecycleNum = int.Parse(kItem.Value["recycle"]);
				kCoachItem.AdvModID = int.Parse(kItem.Value["advModel"]);
								

				string strVal = kItem.Value["teach"];
				string[] strList = strVal.Split(' ');

				for (int i = 0; i < strList.Length; i++)
				{
					if ("null" == strList[i])
					{
						kCoachItem.AbilityList.Add(-1);
					}
					else
						kCoachItem.AbilityList.Add(float.Parse(strList[i]));
				}
				
				strVal = kItem.Value["skill"];
				strList = strVal.Split(' ');
				for (int i = 0; i < strList.Length; i++)
				{
					if ("null" == strList[i])
					{
						kCoachItem.SkillList.Add(-1);
					}
					else
						kCoachItem.SkillList.Add(int.Parse(strList[i]));
				}
				m_kItemList.Add(kCoachItem.ID, kCoachItem);
			}
			return true;
		}
		
		public CoachItem GetItem(int iID)
		{
			CoachItem kItem;
			m_kItemList.TryGetValue(iID, out kItem);
			return kItem;
		}
		
		protected Dictionary<int, CoachItem> m_kItemList = new Dictionary<int, CoachItem>();
	}
}
