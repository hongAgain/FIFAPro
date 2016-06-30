using UnityEngine;

public class SyncObj : MonoBehaviour
{
    public GameObject lookAtTarget;
    public GameObject syncTarget;
    public Camera shotLookAtCamera = null;
    public Camera shotSyncCamera = null;

    [ContextMenu("Sync")]
    public void Sync()
    {
        Sync(lookAtTarget, syncTarget, shotLookAtCamera, shotSyncCamera);
    }

    public static void Sync(GameObject lookAtTarget, GameObject syncTarget, Camera shotLookAtCamera, Camera shotSyncCamera)
    {
        if (lookAtTarget == null) return;
        if (syncTarget == null) return;

        if (shotLookAtCamera == null) return;
        if (shotSyncCamera == null) return;

        Vector3 screenPos = shotLookAtCamera.WorldToScreenPoint(lookAtTarget.transform.position);

        Vector3 newWorldPos = shotSyncCamera.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, 700f));
        syncTarget.transform.position = shotSyncCamera.transform.localToWorldMatrix.MultiplyPoint(newWorldPos);
    }
}