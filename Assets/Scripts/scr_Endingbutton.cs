using UnityEngine;

public class scr_EndCutsceneTrigger : MonoBehaviour
{
    public scr_EndingCutScene endingCutscene;

    private scr_playerScript player;
    private bool used = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        player = other.GetComponent<scr_playerScript>();
        Debug.Log("Entered end trigger. Player found? " + (player != null));
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        player = null;
        Debug.Log("Exited end trigger.");
    }

    void Update()
    {
        if (used) return;

        if (player != null && player.IsInteracting())
        {
            Debug.Log("Interact pressed - starting outro cutscene!");

            used = true;

            if (endingCutscene != null)
                endingCutscene.PlayCutscene();
            else
                Debug.LogError("ENDING CUTSCENE NOT ASSIGNED on trigger!");

            GetComponent<Collider>().enabled = false;
        }
    }
}
