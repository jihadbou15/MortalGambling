using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Team : MonoBehaviour
{
    public delegate void TeamCardHandler(Card.CardData cardData, int id);
    public event TeamCardHandler OnCardActivate;

    private int _id = -1;
    private List<Player> _players;

    public void Initialize(int id)
    {
        _id = id;
    }

    public void Tick()
    {

    }

    public void ApplyDamage(float damage, int playerIdx)
    {

    }

    private void DoCardActivate(Card.CardData cardData)
    {
        OnCardActivate?.Invoke(cardData, _id);
    }
}
