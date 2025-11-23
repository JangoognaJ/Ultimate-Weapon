using UnityEngine;

public class scr_playerCamera : MonoBehaviour
{
    public static scr_playerCamera Instance;   // easy access for explosions

    public Transform target;
    public Vector3 offset;

    private float fixedY;

    // shake stuff
    private float shakeTimer = 0f;
    private float shakeMagnitude = 0f;
    private Vector3 shakeOffset = Vector3.zero;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        fixedY = transform.position.y;
    }

    private void LateUpdate()
    {
        if (!target) return;

       
        if (shakeTimer > 0f)
        {
            shakeTimer -= Time.deltaTime;

            shakeOffset = new Vector3(
                Random.Range(-1f, 1f) * shakeMagnitude,
                Random.Range(-1f, 1f) * shakeMagnitude,
                0f
            );

            if (shakeTimer <= 0f)
                shakeOffset = Vector3.zero;
        }

        
        Vector3 pos = target.position + offset + shakeOffset;
        pos.y = fixedY;   // lock Y

        transform.position = pos;
    }

    
    public void Shake(float duration, float magnitude)
    {
        shakeTimer = duration;
        shakeMagnitude = magnitude;
    }
}