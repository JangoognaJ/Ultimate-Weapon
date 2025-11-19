using UnityEngine;

public class scr_playerCamera : MonoBehaviour
{

    public Transform target;
    public float playercameraSpeed = 5f;
    public Vector3 offset;

    private void LateUpdate()
    {
        Vector3 desiredPosition = target.position + offset;

        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, playercameraSpeed * Time.deltaTime);

        transform.position = smoothedPosition;
    }

}
