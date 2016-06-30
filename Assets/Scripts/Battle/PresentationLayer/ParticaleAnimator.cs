using UnityEngine;

public class ParticaleAnimator : MonoBehaviour
{
    private void Awake()
    {
    }

    // Use this for initialization
    void Start()
    {
        particle = GetComponentsInChildren<ParticleSystem>();
        lastTime = Time.realtimeSinceStartup;
    }

    // Update is called once per frame
    void Update()
    {

        float deltaTime = Time.realtimeSinceStartup - (float)lastTime;
        float fDeltaTime = Time.deltaTime;
        float fUnscaledTime = Time.unscaledDeltaTime;
        for(int i = 0;i < particle.Length;i++)
            particle[i].Simulate(Time.unscaledDeltaTime, true, false); //last must be false!!

        lastTime = Time.realtimeSinceStartup;
    }

    private double lastTime;
    private ParticleSystem[] particle;

}