using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    //Events
    public delegate void Activate(Action action, int index);
    public event Activate OnActivate;

    public delegate void PlayerEmpty();
    public event PlayerEmpty OnPlayerHealthEmpty;
    public event PlayerEmpty OnPlayerStaminaEmpty;

    //Variables
    private float _health;
    [SerializeField] private float _maxHealth = 100;
    private float _stamina;
    [SerializeField] private float _maxStamina = 100;
    private List<Card> _cards = new List<Card>();
    [SerializeField] private int _cardAmount = 3;
    [SerializeField] private Slider _UIHealth = null;
    [SerializeField] private Slider _UIStamina = null;
    [SerializeField] private Transform _cardPosition =  null;
    [SerializeField] private Card _cardPrefab = null;
    [SerializeField] private float _cardOffset = 0.0f;
    [SerializeField] private float _staminaRechargePercent = 0.0f;
    [SerializeField] private float _cardBaseDamage = 0.0f;
    [SerializeField] private float _cardBaseStaminaCost = 0.0f;

    private int _index = -1;

    public void Initialize(int index)
    {
        for(int i = 0; i < _cardAmount; i++)
        {
            Card newCard = GameObject.Instantiate(_cardPrefab);
            newCard.Initialize(new Action(Action.ActionType.Melee, _cardBaseDamage, _cardBaseStaminaCost), (Card.Target)i - 1);
            newCard.OnActivate += OnCardChosen;
            newCard.transform.SetPositionAndRotation(new Vector3(
                _cardPosition.transform.position.x + (_cardOffset * (i - 1)), 
                _cardPosition.transform.position.y, 0),
                newCard.transform.rotation);
            _cards.Add(newCard);
            newCard.transform.SetParent(gameObject.transform);
        }
        Reset();
        _index = index;
    }

    public void Tick()
    {
        if(_health <= 0) OnPlayerHealthEmpty.Invoke();
        if (_stamina <= 0) OnPlayerStaminaEmpty.Invoke();
    }

    private void OnCardChosen(Action action)
    {
        EnableCardInput(false);
        OnActivate.Invoke(action, _index);
    }

    public void OnTurnEnd()
    {
        EnableCardInput(true);
    }

    public void DoHealthChange(float healthChange)
    {
        _health += healthChange;
        if(_health > _maxHealth)
        {
            _health = _maxHealth;
        }
        else if(_health <= 0.0f)
        {
            _health = 0.0f;
            OnPlayerHealthEmpty?.Invoke();
        }
        _UIHealth.value = _health / _maxHealth;
    }

    public void DoStaminaChange(float staminaChange)
    {
        _stamina += staminaChange;
        if (_stamina > _maxStamina)
        {
            _stamina = _maxStamina;
        }
        else if (_stamina <= 0.0f)
        {
            _stamina = 0.0f;
            OnPlayerStaminaEmpty?.Invoke();
        }
        _UIStamina.value = _stamina / _maxStamina;
    }

    public void RechargeStamina()
    {
        DoStaminaChange(_staminaRechargePercent * 0.01f * _maxStamina);
    }

    public void EnableCardInput(bool isEnabled)
    {
        foreach (Card card in _cards)
        {
            card.SetRegisteringInput(isEnabled);
        }
    }

    public void Reset()
    {
        EnableCardInput(true);

        _health = _maxHealth;
        _stamina = _maxStamina;
        _UIHealth.value = _health / _maxHealth;
        _UIStamina.value = _stamina / _maxStamina;
    }
}
