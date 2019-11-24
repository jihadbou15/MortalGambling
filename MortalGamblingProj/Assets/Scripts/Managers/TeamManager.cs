using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamManager : MonoBehaviour
{
    public delegate void TeamManagerCardHandler(List<KeyValuePair<Card.Target, int>> teamChoices);
    public event TeamManagerCardHandler OnCardActivate;

    [SerializeField] private int _teamAmount = 0;
    private List<Team> _teams = new List<Team>();

    private List<KeyValuePair<Card.Target, int>> _teamChoices = new List<KeyValuePair<Card.Target, int>>();

    public void Initialize()
    {
        for(int i = 0; i < _teamAmount; ++i)
        {
            Team newTeam = new Team();
            newTeam.Initialize(i);
            newTeam.OnCardActivate += DoCardActivate;
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

    private void DoCardActivate(Card.Target target, int id)
    {
        _teamChoices.Add(new KeyValuePair<Card.Target, int>( target, id));
    }
}
