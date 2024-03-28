using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*

/// <summary>
/// This is really the "highlight tiles" controller
/// </summary>

we want to build: 
on update, if some conditions happen go to the next state. 
- if you click on end --> move to enemy phase
- if you are done with all moves --> move to enemy phase
- if player == 0: end game phase with loss
- if enemy == 0: end game with win

-- do you call this on update? 


*/



public class ChooseState : ICursorState
{
    public GameObject cursor; 
    public GameManager instance; 
    public Board board; 
    public Boolean endTurn; 

    // some properties that determine what the cursor does
    public ChooseState(GameObject cursor, GameManager instance) {
        this.cursor = cursor;
        this.instance = instance;
        this.board = instance.board;          
    }



    public void InitiatePlayerPhaseSettings(){
        this.endTurn = false;
        instance.moveControllerObject.GetComponent<MoveController>().ResetSelected();
        foreach (GameObject character in instance.charArray){
            character.GetComponent<CharacterGameState>().ResetMoves();
        }

    }

  private IEnumerator DoStartStuff(){
        
        yield return PlayerPhaseScript.InstantiatePlayerPhaseObject(); 
        yield return new WaitForSeconds(0.5f);
    }


    // trigger all the things you want to do when you enter
    public void Enter(){
        InitiatePlayerPhaseSettings();
        MoveController.OnSelectedCharIdChanged += HandleCharIdChanged; 
        instance.StartCoroutine(DoStartStuff());
    }

    public void Update(){
        GameStateMachine gSMachine = instance.gameStateMachine;
        // if we click on end, then we move to enemy phase
        if (endTurn){
            gSMachine.TransitionTo(gSMachine.enemyState);
            return; 
        }
        if (instance.gameStateMachine.endGame){
            gSMachine.TransitionTo(gSMachine.endGameState);
        }
    }

    public void Exit(){
        MoveController.OnSelectedCharIdChanged -= HandleCharIdChanged;
    }

    // lots of move choices below; do we reorganize later into its own move class?


    /// <summary>
    /// HandleCharIdChanged will be called when selectedId from the MoveController updates via a delegate.
    /// </summary>
    /// <param name="charId"> int; the unique ID for the char</param>
    public void HandleCharIdChanged(int charId){
        instance.highlightTilesManager.TriggerSelectedHighlights();
    }




}