using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private Tutorial[] triggers;
    [SerializeField] private CanvasGroup canvas;
    [SerializeField] private TMP_Text textTutorial;
    private IEnumerator showingTutorial;

    private float transitionTime = 1;

    private void Start ()
    {
        for (int i = 0; i < triggers.Length; i++)
        {
            triggers[i].OnEnterTutorial += ShowToturial;
        }
    }

    private void ShowToturial (string textTuto)
    {
        textTutorial.text = textTuto;

        if (showingTutorial != null)
            StopCoroutine(showingTutorial);
        showingTutorial = ShowingTutorial();
        StartCoroutine(showingTutorial);
    }

    IEnumerator ShowingTutorial ()
    {
        float onTime = 0;
        while (onTime < transitionTime)
        {
            onTime += Time.deltaTime;
            float fade = onTime / transitionTime;
            canvas.alpha = fade;
            yield return null;
        }

        yield return new WaitForSeconds(2);

        onTime = 0; 
        while (onTime < transitionTime)
        {
            onTime += Time.deltaTime;
            float fade = onTime / transitionTime;
            canvas.alpha = 1 - fade;
            yield return null;
        }
    }
}