using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleDisappearAppearPlatform : MonoBehaviour
{
    [Header("Disappear platform")]
    [SerializeField] private bool startDisappear = false;
    [SerializeField] private float timePerDisappear = 0;
    [SerializeField] private float disappearDuration = 0;

    [Header("Models")]
    [SerializeField] private int totalModels = 0;
    [SerializeField] private MeshRenderer[] meshRenderers = null;
    [SerializeField] private Collider[] colliders = null;

    private Timer timePerDisappearTimer = null;
    private Timer disappearTimer = null;

    private void Awake()
    {
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