using System;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    public event Action OnEnterCheckPoint = null;
    public event Action<Vector3, Vector3> OnSaveCheckPoint = null;
    public event Action OnExitCheckPoint = null;

    [SerializeField] private Collider colliderTrigger = null;
    [SerializeField] private Collider wall = null;
    [SerializeField] private Transform spawnPoint = null;
    [SerializeField] private LayerMask playerMask = default;

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
            OnSaveCheckPoint?.Invoke(spawnPoint.position, spawnPoint.rotation.eulerAngles);
            OnEnterCheckPoint?.Invoke();
        }
    }

    private void OnTriggerExit (Collider other)
    {
        if (other.CompareTag("Player"))
        {
            colliderTrigger.enabled = false;
            OnExitCheckPoint?.Invoke();
        }
    }
}