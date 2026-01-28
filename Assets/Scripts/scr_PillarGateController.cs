using UnityEngine;

public class scr_PillarGateController : MonoBehaviour
{
    public GameObject gateToDisable;
    public int pillarsNeeded = 2;

    private int pillarsDestroyed = 0;
    private bool done = false;

    public void NotifyPillarDestroyed()
    {
        if (done) return;

        pillarsDestroyed++;

        if (pillarsDestroyed >= pillarsNeeded)
        {
            done = true;

            if (gateToDisable != null)
                gateToDisable.SetActive(false);

            // Destroy all enemies
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (var e in enemies)
                Destroy(e);
        }
    }
}