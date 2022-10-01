using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleDisappearAppearPlatform : MonoBehaviour
{
    [Header("Disappear platform")]
    [SerializeField] private bool startDisappear = false;
    [SerializeField] private float timePerDisappear = 0;
    [SerializeField] private float disappearDuration = 0;

    private MeshRenderer meshRenderer = null;
    private BoxCollider boxCollider = null;
    private Timer timePerDisappearTimer = null;
    private Timer disappearTimer = null;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();    
        boxCollider = GetComponent<BoxCollider>();

        timePerDisappearTimer = new Timer(timePerDisappear, default, true, null, DisappearPlatform);
        disappearTimer = new Timer(disappearDuration, default, false, null, AppearPlatform);
        if (startDisappear) DisappearPlatform();
    }

    private void Update()
    {
        timePerDisappearTimer.Update(Time.deltaTime);
        disappearTimer.Update(Time.deltaTime);
    }

    private void DisappearPlatform()
    {
        meshRenderer.enabled = false;
        boxCollider.enabled = false;
        disappearTimer.Reset();
        disappearTimer.ToggleTimer(true);
    }

    private void AppearPlatform()
    {
        meshRenderer.enabled = true;
        boxCollider.enabled = true;
        timePerDisappearTimer.Reset();
        timePerDisappearTimer.ToggleTimer(true);
    }
}