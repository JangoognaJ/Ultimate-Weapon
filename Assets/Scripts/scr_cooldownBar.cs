using UnityEngine;
using UnityEngine.UI;

public class scr_CooldownBar : MonoBehaviour
{
    public Image fillImage;

    float cooldownDuration = 1f;
    float lastUseTime = -999f;

    public void Setup(float duration)
    {
        cooldownDuration = Mathf.Max(0.0001f, duration);
        fillImage.fillAmount = 1f;
    }

    public void Trigger()
    {
        lastUseTime = Time.time;
        fillImage.fillAmount = 0f; // empty instantly
    }

    void Update()
    {
        float t = (Time.time - lastUseTime) / cooldownDuration;
        fillImage.fillAmount = Mathf.Clamp01(t); // refills over time
    }
}