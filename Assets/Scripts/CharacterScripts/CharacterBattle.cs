using System;
using UnityEngine;

// store in-game data about the char: 
// - enemy vs player
// - hasMoved
// - currentHP
// - in-game modifiers


public static class CharacterBattle {

    
    public static void CommenceBattle(int playerId, int enemyId, GameManager instance, Board board){
        GameObject player = instance.charArray[playerId]; 
        GameObject enemy = instance.charArray[enemyId]; 

        /// mark tbd here

        Attack(playerId, enemyId, instance, board);
        instance.statMenuController.GetComponent<StatMenuManager>().UpdateUnitScreen();
        if (enemy.GetComponent<CharacterGameState>().IsAlive){
            Attack(enemyId, playerId, instance, board);
                instance.statMenuController.GetComponent<StatMenuManager>().UpdateUnitScreen();
        }


        player.GetComponent<CharacterGameState>().HasAttacked = true;

        instance.moveControllerObject.GetComponent<MoveController>().PostActionCleanup();
        instance.cursorStateMachine.chooseState.TriggerSelectedHighlights(); 

        // Debug.Log("playerHP = " + player.GetComponent<CharacterGameState>().GetCurHp() + "; oppHp = " + 
        //             enemy.GetComponent<CharacterGameState>().GetCurHp()); 


    }

    public static void Attack(int playerId, int enemyId, GameManager instance, Board board){
        GameObject player = instance.charArray[playerId]; 
        GameObject enemy = instance.charArray[enemyId]; 

        /// mark tbd here
        
        int playerAttack = player.GetComponent<CharacterStats>().attack; 
        int enemyAttack = enemy.GetComponent<CharacterStats>().attack; 

        enemy.GetComponent<CharacterGameState>().DecreaseHp(playerAttack);


    }


}