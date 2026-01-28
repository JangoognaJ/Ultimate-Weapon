using UnityEngine;

public class scr_meleeSpawner : MonoBehaviour
{
    public GameObject prefabToSpawn;
    public float spawnInterval = 15f;

    private bool spawning = false;

    public void StartSpawning()
    {
        if (spawning) return;
        spawning = true;

        InvokeRepeating(nameof(SpawnOne), 0f, spawnInterval);
    }

    public void StopSpawning()
    {
        spawning = false;
        CancelInvoke(nameof(SpawnOne));
    }

    void SpawnOne()
    {
        if (prefabToSpawn == null) return;
        Instantiate(prefabToSpawn, transform.position, transform.rotation);
    }
}