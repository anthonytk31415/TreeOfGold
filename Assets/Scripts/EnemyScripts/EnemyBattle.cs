using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; 

/*
This is instantiated with EnemyState and used in the CursorStateMachine system
to manage the enemy phase. EnemyState provides actions for doing enemy moves. 

*/


public class EnemyBattle {

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
                if (instance.cursorStateMachine.endGame){
                    Debug.Log("this is the end of the game");
                    break;
                }
            }
        }
        yield return new WaitForSeconds(0.5f);
    }

    // if there is an player unit in the set and it is killable, attack it. Otherwise, 
    // attack the first unit. Otherwise, move max distance to the closest unit. 
    public IEnumerator PerformEnemyMove(GameObject character) {
        Debug.Log("doing move on : " + character);

        // ** highlight character

        // get position, target
        EnemyMove curMove = GetBestMove(character);
        Debug.Log("best move: " + curMove);

        // execute the move
        yield return DoMove(curMove);
        yield return new WaitForSeconds(0.5f);
    }

    private EnemyMove GetBestMove(GameObject character) {
        // get enemies in range; 
        int charId = character.GetComponent<CharacterGameState>().charId; 
        int range = character.GetComponent<CharacterStats>().attackRange; 
        int moves = character.GetComponent<CharacterStats>().moves; 
        int charAttack = character.GetComponent<CharacterStats>().attack;
        Coordinate initialPos = instance.board.FindCharId(charId);

        // get possible moves. 
        HashSet<Coordinate> possibleMoves = CharInteraction.PlayerMoveOptions(initialPos, moves, instance); 

        // note!!! for testing, you could apply the move, but only at the model level, perhaps?

        HashSet<Coordinate> enemyTargets = new(); 
        List<(int, EnemyMove)> enemyMoves = new();
        foreach(Coordinate possibleMove in possibleMoves){
            HashSet<Coordinate> enemies = CharInteraction.EnemiesWithinAttackRange(instance, character, possibleMove, range);                       

            foreach (Coordinate potentialTarget in enemies) {
                if (!enemyTargets.Contains(potentialTarget)){
                    enemyTargets.Add(potentialTarget);
                    GameObject opponent = instance.charArray[instance.board.Get(potentialTarget)]; 
                    // add to the queue the EnemyMove target
                    int opponentHPAfterAttack = opponent.GetComponent<CharacterGameState>().HPAfterAttack(charAttack);
                    EnemyMove curEnemyMove = new EnemyMove(instance, character, possibleMove, potentialTarget); 
                    if (opponentHPAfterAttack == 0){
                        // return curEnemyMove; // figure out how to return this value 
                    }
                    enemyMoves.Add((opponentHPAfterAttack, new EnemyMove(instance, character, possibleMove, potentialTarget)));
                } 
            }
        }
        if (enemyMoves.Count > 0){
            enemyMoves.Sort((x, y) => x.Item1.CompareTo(y.Item1)); 
            return enemyMoves[0].Item2; 
        }
        else {
            // for now, just stay still. 
            return new EnemyMove(instance, character, initialPos, null);
        }
    }


    public IEnumerator DoMove(EnemyMove move){
        int charId = move.character.GetComponent<CharacterGameState>().charId; 
        int enemyId = instance.board.Get(move.posTarget); 
        move.character.GetComponent<CharacterMove>().MoveChar(move.posUnit);
        yield return move.character.GetComponent<CharacterBattle>().PlayerAttackChain(charId, enemyId, instance);

        // later, apply the UI above plates effects

    }


}


