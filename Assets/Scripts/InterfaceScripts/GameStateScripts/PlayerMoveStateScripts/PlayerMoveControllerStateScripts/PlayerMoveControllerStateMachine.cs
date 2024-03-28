using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerMoveControllerStateMachine {

    // states: 
    public SelectedState selectedState;
    public UnselectedState unselectedState;
    public NonPlayerPhase nonPlayerPhase;
    public IPlayerMoveControllerState CurrentState {get; private set; }
    private GameManager gameManager;

    public PlayerMoveControllerStateMachine(GameManager gameManager) {
        this.gameManager = gameManager;
        // this.selectedState;
        // this.unselectedState;
        // this.nonPlayerPhase;
        // this.CurrentState
    }    

    public void Initialize() {

    }

    public void TransitionTo(IPlayerMoveControllerState nextState){
        CurrentState.Exit();
        CurrentState = nextState; 
        CurrentState.Enter();
    }

    public void Update(){
        if (CurrentState != null){
            CurrentState.Update();
        }
    }






}