using System.Collections.Generic;
using UnityEngine;

public class ObstacleAudio : MonoBehaviour
{
    [SerializeField] private List<AudioClip> audioClips;
    [SerializeField] private AudioSource audioSource = default;

    public void PlayOneShot(int index)
    {
        if (index >= audioClips.Count) return;
        audioSource.PlayOneShot(audioClips[index]);
    }
    
    public void PlayLoop(int index)
    {
        if (index >= audioClips.Count) return;
        audioSource.loop = true;
        audioSource.clip = audioClips[index];
    }
    
}
