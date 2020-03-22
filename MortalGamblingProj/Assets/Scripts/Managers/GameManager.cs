using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private InputManager _inputManager = null;
    [SerializeField] private PhaseManager _phaseManager = null;
    [SerializeField] private TurnManager _turnManager = null;
    [SerializeField] private TeamManager _teamManager = null;
    [SerializeField] private GameObject _endScreen = null;

    private bool _hasEnded = false;

    void Start()
    {
        //Initialize all managers
        _inputManager.Initialize();
        _teamManager.Initialize(_phaseManager,_turnManager);
        _teamManager.OnHealthEmpty += DoHealthEmpty;

        int teamAmount = _teamManager.GetTeamAmount();
        _phaseManager.Initialize(teamAmount,_turnManager,_teamManager);
        _turnManager.Initialize(teamAmount,_teamManager,_phaseManager);

        //Initialize Phase Setup
        _phaseManager.PhaseSetup();
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

    private void DoHealthEmpty(int teamId)
    {
        _endScreen.SetActive(true);
        _hasEnded = true;
        _teamManager.Clear();
    }
}
