using UnityEngine;

public class scr_followAndBob : MonoBehaviour
{
    public Transform target;          // the player
    public Vector3 offset = new Vector3(0, 2f, 0);
    public float followSpeed = 5f;

    public float bobHeight = 0.3f;
    public float bobSpeed = 2f;

    private float startY;

    void Start()
    {
        startY = transform.position.y;
    }

    void Update()
    {
        if (target == null) return;

        // Follow player position
        Vector3 followPos = target.position + offset;

        // Bob up and down
        float bob = Mathf.Sin(Time.time * bobSpeed) * bobHeight;

        followPos.y += bob;

        transform.position = Vector3.Lerp(transform.position, followPos, followSpeed * Time.deltaTime);
    }
}