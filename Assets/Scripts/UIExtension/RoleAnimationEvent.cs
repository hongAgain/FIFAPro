using UnityEngine;

public class RoleAnimationEvent : MonoBehaviour
{
    public RoleHelper target;

    void BallVisible()
    {
        target.ball.SetActive(true);
    }

    void BallUnvisible()
    {
        target.ball.SetActive(false);
    }
}