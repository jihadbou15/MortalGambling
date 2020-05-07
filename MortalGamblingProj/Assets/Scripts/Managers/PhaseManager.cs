using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseManager : MonoBehaviour
{
    private int _teamAmount = -1;
    private int _attackTeamIdx = -1;

    private TeamManager _teamManager;
    private TurnManager _turnManager;
    private bool _hasToSwapPhase;

    public void Initialize(int teamAmount, TurnManager turnManager,TeamManager teamManager)
    {
        _teamAmount = teamAmount;
        _attackTeamIdx = Random.Range(0, teamAmount);
        _teamManager = teamManager;
        _turnManager = turnManager;
    }

    public void Tick()
    {

    }

    public int GetAttackingTeamIdx()
    {
        return _attackTeamIdx;
    }

    public void PhaseSetup()
    {
        if (_hasToSwapPhase) SwapPhase();
        int attackingTeamId = GetAttackingTeamIdx();
        _teamManager.CheckTeamDebuffs();

        for (int i = 0; i < _teamManager.GetTeamAmount(); ++i)
        {
            if (i == attackingTeamId)
            {
                _teamManager._enabledTeamID = i;
                _teamManager.SetPhaseFeedback(true, i);
            }
            else _teamManager.SetPhaseFeedback(false, i);
        }

        _turnManager.SetActionAmount(_teamManager.GetPlayerAmount(attackingTeamId));

        _turnManager.ResetTurns();
        _turnManager.DoTurnStart();

    }

    public void EnablePhaseSwapForNextSetup()
    {
        _hasToSwapPhase = true;
    }

    private void SwapPhase()
    {
        ++_attackTeamIdx;
        if(_attackTeamIdx >= _teamAmount)
        {
            _attackTeamIdx = 0;
        }
        _teamManager.RechargeEveryoneStamina();

        _hasToSwapPhase = false;
    }


}
