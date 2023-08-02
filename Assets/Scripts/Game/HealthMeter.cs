using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class HealthMeter : MonoBehaviourPun
{
    [SerializeField] private Image _healthBar;

    public void ChangeHealthMeter(float percentage)
    {
        _healthBar.fillAmount = percentage;
    }
}
