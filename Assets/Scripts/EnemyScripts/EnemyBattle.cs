using System.Collections;
using System.Collections.Generic;
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

        // highlight character


        // get position, target
        GetBestMove(character);


        // execute the move

        yield return new WaitForSeconds(0.5f);
    }
        // HashSet<Coordinate> possibleAttackTargets = CharInteraction.EnemiesWithinAttackRange(gameManager, initialPos, range); 
    private void GetBestMove(GameObject character) {
        // get enemies in range; 
        int charId = character.GetComponent<CharacterGameState>().charId; 
        int range = character.GetComponent<CharacterStats>().attackRange; 
        Coordinate w = instance.board.FindCharId(charId);
        HashSet<Coordinate> possibleAttackTargets = CharInteraction.EnemiesWithinAttackRange(instance, w, range); 

        // if its empty then return move to the closest enemy. 
        if (possibleAttackTargets.Count == 0){
            Debug.Log("No targets");
            return; 
        }
        Debug.Log("there are targets");
        return;
        // otherwise, of all the enemies within range, find the best one. 

        // start a priority queue sorted by smallest damage after attack sequence 
        // then smallest distance to nearest opposing team unit

        // for each move, create a targets set
    }


}


