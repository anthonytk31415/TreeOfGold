using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 

public class SelectedMovedState : IPlayerMoveControllerState {
    private GameManager instance; 
    private MoveController moveController; 
    private PlayerMoveControllerStateMachine playerMoveControllerStateMachine; 
    public SelectedMovedState(GameManager instance, MoveController moveController, PlayerMoveControllerStateMachine playerMoveControllerStateMachine){
        this.instance = instance; 
        this.moveController = moveController;
        this.playerMoveControllerStateMachine = playerMoveControllerStateMachine; 
    }

    public delegate void PositionResetChangedEventHandler(bool positionReset);
    // Define the event based on the delegate
    public static event PositionResetChangedEventHandler OnPositionResetChanged;
    private bool positionReset;
    public bool PositionReset {
        get { return positionReset; }
        set {
            OnPositionResetChanged?.Invoke(positionReset);
            positionReset = false; 
        }
    }


    public void Enter(){
        this.positionReset = false; 
    }
    public void Update(){

    }
    public void Exit(){
        this.positionReset = false; 
    }

    // give the coordinate, its nature, and you're in selected phase, go to the 
    // next phase and do something to selection// you'll probably handle actions 
    public void Process(Coordinate w){
        if (moveController.GetClickTarget(w) == ClickTarget.empty)
        {
            moveController.UndoMove();
            moveController.ResetSelected();
            playerMoveControllerStateMachine.TransitionTo(playerMoveControllerStateMachine.unselectedState);
            PositionReset = true;
            return;        
        }
        int targetUnit = instance.board.Get(w); 
        if (moveController.GetClickTarget(w) == ClickTarget.friend)
        {
            moveController.UndoMove();
            PositionReset = true;
            if (moveController.SelectedId == targetUnit){
                moveController.ResetSelected();
                playerMoveControllerStateMachine.TransitionTo(playerMoveControllerStateMachine.unselectedState);
                return;
            } else {
                moveController.ResetSelected();
                moveController.SelectedId = targetUnit;
                playerMoveControllerStateMachine.TransitionTo(playerMoveControllerStateMachine.selectedUnmovedState);
                return;
            } 
        }
        else if (moveController.GetClickTarget(w) == ClickTarget.enemy)
        {
            if (moveController.SelectedEnemyId == targetUnit){
                moveController.UnselectEnemyUnit();
                return;
            } 
            else {
                // selected enemy is in range
                if (!moveController.IsTargetAttackableEnemy(w))
                {
                    moveController.UndoMove();
                    moveController.ResetSelected();
                    playerMoveControllerStateMachine.TransitionTo(playerMoveControllerStateMachine.unselectedState);
                }
                moveController.SelectedEnemyId = targetUnit;
                return;

            } 
        }
    }


}