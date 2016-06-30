using System;

public class Region
{
    public Region()
    {
        m_fLeft = 0;
        m_fTop = 0;
        m_fRight = 0;
        m_fBottom = 0;
        m_uiID = UInt32.MaxValue;
    }
    public Region(double fLeft,double fTop,double fRight,double fBottom)
    {
        m_fLeft = fLeft;
        m_fTop = fTop;
        m_fRight = fRight;
        m_fBottom = fBottom;
        m_uiID = UInt32.MaxValue;
    }
    public Region(double fLeft, double fTop, double fRight, double fBottom,UInt32 uiID)
    {
        m_fLeft = fLeft;
        m_fTop = fTop;
        m_fRight = fRight;
        m_fBottom = fBottom;
        m_uiID = uiID;
    }

    public bool CheckInRegion(Vector3D kPos)
    {
        if (kPos.Z < m_fRight || kPos.Z > m_fLeft ||
            kPos.X < m_fTop || kPos.X > m_fBottom)
            return false;
        return true;
    }

    public bool InCheckRegion(Vector3D _pos)
    {
        if ((_pos.X > m_fLeft && _pos.X < m_fRight) && (_pos.Z > m_fTop && m_fBottom > _pos.Z))
            return true;

        return false;

    }
    public double Left
    {
        get { return m_fLeft; }
        set { m_fLeft = value; }
    }
    public double Top
    {
        get { return m_fTop; }
        set { m_fTop = value; }
    }
    public double Right
    {
        get { return m_fRight; }
        set { m_fRight = value; }
    }
    public double Bottom
    {
        get { return m_fBottom; }
        set { m_fBottom = value; }
    }

    public UInt32 ID
    {
        get { return m_uiID; }
        set { m_uiID = value; }
    }
    private double m_fLeft;     // 代表z轴
    private double m_fTop;      // 代表x轴
    private double m_fRight;
    private double m_fBottom;
    private UInt32 m_uiID;
}