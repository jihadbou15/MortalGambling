using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Team : MonoBehaviour
{
    public delegate void TeamCardHandler(Card.CardData cardData, int id, int playerId);
    public event TeamCardHandler OnCardActivate;

    private int _id = -1;
    private List<Player> _players;
    private int _playerAmount = 1;

    public void Initialize(int id, Player prefab, Card cardPrefab, Vector3 PlayerPosition)
    {
        _id = id;
        for (int i = 0; i < _playerAmount; i++)
        {
            Player newPlayer = GameObject.Instantiate(prefab);
            newPlayer.Initialize(i, cardPrefab);
            newPlayer.OnActivate += DoCardActivate;
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

    private void DoCardActivate(Card.CardData cardData, int playerId)
    {
        OnCardActivate?.Invoke(cardData, _id, playerId);
    }
}
