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

    HashSet<string> animatorParameterNames; 

    private float curTime; 
    private Vector2 startPos; 
    public Vector2 endPos; 
    public Direction direction;

    private float endTime; 
    public bool traversal; 

    private Queue<Direction> moveQueue; 

    public static Vector2 DirectionToVector(Direction direction){
        Dictionary<Direction, Vector2> directions  = new()
        {
            {Direction.up, Vector2.up},
            {Direction.down, Vector2.down},
            {Direction.left, Vector2.left},
            {Direction.right, Vector2.right}
        }; 
        return directions[direction];
    }

    public static Coordinate DirectionToCoordinate(Direction direction){
        Dictionary<Direction, Coordinate> directions  = new()
        {
            {Direction.up, new Coordinate(0, 1)},
            {Direction.down, new Coordinate(0, -1)},
            {Direction.left, new Coordinate(-1, 0)},
            {Direction.right, new Coordinate(1, 0)}
        };
        return directions[direction];
    }

    public static string DirectionToIdleBool(Direction direction){
        Dictionary<Direction, string> directions  = new()
        {
            {Direction.up, "idleUpBool" },
            {Direction.down, "idleDownBool"},
            {Direction.left, "idleLeftBool"},
            {Direction.right, "idleRightBool"}
        };
        return directions[direction];
    }

    public static Dictionary<Direction, string> directionToWalkBool = new()
    {
        {Direction.up, "walkUpBool" },
        {Direction.down, "walkDownBool"},
        {Direction.left, "walkLeftBool"},
        {Direction.right, "walkRightBool"}
    };

    public static string DirectionToWalkBool(Direction direction){
        Dictionary<Direction, string> directions  = new()
        {
            {Direction.up, "walkUpBool" },
            {Direction.down, "walkDownBool"},
            {Direction.left, "walkLeftBool"},
            {Direction.right, "walkRightBool"}
        };
        return directions[direction];
    }

    public static string DirectionToAttackSwordTrigger(Direction direction){
        Dictionary<Direction, string> directions  = new()
        {
            {Direction.up, "attackSwordUpTrigger" },
            {Direction.down, "attackSwordDownTrigger"},
            {Direction.left, "attackSwordLeftTrigger"},
            {Direction.right, "attackSwordRightTrigger"}
        };
        return directions[direction];
    }

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
        this.animatorParameterNames = new HashSet<string>();
        foreach (var parameter in this.animator.parameters){
            this.animatorParameterNames.Add(parameter.name); 
        };
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

    public void AnimateMoveChar(int charId, Coordinate destination) {
        Coordinate start = instance.board.FindCharId(charId);
        List<Coordinate> pathList = CharInteraction.PathBetweenUnits(instance, start, destination);
        foreach (Coordinate nextCoordinate in pathList) {
            Direction nextDir = Coordinate.DirectionFromAdjacentCoordinates(start, nextCoordinate); 
            moveQueue.Enqueue(nextDir);
            start = nextCoordinate; 
        }             
    }
    


    public void InstantiateTraversal(Direction direction)
    {
        this.traversal = true;
        startPos = character.transform.position;
        endPos = startPos + DirectionToVector(direction); 
        curTime = 0.0f;
        endTime = .12f;
        animator.SetBool(DirectionToIdleBool(this.direction), false);
        this.direction = direction;
        animator.SetBool(DirectionToWalkBool(direction), true);
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
                character.transform.position = startPos + DirectionToVector(direction)*lerpValue;
                curTime += Time.deltaTime; 
            }         
            else {
                character.transform.position = this.endPos; 
                this.traversal = false; 
                this.curTime = 0.0f; 
                animator.SetBool(DirectionToWalkBool(direction), false);
                animator.SetBool(DirectionToIdleBool(direction), true);
            }
        }
    }
    // assumes one unit away and destination is a valid entry
    public void AnimateAttackSword(Direction direction){
        GameObject player = character; 
        Debug.Log(direction);
        GameObject sword = player.transform.Find("").gameObject;
        Coordinate playerCoordinate = instance.board.FindCharId(charId); 
        Coordinate enemyCoordinate = Coordinate.Add(playerCoordinate, DirectionToCoordinate(direction));
        GameObject enemy = instance.charArray[instance.board.Get(enemyCoordinate)];


        string attackCommand = DirectionToAttackSwordTrigger(direction); 
        if (this.animatorParameterNames.Contains(attackCommand)){
            this.animator.ResetTrigger(attackCommand);

            // update orders 


            this.animator.SetTrigger(attackCommand);
        }
    }


    // need to do appropriate character ordering layers: 
    // player, enemy, sword,
    // standard: 3, 3, 2
    // up:  4,2,3
    // down: 3, 3, 4
    // left: 3, 3, 4 
    // right: 3, 3, 4 

}