using System;
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

    private void Start ()
    {
        btnReset.AddBehaviours(Restart, CloseBothSoundPanel);
        btnShutDown.AddBehaviours(null, CloseBothSoundPanel);

        btnSoundMusic.AddBehaviours(null, OpenPanelSoundMusic);
        btnSoundMusic.AddBehaviours(null, ClosePanelSoundSfx);

        btnSoundSfx.AddBehaviours(null, OpenPanelSoundSfx);
        btnSoundSfx.AddBehaviours(null, ClosePanelSoundMusic);

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

    private void Restart() => restartPlayableDirector.Play();

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

}