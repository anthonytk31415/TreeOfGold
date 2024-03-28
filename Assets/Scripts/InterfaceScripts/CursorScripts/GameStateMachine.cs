using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*

..x

*/


[Serializable]
public class GameStateMachine {
    public EnemyState enemyState; 
    public ChooseState chooseState; 
    public EndGameState endGameState;
    public ICursorState CurrentState { get; private set; }
    private GameManager gameManager; 
    public GameObject cursor;
    public bool endGame; 



    public GameStateMachine(GameManager gameManager) {
        this.chooseState = new ChooseState(this.cursor, gameManager);
        this.enemyState = new EnemyState(gameManager);
        this.endGameState = new EndGameState(gameManager);
        this.gameManager = gameManager;
        this.endGame = false; 
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
        CurrentState.Enter();

        // notify other events that state has changed
        // stateChanged?.Invoke;
    }

    // allow the statemachine to update the state
    public void Update(){
        if (CurrentState != null){
            CurrentState.Update();
        }
    }


    public void TriggerEndGame(){
        this.endGame = true; 
    }

}