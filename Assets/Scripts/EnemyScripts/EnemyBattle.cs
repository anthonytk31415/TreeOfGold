using System.Collections;
using UnityEngine;

/*
This is instantiated with EnemyState and used in the CursorStateMachine system
to manage the enemy phase. EnemyState provides actions for doing enemy moves. 

*/


public class EnemyBattle{

    public GameManager instance; 
    public Board board; 

    public EnemyBattle(GameManager instance){
        this.instance = instance; 
        this.board = instance.board; 
    }

    public IEnumerator ApplyEnemyPhase(){
        Debug.Log("initiating apply enemy phase. ");
        GameObject[] charArray = instance.charArray; 
        foreach (GameObject character in charArray){
            CharacterGameState charGameState = character.GetComponent<CharacterGameState>();
            if (!charGameState.isYourTeam && charGameState.IsAlive){
                yield return PerformEnemyMove(character);
            }
        }
        yield return new WaitForSeconds(0.5f);
    }


    // if there is an player unit in the set and it is killable, attack it. Otherwise, 
    // attack the first unit. Otherwise, move max distance to the closest unit. 
    public IEnumerator PerformEnemyMove(GameObject character) {
        Debug.Log("doing move on : " + character);
        yield return new WaitForSeconds(0.5f);
    }

    



}


