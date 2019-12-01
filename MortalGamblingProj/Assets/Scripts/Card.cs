using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Card : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    //Enums
    public enum Target : int
    {
        Head = 1,
        Body = 0,
        Legs = -1,
    }

    //Variables
    [SerializeField] private Action _Action;
    [SerializeField] private Target _Target;
    [SerializeField] private List<Texture2D> _meleeTextures = new List<Texture2D>();
    private Image _image;
    private bool _registeringInput;

    //Events
    public delegate void Activate(Action action);
    public event Activate OnActivate;

    public void Initialize(
        Action action,
        Target target)
    {
        _Action = action;
        _Target = target;
        _Action.Data.BaseDamage = action.Data.BaseDamage + ((float)_Target * (.5f * action.Data.BaseDamage));
        //TODO - Come up with how we want to determine stamina cost
        _Action.Data.StaminaCost = action.Data.StaminaCost + ((float)_Target * (.5f * action.Data.StaminaCost));
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
        if(_registeringInput) OnActivate.Invoke(_Action);
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
