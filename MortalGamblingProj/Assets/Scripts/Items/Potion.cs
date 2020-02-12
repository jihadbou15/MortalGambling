using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Potion : Item
{
    public virtual void Initialize(float healthEffect, float staminaEffect, Player.Debuff debuffEffect, string itemName)
    {
        HealthEffect = healthEffect;
        StaminaEffect = staminaEffect;
        DebuffEffect = debuffEffect;
        ItemName = itemName;
    }
}
