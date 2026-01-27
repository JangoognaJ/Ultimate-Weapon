using UnityEngine;

public class scr_BridgeTrigger : MonoBehaviour
{
    public static bool triggerAHit = false;
    public static bool triggerBHit = false;

    public bool isTriggerA;   // check this on one trigger, leave unchecked on the other
    public GameObject bridge; // assign the same bridge to both

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (isTriggerA)
            triggerAHit = true;
        else
            triggerBHit = true;

        if (triggerAHit && triggerBHit)
        {
            bridge.SetActive(true);
        }
    }
}
