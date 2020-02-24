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
        CANNOTUSEITEM
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
    private Debuff _debuff = Debuff.NONE;
    private bool _debuffCanBeCleared = false;

    //Melee variables
    [SerializeField] private int _meleeAmount = 3;
    [SerializeField] private Transform _meleePosition = null;
    [SerializeField] private Melee _meleePrefab = null;
    [SerializeField] private float _meleeOffset = 0.0f;
    [SerializeField] private float _meleeBaseDamage = 0.0f;
    [SerializeField] private float _meleeBaseStaminaCost = 0.0f;
    [SerializeField] private List<Sprite> _meleeSprites = new List<Sprite>();
    [SerializeField] private List<Sprite> _itemSprites = new List<Sprite>();

    //Item variables
    [SerializeField] private Potion _potionPrefab = null;
    [SerializeField] private Disabler _disablerPrefab = null;
    [SerializeField] private Transform _itemPosition = null;
    [SerializeField] private float _itemOffset = 0.0f;


    private List<Melee> _meleeActions = new List<Melee>();
    private List<Item> _itemActions = new List<Item>();


    public void Initialize(int index)
    {
        for(int i = 0; i < _meleeAmount; i++)
        {
            CreateMelee((Melee.Target)(i - 1), _meleeSprites[i], (i - 1) * _meleeOffset);
        }
        CreatePotion(_itemOffset,false);
        CreatePotion(_itemOffset - 100.0f, true);
        CreateDisabler(_itemOffset + 450.0f, Debuff.CANNOTHEAD);
        CreateDisabler(_itemOffset + 550.0f, Debuff.CANNOTBODY);
        CreateDisabler(_itemOffset + 650.0f, Debuff.CANNOTLEGS);
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

    private void CreatePotion(float offset, bool isStamina)
    {
        Potion newPotion = Instantiate(_potionPrefab);
        if(isStamina) newPotion.Initialize(_itemSprites[1],0, 20,Debuff.NONE,"Stamina Potion");
        else newPotion.Initialize(_itemSprites[0], 20, 0, Debuff.NONE, "Health Potion");
        newPotion.OnActivate += OnItemChosen;
        newPotion.transform.position = _itemPosition.position + new Vector3(offset, 0, 0);
        newPotion.transform.SetParent(transform);
        _itemActions.Add(newPotion);
        newPotion.AddItem(2);
    }

    private void CreateDisabler(float offset, Debuff toDisable)
    {
        Disabler newDisabler = Instantiate(_disablerPrefab);
        
        switch(toDisable)
        {
            case Debuff.CANNOTHEAD:
                newDisabler.Initialize(_meleeSprites[2], toDisable, "Disable Head");
                break;
            case Debuff.CANNOTBODY:
                newDisabler.Initialize(_meleeSprites[1], toDisable, "Disable Body");
                break;
            case Debuff.CANNOTLEGS:
                newDisabler.Initialize(_meleeSprites[0], toDisable, "Disable Legs");
                break;
            default:
                Debug.Log("Could not debuff");
                break;
        }

        newDisabler.OnActivate += OnItemChosen;
        newDisabler.transform.position = _itemPosition.position + new Vector3(offset, 0, 0);
        newDisabler.transform.SetParent(transform);
        _itemActions.Add(newDisabler);
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

    private void OnItemChosen(Action item)
    {
        Item localItem = (Item)item;
        localItem.UseItem();
        OnActivate.Invoke(item, _id);
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

    public void CheckDebuff()
    {
        if (_debuffCanBeCleared) ClearDebuff();
        else if (_debuff != Debuff.NONE) 
            _debuffCanBeCleared = true;
    }

    public void ClearDebuff()
    {
        _debuff = Debuff.NONE;
        _debuffCanBeCleared = false;
    }

    public void RechargeStamina()
    {
        DoStaminaChange(_staminaRechargePercent * 0.01f * _maxStamina);
    }
    public void EnableCardInput(bool isEnabled)
    {
        //CheckDebuff();
        foreach (Melee melee in _meleeActions)
        {
            if (IsMeleeDebuffed(melee,_debuff)) melee.SetRegisteringInput(false);
            else melee.SetRegisteringInput(isEnabled);
        }

        foreach (Item item in _itemActions)
        {
            if (_debuff == Debuff.CANNOTUSEITEM) item.SetRegisteringInput(false);
            else if (item.ItemAmount <= 0) item.SetRegisteringInput(false);
            else item.SetRegisteringInput(isEnabled);
        }
    }

    public void Reset()
    {
        EnableCardInput(true);

        _health = _maxHealth;
        _stamina = _maxStamina;
        _UIHealth.value = _health / _maxHealth;
        _UIStamina.value = _stamina / _maxStamina;
        _debuff = Debuff.NONE;
    }


    private bool IsMeleeDebuffed(Melee melee, Debuff currentDebuff)
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
