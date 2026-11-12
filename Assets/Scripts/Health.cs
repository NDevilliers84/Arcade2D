using System;
using UnityEngine;

public class Health : MonoBehaviour 
{

    [SerializeField] private int _maxHealth = 100;

    private int _current;

    public event Action<Health> Died;

    public int Current => _current;
    public float Ratio => (float)_current / _maxHealth;
    public bool IsDead => _current <= 0;

    private void Awake() {
        _current = _maxHealth;
    }

    public bool TakeDamage(int amount) {
        if (IsDead) return true;

        _current = Mathf.Max(0, _current - amount);

        if (IsDead) Died?.Invoke(this);

        return IsDead;
    }
}
