using UnityEngine;
using UnityEngine.EventSystems;

public class scr_MenuSelect : MonoBehaviour
{
    public GameObject firstButton;

    void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(firstButton);
    }
}
