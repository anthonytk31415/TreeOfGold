// how am i going to write this: 


// Best Move
// most to first enemy as far as possible
// if in position, attack

// collect enemies on the board. 
// put them in an array
// iterate across the array. 

using UnityEngine;

public class EnemyBattle{

    public GameManager instance; 
    public Board board; 

    public EnemyBattle(GameManager instance){
        this.instance = instance; 
        this.board = instance.board; 
    }

    public void ApplyEnemyPhase(){
        GameObject[] charArray = instance.charArray; 
        foreach (GameObject character in charArray){
            CharacterGameState charGameState = character.GetComponent<CharacterGameState>();
            if (!charGameState.isYourTeam && charGameState.IsAlive){
                PerformEnemyMove(character);
            }
        }
    }

    public void PerformEnemyMove(GameObject character) {
        Debug.Log("doing move on : ", character);
    }

    



}


