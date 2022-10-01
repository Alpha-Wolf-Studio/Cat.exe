using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    [SerializeField] private PlayerController playerController = null;
    [SerializeField] private float timerDelay = 0f;

    [SerializeField] private UIGameplay uiGameplay = null;

    private Timer timer = null;

    private void Start()
    {
        timer = new Timer(timerDelay, Timer.MODE.ONCE, false, uiGameplay.UpdateTimerText, EndTimer);

        //Llamar esta funcion para empezar el timer
        timer.ToggleTimer(true);
    }

    private void Update()
    {
        timer.Update(Time.deltaTime);
    }

    private void ResetTimer()
    {
        //resetear el tiempo cuando llega al checkpoint o aumentarlo?
        timer.SetTimer(timerDelay);
    }

    private void EndTimer()
    {
        playerController.Kill();
        timer.ToggleTimer(false);

        //añadir un delay para respawnear al jugar y volver a iniciar el timer
    }
}
