using UnityEngine;
using UnityEngine.UIElements;
using System;
using System.Collections;
using System.Collections.Generic;


// given a character, we have a set of methods that are guaranteed to work 
// assuming we implemented a series of animations appropriately

// apply this during the character instantiate phase
// maybe we do a singleton in the gamemanager and then call it on a charid

// Something weird is happening: the last framee is stopping between sequences so it doesnt complete the full
// Translation
public class CharacterAnimateController : MonoBehaviour
{
    GameManager instance; 
    string charName; 
    int charId; 
    GameObject character; 
    Animator animator;


    private float curTime; 
    private Vector2 startPos; 
    public Vector2 endPos; 
    public Direction direction;
    private float speed = 1.0f;
    private float endTime; 
    public bool traversal; 

    private Queue<Direction> moveQueue; 

    Dictionary<Direction, Vector2> directionToVector = new()
    {
        {Direction.up, Vector2.up},
        {Direction.down, Vector2.down},
        {Direction.left, Vector2.left},
        {Direction.right, Vector2.right}
    };

    Dictionary<Direction, string> directionToIdleBool = new()
    {
        {Direction.up, "idleUpBool" },
        {Direction.down, "idleDownBool"},
        {Direction.left, "idleLeftBool"},
        {Direction.right, "idleRightBool"}
    };
    Dictionary<Direction, string> directionToWalkBool = new()
    {
        {Direction.up, "walkUpBool" },
        {Direction.down, "walkDownBool"},
        {Direction.left, "walkLeftBool"},
        {Direction.right, "walkRightBool"}
    };


    public void Start(){
    }

    public void Update ()
    {
        UpdateMovement();
    }

    public void Initialize(GameManager instance, int charId, GameObject character)
    {
        Debug.Log(character);
        this.instance = instance; 
        this.charId = charId;
        this.character = character;
        this.charName = character.GetComponent<CharacterStats>().name; 
        this.animator = character.GetComponent<Animator>();
        this.direction = Direction.down;
        this.moveQueue = new();
        ApplyInitialState();
    }

    // we'll want to apply 
    public void ApplyAnimationDefaultState()
    {
        if (animator == null){ return;}
        foreach (var param in animator.parameters)
        {
            animator.SetBool(param.name, false);
        }
    }
    public void ApplyInitialState()
    {
        ApplyAnimationDefaultState();
        animator.SetBool("idleDownBool", true);
    }

    // public void ApplyMoves(List<Direction> directions)
    // {
    //     foreach(Direction direction in directions) {
    //         InstantiateTraversal(direction); 
    //     }

    // }



    public void AnimateMoveChar(int charId, Coordinate destination) {
        Coordinate start = instance.board.FindCharId(charId);
        List<Coordinate> pathList = CharInteraction.PathBetweenUnits(instance, start, destination);
        // List<Direction> directions = new(); 
        foreach (Coordinate nextCoordinate in pathList) {
            Direction nextDir = Coordinate.DirectionFromAdjacentCoordinates(start, nextCoordinate); 
            moveQueue.Enqueue(nextDir);
            start = nextCoordinate; 
        }

        // foreach direction in directions: enqueue it
        // ApplyMoves(directions);                 

    }
    


    public void InstantiateTraversal(Direction direction)
    {
        this.traversal = true;
        startPos = character.transform.position;
        endPos = startPos + directionToVector[direction]; 
        curTime = 0.0f;
        endTime = .12f;
        animator.SetBool(directionToIdleBool[this.direction], false);
        this.direction = direction;
        animator.SetBool(directionToWalkBool[direction], true);
    }

    public void UpdateMovement() 
    {
        if (!this.traversal && moveQueue.Count > 0){
            Direction curDirection = moveQueue.Dequeue();
            InstantiateTraversal(curDirection);
        }

        else if (this.traversal){
            if (curTime < endTime){            
                float lerpValue = Mathf.Lerp(0, 1, curTime / endTime); 
                character.transform.position = startPos + directionToVector[this.direction]*lerpValue;
                curTime += Time.deltaTime; 
            }         
            else {
                character.transform.position = this.endPos; 
                this.traversal = false; 
                this.curTime = 0.0f; 
                animator.SetBool(directionToWalkBool[direction], false);
                animator.SetBool(directionToIdleBool[direction], true);
            }
        }
    }


}