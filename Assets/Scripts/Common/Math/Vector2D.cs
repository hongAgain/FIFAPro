using System;

public struct Vector2D
{
   
    public Vector2D(double fx,double fy)
    {
        m_fX = fx;
        m_fY = fy;
    }
    public Vector2D Normalize()
    {
        double dLen = (m_fX * m_fX + m_fY * m_fY);
        dLen = Math.Sqrt(dLen);
        if (dLen <= 0.001F)
        {
            return Vector2D.zero;
        }
        return new Vector2D(m_fX / dLen, m_fY / dLen);
    }
    public static Vector2D operator *(Vector2D kArgA, double kArgB)
    {
        return new Vector2D(kArgA.X * kArgB, kArgA.Y * kArgB);
    }

    public static Vector2D operator *(double kArgB, Vector2D kArgA)
    {
        return new Vector2D(kArgA.X * kArgB, kArgA.Y * kArgB);
    }


    public static Vector2D zero { get{return m_kZero;} }
    public double X
    {
        get { return m_fX; }
        set { m_fX = value; }
    }

    public double Y
    {
        get { return m_fY; }
        set { m_fY = value; }
    }
    private double m_fX;
    private double m_fY;
    
    private static Vector2D m_kZero = new Vector2D(0,0); 
}