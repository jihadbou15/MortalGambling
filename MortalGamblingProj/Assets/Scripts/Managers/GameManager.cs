using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private InputManager _inputManager = null;
    [SerializeField] private PhaseManager _phaseManager = null;
    [SerializeField] private TurnManager _turnManager = null;
    [SerializeField] private TeamManager _teamManager = null;

    void Start()
    {
        _inputManager.Initialize();
        _inputManager.KeyDown += OnKeyDown;

        _teamManager.Initialize();
        _teamManager.OnCardActivate += DoCardActivate;
        _teamManager.OnStaminaEmpty += DoStaminaEmpty;

        _phaseManager.Initialize(_teamManager.GetTeamAmount());
        _phaseManager.OnPhaseEnd += DoPhaseEnd;

        _turnManager.Initialize();
        _turnManager.OnTurnEnd += DoTurnEnd;
    }

    void Update()
    {
        _inputManager.Tick();
        _phaseManager.Tick();
        _turnManager.Tick();
        _teamManager.Tick();
    }

    private void OnKeyDown(KeyCode keyCode)
    {
        //Check for input here
    }

    private void DoPhaseEnd()
    {
        _phaseManager.SwapPhase();
    }

    private void DoTurnEnd(TurnManager.Outcome outcome, List<TeamManager.TeamChoiceData> teamChoices, int defenderIdx, int attackerIdx)
    {
        switch(outcome)
        {
            case TurnManager.Outcome.Parry:
            {
                _phaseManager.SwapPhase();
                break;
            }
            case TurnManager.Outcome.Defend:
            {
                _teamManager.ApplyTeamStaminaChange(defenderIdx, teamChoices[defenderIdx].PlayerIdx, (int)teamChoices[attackerIdx].CardData._baseDamage);
                break;
            }
            case TurnManager.Outcome.Hit:
            {
                _teamManager.ApplyTeamHealthChange(defenderIdx, teamChoices[defenderIdx].PlayerIdx, (int)teamChoices[attackerIdx].CardData._baseDamage);
                break;
            }
        }
    }

    private void DoCardActivate(List<TeamManager.TeamChoiceData> readyTeams)
    {
        _turnManager.ResolveTeams(readyTeams, _phaseManager.GetAttackingTeamIdx());
    }

    private void DoStaminaEmpty(int teamId)
    {
        if(_phaseManager.GetAttackingTeamIdx() == teamId)
        {
            _phaseManager.SwapPhase();
        }
    }
}
