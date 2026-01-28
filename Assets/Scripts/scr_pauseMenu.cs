using UnityEngine;
using UnityEngine.SceneManagement;

public class scr_GameManager : MonoBehaviour
{
    [Header("Pause UI")]
    public GameObject pauseRoot;        // parent panel for pause UI
    public GameObject pauseMainPanel;   // panel with Resume/Controls/Quit
    public GameObject pauseControlsPanel; // panel with controls + Back button

    [Header("Scenes")]
    public string mainMenuSceneName = "MainMenu";

    public bool IsPaused { get; private set; }

    void Start()
    {
        // ensure correct initial state
        Time.timeScale = 1f;
        IsPaused = false;

        if (pauseRoot) pauseRoot.SetActive(false);
        if (pauseMainPanel) pauseMainPanel.SetActive(true);
        if (pauseControlsPanel) pauseControlsPanel.SetActive(false);
    }

    public void TogglePause()
    {
        if (IsPaused) Resume();
        else Pause();
    }

    public void Pause()
    {
        IsPaused = true;
        Time.timeScale = 0f;

        if (pauseRoot) pauseRoot.SetActive(true);
        ShowPauseMain();
    }

    public void Resume()
    {
        IsPaused = false;
        Time.timeScale = 1f;

        if (pauseRoot) pauseRoot.SetActive(false);
    }

    public void ShowPauseControls()
    {
        if (pauseMainPanel) pauseMainPanel.SetActive(false);
        if (pauseControlsPanel) pauseControlsPanel.SetActive(true);
    }

    public void ShowPauseMain()
    {
        if (pauseControlsPanel) pauseControlsPanel.SetActive(false);
        if (pauseMainPanel) pauseMainPanel.SetActive(true);
    }

    public void QuitToMainMenu()
    {
        // IMPORTANT: unpause before changing scenes
        Time.timeScale = 1f;
        IsPaused = false;

        SceneManager.LoadScene(mainMenuSceneName);
    }
}
