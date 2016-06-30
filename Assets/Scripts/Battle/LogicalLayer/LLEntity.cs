using System;

public class LLEntity
{
    public virtual void Destroy()
    {

    }

    public virtual void Update(double dTime)
    {

    }
    //     public Vector3D Position
    //     {
    //         get { return m_kPos; }
    //         set
    //         {
    //             m_kPos =OffsideFieldJudge(value);
    //             if (Math.Abs(m_kPos.X) > 34.29 || Math.Abs(m_kPos.Z) > 52.75)
    //                 UnityEngine.Debug.LogError("Position====" + Position);
    //         }
    //     }
    /// <summary>
    /// 矫正homeposition是否出界
    /// </summary>
    /// <param name="_pos"></param>
    /// <returns></returns>
    public Vector3D PosAdjustment(Vector3D _pos)
    {
        double _width = 34.29d;
        double _length = 52.75d;
        if (Math.Abs(_pos.X) >= _width)
        {
            if (_pos.X <= 0)
            {
                _pos.X = -(_width - 0.5f);
            }
            else
                _pos.X = (_width - 0.5f);
        }
        if (Math.Abs(_pos.Z) >= _length)
        {
            if (_pos.Z <= 0)
            {
                _pos.Z = -(_length - 0.5f);
            }
            else
                _pos.Z = (_length - 0.5f);
        }
        //        return new Vector3D(_pos.X, _pos.Y, _pos.Z);
        return new Vector3D(_pos.X, 0, _pos.Z);
    }
    //     public double RotateAngle
    //     {
    //         get { return m_dRotAngle; }
    //         set { m_dRotAngle = value; }
    //     }
    public virtual void SetPosition(Vector3D _vec)
    {
        if (CanMoveNext)
            m_kPos = PosAdjustment(_vec);
    }
    public Vector3D GetPosition()
    {
        return m_kPos;
    }
    public void SetRoteAngle(double _angle)
    {
        m_dRotAngle = (_angle+360)%360;
    }

    public double GetRotAngle()
    {
        return m_dRotAngle;
    }

    public bool CanMoveNext
    {
        get { return m_bCanMoveNext; }
        set { m_bCanMoveNext = value; }
    }
    protected Vector3D m_kPos = new Vector3D();
    protected double m_dRotAngle = 0.0f;
    protected bool m_bCanMoveNext = true;
    protected double m_SideWidth = 0d;
    protected double m_SideLength = 0d;
}