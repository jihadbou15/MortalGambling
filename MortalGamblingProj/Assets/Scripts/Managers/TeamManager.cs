using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamManager : MonoBehaviour
{
    public struct ActionData
    {
        public Action Action;
        public int TeamID;
        public int PlayerID;

        public ActionData(Action action, int teamID, int playerID)
        {
            Action = action;
            TeamID = teamID;
            PlayerID = playerID;
        }
    }

    public delegate void TeamManagerCardHandler(ActionData actionData);
    public event TeamManagerCardHandler OnCardActivate;

    public delegate void TeamManagerEmpty(int teamIdx);
    public event TeamManagerEmpty OnStaminaEmpty;
    public event TeamManagerEmpty OnHealthEmpty;

    [SerializeField] private int _teamAmount = 0;
    [SerializeField] private Team _teamPrefab = null;
    [SerializeField] private float _teamOffset = 0.0f;
    private List<Team> _teams = new List<Team>();

    public void Initialize()
    {
        GameObject canvas = GameObject.FindGameObjectWithTag("Canvas");
        for(int i = 0; i < _teamAmount; ++i)
        {
            Team newTeam = GameObject.Instantiate(_teamPrefab);
            newTeam.Initialize(i);

            newTeam.OnCardActivate += DoCardActivate;
            newTeam.OnTeamPlayerStaminaEmpty += DoTeamPlayerStaminaEmpty;
            newTeam.OnTeamPlayerHealthEmpty += DoTeamPlayerHealthEmpty;

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
    }

    public int GetTeamAmount()
    {
        return _teamAmount;
    }

    public int GetPlayerAmount(int teamId)
    {
        return _teams[teamId].GetPlayerAmount();
    }

    public void ApplyTeamHealthChange(int teamIdx, int playerIdx, float healthChange)
    {
        _teams[teamIdx].ApplyHealthChange(healthChange, playerIdx);
    }

    public void ApplyTeamStaminaChange(int teamIdx, int playerIdx, float staminaChange)
    {
        _teams[teamIdx].ApplyStaminaChange(staminaChange, playerIdx);
    }

    public void ApplyTeamDebuff(int TeamIdx, int playerIdx, Player.Debuff debuffToApply)
    {
        _teams[TeamIdx].ApplyDebuffChange(debuffToApply, playerIdx);
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

    public void DoTeamPlayerHealthEmpty(int id)
    {
        OnHealthEmpty?.Invoke(id);
    }

    public void EnableTeamCardInput(bool isEnabled, int idx)
    {
        _teams[idx].EnableTeamCardInput(isEnabled);
    }

    public void CheckTeamDebuffs(int idx)
    {
        _teams[idx].CheckTeamDebuff();
    }

    public void Clear()
    {
        foreach(Team team in _teams)
        {
            Destroy(team.gameObject);
        }
    }

    private void DoCardActivate(Action action, int teamId, int playerId)
    {
        OnCardActivate?.Invoke(new ActionData(action, teamId, playerId));
    }

    public void SetPhaseFeedback(bool isAttacking, int teamid)
    {
        _teams[teamid].SetPhaseSprite(isAttacking);
    }
}
