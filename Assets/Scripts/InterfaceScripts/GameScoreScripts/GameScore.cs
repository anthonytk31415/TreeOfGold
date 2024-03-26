using System;
using System.Collections.Generic;
using Codice.CM.Client.Differences.Merge;
using UnityEngine;

public class GameScore 
{
    private int totalPlayerUnits;
    private int totalEnemyUnits;

    private int currentPlayerUnits; 
    private int currentEnemyUnits; 

    public GameScore(GameObject[] charArray){
        (currentPlayerUnits, currentEnemyUnits) = countUnitTypes(charArray);        
        totalPlayerUnits = currentPlayerUnits; 
        totalEnemyUnits = currentEnemyUnits; 
    }

    private (int, int) countUnitTypes(GameObject[] charArray){
        int curPlayers = 0;
        int curEnemies = 0;        
        foreach(GameObject unit in charArray){
            if (unit.GetComponent<CharacterGameState>().isYourTeam){
                curPlayers += 1;
            } else {
                curEnemies += 1;
            }
        }
        return (curPlayers, curEnemies); 
    } 

    public void removePlayerUnit(){
        currentPlayerUnits -= 1;
    }

    public void removeEnemyUnit(){
        currentEnemyUnits -= 1; 
    }
    public int GetCurrentEnemyUnits() {
        return currentEnemyUnits; 
    }


    public Boolean IsATeamDefeated(){
        return currentEnemyUnits == 0 || currentPlayerUnits == 0;
    }
    public Boolean DidPlayerWin(){
        return currentEnemyUnits == 0;
    }
    public Boolean DidEnemyWin(){
        return currentPlayerUnits == 0; 
    }

    public void AuditScore(){
        Debug.Log(String.Format("player score:  {0}, enemy score: {1}", currentPlayerUnits, currentEnemyUnits));
    }

}