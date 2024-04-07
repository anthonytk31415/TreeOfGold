using UnityEngine;
using UnityEngine.UIElements;
using System;
using System.Collections;
using System.Collections.Generic;

// given a character, we have a set of methods that are guaranteed to work 
// assuming we implemented a series of animations appropriately


// apply this during the character instantiate phase

// maybe we do a singleton in the gamemanager and then call it on a charid



// somethign weird is happening: the last framee is stopping between sequences so it doesnt complete the full
// tranlsation

public class CharacterAnimateController : MonoBehaviour
{
    GameManager instance; 
    string charName; 
    int charId; 
    GameObject character; 
    Animator animator;
    Rigidbody2D rb;

    Dictionary<Direction, Vector2> directionToVector = new()
    {
        {Direction.up, new Vector2(0, 1)},
        {Direction.down, new Vector2(0, -1)},
        {Direction.left, new Vector2(-1, 0)},
        {Direction.right, new Vector2(1, 0)}
    };

    Dictionary<Direction, string> directionToIdleBool = new()
    {
        {Direction.up, "idleUpBool" },
        {Direction.down, "idleDownBool"},
        {Direction.left, "idleLeftBool"},
        {Direction.right, "idleRightBool"}
    };



    public void Initialize(GameManager instance)
    {
        this.instance = instance; 

        for (int i = 0; i < instance.charArray.Length; i ++ )
        {
            ApplyInitialState(i);
        }

    }

    public void DefineReferences(int charId)
    {
        // Debug.Log("this: " + this.charId + "; charid: " + charId);

        this.charId = charId;
        this.character = instance.charArray[charId];
        this.charName = character.GetComponent<CharacterStats>().name; 
        this.animator = character.GetComponent<Animator>();
        this.rb = character.GetComponent<Rigidbody2D>();

    }

    public void ApplyInitialState(int charId)
    {
        ApplyAnimationDefaultState(charId);
        animator.SetBool("idleDownBool", true);
    }

    // we'll want to apply 
    public void ApplyAnimationDefaultState(int charId)
    {
        DefineReferences(charId);
        if (animator == null)
        {
            return;
        }
        // define the logic for setting to animiation default; probaly
        // putting all properties to false, and then
        // setting down to true
        // ensure that the default entry position is "down" if that's the case
        foreach (var param in animator.parameters)
        {
            animator.SetBool(param.name, false);
        }
        // animator.SetBool("idleDownBool", true);
    }

    // write methods for animate: up, down, left, right

    // write methods for moving up, down, left, right
    // translation methods

    public IEnumerator ApplyTranslationDir(int charId, Direction direction) {
        DefineReferences(charId);
        Vector2 originalPos = character.transform.position;
        Vector2 step = directionToVector[direction];
        Debug.Log(step);
        string boolName = directionToIdleBool[direction];

        Action<float> translation = (float x) =>{
            Vector2 finalPos = originalPos + x * step;
            character.transform.position =  finalPos;
            // rb.MovePosition(finalPos);
        };


        ApplyAnimationDefaultState(charId);
        // yield return null; 
        // animator.SetBool("idleDownBool", false);
        yield return null; 
        animator.SetBool(boolName, true);
        // Debug.Log(boolName +", : val: "); 
        // apply movement
        yield return null; 
        float totalTime = .2f;
        float totalFrames = 16f;
        yield return Lerp(totalTime, totalFrames, translation);
        yield return null;
    }



// need transiiton time = 0
// need transitions form all states

    // public IEnumerator ApplyDirection(int charId, Direction direction) {
    //     string boolName = directionToIdleBool[direction];
    //     animator.SetBool(boolName, true);
    //     yield return null; 
    // }


    public IEnumerator ApplyMoves(int charId, List<Direction> directions)
    {
        foreach(Direction direction in directions) {
            yield return ApplyTranslationDir(charId, direction);
            yield return null;
        }
    }




    // given total time, we will execute the function evenly in paritions. 
    public static IEnumerator Lerp(
        float totalTime,
        float totalFrames,
        Action<float> action)
    {
        float time = 0f;
        float timeInterval = totalTime / totalFrames;
        yield return new WaitForSeconds(timeInterval);
        while (time < totalTime)
        {
            // Debug.Log("current time: " + time);
            action(time / totalTime);
            yield return new WaitForSeconds(timeInterval);
            time += timeInterval;
        }
        action(1.0f);
        yield return new WaitForSeconds(timeInterval);

    }


}