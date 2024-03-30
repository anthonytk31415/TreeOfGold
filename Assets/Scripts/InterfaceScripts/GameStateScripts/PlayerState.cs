using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;





// we want to build: 
// on update, if some conditions happen go to the next state. 
// - if you click on end --> move to enemy phase
// - if you are done with all moves --> move to enemy phase
// - if player == 0: end game phase with loss
// - if enemy == 0: end game with win

// -- do you call this on update? 

/// <summary>
/// This is really the "highlight tiles" controller
/// </summary>
public class PlayerState : IGameState
{
    public GameObject cursor; 
    public GameManager instance; 
    public Board board; 
    public bool endTurn; 

    public delegate void EndTurnEventHandler(bool endTurn);
    public static event EndTurnEventHandler OnEndTurnChanged; 
    public bool EndTurn {
        get {return endTurn; }
        set { 
            endTurn = value; 
            OnEndTurnChanged?.Invoke(endTurn);
        }
    }


    // some properties that determine what the cursor does
    public PlayerState(GameObject cursor, GameManager instance) {
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
        yield return PhaseBannerManager.InstantiateBanner(instance, PhaseBanner.PlayerPhase);
        // PlayerPhaseScript.InstantiatePlayerPhaseObject(); 
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

    /// <summary>
    /// HandleCharIdChanged will be called when selectedId from the MoveController updates via a delegate.
    /// </summary>
    /// <param name="charId"> int; the unique ID for the char</param>
    public void HandleCharIdChanged(int charId){
        instance.highlightTilesManager.TriggerSelectedHighlights();
    }


}