using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    //Events
    public delegate void Activate(Card.CardData cardData, int index);
    public event Activate OnActivate;

    public delegate void PlayerEmpty();
    public event PlayerEmpty OnPlayerHealthEmpty;
    public event PlayerEmpty OnPlayerStaminaEmpty;

    //Variables
    private float _health;
    [SerializeField] private float _maxHealth;
    private float _stamina;
    [SerializeField] private float _maxStamina;
    private List<Card> _cards = new List<Card>();
    [SerializeField] private int _cardAmount = 3;
    [SerializeField] private Slider _UIHealth;
    [SerializeField] private Slider _UIStamina;
    [SerializeField] private Transform _cardPosition;


    private int _index = -1;

    public void Initialize(int index, Card prefab)
    {
        for(int i = 0; i < _cardAmount; i++)
        {
            Card newCard = GameObject.Instantiate(prefab);
            newCard.Initialize(Card.Type.Melee, (Card.Target)i - 1, (1.5f - (.5f * i)) * 20.0f);
            newCard.OnActivate += OnCardChosen;
            newCard.transform.SetPositionAndRotation(new Vector3(_cardPosition.transform.position.x + (200.0f * i), _cardPosition.transform.position.y, 0),newCard.transform.rotation);
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

    private void OnCardChosen(Card.CardData cardData)
    {
        foreach (Card card in _cards)
        {
            card.SetRegisteringInput(false);
        }
        OnActivate.Invoke(cardData, _index);
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
