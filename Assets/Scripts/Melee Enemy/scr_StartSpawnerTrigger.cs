using UnityEngine;

public class scr_StartSpawnerTrigger : MonoBehaviour
{
    public scr_meleeSpawner[] spawners; // drag all 6 in here

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        foreach (var spawner in spawners)
        {
            if (spawner != null)
                spawner.StartSpawning();
        }

        Destroy(gameObject); // only trigger once
    }
}