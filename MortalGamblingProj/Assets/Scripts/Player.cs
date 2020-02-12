using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public enum Debuff : int
    {
        NONE = -2,
        CANNOTLEGS,
        CANNOTBODY,
        CANNOTHEAD,
        NOHEALING,
        NOSTAMINAREGEN,        
    }

    //Events
    public delegate void Activate(Action action, int playerId);
    public event Activate OnActivate;

    public delegate void PlayerEmpty();
    public event PlayerEmpty OnPlayerHealthEmpty;
    public event PlayerEmpty OnPlayerStaminaEmpty;

    //Player variables
    [SerializeField] private float _maxHealth = 100;
    [SerializeField] private float _maxStamina = 100;
    [SerializeField] private Slider _UIHealth = null;
    [SerializeField] private Slider _UIStamina = null;
    [SerializeField] private float _staminaRechargePercent = 0.0f;

    private int _id = -1;
    private float _health;
    private float _stamina;
    private Debuff _debuff;

    //Melee variables
    [SerializeField] private int _meleeAmount = 3;
    [SerializeField] private Transform _meleePosition =  null;
    [SerializeField] private Melee _meleePrefab = null;
    [SerializeField] private float _meleeOffset = 0.0f;
    [SerializeField] private float _meleeBaseDamage = 0.0f;
    [SerializeField] private float _meleeBaseStaminaCost = 0.0f;
    [SerializeField] private List<Sprite> _meleeSprites = new List<Sprite>();

    //Item variables
    [SerializeField] private Item _itemPrefab = null;

    private List<Melee> _meleeActions = new List<Melee>();

    public void Initialize(int index)
    {
        for(int i = 0; i < _meleeAmount; i++)
        {
            CreateMelee((Melee.Target)(i - 1), _meleeSprites[i], (i - 1) * _meleeOffset);
        }
        Reset();
        _id = index;
    }

    private void CreateMelee(Melee.Target meleeTarget, Sprite meleeSprite, float offset)
    {
        Melee newMelee = Instantiate(_meleePrefab);
        newMelee.Initialize(meleeSprite, meleeTarget, _meleeBaseDamage, _meleeBaseStaminaCost);
        newMelee.OnActivate += OnCardChosen;
        newMelee.transform.position = _meleePosition.position + new Vector3(offset, 0, 0);
        newMelee.transform.SetParent(transform);
        _meleeActions.Add(newMelee);
    }

    public void Tick()
    {
        if(_health <= 0) OnPlayerHealthEmpty.Invoke();
        if (_stamina <= 0) OnPlayerStaminaEmpty.Invoke();
    }

    private void OnCardChosen(Action action)
    {
        OnActivate.Invoke(action, _id);
    }

    public void OnTurnEnd()
    {
        EnableCardInput(true);
    }

    public void DoHealthChange(float healthChange)
    {
        if (_debuff == Debuff.NOHEALING) return;
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
        if (_debuff == Debuff.NOSTAMINAREGEN) return;
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

    public void DoApplyDebuff (Debuff debuffToApply)
    {
        _debuff = debuffToApply;
    }

    public void RechargeStamina()
    {
        DoStaminaChange(_staminaRechargePercent * 0.01f * _maxStamina);
    }

    public void EnableCardInput(bool isEnabled)
    {
        foreach (Melee melee in _meleeActions)
        {
            if (IsDebuffed(melee,_debuff)) melee.SetRegisteringInput(false);
            else melee.SetRegisteringInput(isEnabled);
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

    private bool IsDebuffed(Melee melee, Debuff currentDebuff)
    {
        switch (melee.MeleeTarget)
        {
            case Melee.Target.BODY:
                return currentDebuff == Debuff.CANNOTBODY;
            case Melee.Target.HEAD:
                return currentDebuff == Debuff.CANNOTHEAD;           
            case Melee.Target.LEGS:
                return currentDebuff == Debuff.CANNOTLEGS;
            default:
                return false;
        }
    }
}
