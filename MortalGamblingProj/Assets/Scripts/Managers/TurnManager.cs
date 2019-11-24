using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public delegate void TurnHandler();
    public event TurnHandler OnTurnEnd;

    public void Initialize()
    {

    }

    public void Tick()
    {

    }
}
