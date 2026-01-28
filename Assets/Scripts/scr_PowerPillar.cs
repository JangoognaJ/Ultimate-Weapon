using UnityEngine;
using UnityEngine.UI;

public class scr_PowerPillar : MonoBehaviour
{
    [Header("Health")]
    public int maxHealth = 250;
    private int currentHealth;

    [Header("UI")]
    public Image healthFill; // assign the Fill image

    [Header("Manager")]
    public scr_PillarGateController controller;

    private void Awake()
    {
        currentHealth = maxHealth;
        UpdateBar();
    }

    private void OnCollisionEnter(Collision collision)
    {
        // simplest: take damage if hit by either attack
        if (collision.gameObject.CompareTag("LightAttack"))
        {
            TakeDamage(10); // pick values you like
        }
        else if (collision.gameObject.CompareTag("HeavyAttack"))
        {
            TakeDamage(50);
        }
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth < 0) currentHealth = 0;

        UpdateBar();

        if (currentHealth <= 0)
            Die();
    }

    private void UpdateBar()
    {
        if (healthFill != null)
            healthFill.fillAmount = (float)currentHealth / maxHealth;
    }

    private void Die()
    {
        if (controller != null)
            controller.NotifyPillarDestroyed();

        Destroy(gameObject);
    }
}