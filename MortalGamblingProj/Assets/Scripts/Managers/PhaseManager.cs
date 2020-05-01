using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseManager : MonoBehaviour
{
    private int _teamAmount = -1;
    private int _attackTeamIdx = -1;

    private TeamManager _teamManager;
    private TurnManager _turnManager;
    public bool _hasToSwapPhase;


    public enum PhaseStage
    {
        Setup,
        InPhase,
        PhaseEnd
    }

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
        int attackingTeamId = GetAttackingTeamIdx();
        _teamManager.CheckTeamDebuffs();

        for (int i = 0; i < _teamManager.GetTeamAmount(); ++i)
        {
            bool enable = false;
            if (i == attackingTeamId)
            {
                enable = true;
                _teamManager._enabledTeamID = i;
                _teamManager.SetPhaseFeedback(true, i);
            }
            else _teamManager.SetPhaseFeedback(false, i);

            _teamManager.EnableTeamCardInput(enable, i);

        }

        _turnManager.SetActionAmount(_teamManager.GetPlayerAmount(attackingTeamId));
    }

    public void SwapPhase()
    {
        _turnManager.ResetTurns();

        ++_attackTeamIdx;
        if(_attackTeamIdx >= _teamAmount)
        {
            _attackTeamIdx = 0;
        }
        DoPhaseEnd();

        _hasToSwapPhase = false;
    }

    public void DoPhaseEnd()
    {
        _teamManager.RechargeEveryoneStamina();
        PhaseSetup();
    }
}
