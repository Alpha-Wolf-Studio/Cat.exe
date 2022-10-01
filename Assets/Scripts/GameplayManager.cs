using System.Collections;
using UnityEngine;

public class GameplayManager : MonoBehaviourSingleton<GameplayManager>
{
    public LayerMask layerPlayer;
    public PlayerController playerController;
    public CameraController cameraController;
    [SerializeField] private CheckPointManager checkPointManager;
    [SerializeField] private UIGameplay uiGameplay;

    private readonly float timerDelay = 10f;
    private readonly float timeToRespawn = 2;
    private Timer timer;
    
    private void Start ()
    {
        playerController.OnDeath += KillPlayer;

        timer = new Timer(timerDelay, Timer.MODE.ONCE, false, uiGameplay.UpdateTimerText, EndTimer);

        timer.ToggleTimer(true); //Llamar esta funcion para empezar el timer

        checkPointManager.SetCheckPointCallbacks(EnterCheckPoint, () => timer.ToggleTimer(true));
    }

    private void Update ()
    {
        timer.Update(Time.deltaTime);
    }

    private void EnterCheckPoint ()
    {
        //resetear el tiempo cuando llega al checkpoint o aumentarlo?
        timer.SetTimer(timerDelay, false);
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
        timer.ToggleTimer(false);
        CheckPointManager.lastCheckPoint.ResetCheckPoint();
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
}