using UnityEngine;
using UnityEngine.AI;

public class scr_meleeEnemy : scr_baseEnemy
{
    public float rotateSpeed = 10f;   // optional (agent can rotate too)
    public float stopDistance = 1.0f;

    private NavMeshAgent agent;

    protected override void Awake()
    {
        base.Awake();

        agent = GetComponent<NavMeshAgent>();
        if (agent == null) agent = gameObject.AddComponent<NavMeshAgent>();

        agent.speed = moveSpeed;
        agent.stoppingDistance = stopDistance;

        // Your enemies use Rigidbody right now; easiest is:
        // Make Rigidbody kinematic so it doesn't fight the agent.
        if (rb != null)
        {
            rb.isKinematic = true;
        }
    }

    void Update()
    {
        if (player == null || currentHealth <= 0) return;

        agent.SetDestination(player.position);
    }
}