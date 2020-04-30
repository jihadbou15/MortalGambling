using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAI : MonoBehaviour
{
    [SerializeField] private Player pawn;
    private List<Melee> _meleeActions = new List<Melee>();
    private bool _activated = false;


    public void Initialize(List<Melee> meleeActions)
    {
        _meleeActions = meleeActions;
    }

    public void ChooseAction()
    {
        if (_activated)
        {
            int choice = Random.Range(0, _meleeActions.Count - 1);
            _meleeActions[choice].InvokeCallback();
            _activated = false;
            Debug.LogWarning(_meleeActions[choice].MeleeTarget.ToString());
        }
    }

    public void SetAI(bool isActive)
    {
        _activated = isActive;
    }
}
