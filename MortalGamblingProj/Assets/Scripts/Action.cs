using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action : MonoBehaviour
{
    public enum ActionType
    {
        Melee,
        Magic,
        Item
    }

    [System.Serializable]
    public struct ActionData
    {
        public ActionType Type;
        public float BaseDamage;
        public float StaminaCost;
    }

    [SerializeField] public ActionData Data;

    public Action(ActionType type, float baseDamage, float staminaCost)
    {
        Data.Type = type;
        Data.BaseDamage = baseDamage;
        Data.StaminaCost = staminaCost;
    }
}
