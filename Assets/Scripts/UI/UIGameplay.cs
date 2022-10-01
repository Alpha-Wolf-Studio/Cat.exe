using UnityEngine;
using TMPro;

public class UIGameplay : MonoBehaviour
{
    [SerializeField] private TMP_Text timerText = null;

    public void UpdateTimerText(float timerValue)
    {
        int seconds = (int)timerValue % 60;
        int minutes = (int)timerValue / 60;

        string secondsTxt = seconds < 10 ? "0" + seconds : seconds.ToString();
        string minutesTxt = minutes < 10 ? "0" + minutes : minutes.ToString();

        timerText.text = minutesTxt + ":" + secondsTxt;
    }
}
