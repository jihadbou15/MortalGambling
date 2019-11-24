using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField] private List<Card> _cards;
    [SerializeField] private Slider _UIHealth;
    [SerializeField] private Slider _UIStamina;

    private void Initialize()
    {
        foreach(Card card in _cards) 
        {
            card.OnActivate += OnCardChosen;
        }
        Reset();
    }


    private void OnCardChosen(Card.Target target, float damage)
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
        _UIHealth.value = _health / _maxHealth;
    }

    public void OnStaminaLost(float staminaLost)
    {
        _stamina -= staminaLost;
        _UIStamina.value = _stamina / _maxStamina;
    }

    public void Reset()
    {
        foreach (Card card in _cards)
        {
            card.SetRegisteringInput(true);
        }

        _health = _maxHealth;
        _stamina = _maxStamina;
        _UIHealth.value = _health / _maxHealth;
        _UIStamina.value = _stamina / _maxStamina;
    }
}
