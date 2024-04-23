using UnityEngine;
using UnityEngine.UIElements;
using System;
using System.Collections;
using System.Collections.Generic;


public class CharacterAnimateCommandWalk : ICharacterAnimateCommand {

    GameManager instance; 
    GameObject character; 
    int charId; 
    Animator animator;
    CharacterAnimateCommandData characterAnimateCommandData;     
    // CharacterAnimateType characterAnimateType;
    CharacterAnimateController characterAnimateController ;
    float curTime; 
    float endTime; 
    Vector2 endPos; 
    Vector2 startPos; 
    Direction direction; 
    public CharacterAnimateCommandWalk (GameManager instance, GameObject character, 
            CharacterAnimateController characterAnimateController)
    {
        this.instance = instance; 
        this.character = character; 
        this.animator = character.GetComponent<Animator>(); 
        this.characterAnimateController = characterAnimateController;
        this.charId = instance.GetCharId(character);
    }

    /// <summary>
    /// Required method to initiate 
    /// </summary>
    public void InstantiateCommand(){
        Debug.Log("walk initiated");
        this.characterAnimateCommandData = characterAnimateController.characterAnimateCommandData;
        this.curTime = 0.0f; 
        this.endTime = this.characterAnimateCommandData.endTime; 
        this.startPos = this.character.transform.position;
        this.direction = this.characterAnimateCommandData.direction;
        this.endPos = this.characterAnimateCommandData.endPos;
        // this.characterAnimateController.ApplyAnimationDefaultState();
        this.animator.SetBool(DirectionUtility.DirectionToWalkBool(direction), true);
    }

    /// <summary>
    /// Required method to process 
    /// </summary>
    public void ProcessCommand(){
        float lerpValue = Mathf.Lerp(0, 1, this.curTime / this.endTime); 
        character.transform.position = startPos + DirectionUtility.DirectionToVector(direction)*lerpValue;
        this.curTime += Time.deltaTime; 
    }

    /// <summary>
    /// Required method to trigger processing update; Typically a condition that has 
    /// to be met like curtime < endtime 
    /// </summary>
    public bool Processing(){
        bool res = this.curTime < this.endTime;
        return res; 
    }

    /// <summary>
    /// Required method to terminate
    /// </summary>
    public void TerminateCommand(){
        character.transform.position = this.endPos; 
        this.curTime = 0.0f; 
        // this.characterAnimateController.ApplyAnimationDefaultState();
        animator.SetBool(DirectionUtility.DirectionToIdleBool(direction), true);

    }

    /// <summary>
    /// Call DoMove to actually do the animation. 
    /// </summary>
    /// <param name="destination"></param>
    public void DoMove(Coordinate destination){
        Coordinate start = instance.board.FindCharId(this.charId);
        List<Coordinate> pathList = CharInteraction.ShortestPathBetweenCoordinates(instance, start, destination);
        // Debug.Log("path to go to: ");
        // AuditDebug.DebugIter(pathList);
        foreach (Coordinate nextCoordinate in pathList) {
            Direction nextDir = Coordinate.DirectionFromAdjacentCoordinates(start, nextCoordinate); 
            (double x, double y ) = instance.board.ConvertMatToSceneCoords(start);
            Vector2 finalVector = new Vector2((float)x, (float)y) + DirectionUtility.DirectionToVector(nextDir);
            CharacterAnimateCommandData moveData = new CharacterAnimateCommandData(
                    .12f, 
                    characterAnimateController.characterAnimateCommandWalk, 
                    nextDir, 
                    finalVector, 
                    true);
            characterAnimateController.animateQueue.Enqueue(moveData);
            start = nextCoordinate; 
        }     
    }
    
}