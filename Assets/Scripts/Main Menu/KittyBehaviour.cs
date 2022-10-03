using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

//Consideraciones:
//-Empezar el panel apagado.
//-Empezar el mensaje apagado.
//--Se activara primero el panel.
//--Se activara segundo el mensaje.

public class KittyBehaviour : MonoBehaviour
{
    #region VARIABLES
    #region SERIALIZED VARIABLES
    [SerializeField] private CanvasGroup kittyPanel = null;
    [SerializeField] private CanvasGroup messageText = null;
    [SerializeField] private int kittySpawnTime = 5;
    [SerializeField] private int kittySpawnMessage = 5;
    #endregion

    #region STATIC VARIABLES

    #endregion

    #region PROTECTED VARIABLES

    #endregion

    #region PRIVATE VARIABLES
    private float counter = 0;
    private bool kittyShowed = false;
    #endregion
    #endregion

    #region METHODS
    #region PUBLIC METHODS

    #endregion

    #region STATIC METHODS

    #endregion

    #region PROTECTED METHODS

    #endregion

    #region PRIVATE METHODS
    private void Update()
    {
        counter += Time.deltaTime;
        if(!kittyShowed && counter>kittySpawnTime)
        {
            StartCoroutine(MakePanelVisible());
        }
        if(counter>kittySpawnMessage+kittySpawnTime)
        {
            StartCoroutine(MakeMessageVisible());
        }
    }
    private void DisableThis()
    {
        gameObject.GetComponent<KittyBehaviour>().enabled = false;
    }
    IEnumerator MakePanelVisible()
    {
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime;
            kittyPanel.alpha = t;
            yield return null;
        }
        kittyPanel.blocksRaycasts = true;
        kittyPanel.interactable = true;
    }
    IEnumerator MakeMessageVisible()
    {
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime;
            messageText.alpha = t;
            yield return null;
        }
        messageText.blocksRaycasts = true;
        messageText.interactable = true;
        DisableThis();
    }
    #endregion
    #endregion
}