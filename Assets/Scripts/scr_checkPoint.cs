using UnityEngine;

public class scr_Checkpoint : MonoBehaviour
{
    public int checkpointNumber = 1;
    public Transform respawnPoint; // drag the empty child here

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        scr_playerScript player = other.GetComponent<scr_playerScript>();
        if (player != null)
        {
            player.SetCheckpoint(checkpointNumber, respawnPoint);
        }
    }
}