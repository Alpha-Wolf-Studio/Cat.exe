using System;
using System.Collections;
using UnityEngine;

public class GameplayManager : MonoBehaviourSingleton<GameplayManager>
{
    [Header("Gameplay")]
    public LayerMask layerPlayer;
    public PlayerController playerController;
    public CameraController cameraController;
    [SerializeField] private CheckPointManager checkPointManager;
    [SerializeField] private EndPoint endPoint;
    [Header("User Interface")]
    [SerializeField] private UIGameplay uiGameplay;
    
    private readonly float timerDelay = 10f;
    private readonly float timeToRespawn = 2;
    private bool isTimeStoped = false;
    private Timer timer;
    private float globalTime = 0;
    private void Start ()
    {
        playerController.OnDeath += KillPlayer;
        if(endPoint) endPoint.OnPlayerReachedTheEnd += OnPlayerWon;
        checkPointManager.SetCheckPointCallbacks(EnterCheckPoint, StartTime);
        
        timer = new Timer(timerDelay, Timer.MODE.ONCE, false, uiGameplay.UpdateTimerText, EndTimer);
        StartTime();
    }

    private void OnDestroy()
    {
        playerController.OnDeath -= KillPlayer;
        if(endPoint) endPoint.OnPlayerReachedTheEnd -= OnPlayerWon;
    }

    private void Update()
    {
        if (isTimeStoped) return;
        timer.Update(Time.deltaTime);
        globalTime += Time.deltaTime;
    }

    private void EnterCheckPoint ()
    {
        StopTime();
        ResetTime();
        SetCameraRotation(); //Rotar camara
    }

    private void SetCameraRotation ()
    {
        CheckPointManager.CurrentLastTransform(out var pos, out var rot);
        cameraController.Rotate(rot);
    }

    private void EndTimer ()
    {
        KillPlayer();
    }

    public void KillPlayer ()
    {
        StopTime();
        StartCoroutine(ReSpawningPlayer());
    }

    private IEnumerator ReSpawningPlayer ()
    {
        float time = 0;
        while (time < timeToRespawn)
        {
            time += Time.deltaTime;

            if (Input.GetKeyDown(KeyCode.R))
                break;

            yield return null;
        }

        timer.Reset();
        playerController.Respawn();
    }

    private void StartTime()
    {
        timer.ToggleTimer(true);
        isTimeStoped = false;
    }

    private void StopTime()
    {
        timer.ToggleTimer(false);
        isTimeStoped = true;
        CheckPointManager.lastCheckPoint.ResetCheckPoint();
    }

    private void ResetTime()
    {
        timer.SetTimer(timerDelay, false);
    }
    
    private void OnPlayerWon()
    {
        StopTime();
        Debug.Log(globalTime);
    }
    
}