using System;
using UnityEngine;

public class EndPoint : MonoBehaviour
{

    public Action OnPlayerReachedTheEnd;

    private void OnTriggerEnter(Collider other)
    {
        if (Utils.CheckLayerInMask(GameplayManager.Get().layerPlayer, other.gameObject.layer))
        {
            OnPlayerReachedTheEnd?.Invoke();
        }
    }
}
