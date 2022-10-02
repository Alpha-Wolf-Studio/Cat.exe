using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerAudioController : MonoBehaviour
{

    [System.Serializable]
    public class ClipConfiguration
    {
        [SerializeField] private List<AudioClip> audioClips;
        [SerializeField] private ReproductionType ConfigType = default;
        public enum ReproductionType
        {
            OnlyFirst,
            Sequential,
            Random,
            RandomNotRepeated
        }

        private int lastSelectedIndex = 0;
        private List<int> randomIntList = new List<int>();

        public AudioClip GetAnAudioClip()
        {
            if (audioClips.Count == 0) return null;
            
            switch (ConfigType)
            {
                case ReproductionType.OnlyFirst:
                    return audioClips[0];
                
                case ReproductionType.Random:
                    int randomIndex = Random.Range(0, audioClips.Count);
                    return audioClips[randomIndex];
                
                case ReproductionType.Sequential:
                    lastSelectedIndex++;
                    if (lastSelectedIndex >= audioClips.Count) lastSelectedIndex = 0;
                    return audioClips[lastSelectedIndex];
                
                case ReproductionType.RandomNotRepeated:
                    if (randomIntList.Count == 0)
                    {
                        for (int i = 0; i < audioClips.Count; i++)
                        {
                            randomIntList.Add(i);
                        }
                        ShuffleList(randomIntList);
                    }

                    int index = randomIntList[0];
                    randomIntList.RemoveAt(0);
                    return audioClips[index];

                default:
                    return audioClips[0];
            }

        }
        
        private void ShuffleList<T>(List<T> list)
        {
            var count = list.Count;
            var last = count - 1;
            for (var i = 0; i < last; ++i)
            {
                var r = Random.Range(i, count);
                (list[i], list[r]) = (list[r], list[i]);
            }
        }

    }


    [SerializeField] private ClipConfiguration stepsConfigurations = default;
    [SerializeField] private ClipConfiguration dieConfigurations = default;
    [SerializeField] private ClipConfiguration dashConfigurations = default;
    [SerializeField] private ClipConfiguration jumpConfigurations = default;

    [Header("References")] 
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private PlayerController playerController;


    private void Start()
    {
        playerController.OnDeath += delegate
        {
            AudioClip clip = dieConfigurations.GetAnAudioClip();
            if(clip) audioSource.PlayOneShot(clip);
        };
    }

    public void PlayDashSound()
    {
        AudioClip clip = dashConfigurations.GetAnAudioClip();
        if(clip) audioSource.PlayOneShot(clip);
    }
    
    public void PlayJumpSound()
    {
        AudioClip clip = jumpConfigurations.GetAnAudioClip();
        if(clip) audioSource.PlayOneShot(clip);
    }
    
    public void PlayStepSound()
    {
        AudioClip clip = stepsConfigurations.GetAnAudioClip();
        if(clip) audioSource.PlayOneShot(clip);
    }
    
}
