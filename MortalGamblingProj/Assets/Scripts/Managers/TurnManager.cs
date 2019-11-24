﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public enum Outcome
    {
        Parry,
        Defend,
        Hit
    }

    public delegate void TurnHandler(Outcome outcome, List<TeamManager.TeamChoiceData> teamChoices, int defenderIdx, int attackerIdx);
    public event TurnHandler OnTurnEnd;

    public void Initialize()
    {

    }

    public void Tick()
    {

    }

    public void ResolveTeams(List<TeamManager.TeamChoiceData> teamChoices, int attackingTeamIdx)
    {
        int defendingTeamIdx = 0;
        if(attackingTeamIdx == defendingTeamIdx)
        {
            defendingTeamIdx++;
        }
        
        if(teamChoices[attackingTeamIdx].CardData._type == Card.Type.Melee &&
            teamChoices[defendingTeamIdx].CardData._type == Card.Type.Melee)
        {
            int difference = Mathf.Abs((int)teamChoices[0].CardData._target - (int)teamChoices[1].CardData._target);

            if (difference == 0)
            {
                OnTurnEnd?.Invoke(Outcome.Parry, teamChoices, defendingTeamIdx, attackingTeamIdx);
            }
            else if (difference == 1)
            {
                OnTurnEnd?.Invoke(Outcome.Defend, teamChoices, defendingTeamIdx, attackingTeamIdx);
            }
            else if (difference == 2)
            {
                OnTurnEnd?.Invoke(Outcome.Hit, teamChoices, defendingTeamIdx, attackingTeamIdx);
            }
        }
        else if(teamChoices[attackingTeamIdx].CardData._type == Card.Type.Magic &&
            teamChoices[defendingTeamIdx].CardData._type == Card.Type.Magic)
        {

        }
        else if (teamChoices[attackingTeamIdx].CardData._type == Card.Type.Item &&
            teamChoices[defendingTeamIdx].CardData._type == Card.Type.Item)
        {

        }
    }
}
