using UnityEngine;

public class Potion : Item
{
    public virtual void Initialize(Sprite potionSprite, float healthEffect, float staminaEffect, Player.Debuff debuffEffect, string itemName)
    {
        _image.sprite = potionSprite;
        HealthEffect = healthEffect;
        StaminaEffect = staminaEffect;
        DebuffEffect = debuffEffect;
        ItemName = itemName;
    }
}
