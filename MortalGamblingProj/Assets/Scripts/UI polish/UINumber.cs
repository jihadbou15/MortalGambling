using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UINumber : MonoBehaviour
{
    [SerializeField] private Text text = null;
    [SerializeField] private float _lerpSpeed = 0.1f;
    private float _currentValue = 0.0f;
    private float _lerpingValue = 0.0f;

    public void Initialize(float currentValue)
    {
        _currentValue = currentValue;
        _lerpingValue = currentValue;
    }

    public void OnValueChange(float newValue)
    {
        _currentValue = newValue;
    }

    void Update()
    {
        _lerpingValue =  Mathf.Lerp(_lerpingValue, _currentValue, _lerpSpeed);
        text.text =_lerpingValue.ToString("#.");
    }
}
