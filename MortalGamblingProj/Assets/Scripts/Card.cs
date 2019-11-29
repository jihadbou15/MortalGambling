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
    public enum Target : int
    {
        Head = 1,
        Body = 0,
        Legs = -1,
    }

    //Variables
    [System.Serializable]
    public struct CardData
    {
        public Type _type;
        public Target _target;
        public float _baseDamage;
    }

    [SerializeField] private CardData _cardData;
    [SerializeField] private List<Texture2D> _meleeTextures = new List<Texture2D>();
    private Image _image;
    private bool _registeringInput;

    //Events
    public delegate void Activate(CardData cardData);
    public event Activate OnActivate;

    public void Initialize(
        Type type,
        Target target,
        float baseDamage)
    {
        _cardData._type = type;
        _cardData._target = target;
        _cardData._baseDamage = baseDamage + ((float)_cardData._target * (.5f * baseDamage));
        _image = gameObject.GetComponent<Image>();
        _image.sprite = Sprite.Create(
            _meleeTextures[(int)target + 1], 
            new Rect(0.0f, 0.0f, _meleeTextures[(int)target + 1].width, _meleeTextures[(int)target + 1].height), 
            new Vector2(0.5f, 0.5f));
        _registeringInput = false;
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
        if (_registeringInput) _image.color = Color.red;
    }

    public void SetRegisteringInput(bool isRegisteringInput)
    {
        _registeringInput = isRegisteringInput;
        _image.color = Color.red;
    }
}
