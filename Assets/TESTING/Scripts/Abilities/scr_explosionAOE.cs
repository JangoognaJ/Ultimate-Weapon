using UnityEngine;

public class scr_explosionAOE : MonoBehaviour
{
    public float radius = 4f;
    public int enemyDamage = 100;
    public int playerDamage = 20;
    public float lifeTime = 0.25f;

    void Start()
    {

        scr_playerCamera.Instance?.Shake(0.25f, 0.5f);

        Collider[] hits = Physics.OverlapSphere(transform.position, radius);

        foreach (Collider col in hits)
        {
            
            scr_baseEnemy enemy = col.GetComponentInParent<scr_baseEnemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(enemyDamage);
                continue;
            }

           
            scr_playerScript player = col.GetComponentInParent<scr_playerScript>();
            if (player != null)
            {
                player.TakeDamage(playerDamage);
            }
        }

        Destroy(gameObject, lifeTime);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
