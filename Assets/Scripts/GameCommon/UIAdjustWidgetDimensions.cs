using UnityEngine;
using System.Collections;

public class UIAdjustWidgetDimensions : MonoBehaviour 
{
    private UIWidget m_widget;

    public EPivot m_pivot = EPivot.None;
    public enum EPivot
    {
        None,
        Top,
        Left,
        Right,
        Bottom,
    }
	// Use this for initialization
	void Start () 
    {
        m_widget = GetComponent<UIWidget>();
        if(m_widget != null)
        {
            SetPivotDimensions();
        }
	}
	
    //// Update is called once per frame
#if UNITY_EDITOR
    void Update()
    {
        SetPivotDimensions();
    }
#endif
    void SetPivotDimensions()
    {
        switch (m_pivot)
        {
            case EPivot.None:
                {
                    Util.Log("EPivot.None!!!");
                    if (m_widget.pivot != UIWidget.Pivot.Center)
                    {
                        m_widget.pivot = UIWidget.Pivot.Center;
                        m_widget.MakePixelPerfect();
                    }
                }
                break;
            case EPivot.Left:
                {
                    if(!IsLeftPivot(m_widget.pivot))
                        m_widget.pivot = UIWidget.Pivot.Left;

                    m_widget.SetDimensions(Screen.width, m_widget.height);
                }
                break;
            case EPivot.Right:
                {
                    if (!IsRightPivot(m_widget.pivot))
                        m_widget.pivot = UIWidget.Pivot.Right;

                    m_widget.SetDimensions(Screen.width, m_widget.height);
                }
                break;
            case EPivot.Top:
                {
                    if (!IsTopPivot(m_widget.pivot))
                        m_widget.pivot = UIWidget.Pivot.Top;

                    m_widget.SetDimensions(m_widget.width, Screen.height);
                }
                break;
            case EPivot.Bottom:
                {
                    if (!IsBottomPivot(m_widget.pivot))
                        m_widget.pivot = UIWidget.Pivot.Bottom;

                    m_widget.SetDimensions(m_widget.width, Screen.height);
                }
                break;
        }
    }

    bool IsLeftPivot(UIWidget.Pivot pivot_)
    {
        if (UIWidget.Pivot.Left == pivot_)
            return true;

        return false;
    }

    bool IsRightPivot(UIWidget.Pivot pivot_)
    {
        if (UIWidget.Pivot.Right == pivot_)
            return true;

        return false;
    }

    bool IsTopPivot(UIWidget.Pivot pivot_)
    {
        if (UIWidget.Pivot.Top == pivot_)
            return true;

        return false;
    }

    bool IsBottomPivot(UIWidget.Pivot pivot_)
    {
        if (UIWidget.Pivot.Bottom == pivot_)
            return true;

        return false;
    }
}
