using UnityEngine;
using UnityEngine.UI;

public class scr_enemyHealthBar : MonoBehaviour
{
    public scr_baseEnemy enemy;  
    public Image fillImage;      

    private Transform cam;

    void Start()
    {
        cam = Camera.main.transform;
    }

    void LateUpdate()
    {
        if (enemy == null) return;

        
        float t = (float)enemy.CurrentHealth / enemy.MaxHealth;
        fillImage.fillAmount = t;

       
        if (cam != null)
        {
            transform.forward = cam.forward;
        }
    }
}