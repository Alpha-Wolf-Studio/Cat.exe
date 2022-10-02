using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameplayAudioControl : MonoBehaviour
{

    [Header("SFX")] 
    [SerializeField] private AudioClip timerRunningOutClip = default;
    [SerializeField] private AudioClip respawnAudioClip = default;
    [SerializeField] private AudioClip[] playerStartClips = default;
    [SerializeField] private AudioClip[] playerReachCheckPointClips = default;
    
    [Header("Music")]
    [SerializeField] private AudioClip gameplayMusic = default;
    [SerializeField] private AudioClip gameplayEndMusic = default;
    
    [Header("References")]
    [SerializeField] private GameplayManager gameplayManager;

    private void Start()
    {
        AudioManager.Get().PlayMusic(gameplayMusic);
        
        gameplayManager.OnPlayerStart += delegate
        {
            int index = Random.Range(0, playerStartClips.Length);
            AudioManager.Get().PlaySfx(playerStartClips[index]);
        };
        
        gameplayManager.OnPlayerReachedCheckPoint += delegate
        {
            int index = Random.Range(0, playerReachCheckPointClips.Length);
            AudioManager.Get().PlaySfx(playerReachCheckPointClips[index]);
        };
        
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
