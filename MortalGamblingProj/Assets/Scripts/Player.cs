using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{


    //Events
    public delegate void Activate(Card.Target target, Team _OwningTeam);
    public event Activate OnActivate;

    //Variables
    [SerializeField] private float _health;
    [SerializeField] private float _maxHealth;
    [SerializeField] private float _stamina;
    [SerializeField] private float _maxStamina;
    private List<Card> _cards;

    private void Initialize()
    {
        foreach(Card card in _cards) 
        {
            card.OnActivate += OnCardChosen;
        }
    }


    private void OnCardChosen(Card.Target target)
    {
        foreach (Card card in _cards)
        {
            card.SetRegisteringInput(false);
        }
    }

    public void OnTurnEnd()
    {
        foreach (Card card in _cards)
        {
            card.SetRegisteringInput(true);
        }
    }

    public void OnHealthLost(float healthLost)
    {
        _health -= healthLost;
    }

    public void OnStaminaLost(float staminaLost)
    {
        _stamina -= staminaLost;
    }

    public void Reset()
    {
        foreach (Card card in _cards)
        {
            card.SetRegisteringInput(true);
        }

        _health = _maxHealth;
        _stamina = _maxStamina;
    }
}
