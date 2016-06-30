using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TweenCircle : UITweener
{

    public Vector3 from;
    public Vector3 to;

    Transform mTransCircle;

    public Transform cachedTransformCircle { get { if (mTransCircle == null) mTransCircle = transform; return mTransCircle; } }

    public Vector3 valueCircle { get { return cachedTransformCircle.localEulerAngles; } set { cachedTransformCircle.localEulerAngles = value; } }

    protected override void OnUpdate(float factor, bool isFinished)
    {
        valueCircle = Vector3.Slerp(from, to, factor);

        CalcDragEndPos();
    }

    static public TweenCircle Begin(GameObject go, float duration, Vector3 to)
    {
        TweenCircle comp = UITweener.Begin<TweenCircle>(go, duration);
        comp.from = comp.valueCircle;
        comp.to = to;
        if (duration <= 0f)
        {
            comp.Sample(1f, true);
            comp.enabled = false;
        }
        return comp;
    }
    //////////////////////////////////////////////////////////////////////////////////////////

    public GameObject[] objCircle;
    private float coeffRotate = 0.2f;

    public delegate void OnDragEndCircleDelegate(int id_);
    public delegate void OnDragCircleDelegate(GameObject go, Vector2 delta);
    public delegate void OnDragStartCircleDelegate(GameObject go);
    private Transform mTrans;
    private int m_offsetDeg;
    private float m_offset;
    private float m_offsetDelta;
    private int m_centerId;
    private int m_posIndex;
    private Vector3 m_centerPos;

    public Vector3 ChildValue
    {
        set 
        {
            for (int i = 0; i < objCircle.Length; ++i )
                objCircle[i].transform.localEulerAngles = value;
            
        }
    }
    public OnDragEndCircleDelegate OnDragEndCircle;
    public OnDragCircleDelegate OnDragCircle;
    public OnDragStartCircleDelegate OnDragStartCircle;
	// Use this for initialization
    void Start()
    {
        m_centerId = 3;
        if (objCircle != null)
            m_offsetDeg = 360/objCircle.Length;


        for (int i = 0; i < objCircle.Length; ++i)
        {
            UIEventListener.Get(objCircle[i]).onDragStart = OnDragStart;
            UIEventListener.Get(objCircle[i]).onDrag = OnDrag;
            UIEventListener.Get(objCircle[i]).onDragEnd = OnDragEnd;
        }
        m_centerPos = objCircle[2].transform.position;

        InitCirclePos();
    }

    //// Update is called once per frame
    //void Update()
    //{

    //}
    void InitCirclePos()
    {
        m_offset = -m_offsetDeg * m_posIndex;
        Begin(gameObject, 0.0f, new Vector3(0, 0, -m_offsetDeg * m_posIndex));
        ChildValue = new Vector3(0, 0, m_offsetDeg * m_posIndex);
    }

    public void SetCirclePos(int index_)
    {
        m_posIndex = index_;
    }

    public void OnDragStart(GameObject go)
    {
        if (OnDragStartCircle != null)
        {
            OnDragStartCircle(go);
        }
    }
    
    public void OnDrag(GameObject go, Vector2 delta)
    {
        m_offsetDelta = Mathf.Abs(delta.x + delta.y) * coeffRotate;

        if (delta.y < 0)
        {
            m_offset -= m_offsetDelta;
        }
        else
        {
            m_offset += m_offsetDelta;
        }

        valueCircle = new Vector3(0, 0, m_offset);
        ChildValue = new Vector3(0, 0, -m_offset);

        if (OnDragCircle != null)
        {
            OnDragCircle(go, delta);
        }
    }

    public void OnDragEnd(GameObject go_)
    {
        float offset = Mathf.Floor(cachedTransformCircle.localEulerAngles.z / m_offsetDeg) * m_offsetDeg;

        Begin(gameObject, 0.2f, new Vector3(0, 0, offset));

        ChildValue = new Vector3(0, 0, -offset);
        m_offset = offset;
    }

    void CalcDragEndPos()
    {
        if (OnDragEndCircle != null)
        {
            for (int i = 0; i < objCircle.Length; ++i)
            {
                if (objCircle[i].transform.position.x > m_centerPos.x - 0.1f && objCircle[i].transform.position.x < m_centerPos.x + 0.1f &&
                    objCircle[i].transform.position.y > m_centerPos.y - 0.1f && objCircle[i].transform.position.y < m_centerPos.y + 0.1f)
                {
                    m_centerId = int.Parse(objCircle[i].name);
                    OnDragEndCircle(m_centerId);
                }
            }

        }
    }
}
