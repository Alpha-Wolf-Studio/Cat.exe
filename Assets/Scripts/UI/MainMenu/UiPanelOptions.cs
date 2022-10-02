using UnityEngine;
using UnityEngine.UI;

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

    private void Start ()
    {
        btnReset.AddBehaviours();
        btnShutDown.AddBehaviours();
        btnSoundMusic.AddBehaviours(null, OpenPanelSoundMusic);
        btnSoundSfx.AddBehaviours(null, OpenPanelSoundSfx);

        sliderSoundMusic.onValueChanged.AddListener(ChangeVolumeMusic);
        sliderSoundSfx.onValueChanged.AddListener(ChangeVolumeEffect);
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

    private void ChangeVolumeEffect (float newValue) => AudioManager.Get().SetSFXVolume(newValue);
    private void ChangeVolumeMusic (float newValue) => AudioManager.Get().SetMusicVolume(newValue);

    private void OpenPanelSoundMusic () => panelSoundMusic.gameObject.SetActive(true);
    private void OpenPanelSoundSfx () => panelSoundSfx.gameObject.SetActive(true);

    private void ClosePanelSoundMusic() => panelSoundMusic.gameObject.SetActive(false);
    private void ClosePanelSoundSfx() => panelSoundSfx.gameObject.SetActive(false);


}