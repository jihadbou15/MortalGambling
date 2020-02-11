using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : Action
{   
    public float HealthEffect { get; protected set; } = 0.0f;
    public float StaminaEffect { get; protected set; } = 0.0f;
    public Player.Debuff DebuffEffect { get; protected set; } = Player.Debuff.NONE;
    public string ItemName { get; protected set; } = "";
    public int ItemAmount { get; protected set; } = 1;
    [SerializeField] protected Text GUIItemAmount;
}
