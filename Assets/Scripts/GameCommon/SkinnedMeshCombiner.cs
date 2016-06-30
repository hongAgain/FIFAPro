using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Common.Log;

public class SkinnedMeshCombiner : MonoBehaviour {

	public Animation animNode = null;
	public Transform skinnedMeshRendererRoot = null;
//	public Transform boneRoot = null;
	
	//<shaderName,<textureName,smr list>>
	//<shaderName,<textureName,<skinColor,smr list>>>
	public Dictionary<string,Dictionary<string,Dictionary<Color,List<SkinnedMeshRenderer>>>> SMRGroups = new Dictionary<string,Dictionary<string,Dictionary<Color,List<SkinnedMeshRenderer>>>>();
	public static Dictionary<string,Dictionary<string,Dictionary<Color,Material>>> cachedMaterials = new Dictionary<string,Dictionary<string,Dictionary<Color,Material>>>();

	public bool m_IsCombiner=false;	
	public bool m_IsVisable = true;

	public void Initialize()
	{
		animNode = gameObject.GetComponentInChildren<Animation>();
        // commented by dracula_jin
		//skinnedMeshRendererRoot = gameObject.GetComponent<AvatarBody>().Body_1.transform.parent;
        skinnedMeshRendererRoot = gameObject.transform;
	}

	public void CombineSkinnedMesh(System.Action<Transform,Animation> onOver = null)
	{
		CacheAndStopAnim();
		CombineAllSkinnedMeshTest();
		RePlayCachedAnim();
		if(onOver!=null)
			onOver(transform,animNode);
	}

	private float currentAnimWeight = -1f;
	private string currentAnimName = "";

	private void CacheAndStopAnim()
	{
		if(	animNode!=null && animNode.clip!=null)
		{
			if(animNode.isPlaying)
			{
				currentAnimWeight = -1f;
				//find out the heaviest played anim clip
				foreach(AnimationState a in animNode)
				{
					if(a.enabled && a.weight > currentAnimWeight)
					{
						currentAnimWeight = a.weight;
						currentAnimName = a.name;
					}
				}
				animNode.playAutomatically = false;
				animNode.Stop();
			}
			else
			{
                LogManager.Instance.RedLog("==========>SkinnedMeshCombiner animNode not playing");
			}
		}
	}
	
	private void RePlayCachedAnim()
	{
		//don't know why!
		animNode.gameObject.SetActive(false);
		animNode.gameObject.SetActive(true);

		if(currentAnimName != "" && animNode!=null && animNode.clip!=null)
		{
			animNode.playAutomatically = true;
//			animNode[currentAnimName].wrapMode = WrapMode.Loop;
//			animNode.wrapMode = WrapMode.Loop;
			animNode.Play(currentAnimName);
		}
	}

	/// <summary>
	/// Combine all skinned mesh renderer's mesh together and use a unified mat
	/// </summary>
	private void CombineAllSkinnedMeshTest()
	{		
		//sort and generate smrgroups
		GenerateSMRGroups();				
		//for each list with same shader, generate one skinnedMeshRenderer for them, named by shader		
		foreach (string shaderName in SMRGroups.Keys) 
		{
			foreach (string textureName in SMRGroups[shaderName].Keys) 
			{
				foreach (Color skinColor in SMRGroups[shaderName][textureName].Keys) 
				{
					SkinnedMeshRenderer[] smrs = SMRGroups[shaderName][textureName][skinColor].ToArray ();								
					CacheMaterial(shaderName,textureName,skinColor,smrs[0].sharedMaterial);

                    int combineInstancesCount = 0;
                    int boneCount = 0;
                    int boneWeightCount = 0;
                    for(int i = 0; i<smrs.Length; i++)
                    {
                        combineInstancesCount += smrs[i].sharedMesh.subMeshCount;
                        boneCount += smrs[i].bones.Length;
                        boneWeightCount += smrs[i].sharedMesh.boneWeights.Length;
                    }
                    CombineInstance[] combineInstances = new CombineInstance[combineInstancesCount];
                    Transform[] bones = new Transform[boneCount];
                    Matrix4x4[] bindposes = new Matrix4x4[boneCount];
                    BoneWeight[] boneWeights = new BoneWeight[boneWeightCount];

                    int boneOffset = 0;
                    int combineInstancesIndex = 0;
                    int boneIndex = 0;
                    int boneWeightIndex = 0;

					//search through all smrs in this list
					for (int i = 0; i<smrs.Length; i++)
					{
						SkinnedMeshRenderer smr = smrs [i];

						//add every sub mesh under smr
						for (int sub = 0; sub < smr.sharedMesh.subMeshCount; sub++)
						{
							CombineInstance ci = new CombineInstance ();
							ci.mesh = smr.sharedMesh;
							ci.transform = gameObject.transform.localToWorldMatrix * smr.transform.localToWorldMatrix;
                            combineInstances[combineInstancesIndex++] = ci;
						}
						//set boneweight
						BoneWeight[] meshBoneweight = smr.sharedMesh.boneWeights;
						// May want to modify this if the renderer shares bones as unnecessary bones will get added.
						foreach (BoneWeight bw in meshBoneweight) {
							BoneWeight bWeight = bw;
							bWeight.boneIndex0 += boneOffset;
							bWeight.boneIndex1 += boneOffset;
							bWeight.boneIndex2 += boneOffset;
							bWeight.boneIndex3 += boneOffset;
                            boneWeights[boneWeightIndex++] = bWeight;
						}
						boneOffset += smr.bones.Length;
						
						Transform[] meshBones = smr.bones;
                        for(int j = 0; j<smr.bones.Length; j++)
                        {
                            bones[boneIndex] = smr.bones[j];
                            bindposes[boneIndex] = bones[boneIndex].worldToLocalMatrix * transform.worldToLocalMatrix;
                            boneIndex++;
                        }

						GameObject.Destroy (smr.gameObject);
					}

					//generate one combined SMR under root
					GameObject combinedSMRGO = new GameObject ("Combined_Mesh");
					combinedSMRGO.transform.parent = skinnedMeshRendererRoot;			
					SkinnedMeshRenderer r = combinedSMRGO.AddComponent<SkinnedMeshRenderer> ();
					
					//set combined data
					r.bones = bones;
					r.sharedMaterial = GetCachedMaterial(shaderName,textureName,skinColor);				
					r.sharedMesh = new Mesh ();
					r.sharedMesh.CombineMeshes (combineInstances, true, true);
					//			r.sharedMesh.uv = atlasUVs;			
					r.sharedMesh.boneWeights = boneWeights;			
					r.sharedMesh.bindposes = bindposes;			
					r.sharedMesh.RecalculateBounds ();

					r.castShadows = true;
					r.receiveShadows = false;
				}
			}
		}
	}

	//logic in this function is currently related to the specific architechture in Game FiFa, try to fix it later
	//sort smr using their default material's shader
	private void GenerateSMRGroups()
	{
		SkinnedMeshRenderer[] smrs = skinnedMeshRendererRoot.GetComponentsInChildren<SkinnedMeshRenderer>();
		for(int i=0;i<smrs.Length;i++)
		{
			if(smrs[i]!=null && null != smrs[i].material)
			{
				var mat = smrs[i].material;
                if (false == mat.HasProperty("_SkinColor"))
                    continue;

                var shaderName = mat.shader.name;
                var textureName = mat.mainTexture.name;

                Color skinColor = mat.GetColor("_SkinColor");
				if(SMRGroups.ContainsKey(shaderName))
				{
					if(SMRGroups[shaderName].ContainsKey(textureName))
					{
						if(!SMRGroups[shaderName][textureName].ContainsKey(skinColor))
						{
							SMRGroups[shaderName][textureName][skinColor] = new List<SkinnedMeshRenderer> ();
						}						
						SMRGroups[shaderName][textureName][skinColor].Add(smrs[i]);
					}
					else
					{
						SMRGroups[shaderName][textureName] = new Dictionary<Color, List<SkinnedMeshRenderer>>();
						SMRGroups[shaderName][textureName][skinColor] = new List<SkinnedMeshRenderer> ();
						SMRGroups[shaderName][textureName][skinColor].Add(smrs[i]);
					}					
				}
				else
				{
                    SMRGroups.Add(shaderName, new Dictionary<string, Dictionary<Color, List<SkinnedMeshRenderer>>>());
					SMRGroups[shaderName].Add(textureName,new Dictionary<Color, List<SkinnedMeshRenderer>>());
					SMRGroups[shaderName][textureName].Add(skinColor,new List<SkinnedMeshRenderer> ());
					SMRGroups[shaderName][textureName][skinColor].Add(smrs[i]);
				}
			}
		}
	}

	private void CacheMaterial(string shaderName,string textureName,Color skinColor,Material matToCache)
	{
		if(cachedMaterials.ContainsKey(shaderName))
		{
			if(cachedMaterials[shaderName].ContainsKey(textureName))
			{
				if(cachedMaterials[shaderName][textureName].ContainsKey(skinColor))
				{
					//already cached this material
					return;
				}
				else
				{
//					cachedMaterials[shaderName][textureName].Add(skinColor,matToCache);
					cachedMaterials[shaderName][textureName][skinColor] = matToCache;
				}
			}
			else
			{
				//add this mat to this textureName
				cachedMaterials[shaderName][textureName] = new Dictionary<Color, Material>();
				cachedMaterials[shaderName][textureName][skinColor] = matToCache;
			}
		}
		else
		{
			//add this mat to this shaderName/textureName
//			Dictionary<string,Material> tempDict = new Dictionary<string,Material> ();
//			tempDict.Add(textureName,matToCache);
//			cachedMaterials.Add(shaderName,tempDict);

			cachedMaterials[shaderName] = new Dictionary<string, Dictionary<Color, Material>>();
			cachedMaterials[shaderName][textureName] = new Dictionary<Color, Material>();
			cachedMaterials[shaderName][textureName][skinColor] = matToCache;
		}
	}

	private Material GetCachedMaterial(string shaderName,string textureName,Color skinColor)
	{
//		Debug.LogError("GetCachedMaterial:"+shaderName+"/"+textureName);
		return cachedMaterials[shaderName][textureName][skinColor];
	}
}
