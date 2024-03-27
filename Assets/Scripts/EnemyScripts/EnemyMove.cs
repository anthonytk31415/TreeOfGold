/* 

This class will first be a data container that contains a move and potentially a target
We do not verify if this is a valid move at all; it simply defines where you move and what you do. 
*/

using System.Collections;
using UnityEngine;


public class EnemyMove {
    private GameManager instance; 
    public GameObject character {get; private set; } 
    public Coordinate posUnit {get; private set; } 
    public Coordinate posTarget {get; private set; } 
        
    // not a great name, but represents a possible automated move during the enemy phase.
    // this is basically a container that contains all of the relevant data that is necessary
    // to do a move: character game object, its position, reference to the game manager, and 
    // the gameObject target character; this probably sohuld be the position

    public EnemyMove(GameManager instance, GameObject character, Coordinate posUnit, Coordinate posTarget)
    {
        this.character = character; 
        this.instance = instance;
        this.posUnit = posUnit; 
        this.posTarget =  posTarget; 
    }

    public override string ToString()
    {
        return $"Character: {character}, posUnit: {posUnit}, posTarget: {posTarget}";
    }


}