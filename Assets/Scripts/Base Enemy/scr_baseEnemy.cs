using UnityEngine;

public class scr_baseEnemy : MonoBehaviour
{
    [Header("Stats")]
    public int maxHealth = 100;
    public float moveSpeed = 3f;

    [Header("Flash on Hit")]
    public Renderer[] renderersToFlash;
    public float flashDuration = 0.15f;

    [Header("Contact Damage")]
    public int contactDamage = 10;

    [Header("drops")]
    public GameObject essencePrefab;

    public int CurrentHealth => currentHealth;
    public int MaxHealth => maxHealth;

    protected int currentHealth;
    protected Transform player;
    protected Rigidbody rb;

    private bool isFlashing = false;

    protected virtual void Awake()
    {

        rb = GetComponent<Rigidbody>();

        currentHealth = maxHealth;

      
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;

     
        if (renderersToFlash == null || renderersToFlash.Length == 0)
        {
            renderersToFlash = GetComponentsInChildren<Renderer>();
        }

       
        if (rb != null)
        {
            rb.constraints = RigidbodyConstraints.FreezeRotationX |
                             RigidbodyConstraints.FreezeRotationZ;
        }
    }

    public virtual void TakeDamage(int amount)
    {
        currentHealth -= amount;

        Debug.Log($"{gameObject.name} took {amount} damage! HP left: {currentHealth}");

        if (!isFlashing) StartCoroutine(FlashRoutine());

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        Debug.Log($"{gameObject.name} died!");

        if (essencePrefab != null)
        {
            Instantiate(essencePrefab, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }

    private System.Collections.IEnumerator FlashRoutine()
    {
        isFlashing = true;

        foreach (var r in renderersToFlash)
            r.enabled = false;

        yield return new WaitForSeconds(flashDuration);

        foreach (var r in renderersToFlash)
            r.enabled = true;

        isFlashing = false;
    }
}
