using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ObjectShake : MonoBehaviour
{
    [Header("Shake data")]
    [SerializeField] private bool startRightDireccion = true;
    [SerializeField] private float timePerShake = 0;
    [SerializeField] private float duration = 0;
    [SerializeField] private float rangeX = 0;

    [Header("Events")]
    [SerializeField] private UnityEvent OnFinishShake = null;

    private Vector3 originalPosition = Vector3.zero;
    private bool rightXShake = false;
    private float timeForShake = 0;
    private float timeForDuration = 0;
    private ObstacleDisappearPlatform obstacleDisappearPlatform = null;

    /// <summary>
    /// Assign private parameters
    /// </summary>
    private void Awake()
    {
        obstacleDisappearPlatform = GetComponent<ObstacleDisappearPlatform>();
        if (startRightDireccion) rightXShake = true;
        originalPosition = transform.position;
    }

    /// <summary>
    /// Update shake movement
    /// </summary>
    private void Update()
    {
        if (obstacleDisappearPlatform)
        {
            if (obstacleDisappearPlatform.Shaking)
            {
                timeForDuration += Time.deltaTime;
                Shake();
            }
        }
    }

    /// <summary>
    /// Shake function
    /// </summary>
    public void Shake()
    {
        if (timeForDuration < duration)
        {
            if (timeForShake > timePerShake)
            {
                float x = 0;

                if (rightXShake) x = transform.position.x + rangeX;
                else x = transform.position.x - rangeX;
                transform.position = new Vector3(x, transform.position.y, transform.position.z);

                rightXShake = !rightXShake;
                timeForShake = 0;
            }
            else
            {
                timeForShake += Time.deltaTime;
            }
        }
        else
        {
            transform.position = originalPosition;
            obstacleDisappearPlatform.Shaking = false;
            OnFinishShake?.Invoke();
            timeForDuration = 0;
        }
    }
}