using UnityEngine;
using UnityEngine.UIElements;
using System;
using System.Collections;
using System.Collections.Generic;

// will need to worry about layer ordering between sword, attacker and attackee
public class CharacterAnimateCommandAttackSword : ICharacterAnimateCommand {

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
    public CharacterAnimateCommandAttackSword (GameManager instance, GameObject character, 
            CharacterAnimateController characterAnimateController)
    {
        this.instance = instance; 
        this.character = character; 
        this.animator = character.GetComponent<Animator>(); 
        this.characterAnimateController = characterAnimateController;
    }

    public void InstantiateCommand(){
        Debug.Log("Attack sword initiated");
        this.characterAnimateCommandData = characterAnimateController.characterAnimateCommandData;
        this.curTime = 0.0f; 
        this.endTime = this.characterAnimateCommandData.endTime; 
        this.startPos = this.character.transform.position;
        this.direction = this.characterAnimateCommandData.direction;
        this.endPos = this.characterAnimateCommandData.endPos;
        // this.characterAnimateController.ApplyAnimationDefaultState();
        this.animator.ResetTrigger(DirectionUtility.DirectionToAttackSwordTrigger(direction));
        this.animator.SetTrigger(DirectionUtility.DirectionToAttackSwordTrigger(direction));
    }

    public void ProcessCommand(){
        Debug.Log("process called for attackSword"); 
        this.curTime += Time.deltaTime; 
    }

    public bool Processing(){
        bool res = this.curTime < this.endTime;
        return res; 
    }
    public void TerminateCommand(){
        Debug.Log("Terminate called");
        // character.transform.position = this.endPos; 
        this.curTime = 0.0f; 
        // this.characterAnimateController.ApplyAnimationDefaultState();
        animator.SetBool(DirectionUtility.DirectionToIdleBool(direction), true);                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                          

    }

    public void DoMove(Direction direction){
        // characterAnimateController.characterAnimateCommandBlinkBlack.DoMove(direction); 
        
        CharacterAnimateCommandData commandData = new CharacterAnimateCommandData(
                0.33f, 
                characterAnimateController.characterAnimateCommandAttackSword, 
                direction, 
                new Vector2(-99, -99), 
                false);
        characterAnimateController.animateQueue.Enqueue(commandData); 

        // characterAnimateController.characterAnimateCommandBlinkRed.DoMove(direction); 
    }

    public void Enqueue(CharacterAnimateCommandData characterAnimateCommandData){
    }


}