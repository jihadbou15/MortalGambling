using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Potion : Item
{
    public virtual void Initialize(Sprite itemSprite, Text guiItemAmount, float healthEffect, float staminaEffect, Player.Debuff debuffEffect, string itemName)
    {
        _image.sprite = itemSprite;
        HealthEffect = healthEffect;
        StaminaEffect = staminaEffect;
        DebuffEffect = debuffEffect;
        ItemName = itemName;
        GUIItemAmount = guiItemAmount;
    }
}
