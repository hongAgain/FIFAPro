using UnityEngine;
using UnityEditor;
using System.IO;
using System;

public class AnimPrefabGen : Editor
{
    [MenuItem("FIFA Editor/动画文件/战斗角色动画")]
    public static void CreateBattlePrefab()
    {
        GameObject kGameObj = ReloadPrefab("Assets/Resources/Char/Player.prefab", "/FBX/Animation/Battle/");
        AddAnimations(kGameObj, "/FBX/Animation/Idle/");
        Animation kAnimation = kGameObj.transform.FindChild("Animation").gameObject.GetComponent<Animation>();
        UnityEngine.Object kObj = AssetDatabase.LoadAssetAtPath("Assets/FBX/Animation/Default.anim", typeof(AnimationClip));
        kAnimation.AddClip(kObj as AnimationClip, "Default");
        EditorUtility.SetDirty(kGameObj);
        AssetDatabase.SaveAssets();
    }
    [MenuItem("FIFA Editor/动画文件/主界面角色动画")]
    public static void CreateLobbyPrefab()
    {
        ReloadPrefab("Assets/Resources/Char/LobbyHeroAnim.prefab", "/FBX/Animation/LobbyHeroPose/");

    }

    [MenuItem("FIFA Editor/动画文件/换装动画")]
    public static void CreateAvatarPrefab()
    {
        GameObject kGameObject = ReloadPrefab("Assets/Resources/Char/AvatarAnim.prefab", "/FBX/Animation/Avatar/");
        Animation kAnimation = kGameObject.transform.FindChild("Animation").gameObject.GetComponent<Animation>();
        UnityEngine.Object kObj = AssetDatabase.LoadAssetAtPath("Assets/FBX/Animation/Idle/Idle.anim", typeof(AnimationClip));
        kAnimation.AddClip(kObj as AnimationClip, "Idle");
        EditorUtility.SetDirty(kGameObject);
        AssetDatabase.SaveAssets();
    }


	[MenuItem("FIFA Editor/动画文件/选球员动画")]
	public static void CreateSelRolePrefab()
	{
		GameObject kGameObject = ReloadPrefab("Assets/Resources/Char/RoleSelAnim.prefab", "/FBX/Animation/RoleSel/");
        Animation kAnimation = kGameObject.transform.FindChild("Animation").gameObject.GetComponent<Animation>();
		UnityEngine.Object kObj = AssetDatabase.LoadAssetAtPath("Assets/FBX/Animation/Idle/Idle.anim" , typeof(AnimationClip));
		kAnimation.AddClip(kObj as AnimationClip, "Idle");
		EditorUtility.SetDirty(kGameObject);
		AssetDatabase.SaveAssets();
	}

	[MenuItem("FIFA Editor/动画文件/新手教练动画")]
	public static void CreateCoachScoutPrefab()
	{
		GameObject kGameObject = ReloadPrefab("Assets/Resources/Char/CoachSelectAnim.prefab", "/FBX/Animation/CoachSelect/");
		EditorUtility.SetDirty(kGameObject);
		AssetDatabase.SaveAssets();
	}	
	
	protected static GameObject ReloadPrefab(String strPrefab,String strAniPath)
    {
        GameObject kGameObject = (GameObject)AssetDatabase.LoadAssetAtPath(strPrefab, typeof(GameObject));
        Transform kAnimaTransform = kGameObject.transform.FindChild("Animation");
        Animation kAnimation = kAnimaTransform.gameObject.GetComponent<Animation>();
        if (null != kAnimation)
            GameObject.DestroyImmediate(kAnimation, true);
        EditorUtility.SetDirty(kGameObject);
        GameObject kAniObj = kAnimaTransform.gameObject;
        kAnimation = kAniObj.AddComponent<Animation>();
        DirectoryInfo kDirInfo = new DirectoryInfo(Application.dataPath + strAniPath);
        foreach (FileInfo kFile in kDirInfo.GetFiles("*.anim"))
        {
            string strFileName = kFile.Name;
            strFileName = strFileName.Replace(".anim", "");
            UnityEngine.Object kObj = AssetDatabase.LoadAssetAtPath("Assets" + strAniPath + kFile.Name, typeof(AnimationClip));
            kAnimation.AddClip(kObj as AnimationClip, strFileName);
        }
        EditorUtility.SetDirty(kGameObject);
        AssetDatabase.SaveAssets();
		return kGameObject;
    }
    protected static GameObject AddAnimations(GameObject kGameObject, String strAniPath)
    {
        Animation kAnimation = kGameObject.transform.FindChild("Animation").gameObject.GetComponent<Animation>();
        DirectoryInfo kDirInfo = new DirectoryInfo(Application.dataPath + strAniPath);
        foreach (FileInfo kFile in kDirInfo.GetFiles("*.anim"))
        {
            string strFileName = kFile.Name;
            strFileName = strFileName.Replace(".anim", "");
            UnityEngine.Object kObj = AssetDatabase.LoadAssetAtPath("Assets" + strAniPath + kFile.Name, typeof(AnimationClip));
            kAnimation.AddClip(kObj as AnimationClip, strFileName);
        }
        EditorUtility.SetDirty(kGameObject);
        AssetDatabase.SaveAssets();
        return kGameObject;
    }
}