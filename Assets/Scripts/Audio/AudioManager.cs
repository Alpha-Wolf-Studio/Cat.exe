using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviourSingleton<AudioManager>
{
    [Header("Audio data")]
    [SerializeField] private AudioSource[] audioSources;
    [SerializeField] private AudioMixer[] audioMixers;

    //private const string VolumeKeyName = "Volume";
    public readonly string VOLUME_SFX_KEYNAME = "Volume_SFX";
    public readonly string VOLUME_MUSIC_KEYNAME = "Volume_Music";
    private const float LinearToDecibelCoefficient = 20f;
    private const float MinLinearValue = 0.00001f;
    private const float MaxLinearValue = 1f;

    [SerializeField] private AudioClip sfxDefault;
    [SerializeField] private AudioClip musicMainMenuIntro;
    [SerializeField] private AudioClip musicMainMenu;
    [SerializeField] private AudioClip musicGameplay;

    private AudioSource SfxSource => audioSources[(int)MixerType.Sfx];
    private AudioSource MusicSource => audioSources[(int)MixerType.Music];

    private void Start()
    {
        if (PlayerPrefs.HasKey(VOLUME_SFX_KEYNAME))
        {
            SetSFXVolume(PlayerPrefs.GetFloat(VOLUME_SFX_KEYNAME));
        }
        
        if (PlayerPrefs.HasKey(VOLUME_MUSIC_KEYNAME))
        {
            SetMusicVolume(PlayerPrefs.GetFloat(VOLUME_MUSIC_KEYNAME));
        }
    }

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
        SetVolume(volumeLevel, audioMixers[(int)MixerType.Sfx], VOLUME_SFX_KEYNAME);
        PlayerPrefs.SetFloat(VOLUME_SFX_KEYNAME, volumeLevel);
    }

    public void SetMusicVolume(float volumeLevel)
    {
        SetVolume(volumeLevel, audioMixers[(int)MixerType.Music], VOLUME_MUSIC_KEYNAME);
        PlayerPrefs.SetFloat(VOLUME_MUSIC_KEYNAME, volumeLevel);
    }

    private void SetVolume(float volumeLevel, AudioMixer mixer, string mixerKey)
    {
        volumeLevel = Mathf.Clamp(volumeLevel, MinLinearValue, MaxLinearValue);

        float desiredMixerDecibels = LinearToDecibel(volumeLevel);

        mixer.SetFloat(mixerKey, desiredMixerDecibels);
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