using UnityEngine;

public class scr_MenuPages : MonoBehaviour
{
    public GameObject mainMenuPanel;
    public GameObject controlsPanel;

    public void ShowControls()
    {
        mainMenuPanel.SetActive(false);
        controlsPanel.SetActive(true);
    }

    public void ShowMainMenu()
    {
        controlsPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }
}
