using UnityEngine;
using UnityEngine.Events;

public class AnimatorEvent : MonoBehaviour
{
    [Header("Dash")]
    [SerializeField] private UnityEvent OnDashStart = null;
    [SerializeField] private UnityEvent OnDashEnd = null;
    [Header("Jump")]
    [SerializeField] private UnityEvent OnJumpStart = null;
    [SerializeField] private UnityEvent OnJumpEnd = null;
    [Header("Die")]
    [SerializeField] private UnityEvent OnDieStart = null;
    [SerializeField] private UnityEvent OnDieEnd = null;
    [Header("Steps")]
    [SerializeField] private UnityEvent OnStep = null;
    
    private void DashStart() => OnDashStart?.Invoke();
    private void DashEnd() => OnDashEnd?.Invoke();
    
    private void JumpStart() => OnJumpStart?.Invoke();
    private void JumpEnd() => OnJumpEnd?.Invoke();
    
    private void DieStart() => OnDieStart?.Invoke();
    private void DieEnd() => OnDieEnd?.Invoke();
    
    private void Step() => OnStep?.Invoke();
    
}
