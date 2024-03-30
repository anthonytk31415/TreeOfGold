using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor.PackageManager;

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
        GameObject[] charArray = instance.charArray; 
        foreach (GameObject character in charArray){
            CharacterGameState charGameState = character.GetComponent<CharacterGameState>();
            if (!charGameState.isYourTeam && charGameState.IsAlive){
                yield return PerformEnemyMove(character);
                if (instance.gameStateMachine.endGame){
                    break;
                }
            }
        }
        yield return new WaitForSeconds(0.5f);
    }

    // if there is an player unit in the set and it is killable, attack it. Otherwise, 
    // attack the first unit. Otherwise, move max distance to the closest unit. 
    public IEnumerator PerformEnemyMove(GameObject character) {
        // ** highlight character

        // get position, target
        EnemyMove curMove = GetBestMove(character);

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

            foreach (Coordinate potentialEnemyTarget in enemies) {
                if (!enemyTargets.Contains(potentialEnemyTarget)){
                    enemyTargets.Add(potentialEnemyTarget);
                    GameObject opponent = instance.charArray[instance.board.Get(potentialEnemyTarget)]; 
                    // add to the queue the EnemyMove target
                    int opponentHPAfterAttack = opponent.GetComponent<CharacterGameState>().HPAfterAttack(charAttack);
                    enemyMoves.Add((opponentHPAfterAttack, new EnemyMove(instance, character, possibleMove, potentialEnemyTarget, true)));
                } 
            }
        }
        // if you have moves where you can attack, pick the one with the opponent's least ending-hp
        if (enemyMoves.Count > 0){
            enemyMoves.Sort((x, y) => x.Item1.CompareTo(y.Item1)); 
            return enemyMoves[0].Item2; 
        }
        // otherwise, stay still (for now); this is buggy. lets give the position an entry. 
        else {
            // Debug.Log("no actual attacks, now move closest");
            List<Coordinate> closestEnemyPath = CharInteraction.FindClosestOpponent(instance, initialPos);
            // Debug.Log("length of closest path: " + closestEnemyPath.Count);
            AuditDebug.DebugIter(closestEnemyPath);
            if (closestEnemyPath.Count == 0) {
                return new EnemyMove(instance, character, initialPos, null, false);
            }
            else {
                return new EnemyMove(instance, character, closestEnemyPath[moves-1], null, false);
            }            
        }
    }


    public IEnumerator DoMove(EnemyMove move){
        // Debug.Log("current move: " + move);        
        int charId = move.character.GetComponent<CharacterGameState>().charId; 
        // Debug.Log("with charId: " + charId);
        // Debug.Log("move triggered with: " + move.posUnit + "on char:" + move.character );
        move.character.GetComponent<CharacterMove>().MoveChar(move.posUnit);
        if (move.attack){
            int enemyId = instance.board.Get(move.posTarget); 
            yield return move.character.GetComponent<CharacterBattle>().PlayerAttackChain(charId, enemyId, instance);
        } 
        yield return null;

        // later, apply the UI above plates effects

    }


}


