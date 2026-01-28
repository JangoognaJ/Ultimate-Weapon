using UnityEngine;

public class scr_heavyAttack : MonoBehaviour
{
    public float speed = 20f;
    public float lifeTime = 5f;

    [SerializeField] private int baseDamage = 50;

    public GameObject explosionPrefab;

    private float damageMultiplier = 1f;

    Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Called by player via BroadcastMessage
    public void SetDamageMultiplier(float mult)
    {
        damageMultiplier = Mathf.Max(0f, mult);
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
        // If it hits a destructible wall → explode
        if (collision.gameObject.CompareTag("Destructible"))
        {
            Explode();
            return;
        }

        // If it hits a light attack → explode
        if (collision.gameObject.CompareTag("LightAttack"))
        {
            Destroy(collision.gameObject);
            Explode();
            return;
        }

        // Ignore player
        if (collision.gameObject.CompareTag("Player")) return;

        // Damage enemy
        scr_baseEnemy enemy = collision.gameObject.GetComponent<scr_baseEnemy>();
        if (enemy != null)
        {
            int finalDamage = Mathf.RoundToInt(baseDamage * damageMultiplier);
            enemy.TakeDamage(finalDamage);
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
