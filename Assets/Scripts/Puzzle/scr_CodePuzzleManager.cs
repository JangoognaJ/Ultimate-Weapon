using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_CodePuzzleManager : MonoBehaviour
{
    [Header("Correct Code")]
    public int[] correctCode = new int[] { 4, 2, 0, 6, 7 };

    [Header("References")]
    public GameObject gateToDisable;
    public Renderer submitRenderer;

    [Header("Colors")]
    public Color submitIdleColor = Color.white;
    public Color submitWrongColor = Color.red;
    public Color submitRightColor = Color.green;

    private List<int> input = new List<int>();
    private List<scr_NumberPillar> selectedPillars = new List<scr_NumberPillar>();
    private bool locked = false;

    void Start()
    {
        if (submitRenderer != null)
            submitRenderer.material.color = submitIdleColor;
    }

    public void AddNumber(int number, scr_NumberPillar pillar)
    {
        if (locked) return;

        // optional: prevent selecting the same pillar twice in one attempt
        if (pillar != null && pillar.IsSelected) return;

        input.Add(number);
        if (pillar != null)
        {
            pillar.SetSelected(true);
            selectedPillars.Add(pillar);
        }
    }

    public void Submit()
    {
        if (locked) return;

        bool correct = IsCorrect();

        if (correct)
        {
            StartCoroutine(CorrectRoutine());
        }
        else
        {
            StartCoroutine(WrongRoutine());
        }
    }

    private bool IsCorrect()
    {
        if (input.Count != correctCode.Length) return false;

        for (int i = 0; i < correctCode.Length; i++)
        {
            if (input[i] != correctCode[i])
                return false;
        }
        return true;
    }

    private IEnumerator WrongRoutine()
    {
        locked = true;

        if (submitRenderer != null)
            submitRenderer.material.color = submitWrongColor;

        yield return new WaitForSeconds(3f);

        // reset
        input.Clear();

        foreach (var p in selectedPillars)
            if (p != null) p.SetSelected(false);

        selectedPillars.Clear();

        if (submitRenderer != null)
            submitRenderer.material.color = submitIdleColor;

        locked = false;
    }

    private IEnumerator CorrectRoutine()
    {
        locked = true;

        if (submitRenderer != null)
            submitRenderer.material.color = submitRightColor;

        if (gateToDisable != null)
            gateToDisable.SetActive(false);

        yield return null;
    }
}
