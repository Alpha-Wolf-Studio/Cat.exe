using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class AnimatorEvent : MonoBehaviour
{
    [SerializeField] private UnityEvent OnDashStart = null;
    [SerializeField] private UnityEvent OnDashEnd = null;
    private void DashStart() => OnDashStart?.Invoke();
    private void DashEnd() => OnDashEnd?.Invoke();
}
