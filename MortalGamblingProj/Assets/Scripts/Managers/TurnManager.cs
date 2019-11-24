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

    public void ResolveTeams(List<KeyValuePair<Card.Target, int>> teamChoices, int attackingTeamIdx)
    {
        foreach(KeyValuePair<Card.Target, int> teamChoice in teamChoices)
        {
            
        }

        int difference = Mathf.Abs((int)teamChoices[0].Key - (int)teamChoices[1].Key);

        if (difference == 0)
        {
            //Parry
        }
        else if(difference == 1)
        {
            //Defend
        }
        else if(difference == 2)
        {
            //Hit
        }
    }
}
