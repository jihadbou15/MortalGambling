using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamManager : MonoBehaviour
{
    public delegate void TeamManagerCardHandler(Dictionary<Card.Target, int>);
    public event TeamManagerCardHandler OnCardActivate;

    [SerializeField] private int _teamAmount = 0;
    private List<Team> _teams = new List<Team>();

    private Dictionary<Card.Target, int> _readyTeams = new Dictionary<Card.Target, int>();

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
        if(_readyTeams.Count >= _teamAmount)
        {
            OnCardActivate?.Invoke(_readyTeams);
            _readyTeams.Clear();
        }
    }

    private void DoCardActivate(Card.Target target, int id)
    {
        _readyTeams.Add(target, id);
    }
}
