using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageMAnager : MonoBehaviour
{
    private TurnManager turnManager;
    private PhaseManager phaseManager;

    public enum PhaseStage
    {
        Setup,
        InPhase,
        SwapPhase
    }

    private enum TurnStage
    {
        Setup,
        ChooseAction,
        AddToResolver,
        SwapInput,
        TurnResolve,
        TurnEnd
    }

    public void Initialize(TurnManager turnManager,PhaseManager phaseManager)
    {
        
    }



    public void Tick()
    {
        
    }
}
