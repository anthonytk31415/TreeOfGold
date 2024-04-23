using UnityEngine;
using UnityEngine.UIElements;
using System;
using System.Collections;
using System.Collections.Generic;


public class CharacterAnimateCommandIdle : ICharacterAnimateCommand {

    GameManager instance; 
    GameObject character; 
    Animator animator;
    CharacterAnimateCommandData characterAnimateCommandData;     
    // CharacterAnimateType characterAnimateType;
    CharacterAnimateController characterAnimateController ;

    public CharacterAnimateCommandIdle (GameManager instance, GameObject character, 
            CharacterAnimateController characterAnimateController)
    {
        this.instance = instance; 
        this.character = character; 

        this.animator = character.GetComponent<Animator>(); 
        this.characterAnimateController = characterAnimateController;
    }

    public void InstantiateCommand(){
        this.characterAnimateCommandData = this.characterAnimateController.characterAnimateCommandData;
        animator.SetBool(DirectionUtility.DirectionToIdleBool(characterAnimateCommandData.direction), true);
    }
    public void ProcessCommand(){
        // this.character.GetComponent<CharacterAnimateController>().ApplyAnimationDefaultState();
        // this.animator.SetBool(DirectionUtility.DirectionToIdleBool(this.direction), true);
        // this.curTime = this.endTime; 

    }

    public bool Processing(){
        return false; 
    }
    public void TerminateCommand(){

    }

    public void DoMove(Direction direction){
        CharacterAnimateCommandData idleData = new CharacterAnimateCommandData(
                0.0f, 
                characterAnimateController.characterAnimateCommandIdle, 
                direction, 
                new Vector2(-99, -99), 
                false); 
        characterAnimateController.animateQueue.Enqueue(idleData); 
    }

}