using UnityEngine;

public class scr_heavyAttack : MonoBehaviour
{
    public float speed = 10f;
    public float lifeTime = 2f;

    Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void OnEnable()
    {

        rb.linearVelocity = transform.forward * speed;

    }

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.CompareTag("Player")) return;

        scr_enemyHealth enemy = collision.gameObject.GetComponent<scr_enemyHealth>();
        if (enemy != null)
        {
            enemy.TakeDamage(50);
        }

        Destroy(gameObject);
    }
}