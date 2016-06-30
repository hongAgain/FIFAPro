using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public class CombineLobbyPlayer
{
	public static SkinnedMeshRenderer Combine(Transform transform)
	{
        Transform[] combinebones = new Transform[4];
        combinebones[0] = transform.Find("Position/leader");
        combinebones[1] = transform.Find("Position/left_wrister");
        combinebones[2] = transform.Find("Position/right_wrister");
        combinebones[3] = transform.Find("Position/shirt");

        SkinnedMeshRenderer[] smrs = new SkinnedMeshRenderer[combinebones.Length];
        for (int i = 0; i < combinebones.Length; ++i)
        {
            smrs[i] = combinebones[i].GetComponent<SkinnedMeshRenderer>();
        }

        int combineInstancesCount = 0;
        int boneCount = 0;
        int boneWeightCount = 0;
        for (int i = 0; i < smrs.Length; i++)
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
        for (int i = 0; i < smrs.Length; i++)
        {
            SkinnedMeshRenderer smr = smrs[i];

            //add every sub mesh under smr
            for (int sub = 0; sub < smr.sharedMesh.subMeshCount; sub++)
            {
                CombineInstance ci = new CombineInstance();
                ci.mesh = smr.sharedMesh;
                ci.transform = smr.transform.localToWorldMatrix;
                combineInstances[combineInstancesIndex++] = ci;
            }
            //set boneweight
            BoneWeight[] meshBoneweight = smr.sharedMesh.boneWeights;
            // May want to modify this if the renderer shares bones as unnecessary bones will get added.
            foreach (BoneWeight bw in meshBoneweight)
            {
                BoneWeight bWeight = bw;
                bWeight.boneIndex0 += boneOffset;
                bWeight.boneIndex1 += boneOffset;
                bWeight.boneIndex2 += boneOffset;
                bWeight.boneIndex3 += boneOffset;
                boneWeights[boneWeightIndex++] = bWeight;
            }
            boneOffset += smr.bones.Length;

            Transform[] meshBones = smr.bones;
            for (int j = 0; j < smr.bones.Length; j++)
            {
                bones[boneIndex] = smr.bones[j];
                bindposes[boneIndex] = bones[boneIndex].worldToLocalMatrix;
                boneIndex++;
            }

            GameObject.Destroy(smr.gameObject);
        }

        //generate one combined SMR under root
        GameObject combinedSMRGO = new GameObject("Combined_Mesh");
        combinedSMRGO.transform.parent = transform;
        SkinnedMeshRenderer r = combinedSMRGO.AddComponent<SkinnedMeshRenderer>();

        //set combined data
        r.bones = bones;
        r.sharedMesh = new Mesh();
        r.sharedMesh.CombineMeshes(combineInstances, true, true);
        //			r.sharedMesh.uv = atlasUVs;			
        r.sharedMesh.boneWeights = boneWeights;
        r.sharedMesh.bindposes = bindposes;
        r.sharedMesh.RecalculateBounds();

        r.castShadows = true;
        r.receiveShadows = false;

        GameObject.Destroy(transform.Find("Position/number").gameObject);
	    return r;
	}
}
