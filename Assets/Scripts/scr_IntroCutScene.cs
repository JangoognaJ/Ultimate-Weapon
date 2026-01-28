using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class scr_IntroCutscene : MonoBehaviour
{ 

    public GameObject gameplayUI; // drag your HUD canvas here
    public GameObject cutsceneRoot;   // whole canvas
    public TMP_Text textUI;
    public CanvasGroup textGroup;

    public string[] lines;
    public float fadeTime = 1f;
    public float holdTime = 2f;

    public scr_playerScript player; // drag your player here

    void Start()
    {
        StartCoroutine(RunCutscene());
    }

    IEnumerator RunCutscene()
    {
        // freeze player
        if (player != null)
            player.enabled = false;

        // hide gameplay UI
        if (gameplayUI != null)
            gameplayUI.SetActive(false);

        cutsceneRoot.SetActive(true);

        for (int i = 0; i < lines.Length; i++)
        {
            textUI.text = lines[i];

            yield return Fade(0f, 1f);
            yield return new WaitForSeconds(holdTime);
            yield return Fade(1f, 0f);
        }

        yield return FadeBlackOut();

        cutsceneRoot.SetActive(false);

        // show gameplay UI again
        if (gameplayUI != null)
            gameplayUI.SetActive(true);

        // unfreeze player
        if (player != null)
            player.enabled = true;
    }

    IEnumerator Fade(float from, float to)
    {
        float t = 0f;
        textGroup.alpha = from;

        while (t < fadeTime)
        {
            t += Time.deltaTime;
            textGroup.alpha = Mathf.Lerp(from, to, t / fadeTime);
            yield return null;
        }

        textGroup.alpha = to;
    }

    IEnumerator FadeBlackOut()
    {
        Image img = cutsceneRoot.GetComponentInChildren<Image>();
        float t = 0f;
        Color c = img.color;

        while (t < fadeTime)
        {
            t += Time.deltaTime;
            c.a = Mathf.Lerp(1f, 0f, t / fadeTime);
            img.color = c;
            yield return null;
        }
    }
}
