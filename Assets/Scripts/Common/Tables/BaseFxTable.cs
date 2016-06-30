using System.Collections.Generic;

namespace Common.Tables
{

    public enum EFxType
    {
        Fx = 0,
        Right_UI,
        Bottom_UI,
        Invalid
    }
    public class BaseFxItem
    {
        public int ID;
        public string Name;
        public double DelayTime;
        public double LiveTime;
        public bool Loop;
        public EFxType FxType = EFxType.Invalid;
        public double HorizontalOffset = 0;
        public double HorizontalOffsetTime = 0;
        public double VerticalOffset = 0;
        public double VerticalOffsetTime = 0;
        public Vector3D Pos;
    }

    public class BaseFxTable
    {
        public BaseFxTable()
        {

        }

        public bool InitTable()
        {
            JsonTable kTable = DataManager.Instance.ReadJsonTable("Tables/Battle/BaseFx") as JsonTable;
            if (null == kTable)
                return false;

            foreach (var kItem in kTable.ItemList)
            {
                BaseFxItem kEffectItem = new BaseFxItem();
                kEffectItem.ID = int.Parse(kItem.Key);
                kEffectItem.Name = kItem.Value["fx_name"];
                string strVal = null;
                kItem.Value.TryGetValue("pos", out strVal);
                if (string.IsNullOrEmpty(strVal))
                    kEffectItem.Pos = Vector3D.zero;
                else
                    kEffectItem.Pos = TableUtil.GetVector3(strVal, ',');
                kItem.Value.TryGetValue("fx_delay_time", out strVal);
                if (string.IsNullOrEmpty(strVal))
                    kEffectItem.DelayTime = 0;
                else
                    kEffectItem.DelayTime = double.Parse(strVal);
                kItem.Value.TryGetValue("loop", out strVal);
                if (string.IsNullOrEmpty(strVal))
                    kEffectItem.Loop = false;
                else
                    kEffectItem.Loop = TableUtil.GetBoolean(strVal);
                    
                kItem.Value.TryGetValue("type", out strVal);
                if (string.IsNullOrEmpty(strVal))
                    kEffectItem.FxType = EFxType.Invalid;
                else
                    kEffectItem.FxType = (EFxType)(int.Parse(strVal));

                kItem.Value.TryGetValue("live_time", out strVal);
                if (string.IsNullOrEmpty(strVal))
                    kEffectItem.LiveTime = 0;
                else
                    kEffectItem.LiveTime = double.Parse(strVal);

                kItem.Value.TryGetValue("horizontal_offset", out strVal);
                if (string.IsNullOrEmpty(strVal))
                    kEffectItem.HorizontalOffset = 0;
                else
                    kEffectItem.HorizontalOffset = double.Parse(strVal);

                kItem.Value.TryGetValue("horizontal_offset_time", out strVal);
                if (string.IsNullOrEmpty(strVal))
                    kEffectItem.HorizontalOffsetTime = 0;
                else
                    kEffectItem.HorizontalOffsetTime = double.Parse(strVal);

                kItem.Value.TryGetValue("vertical_offset", out strVal);
                if (string.IsNullOrEmpty(strVal))
                    kEffectItem.VerticalOffset = 0;
                else
                    kEffectItem.VerticalOffset = double.Parse(strVal);

                kItem.Value.TryGetValue("vertical_offset_time", out strVal);
                if (string.IsNullOrEmpty(strVal))
                    kEffectItem.VerticalOffsetTime = 0;
                else
                    kEffectItem.VerticalOffsetTime = double.Parse(strVal);
                m_kItemList.Add(kEffectItem.ID, kEffectItem);
            }

            return true;
        }
        public BaseFxItem GetItem(int iID)
        {
            BaseFxItem kItem;
            m_kItemList.TryGetValue(iID, out kItem);
            return kItem;
        }


        protected Dictionary<int, BaseFxItem> m_kItemList = new Dictionary<int, BaseFxItem>();
    }
}