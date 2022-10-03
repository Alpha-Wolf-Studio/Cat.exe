using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ObstacleDisappearPlatform : MonoBehaviour
{
    [SerializeField] private UnityEvent OnStartShake = null;

    [Header("Platform material")] [SerializeField]
    private Material obstacleBaseMaterial = null;

    [SerializeField] private Material obstacleEmissiveMaterial = null;
    [SerializeField] private float timeForEffect = 1;

    [Header("Models")] [SerializeField] private int totalModels = 0;
    [SerializeField] private MeshRenderer[] meshRenderers = null;
    [SerializeField] private Collider[] colliders = null;

    private FloatLerper disolveLerper = new FloatLerper();
    private ChildrenCollision[] childrenCollision;
    private bool shaking = false;

    /// Properties
    public bool Shaking
    {
        get => shaking;
        set => shaking = value;
    }

    private void Awake ()
    {
        foreach (var renderer in meshRenderers)
        {
            renderer.material.SetFloat("_Cutoff", 0);
        }
        disolveLerper.SetLerperValues(0, 1, timeForEffect, Lerper<float>.LERPER_TYPE.STEP_SMOOTH);

        childrenCollision = GetComponentsInChildren<ChildrenCollision>();
        for (int i = 0; i < childrenCollision.Length; i++)
        {
            childrenCollision[i].OnHit += CheckIsPlayer;
        }
    }

    private void Update ()
    {
        UpdateDisappearPlatform();
    }

    public void ActiveDisappearPlatform ()
    {
        disolveLerper.ActiveLerper();
    }

    public void UpdateDisappearPlatform ()
    {
        if (disolveLerper.Active)
        {
            disolveLerper.UpdateLerper();
            foreach (var renderer in meshRenderers)
            {
                renderer.material.SetFloat("_Cutoff", disolveLerper.GetValue());
            }

            if (disolveLerper.GetValue() > 0.5f)
            {
                for (int i = 0; i < totalModels; i++)
                    colliders[i].enabled = false;
            }
        }

        if (disolveLerper.Reached)
        {
            for (int i = 0; i < totalModels; i++)
                meshRenderers[i].enabled = false;
        }
    }


    private void OnCollisionEnter (Collision other)
    {
        CheckIsPlayer(other.transform);
    }

    public void CheckIsPlayer (Transform other)
    {
        if (Utils.CheckLayerInMask(GameplayManager.Get().layerPlayer, other.gameObject.layer))
        {
            shaking = true;
            OnStartShake?.Invoke();
            Invoke(nameof(RestartPlatform), 3f);
        }
    }

    void RestartPlatform ()
    {
        foreach (var renderer in meshRenderers)
        {
            renderer.material.SetFloat("_Cutoff", 0);
        }
        disolveLerper.SetLerperValues(0, 1, timeForEffect, Lerper<float>.LERPER_TYPE.STEP_SMOOTH);

        for (int i = 0; i < totalModels; i++)
            colliders[i].enabled = true;

        for (int i = 0; i < totalModels; i++)
            meshRenderers[i].enabled = true;
    }
}