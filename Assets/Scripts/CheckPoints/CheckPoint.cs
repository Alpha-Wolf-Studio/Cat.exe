using System;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    public event Action OnEnterCheckPoint;
    public event Action<Vector3, Vector3> OnSaveCheckPoint;
    public event Action OnExitCheckPoint;

    [SerializeField] private Collider colliderTrigger;
    [SerializeField] private Collider wall;
    [SerializeField] private Transform spawnPoint;

    public bool wasActivated;

    public int ID { get; set; } = 0;
    
    
    private void OnTriggerEnter (Collider other)
    {
        if (wasActivated)
            return;

        if (other.CompareTag("Player"))
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