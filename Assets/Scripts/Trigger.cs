using UnityEngine;

public struct Trigger
{
    private float _endTime;
    private const float Duration = 0.4f;

    public bool Value => Time.time < _endTime;

    public void Launch()
    {
        _endTime = Time.time + Duration;
    }

    public void Stop()
    {
        _endTime = Time.time;
    }
    
}