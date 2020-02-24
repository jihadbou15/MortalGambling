using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disabler : Item
{
    public virtual void Initialize(Sprite disablerSprite, Player.Debuff debuffEffect, string itemName)
    {
        _image.sprite = disablerSprite;
        HealthEffect = 0.0f;
        StaminaEffect = 0.0f;
        DebuffEffect = debuffEffect;
        ItemName = itemName;
    }
}
