using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerMoveControllerStateMachine {

    // states: 
    public SelectedUnmovedState selectedUnmovedState;
    public SelectedMovedState selectedMovedState;
    public UnselectedState unselectedState;
    public NonPlayerPhaseState nonPlayerPhaseState;
    public IPlayerMoveControllerState CurrentState {get; private set; }
    private GameManager gameManager;
    private MoveController moveController; 
    private PlayerMoveControllerStateMachine playerMoveControllerStateMachine; 

    public PlayerMoveControllerStateMachine(GameManager gameManager, MoveController moveController) {
        this.selectedUnmovedState = new SelectedUnmovedState(gameManager, moveController, this);   
        this.selectedMovedState = new SelectedMovedState(gameManager, moveController, this);   
        this.unselectedState = new UnselectedState(gameManager, moveController, this);   
        this.nonPlayerPhaseState = new NonPlayerPhaseState(gameManager, moveController, this);   
    }    

    public void Initialize() {
        this.CurrentState = unselectedState; 
        
    }

    public void TransitionTo(IPlayerMoveControllerState nextState){
        CurrentState.Exit();
        CurrentState = nextState; 
        CurrentState.Enter();
    }



    public void ProcessClick(Coordinate w){
        CurrentState.Process(w); 
    }

    


}