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
    [SerializeField] private Team _teamPrefab = null;
    [SerializeField] private float _teamOffset = 0.0f;
    private List<Team> _teams = new List<Team>();

    private List<TeamChoiceData> _teamChoices = new List<TeamChoiceData>();

    public void Initialize()
    {
        GameObject canvas = GameObject.FindGameObjectWithTag("Canvas");
        for(int i = 0; i < _teamAmount; ++i)
        {
            Team newTeam = GameObject.Instantiate(_teamPrefab);
            newTeam.Initialize(i);

            newTeam.OnCardActivate += DoCardActivate;
            newTeam.OnTeamPlayerStaminaEmpty += DoTeamPlayerStaminaEmpty;

            newTeam.transform.SetParent(canvas.transform);
            newTeam.SetPosition(new Vector2(
                Mathf.Cos(i * (2.0f * Mathf.PI / _teamAmount) - Mathf.PI / 2.0f) * _teamOffset, 
                Mathf.Sin(i * (2.0f * Mathf.PI / _teamAmount) - Mathf.PI / 2.0f) * _teamOffset));
            newTeam.SetRotation(i * (360.0f / _teamAmount));
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
        _teams[teamIdx].ApplyStaminaChange(staminaChange, playerIdx);
    }

    public void RechargeEveryoneStamina()
    {
        foreach(Team team in _teams)
        {
            team.RechargeTeamStamina();
        }
    }

    public void DoTeamPlayerStaminaEmpty(int id)
    {
        OnStaminaEmpty?.Invoke(id);
    }

    public void EnableTeamCardInput(bool isEnabled, int idx)
    {
        _teams[idx].EnableTeamCardInput(isEnabled);
    }

    private void DoCardActivate(Card.CardData target, int id, int playerId)
    {
        _teamChoices.Add(new TeamChoiceData( target, id, playerId));
        _teams[(id - 1) * -1].EnableTeamCardInput(true);
        if(_teamChoices.Count == 1)
        {
            _teams[id].ApplyStaminaChange(-target.StaminaCost, playerId);
        }
    }
}
