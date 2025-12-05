using UnityEngine;

public class scr_movingPlatform1 : MonoBehaviour
{
    private float moveDistance = 20f;
    private float moveSpeed = 0.5f;

    private Vector3 startPos;
    private Rigidbody rb;
    private float t;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        startPos = transform.position;
    }

    private void FixedUpdate()
    {
        t += Time.fixedDeltaTime * moveSpeed;
        float offset = Mathf.Sin(t) * moveDistance;
        Vector3 targetPos = startPos + new Vector3(offset, 0f, 0f);

        rb.MovePosition(targetPos);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(null);
        }
    }

}
