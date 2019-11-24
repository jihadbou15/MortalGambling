using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Team : MonoBehaviour
{
    public delegate void TeamCardHandler(Card.Target target, int id);
    public event TeamCardHandler OnCardActivate;

    private int _id = -1;

    public void Initialize(int id)
    {
        _id = id;
    }

    public void Tick()
    {

    }

    private void DoCardActivate(Card.Target target)
    {
        OnCardActivate?.Invoke(target, _id);
    }
}
