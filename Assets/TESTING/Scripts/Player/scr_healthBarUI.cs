using UnityEngine;
using UnityEngine.UI;

public class scr_healthBarUI : MonoBehaviour
{
    public scr_playerScript player;
    public Image fillImage;
    public float smoothSpeed = 8f;

    private float currentFill = 1f;

    private void Start()
    {
        if (player != null)
        {
            currentFill = (float)player.CurrentHealth / player.MaxHealth;
            fillImage.fillAmount = currentFill;
        }
    }

    private void Update()
    {
        if (player == null || fillImage == null) return;

        
        float target = (float)player.CurrentHealth / player.MaxHealth;

        
        currentFill = Mathf.Lerp(currentFill, target, smoothSpeed * Time.deltaTime);

        
        fillImage.fillAmount = Mathf.Clamp01(currentFill);
    }
}
