using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.SceneManagement;

public class scr_playerScript : MonoBehaviour
{

    InputSystem_Actions controls;
    Vector2 moveInput;

    public GameObject lightAttackPrefab;
    public GameObject heavyAttackPrefab;
    public Transform attackSpawnPoint;

    private float gravityMultiplier = 1.5f;
    private float moveSpeed = 5f;
    private float jumpForce = 6f;
    private float dashSpeed = 50f;
    private float dashDuration = 0.1f;
    private float dashCooldown = 2f;
    private float lightAttackCooldown = 0.3f;
    private float heavyAttackCooldown = 5f;
    private float nextLightAttackTime = 0f;
    private float nextHeavyAttackTime = 0f;

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

            rb.interpolation = RigidbodyInterpolation.Interpolate;

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

        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y);

        if (move.sqrMagnitude > 0.0001f)
        {
            move = move.normalized;
            rb.MovePosition(rb.position + move * moveSpeed * Time.fixedDeltaTime);
        }
    }

    private void Update()
    {

        RotateTowardsMouse();

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

    void RotateTowardsMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Vector3 lookPos = hit.point;
            lookPos.y = transform.position.y;

            transform.LookAt(lookPos);
        }
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

        if (moveInput.sqrMagnitude > 0.01f)
            dashDir = new Vector3(moveInput.x, 0, moveInput.y).normalized;
        else
            dashDir = transform.forward;

        rb.linearVelocity = dashDir * dashSpeed;

        yield return new WaitForSeconds(dashDuration);

        rb.linearVelocity = Vector3.zero;

        isDashing = false;

        yield return new WaitForSeconds(dashCooldown);

        canDash = true;
    }

    void PlayerJump()
    {
        if (!isGrounded) return;

        isGrounded = false;
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

    }

    private void OnCollisionEnter(Collision collision)
    {
     
        foreach (ContactPoint contact in collision.contacts)
        {
            if (contact.normal.y > 0.5f)
            {
                isGrounded = true;
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
            yield return new WaitForSeconds(0.1f);
            heat = Mathf.Max(0f, heat - 5f);
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

