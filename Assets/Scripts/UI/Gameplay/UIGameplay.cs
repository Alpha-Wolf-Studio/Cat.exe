using System;
using UnityEngine;
using TMPro;

public class UIGameplay : MonoBehaviour
{
    [Header("Gameplay")]
    [SerializeField] private GameObject panelGameplay = default;
    [SerializeField] private TMP_Text timerText = null;

    [Header("End Game Panel")] 
    [SerializeField] private UIPanelEndGame uiPanelEndGame = default;

    private void Start()
    {
        SceneManagerSingleton.Get().ChangeZoomState(true);
    }

    public void UpdateTimerText(float timerValue)
    {
        int seconds = (int)timerValue % 60;
        int minutes = (int)timerValue / 60;

        string secondsTxt = seconds < 10 ? "0" + seconds : seconds.ToString();
        string minutesTxt = minutes < 10 ? "0" + minutes : minutes.ToString();

        timerText.text = minutesTxt + ":" + secondsTxt;
    }

    public void PlayerFinished(float time)
    {
        panelGameplay.SetActive(false);
        uiPanelEndGame.SetPanel(time);
        uiPanelEndGame.OnPanelExit += ExitGame;
    }

    public void ExitGame()
    {
        uiPanelEndGame.OnPanelExit -= ExitGame;
        SceneManagerSingleton.Get().LoadScene(SceneManagerSingleton.SceneIndex.MAIN_MENU);
    }
    
}
