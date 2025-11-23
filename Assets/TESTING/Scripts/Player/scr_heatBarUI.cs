using UnityEngine;
using UnityEngine.UI;

public class scr_heatBarUI : MonoBehaviour
{
    public scr_playerScript player;
    public Image fillImage;
    public float smoothSpeed = 8f;

    private float currentFill = 0f;

    private void Start()
    {
        if (player != null && player.MaxHeat > 0f)
        {
            currentFill = player.Heat / player.MaxHeat;
            if (fillImage != null)
                fillImage.fillAmount = currentFill;
        }
    }

    void Update()
    {
        if (player == null || fillImage == null || player.MaxHeat <= 0f) return;

        // target heat ratio
        float target = player.Heat / player.MaxHeat;

        // smooth interpolation
        currentFill = Mathf.Lerp(currentFill, target, smoothSpeed * Time.deltaTime);

        // apply to UI image
        fillImage.fillAmount = Mathf.Clamp01(currentFill);
    }
}