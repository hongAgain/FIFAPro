using System;
using System.Collections.Generic;

public class LRU<T> where T : ILruItem<T>
{
    private List<T> mItems = new List<T>();

    private int mCapbility;

    public int Capbility
    {
        get { return mCapbility; }
        set { mCapbility = value; }
    }

    public LRU(int size)
    {
        mCapbility = size;
    }

    public void AddItem(T a)
    {
        if (mItems.Count >= mCapbility && mCapbility > 0)
        {
            mItems.Sort(Cmp);
            while (mItems.Count >= mCapbility)
            {
                const int idx = 0;
                mItems[idx].OnRmv();
                mItems.RemoveAt(idx);
            }
        }
        if (mCapbility > 0)
        {
            a.OnRecycle();
            mItems.Add(a);
        }
        else
        {
            a.OnRmv();
        }
    }

    public void RmvItem(T a)
    {
        mItems.Remove(a);
    }

    public void Release()
    {
        foreach (var a in mItems)
        {
            a.OnRmv();
        }
        mItems.Clear();
    }

    public bool Contians(T ui)
    {
        return mItems.Contains(ui);
    }

    private static int Cmp(T a, T b)
    {
        return a.CompareTo(b);
    }
}

public interface ILruItem<T> : IComparable<T>
{
    void OnRecycle();
    void OnRmv();
}
