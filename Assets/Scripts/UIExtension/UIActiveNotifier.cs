using UnityEngine;

public class UIActiveNotifier : MonoBehaviour
{
    public GameObject target;
    public string message = "";
    public object data;

    void OnEnable()
    {
        target.SendMessage(message, data);
    }
}