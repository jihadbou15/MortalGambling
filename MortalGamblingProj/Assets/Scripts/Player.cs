using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    //Events
    public delegate void Activate(Card.CardData cardData);
    public event Activate OnActivate;

    public delegate void PlayerEmpty();
    public event PlayerEmpty OnPlayerHealthEmpty;
    public event PlayerEmpty OnPlayerStaminaEmpty;

    //Variables
    [SerializeField] private float _health;
    [SerializeField] private float _maxHealth;
    [SerializeField] private float _stamina;
    [SerializeField] private float _maxStamina;
    [SerializeField] private List<Card> _cards;
    [SerializeField] private Slider _UIHealth;
    [SerializeField] private Slider _UIStamina;

    public void Initialize()
    {
        foreach(Card card in _cards) 
        {
            card.OnActivate += OnCardChosen;
        }
        Reset();
    }

    public void Tick()
    {
        if(_health <= 0) OnPlayerHealthEmpty.Invoke();
        if (_stamina <= 0) OnPlayerStaminaEmpty.Invoke();
    }

    private void OnCardChosen(Card.CardData cardData)
    {
        foreach (Card card in _cards)
        {
            card.SetRegisteringInput(false);
        }
        OnActivate.Invoke(cardData);
    }

    public void OnTurnEnd()
    {
        foreach (Card card in _cards)
        {
            card.SetRegisteringInput(true);
        }
    }

    public void OnHealthChange(float healthChange)
    {
        _health += healthChange;
        _UIHealth.value = _health / _maxHealth;
    }

    public void OnStaminaChange(float staminaChange)
    {
        _stamina += staminaChange;
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
