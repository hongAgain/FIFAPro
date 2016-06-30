using System;

public struct Vector3D 
{
    public Vector3D(double fx, double fy, double fz)
    {
        m_fX = fx;
        m_fY = fy;
        m_fZ = fz;
    }

    public Vector3D(Vector3D kVec)
    {
        m_fX = kVec.X;
        m_fY = kVec.Y;
        m_fZ = kVec.Z;
    }

    public double Distance(Vector3D kPos)
    {
        double fX = kPos.X - m_fX;
        double fY = kPos.Y - m_fY;
        double fZ = kPos.Z - m_fZ;
        return Math.Sqrt(fX * fX + fY * fY + fZ * fZ);
    }

    public Vector3D Normalize()
    {
        double dLen = (m_fX * m_fX + m_fY * m_fY + m_fZ * m_fZ);
        dLen = Math.Sqrt(dLen);
        if (dLen <= 0.001F)
        {
            return Vector3D.zero;
        }
        return new Vector3D(m_fX / dLen, m_fY / dLen, m_fZ / dLen);
    }

    public double Length()
    {
        return Math.Sqrt(m_fX * m_fX + m_fY * m_fY + m_fZ * m_fZ);
    }
    

    public static Vector3D operator -(Vector3D kArgA)
    {
        return new Vector3D(-kArgA.X, -kArgA.Y, -kArgA.Z);
    }

    public static Vector3D operator +(Vector3D kArgA, Vector3D kArgB)
    {
        return new Vector3D(kArgA.X + kArgB.X, kArgA.Y + kArgB.Y, kArgA.Z + kArgB.Z);
    }

    public static Vector3D operator -(Vector3D kArgA, Vector3D kArgB)
    {
        return new Vector3D(kArgA.X - kArgB.X, kArgA.Y - kArgB.Y, kArgA.Z - kArgB.Z);
    }

    public static Vector3D operator *(Vector3D kArgA, double kArgB)
    {
        return new Vector3D(kArgA.X * kArgB, kArgA.Y * kArgB, kArgA.Z * kArgB);
    }

    public static Vector3D operator *(double kArgB, Vector3D kArgA)
    {
        return new Vector3D(kArgA.X * kArgB, kArgA.Y * kArgB, kArgA.Z * kArgB);
    }

    public static Vector3D operator /(Vector3D kArgA, double kArgB)
    {
        return new Vector3D(kArgA.X / kArgB, kArgA.Y / kArgB, kArgA.Z / kArgB);
    }

    public static Vector3D zero { get { return m_kZero; } }
    public static Vector3D forward { get { return m_kForward; } }
    public static Vector3D right { get { return m_kRight; } }
    public static Vector3D up { get { return m_kUp; } }

    public static double Dot(Vector3D a, Vector3D b)
    {
        return a.m_fX * b.m_fX + a.m_fY * b.m_fY + a.m_fZ * b.m_fZ;
    }

    public static Vector3D Cross(Vector3D a, Vector3D b)
    {
        //(1,2,3)×(4,5,6)=(12-15,12-6,5-8)=（-3,6,-3）
        return new Vector3D(a.m_fY * b.m_fZ - b.m_fY * a.m_fZ, a.m_fZ * b.m_fX - b.m_fZ * a.m_fX, a.m_fX * b.m_fY - b.m_fX * a.m_fY);
    }

    public static Vector3D Reflect(Vector3D I, Vector3D N)
    {
        I.Normalize();
        N.Normalize();
        Vector3D R = I - N * 2 * Vector3D.Dot(I, N);
        return R;
    }

    //public static double Angle(Vector3D _dir,Vector3D _dirtmp)
    //{
    //    double _angle = 0d;
    //    double _f = Dot(_dir, _dirtmp);
    //    _angle = Math.Acos(_f) * ParabolaMath.Deg2Rad;
    //    return _angle;
    //}
    
    public override string ToString()
    {
        return string.Format("x = {0} y = {1} z = {2}", m_fX, m_fY, m_fZ);
    }

    public static bool operator ==(Vector3D kArgA, Vector3D kArgB)
    {
        if (Math.Abs(kArgA.X - kArgB.X) > float.Epsilon)
            return false;

        if (Math.Abs(kArgA.Y - kArgB.Y) > float.Epsilon)
            return false;

        if (Math.Abs(kArgA.Z - kArgB.Z) > float.Epsilon)
            return false;
        return true;
    }

    public static bool operator !=(Vector3D kArgA, Vector3D kArgB)
    {
        return !(kArgA == kArgB);
    }


    public override bool Equals(object kObj)
    {
        if(null == kObj)
            return false;
        if(kObj.GetType() != this.GetType())
            return false;
            
        Vector3D kVec = (Vector3D)kObj ;
        if(false == Object.Equals(m_fX,kVec.X))
            return false;
        if(false == Object.Equals(m_fY,kVec.Y))
            return false;
        if(false == Object.Equals(m_fZ,kVec.Z))
            return false;
        return true;
    }
    public Vector3D normalized
    {
        get { return Normalize(); }
    }
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

    public double Z
    {
        get { return m_fZ; }
        set { m_fZ = value; }
    }
    private double m_fX;
    private double m_fY;
    private double m_fZ;
    private static Vector3D m_kZero = new Vector3D(0,0,0);
    private static Vector3D m_kForward = new Vector3D(0d, 0d, 1d);
    private static Vector3D m_kRight = new Vector3D(1d, 0d, 0d);
    private static Vector3D m_kUp = new Vector3D(0d, 1d, 0d);
}