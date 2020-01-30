using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Melee : Action
{
    //Card Specific Data
    [System.Serializable]
    public enum Target : int
    {
        HEAD = 1,
        BODY = 0,
        LEGS = -1,
        NONE = 2
    }

    public Target MeleeTarget { get; private set; } = Target.NONE;
    public float Damage { get; private set; } = 0.0f;
    public float StaminaCost { get; private set; } = 0.0f;

    public void Initialize(Sprite meleeSprite, Target meleeTarget, float baseDamage, float baseStaminaCost)
    {
        _image.sprite = meleeSprite;
        MeleeTarget = meleeTarget;

        Damage = baseDamage + ((int)meleeTarget * 0.5f * baseDamage);
        StaminaCost = baseStaminaCost + ((int)meleeTarget * 0.5f * baseStaminaCost);
    }
}
