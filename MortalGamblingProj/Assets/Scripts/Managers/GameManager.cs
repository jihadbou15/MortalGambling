﻿using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private InputManager _inputManager = null;
    [SerializeField] private PhaseManager _phaseManager = null;
    [SerializeField] private TurnManager _turnManager = null;
    [SerializeField] private TeamManager _teamManager = null;
    [SerializeField] private GameObject _endScreen = null;

    private int _enabledTeamID = -1;
    private bool _hasToSwapPhase = false;
    private bool _hasEnded = false;

    void Start()
    {
        //Initialize all managers
        _inputManager.Initialize();
        _inputManager.OnKeyDown += DoKeyDown;

        _teamManager.Initialize();
        _teamManager.OnCardActivate += DoCardActivate;
        _teamManager.OnStaminaEmpty += DoStaminaEmpty;
        _teamManager.OnHealthEmpty += DoHealthEmpty;

        int teamAmount = _teamManager.GetTeamAmount();

        _phaseManager.Initialize(teamAmount);
        _phaseManager.OnPhaseEnd += DoPhaseEnd;

        _turnManager.Initialize(teamAmount);
        _turnManager.OnTurnEnd += DoTurnEnd;
        _turnManager.OnApplyTurnOutcome += DoApplyTurnOutcome;

        //Initialize Phase Setup
        PhaseSetup();

        _endScreen.SetActive(false);
    }

    void Update()
    {
        if(_hasEnded)
        {
            return;
        }

        _inputManager.Tick();
        _phaseManager.Tick();
        _turnManager.Tick();
        _teamManager.Tick();
    }

    void PhaseSetup()
    {
        int attackingTeamId = _phaseManager.GetAttackingTeamIdx();
        _teamManager.CheckTeamDebuffs();

        for (int i = 0; i < _teamManager.GetTeamAmount(); ++i)
        {
            bool enable = false;
            if (i == attackingTeamId)
            {
                enable = true;
                _enabledTeamID = i;
                _teamManager.SetPhaseFeedback(true, i);
            }
            else _teamManager.SetPhaseFeedback(false, i);

            _teamManager.EnableTeamCardInput(enable, i);

        }

        _turnManager.SetActionAmount(_teamManager.GetPlayerAmount(attackingTeamId));
    }

    void SwapPhase()
    {
        _turnManager.ResetTurns();
        _phaseManager.SwapPhase();
        _hasToSwapPhase = false;
    }

    private void DoKeyDown(KeyCode keyCode)
    {
        //Check for input here
    }

    private void DoPhaseEnd()
    {
        //_teamManager.CheckTeamDebuffs();
        PhaseSetup();
        _teamManager.RechargeEveryoneStamina();
    }

    private void DoTurnEnd()
    {
        int activeID = _phaseManager.GetAttackingTeamIdx();
        int teamAmount = _teamManager.GetTeamAmount();

        activeID += _turnManager.GetTeamCounter();
        if(activeID >= teamAmount)
        {
            activeID = 0;
        }

        _teamManager.EnableTeamCardInput(false, _enabledTeamID);
        _teamManager.EnableTeamCardInput(true, activeID);
        _enabledTeamID = activeID;
    }

    private void DoApplyTurnOutcome(TurnManager.Outcome outcome, TeamManager.ActionData attackerAction, TeamManager.ActionData defenderAction)
    {
               
        switch (outcome)
        {
            case TurnManager.Outcome.Parry:
            {
                Melee attackerMeleeAction = (Melee)attackerAction.Action;
                _teamManager.ApplyTeamStaminaChange(attackerAction.TeamID, attackerAction.PlayerID, -(int)attackerMeleeAction.StaminaCost);
                SwapPhase();
                break;
            }
            case TurnManager.Outcome.Defend:
            {
                Melee attackerMeleeAction = (Melee)attackerAction.Action;
                Melee defenderMeleeAction = (Melee)defenderAction.Action;
                _teamManager.ApplyTeamStaminaChange(attackerAction.TeamID, attackerAction.PlayerID, -(int)attackerMeleeAction.StaminaCost);
                _teamManager.ApplyTeamStaminaChange(defenderAction.TeamID, defenderAction.PlayerID, -(int)(attackerMeleeAction.StaminaCost + defenderMeleeAction.DefensiveStaminaPenalty));
                PhaseSetup();
                break;
            }
            case TurnManager.Outcome.Hit:
            {
				Melee attackerMeleeAction = (Melee)attackerAction.Action;
                _teamManager.ApplyTeamStaminaChange(attackerAction.TeamID, attackerAction.PlayerID, -(int)attackerMeleeAction.StaminaCost);
                _teamManager.ApplyTeamHealthChange(defenderAction.TeamID, defenderAction.PlayerID, -(int)attackerMeleeAction.StaminaCost);
                PhaseSetup();
                break;
            }
            case TurnManager.Outcome.ItemUse:
            {
                if (defenderAction.Action.Type == Action.ActionType.ITEM)
                {
                    Item defenderItemAction = (Item)defenderAction.Action;
                    _teamManager.ApplyTeamHealthChange(defenderAction.TeamID, defenderAction.PlayerID, defenderItemAction.HealthEffect);
                    _teamManager.ApplyTeamStaminaChange(defenderAction.TeamID, defenderAction.PlayerID, defenderItemAction.StaminaEffect);
                    //Process debuff
                    _teamManager.ApplyTeamDebuff(attackerAction.TeamID, attackerAction.PlayerID, defenderItemAction.DebuffEffect);

                    if(attackerAction.Action.Type != Action.ActionType.ITEM) PhaseSetup();
                } 
                    
                if (attackerAction.Action.Type == Action.ActionType.ITEM)
                {
                    Item attackerItemAction = (Item)attackerAction.Action;
                    _teamManager.ApplyTeamHealthChange(attackerAction.TeamID, attackerAction.PlayerID, attackerItemAction.HealthEffect);
                    _teamManager.ApplyTeamStaminaChange(attackerAction.TeamID, attackerAction.PlayerID, attackerItemAction.StaminaEffect);
                    //Process debuff
                    _teamManager.ApplyTeamDebuff(defenderAction.TeamID, defenderAction.PlayerID, attackerItemAction.DebuffEffect);
                    SwapPhase();
                }
                else
                {
                    Melee attackerMeleeAction = (Melee)attackerAction.Action;
                    _teamManager.ApplyTeamStaminaChange(attackerAction.TeamID, attackerAction.PlayerID, -(int)attackerMeleeAction.StaminaCost);
                    _teamManager.ApplyTeamHealthChange(defenderAction.TeamID, defenderAction.PlayerID, -(int)attackerMeleeAction.StaminaCost);
                    PhaseSetup();
                }
                
                break;
            }
        }

        if (_hasToSwapPhase)
        {
            SwapPhase();
        }
    }

    private void DoCardActivate(TeamManager.ActionData actionData)
    {
        _turnManager.AddToResolver(actionData);
    }

    private void DoStaminaEmpty(int teamId)
    {
        if(_phaseManager.GetAttackingTeamIdx() == teamId)
        {
            _hasToSwapPhase = true;
        }
    }

    private void DoHealthEmpty(int teamId)
    {
        _endScreen.SetActive(true);
        _hasEnded = true;
        _teamManager.Clear();
    }
}
