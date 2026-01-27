using UnityEngine;

public class BridgeTrigger : MonoBehaviour
{
    public static bool trigger1Hit;
    public static bool trigger2Hit;

    public bool isTrigger1; // check this on one trigger, leave unchecked on the other
    public GameObject bridge;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (isTrigger1)
            trigger1Hit = true;
        else
            trigger2Hit = true;

        if (trigger1Hit && trigger2Hit)
        {
            bridge.SetActive(true);
        }
    }
}