using UnityEngine;
using UnityEngine.Animations;

public class scr_playerScript : MonoBehaviour
{

    InputSystem_Actions controls;
    Vector2 moveInput;
    public float moveSpeed = 5f;

    void Awake()
    {
        controls = new InputSystem_Actions();

        controls.PlayerControls.Movement.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.PlayerControls.Movement.canceled += ctx => moveInput = Vector2.zero;
       
    }

    private void OnEnable()
    {
        controls.PlayerControls.Enable();
    }

    private void OnDisable()
    {
        controls.PlayerControls.Disable();
    }

    private void Update()
    {
        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y);

        transform.Translate(move * moveSpeed * Time.deltaTime, Space.World);

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

}
