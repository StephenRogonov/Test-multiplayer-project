using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HealthController : MonoBehaviour
{
    [SerializeField] private float _currentHealth;
    [SerializeField] private float _maxHealth;

    public UnityEvent OnDeath;
    public UnityEvent<float> OnHealthChange;

    private void Awake()
    {
        _currentHealth = _maxHealth;
    }

    public void TakeDamage(float damageAmount)
    {
        _currentHealth -= damageAmount;
        SendRemainingPercentage(_currentHealth / _maxHealth);
        IsDeadCheck();
    }

    private void SendRemainingPercentage(float health)
    {
        OnHealthChange.Invoke(health);
    }

    public void IsDeadCheck()
    {
        if (_currentHealth <= 0)
        {
            OnDeath.Invoke();
        }
    }
}
