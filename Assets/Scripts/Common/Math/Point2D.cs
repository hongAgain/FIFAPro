public struct Point2D
{
    public Point2D(int iX, int iY)
    {
        m_iX = iX;
        m_iY = iY;
    }

    public int X
    {
        get { return m_iX; }
        set { m_iX = value; }
    }

    public int Y
    {
        get { return m_iY; }
        set { m_iY = value; }
    }

    private int m_iX;
    private int m_iY;
}