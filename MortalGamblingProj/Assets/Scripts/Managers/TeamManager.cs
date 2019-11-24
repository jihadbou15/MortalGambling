using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamManager : MonoBehaviour
{
    public struct TeamChoiceData
    {
        public TeamChoiceData(Card.CardData cardData, int teamIdx, int playerIdx)
        {
            CardData = cardData;
            TeamIdx = teamIdx;
            PlayerIdx = playerIdx;
        }

        public Card.CardData CardData;
        public int TeamIdx;
        public int PlayerIdx;
    }

    public delegate void TeamManagerCardHandler(List<TeamChoiceData> teamChoices);
    public event TeamManagerCardHandler OnCardActivate;

    public delegate void TeamManagerStaminaEmpty(int teamIdx);
    public event TeamManagerStaminaEmpty OnStaminaEmpty;

    [SerializeField] private int _teamAmount = 0;
    [SerializeField] private Player _playerPrefab = null;
    [SerializeField] private Card _cardPrefab = null;
    [SerializeField] private List<Vector3> _teamPositions = new List<Vector3>();
    private List<Team> _teams = new List<Team>();

    private List<TeamChoiceData> _teamChoices = new List<TeamChoiceData>();

    public void Initialize()
    {
        for(int i = 0; i < _teamAmount; ++i)
        {
            Team newTeam = new Team();
            newTeam.Initialize(i, _playerPrefab, _cardPrefab, _teamPositions[i]);
            newTeam.OnCardActivate += DoCardActivate;
            newTeam.OnTeamPlayerStaminaEmpty += DoTeamPlayerStaminaEmpty;
            _teams.Add(newTeam);
        }
    }

    public void Tick()
    {
        foreach(Team team in _teams)
        {
            team.Tick();
        }

        if(_teamChoices.Count >= _teamAmount)
        {
            OnCardActivate?.Invoke(_teamChoices);
            _teamChoices.Clear();
        }
    }

    public int GetTeamAmount()
    {
        return _teamAmount;
    }

    public void ApplyTeamHealthChange(int teamIdx, int playerIdx, int healthChange)
    {
        _teams[teamIdx].ApplyHealthChange(healthChange, playerIdx);
    }

    public void ApplyTeamStaminaChange(int teamIdx, int playerIdx, int staminaChange)
    {
        _teams[teamIdx].ApplyHealthChange(staminaChange, playerIdx);
    }

    public void DoTeamPlayerStaminaEmpty(int id)
    {
        OnStaminaEmpty?.Invoke(id);
    }

    private void DoCardActivate(Card.CardData target, int id, int playerId)
    {
        _teamChoices.Add(new TeamChoiceData( target, id, playerId));
    }
}
