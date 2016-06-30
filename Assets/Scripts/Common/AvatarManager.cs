using Common;
using Common.Tables;
using System.Collections;
using System.Collections.Generic;
using Common.Log;
using LuaInterface;
using UnityEngine;

public class AvatarManager : MonoBehaviour
{
    public static AvatarManager Instance;

    public void Awake()
    {
        Instance = this;
    }

    public GameObject CreateRoleObject(int iHeroID, string strActionCollection, int iClubID,bool bHost)
    {
        HeroItem kItem = TableManager.Instance.HeroTbl.GetItem(iHeroID);
        if (null == kItem)
            return null;
        return CreateRoleObject(iHeroID, iClubID, strActionCollection, kItem, bHost);
    }

    public GameObject CreateRoleObjectForGoalCeleB(int iHeroID, string strActionCollection, int iClubID, bool bHost)
    {
        HeroItem kItem = TableManager.Instance.HeroTbl.GetItem(iHeroID);
        if (null == kItem)
            return null;
        kItem.ModelID = 5;
        return CreateRoleObject(iHeroID, iClubID, strActionCollection, kItem, bHost);
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="uiHeroID"></param>
    /// <param name="iInClubID"></param>
    /// <param name="strPrefabName"></param>
    /// <param name="kItem"></param>
    /// <param name="bHost"> true 为主场，false </param>
    /// <returns></returns>
    public GameObject CreateRoleObject(int uiHeroID, int iInClubID, string strPrefabName, HeroItem kItem,bool bHost)
    {
        GameObject kPlayer = ResourceManager.Instance.Load("Char/" + strPrefabName, true) as GameObject;
        if (null == kPlayer)
            kPlayer = new GameObject("EmptyAnimation");
        string strID = string.Format("{0}", kItem.ModelID);
        kPlayer.name = strID;
        kPlayer.transform.localPosition = Vector3.zero;
        kPlayer.transform.localRotation = Quaternion.identity;

        string strModelID = strID;
        if (m_bBattleMode)
            strModelID = "1"+ strModelID;           // 战斗用的模型
        GameObject kBodyObj = ResourceManager.Instance.Load("Char/Body/" + strModelID, true) as GameObject;
        List<Transform> kChildList = new List<Transform>();
        foreach (Transform kChild in kBodyObj.transform)
            kChildList.Add(kChild);
        Transform kAnimObj = kPlayer.transform.FindChild("Animation");
        foreach (Transform kChild in kChildList)
            kChild.parent = kAnimObj;
        GameObject.Destroy(kBodyObj);

        //查找head root节点
        Transform kHeadRoot = kAnimObj.Find("All_001/All_002/Root/Bip001 Pelvis/Bip001 Spine/Bip001 Spine1/Bip001 Neck/Head");

        int iClubID = (int.MaxValue == iInClubID) ? kItem.ClubID : iInClubID;

        Transform kBallTrans = kAnimObj.Find("ball");
        if (null != kBallTrans)
        {
            GameObject kBallObj = kBallTrans.gameObject;
            kBallTrans.renderer.material = new Material(Shader.Find("Custom/Uniform"));
            kBallTrans.renderer.material.SetColor("_Color", new Color(190 / 255f, 190 / 255f, 190 / 255f));
            kBallTrans.renderer.material.mainTexture = ResourceManager.Instance.LoadTexture("Textures/soccer texture");
            kBallTrans.renderer.material.SetTexture("_BumpMap", ResourceManager.Instance.LoadTexture("Textures/soccer normal"));
        }

        GameObject kShirt = kAnimObj.Find("Position/shirt").gameObject;
        Material shirtMat = new Material(Shader.Find("Custom/Uniform"));

        string uniformTexSuffix = "1";
        if (false == bHost)
            uniformTexSuffix = "2";
        uniformTexSuffix = (kItem.Pos == 29) ? "3" : uniformTexSuffix;
        Texture shirtMainTexture = ResourceManager.Instance.LoadTexture(string.Format("Textures/Uniform/shirt/shirt_{0}{1}", iClubID, uniformTexSuffix));
        Texture shirtGlossTexture = ResourceManager.Instance.LoadTexture(string.Format("Textures/Uniform/shirt/shirt_{0}{1}_spe", iClubID, uniformTexSuffix));
        Texture shirt_normal = null;
        switch (kItem.ModelID % 3)
        {
            case 1:
                shirt_normal = ResourceManager.Instance.LoadTexture("Textures/Uniform/shirt/shirt_normal_weak");
                break;
            case 2:
                shirt_normal = ResourceManager.Instance.LoadTexture("Textures/Uniform/shirt/shirt_normal");
                break;
            case 0:
                shirt_normal = ResourceManager.Instance.LoadTexture("Textures/Uniform/shirt/shirt_normal_fat");
                break;
        }
        DealMaterial_Shirt(shirtMat, shirtMainTexture, shirt_normal, shirtGlossTexture);
        kShirt.renderer.material = shirtMat;

        GameObject kLeader = kAnimObj.Find("Position/leader").gameObject;
        kLeader.renderer.material = shirtMat;

        GameObject kLeftWrister = kAnimObj.Find("Position/left_wrister").gameObject;
        kLeftWrister.renderer.material = shirtMat;

        GameObject kRightWrister = kAnimObj.transform.Find("Position/right_wrister").gameObject;
        kRightWrister.renderer.material = shirtMat;

        GameObject kBody = kAnimObj.Find("Position/body").gameObject;
        Material bodyMat = new Material(Shader.Find("Custom/AdjustColor"));
        string alpha = (kItem.ModelID >= 4) ? "alpha01" : "alpha02";
        Texture Channel = ResourceManager.Instance.LoadTexture(string.Format("Textures/alpha/{0}", alpha));
        DealMaterial_Body(bodyMat, shirtMainTexture, kItem.SkinColor, Channel, shirt_normal);
        kBody.renderer.material = bodyMat;

        GameObject kShoes = kAnimObj.Find("Position/shoes").gameObject;
        Material shoesMat = new Material(Shader.Find("Custom/Uniform"));
        Texture shoesMainTexture = ResourceManager.Instance.LoadTexture(string.Format("Textures/Uniform/shoes/{0}", kItem.ShoesID));
        Texture normal_shoes = ResourceManager.Instance.LoadTexture(string.Format("Textures/Uniform/shoes/{0}_normal", kItem.ShoesID));
        DealMaterial_Shoes(shoesMat, shoesMainTexture, normal_shoes);
        kShoes.renderer.material = shoesMat;

        GameObject kHose = kAnimObj.Find("Position/hose").gameObject;
        Material hoseMat = new Material(Shader.Find("Custom/Uniform"));
        string strHoseName = string.Format("Textures/Uniform/hose/hose_{0}{1}", iClubID, uniformTexSuffix);
        Texture hoseMainTexture = ResourceManager.Instance.LoadTexture(strHoseName);
        Texture normal_hose = ResourceManager.Instance.LoadTexture("Textures/Uniform/hose/hose_normal");
        DealMaterial_Hose(hoseMat, hoseMainTexture, normal_hose);
        kHose.renderer.material = hoseMat;

        CreateHeadObj(kHeadRoot, kItem.HeadID, Vector3.zero, kAnimObj.GetComponent<Animation>());

        GameObject kNumber = kAnimObj.Find("Position/number").gameObject;
        kNumber.SetActive(false);

        return kPlayer;
    }

    public void CreateLobbyPlayers(string uniformId, LuaTable heroDatas)
    {
        StartCoroutine(DealCreateLobbyPlayers(uniformId, heroDatas));
    }

    private IEnumerator DealCreateLobbyPlayers(string uniformId, LuaTable heroDatas)
    {
        Texture alpha01 = ResourceManager.Instance.LoadTexture("Textures/alpha/alpha01");
        Texture alpha02 = ResourceManager.Instance.LoadTexture("Textures/alpha/alpha02");
        
        Material bodyMat = new Material(Shader.Find("Custom/AdjustColor"));

        Material shirtMat = new Material(Shader.Find("Custom/Uniform"));
        Texture shirt01 = ResourceManager.Instance.LoadTexture(string.Format("Textures/Uniform/shirt/shirt_{0}1", uniformId));
        Texture shirt02 = ResourceManager.Instance.LoadTexture(string.Format("Textures/Uniform/shirt/shirt_{0}3", uniformId));
        Texture shirtGloss01 = ResourceManager.Instance.LoadTexture(string.Format("Textures/Uniform/shirt/shirt_{0}1_spe", uniformId));
        Texture shirtGloss02 = ResourceManager.Instance.LoadTexture(string.Format("Textures/Uniform/shirt/shirt_{0}3_spe", uniformId));
        Texture shirt_normal01 = ResourceManager.Instance.LoadTexture("Textures/Uniform/shirt/shirt_normal_weak");
        Texture shirt_normal02 = ResourceManager.Instance.LoadTexture("Textures/Uniform/shirt/shirt_normal");
        Texture shirt_normal03 = ResourceManager.Instance.LoadTexture("Textures/Uniform/shirt/shirt_normal_fat");

        Material hoseMat = new Material(Shader.Find("Custom/Uniform"));
        Texture hose01 = ResourceManager.Instance.LoadTexture(string.Format("Textures/Uniform/hose/hose_{0}1", uniformId));
        Texture hose02 = ResourceManager.Instance.LoadTexture(string.Format("Textures/Uniform/hose/hose_{0}3", uniformId));
        hoseMat.SetColor("_UniformColor", Color.white * 190 / 255f);
        Texture hose_normal = ResourceManager.Instance.LoadTexture("Textures/Uniform/hose/hose_normal");

        Material shoesMat = new Material(Shader.Find("Custom/Uniform"));

        yield return null;

        int i = 1;
        foreach (var key in heroDatas.Keys)
        {
            var heroId = heroDatas[(double)key].ToString();
            
            HeroItem kItem = TableManager.Instance.HeroTbl.GetItem(System.Convert.ToInt32(heroId));

            GameObject kPlayer = ResourceManager.Instance.Load("Char/LobbyHeroAnim", true) as GameObject;
            if (null == kPlayer)
                kPlayer = new GameObject("EmptyAnimation");
            //kPlayer.name = kItem.ModelID.ToString();
            kPlayer.transform.localPosition = Vector3.zero;
            kPlayer.transform.localRotation = Quaternion.identity;
            Transform animRoot = kPlayer.transform.Find("Animation");

            GameObject kBodyObj = ResourceManager.Instance.Load(string.Format("Char/Body/{0}",kItem.ModelID), true) as GameObject;
            kBodyObj.transform.Find("All_001").parent = animRoot;
            kBodyObj.transform.Find("Position").parent = animRoot;
            GameObject.Destroy(kBodyObj);

            //查找head root节点
            Transform kHeadRoot = kPlayer.transform.Find("Animation/All_001/All_002/Root/Bip001 Pelvis/Bip001 Spine/Bip001 Spine1/Bip001 Neck/Head");

            CreateHeadObj(kHeadRoot, kItem.HeadID, Vector3.zero, animRoot.GetComponent<Animation>());

            var hoseMesh = kPlayer.transform.Find("Animation/Position/hose").GetComponent<SkinnedMeshRenderer>();
            hoseMesh.sharedMaterial = hoseMat;
            
            var shoesMesh = kPlayer.transform.Find("Animation/Position/shoes").GetComponent<SkinnedMeshRenderer>();
            shoesMesh.sharedMaterial = shoesMat;
            Texture shoesMainTexture = ResourceManager.Instance.LoadTexture(string.Format("Textures/Uniform/shoes/{0}", kItem.ShoesID));
            Texture shoes_normal = ResourceManager.Instance.LoadTexture(string.Format("Textures/Uniform/shoes/{0}_normal", kItem.ShoesID));
            DealMaterial_Shoes(shoesMat, shoesMainTexture, shoes_normal);

            var bodyMesh = kPlayer.transform.Find("Animation/Position/body").GetComponent<SkinnedMeshRenderer>();
            bodyMesh.sharedMaterial = bodyMat;

            var combineMesh = CombineLobbyPlayer.Combine(animRoot);
            combineMesh.sharedMaterial = shirtMat;

            Texture shirt_normal = null;
            switch (kItem.ModelID % 3)
            {
                case 1:
                    shirt_normal = shirt_normal01;
                    break;
                case 2:
                    shirt_normal = shirt_normal02;
                    break;
                case 0:
                    shirt_normal = shirt_normal03;
                    break;
            }
            if (kItem.Pos == 29)
            {
                DealMaterial_Body(bodyMesh.material, shirt02, kItem.SkinColor, alpha02, shirt_normal);
                DealMaterial_Shirt(combineMesh.material, shirt02, shirt_normal, shirtGloss02);
                DealMaterial_Hose(hoseMesh.material, hose02, hose_normal);
            }
            else
            {
                DealMaterial_Body(bodyMesh.material, shirt01, kItem.SkinColor, alpha01, shirt_normal);
                DealMaterial_Shirt(combineMesh.material, shirt01, shirt_normal, shirtGloss01);
                DealMaterial_Hose(hoseMesh.material, hose01, hose_normal);
            }

            var anim = animRoot.animation;
            anim.clip = anim.GetClip("POSE_" + i.ToString("D2"));
            anim.Play();

            heroDatas["Go_0" + i] = kPlayer;
            ++i;
        }
        LuaScriptMgr.Instance.CallLuaFunction("LobbySceneManager.InitLobbyPlayers", heroDatas);
    }

	public GameObject CreateCoachObject(int iCoachID, string animePrefabName)
	{
		GameObject kPlayer = ResourceManager.Instance.Load("Char/" + animePrefabName, true) as GameObject;
		if (null == kPlayer)
			kPlayer = new GameObject("EmptyAnimation");
		kPlayer.name = iCoachID.ToString();
		kPlayer.transform.localPosition = Vector3.zero;
		kPlayer.transform.localRotation = Quaternion.identity;

		GameObject kBodyObj = ResourceManager.Instance.Load("Char/Body/Coach1", true) as GameObject;
		List<Transform> kChildList = new List<Transform>();
		foreach (Transform kChild in kBodyObj.transform)
			kChildList.Add(kChild);
		Transform kAnimObj = kPlayer.transform.FindChild("Animation");
		foreach (Transform kChild in kChildList)
			kChild.parent = kAnimObj;
		GameObject.Destroy(kBodyObj);
		
		GameObject kBody = kAnimObj.Find("body/body 1").gameObject;
		//set material properties
		Material shirtMat = new Material(Shader.Find("Custom/AdjustColor"));
		kBody.renderer.material = shirtMat;

		//set head properties
		Transform kHeadRoot = kAnimObj.Find("All_001/All_002/Root/Bip001 Pelvis/Bip001 Spine/Bip001 Spine1/Bip001 Neck/Head");
        CreateHeadObj(kHeadRoot, iCoachID, new Vector3(0.039f, 0.024f, 0f), kAnimObj.GetComponent<Animation>());
		return kPlayer;
	}

    public bool BattleMode
    {
        get { return m_bBattleMode; }
        set { m_bBattleMode = value; }
    }

    private bool m_bBattleMode = false;

    private void CreateHeadObj(Transform root, int headId, Vector3 pos,Animation _animation)
    {
        GameObject kHeadObj = ResourceManager.Instance.Load(string.Format("Char/Head/{0}", headId), true) as GameObject;
        if (kHeadObj != null)
        {

            kHeadObj.transform.parent = root;
            kHeadObj.transform.localPosition = pos;
            HeadDebug _debug = root.gameObject.AddComponent<HeadDebug>();
            if (_debug)
            {
                _debug.m_animation = _animation;
            }
            Material headMat = new Material(Shader.Find("Specular"));
            headMat.mainTexture = ResourceManager.Instance.LoadTexture(string.Format("Textures/Uniform/Head/{0}", headId));
            headMat.color = new Color(250 / 255f, 250 / 255f, 250 / 255f);
            headMat.SetColor("_SpecColor", new Color(60 / 255f, 60 / 255f, 60 / 255f));
            headMat.SetFloat("_Shininess", 0.6f);
            Renderer kHead = kHeadObj.GetComponentInChildren<Renderer>();
            kHead.material = headMat;
        }
        else
        {
            LogManager.Instance.YellowLog("Haven't head asset {0}", headId);
        }
    }

    private void DealMaterial_Body(Material mat, Texture main, Color skin, Texture channel, Texture normal)
    {
        mat.mainTexture = main;
        mat.SetColor("_SkinColor", skin);
        mat.SetTexture("_Channel", channel);
        mat.SetTexture("_NormalMap", normal);
    }

    private void DealMaterial_Shirt(Material mat, Texture main, Texture normal, Texture gloss)
    {
        mat.SetColor("_Color", new Color(190 / 255f, 190 / 255f, 190 / 255f));
        mat.SetColor("_SpecColor", new Color(163 / 255f, 163 / 255f, 163 / 255f));
        mat.SetFloat("_Shininess", 0.4f);
        mat.mainTexture = main;
        mat.SetTexture("_TransGlossTex", gloss);
        mat.SetTexture("_BumpMap", normal);
    }

    private void DealMaterial_Hose(Material mat, Texture main, Texture normal)
    {
        mat.SetColor("_Color", new Color(190 / 255f, 190 / 255f, 190 / 255f));
        mat.SetColor("_SpecColor", new Color(111 / 255f, 111 / 255f, 111 / 255f));
        mat.SetFloat("_Shininess", 0.4f);
        mat.mainTexture = main;
        mat.SetTexture("_BumpMap", normal);
    }

    private void DealMaterial_Shoes(Material mat, Texture main, Texture normal)
    {
        mat.SetColor("_Color", Color.white);
        mat.SetColor("_SpecColor", new Color(0.5f, 0.5f, 0.5f));
        mat.SetFloat("_Shininess", 0.3f);
        mat.mainTexture = main;
        mat.SetTexture("_BumpMap", normal);
    }
}