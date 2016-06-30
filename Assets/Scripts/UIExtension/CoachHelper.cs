using System.Collections;
using Common;
using Common.Log;
using UnityEngine;

public class CoachHelper : MonoBehaviour
{
    private Animation mAnimation = null;
    private Transform body;
    private Transform head;

    public enum Coach
    {
        illegal,
        XMN,
        FGS,
        GDOL,
    }

    void Awake()
    {
        mAnimation = gameObject.GetComponentInChildren<Animation>();

        body = transform.Find("Animation/body/body 1");
        head = transform.Find("Animation/All_001/All_002/Root/Bip001 Pelvis/Bip001 Spine/Bip001 Spine1/Bip001 Neck/Head");
    }

    void OnEnable()
    {
        if (mAnimation != null)
        {
            mAnimation.Play();
        }
    }

    public void PlayAnim(string anim, string idle)
    {
        if (mAnimation != null && !mAnimation.IsPlaying(anim))
        {
            mAnimation.Play(anim);
            mAnimation.PlayQueued(idle);
        }
    }

    public void InitUniform(int coachEnum)
    {
        switch ((Coach)coachEnum)
        {
            case Coach.XMN:
                DealBodyMat(new Color(210 / 255f, 210 / 255f, 210 / 255f),
                    new Color(38 / 255f, 38 / 255f, 38 / 255f), 0.4f,
                    ResourceManager.Instance.LoadTexture("Textures/Uniform/shirt/JL_body1"),
                    ResourceManager.Instance.LoadTexture("Textures/Uniform/shirt/JL_body1_normal"));
                break;
            case Coach.FGS:
                DealBodyMat(new Color(210 / 255f, 210 / 255f, 210 / 255f),
                    new Color(65 / 255f, 65 / 255f, 112 / 255f), 0.4f,
                    ResourceManager.Instance.LoadTexture("Textures/Uniform/shirt/JL_body2"),
                    ResourceManager.Instance.LoadTexture("Textures/Uniform/shirt/JL_body2_normal"));
                break;
            case Coach.GDOL:
                DealBodyMat(new Color(210 / 255f, 210 / 255f, 210 / 255f),
                    new Color(72 / 255f, 72 / 255f, 88 / 255f), 0.4f,
                    ResourceManager.Instance.LoadTexture("Textures/Uniform/shirt/JL_body3"),
                    ResourceManager.Instance.LoadTexture("Textures/Uniform/shirt/JL_body3_normal"));
                break;
            default:
                LogManager.Instance.LogError("Cannot deal coach type " + coachEnum);
                break;
        }
    }

    public void FadeOut()
    {
        StartCoroutine(FadeOut(0.3f));
    }

    IEnumerator FadeOut(float dur)
    {
        Material bodyMat = body.renderer.sharedMaterial;
        bodyMat.shader = Shader.Find("Transparent/Bumped Specular");

        Material headMat = head.GetComponentInChildren<Renderer>().sharedMaterial;
        headMat.shader = Shader.Find("Transparent/Bumped Specular");

        float t = Time.realtimeSinceStartup;
        Color color = bodyMat.color;
        float from = color.a;

        while (true)
        {
            float factor = (Time.realtimeSinceStartup - t) / dur;
            if (factor >= 1)
            {
                yield break;
            }
            else
            {
                color.a = Mathf.Lerp(from, 0f, factor);
                bodyMat.color = color;
                headMat.color = color;
                yield return null;   
            }
        }
    }

    public void FadeIn()
    {
        Material bodyMat = body.renderer.sharedMaterial;
        bodyMat.shader = Shader.Find("Bumped Specular");

        Material headMat = head.GetComponentInChildren<Renderer>().sharedMaterial;
        headMat.shader = Shader.Find("Specular");

        Color color = bodyMat.color;
        color.a = 1f;
        bodyMat.color = color;
        headMat.color = color;
    }

    private void DealBodyMat(Color color, Color specColor, float shiniess, Texture mainTexture, Texture normal)
    {
        SkinnedMeshRenderer renderer = body.GetComponent<SkinnedMeshRenderer>();
        Material mat = new Material(Shader.Find("Bumped Specular"));
        mat.SetColor("_Color", color);
        mat.SetColor("_SpecColor", specColor);
        mat.SetFloat("_Shininess", shiniess);
        mat.mainTexture = mainTexture;
        mat.SetTexture("_BumpMap", normal);
        renderer.sharedMaterial = mat;
    }
}
