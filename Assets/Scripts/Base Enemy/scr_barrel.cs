using UnityEngine;

public class scr_barrel : MonoBehaviour
{
    [SerializeField] private GameObject essencePrefab;
    [SerializeField] private int essenceCount = 5;
    [SerializeField] private float scatterRadius = 1.5f;

    private void OnCollisionEnter(Collision collision)
    {
        // If hit by any attack
        if (collision.gameObject.CompareTag("LightAttack") ||
            collision.gameObject.CompareTag("HeavyAttack"))
        {
            BreakBarrel();
        }
    }

    private void BreakBarrel()
    {
        for (int i = 0; i < essenceCount; i++)
        {
            Vector3 randomOffset = Random.insideUnitSphere * scatterRadius;
            randomOffset.y = 0f; // keep them on the ground

            Instantiate(
                essencePrefab,
                transform.position + randomOffset,
                Quaternion.identity
            );
        }

        Destroy(gameObject);
    }
}