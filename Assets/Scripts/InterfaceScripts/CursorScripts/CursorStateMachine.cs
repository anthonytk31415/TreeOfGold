using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
We should name this the game state machine. 


ChooseState 
- this is really the ability for the player to issue commands to its own units. 
- Change the name to this? 


We will have 3 phases: 

player phase (i.e. when the ChooseState is active)



..x

*/


[Serializable]
public class CursorStateMachine {
    public EnemyState enemyState; 
    public ChooseState chooseState; 
    public ICursorState CurrentState { get; private set; }

    private GameManager gameManager; 

    public GameObject cursor;

    public CursorStateMachine(GameManager gameManager) {
        this.chooseState = new ChooseState(this.cursor, gameManager);
        this.enemyState = new EnemyState(gameManager);
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