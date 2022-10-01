using System;
using UnityEngine;

public class ChildrenCollision : MonoBehaviour
{
    public event Action<Transform> OnHit;

    [SerializeField] private bool isTrigger;

    private void OnCollisionEnter (Collision other)
    {
        if (isTrigger)
            return;

        OnHit?.Invoke(other.transform);
    }

    private void OnTriggerEnter (Collider other)
    {
        if (!isTrigger)
            return;

        OnHit?.Invoke(other.transform);
    }
}