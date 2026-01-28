using UnityEngine;

public class scr_SubmitPillar : MonoBehaviour
{
    public scr_CodePuzzleManager manager;

    private void OnCollisionEnter(Collision collision)
    {
        if (!IsAttack(collision.gameObject)) return;

        manager.Submit();
    }

    private bool IsAttack(GameObject obj)
    {
        return obj.CompareTag("LightAttack") || obj.CompareTag("HeavyAttack");
    }
}