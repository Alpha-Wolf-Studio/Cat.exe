using System;

public class Timer
{
    private bool on = false;
    private float target = 0f;
    private float currValue = 0f;
    private MODE mode = default;
    private Action onReached = null;

    public enum MODE { ONCE, REPEAT }

    public float CurrValue => currValue;
    public bool On => on;

    public Timer(float target, MODE mode = default, bool autostart = false, Action onReached = null)
    {
        this.mode = mode;
        this.onReached = onReached;

        SetTimer(target, autostart);
    }

    public void SetTimer(float target, bool autostart = true)
    {
        this.target = target;

        Reset();
        ToggleTimer(autostart);
    }

    public void ToggleTimer(bool state)
    {
        on = state;
    }

    public void Reset()
    {
        currValue = 0;
    }

    public void Update(float deltaTime)
    {
        if (!on) return;

        currValue += deltaTime;
        if (currValue >= target)
        {
            OnReachTarget();
        }
    }

    private void OnReachTarget()
    {
        on = false;
        currValue = target;

        onReached?.Invoke();

        if (mode == MODE.REPEAT)
        {
            SetTimer(target);
        }
    }
}