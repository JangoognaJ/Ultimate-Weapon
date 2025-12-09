using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.SceneManagement;

public class scr_playerScript : MonoBehaviour
{

    InputSystem_Actions controls;
    Vector2 moveInput;

    private Vector3 lastMoveDirection = Vector3.zero;

    public GameObject lightAttackPrefab;
    public GameObject heavyAttackPrefab;
    public Transform attackSpawnPoint;

    private float gravityMultiplier = 1.5f;
    private float moveSpeed = 10f;
    private float jumpForce = 10f;
    [SerializeField] private int maxJumps = 2;
    private int jumpsRemaining;
    [SerializeField] private Transform modelRoot;   
    [SerializeField] private float flipDuration = 0.3f;
    private float dashSpeed = 50f;
    private float dashDuration = 0.2f;
    private float dashCooldown = 2f;
    private float lightAttackCooldown = 0.1f;
    private float heavyAttackCooldown = 5f;
    private float nextLightAttackTime = 0f;
    private float nextHeavyAttackTime = 0f;
    [SerializeField] private float rotateSpeed = 10f;
    private Vector3 moveDir = Vector3.zero;

    private float heat = 0f;
    private float maxHeat = 100f;
    private float lastAttackTime = -999f;   
    private float nextDissipateTime = 0f;   
    private bool isOverheated = false;
    private bool isOverheatRoutineRunning = false;
    public float Heat => heat;
    public float MaxHeat => maxHeat;

    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color overheatColor = new Color(0.3f, 0.7f, 1f, 1f); 

    private Color currentBaseColor;


    public int CurrentHealth => currentHealth;
    public int MaxHealth => maxHealth;

    [SerializeField] private int maxHealth = 100;
    [SerializeField] private Renderer[] renderersToFlash;
    private int currentHealth;
    private float invulnerabilityDuration = 1f;


    private bool canDash = true;
    private bool isDashing = false;
    private bool isGrounded = true;
    private bool isInvulnerable = false;
    private bool isFlipping = false;


    Rigidbody rb;

    void Awake()
    {
        {
            controls = new InputSystem_Actions();

            controls.PlayerControls.Movement.performed += ctx =>
            {
                moveInput = ctx.ReadValue<Vector2>();


                if (moveInput.sqrMagnitude < 0.01f)
                    moveInput = Vector2.zero;
            };

            controls.PlayerControls.Movement.canceled += ctx => moveInput = Vector2.zero;

            controls.PlayerControls.Jump.performed += ctx => PlayerJump();

            controls.PlayerControls.Dash.performed += ctx => PlayerDash();

            controls.PlayerControls.LightAttack.performed += ctx => PlayerLightAttack();

            controls.PlayerControls.HeavyAttack.performed += ctx => PlayerHeavyAttack();


            rb = GetComponent<Rigidbody>();
            rb.interpolation = RigidbodyInterpolation.None;  
            rb.constraints = RigidbodyConstraints.FreezeRotationX |
                             RigidbodyConstraints.FreezeRotationZ;

            currentHealth = maxHealth;

            if (renderersToFlash == null || renderersToFlash.Length == 0)
            {
                renderersToFlash = GetComponentsInChildren<Renderer>();
            }

            if (renderersToFlash != null && renderersToFlash.Length > 0)
            {
                currentBaseColor = renderersToFlash[0].material.color;
            }
        }

        jumpsRemaining = maxJumps;

    }

    private void OnEnable()
    {
        controls.PlayerControls.Enable();
    }

    private void OnDisable()
    {
        controls.PlayerControls.Disable();
    }

    private void FixedUpdate()
    {
        if (isDashing) return;

        rb.AddForce(Physics.gravity * (gravityMultiplier - 1f), ForceMode.Acceleration);

        moveDir = Vector3.zero;

        if (moveInput.sqrMagnitude > 0.0001f)
        {
            Vector3 camForward = Camera.main.transform.forward;
            Vector3 camRight = Camera.main.transform.right;

            camForward.y = 0f;
            camRight.y = 0f;

            camForward.Normalize();
            camRight.Normalize();

            moveDir = camForward * moveInput.y + camRight * moveInput.x;

            moveDir.Normalize();

            rb.MovePosition(rb.position + moveDir * moveSpeed * Time.fixedDeltaTime);
        }
        if(moveDir.sqrMagnitude > 0.0001f)
        {
            lastMoveDirection = moveDir;
        }
    }

    private void Update()
    {
        if (!isDashing && moveInput.sqrMagnitude > 0.0001f && moveDir.sqrMagnitude > 0.0001f)
        {
            Quaternion targetRot = Quaternion.LookRotation(moveDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotateSpeed * Time.deltaTime);
        }
        
        

        if (!isOverheated && heat > 0f)
        {
            
            if (Time.time - lastAttackTime >= 1f)
            {
                if (Time.time >= nextDissipateTime)
                {
                    heat = Mathf.Max(0f, heat - 5f);  
                    nextDissipateTime = Time.time + 0.5f; 
                     Debug.Log($"Heat: {heat}");
                }
            }
        }

        UpdateHeatGlow();

    }

    void PlayerDash()
    {
        if (!canDash) return;

        heat = Mathf.Max(0f, heat - 10f);

        StartCoroutine(DashRoutine());
    }

    System.Collections.IEnumerator DashRoutine()
    {
        canDash = false;
        isDashing = true;

        Vector3 dashDir;

        if (lastMoveDirection.sqrMagnitude > 0.0001f)
        {
            dashDir = lastMoveDirection.normalized;
        }
        else
        {
            dashDir = transform.forward;
        }
        rb.linearVelocity = dashDir * dashSpeed;

        yield return new WaitForSeconds(dashDuration);

        rb.linearVelocity = Vector3.zero;
        isDashing = false;

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    void PlayerJump()
    {
    
        if (jumpsRemaining <= 0) return;

        bool isDoubleJump = !isGrounded && jumpsRemaining == 1;

        Vector3 v = rb.linearVelocity;
        v.y = 0f;
        rb.linearVelocity = v;

        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

        isGrounded = false;
        jumpsRemaining--;

     
        if (isDoubleJump)
        {
            StartCoroutine(DoubleJumpFlip());
        }
    }

    System.Collections.IEnumerator DoubleJumpFlip()
    {
        if (isFlipping || modelRoot == null)
            yield break;

        isFlipping = true;

        float elapsed = 0f;
        Quaternion startRot = modelRoot.localRotation;

        while (elapsed < flipDuration)
        {
            float t = elapsed / flipDuration;

       
            float angle = Mathf.Lerp(0f, 360f, t);

            modelRoot.localRotation = startRot * Quaternion.Euler(angle, 0f, 0f);

            elapsed += Time.deltaTime;
            yield return null;
        }

     
        modelRoot.localRotation = startRot;

        isFlipping = false;
    }


    private void OnCollisionEnter(Collision collision)
    {

        foreach (ContactPoint contact in collision.contacts)
        {
            if (contact.normal.y > 0.5f)
            {
                isGrounded = true;
                jumpsRemaining = maxJumps; 
                break;
            }
        }

        scr_baseEnemy enemy = collision.gameObject.GetComponent<scr_baseEnemy>();
        if (enemy != null)
        {
            TakeDamage(enemy.contactDamage);
        }

    }

    private void OnCollisionStay(Collision collision)
    {
        
        scr_baseEnemy enemy = collision.gameObject.GetComponent<scr_baseEnemy>();
        if (enemy != null)
        {
            
            TakeDamage(enemy.contactDamage);
        }
    }

    private void PlayerLightAttack()
    {
        if (isDashing || !isGrounded || isOverheated) return;

        if (Time.time < nextLightAttackTime) return;

        SpawnAttack(lightAttackPrefab);
        AddHeat(5f);

        nextLightAttackTime = Time.time + lightAttackCooldown;
    }

    private void PlayerHeavyAttack()
    {
        if (isDashing || !isGrounded || isOverheated) return;

        if (Time.time < nextHeavyAttackTime) return;

        SpawnAttack(heavyAttackPrefab);
        AddHeat(20f);

        nextHeavyAttackTime = Time.time + heavyAttackCooldown;
    }


    private void SpawnAttack(GameObject prefab)
    {
        if(prefab == null) return;

        Vector3 spawnPosition = attackSpawnPoint ? attackSpawnPoint.position : transform.position;
        Quaternion spawnRotation = transform.rotation;

        Instantiate(prefab, spawnPosition, spawnRotation);
    }

    public void TakeDamage(int amount)
    {
        if (isInvulnerable) return;

        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
            return;
        }

        Debug.Log($"Player took {amount} damage. current HP: {currentHealth}");

        StartCoroutine(InvulnerabilityRoutine());
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth)
            currentHealth = maxHealth;
    }

    private void Die()
    {
        Debug.Log("Player died!");

        StartCoroutine(DeathRoutine());
    }

    private System.Collections.IEnumerator DeathRoutine()
    {
        Debug.Log("The player has died!");

        controls.PlayerControls.Disable();
        rb.linearVelocity = Vector3.zero;
        rb.isKinematic = true;

        if (renderersToFlash != null)
        {
            foreach (var r in renderersToFlash)
            {
                if (r != null)
                    r.enabled = false;
            }
        }

        yield return new WaitForSeconds(3f);

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private System.Collections.IEnumerator InvulnerabilityRoutine()
    {
        isInvulnerable = true;

        float elapsed = 0f;
        float flashInterval = 0.1f;
        bool visible = true;

        while (elapsed < invulnerabilityDuration)
        {
            visible = !visible;

            if (renderersToFlash != null)
            {
                foreach (var r in renderersToFlash)
                {
                    if (r != null)
                        r.enabled = visible;
                }
            }

            yield return new WaitForSeconds(flashInterval);
            elapsed += flashInterval;
        }

        
        if (renderersToFlash != null)
        {
            foreach (var r in renderersToFlash)
            {
                if (r != null)
                    r.enabled = true;
            }
        }

        isInvulnerable = false;
    }
    private void AddHeat(float amount)
    {
        if (isOverheated) return;

        heat += amount;
        heat = Mathf.Min(heat, maxHeat);
        lastAttackTime = Time.time;
        Debug.Log($"Heat gained: {amount}, current heat: {heat}");

        if (heat >= maxHeat && !isOverheatRoutineRunning)
        {
            StartCoroutine(OverheatRoutine());
        }
    }

    public void ReduceHeat(float amount)
    {
        heat = Mathf.Max(0f, heat - amount);
    }

    private System.Collections.IEnumerator OverheatRoutine()
    {
        isOverheatRoutineRunning = true;
        isOverheated = true;

        Debug.Log("Player OVERHEATED!");

        
        TakeDamage(10);
        yield return new WaitForSeconds(2f);

        
        TakeDamage(5);
        yield return new WaitForSeconds(2f);

        
        TakeDamage(5);

        
        while (heat > 0f)
        {
            yield return new WaitForSeconds(0.5f);
            heat = Mathf.Max(0f, heat - 10f);
            Debug.Log($"Cooling down, heat: {heat}");
        }

        isOverheated = false;
        isOverheatRoutineRunning = false;
        lastAttackTime = Time.time; 

        Debug.Log("Player cooled down.");
    }

    private void UpdateHeatGlow()
    {
        if (renderersToFlash == null || renderersToFlash.Length == 0) return;

        
        float t = 0f;

        if (heat >= 40f)
        {
            
            t = Mathf.InverseLerp(40f, maxHeat, heat);
        }

        
        Color targetColor = Color.Lerp(currentBaseColor, overheatColor, t);

        foreach (var r in renderersToFlash)
        {
            if (r != null)
            {
                
                r.material.color = targetColor;

              
                r.material.EnableKeyword("_EMISSION");
                r.material.SetColor("_EmissionColor", targetColor * t);
            }
        }
    }
}

