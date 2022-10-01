using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleHackDoor : MonoBehaviour
{
    [Header("Hack door")]
    [SerializeField] private bool startOpen = false;
    [SerializeField] private float timePerOpenDoor = 0;
    [SerializeField] private float openDoorDuration = 0;

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
}
