using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;

public class UiPanelOptions : MonoBehaviour
{
    [SerializeField] private UiButtonEffect btnReset;
    [SerializeField] private UiButtonEffect btnShutDown;
    [SerializeField] private UiButtonEffect btnSoundMusic;
    [SerializeField] private UiButtonEffect btnSoundSfx;
    [SerializeField] private UiButtonEffect panelSoundMusic;
    [SerializeField] private UiButtonEffect panelSoundSfx;

    [SerializeField] private Slider sliderSoundMusic;
    [SerializeField] private Slider sliderSoundSfx;
    [SerializeField] private PlayableDirector restartPlayableDirector;
    [SerializeField] private CanvasGroup shutDownPanel = default;

    private void Start ()
    {
        btnReset.AddBehaviours(Restart, CloseBothSoundPanel);
        btnShutDown.AddBehaviours(ShutDown, CloseBothSoundPanel);

        btnSoundMusic.AddBehaviours(null, OpenPanelSoundMusic);
        btnSoundMusic.AddBehaviours(null, ClosePanelSoundSfx);

        btnSoundSfx.AddBehaviours(null, OpenPanelSoundSfx);
        btnSoundSfx.AddBehaviours(null, ClosePanelSoundMusic);

        var sfxString = AudioManager.Get().VOLUME_SFX_KEYNAME;
        if (PlayerPrefs.HasKey(sfxString))
        {
            sliderSoundSfx.value = PlayerPrefs.GetFloat(sfxString);
        }
        
        var volumeString = AudioManager.Get().VOLUME_MUSIC_KEYNAME;
        if (PlayerPrefs.HasKey(volumeString))
        {
            sliderSoundMusic.value = PlayerPrefs.GetFloat(volumeString);
        }
        
        sliderSoundMusic.onValueChanged.AddListener(ChangeVolumeMusic);
        sliderSoundSfx.onValueChanged.AddListener(ChangeVolumeEffect);
    }

    private void OnEnable ()
    {
        ClosePanelSoundMusic();
        ClosePanelSoundSfx();
    }

    private void OnDestroy ()
    {
        RemoveAllListeners();
    }

    private void RemoveAllListeners ()
    {
        sliderSoundMusic.onValueChanged.RemoveAllListeners();
        sliderSoundSfx.onValueChanged.RemoveAllListeners();
    }

    private void Restart()
    {
        AudioManager.Get().StopMusic();
        restartPlayableDirector.Play();
    }

    private void ChangeVolumeEffect (float newValue)
    {
        AudioManager.Get().SetSFXVolume(newValue);
        AudioManager.Get().PlaySoundSfxDefault();
    }

    private void ChangeVolumeMusic (float newValue) => AudioManager.Get().SetMusicVolume(newValue);

    private void OpenPanelSoundMusic () => panelSoundMusic.gameObject.SetActive(true);
    private void OpenPanelSoundSfx () => panelSoundSfx.gameObject.SetActive(true);

    private void ClosePanelSoundMusic() => panelSoundMusic.gameObject.SetActive(false);
    private void ClosePanelSoundSfx() => panelSoundSfx.gameObject.SetActive(false);
    public void CloseBothSoundPanel ()
    {
        ClosePanelSoundMusic();
        ClosePanelSoundSfx();
    }
    
    private void ShutDown() => StartCoroutine(ShutDownCoroutine());

    private IEnumerator ShutDownCoroutine()
    {
        
        AudioManager.Get().StopMusic();
        
        float t = 0;
        shutDownPanel.interactable = true;
        shutDownPanel.blocksRaycasts = true;
        while (t < 1)
        {
            t += Time.deltaTime;
            shutDownPanel.alpha = t;
            yield return null;
        }
        shutDownPanel.alpha = 1;
    }


}