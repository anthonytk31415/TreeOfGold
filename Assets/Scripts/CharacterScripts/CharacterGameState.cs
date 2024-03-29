using System;
using UnityEngine;
/*

This is all about the state of an individual unit (team or enemy). 

Important stats: 
- enemy vs player
- hasMoved
- currentHP
- in-game modifiers

*/

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



    private Boolean isAlive; 
    public Boolean IsAlive {
        get {return isAlive;}
        set {isAlive = value;} 
    }

    private Boolean performedAction; 
    public Boolean PerformedAction {
        get {return performedAction;}
        set {performedAction = value;} 
    }

    private Boolean hasAttacked; // update later; placeholder
    public Boolean HasAttacked {
        get {return hasAttacked;} 
        set {if (value == true) {
                hasAttacked = true;
                performedAction = true; 
            } 
        }
    }

    public int curHp; 
    public int totalHp; 
    // Other methods as needed

    public void Initialize(GameManager gameManager, int charId, Boolean isYourTeam){
        this.gameManager = gameManager; 
        this.board = gameManager.board;
        this.charId = charId;  
        this.isYourTeam = isYourTeam; 
        this.curHp = gameManager.GetCharacter(charId).GetComponent<CharacterStats>().GetHp();
        this.totalHp = gameManager.GetCharacter(charId).GetComponent<CharacterStats>().GetHp();
        this.isAlive = true; 
        ResetMoves();
    }
    



    public void ResetMoves(){
        if (isAlive){
            hasMoved = false; 
            hasAttacked = false;
            performedAction = false;
        }
    }


    // used to calculate damage; might have to do some decoupling later for this method
    public int HPAfterAttack(int dmg){
        return Math.Max(0, curHp - dmg);
    }

    public void DecreaseHp(int dmg)
    {
        curHp = HPAfterAttack(dmg); 
        if (curHp == 0){
            IsAlive = false; 
        }
    }

    public int GetCurHp() 
    {
        return curHp; 
    }

       public int GetTotalHp() 
    {
        return totalHp; 
    }

    public void IncreaseHp(int heal)
    {
        
    }


}