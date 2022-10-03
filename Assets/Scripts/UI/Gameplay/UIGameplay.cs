using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIGameplay : MonoBehaviour
{
    [Header("Gameplay")]
    [SerializeField] private GameObject panelGameplay;
    [SerializeField] private TMP_Text textTimer;
    [SerializeField] private TMP_Text textTimerTotal;
    [SerializeField] private Button btnExit;
    
    [Header("End Game Panel")] 
    [SerializeField] private UIPanelEndGame uiPanelEndGame;
    [SerializeField] private Slider sliderSoundMusic;
    [SerializeField] private Slider sliderSoundSfx;

    private void Start()
    {
        SceneManagerSingleton.Get().ChangeZoomState(true);
        sliderSoundMusic.onValueChanged.AddListener(ChangeMusicVolume);
        sliderSoundSfx.onValueChanged.AddListener(ChangeSfxVolume);
        btnExit.onClick.AddListener(ButtonExit);
    }

    private void ButtonExit () => ExitGame();

    private void Update ()
    {
        UpdateTimerText();
    }

    public void UpdateTimerText ()
    {
        int seconds = (int) GameplayManager.Get().GlobalTime % 60;
        int minutes = (int)GameplayManager.Get().GlobalTime / 60;

        string secondsTxt = seconds < 10 ? "0" + seconds : seconds.ToString();
        string minutesTxt = minutes < 10 ? "0" + minutes : minutes.ToString();

        textTimerTotal.text = minutesTxt + ":" + secondsTxt;
    }

    private void ChangeSfxVolume (float newValue) => AudioManager.Get().SetMusicVolume(newValue);
    private void ChangeMusicVolume (float newValue) => AudioManager.Get().SetMusicVolume(newValue);

    public void UpdateTimerText(float timerValue)
    {
        int seconds = (int)timerValue % 60;
        int minutes = (int)timerValue / 60;

        string secondsTxt = seconds < 10 ? "0" + seconds : seconds.ToString();
        string minutesTxt = minutes < 10 ? "0" + minutes : minutes.ToString();

        textTimer.text = minutesTxt + ":" + secondsTxt;
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
        SceneManagerSingleton.Get().ChangeZoomState(false);
        SceneManagerSingleton.Get().LoadScene(SceneManagerSingleton.SceneIndex.MAIN_MENU);
    }
}