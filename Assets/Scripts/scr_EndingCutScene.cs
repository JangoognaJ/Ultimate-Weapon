using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class scr_EndingCutScene : MonoBehaviour
{
    [Header("UI")]
    public GameObject cutsceneRoot;        // your cutscene canvas (or panel)
    public Image blackImage;               // fullscreen black image
    public TMP_Text textUI;                // TMP text
    public CanvasGroup textGroup;          // canvas group on the TMP text

    [Header("Optional: Hide gameplay UI")]
    public GameObject gameplayUI;          // your HUD canvas (optional)

    [Header("Text")]
    public string[] lines;
    public float fadeTime = 1f;
    public float holdTime = 5f;

    [Header("Scene")]
    public string mainMenuSceneName = "MainMenu";

    public void PlayCutscene()
    {
        StopAllCoroutines();
        StartCoroutine(RunCutscene());
    }

    private IEnumerator RunCutscene()
    {
        Debug.Log($"cutsceneRoot: {cutsceneRoot}");
        Debug.Log($"blackImage: {blackImage}");
        Debug.Log($"textUI: {textUI}");
        Debug.Log($"textGroup: {textGroup}");

        if (gameplayUI != null) gameplayUI.SetActive(false);
        if (cutsceneRoot != null) cutsceneRoot.SetActive(true);

        // start black + hide text
        if (blackImage != null)
        {
            Color c = blackImage.color;
            c.a = 1f;
            blackImage.color = c;
        }
        if (textGroup != null) textGroup.alpha = 0f;

        // show each line
        for (int i = 0; i < lines.Length; i++)
        {
            if (textUI != null) textUI.text = lines[i];

            yield return FadeText(0f, 1f);
            yield return new WaitForSecondsRealtime(holdTime);
            yield return FadeText(1f, 0f);
        }

        // fade from black into nothing (optional – for ending you can keep it black too)
        yield return FadeBlack(1f, 1f); // keep black (looks good before loading)

        // load main menu
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuSceneName);
    }

    private IEnumerator FadeText(float from, float to)
    {
        if (textGroup == null) yield break;

        float t = 0f;
        textGroup.alpha = from;

        while (t < fadeTime)
        {
            t += Time.unscaledDeltaTime;
            textGroup.alpha = Mathf.Lerp(from, to, t / fadeTime);
            yield return null;
        }

        textGroup.alpha = to;
    }

    private IEnumerator FadeBlack(float from, float to)
    {
        if (blackImage == null) yield break;

        float t = 0f;
        Color c = blackImage.color;
        c.a = from;
        blackImage.color = c;

        while (t < fadeTime)
        {
            t += Time.unscaledDeltaTime;
            c.a = Mathf.Lerp(from, to, t / fadeTime);
            blackImage.color = c;
            yield return null;
        }

        c.a = to;
        blackImage.color = c;
    }
}