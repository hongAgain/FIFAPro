using System;

public struct Sector
{
    private double mRadius;

    private double mRadian;
    
    private Vector3D mDir;

    private double mCacheHalfcos;
    private Vector3D mCachePointOnDir;

    public Sector(double radius, float angle, Vector3D dir)
    {
        mRadius = radius;
        mRadian = angle * Math.PI / 180;
        mDir = dir.Normalize();

        mCacheHalfcos = Math.Cos(mRadian / 2);
        mCachePointOnDir = mDir.Normalize() * mRadius;
    }

    public double Radius { get { return mRadius; } }

    public double Radian { get { return mRadian; } }

    public Vector3D Dir { get { return mDir; } }

    public Vector3D PointOnDir { get { return mCachePointOnDir; } }

    public bool InArea(Vector3D point, Vector3D center)
    {
        double d = point.Distance(center);
        if (d > mRadius)
        {
            return false;
        }

        Vector3D dir = point - center;
        double dot = Vector3D.Dot(dir, mDir);
        if (dot <= mCacheHalfcos)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}