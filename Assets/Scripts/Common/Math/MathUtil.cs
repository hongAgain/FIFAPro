using System;

public class MathUtil
{
#if FIFA_CLIENT
    public static UnityEngine.Vector3 ConverToVector3(Vector3D kVec)
    {
        return new UnityEngine.Vector3((float)kVec.X, (float)kVec.Y, (float)kVec.Z);
    }
    public static Vector3D ConverToVector3D(UnityEngine.Vector3 kVec)
    {
        return new Vector3D(kVec.x, kVec.y, kVec.z);
    }
#endif
    public static Vector3D GetDir(Vector3D kOri, Vector3D kTarget)
    {
        Vector3D kDir = kTarget - kOri;
        kDir = kDir.Normalize();
        return kDir;
    }

    /// <summary>
    /// Gets the horizontal angle(0 - 360). 
    /// angle 0 means facing Z-positiveInfinity direction.
    /// angle 90 means facing X-positiveInfinity direction.
    /// angle 180 means facing Z-negativeInfinity direction.
    /// angle 270 means facing X-negativeInfinity direction.
    /// </summary>
    /// <returns>The angle.</returns>
    /// <param name="kOri">K ori.</param>
    /// <param name="kTarget">K target.</param>
    public static double GetAngle(Vector3D kOri, Vector3D kTarget)
    {
        Vector3D kDir = MathUtil.GetDir(kOri, kTarget);
        double dAngle = 0;
        if (kDir.Z == 0)
        {
            if (kDir.X > 0)
                dAngle = 90;
            else
                dAngle = 270;
        }
        else if (kDir.X == 0)
        {
            if(kDir.Z > 0)
                dAngle = 0d;
            else
                dAngle = 180d;
        }
        else
        {
            //not an on-axis value
            double tanValue = kDir.X / kDir.Z;
            // dAngle : (-90,0)U(0,90)
            dAngle = Math.Atan( tanValue ) * 180d / Math.PI;
            double dAngleOffset = 0d;
            if(kDir.Z < 0)
            {
                dAngleOffset = 180d;
            }
            dAngle = (dAngle+dAngleOffset+360d)%360;
        }
        return dAngle;

//        // 球员的朝向是向着目标点的,单位向量
//        Vector3D kDir = MathUtil.GetDir(kOri, kTarget);
//        double dAngle = 0;
//        if (kDir.Z == 0)
//        {
//            if (kDir.X > 0)
//                dAngle = 90;
//            else
//                dAngle = 270;
//        }
//        else
//        {
//            dAngle = Math.Atan(kDir.X / kDir.Z) * 180 / Math.PI;
//        }
//
//        if (kOri.Z > kTarget.Z)
//        {
//            dAngle = 180.0f + dAngle;
//        }
//        if(dAngle<0d)
//        {
//            dAngle += 360d;
//        }
//        return dAngle;
    }

    public static double GetMinAngle(double angleA,double angleB)
    {
        double correctedAngleA = (angleA+360d)%360;
        double correctedAngleB = (angleB+360d)%360;
        double delta = Math.Abs(correctedAngleA - correctedAngleB);
        return Math.Min(delta, 360d-delta);
    }

    public static Vector3D GetDirFormAngle(double dAngle)
    {
        Vector3D kVec = new Vector3D();
        kVec.X = Math.Sin(dAngle * Math.PI / 180);
        kVec.Z = Math.Cos(dAngle * Math.PI / 180);
        kVec.Y = 0;
        kVec = kVec.Normalize();
        return kVec;
    }

    /// <summary>
    ///坐标系转换
    /// </summary>
    /// <param name="_v"></param>
    /// <param name="_angle"></param>
    /// <returns></returns>
    public static Vector3D GetNewVector3dForWorld(Vector3D _v,double _angle,int _param)
    {
        double _sin = Math.Sin(_angle * Math.PI / 180);
        double _cos = Math.Cos(_angle * Math.PI / 180);
        _v.X *= _param;
        double _x = _v.Z * _sin - _v.X * _cos;
        double _y = _v.Y;
        double _z = _v.X * _sin + _v.Z * _cos;
        return new Vector3D(_x,_y,_z);
    }

    /// <summary>
    /// Determines if is vector target is between the specified target a and b.
    /// System.Math is using right hand principle, Unity.Math uses the left.
    /// </summary>
    /// <returns><c>true</c> if is vector between the specified target a and b; otherwise, <c>false</c>.</returns>
    /// <param name="target">Target.</param>
    /// <param name="a">The vector a.</param>
    /// <param name="b">The vector b.</param>
    public static bool IsVectorBetween(Vector3D target,Vector3D a, Vector3D b)
    {
        Vector3D crossWithA = Vector3D.Cross(target,a);
        Vector3D crossWithB = Vector3D.Cross(target,b);
        return Vector3D.Dot(crossWithA,crossWithB) <= 0;
    }



}