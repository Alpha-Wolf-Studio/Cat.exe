using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleDisappearPlatform : MonoBehaviour
{
    [Header("Platform material")]
    [SerializeField] private Material dissolve = null;
    [SerializeField] private float timeForEffect = 1;

    private MeshRenderer meshRenderer = null;
    private BoxCollider boxCollider = null;
    private FloatLerper disolveLerper = new FloatLerper();
    private bool shaking = false;

    /// Properties
    public bool Shaking { get => shaking; set => shaking = value; }

    private void Awake()
    {
        dissolve.SetFloat("_Cutoff", 0);
        meshRenderer = GetComponent<MeshRenderer>();
        boxCollider = GetComponent<BoxCollider>();
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
            dissolve.SetFloat("_Cutoff", disolveLerper.GetValue());
        }
        if (disolveLerper.Reached)
        {
            meshRenderer.enabled = false;
            boxCollider.enabled = false;
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
        }
    }
}