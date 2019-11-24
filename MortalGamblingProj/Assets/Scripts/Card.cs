using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Card : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    //Enums
    public enum Type
    {
        Melee,
        Magic
    }
    public enum Target
    {
        Head,
        Body,
        Legs
    }

    //Variables
    [SerializeField] private Type _type;
    [SerializeField] private Target _target;
    private Image _image;
    private bool _registeringInput;

    //Events
    public delegate void Activate(Target target);
    public event Activate OnActivate;

    void Initialized()
    {
        _image = gameObject.GetComponent<Image>();
    }

    void Tick()
    {

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(_registeringInput) OnActivate.Invoke(_target);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_registeringInput) _image.color = Color.blue;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_registeringInput) _image.color = Color.clear;
    }

    public void SetRegisteringInput(bool isRegisteringInput)
    {
        _registeringInput = isRegisteringInput;
        _image.color = Color.red;
    }
}
