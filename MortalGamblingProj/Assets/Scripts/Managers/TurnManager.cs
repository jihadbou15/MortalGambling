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
        if(_attackerActions[0].Action.Type == Action.ActionType.MELEE &&
            _defenderActions[0].Action.Type == Action.ActionType.MELEE)
        {
            Melee melee1 = (Melee)_attackerActions[0].Action;
            Melee melee2 = (Melee)_defenderActions[0].Action;
            int difference = Mathf.Abs((int)melee1.MeleeTarget - (int)melee2.MeleeTarget);

            if (difference == 0)
            {
                OnApplyTurnOutcome?.Invoke(Outcome.Parry, _attackerActions[0], _defenderActions[0]);
            }
            else if (difference == 1)
            {
                OnApplyTurnOutcome?.Invoke(Outcome.Defend, _attackerActions[0], _defenderActions[0]);
            }
            else if (difference == 2)
            {
                OnApplyTurnOutcome?.Invoke(Outcome.Hit, _attackerActions[0], _defenderActions[0]);
            }
        }
        else if (_attackerActions[0].Action.Type == Action.ActionType.MAGIC &&
            _attackerActions[0].Action.Type == Action.ActionType.MAGIC)
        {

        }
        else if (_attackerActions[0].Action.Type == Action.ActionType.ITEM &&
            _attackerActions[0].Action.Type == Action.ActionType.ITEM)
        {

        }
    }
}
