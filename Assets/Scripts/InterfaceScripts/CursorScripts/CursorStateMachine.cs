using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CursorStateMachine {
    public ICursorState CurrentState { get; private set; }

    private GameManager gameManager; 
    private Board board; 

    private GameObject cursor;

    // reference to state objects
    public ChooseState chooseState; 
    // public ProfileState profileState; 
    // public MoveState moveState; 
    // public CommandState commandState; 

    // event to notify other objects of the state change
    // public event Action<CursorStateMachine> stateChanged; 

    // need to build a CursorController
    // to manipulate this in between phases
    public CursorStateMachine(GameManager gameManager, Board board) {
        this.cursor = CursorObject.Initialize(board, gameManager);
        this.chooseState = new ChooseState(this.cursor, gameManager, board);
        // this.profileState = new ProfileState(cursor);
        // this.moveState = new MoveState(cursor);
        // this.commandState = new CommandState(cursor);

        this.gameManager = gameManager;
        this.board = board;  

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