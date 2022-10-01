using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleLaser : MonoBehaviour, IObstacle
{
    [Header("Laser data")]
    public GameObject laser = null;
    public float initialSize = 0;
    public float maximumSize = 0;
    public float growSpeed = 0;
    public float duration = 0;

    private FloatLerper growLerper = new FloatLerper();
    private Timer timer = null;

    private void Start()
    {
        timer = new Timer(duration, default, false, null, TurnOffLaser);
    }

    private void Update()
    {
        UpdateGrowLerper();
        UpdateDurationTimer();

        if (Input.GetKeyDown(KeyCode.N)) TurnOnLaser();
    }

    private void TurnOnLaser()
    {
        growLerper.SetLerperValues(initialSize, maximumSize, growSpeed, Lerper<float>.LERPER_TYPE.STEP_SMOOTH, true);
        timer.ToggleTimer(true);
    }

    private void TurnOffLaser()
    {
        growLerper.SetLerperValues(maximumSize, initialSize, growSpeed, Lerper<float>.LERPER_TYPE.STEP_SMOOTH, true);
    }

    private void UpdateGrowLerper()
    {
        if (growLerper.Active)
        {
            growLerper.UpdateLerper();
            
            /// Laser scale
            Vector3 newScale = new Vector3(laser.transform.localScale.x, growLerper.GetValue(), laser.transform.localScale.z);
            laser.transform.localScale = newScale;

            /// Laser position
            Vector3 updatePosition = new Vector3(growLerper.GetValue() - initialSize, laser.transform.localPosition.y, laser.transform.localPosition.z);
            laser.transform.localPosition = updatePosition;
        }
    }

    private void UpdateDurationTimer()
    {
        timer.Update(Time.deltaTime);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.CompareTag("Player"))
        {
            IDamageable damageable = other.transform.GetComponent<IDamageable>();
            if (damageable != null) Kill(damageable);
        }
    }

    public void Kill(IDamageable damageable)
    {
        damageable.Kill();
    }
}