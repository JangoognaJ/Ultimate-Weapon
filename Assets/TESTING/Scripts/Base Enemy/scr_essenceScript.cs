using UnityEngine;

public class scr_essence : MonoBehaviour
{
    public float lifeTime = 5f;        
    public float attractRadius = 4f;   
    public float moveSpeed = 8f;       
    public int healAmount = 5;         

    private Transform player;
    private float spawnTime;
    private bool isHoming = false;

    void Start()
    {
        spawnTime = Time.time;

      
        scr_playerScript playerScript = Object.FindFirstObjectByType<scr_playerScript>();
        if (playerScript != null)
        {
            player = playerScript.transform;
            Debug.Log($"Essence: found player {player.name}");
        }
        else
        {
            Debug.LogError("Essence: NO scr_playerScript found in scene!");
        }
    }

    void Update()
    {
       
        if (Time.time - spawnTime >= lifeTime)
        {
            Destroy(gameObject);
            return;
        }

        if (player == null) return;

        float dist = Vector3.Distance(transform.position, player.position);

        
        Debug.Log($"Essence: distance to player = {dist}");

        if (!isHoming && dist <= attractRadius)
        {
            isHoming = true;
            Debug.Log("Essence: started homing");
        }

        
        if (isHoming)
        {
            Vector3 dir = (player.position - transform.position).normalized;
            transform.position += dir * moveSpeed * Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Essence: OnTriggerEnter with {other.name}");

        
        scr_playerScript playerScript =
            other.GetComponent<scr_playerScript>() ??
            other.GetComponentInParent<scr_playerScript>();

        if (playerScript != null)
        {
            Debug.Log("Essence: healing player");
            playerScript.Heal(healAmount);
            playerScript.ReduceHeat(5f);
            Destroy(gameObject);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, attractRadius);
    }
}