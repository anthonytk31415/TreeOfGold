using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CursorStateMachine {
    public ICursorState CurrentState { get; private set; }
    private GameManager gameManager; 

    public GameObject cursor;
    public ChooseState chooseState; 

    public CursorStateMachine(GameManager gameManager) {
        this.chooseState = new ChooseState(this.cursor, gameManager);
        this.gameManager = gameManager;
    }


    // set the starting state
    public void Initialize(ICursorState state){
        CurrentState = state; 
        state.Enter(); 
        // notify other events that state has changed
        // stateChanged?.Invoke;
    }

    // exit this state and enter another
    public void TransitionTo(ICursorState nextState){
        CurrentState.Exit();
        CurrentState = nextState; 
        nextState.Enter();

        // notify other events that state has changed
        // stateChanged?.Invoke;
    }

    // allow the statemachine to update the state
    public void Update(){
        if (CurrentState != null){
            CurrentState.Update();
        }
    }

}