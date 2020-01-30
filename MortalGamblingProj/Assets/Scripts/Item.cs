using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : Action
{
    [System.Serializable]
    public enum Debuff : int
    {
        HEAD = 1,
        BODY = 0,
        LEGS = -1,
        NONE = 2
    }
    public float HealthEffect { get; private set; } = 0.0f;
    public float StaminaEffect { get; private set; } = 0.0f;
    public Debuff DebuffEffect { get; private set; } = Debuff.NONE;
    public string ItemName { get; private set; } = "";

    // Start is called before the first frame update
    public void Initialize(Sprite itemSprite, float healthEffect, float staminaEffect, Debuff debuffEffect, string itemName)
    {
        _image.sprite = itemSprite;
        HealthEffect = healthEffect;
        StaminaEffect = staminaEffect;
        DebuffEffect = debuffEffect;
        ItemName = itemName;
    }
}
