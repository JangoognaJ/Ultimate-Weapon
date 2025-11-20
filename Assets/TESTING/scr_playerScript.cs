using UnityEngine;
using UnityEngine.Animations;

public class scr_playerScript : MonoBehaviour
{

    InputSystem_Actions controls;
    Vector2 moveInput;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 6f;
    [SerializeField] private float dashSpeed = 50f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private float dashCooldown = 2f;


    private bool canDash = true;
    private bool isDashing = false;
    private bool isGrounded = true;

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

            controls.PlayerControls.Jump.performed += ctx => Jump();

            controls.PlayerControls.Dash.performed += ctx => PlayerDash();

            rb = GetComponent<Rigidbody>();

            rb.interpolation = RigidbodyInterpolation.Interpolate;

            rb.constraints = RigidbodyConstraints.FreezeRotationX |
                             RigidbodyConstraints.FreezeRotationZ;
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

    void Jump()
    {
        if (!isGrounded) return;

        isGrounded = false;
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
}
