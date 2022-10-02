using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIPanelEndGame : MonoBehaviour
{

    public Action OnPanelExit;

    [Header("Time Panel")]
    [SerializeField] private TextMeshProUGUI timeTextComponent = default;

    [Header("Wait Panel")]
    [SerializeField] private GameObject waitPanel = default;
    [Header("Submit Time Panel")]
    [SerializeField] private GameObject timePanel = default;
    [SerializeField] private TMP_InputField nameInputField = default;
    [SerializeField] private Button submitTimeButton = default;
    [Header("Error Panel")]
    [SerializeField] private GameObject errorPanel = default;

    [SerializeField] private Button continueButton = default;

    private float timeOnEnd = 0;

    public void SetPanel(float time)
    {
        
        gameObject.SetActive(true);
        
        timeOnEnd = time;
        timeTextComponent.text = "Time: " + time;
        
        PlayfabManager.Get().Login();
        PlayfabManager.Get().OnConnection += OnConnection;
        
        waitPanel.SetActive(true);
    }
    
    private void OnDisable()
    {
        PlayfabManager.Get().OnConnection -= OnConnection;
    }

    private void OnConnection(bool isConnected)
    {
        
        waitPanel.SetActive(false);
        
        if (isConnected)
        {
            timePanel.SetActive(true);
            submitTimeButton.onClick.AddListener(delegate
            {
                string initials = nameInputField.text;
                PlayfabManager.Get().SubmitTimeScore(timeOnEnd, initials);
                OnPanelExit?.Invoke();
            });
        }
        else
        {
            errorPanel.SetActive(true);
            continueButton.onClick.AddListener(delegate
            {
                OnPanelExit?.Invoke();
            });
        }
    }
    
    
}
