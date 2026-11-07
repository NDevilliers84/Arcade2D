using UnityEngine;

public struct Trigger
{
    [SerializeField] private float _endTime;
    private const float Duration = 0.3f;

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