using UnityEngine;
using System.Collections;

public class UISliderPower : MonoBehaviour 
{
    public enum ClipDirection
    {
        Left2Right,
        Right2Left,
    }

    public UIPanel m_clipPanel;
    public UISprite m_clipSprite;
    public Transform m_thumb;
    public UILabel m_lblLeft;
    public UILabel m_lblRight;
    public ClipDirection m_clipDirection = ClipDirection.Left2Right;

    private float m_coeffValue;
    private float m_lenSprite;
    private float m_thumbPosX;
    private float m_panelOffset;

    public float CoeffValue
    {
        get { return m_coeffValue; }
        set 
        { 
            m_coeffValue = value;
            if (m_clipDirection == ClipDirection.Left2Right)
            {
                m_thumbPosX = m_lenSprite * m_coeffValue - m_lenSprite / 2;
                m_panelOffset = m_lenSprite * m_coeffValue;
                if (m_lblLeft != null)
                    m_lblLeft.text = string.Format("{0}%", Mathf.RoundToInt(m_coeffValue * 100));
                if (m_lblRight != null)
                    m_lblRight.text = string.Format("{0}%", Mathf.RoundToInt((1 - m_coeffValue) * 100));

            } 
            else if (m_clipDirection == ClipDirection.Right2Left)
            {
                m_thumbPosX = m_lenSprite / 2 - m_lenSprite * m_coeffValue;
                m_panelOffset = -m_lenSprite * m_coeffValue;
                if (m_lblLeft != null)
                    m_lblLeft.text = string.Format("{0}%", Mathf.RoundToInt((1 - m_coeffValue) * 100));
                if (m_lblRight != null)
                    m_lblRight.text = string.Format("{0}%", Mathf.RoundToInt(m_coeffValue * 100));
            }

            UpdateSliderPower();
        }
    }
	// Use this for initialization
	void Start () 
    {
        m_lenSprite = m_clipSprite.width;
        CoeffValue = m_coeffValue;
	}

    void UpdateSliderPower()
    {
        m_clipPanel.clipOffset = new Vector2(m_panelOffset,0);

        UpdateThumbPos();
    }

    void UpdateThumbPos()
    {
        m_thumb.localPosition = new Vector3(m_thumbPosX, 0, 0);
    }

}
