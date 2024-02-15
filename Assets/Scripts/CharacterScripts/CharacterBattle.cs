using System;
using UnityEngine;

// store in-game data about the char: 
// - enemy vs player
// - hasMoved
// - currentHP
// - in-game modifiers


public static class CharacterBattle {

    
    public static void Battle(int playerId, int enemyId, GameManager instance, Board board){
        GameObject player = instance.charArray[playerId]; 
        GameObject enemy = instance.charArray[enemyId]; 

        /// mark tbd here
    }


}