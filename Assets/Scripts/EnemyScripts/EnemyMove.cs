/* 

This class will first be a data container that contains a move and potentially a target
We do not verify if this is a valid move at all; it simply defines where you move and what you do. 
*/

using System.Collections;
using UnityEngine;


public class EnemyMove {
    public GameObject character {get; private set; } 
    public Coordinate position {get; private set; } 
    public GameObject targetChar {get; private set; } 
    private GameManager instance; 
        


    public EnemyMove(GameObject character, Coordinate w, GameManager instance, GameObject targetChar)
    {
        this.character = character; 
        this.position = w; 
        this.instance = instance;
        this.targetChar =  targetChar; 
    }



}