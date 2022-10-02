using System;
using System.Collections;
using UnityEngine;

public class GameplayManager : MonoBehaviourSingleton<GameplayManager>
{

    public Action OnPlayerTimeAlmostEnd = default;
    public Action OnPlayerRespawn = default;
    public Action OnPlayerWon = default;

    [Header("Gameplay")]
    public LayerMask layerPlayer;
    public PlayerController playerController;
    public CameraController cameraController;
    [SerializeField] private CheckPointManager checkPointManager;
    [SerializeField] private EndPoint endPoint;
    [Header("User Interface")]
    [SerializeField] private UIGameplay uiGameplay;
    
    private readonly float timerDelay = 10f;
    private readonly float timerAlmostDelay = 8f;
    private readonly float timeToRespawn = 2;
    private bool isTimeStoped = false;
    private Timer timer;
    private Timer timerAlmost;
    private float globalTime = 0;
    private void Start ()
    {
        AudioManager.Get().PlayMusicGameplay();

        playerController.OnDeath += KillPlayer;
        if(endPoint) endPoint.OnPlayerReachedTheEnd += PlayerWon;
        checkPointManager.SetCheckPointCallbacks(EnterCheckPoint, StartTime);
        
        timer = new Timer(timerDelay, Timer.MODE.ONCE, false, uiGameplay.UpdateTimerText, EndTimer);
        timerAlmost = new Timer(timerAlmostDelay, Timer.MODE.ONCE, false, null, AlmostEndTimer);
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
        timerAlmost.Update(Time.deltaTime);
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

    private void EndTimer () => KillPlayer();

    private void AlmostEndTimer() => OnPlayerTimeAlmostEnd?.Invoke();

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
        timerAlmost.Reset();
        playerController.Respawn();
        OnPlayerRespawn?.Invoke();
    }

    private void StartTime()
    {
        timer.ToggleTimer(true);
        timerAlmost.ToggleTimer(true);
        isTimeStoped = false;
    }

    private void StopTime()
    {
        timer.ToggleTimer(false);
        timerAlmost.ToggleTimer(false);
        isTimeStoped = true;
        CheckPointManager.lastCheckPoint.ResetCheckPoint();
    }

    private void ResetTime()
    {
        timer.SetTimer(timerDelay, false);
        timerAlmost.SetTimer(timerAlmostDelay, false);
    }
    
    private void PlayerWon()
    {
        playerController.enabled = false;
        StopTime();
        uiGameplay.PlayerFinished(globalTime);
        OnPlayerWon?.Invoke();
    }
    
}