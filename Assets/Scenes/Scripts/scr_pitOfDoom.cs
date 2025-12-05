using UnityEngine;

public class scr_pitOfDoom : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        scr_playerScript player = collision.gameObject.GetComponent<scr_playerScript>();
        if(player != null)
        {
            player.TakeDamage(player.MaxHealth);
        }
    }
}
