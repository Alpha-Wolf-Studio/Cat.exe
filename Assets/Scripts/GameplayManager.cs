using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    [SerializeField] private PlayerController playerController = null;
    [SerializeField] private CheckPointManager checkPointManager = null;
    [SerializeField] private UIGameplay uiGameplay = null;

    [SerializeField] private float timerDelay = 0f;

    private Timer timer = null;

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

        //Rotar camara
    }

    private void EndTimer()
    {
        playerController.Kill();
        timer.ToggleTimer(false);

        //añadir un delay para respawnear al jugar y volver a iniciar el timer
    }
}
