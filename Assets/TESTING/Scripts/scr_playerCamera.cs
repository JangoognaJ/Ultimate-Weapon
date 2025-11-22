using UnityEngine;

public class scr_playerCamera : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;

    private float fixedY;

    private void Start()
    {
        fixedY = transform.position.y; 
    }

    private void LateUpdate()
    {
        if (!target) return;

        Vector3 pos = target.position + offset;
        pos.y = fixedY; // lock Y

        transform.position = pos;
    }
}