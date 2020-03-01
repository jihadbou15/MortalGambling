using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBarSmoothing : MonoBehaviour
{
    [SerializeField] private Slider slider = null;
    [SerializeField] private float _lerpSpeed = 0.1f; 
    private float _currentValue = 0.0f;
    // Start is called before the first frame update
    public void Initialize(float currentValue)
    {
        _currentValue = currentValue;
    } 

    // Update is called once per frame
    public void OnValueChange(float newValue)
    {
        _currentValue = newValue;
    }

    private void Update()
    {
        slider.value = Mathf.Lerp(slider.value, _currentValue, _lerpSpeed);
    }
}
