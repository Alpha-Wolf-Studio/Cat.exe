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

    private bool growing = false;
    private FloatLerper growLerper = new FloatLerper();

    private void Update()
    {
        UpdateGrowLerper();

        if (Input.GetKeyDown(KeyCode.N)) TurnOnLaser();
        if (Input.GetKeyDown(KeyCode.M)) TurnOffLaser();
    }

    private void TurnOnLaser()
    {
        growLerper.SetLerperValues(initialSize, maximumSize, growSpeed, Lerper<float>.LERPER_TYPE.STEP_SMOOTH, true);
        growing = true;
    }

    private void TurnOffLaser()
    {
        growLerper.SetLerperValues(maximumSize, initialSize, growSpeed, Lerper<float>.LERPER_TYPE.STEP_SMOOTH, true);
        growing = false;
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
