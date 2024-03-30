using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  Do we really need this? when the turn ends, we disable effects coming from 
///  the MoveController Mouse Controller...
/// </summary>
public class NonPlayerPhaseState : IPlayerMoveControllerState {


    private GameManager instance; 
    private MoveController moveController; 
    private PlayerMoveControllerStateMachine playerMoveControllerStateMachine; 
    public NonPlayerPhaseState(GameManager instance, MoveController moveController, PlayerMoveControllerStateMachine playerMoveControllerStateMachine){
        this.instance = instance; 
        this.moveController = moveController;
        this.playerMoveControllerStateMachine = playerMoveControllerStateMachine; 
        PlayerState.OnEndTurnChanged += HandleEndTurnChanged; 
        // instance.gameStateMachine.playerState.OnEndTurnChanged += HandleEndTurnChanged; 
    }

    public void HandleEndTurnChanged(bool endTurn){
        // if (endTurn){
        //     playerMoveControllerStateMachine.TransitionTo(playerMoveControllerStateMachine.nonPlayerPhaseState);            
        // }
        // else {
        //     playerMoveControllerStateMachine.TransitionTo(playerMoveControllerStateMachine.unselectedState);            
        // }

    }

    public void Enter(){

    }
    public void Update(){
        // when you're back in player phase, transition to unselected
        // when endturn is clicked
    }
    public void Exit(){
        // transition to unselected
        // reset the selected items 
    }

    public void Process(Coordinate w){
        // probably does nothing during this time. 
    }
}