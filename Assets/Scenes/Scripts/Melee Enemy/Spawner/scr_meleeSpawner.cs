using UnityEngine;

public class scr_meleeSpawner : MonoBehaviour
{
    public GameObject prefabToSpawn;
    public float spawnInterval = 15f;

    void Start()
    {
        InvokeRepeating(nameof(SpawnOne), 0f, spawnInterval);
    }

    void SpawnOne()
    {
        if (prefabToSpawn == null) return;

        Instantiate(prefabToSpawn, transform.position, transform.rotation);
    }
}
