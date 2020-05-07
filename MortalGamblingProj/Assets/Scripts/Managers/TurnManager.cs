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

            if (defenderTarget == GetsParriedBy)
            {
                return Outcome.Parry;
            }
            else 
            {
                foreach(Melee.Target target in GetsHitOn)
                {
                    if (defenderTarget == target) return Outcome.Hit;
                }
            }

            return Outcome.Defend;
        }

        public Melee.Target Attacks;
        public Melee.Target GetsParriedBy;
        public List<Melee.Target> GetsHitOn;
    }

    [SerializeField] private MeleeResolver _resolver;

    private List<TeamManager.ActionData> _attackerActions = new List<TeamManager.ActionData>();
    private List<TeamManager.ActionData> _defenderActions = new List<TeamManager.ActionData>();
    private int _actionAmount = 0;
    private int _actionCounter = 0;
    private int _teamAmount = 0;
    private int _teamCounter = 0;
    private bool _isAttacking = false;

    private TeamManager _teamManager;
    private PhaseManager _phaseManager;


    public void Initialize(int teamAmount, TeamManager teamManager, PhaseManager phaseManager)
    {
        _teamManager = teamManager;
        _phaseManager = phaseManager;
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

    public void ResetTurns ()
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
                DoTurnStart();
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
                    DoTurnStart();
                }
            }
        }

        if(_teamCounter >= _teamAmount)
        {
            ResolveTeams();
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
            //Add potential magic system
        }
        else if(attackerType == Action.ActionType.ITEM ||
            defenderType == Action.ActionType.ITEM)
        {
            DoApplyTurnOutcome(Outcome.ItemUse, _attackerActions[0], _defenderActions[0]);
        }
    }

    public void ResolveMelee()
    {
        Melee.Target attackerTarget = ((Melee)_attackerActions[0].Action).MeleeTarget;
        Melee.Target defenderTarget = ((Melee)_defenderActions[0].Action).MeleeTarget;
        
        Outcome outcome = _resolver.GetOutcome(attackerTarget, defenderTarget);
        DoApplyTurnOutcome(outcome, _attackerActions[0], _defenderActions[0]);
    }

    private void DoApplyTurnOutcome(Outcome outcome, TeamManager.ActionData attackerAction, TeamManager.ActionData defenderAction)
    {

        switch (outcome)
        {
            case Outcome.Parry:
                {
                    Melee attackerMeleeAction = (Melee)attackerAction.Action;
                    _teamManager.ApplyTeamStaminaChange(attackerAction.TeamID, attackerAction.PlayerID, -(int)attackerMeleeAction.StaminaCost);
                    _phaseManager.EnablePhaseSwapForNextSetup();
                    break;
                }
            case Outcome.Defend:
                {
                    Melee attackerMeleeAction = (Melee)attackerAction.Action;
                    Melee defenderMeleeAction = (Melee)defenderAction.Action;
                    _teamManager.ApplyTeamStaminaChange(attackerAction.TeamID, attackerAction.PlayerID, -(int)attackerMeleeAction.StaminaCost);
                    _teamManager.ApplyTeamStaminaChange(defenderAction.TeamID, defenderAction.PlayerID, -(int)(attackerMeleeAction.StaminaCost + defenderMeleeAction.DefensiveStaminaPenalty));
                    break;
                }
            case Outcome.Hit:
                {
                    Melee attackerMeleeAction = (Melee)attackerAction.Action;
                    _teamManager.ApplyTeamStaminaChange(attackerAction.TeamID, attackerAction.PlayerID, -(int)attackerMeleeAction.StaminaCost);
                    _teamManager.ApplyTeamHealthChange(defenderAction.TeamID, defenderAction.PlayerID, -(int)attackerMeleeAction.StaminaCost);
                    break;
                }
            case Outcome.ItemUse:
                {
                    if (defenderAction.Action.Type == Action.ActionType.ITEM)
                    {
                        Item defenderItemAction = (Item)defenderAction.Action;
                        _teamManager.ApplyTeamHealthChange(defenderAction.TeamID, defenderAction.PlayerID, defenderItemAction.HealthEffect);
                        _teamManager.ApplyTeamStaminaChange(defenderAction.TeamID, defenderAction.PlayerID, defenderItemAction.StaminaEffect);
                        //Process debuff
                        _teamManager.ApplyTeamDebuff(attackerAction.TeamID, attackerAction.PlayerID, defenderItemAction.DebuffEffect);
                    }

                    if (attackerAction.Action.Type == Action.ActionType.ITEM)
                    {
                        Item attackerItemAction = (Item)attackerAction.Action;
                        _teamManager.ApplyTeamHealthChange(attackerAction.TeamID, attackerAction.PlayerID, attackerItemAction.HealthEffect);
                        _teamManager.ApplyTeamStaminaChange(attackerAction.TeamID, attackerAction.PlayerID, attackerItemAction.StaminaEffect);
                        //Process debuff
                        _teamManager.ApplyTeamDebuff(defenderAction.TeamID, defenderAction.PlayerID, attackerItemAction.DebuffEffect);
                        _phaseManager.EnablePhaseSwapForNextSetup();
                    }
                    else
                    {
                        Melee attackerMeleeAction = (Melee)attackerAction.Action;
                        _teamManager.ApplyTeamStaminaChange(attackerAction.TeamID, attackerAction.PlayerID, -(int)attackerMeleeAction.StaminaCost);
                        _teamManager.ApplyTeamHealthChange(defenderAction.TeamID, defenderAction.PlayerID, -(int)attackerMeleeAction.StaminaCost);
                    }
                    break;
                }
        }

        _phaseManager.PhaseSetup();

    }

    public void DoTurnStart()
    {
        int activeID = _phaseManager.GetAttackingTeamIdx();
        int teamAmount = _teamManager.GetTeamAmount();

        //WHAT THE FUCK IS THIS SHIT
        activeID += _teamCounter;
        if (activeID >= teamAmount)
        {
            activeID = 0;
        }

        _teamManager.EnableTeamCardInput(activeID);
    }
}
