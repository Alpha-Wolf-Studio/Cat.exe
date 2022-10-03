using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviourSingleton<AudioManager>
{
    [Header("Audio data")]
    [SerializeField] private AudioSource[] audioSources;
    [SerializeField] private AudioMixer[] audioMixers;

    private const string VolumeKeyName = "Volume";
    private const float LinearToDecibelCoefficient = 20f;
    private const float MinLinearValue = 0.00001f;
    private const float MaxLinearValue = 1f;

    [SerializeField] private AudioClip sfxDefault;
    [SerializeField] private AudioClip musicMainMenuIntro;
    [SerializeField] private AudioClip musicMainMenu;
    [SerializeField] private AudioClip musicGameplay;

    private AudioSource SfxSource => audioSources[(int)MixerType.Sfx];
    private AudioSource MusicSource => audioSources[(int)MixerType.Music];


    private float LinearToDecibel(float linearValue) => LinearToDecibelCoefficient * Mathf.Log10(linearValue);

    public void PlaySfx(AudioClip clip)
    {
        if(clip) SfxSource.PlayOneShot(clip);
    }

    public void PlayMusic(AudioClip clip)
    {
        if (!clip) return;

        MusicSource.clip = clip;
        MusicSource.Play();
    }

    public void SwitchPauseState()
    {
        if (MusicSource.isPlaying)
            MusicSource.Pause();
        else
            MusicSource.Play();
    }

    public void StopMusic()
    {
        if (MusicSource.isPlaying)
            MusicSource.Stop();
    }

    public void SetSFXVolume(float volumeLevel)
    {
        SetVolume(MixerType.Sfx, volumeLevel);
    }

    public void SetMusicVolume(float volumeLevel)
    {
        SetVolume(MixerType.Music, volumeLevel);
    }

    private void SetVolume(MixerType mixerType, float volumeLevel)
    {
        volumeLevel = Mathf.Clamp(volumeLevel, MinLinearValue, MaxLinearValue);

        float desiredMixerDecibels = LinearToDecibel(volumeLevel);

        audioMixers[(int)mixerType].SetFloat(VolumeKeyName, desiredMixerDecibels);
    }

    private IEnumerator FirstTimeMainMenuMusicIEnumerator()
    {
        MusicSource.loop = false;
        PlayMusic(musicMainMenuIntro);
        
        while (MusicSource.isPlaying)
        {
            yield return null;
        }

        MusicSource.loop = true;
        PlayMusic(musicMainMenu);
    }
    
    public void PlayMusicMainMenuFirstTime()
    {
        StartCoroutine(FirstTimeMainMenuMusicIEnumerator());
    }

    public void PlayMusicMainMenu () => PlayMusic(musicMainMenu);
    public void PlayMusicGameplay () => PlayMusic(musicGameplay);
    public void PlaySoundSfxDefault () => PlaySfx(sfxDefault);
}