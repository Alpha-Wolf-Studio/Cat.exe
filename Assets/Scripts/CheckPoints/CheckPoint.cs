using System;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    public event Action OnEnterCheckPoint;
    public event Action<CheckPoint> OnSaveCheckPoint;
    public event Action OnExitCheckPoint;

    [SerializeField] private Collider colliderTrigger;
    [SerializeField] private Collider wall;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private LayerMask playerMask;

    public bool wasActivated = false;

    public int ID { get; set; } = 0;

    private void OnTriggerEnter (Collider other)
    {
        if (wasActivated)
            return;

        if (Utils.CheckLayerInMask(playerMask, other.gameObject.layer))
        {
            wasActivated = true;
            wall.enabled = true;
            OnSaveCheckPoint?.Invoke(this);
            OnEnterCheckPoint?.Invoke();
        }
    }

    private void OnTriggerExit (Collider other)
    {
        if (Utils.CheckLayerInMask(playerMask, other.gameObject.layer))
        {
            colliderTrigger.enabled = false;
            OnExitCheckPoint?.Invoke();
        }
    }

    public void ResetCheckPoint ()
    {
        colliderTrigger.enabled = true;
    }

    public Vector3 GetPositionSpawn () => spawnPoint.position;
    public Vector3 GetRotationSpawn () => spawnPoint.rotation.eulerAngles;
}