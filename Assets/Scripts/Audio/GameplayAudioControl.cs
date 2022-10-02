using System;
using UnityEngine;

public class GameplayAudioControl : MonoBehaviour
{

    [Header("SFX")] 
    [SerializeField] private AudioClip timerRunningOutClip = default;
    [SerializeField] private AudioClip respawnAudioClip = default;
    
    [Header("Music")]
    [SerializeField] private AudioClip gameplayMusic = default;
    [SerializeField] private AudioClip gameplayEndMusic = default;
    
    [Header("References")]
    [SerializeField] private GameplayManager gameplayManager;

    private void Start()
    {
        AudioManager.Get().PlayMusic(gameplayMusic);
        
        gameplayManager.OnPlayerRespawn += delegate
        {
            AudioManager.Get().PlaySfx(respawnAudioClip);
        };
        
        gameplayManager.OnPlayerWon += delegate
        {
            AudioManager.Get().PlayMusic(gameplayEndMusic);   
        };
        
        gameplayManager.OnPlayerTimeAlmostEnd += delegate
        {
            AudioManager.Get().PlaySfx(timerRunningOutClip);
        };
    }
}
