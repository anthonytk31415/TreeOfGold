using System;
using UnityEngine;

// store in-game data about the char: 
// - enemy vs player
// - hasMoved
// - currentHP
// - in-game modifiers


public class CharacterGameState : MonoBehaviour
{
    public GameManager gameManager; 
    public Board board; 
    public int charId;  

    public Boolean player;  // True = your team; False = opponent team
    public Boolean hasMoved; 
    public int curHp; 
    // Other methods as needed


    public void Initialize(GameManager gameManager, Board board, int charId, Boolean player){
        this.gameManager = gameManager; 
        this.board = board;
        this.charId = charId;  
        this.player = player; 
        this.hasMoved = false; 
        this.curHp = gameManager.GetCharacter(charId).GetComponent<CharacterStats>().GetHp();
    }

    public void Moved(){
        hasMoved = true; 
    }
    public void NotMoved()
    {
        hasMoved = false; 
    }
    public Boolean HasMoved(){
        return hasMoved; 
    }

    public void DecreaseHp(int dmg)
    {
        curHp = Math.Max(0, curHp - dmg); 
    }

    public int GetCurHp() 
    {
        return curHp; 
    }

    public void IncreaseHp(int heal)
    {
        
    }

}