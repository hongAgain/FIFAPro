using Common.Tables;
using System.Collections.Generic;
using UnityEngine;

public class GhostFx : MonoBehaviour
{

    class CloneMeshObject
    {

        public CloneMeshObject(Transform kTransform,int iLayer,float fDelta)
        {
            m_kMeshObject = new GameObject();
            m_kMeshObject.layer = iLayer;
            m_kMeshObject.transform.position = kTransform.position;
            m_kMeshObject.transform.rotation= kTransform.rotation;
            m_kMeshObject.transform.localScale = kTransform.localScale;
            m_fDelta = fDelta;
        }

        public void Init()
        {
            m_kRenderer = m_kMeshObject.GetComponentsInChildren<MeshRenderer>();
        }
        public void Update(float fTime)
        {
            for(int i = 0;i < m_kRenderer.Length;i++)
            {
                MeshRenderer kRenderer = m_kRenderer[i];
                if (null == kRenderer)
                    continue;
                //if(kRenderer.material.HasProperty("_AllPower"))
                //{
                //    float fVal = kRenderer.material.GetFloat("_AllPower");
                //    fVal += (m_fDelta*fTime);
                //    kRenderer.material.SetFloat("_AllPower", fVal);
                //}
            }
        }
        public void Destroy()
        {
            if(null != m_kMeshObject)
            {
                GameObject.Destroy(m_kMeshObject);
                m_kMeshObject = null;
            }
        }
        public GameObject MeshObject
        {
            get { return m_kMeshObject; }
            set { m_kMeshObject = value; }
        }

        public float AliveTime
        {
            get { return m_fAliveTime; }
            set { m_fAliveTime = value; }
        }
        private MeshRenderer[] m_kRenderer;
        private GameObject m_kMeshObject = null;    // 克隆的对象
        private float m_fAliveTime = 0;             // 已存活时间
        private float m_fDelta = 0;
    }

    void Start()
    {
        GhostEffectTable kTable = TableManager.Instance.GhostEffectTbl;
        if (null == kTable)
            return;
        m_kEffectItem = kTable.GetItem(m_iID);
        if (null != m_kEffectItem)
            m_bInitialized = true;
        m_dAllPowerDelta = (m_kEffectItem.AllPowerEnd- m_kEffectItem.AllPowerBegin) / m_kEffectItem.LiveTime;
        m_fElapseTime = 0;
        m_bStartClone = false;
        //m_kAnimation.transform.localScale
        m_bMirror = false;

        m_kAnimation = gameObject.GetComponentInChildren<Animation>();
        m_kSkinRendererList = gameObject.GetComponentsInChildren<SkinnedMeshRenderer>();
        m_kHeadObj = gameObject.transform.FindChild("Animation/All_001/All_002/Root/Bip001 Pelvis/Bip001 Spine/Bip001 Spine1/Bip001 Neck/Head").gameObject;
        m_kMeshRendererList = m_kHeadObj.GetComponentsInChildren<MeshRenderer>();
        m_kAniObj = gameObject.transform.FindChild("Animation").gameObject;
        if (m_kAniObj.transform.localScale.x < 0)
            m_bMirror = true;
        GameObject kPosObj = m_kAniObj.transform.FindChild("Position").gameObject;
        m_kScale = kPosObj.transform.localScale;
        m_kRot = m_kAnimation.transform.localRotation;
        m_iLayer = LayerMask.NameToLayer("SceneFx");
        CreateMaterial();
        m_iClonedNum = 0;
    }


    private void CreateMaterial()
    {
        m_kMirrorMaterial = new Material(Shader.Find("Custom/GlowMirror"));
        if (m_kMirrorMaterial.HasProperty("_AllPower"))
            m_kMirrorMaterial.SetFloat("_AllPower", (float)m_kEffectItem.AllPowerBegin);
        m_kMaterial = new Material(Shader.Find("Custom/Glow"));
        if (m_kMaterial.HasProperty("_AllPower"))
            m_kMaterial.SetFloat("_AllPower", (float)m_kEffectItem.AllPowerBegin);

    }
    // Update is called once per frame
    void Update()
    {
        UpdateCloneMesh();
        if (false == m_bInitialized)
            return;
        if (m_iClonedNum >= m_kEffectItem.Count)
            return;
        if(false == m_bStartClone)
        {
            m_fElapseTime += (GlobalBattleInfo.Instance.DeltaTime/ GlobalBattleInfo.Instance.ScaleTime);
            if(m_fElapseTime > m_kEffectItem.StartTime)
            {
                CloneMesh();
                m_fElapseTime = 0;
                m_bStartClone = true;   
            }
        }
        else
        {
            if(m_fElapseTime > m_kEffectItem.DeltaTime)
            {
                CloneMesh();
                m_fElapseTime = 0;
            }
            else
            {
                m_fElapseTime += (GlobalBattleInfo.Instance.DeltaTime / GlobalBattleInfo.Instance.ScaleTime);
            }
        }
    }

    void UpdateCloneMesh()
    {
        for(int i = m_kClonedObjList.Count - 1; i >= 0;i-- )
        {
            CloneMeshObject kObject = m_kClonedObjList[i];
            if (null == kObject)
                continue;
            kObject.AliveTime += (GlobalBattleInfo.Instance.DeltaTime / GlobalBattleInfo.Instance.ScaleTime); 
            kObject.Update(GlobalBattleInfo.Instance.DeltaTime / GlobalBattleInfo.Instance.ScaleTime);
            if (kObject.AliveTime> m_kEffectItem.LiveTime)
            {
                kObject.Destroy();
                m_kClonedObjList.Remove(kObject);
            }
        }
    }

    void OnDestroy()
    {
        for (int i = 0; i < m_kClonedObjList.Count; i++)
        {
            m_kClonedObjList[i].Destroy();
        }
        m_kClonedObjList.Clear();
        m_iClonedNum = 0;
    }

    private void CloneMesh()
    {
        
        CloneMeshObject kCloneMeshObject = new CloneMeshObject(m_kAniObj.transform, m_iLayer, (float)m_dAllPowerDelta);
        for (int i = 0; i < m_kSkinRendererList.Length; i++)
        {
            string strName = m_kSkinRendererList[i].gameObject.transform.name;
            if (strName == "ball" || strName == "right_wrister")
                continue;
            Mesh kFrameMesh = new Mesh();
            m_kAnimation.Sample();
            m_kSkinRendererList[i].BakeMesh(kFrameMesh);
            
            GameObject kChildObj = new GameObject();
            kChildObj.name = strName;
            kChildObj.transform.parent = kCloneMeshObject.MeshObject.transform;
            kFrameMesh.name = string.Format("Baked Mesh");
            Vector3 kPos = m_kSkinRendererList[i].transform.localPosition;
            kPos.Scale(m_kScale);
            kChildObj.transform.localPosition = kPos;
            kChildObj.transform.localRotation = m_kSkinRendererList[i].transform.localRotation;
            kChildObj.transform.localScale = m_kSkinRendererList[i].transform.localScale;

            MeshFilter kMeshFilter = kChildObj.AddComponent<MeshFilter>();
            kMeshFilter.mesh = kFrameMesh;
            MeshRenderer kRenderer = kChildObj.AddComponent<MeshRenderer>();
            if(m_bMirror)
                kRenderer.sharedMaterial = m_kMirrorMaterial;
            else
                kRenderer.sharedMaterial = m_kMaterial;

            kChildObj.layer = m_iLayer;
        }

        for (int i = 0; i < m_kMeshRendererList.Length; i++)
        {
            GameObject kChildObj = GameObject.Instantiate(m_kMeshRendererList[i].gameObject) as GameObject;
            kChildObj.transform.parent = kCloneMeshObject.MeshObject.transform;
            kChildObj.transform.position = m_kHeadObj.transform.position;
            kChildObj.transform.localRotation = m_kHeadObj.transform.localRotation;
            kChildObj.transform.localScale = m_kMeshRendererList[i].transform.localScale;
            MeshRenderer kRenderer = kChildObj.GetComponent<MeshRenderer>();
            kRenderer.sharedMaterial = m_kMaterial;
            kChildObj.layer = m_iLayer;
        }
        kCloneMeshObject.Init();
        m_kClonedObjList.Add(kCloneMeshObject);
        m_iClonedNum++;
    }
    public int ID
    {
        get { return m_iID; }
        set { m_iID = value; }
    }

    private int m_iID = -1;
    private bool m_bInitialized = false;
    private GhostEffectItem m_kEffectItem = null;

    private Animation m_kAnimation = null;
    private SkinnedMeshRenderer[] m_kSkinRendererList;
    private MeshRenderer[] m_kMeshRendererList;
    private Vector3 m_kScale = Vector3.one;
    private Quaternion m_kRot = Quaternion.identity;
    private Material m_kMaterial;
    private Material m_kMirrorMaterial;
    private List<CloneMeshObject> m_kClonedObjList = new List<CloneMeshObject>();
    private GameObject m_kHeadObj = null;
    private float m_fElapseTime = 0;
    private bool m_bStartClone = false;
    private int m_iLayer = -1;
    private int m_iClonedNum = 0;
    private GameObject m_kAniObj = null;
    private double m_dAllPowerDelta = 0;
    private bool m_bMirror = false;
}
