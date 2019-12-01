using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Item : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Action _action = null;
    [SerializeField] private List<Texture2D> _itemTextures = new List<Texture2D>();

    private Image _image = null;
    private bool _registeringInput;

    //Events
    public delegate void Activate(Action action);
    public event Activate OnActivate;

    public void Initialize(Action action)
    {
        _action = action;
    }

    public void Tick()
    {

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_registeringInput) OnActivate.Invoke(_action);
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
