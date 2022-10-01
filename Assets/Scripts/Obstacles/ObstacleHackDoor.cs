using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleHackDoor : MonoBehaviour, IObstacle
{
    [Header("Hack door")]
    public bool startOpen = false;
    public float timePerOpenDoor = 0;
    public float openDoorDuration = 0;

    private Animator animator = null;
    private Timer timePerOpenDoorTimer = null;
    private Timer openDoorTimer = null;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        timePerOpenDoorTimer = new Timer(timePerOpenDoor, default, true, null, OpenDoor);
        openDoorTimer = new Timer(openDoorDuration, default, false, null, CloseDoor);
        if (startOpen) OpenDoor();
    }

    private void Update()
    {
        timePerOpenDoorTimer.Update(Time.deltaTime);
        openDoorTimer.Update(Time.deltaTime);
    }

    private void OpenDoor()
    {
        animator.Play("Open");
        openDoorTimer.Reset();
        openDoorTimer.ToggleTimer(true);
    }

    private void CloseDoor()
    {
        animator.Play("Close");
        timePerOpenDoorTimer.Reset();
        timePerOpenDoorTimer.ToggleTimer(true);
    }



    private void OnCollisionEnter(Collision other)
    {
        CheckIsPlayer(other.transform);
    }

    public void CheckIsPlayer(Transform other)
    {
        if (Utils.CheckLayerInMask(GameplayManager.Get().layerPlayer, other.gameObject.layer))
        {
            IDamageable damageable = other.transform.GetComponent<IDamageable>();
            if (damageable != null)
                Kill(damageable);
        }
    }

    public void Kill(IDamageable damageable)
    {
        damageable.Kill();
    }
}
