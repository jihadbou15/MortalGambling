using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    private TurnManager _turnManager;
    private PhaseManager _phaseManager;

    PhaseStage _phaseStage;
    TurnStage _turnStage;

    List<StageExecutions> stages;

    public enum PhaseStage
    {
        Setup, //
        InPhase, //
        SwapPhase, // only on swap
    }

    private enum TurnStage
    {
        Setup,
        ChooseAction,
        AddToResolver,
        SwapInput, TurnResolve,
        TurnEnd
    }

    public void Initialize(TurnManager turnManager,PhaseManager phaseManager)
    {
        _turnManager = turnManager;
        _phaseManager = phaseManager;
    }

    public void StartCombat()
    {
        _phaseStage = PhaseStage.Setup;
    }

    private void InCombat()
    {

    }

    public void Tick()
    {
        
    }

    private void RunStage()
    {

    }
}
