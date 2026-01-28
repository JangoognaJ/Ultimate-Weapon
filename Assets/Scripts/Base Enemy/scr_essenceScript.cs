using UnityEngine;

public class scr_essence : MonoBehaviour
{
    public float lifeTime = 30f;        
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
           
        }
        else
        {
           
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

        
        

        if (!isHoming && dist <= attractRadius)
        {
            isHoming = true;
           
        }

        
        if (isHoming)
        {
            Vector3 dir = (player.position - transform.position).normalized;
            transform.position += dir * moveSpeed * Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        

        
        scr_playerScript playerScript =
            other.GetComponent<scr_playerScript>() ??
            other.GetComponentInParent<scr_playerScript>();

        if (playerScript != null)
        {
            playerScript.AddEssenceStack();
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