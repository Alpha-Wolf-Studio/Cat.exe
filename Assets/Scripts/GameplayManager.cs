using UnityEngine;

public class GameplayManager : MonoBehaviourSingleton<GameplayManager>
{
    public PlayerController playerController;
    public CameraController cameraController;
    [SerializeField] private CheckPointManager checkPointManager;
    [SerializeField] private UIGameplay uiGameplay;

    private readonly float timerDelay = 10f;

    private Timer timer;

    private void Start()
    {
        timer = new Timer(timerDelay, Timer.MODE.ONCE, false, uiGameplay.UpdateTimerText, EndTimer);

        //Llamar esta funcion para empezar el timer
        timer.ToggleTimer(true);

        checkPointManager.SetEnterCheckPointCallback(EnterCheckPoint);
    }

    private void Update()
    {
        timer.Update(Time.deltaTime);
    }

    private void EnterCheckPoint()
    {
        //resetear el tiempo cuando llega al checkpoint o aumentarlo?
        timer.SetTimer(timerDelay);
        SetCameraRotation(); //Rotar camara
    }

    private void SetCameraRotation ()
    {
        CheckPointManager.CurrentLastTransform(out var pos, out var rot);
        cameraController.Rotate(rot);
    }

    private void EndTimer()
    {
        playerController.Kill();
        timer.ToggleTimer(false);

        //añadir un delay para respawnear al jugar y volver a iniciar el timer
    }
}
