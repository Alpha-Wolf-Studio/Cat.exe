using UnityEngine;
using UnityEngine.Events;

public class ObstacleHackDoor : MonoBehaviour
{

    [SerializeField] private UnityEvent OnOpenDoor = default;
    [SerializeField] private UnityEvent OnCloseDoor = default;
    
    [Header("Hack door")]
    [SerializeField] private bool startOpen = false;
    [SerializeField] private float timePerOpenDoor = 0;
    [SerializeField] private float openDoorDuration = 0;
    private Animator animator = null;
    private Timer timePerOpenDoorTimer = null;
    private Timer openDoorTimer = null;
    
    private ChildrenCollision[] childrenCollision = default;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        
        childrenCollision = GetComponentsInChildren<ChildrenCollision>();
        for (int i = 0; i < childrenCollision.Length; i++)
        {
            childrenCollision[i].OnHit += CheckIsPlayer;
        }
        
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
        OnOpenDoor?.Invoke();
    }

    private void CloseDoor()
    {
        animator.Play("Close");
        timePerOpenDoorTimer.Reset();
        timePerOpenDoorTimer.ToggleTimer(true);
        OnCloseDoor?.Invoke();
    }
    
    public void CheckIsPlayer (Transform other)
    {
        if (Utils.CheckLayerInMask(GameplayManager.Get().layerPlayer, other.gameObject.layer))
        {
            IDamageable damageable = other.transform.GetComponent<IDamageable>();
            if (damageable != null)
                Kill(damageable);
        }
    }

    public void Kill (IDamageable damageable)
    {
        damageable.Kill();
    }
    
}
