using UnityEngine;

public class DestroyOnExplosion : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("HeavyAttack"))
        {
            Destroy(gameObject); // or SetActive(false)
        }
    }
}