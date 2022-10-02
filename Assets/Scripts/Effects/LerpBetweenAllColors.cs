using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LerpBetweenAllColors : MonoBehaviour
{
    [SerializeField] private Material material = null;
    [SerializeField] private string varName = string.Empty;
    [SerializeField] private float duration = 1;
    [SerializeField] private bool on = true;

    private void Start()
    {
        LerpBetweenColorsMain();
    }

    private void LerpBetweenColorsMain()
    {
        StartCoroutine(LerpBetweenColors(Color.red, Color.blue, 
            () => StartCoroutine(LerpBetweenColors(Color.blue, Color.yellow,
            () => StartCoroutine(LerpBetweenColors(Color.yellow, Color.red, 
            () => LerpBetweenColorsMain()))))));
    }

    private IEnumerator LerpBetweenColors(Color a, Color b, Action callback)
    {
        float time = 0;

        while (time < duration)
        {
            time += Time.deltaTime;
            material.SetColor(varName, Color.Lerp(a, b, time / duration));
            yield return null;  
        }

        callback?.Invoke();
    }

}
