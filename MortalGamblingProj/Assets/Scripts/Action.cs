﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Action
{
    [System.Serializable]
    public enum ActionType
    {
        Melee,
        Magic,
        Item
    }

    [System.Serializable]
    public enum Target : int
    {
        Head = 1,
        Body = 0,
        Legs = -1,
    }


    [System.Serializable]
    public struct ActionData
    {
        public Target Target;
        public float BaseDamage;
        public float StaminaCost;
    }

    public ActionType Type;
    public ActionData Data;

    public Action(ActionType type, Target target, float baseDamage, float staminaCost)
    {
        Type = type;
        Data.Target = target;
        Data.BaseDamage = baseDamage;
        Data.StaminaCost = staminaCost;
    }
}
