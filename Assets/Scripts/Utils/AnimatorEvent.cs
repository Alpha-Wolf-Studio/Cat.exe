using UnityEngine;
using UnityEngine.Events;

public class AnimatorEvent : MonoBehaviour
{
    [SerializeField] private UnityEvent callback = null;

    public void Callback()
    {
        callback?.Invoke();
    }
}
