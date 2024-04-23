using UnityEngine;
using UnityEngine.UIElements;
using System;
using System.Collections;
using System.Collections.Generic;

// will need to worry about layer ordering between sword, attacker and attackee
public class CharacterAnimateCommandBlinkBlack : ICharacterAnimateCommand {

    GameManager instance; 
    GameObject character; 
    Animator animator;
    CharacterAnimateCommandData characterAnimateCommandData;     
    // CharacterAnimateType characterAnimateType;
    CharacterAnimateController characterAnimateController ;
    float curTime; 
    float endTime; 
    Vector2 endPos; 
    Vector2 startPos; 
    Direction direction; 
    public CharacterAnimateCommandBlinkBlack (GameManager instance, GameObject character, 
            CharacterAnimateController characterAnimateController)
    {
        this.instance = instance; 
        this.character = character; 
        this.animator = character.GetComponent<Animator>(); 
        this.characterAnimateController = characterAnimateController;
    }

    public void InstantiateCommand(){
        Debug.Log("Blink Black initiated");
        this.characterAnimateCommandData = characterAnimateController.characterAnimateCommandData;
        this.curTime = 0.0f; 
        this.endTime = this.characterAnimateCommandData.endTime; 
        this.startPos = this.character.transform.position;
        this.direction = this.characterAnimateCommandData.direction;
        this.endPos = this.characterAnimateCommandData.endPos;
        // this.characterAnimateController.ApplyAnimationDefaultState();
        // animator.SetBool(DirectionUtility.DirectionToIdleBool(direction), true);
        animator.SetTrigger("idleBlinkBlackDownTrigger");
        // this.animator.ResetTrigger(DirectionUtility.DirectionToBlinkBlack(direction));
        // this.animator.SetTrigger(DirectionUtility.DirectionToBlinkBlack(direction));
    }

    public void ProcessCommand(){
        Debug.Log("process called for BlinkBlack"); 
        this.curTime += Time.deltaTime; 
    }

    public bool Processing(){
        bool res = this.curTime < this.endTime;
        return res; 
    }
    public void TerminateCommand(){
        Debug.Log("Terminate called");
        this.curTime = 0.0f; 
        // this.characterAnimateController.ApplyAnimationDefaultState();
        // animator.SetBool("idleBlinkBlackLeftBool", false);
        // animator.SetBool(DirectionUtility.DirectionToIdleBool(direction), true);
        animator.SetTrigger("idleDownTrigger");

    }

    public void DoMove(Direction direction){
        CharacterAnimateCommandData commandData = new CharacterAnimateCommandData(
                .2f, 
                characterAnimateController.characterAnimateCommandBlinkBlack, 
                direction, 
                new Vector2(-99, -99), 
                false);
        characterAnimateController.animateQueue.Enqueue(commandData); 
    }

    public void Enqueue(CharacterAnimateCommandData characterAnimateCommandData){
    }


}