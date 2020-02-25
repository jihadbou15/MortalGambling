using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class Action : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [System.Serializable]
    public enum ActionType
    {
        MELEE,
        MAGIC,
        ITEM,
        NONE
    }

    public ActionType Type = ActionType.NONE;

    public delegate void ActionCallback(Action action);
    public event ActionCallback OnActivate;

    [SerializeField] protected Image _image = null;
    public bool _isRegisteringInput { get; private set; } = false;

    public virtual void OnPointerClick(PointerEventData eventData) 
    {
        if (_isRegisteringInput) InvokeCallback();
    }

    public void InvokeCallback()
    {
        OnActivate.Invoke(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_isRegisteringInput) _image.color = Color.blue;
        else _image.color = Color.grey;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_isRegisteringInput) _image.color = Color.white;
        else _image.color = Color.grey;
    }

    public void SetRegisteringInput(bool isRegisteringInput)
    {
        _isRegisteringInput = isRegisteringInput;
        if (_isRegisteringInput) _image.color = Color.white;
        else _image.color = Color.grey;
    }
}
