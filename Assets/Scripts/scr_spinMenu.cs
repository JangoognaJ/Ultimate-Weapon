using UnityEngine;

public class scr_SlowSpin : MonoBehaviour
{
    public float spinSpeed = 30f; // degrees per second

    void Update()
    {
        transform.Rotate(0f, spinSpeed * Time.deltaTime, 0f);
    }
}