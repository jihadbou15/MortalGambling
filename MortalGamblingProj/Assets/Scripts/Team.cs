using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Team : MonoBehaviour
{
    public delegate void TeamCardHandler(Card.CardData cardData, int id, int playerId);
    public event TeamCardHandler OnCardActivate;

    public delegate void TeamPlayerEmpty(int teamId);
    public event TeamPlayerEmpty OnTeamPlayerHealthEmpty;
    public event TeamPlayerEmpty OnTeamPlayerStaminaEmpty;

    [SerializeField] private Player _playerPrefab = null;
    private int _id = -1;
    private List<Player> _players = new List<Player>();
    private int _playerAmount = 1;

    public void Initialize(int id)
    {
        _id = id;
        for (int i = 0; i < _playerAmount; i++)
        {
            Player newPlayer = GameObject.Instantiate(_playerPrefab);
            newPlayer.Initialize(i);
            newPlayer.OnActivate += DoCardActivate;
            newPlayer.OnPlayerHealthEmpty += OnPlayerHealthEmpty;
            _players.Add(newPlayer);
            newPlayer.transform.SetParent(gameObject.transform);
        }        
    }

    public void Tick()
    {

    }

    public void ApplyHealthChange(float healthChange, int playerIdx)
    {
        _players[playerIdx].OnHealthChange(healthChange);
    }

    public void ApplyStaminaChange(float staminaChange, int playerIdx)
    {
        _players[playerIdx].OnStaminaChange(staminaChange);
    }

    public void OnPlayerHealthEmpty()
    {
        OnTeamPlayerHealthEmpty.Invoke(_id);
    }

    public void OnPlayerStaminaEmpty()
    {
        OnTeamPlayerStaminaEmpty.Invoke(_id);
    }

    private void DoCardActivate(Card.CardData cardData, int playerId)
    {
        OnCardActivate?.Invoke(cardData, _id, playerId);
    }
}
