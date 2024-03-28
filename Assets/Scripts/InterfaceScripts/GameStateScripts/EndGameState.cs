using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// EndGameState - end state for the game. We have decided a winner. 
/// </summary>
public class EndGameState : IGameState
{


    public GameManager instance; 
    public Board board; 


    // some properties that determine what the cursor does
    public EndGameState(GameManager instance) {
        this.instance = instance;
        this.board = instance.board;  
    }


    public void TriggerBanner(){
        if (instance.gameScore.IsATeamDefeated()){
            if (instance.gameScore.DidPlayerWin()){
                instance.StartCoroutine(PlayerWinScript.InstantiatePlayerWinBanner());
            }
            else if (instance.gameScore.DidEnemyWin()) {
                instance.StartCoroutine(PlayerLoseScript.InstantiatePlayerLoseBanner());
            }
        }
        // do we need to do variable cleanup? 
        // do a DISABLE EVERYTHING
    }


    // trigger all the things you want to do when you enter
    public void Enter(){
        TriggerBanner();
    }

    public void Update(){

    }

    public void Exit(){

    }

    // lots of move choices below; do we reorganize later into its own move class?



}