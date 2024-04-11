using UnityEngine;
using UnityEngine.UIElements;
using System;
using System.Collections;
using System.Collections.Generic;


public class CharacterAnimateCommandData{
    
    public CharacterAnimateType characterAnimateType {get; set;}
    public Direction direction {get; set;}
    public Vector2 endPos {get; set;}
    public bool requiresMove {get; set;}
    public Action animateCommand {get; set;}

    public float endTime {get; set;}


    /// <summary>
    /// data to initiate data
    /// </summary>
    /// <param name="endTime">end time of the procedure</param>
    /// <param name="characterAnimateType">the type of animation </param>
    /// <param name="direction"> direction char will be facing </param>
    /// <param name="endPos"> end position (if any required) </param>
    /// <param name="requireMove"> boolean: does char need to be moved? </param>
    public CharacterAnimateCommandData(float endTime, CharacterAnimateType characterAnimateType, 
            Direction direction, Vector2 endPos, bool requireMove)
    {
        this.endTime = endTime;
        this.characterAnimateType = characterAnimateType;
        this.direction = direction;
        this.endPos = endPos;
        this.requiresMove = requireMove;      
    }


}