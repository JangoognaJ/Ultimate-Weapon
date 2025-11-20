using UnityEngine;

public class scr_playerCamera : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;

    private void LateUpdate()
    {
        if (!target) return;

        transform.position = target.position + offset;
    }
}