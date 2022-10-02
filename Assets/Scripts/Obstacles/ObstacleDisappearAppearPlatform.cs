using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleDisappearAppearPlatform : MonoBehaviour
{
    [Header("Disappear platform")]
    [SerializeField] private bool startDisappear;
    [SerializeField] private float timePerDisappear;
    [SerializeField] private float disappearDuration;

    [Header("Models")]
    [SerializeField] private int totalModels;
    [SerializeField] private MeshRenderer[] meshRenderers;
    [SerializeField] private Collider[] colliders;

    private Timer timePerDisappearTimer;
    private Timer disappearTimer;

    private void Awake()
    {
        timePerDisappearTimer = new Timer(timePerDisappear, default, true, null, DisappearPlatform);
        disappearTimer = new Timer(disappearDuration, default, false, null, AppearPlatform);
        if (startDisappear) 
            DisappearPlatform();
    }

    private void Update()
    {
        timePerDisappearTimer.Update(Time.deltaTime);
        disappearTimer.Update(Time.deltaTime);
    }

    private void DisappearPlatform()
    {
        for (int i = 0; i < totalModels; i++)
        {
            meshRenderers[i].enabled = false;
            colliders[i].enabled = false;
        }

        disappearTimer.Reset();
        disappearTimer.ToggleTimer(true);
    }

    private void AppearPlatform()
    {
        for (int i = 0; i < totalModels; i++)
        {
            meshRenderers[i].enabled = true;
            colliders[i].enabled = true;
        }

        timePerDisappearTimer.Reset();
        timePerDisappearTimer.ToggleTimer(true);
    }
}