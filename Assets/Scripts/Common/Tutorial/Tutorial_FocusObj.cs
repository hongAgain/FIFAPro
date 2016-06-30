using UnityEngine;
using System.Collections.Generic;
using Common.Log;
using LuaInterface;

public class Tutorial_FocusObj : MonoBehaviour
{
    private class Pair
    {
        public UIWidget mWidget;
        public UIPanel mPanel;
    }

    private List<Pair> mCachedWidgets = new List<Pair>();

    private GameObject mRoot;
    private Transform mOrgParent;

    private ClickExtra mClickExtra;
    private DragDropExtra mDragDropExtra;

    public void Init(UIPanel newPanel, bool changeParent)
    {
        mRoot = gameObject;
        mOrgParent = mRoot.transform.parent;
        if (changeParent)
        {
            mRoot.BroadcastMessage("Start", SendMessageOptions.DontRequireReceiver);
            mRoot.transform.parent = newPanel.transform;
            foreach (var widget in mRoot.GetComponentsInChildren<UIWidget>())
            {
                var pair = new Pair();
                pair.mWidget = widget;
                pair.mPanel = widget.panel;
                mCachedWidgets.Add(pair);

                if (widget.panel != null)
                {
                    widget.panel.RemoveWidget(widget);
                }
                else
                {
                    LogView.Warn(string.Format("{0} doesn't have UIPanel yet!", widget.name));
                }
                newPanel.AddWidget(widget);
            }
            mRoot.BroadcastMessage("ParentHasChanged", SendMessageOptions.DontRequireReceiver);
        }
    }

    public void WatchClick(LuaFunction onClick, bool cover)
    {
        if (mClickExtra != null)
        {
            mClickExtra.Release();
        }
        mClickExtra = new ClickExtra(mRoot, onClick, cover);
    }

    public void WatchDragDrop(LuaFunction onDragStart, LuaFunction onDragEnd, LuaFunction onDrop)
    {
        if (mDragDropExtra != null)
        {
            mDragDropExtra.Release();
        }
        mDragDropExtra = new DragDropExtra(mRoot, onDragStart, onDragEnd, onDrop);
    }

    public void Release()
    {
        for (int i = 0; i < mCachedWidgets.Count; ++i)
        {
            var cacheTarget = mCachedWidgets[i];
            var widget = cacheTarget.mWidget;
            if (widget != null)
            {
                if (widget.panel != null)
                {
                    widget.panel.RemoveWidget(widget);
                }

                if (cacheTarget.mPanel != null)
                {
                    cacheTarget.mPanel.AddWidget(widget);
                }
            }
        }

        mRoot.transform.parent = mOrgParent;
        mRoot.BroadcastMessage("ParentHasChanged", SendMessageOptions.DontRequireReceiver);

        mRoot = null;
        mOrgParent = null;
        mCachedWidgets.Clear();
        mCachedWidgets = null;

        if (mClickExtra != null)
        {
            mClickExtra.Release();
            mClickExtra = null;
        }

        if (mDragDropExtra != null)
        {
            mDragDropExtra.Release();
            mDragDropExtra = null;
        }

        Destroy(this);
    }

    void OnClick()
    {
        if (mClickExtra != null)
        {
            mClickExtra.OnClick();
        }
    }

    void OnDragStart()
    {
        if (mDragDropExtra != null)
        {
            mDragDropExtra.OnDragStart();
        }
    }

    void OnDragEnd()
    {
        if (mDragDropExtra != null)
        {
            mDragDropExtra.OnDragEnd();
        }
    }

    void OnDrop(GameObject go)
    {
        if (mDragDropExtra != null)
        {
            mDragDropExtra.OnDrop(go);
        }
    }
}

class ClickExtra
{
    private UIEventListener mListener;
    private LuaFunction mOnClick;
    private UIEventListener.VoidDelegate mOrgOnClick;
    private bool mCover = false;

    public ClickExtra(GameObject go, LuaFunction onClick, bool cover)
    {
        mListener = go.GetComponent<UIEventListener>();
        mOnClick = onClick;
        mCover = cover;

        if (mListener == null)
        {
            LogManager.Instance.YellowLog("Cover Click but haven't UIEventListener name = " + go.name);
            mListener = go.AddComponent<UIEventListener>();
        }

        mOrgOnClick = mListener.onClick;
        mListener.onClick = null;
    }

    public void Release()
    {
        mListener.onClick = mOrgOnClick;

        mListener = null;
        mOnClick = null;
        mOrgOnClick = null;
        mCover = false;
    }

    public void OnClick()
    {
        if (mListener != null)
        {
            if (mCover == false && mOrgOnClick != null)
            {
                mOrgOnClick(mListener.gameObject);
            }

            mOnClick.Call();
        }
    }
}

class DragDropExtra
{
    private UIEventListener mListener;
    private LuaFunction mOnDragStart;
    private LuaFunction mOnDragEnd;
    private LuaFunction mOnDrop;

    private UIEventListener.VoidDelegate mOrgOnDragStart;
    private UIEventListener.VoidDelegate mOrgOnDragEnd;
    private UIEventListener.ObjectDelegate mOrgOnDrop;

    public DragDropExtra(GameObject go, LuaFunction onDragStart, LuaFunction onDragEnd, LuaFunction onDrop)
    {
        mListener = go.GetComponent<UIEventListener>();
        mOnDragStart = onDragStart;
        mOnDragEnd = onDragEnd;
        mOnDrop = onDrop;
        if (mListener != null)
        {
            mOrgOnDragStart = mListener.onDragStart;
            mOrgOnDragEnd = mListener.onDragEnd;
            mOrgOnDrop = mListener.onDrop;

            mListener.onDragStart = null;
            mListener.onDragEnd = null;
            mListener.onDrop = null;
        }
        else
        {
            LogManager.Instance.RedLog("Cover DragDrop but haven't UIEventListener name = " + go.name);
        }
    }

    public void Release()
    {
        mListener.onDragStart = mOrgOnDragStart;
        mListener.onDragEnd = mOrgOnDragEnd;
        mListener.onDrop = mOrgOnDrop;

        //mListener = null;
        mOnDragStart = null;
        mOnDragEnd = null;
        mOnDrop = null;

        mOrgOnDragStart = null;
        mOrgOnDragEnd = null;
        mOrgOnDrop = null;
    }

    public void OnDragStart()
    {
        if (mListener != null)
        {
            mOnDragStart.Call(mListener.gameObject);
        }
    }

    public void OnDragEnd()
    {
        if (mListener != null)
        {
            mOnDragEnd.Call(mListener.gameObject);
        }
    }

    public void OnDrop(GameObject go)
    {
        if (mListener != null)
        {
            mOnDrop.Call();
            mListener.onDrop(mListener.gameObject, go);
        }
    }
}