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

        _phaseManager.Initialize();
        _phaseManager.OnPhaseEnd += DoPhaseEnd;

        _turnManager.Initialize();
        _turnManager.OnTurnEnd += DoTurnEnd;

        _teamManager.Initialize();
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

    }

    private void DoTurnEnd()
    {

    }
}
