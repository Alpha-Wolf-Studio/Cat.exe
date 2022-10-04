using System;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public event Action<string> OnEnterTutorial;
    [SerializeField] private string textTutorial;
    private Collider collider;

    private void Start ()
    {
        collider = GetComponent<Collider>();
    }

    private void OnTriggerEnter (Collider other)
    {
        if (Utils.CheckLayerInMask(GameplayManager.Get().layerPlayer, other.gameObject.layer))
        {
            OnEnterTutorial?.Invoke(textTutorial);
            collider.enabled = false;
        }
    }

    private void OnCollisionEnter (Collision other)
    {
        if (Utils.CheckLayerInMask(GameplayManager.Get().layerPlayer, other.gameObject.layer))
        {
            OnEnterTutorial?.Invoke(textTutorial);
            collider.enabled = false;
        }
    }
}