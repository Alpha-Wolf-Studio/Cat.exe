using System;
using UnityEngine;

public class ShadowUnderPlayer : MonoBehaviour
{
    private Transform target;
    private float startY;
    [SerializeField] private Light[] spotlights;
    [SerializeField] private float maxSpotAngle = 30;
    [SerializeField] private float multiplyFactorScale = 1.5f;

    void Start ()
    {
        target = GameplayManager.Get().playerController.transform;
        startY = target.position.y;
    }
    
    void Update ()
    {
        foreach (var spotlight in spotlights)
        {
            spotlight.spotAngle = (startY / (target.position.y * multiplyFactorScale)) * maxSpotAngle;
        }
    }
}