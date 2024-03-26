using System; 
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

    public void Enter(){
        Debug.Log("do not forget to add the Enemy Phase Banner");
        Debug.Log("entering Enemy State for the Enemy Phase.");
        Debug.Log("From here, we'll kick off the Enemy functions like DoMove for each enemy in the CharArray that is alive. ");
        enemyBattle.ApplyEnemyPhase();
        CursorStateMachine csMachine = instance.cursorStateMachine; 
        csMachine.TransitionTo(csMachine.chooseState);
    }

    // prob do not need anything here
    public void Update(){

    }
    public void Exit(){
        Debug.Log("Exiting Enemy State");
    }



}