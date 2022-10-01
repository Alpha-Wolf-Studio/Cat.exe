using System;

public class Timer
{
    private bool on = false;
    private float start = 0f;
    private float currValue = 0f;
    private MODE mode = default;
    private Action<float> onUpdate = null;
    private Action onReached = null;

    public enum MODE { ONCE, REPEAT }

    public float CurrValue => currValue;
    public bool On => on;

    public Timer(float start, MODE mode = default, bool autostart = false, Action<float> onUpdate = null, Action onReached = null)
    {
        this.mode = mode;
        this.onUpdate = onUpdate;
        this.onReached = onReached;

        SetTimer(start, autostart);
    }

    public void SetTimer(float start, bool autostart = true)
    {
        this.start = start;

        Reset();
        ToggleTimer(autostart);
    }

    public void ToggleTimer(bool state)
    {
        on = state;
    }

    public void Reset()
    {
        currValue = start;

        onUpdate?.Invoke(currValue);
    }

    public void Update(float deltaTime)
    {
        if (!on) return;

        currValue -= deltaTime;
        if (currValue <= 0)
        {
            OnReachTarget();
        }

        onUpdate?.Invoke(currValue);
    }

    private void OnReachTarget()
    {
        on = false;
        currValue = 0;

        onReached?.Invoke();

        if (mode == MODE.REPEAT)
        {
            SetTimer(start);
        }
    }
}