using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardMovement : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Transform _transform;
    private Vector3 startPosition;
    private Vector3 targetPosition;
    [SerializeField] private float offset = 0;
    [SerializeField] private float lerpSpeed = 0;
    private bool pointerIsHovering = false;
    private Action linkedAction;

    public void Initialize()
    {
        _transform = GetComponent<Transform>();
        linkedAction = GetComponent<Action>();
        startPosition = transform.localPosition;
        targetPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + offset, transform.localPosition.z);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        pointerIsHovering = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        pointerIsHovering = false;
    }
    public void Update()
    {
        if(pointerIsHovering && linkedAction._isRegisteringInput) transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, lerpSpeed);
        else transform.localPosition = Vector3.Lerp(transform.localPosition, startPosition, lerpSpeed);
    }
}
