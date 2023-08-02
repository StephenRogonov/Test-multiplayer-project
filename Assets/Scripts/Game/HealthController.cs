using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;

public class HealthController : MonoBehaviourPun
{
    [SerializeField] private float _currentHealth;
    [SerializeField] private float _maxHealth;

    private PhotonView _photonView;

    public UnityEvent OnDeath;
    public UnityEvent<float> OnHealthChange;

    private void Awake()
    {
        _currentHealth = _maxHealth;
        _photonView = GetComponent<PhotonView>();
    }

    public void TakeDamage(float damage)
    {
        _photonView.RPC(nameof(RPC_TakeDamage), _photonView.Owner, damage);
    }

    [PunRPC]
    public void RPC_TakeDamage(float damage)
    {
        _currentHealth -= damage;
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
