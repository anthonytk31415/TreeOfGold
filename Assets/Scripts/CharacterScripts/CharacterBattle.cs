using System;
using UnityEngine;
using System.Collections;

using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity;




// this controller CharacterBattle updates both the model and the UI. Is this not a good separation of concerns? 
/// <summary>
/// CharacterBattle handles the management of the event when a player does something to a unit (attack, move, etc.). 
/// From these events, it kicks off: 
/// (1) moving the character
/// (2) updating menus and health bar ui
/// (3) updating cursor 
/// </summary>
public class CharacterBattle : MonoBehaviour
{

    public void Start(){
    }

    /// <summary>
    /// CommenceBattle calls the battle coroutines. This method is used to initiate the battle sequence.
    /// </summary>
    public void CommenceBattle(int playerId, int enemyId, GameManager instance){
        StartCoroutine(PlayerAttackChain(playerId, enemyId, instance));
    }



    /// <summary>
    /// IEnumerator - triggers when player initiates attack. Sequence: 
    /// player Attack enemy > if enemy still alive, enemy attack player
    /// This function calls the player animation, then the calls to the UI components  
    /// </summary> 
    public IEnumerator PlayerAttackChain(int playerId, int enemyId, GameManager instance){
        GameObject player = instance.charArray[playerId]; 
        GameObject enemy = instance.charArray[enemyId]; 
        Board board = instance.board; 

        yield return PlayerAttackEnemy(playerId, enemyId, instance, board);        
        if (enemy.GetComponent<CharacterGameState>().IsAlive){
            yield return PlayerAttackEnemy(enemyId, playerId, instance, board);            
        }
        player.GetComponent<CharacterGameState>().HasAttacked = true;

        yield return null;
    }

    /// <summary>
    /// Performs attack between unit and opponent;  
    /// This handles both View and model attack implementation
    /// </summary>
    public IEnumerator PlayerAttackEnemy(int playerId, int enemyId, GameManager instance, Board board){
        GameObject player = instance.charArray[playerId]; 
        GameObject enemy = instance.charArray[enemyId]; 
        int playerAttack = player.GetComponent<CharacterStats>().attack; 

        // View implementation
        yield return instance.battleEffectsManagerObject.GetComponent<BattleEffectsManager>().TriggerBlackThenRed(player, enemy); 

        // Model implementation
        enemy.GetComponent<CharacterGameState>().DecreaseHp(playerAttack);
        yield return enemy.GetComponentInChildren<HealthBarManager>().UpdateHealthBar(enemy);

        if (!enemy.GetComponent<CharacterGameState>().IsAlive){
            yield return ApplyDeathSequence(enemyId, instance, board);
        }

        instance.statMenuController.GetComponent<StatMenuManager>().UpdateUnitScreen();
        instance.moveControllerObject.GetComponent<MoveController>().ResetSelected();
        yield return AssessWinner(instance);
    }
    /// <summary> 
    /// Animation for applying death sequence. as well as apply death to model
    /// </summary>
    public IEnumerator ApplyDeathSequence(int unitId, GameManager instance, Board board){
        Coordinate w = board.FindCharId(unitId);     
        board.PutEmpty(w);
        GameObject unit = instance.charArray[unitId];
        unit.GetComponent<CharacterGameState>().IsAlive = false;
        if (unit.GetComponent<CharacterGameState>().isYourTeam){
            instance.gameScore.removePlayerUnit();
        } else {
            instance.gameScore.removeEnemyUnit();
        } 
        yield return instance.battleEffectsManagerObject.GetComponent<BattleEffectsManager>().DeathFadeout(unit); 
        unit.SetActive(false); 
        yield return null; 
    }
    /// <summary> 
    /// call this after each battle to transition the game to the EndGameState if a team is defeated
    /// </summary> 
    public IEnumerator AssessWinner(GameManager instance){        
        if (instance.gameScore.IsATeamDefeated()){
            instance.gameStateMachine.TriggerEndGame();           
            yield break;
        }
        yield return null; 

    }
}