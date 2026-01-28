using UnityEngine;

public class scr_lightAttack : MonoBehaviour
{
    public float speed = 60f;
    public float lifeTime = 5f;

    [SerializeField] private int baseDamage = 10;

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
        if (collision.gameObject.CompareTag("Player")) return;

        scr_baseEnemy enemy = collision.gameObject.GetComponent<scr_baseEnemy>();
        if (enemy != null)
        {
            int finalDamage = Mathf.RoundToInt(baseDamage * damageMultiplier);
            enemy.TakeDamage(finalDamage);
        }

        Destroy(gameObject);
    }
}
