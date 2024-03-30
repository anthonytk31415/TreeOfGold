using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
///  This controls the phases of the game, between Player, Enemy and EndGame phases. It uses
///  the State Pattern to change states.
/// </summary>

[Serializable]
public class GameStateMachine {
    public EnemyState enemyState; 
    public PlayerState playerState; 
    public EndGameState endGameState;
    public IGameState CurrentState { get; private set; }
    private GameManager gameManager; 
    public GameObject cursor;
    public bool endGame; 




    public GameStateMachine(GameManager gameManager) {
        this.playerState = new PlayerState(this.cursor, gameManager);
        this.enemyState = new EnemyState(gameManager);
        this.endGameState = new EndGameState(gameManager);
        this.gameManager = gameManager;
        this.endGame = false; 
    }

    // set the starting state
    public void Initialize(IGameState state){
        CurrentState = state; 
        state.Enter(); 
        // notify other events that state has changed
        // stateChanged?.Invoke;
    }

    // exit this state and enter another
    public void TransitionTo(IGameState nextState){
        CurrentState.Exit();
        CurrentState = nextState; 
        CurrentState.Enter();

        // notify other events that state has changed
        // stateChanged?.Invoke;
    }

    // allow the state machine to update the state
    public void Update(){
        if (CurrentState != null){
            CurrentState.Update();
        }
    }


    public void TriggerEndGame(){
        this.endGame = true; 
    }

    public bool IsPlayerState(){
        return CurrentState == playerState; 
    }

}