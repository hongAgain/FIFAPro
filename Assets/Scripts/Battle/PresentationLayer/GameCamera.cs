using System;
using UnityEngine;

public class GameCamera : MonoBehaviour
{
    void Start()
    {
        m_kCamera = gameObject.transform;
        m_kOffset = m_kCamera.position;
        m_kAnimation = gameObject.GetComponent<Animation>();
    }
    void  LateUpdate()
    {
        if (null == m_kTarget || null == m_kCamera)
		{
            return;
		}
        if (null != m_kAnimation && m_kAnimation.isPlaying)
        {
            m_kOffset = m_kCamera.position;
        }
        else
        {
            Vector3 kTargetPos = new Vector3(m_kTarget.position.x, m_kTarget.position.y / 3f, m_kTarget.position.z);
            kTargetPos = kTargetPos + m_kOffset;
            m_kCamera.position = Vector3.Lerp(m_kCamera.position, kTargetPos, Time.deltaTime / m_fSpeed);
			if (m_kCamera.position.z > 50)
			{
				Vector3 tempPos = m_kCamera.position;
				tempPos.z = 50f;
				m_kCamera.position = tempPos;
			}
			else if (m_kCamera.position.z < -50)
			{
				Vector3 tempPos = m_kCamera.position;
				tempPos.z = -50f;
				m_kCamera.position = tempPos;
			}
        }
    }

    public float Speed
    {
        get { return m_fSpeed; }
        set { m_fSpeed = value; }
    }

    public Transform Target
    {
        get { return m_kTarget; }
        set { m_kTarget = value; }
    }

    private float m_fSpeed = 0.2f;
    private Vector3 m_kOffset = Vector3.zero;
    private Transform m_kTarget = null;
    private Transform m_kCamera = null;
    private Animation m_kAnimation = null;
}
