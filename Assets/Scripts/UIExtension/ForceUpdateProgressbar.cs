using UnityEngine;

public class ForceUpdateProgressbar : MonoBehaviour
{
    void Start()
    {
        m_kBarList = gameObject.GetComponentsInChildren<UIProgressBar>();
    }

    void Update()
    {
        for(int i = 0;i < m_kBarList.Length;i++)
        {
            if (null == m_kBarList[i])
                continue;

            m_kBarList[i].ForceUpdate();
        }
    }
    private UIProgressBar[] m_kBarList;
}