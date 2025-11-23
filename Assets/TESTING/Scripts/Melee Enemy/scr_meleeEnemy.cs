using UnityEngine;
using UnityEngine.UIElements;

public class scr_meleeEnemy : scr_baseEnemy
{
    public float rotateSpeed = 10f;
    public float stopDistance = 0.5f;

    protected override void Awake()
    {
        base.Awake();   
    }

    void FixedUpdate()
    {
        if (player == null || currentHealth <= 0)
            return;

        Vector3 dir = player.position - transform.position;
        dir.y = 0f;

        
        if (dir.sqrMagnitude > 0.001f)
        {
            Quaternion targetRot = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRot,
                rotateSpeed * Time.fixedDeltaTime
            );
        }

        
        float dist = dir.magnitude;

        if (dist > stopDistance)
        {
            Vector3 moveDir = dir.normalized;
            rb.MovePosition(rb.position + moveDir * moveSpeed * Time.fixedDeltaTime);
        }
    }
}