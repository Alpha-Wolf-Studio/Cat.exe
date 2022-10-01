using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleDisappearPlatform : MonoBehaviour
{
    private bool shaking = false;

    public bool Shaking { get => shaking; set => shaking = value; }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M)) shaking = true;
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