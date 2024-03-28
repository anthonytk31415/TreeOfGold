using System;
using UnityEngine;
using System.Collections;

using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity;

// CharacterBattle handles the management of the event when a player does something to a unit (attack, move, etc.). 
// From these events, it kicks off: 
// - moving the character
// - updating menus and health bar ui
// - updating cursor 


// note as of feb 18: the async functions work! might need some cleanup, but the logic is there!
// yield return is the way to go!

// this controller CharacterBattle updates both the model and the UI. Is this not a good separation of concerns? 

public class CharacterBattle : MonoBehaviour
{

    public void Start(){
    }


    public void CommenceBattle(int playerId, int enemyId, GameManager instance){
        StartCoroutine(PlayerAttackChain(playerId, enemyId, instance));
    }

    // triggers when player initiates attack. Sequence: 
    // player Attack enemy > if enemy still alive, enemy attack player
    // This function calls the player animation, then the calls to the UI components 

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

    // This purely does the animation of player attacking enemy. 
    public IEnumerator PlayerAttackEnemy(int playerId, int enemyId, GameManager instance, Board board){
        GameObject player = instance.charArray[playerId]; 
        GameObject enemy = instance.charArray[enemyId]; 
        int playerAttack = player.GetComponent<CharacterStats>().attack; 
        yield return instance.battleManagerObject.GetComponent<BattleManager>().TriggerBlackThenRed(player, enemy); 
        enemy.GetComponent<CharacterGameState>().DecreaseHp(playerAttack);
        yield return enemy.GetComponentInChildren<HealthBarManager>().UpdateHealthBar(enemy);

        if (!enemy.GetComponent<CharacterGameState>().IsAlive){
            yield return ApplyDeathSequence(enemyId, instance, board);
        }
        instance.statMenuController.GetComponent<StatMenuManager>().UpdateUnitScreen();
        instance.moveControllerObject.GetComponent<MoveController>().ResetSelected();
        yield return AssessWinner(instance);
    }

    // Animation for applying death sequence. as well as apply death to model
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
        instance.gameScore.AuditScore();
        yield return instance.battleManagerObject.GetComponent<BattleManager>().DeathFadeout(unit); 
        unit.SetActive(false); 
        yield return null; 

        // THIS MECHANICALLY WORKS BUT NEEDS AUDITING ON WHERE TO PLACE IT AND LOGIC ETC.
        // if (instance.gameScore.GetCurrentEnemyUnits() <= 1) {               // PLACEHOLDER FOR TEST
        //     yield return PlayerWinScript.InstantiatePlayerWinBanner();
        // }
    }

    public IEnumerator AssessWinner(GameManager instance){
        
        if (instance.gameScore.IsATeamDefeated()){
            instance.cursorStateMachine.TriggerEndGame();           
            yield break;
        }
        yield return null; 

    }
}