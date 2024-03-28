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

    private IEnumerator DoStartStuff(){
        yield return EnemyPhaseScript.InstantiateEnemyPhaseObject(); 
        yield return enemyBattle.ApplyEnemyPhase();
        CursorStateMachine csMachine = instance.cursorStateMachine; 
        csMachine.TransitionTo(csMachine.chooseState);
        yield return null; 
    }

    public void Enter(){
        Debug.Log("entering Enemy State for the Enemy Phase.");
        
        instance.StartCoroutine(DoStartStuff());
    }

    // prob do not need anything here
    public void Update(){

    }
    public void Exit(){
        MoveController mc  = instance.moveControllerObject.GetComponent<MoveController>();
        Debug.Log("Exiting Enemy State");
        Debug.Log("selId: " + mc.SelectedId +"; selEnemyID: " + mc.SelectedEnemyId);
    }

}