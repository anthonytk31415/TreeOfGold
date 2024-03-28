using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyState : ICursorState
{
    public GameManager instance; 
    public Board board; 
    public EnemyBattle enemyBattle; 

    public EnemyState(GameManager instance) {
        this.instance = instance;
        this.board = instance.board;  
        this.enemyBattle = new EnemyBattle(instance);
    }

    private IEnumerator EnemyPhaseAsyncMethods(){
        yield return EnemyPhaseScript.DisplayEnemyPhaseBanner(); 
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
        // Debug.Log("Exiting Enemy State");
        // Debug.Log("selId: " + mc.SelectedId +"; selEnemyID: " + mc.SelectedEnemyId);
    }

}