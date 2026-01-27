using UnityEngine;

public class scr_InteractDestroy : MonoBehaviour
{
    public GameObject objectToDestroy;

    private bool playerInside = false;
    private scr_playerScript player;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;
            player = other.GetComponent<scr_playerScript>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
            player = null;
        }
    }

    private void Update()
    {
        if (playerInside && player != null && player.IsInteracting())
        {
            Destroy(objectToDestroy);
            Destroy(gameObject); // optional: remove trigger after use
        }
    }
}