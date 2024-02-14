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

    public Boolean isYourTeam;  // True = your team; False = opponent team
    private Boolean hasMoved; 
    public Boolean HasMoved {
        get {return hasMoved;}
        set {hasMoved = value;} 
    }
    
    private Boolean performedAction; 
    public Boolean PerformedAction {
        get {return performedAction;}
        set {performedAction = value;} 
    }


    public int curHp; 
    // Other methods as needed


    public void Initialize(GameManager gameManager, Board board, int charId, Boolean isYourTeam){
        this.gameManager = gameManager; 
        this.board = board;
        this.charId = charId;  
        this.isYourTeam = isYourTeam; 
        this.hasMoved = false; 
        this.performedAction = false;
        this.curHp = gameManager.GetCharacter(charId).GetComponent<CharacterStats>().GetHp();
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