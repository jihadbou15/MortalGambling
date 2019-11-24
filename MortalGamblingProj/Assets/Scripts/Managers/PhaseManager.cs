using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseManager : MonoBehaviour
{
    public delegate void PhaseHandler();
    public event PhaseHandler OnPhaseEnd;

    private int _teamAmount = -1;
    private int _attackTeamIdx = -1;

    public void Initialize(int teamAmount)
    {
        _teamAmount = teamAmount;
        _attackTeamIdx = Random.Range(0, teamAmount);
    }

    public void Tick()
    {

    }

    public int GetAttackingTeamIdx()
    {
        return _attackTeamIdx;
    }

    public void SwapPhase()
    {
        ++_attackTeamIdx;
        if(_attackTeamIdx >= _teamAmount)
        {
            _attackTeamIdx = 0;
        }
    }
}
