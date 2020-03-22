using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class Action : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler ,IPointerDownHandler
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
    private bool _isHovering = false;
    private float _lerpSpeed = 0.2f;

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
        _isHovering = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _isHovering = false;
    }

    public void SetRegisteringInput(bool isRegisteringInput)
    {
        _isRegisteringInput = isRegisteringInput;
    }

    private void Update()
    {
        if (_isRegisteringInput)
        {
            if (_isHovering) SetHoveringFeedback();
            else SetUnHoveringFeedback();
        }
        else SetDisabledFeedback();

    }

    private void SetHoveringFeedback()
    {

        //_image.color = Color.Lerp(_image.color,Color.blue, _lerpSpeed);
        _image.color = Color.blue;
        transform.SetAsLastSibling();
    }

    private void SetUnHoveringFeedback()
    {
        _image.color = Color.Lerp(_image.color, Color.white, _lerpSpeed);
        transform.SetAsFirstSibling();
    }

    private void SetDisabledFeedback()
    {
        _image.color = Color.Lerp(_image.color, Color.grey, _lerpSpeed);
        transform.SetAsFirstSibling();
    }

    public void OnPointerUp(PointerEventData eventData)
    {

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        
    }
}
