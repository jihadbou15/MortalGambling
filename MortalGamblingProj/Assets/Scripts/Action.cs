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
    private bool _isRegisteringInput = false;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_isRegisteringInput) OnActivate.Invoke(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_isRegisteringInput) _image.color = Color.blue;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_isRegisteringInput) _image.color = Color.red;
    }

    public void SetRegisteringInput(bool isRegisteringInput)
    {
        _isRegisteringInput = isRegisteringInput;
        _image.color = Color.red;
    }
}
