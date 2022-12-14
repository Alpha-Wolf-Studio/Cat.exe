using UnityEngine;
using UnityEngine.Events;

public class ObstacleLaser : MonoBehaviour, IObstacle
{
    
    [SerializeField] private UnityEvent OnLaserOn;
    [SerializeField] private UnityEvent OnLaserOff;
    [SerializeField] private UnityEvent OnTwinkle;
    
    [Header("Laser")]
    [SerializeField] private GameObject laser = null;
    [SerializeField] private float timePerLaser = 0;
    [SerializeField] private bool firstLaserWithoutTimer = false;
    [SerializeField] private float maximumSize = 0;
    [SerializeField] private float growSpeed = 0;
    [SerializeField] private float laserDuration = 0;

    [Header("Twinkles")]
    [SerializeField] private float maximumTwinkles = 0;
    [SerializeField] private float twinklesDuration = 0;
    [SerializeField] private float timePerTwinkle = 0;

    /// Laser private parameter
    private const float initialSize = 1;
    private FloatLerper growLerper = new FloatLerper();
    private Timer timePerLaserTimer = null;
    private Timer laserDurationTimer = null;

    /// Twinkles private parameter
    private bool turnOffLaser = false;
    private MeshRenderer laserMeshRenderer = null;
    private Timer twinkleDurationTimer = null;
    private float timePerTwinkleTimer = 0;
    private int actualTwinkles = 0;

    private ChildrenCollision[] childrenCollision;
    
    private void Start()
    {
        timePerLaserTimer = new Timer(timePerLaser, default, true, null, TurnOnLaser);
        laserDurationTimer = new Timer(laserDuration, default, false, null, TurnOffLaser);
        if (firstLaserWithoutTimer) TurnOnLaser();

        laserMeshRenderer = laser.GetComponent<MeshRenderer>();
        twinkleDurationTimer = new Timer(twinklesDuration, default, false, null, NextTwinkle);
        
        childrenCollision = GetComponentsInChildren<ChildrenCollision>();
        for (int i = 0; i < childrenCollision.Length; i++)
        {
            childrenCollision[i].OnHit += CheckIsPlayer;
        }
        
    }

    private void Update()
    {
        timePerLaserTimer.Update(Time.deltaTime);
        UpdateGrowLerper();
        laserDurationTimer.Update(Time.deltaTime);

        UpdateTurnOffSecuence();
        twinkleDurationTimer.Update(Time.deltaTime);
    }

    private void TurnOnLaser()
    {
        growLerper.SetLerperValues(initialSize, maximumSize, growSpeed, Lerper<float>.LERPER_TYPE.STEP_SMOOTH, true);
        laserDurationTimer.Reset();
        laserDurationTimer.ToggleTimer(true);
        OnLaserOn?.Invoke();
    }

    private void TurnOffLaser()
    {
        growLerper.SetLerperValues(maximumSize, initialSize, growSpeed, Lerper<float>.LERPER_TYPE.STEP_SMOOTH);
        turnOffLaser = true;
        actualTwinkles = 0;
        OnLaserOff?.Invoke();
    }

    private void UpdateGrowLerper()
    {
        if (growLerper.Active)
        {
            growLerper.UpdateLerper();
            Vector3 newScale = new Vector3(growLerper.GetValue(), laser.transform.localScale.y, laser.transform.localScale.z);
            laser.transform.localScale = newScale;
        }
    }

    private void UpdateTurnOffSecuence()
    {
        if (turnOffLaser)
        {
            if (actualTwinkles < maximumTwinkles)
            {
                timePerTwinkleTimer += Time.deltaTime;

                if (timePerTwinkleTimer >= timePerTwinkle)
                {
                    laserMeshRenderer.enabled = false;
                    twinkleDurationTimer.ToggleTimer(true);
                    OnTwinkle?.Invoke();
                }
            }
            else
            {
                turnOffLaser = false;
                growLerper.ActiveLerper();
                timePerLaserTimer.Reset();
                timePerLaserTimer.ToggleTimer(true);
            }
        }
    }

    private void NextTwinkle()
    {
        laserMeshRenderer.enabled = true;
        twinkleDurationTimer.Reset();
        actualTwinkles++;

        if (actualTwinkles < maximumTwinkles) timePerTwinkleTimer = 0;
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