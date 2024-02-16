using System;
using UnityEngine;

// store in-game data about the char: 
// - enemy vs player
// - hasMoved
// - currentHP
// - in-game modifiers


public static class CharacterBattle {

    
    public static void CommenceBattle(int playerId, int enemyId, GameManager instance){
        GameObject player = instance.charArray[playerId]; 
        GameObject enemy = instance.charArray[enemyId]; 
        Board board = instance.board; 
        /// mark tbd here

        PlayerAttackEnemy(playerId, enemyId, instance, board);
        instance.statMenuController.GetComponent<StatMenuManager>().UpdateUnitScreen();

        if (enemy.GetComponent<CharacterGameState>().IsAlive){
            PlayerAttackEnemy(enemyId, playerId, instance, board);
            instance.statMenuController.GetComponent<StatMenuManager>().UpdateUnitScreen();
        }

        player.GetComponent<CharacterGameState>().HasAttacked = true;
        instance.moveControllerObject.GetComponent<MoveController>().PostActionCleanup();
        
        Debug.Log("can you make it this farin commence battle?");
        // instance.cursorStateMachine.chooseState.TriggerSelectedHighlights(); 
    }

    public static void PlayerAttackEnemy(int playerId, int enemyId, GameManager instance, Board board){
        GameObject player = instance.charArray[playerId]; 
        GameObject enemy = instance.charArray[enemyId]; 
        int playerAttack = player.GetComponent<CharacterStats>().attack; 
        enemy.GetComponent<CharacterGameState>().DecreaseHp(playerAttack);

        if (!enemy.GetComponent<CharacterGameState>().IsAlive){
            Debug.Log("applying death sequence on : " + enemyId);
            ApplyDeathSequence(enemyId, instance, board);
        }
    }
 
    public static void ApplyDeathSequence(int unitId, GameManager instance, Board board){
        Coordinate w = board.FindCharId(unitId); 
        Debug.Log("death coordinate: " + w + "; get: ");

        Debug.Log("get on board: " + board.Get(w));
    
        board.PutEmpty(w);
        Debug.Log(board.Get(w));
        GameObject unit = instance.charArray[unitId];
        unit.GetComponent<CharacterGameState>().IsAlive = false;
        if (unit.GetComponent<CharacterGameState>().isYourTeam){
            instance.gameScore.removePlayerUnit();
        } else {
            instance.gameScore.removeEnemyUnit();
        } 
        // Placeholder: do some sort of death animation
        unit.SetActive(false); 
    }

}