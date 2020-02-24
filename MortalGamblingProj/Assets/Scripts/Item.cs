using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Item : Action
{   
    public float HealthEffect { get; protected set; } = 0.0f;
    public float StaminaEffect { get; protected set; } = 0.0f;
    public Player.Debuff DebuffEffect { get; protected set; } = Player.Debuff.NONE;
    public string ItemName { get; protected set; } = "";
    public int ItemAmount { get; protected set; } = 1;
    [SerializeField] protected Text GUIItemAmount;


    public void UseItem()
    {
        ItemAmount--;
        GUIItemAmount.text = ItemAmount.ToString();
        if (ItemAmount < 0) ItemAmount = 0; 
    }

    public void AddItem(int amount)
    {
        ItemAmount += amount;
        GUIItemAmount.text = ItemAmount.ToString();
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button.Equals(PointerEventData.InputButton.Right)) AddItem(1);
        else if (_isRegisteringInput) InvokeCallback();
    }

}
