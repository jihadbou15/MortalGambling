using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemMenu : MonoBehaviour, IPointerClickHandler 
{
    [SerializeField] private float _itemSpacing = 0;
    [SerializeField] private Player Player = null;
    private List<Item> _itemActions = new List<Item>();
    private bool _isOpen = false;

    public void Initialize(List<Item> items)
    {
        _itemActions = items;
        transform.SetAsLastSibling();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(Player._ItemsEnabled) _isOpen = !_isOpen;
    }

    private void Update()
    {
        if (_isOpen) OpenMenu();
        else CloseMenu();
    }

    private void OpenMenu()
    {
        for(int i = 0; i < _itemActions.Count; i++)
        {
            Vector3 newPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + (i * _itemSpacing) + 100.0f, transform.localPosition.z);
            _itemActions[i].transform.localPosition = Vector3.Lerp(_itemActions[i].transform.localPosition,newPosition, 0.1f);
        }
    }

    private void CloseMenu()
    {
        foreach(Item item in _itemActions)
        {
            item.transform.localPosition = Vector3.Lerp(item.transform.localPosition, transform.localPosition, 0.1f);
        }
    }

    public void SetOpenFlag(bool isOpen)
    {
        _isOpen = isOpen;
    }
}
