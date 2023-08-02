using System;
using TMPro;
using UnityEngine;

public class CoinCounter : MonoBehaviour
{
    [SerializeField] private TMP_Text _coinCounter;
    [SerializeField] private TMP_Text _winTotalCoinsCollected;

    private int _value;

    void Start()
    {
        _value = Convert.ToInt32(_coinCounter.text);
    }

    public void CoinCollect()
    {
        _value += 1;
        _coinCounter.text = _value.ToString();
        _winTotalCoinsCollected.text = _coinCounter.text;
    }
}
