using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ObstacleDisappearPlatform : MonoBehaviour
{

    [SerializeField] private UnityEvent OnStartShake = null;
    
    [Header("Platform material")]
    [SerializeField] private Material obstacleBaseMaterial = null;
    [SerializeField] private Material obstacleEmissiveMaterial = null;
    [SerializeField] private float timeForEffect = 1;

    [Header("Models")]
    [SerializeField] private int totalModels = 0;
    [SerializeField] private MeshRenderer[] meshRenderers = null;
    [SerializeField] private Collider[] colliders = null;

    private FloatLerper disolveLerper = new FloatLerper();
    private bool shaking = false;

    /// Properties
    public bool Shaking { get => shaking; set => shaking = value; }

    private void Awake()
    {
        obstacleBaseMaterial.SetFloat("_Cutoff", 0);
        obstacleEmissiveMaterial.SetFloat("_Cutoff", 0);
        disolveLerper.SetLerperValues(0, 1, timeForEffect, Lerper<float>.LERPER_TYPE.STEP_SMOOTH);
    }

    private void Update()
    {
        UpdateDisappearPlatform();
        //if (Input.GetKeyDown(KeyCode.M)) shaking = true;
    }

    public void ActiveDisappearPlatform()
    {
        disolveLerper.ActiveLerper();
    }

    public void UpdateDisappearPlatform()
    {
        if (disolveLerper.Active)
        {
            disolveLerper.UpdateLerper();
            obstacleBaseMaterial.SetFloat("_Cutoff", disolveLerper.GetValue());
            obstacleEmissiveMaterial.SetFloat("_Cutoff", disolveLerper.GetValue());
        }
        if (disolveLerper.Reached)
        {
            for (int i = 0; i < totalModels; i++)
            {
                meshRenderers[i].enabled = false;
                colliders[i].enabled = false;
            }
        }
    }


    private void OnCollisionEnter(Collision other)
    {
        CheckIsPlayer(other.transform);
    }

    public void CheckIsPlayer(Transform other)
    {
        if (Utils.CheckLayerInMask(GameplayManager.Get().layerPlayer, other.gameObject.layer))
        {
            shaking = true;
            OnStartShake?.Invoke();
        }
    }
}