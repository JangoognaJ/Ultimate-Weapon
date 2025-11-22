using UnityEngine;

public class scr_meleeSpawner : MonoBehaviour
{
    public Transform target;
    public Transform spawnPoint;
    public GameObject enemyPrefab;  

    public float rotateSpeed = 5f;    
    public float spawnInterval = 10f; 

    void Start()
    {
        // Auto-find player if not assigned
        if (target == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
                target = playerObj.transform;
        }

        // Start spawning loop
        StartCoroutine(SpawnLoop());
    }

    void Update()
    {
        if (target == null) return;

        // Direction to player, keeping only XZ (no tilt)
        Vector3 dir = target.position - transform.position;
        dir.y = 0f;

        if (dir.sqrMagnitude > 0.001f)
        {
            Quaternion lookRot = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                lookRot,
                rotateSpeed * Time.deltaTime
            );
        }
    }

    System.Collections.IEnumerator SpawnLoop()
    {
        while (true)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnEnemy()
    {
        if (enemyPrefab == null || spawnPoint == null) return;

        Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
    }
}