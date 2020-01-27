using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public enum Outcome
    {
        Parry,
        Defend,
        Hit,
        ItemUse
    }

    [System.Serializable]
    struct MeleeResolver
    {
        public Outcome GetOutcome(Melee.Target attackerTarget, Melee.Target defenderTarget)
        {
            MeleeAttack found = MeleeAttacks.Find((MeleeAttack meleeAttack) => 
            {
                return meleeAttack.Attacks == attackerTarget;
            });

            return found.GetOutcome(defenderTarget);
        }

        public List<MeleeAttack> MeleeAttacks;
    }

    [System.Serializable]
    struct MeleeAttack
    {
        public Outcome GetOutcome(Melee.Target defenderTarget)
        {
            if(defenderTarget == GetsParriedBy)
            {
                return Outcome.Parry;
            }
            else if(defenderTarget == GetsHitOn)
            {
                return Outcome.Hit;
            }
            return Outcome.Defend;
        }

        public Melee.Target Attacks;
        public Melee.Target GetsParriedBy;
        public Melee.Target GetsHitOn;
    }

    [SerializeField] private MeleeResolver _resolver;

    public delegate void TurnHandler();
    public event TurnHandler OnTurnEnd;

    public delegate void TurnResolverHandler(Outcome outcome, TeamManager.ActionData attackerAction, TeamManager.ActionData defenderAction);
    public event TurnResolverHandler OnApplyTurnOutcome;

    private List<TeamManager.ActionData> _attackerActions = new List<TeamManager.ActionData>();
    private List<TeamManager.ActionData> _defenderActions = new List<TeamManager.ActionData>();
    private int _actionAmount = 0;
    private int _actionCounter = 0;
    private int _teamAmount = 0;
    private int _teamCounter = 0;
    private bool _isAttacking = false;

    public void Initialize(int teamAmount)
    {
        _teamAmount = teamAmount;
        _isAttacking = true;
    }

    public void Tick()
    {

    }

    public void SetActionAmount(int actionAmount)
    {
        _actionAmount = actionAmount;
    }

    public int GetTeamCounter()
    {
        return _teamCounter;
    }

    public void ResetTurns()
    {
        _isAttacking = true;
        _actionCounter = 0;
        _teamCounter = 0;
        _attackerActions.Clear();
        _defenderActions.Clear();
    }

    public void AddToResolver(TeamManager.ActionData actionData)
    {
        if(_isAttacking)
        {
            _attackerActions.Add(actionData);
            ++_actionCounter;
            if(_actionCounter >= _actionAmount)
            {
                _isAttacking = false;
                _actionCounter = 0;
                _teamCounter++;
                OnTurnEnd?.Invoke();
            }
        }
        else
        {
            _defenderActions.Add(actionData);
            ++_actionCounter;
            if (_actionCounter >= _actionAmount)
            {
                _actionCounter = 0;
                _teamCounter++;
                if(_teamCounter < _teamAmount)
                {
                    OnTurnEnd?.Invoke();
                }
            }
        }

        if(_teamCounter >= _teamAmount)
        {
            ResolveTeams();
            ResetTurns();
        }
    }

    public void ResolveTeams()
    {
        var attackerType = _attackerActions[0].Action.Type;
        var defenderType = _defenderActions[0].Action.Type;

        if (attackerType == Action.ActionType.MELEE &&
            defenderType == Action.ActionType.MELEE)
        {
            ResolveMelee();
        }
        else if (attackerType == Action.ActionType.MAGIC &&
                defenderType == Action.ActionType.MAGIC)
        {

        }
        else
        {

        }
    }

    public void ResolveMelee()
    {
        Melee.Target attackerTarget = ((Melee)_attackerActions[0].Action).MeleeTarget;
        Melee.Target defenderTarget = ((Melee)_defenderActions[0].Action).MeleeTarget;

        Outcome outcome = _resolver.GetOutcome(attackerTarget, defenderTarget);

        OnApplyTurnOutcome?.Invoke(outcome, _attackerActions[0], _defenderActions[0]);
    }
}
