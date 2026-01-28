using UnityEngine;

public class scr_NumberPillar : MonoBehaviour
{
    public int number = 0;
    public scr_CodePuzzleManager manager;

    [Header("Visual")]
    public Renderer rend;
    public Color idleColor = new Color(0f, 1f, 1f, 1f); // glowy-ish cyan
    public Color selectedColor = Color.gray;

    public bool IsSelected { get; private set; }

    void Awake()
    {
        if (rend == null) rend = GetComponentInChildren<Renderer>();
        SetSelected(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!IsAttack(collision.gameObject)) return;

        manager.AddNumber(number, this);
    }

    private bool IsAttack(GameObject obj)
    {
        return obj.CompareTag("LightAttack") || obj.CompareTag("HeavyAttack");
    }

    public void SetSelected(bool selected)
    {
        IsSelected = selected;

        if (rend == null) return;

        Color target = selected ? selectedColor : idleColor;

        rend.material.color = target;

        // handle glow too
        rend.material.EnableKeyword("_EMISSION");
        rend.material.SetColor("_EmissionColor", target);
    }
}