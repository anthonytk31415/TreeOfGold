using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyState : IGameState
{
    public GameManager instance; 
    public Board board; 
    public EnemyBattle enemyBattle; 

    public EnemyState(GameManager instance) {
        this.instance = instance;
        this.board = instance.board;  
        this.enemyBattle = new EnemyBattle(instance);
    }

    /// <summary>
    /// Use this method to chain enemy phase IEnumerators during the start phase
    /// </summary>
    /// <returns>IEnumerator</returns>
    private IEnumerator EnemyPhaseAsyncMethods(){
        // yield return EnemyPhaseScript.DisplayEnemyPhaseBanner(); 
        yield return PhaseBannerManager.InstantiateBanner(instance, PhaseBanner.EnemyPhase);
        yield return enemyBattle.ApplyEnemyPhase();
        GameStateMachine gSMachine = instance.gameStateMachine; 
        if (instance.gameStateMachine.endGame){
            gSMachine.TransitionTo(gSMachine.endGameState);
        }
        else {
            gSMachine.TransitionTo(gSMachine.playerState);     
        }   
        yield return null; 
    }

    public void Enter(){        
        instance.StartCoroutine(EnemyPhaseAsyncMethods());
    }

    // prob do not need anything here
    public void Update(){

    }
    public void Exit(){
        MoveController mc  = instance.moveControllerObject.GetComponent<MoveController>();
    }

}