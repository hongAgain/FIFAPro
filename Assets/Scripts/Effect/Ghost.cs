using UnityEngine;
using System.Collections.Generic;
using Common;

public class Ghost : MonoBehaviour
{
    
    void Start()
    {
        m_kAnimation = gameObject.GetComponentInChildren<Animation>();
        m_kSkinnedMeshRenderers = gameObject.GetComponentsInChildren<SkinnedMeshRenderer>();
        //if (null != m_kAnimation.GetClip("ShouMenYuan_puQiu_L"))
        //    m_kAnimation.CrossFade("ShouMenYuan_puQiu_L");
        //m_kDir = Vector3.Normalize(m_kTargetPos);
        GameObject kPosObj = gameObject.transform.FindChild("Position").gameObject;
        m_kScale = kPosObj.transform.localScale;
        m_kMaterial = new Material(Shader.Find("Custom/Glow"));
        //if(m_kMaterial.HasProperty("_RimColor"))
        //    m_kMaterial.SetColor("_RimColor", new Color(0,0,1,0.1f));
        //if (m_kMaterial.HasProperty("_InnerColor"))
        //    m_kMaterial.SetColor("_InnerColor", new Color(0, 0, 1, 0.1f));
    }

    // Update is called once per frame
    void Update()
    {
        //if (m_bRunToTarget)
        //{
        //    CloneMesh();
        //    //m_fElapse += Time.deltaTime;

        //    //if (m_fElapse > 0.05f)
        //    //{
        //    //    CloneMesh();
        //    //}
        //    //else
        //    //    m_fElapse = 0;
        //}

    }
    public void OnGUI()
    {
        GUIContent kGUIContext = new GUIContent();
        kGUIContext.text = "应用";
        if (GUI.Button(new Rect(10, 10, 100, 20), kGUIContext))
        {
        //    m_bRunToTarget = true;
            m_kAnimation.CrossFade(m_AnkName);
            CloneMesh();
        }
        //kGUIContext = new GUIContent();
        //kGUIContext.text = "重置";
        //if (GUI.Button(new Rect(10, 30, 100, 20), kGUIContext))
        //{
        //    m_bRunToTarget = false;
        //    m_fElapse = 0;
        //    gameObject.transform.localPosition = Vector3.zero;
        //    for (int i = 0; i < m_kObjList.Count; i++)
        //    {
        //        Object.Destroy(m_kObjList[i]);
        //    }
        //    m_kObjList.Clear();

        //    gameObject.transform.FindChild("Position").gameObject.SetActive(true);
        //}
    }

    private void CloneMesh()
    {
        //if (m_kObjList.Count > 3)
        //{
        //    //GameObject kRemoveObj = m_kObjList[0];
        //    //m_kObjList.Remove(kRemoveObj);
        //    //Object.Destroy(kRemoveObj);
        //    gameObject.transform.FindChild("Position").gameObject.SetActive(false);
        //    return;
        //}
        //if (m_kObjList.Count == 3)
        //{
        //    if (null != m_kParticles)
        //    {
        //        GameObject.Destroy(m_kParticles);
        //    }

        //    m_kParticles = GameObject.Instantiate(Resources.Load("Particles/shoot_partical01")) as GameObject;
        //}

        GameObject kObj = new GameObject();
     //   kObj.name = string.Format("Baked Mesh {0} ", m_kObjList.Count);
        kObj.transform.localPosition = transform.localPosition;
        kObj.transform.localRotation = transform.localRotation;
        kObj.transform.localScale = transform.localScale;
        for (int i = 0; i < m_kSkinnedMeshRenderers.Length; i++)
        {
            Mesh kFrameMesh = new Mesh();
            m_kAnimation.Sample();
            m_kSkinnedMeshRenderers[i].BakeMesh(kFrameMesh);

            GameObject kChildObj = new GameObject();
            kChildObj.name = string.Format("{0}", i);
            kChildObj.transform.parent = kObj.transform;
            kFrameMesh.name = string.Format("Baked Mesh");
            Vector3 kPos = m_kSkinnedMeshRenderers[i].transform.localPosition;
            kPos.Scale(m_kScale);
            kChildObj.transform.localPosition = kPos + new Vector3(i * 0.05f, 0, 0);
            kChildObj.transform.localRotation = m_kSkinnedMeshRenderers[i].transform.localRotation;
            kChildObj.transform.localScale = m_kSkinnedMeshRenderers[i].transform.localScale;

            MeshFilter kMeshFilter = kChildObj.AddComponent<MeshFilter>();
            kMeshFilter.mesh = kFrameMesh;
            MeshRenderer kRenderer = kChildObj.AddComponent<MeshRenderer>();
            kRenderer.sharedMaterial = m_kMaterial;
        }

     //   m_kObjList.Add(kObj);
    }

    [SerializeField]
    private string m_AnkName = "Pao_TuiShe";
    [SerializeField]
    private Vector3 m_TargetPos = Vector3.zero;
    private Animation m_kAnimation = null;
    //private float m_fElapse = 0;
    //private bool m_bRunToTarget = false;
    //private Vector3 m_kDir;
    //private float m_fSpeed = 5.0f;
    //private List<GameObject> m_kObjList = new List<GameObject>();
    //private MeshFilter[] m_kMeshFilters;
    private SkinnedMeshRenderer[] m_kSkinnedMeshRenderers;
    private Vector3 m_kScale = Vector3.one;
    private Material m_kMaterial;
    //private GameObject m_kParticles = null;
}
