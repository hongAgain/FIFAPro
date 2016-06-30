using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{
    public Transform target = null;
    public Vector3 offsetPosition = Vector3.zero;

    void Start()
    {

    }

    void LateUpdate()
    {
        if (target != null)
        {
            oldPos = transform.position;
            Vector3 newPos = new Vector3(target.position.x + offsetPosition.x, target.position.y + offsetPosition.y, target.position.z + offsetPosition.z);

            float lerpX = Mathf.Lerp(oldPos.x, newPos.x, Time.time);
            float lerpY = Mathf.Lerp(oldPos.y, newPos.y, Time.time);
            float lerpZ = Mathf.Lerp(oldPos.z, newPos.z, Time.time);
            transform.position = new Vector3(lerpX, lerpY, lerpZ); 
            transform.LookAt(target);
        }
    }
    private Vector3 oldPos = Vector3.zero;

}
