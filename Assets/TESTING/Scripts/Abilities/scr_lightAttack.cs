using UnityEngine;

public class scr_lightAttack : MonoBehaviour
{
    public float speed = 30f;
    public float lifeTime = 5f;

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

        scr_baseEnemy enemy = collision.gameObject.GetComponent<scr_baseEnemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(10); 
        }

        Destroy(gameObject);
    }
}