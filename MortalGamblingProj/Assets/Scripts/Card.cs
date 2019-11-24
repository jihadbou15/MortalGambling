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
        Magic,
        Item
    }
    public enum Target
    {
        Head = 1,
        Body = 0,
        Legs = -1,
    }

    //Variables
    [System.Serializable]
    public struct CardData
    {
        private Type _type;
        private Target _target;
        private float _baseDamage;
    }

    [SerializeField] private CardData _cardData;
    private Image _image;
    private bool _registeringInput;

    //Events
    public delegate void Activate(CardData cardData);
    public event Activate OnActivate;

    public void Initialized()
    {
        _image = gameObject.GetComponent<Image>();
    }

    public void Tick()
    {

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(_registeringInput) OnActivate.Invoke(_cardData);
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
