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

    //Events
    public delegate void Activate(Target target);
    public event Activate onActivate;

    void Initialized()
    {
        _image = gameObject.GetComponent<Image>();
    }

    void Tick()
    {

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        onActivate.Invoke(_target);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _image.color = Color.blue;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _image.color = Color.clear;
    }
}
