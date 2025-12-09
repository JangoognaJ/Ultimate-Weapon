using UnityEngine;

public class scr_heavyAttack : MonoBehaviour
{
    public float speed = 20f;
    public float lifeTime = 5f;

   
    public GameObject explosionPrefab;

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

        if (collision.gameObject.CompareTag("LightAttack"))
        {
    
            Destroy(collision.gameObject);

   
            Explode();
            return;
        }

       
        if (collision.gameObject.CompareTag("Player")) return;

  
        scr_baseEnemy enemy = collision.gameObject.GetComponent<scr_baseEnemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(50);
        }

        Destroy(gameObject);
    }

    private void Explode()
    {
        if (explosionPrefab != null)
        {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}