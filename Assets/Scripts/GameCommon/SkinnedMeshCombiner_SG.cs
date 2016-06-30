using UnityEngine;
using System.Collections.Generic;
using Common.Log;

public class SkinnedMeshCombiner_SG : MonoBehaviour 
{
	public Animation animNode = null;
	public Transform skinnedMeshRendererRoot = null;
	
	public bool m_IsCombiner=false;	
	public bool m_IsVisable = true;
	
	private Dictionary<SkinnedMeshRenderer,SkinnedMeshRenderer> m_HaveCombine = new Dictionary<SkinnedMeshRenderer, SkinnedMeshRenderer>(); 

	public void Initialize()
	{
//		Debug.LogError("Initialize");
		animNode = gameObject.GetComponentInChildren<Animation>();
//		skinnedMeshRendererRoot = gameObject.GetComponent<AvatarBody>().Body_1.transform.parent;
	}

	public void CombineSkinnedMesh(System.Action<Transform,Animation> onOver = null)
	{
		CacheAndStopAnim();
		CombineSkinnedMeshAlgo();
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
                LogManager.Instance.LogError("==========>SkinnedMeshCombiner animNode not playing");
			}
		}
	}
	
	private void RePlayCachedAnim()
	{
		if(currentAnimName != "" && animNode!=null && animNode.clip!=null)
		{
			animNode.playAutomatically = true;
			animNode[currentAnimName].wrapMode = WrapMode.Loop;
			animNode.wrapMode = WrapMode.Loop;
			animNode.Play(currentAnimName);
		}
	}

	public void CombineSkinnedMeshAlgo()
	{
//		SkinnedMeshRenderer[] smRenderers = transform.GetComponentsInChildren<SkinnedMeshRenderer>();
		SkinnedMeshRenderer[] smRenderers = null;
		List<SkinnedMeshRenderer> tmplist = new List<SkinnedMeshRenderer>();		
		int num=0;		
		
		while((tmplist = GetNeedCombine()).Count>1 && num<1)
		{
			num++;			
			smRenderers = tmplist.ToArray();			
			if(smRenderers.Length<1)
			{
                LogManager.Instance.LogError("=========== have no skinnedmeshrenderer");				
				m_IsCombiner = true;				
				return;
			}
			List<Transform> bones = new List<Transform>();			
			List<BoneWeight> boneWeights = new List<BoneWeight>();			
			List<CombineInstance> combineInstances = new List<CombineInstance>();			
			List<Texture2D> textures = new List<Texture2D>();			
			int numSubs = 0;
			
			foreach(SkinnedMeshRenderer smr in smRenderers)
			{
				numSubs += smr.sharedMesh.subMeshCount;
			}
			int[] meshIndex = new int[numSubs];			
			int boneOffset = 0;			
			Shader shader = tmplist[0].material.shader;			
			for( int s = 0; s < smRenderers.Length; s++ ) 
			{
				SkinnedMeshRenderer smr = smRenderers[s];				
				BoneWeight[] meshBoneweight = smr.sharedMesh.boneWeights;
				// May want to modify this if the renderer shares bones as unnecessary bones will get added.
				foreach( BoneWeight bw in meshBoneweight ) 
				{
					BoneWeight bWeight = bw;					
					bWeight.boneIndex0 += boneOffset;					
					bWeight.boneIndex1 += boneOffset;					
					bWeight.boneIndex2 += boneOffset;					
					bWeight.boneIndex3 += boneOffset;					
					boneWeights.Add( bWeight );
				}
				boneOffset += smr.bones.Length;				
				Transform[] meshBones = smr.bones;
				foreach( Transform bone in meshBones )
				{
					bones.Add( bone );
				}
				if( smr.material.mainTexture != null )
				{
					textures.Add( smr.renderer.material.mainTexture as Texture2D );
				}
				CombineInstance ci = new CombineInstance();
				ci.mesh = smr.sharedMesh;
				meshIndex[s] = ci.mesh.vertexCount;				
				ci.transform = gameObject.transform.localToWorldMatrix * smr.transform.localToWorldMatrix;				
				combineInstances.Add( ci );

				Object.Destroy( smr.gameObject );
			}

			List<Matrix4x4> bindposes = new List<Matrix4x4>();			
			for( int b = 0; b < bones.Count; b++ ) 
			{
				Matrix4x4 tmpmatri = bones[b].worldToLocalMatrix * transform.worldToLocalMatrix;				
				bindposes.Add( tmpmatri );
			}
			GameObject newobj = new GameObject("CombinedMesh");			
			newobj.transform.parent = gameObject.transform;			
			SkinnedMeshRenderer r = newobj.AddComponent<SkinnedMeshRenderer>();			
			r.enabled = m_IsVisable;			
			r.sharedMesh = new Mesh();			
			r.sharedMesh.CombineMeshes( combineInstances.ToArray(), true, true );			
			m_HaveCombine.Add(r,r);
			Texture2D skinnedMeshAtlas = new Texture2D( 512, 512 );
			Rect[] packingResult = skinnedMeshAtlas.PackTextures( textures.ToArray(), 0 ,512);
			Vector2[] originalUVs = r.sharedMesh.uv;			
			Vector2[] atlasUVs = new Vector2[originalUVs.Length];
			
			int rectIndex = 0;
			int vertTracker = 0;			
			for( int i = 0; i < atlasUVs.Length; i++ ) 
			{
				atlasUVs[i].x = Mathf.Lerp( packingResult[rectIndex].xMin, packingResult[rectIndex].xMax, originalUVs[i].x );				
				atlasUVs[i].y = Mathf.Lerp( packingResult[rectIndex].yMin, packingResult[rectIndex].yMax, originalUVs[i].y );				
				if( i >= meshIndex[rectIndex]-1 + vertTracker ) 
				{                
					vertTracker += meshIndex[rectIndex];					
					rectIndex++;                
				}
			}
			
			Material combinedMat = new Material( shader );			
			combinedMat.mainTexture = skinnedMeshAtlas;			
			r.sharedMesh.uv = atlasUVs;			
			r.sharedMaterial = combinedMat;			
			r.bones = bones.ToArray();			
			r.sharedMesh.boneWeights = boneWeights.ToArray();			
			r.sharedMesh.bindposes = bindposes.ToArray();			
			r.sharedMesh.RecalculateBounds();

			r.castShadows = true;
			r.receiveShadows = false;
		}
		m_IsCombiner = true;
	}
	
	bool IsSameMaterial(Material mat1,Material mat2)
	{
		if(!mat1.shader.Equals(mat2.shader))
		{
			return false;
		}
		return true;
	}
	
	List<SkinnedMeshRenderer> GetNeedCombine()
	{
		SkinnedMeshRenderer[] smRenderers = transform.GetComponentsInChildren<SkinnedMeshRenderer>();		
		List<SkinnedMeshRenderer> tmplist = new List<SkinnedMeshRenderer>();		
		Material mat = null;
		
		for(int i=0;i<smRenderers.Length;i++)
		{			
			if( smRenderers[i]!= null && !m_HaveCombine.ContainsKey(smRenderers[i]))
			{
				if(mat == null)
				{
					mat = smRenderers[i].material;					
					tmplist.Add(smRenderers[i]);
				}
				else
				{
					if(IsSameMaterial(mat,smRenderers[i].material))
					{
						tmplist.Add(smRenderers[i]);
					}
				}				
			}
		}
		return tmplist;
	}
}