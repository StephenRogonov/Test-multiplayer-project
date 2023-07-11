using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthMeter : MonoBehaviour
{
    [SerializeField] private Image _healthBar;

    public void ChangeHealthMeter(float percentage)
    {
        _healthBar.fillAmount = percentage;
    }
}