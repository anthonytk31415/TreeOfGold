using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;


public class UnselectedState : IPlayerMoveControllerState {
    private GameManager instance; 
    private MoveController moveController; 
    private PlayerMoveControllerStateMachine playerMoveControllerStateMachine; 

    public UnselectedState(GameManager instance, MoveController moveController, PlayerMoveControllerStateMachine playerMoveControllerStateMachine){
        this.instance = instance; 
        this.moveController = moveController;
        this.playerMoveControllerStateMachine = playerMoveControllerStateMachine; 
    }

    public void Enter(){
    }
    public void Update(){
        // if end turn is clicked, go to non player phase
    }
    public void Exit(){
    }

    public void Process(Coordinate w){
        if (moveController.GetClickTarget(w) == ClickTarget.empty){
            moveController.ResetSelected();
            return;
        }
        int targetUnit = instance.board.Get(w);
        if (moveController.GetClickTarget(w) == ClickTarget.friend){
            moveController.ResetSelected();
            moveController.SelectedId = targetUnit;         
            
            playerMoveControllerStateMachine.TransitionTo(playerMoveControllerStateMachine.selectedUnmovedState);
            return;
        }
        else if (moveController.GetClickTarget(w) == ClickTarget.enemy){
            if (targetUnit == moveController.SelectedEnemyId){
                moveController.ResetSelected();
            } else {
                moveController.SelectedEnemyId = targetUnit; 
            }
            return; 
        }
        
    }


    public void MyTest(){
        Coordinate w = instance.board.FindCharId(0);
        Coordinate v = instance.board.FindCharId(2);
        List<Coordinate> path = CharInteraction.PathBetweenUnits(instance, w, v);
        Debug.Log("Audit MYTEST from UnselectedState; length of path: " + path.Count);
    }


}