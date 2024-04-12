using UnityEngine;
using UnityEngine.UIElements;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.TextCore.Text;
using System.Linq;



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

    // public float curTime; 
    // private Vector2 startPos; 
    public Vector2 endPos; 
    public Direction direction;

    private float endTime; 
    public bool traversal; 
    public bool wip; 
    private Queue<Direction> moveQueue; 

    private Queue<CharacterAnimateCommandData> animateQueue;

    public CharacterAnimateCommandData characterAnimateCommandData; 
    private CharacterAnimateCommand characterAnimateCommand;

    public CharacterAnimateCommandIdle characterAnimateCommandIdle; 
    public CharacterAnimateCommandWalk characterAnimateCommandWalk;

    public void Start(){
        // SelectedMovedState.OnPositionResetChanged += HandlePositionResetChange;
    }

    public void Update ()
    {
        // UpdateMovement();
        UpdateAnimation();
    }

    public void Exit() {
        // SelectedMovedState.OnPositionResetChanged -= HandlePositionResetChange;
    }

    public void HandlePositionResetChange(bool positionReset) {
        // Debug.Log("position triggered");
        // ApplyInitialState();
    }

    /// <summary>
    /// Call this after you add component to the character object during startup. 
    /// </summary>
    /// <param name="instance"></param>
    /// <param name="charId"></param>
    /// <param name="character"></param>
    public void Initialize(GameManager instance, int charId, GameObject character)
    {
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
        this.animateQueue = new();
        this.wip = false; 

        this.characterAnimateCommandIdle = new CharacterAnimateCommandIdle(this.instance, this.character, this);
        this.characterAnimateCommandWalk = new CharacterAnimateCommandWalk(this.instance, this.character, this);
            // yes

        ApplyInitialState();
        // ApplyQueueDefaultState();
    }

    /// <summary>
    ///  Make all parameters false. 
    /// </summary>
    public void ApplyAnimationDefaultState()
    {
        if (animator == null){ return;}
        foreach (var param in animator.parameters)
        {
            if (param.name.Contains("Bool")){
                animator.SetBool(param.name, false);
            }
        }
    }

    /// <summary>
    /// Make all parameters false and set to default state -> idleDownBool = false.  
    /// </summary>
    public void ApplyInitialState()
    {
        ApplyAnimationDefaultState();
        animator.SetBool("idleDownBool", true);
        // Debug.Log("current stance of idleDownBOol: " + animator.GetBool("idleDownBool"));
    }

    public void AnimateIdle(Direction direction){
        CharacterAnimateCommandData idleData = new CharacterAnimateCommandData(
                0.0f, 
                CharacterAnimateType.idle, 
                direction, 
                new Vector2(-99, -99), 
                false);
        animateQueue.Enqueue(idleData); 
    }

    // having trouble with coordinates 
    /// <summary>
    ///  ** need to clean up code below and also deal with isseu where 
    ///  you dont go back to "idledown" after the move is completed. 
    ///  I think the queue i s working. 
    /// </summary>
    /// <param name="destination"></param>
    public void AnimateMoveChar(Coordinate destination) {
        Coordinate start = instance.board.FindCharId(charId);
        // (double x, double y) = instance.board.ConvertMatToSceneCoords(start);
        List<Coordinate> pathList = CharInteraction.PathBetweenUnits(instance, start, destination);
        foreach (Coordinate nextCoordinate in pathList) {
            Direction nextDir = Coordinate.DirectionFromAdjacentCoordinates(start, nextCoordinate); 
            (double x, double y ) = instance.board.ConvertMatToSceneCoords(start);
            // Debug.Log("vector of start: " + x + "; " + y);
            // Debug.
            Vector2 finalVector = new Vector2((float)x, (float)y) + DirectionUtility.DirectionToVector(nextDir);
            // Debug.Log("start " + start + "; final vector: " + finalVector);
            CharacterAnimateCommandData moveData = new CharacterAnimateCommandData(
                    .12f, 
                    CharacterAnimateType.walk, 
                    nextDir, 
                    finalVector, 
                    true);
            animateQueue.Enqueue(moveData);
            start = nextCoordinate; 
        }     
        // Debug.Log("pathlist: " );
        // AuditDebug.DebugIter(pathList); 
        // Debug.Log("animateQueue: " );
        // AuditDebug.DebugIter(animateQueue);
    }

    // public void AnimateMoveCharOLD(int charId, Coordinate destination) {
    //     Coordinate start = instance.board.FindCharId(charId);
    //     List<Coordinate> pathList = CharInteraction.PathBetweenUnits(instance, start, destination);
    //     foreach (Coordinate nextCoordinate in pathList) {
    //         Direction nextDir = Coordinate.DirectionFromAdjacentCoordinates(start, nextCoordinate); 
    //         moveQueue.Enqueue(nextDir);
    //         start = nextCoordinate; 
    //     }             
    // }
    

    // public void InstantiateTraversal(Direction direction)
    // {
    //     this.traversal = true;
    //     startPos = character.transform.position;
    //     endPos = startPos + DirectionUtility.DirectionToVector(direction); 
    //     // curTime = 0.0f;
    //     endTime = .12f;
    //     animator.SetBool(DirectionUtility.DirectionToIdleBool(this.direction), false);
    //     this.direction = direction;
    //     animator.SetBool(DirectionUtility.DirectionToWalkBool(direction), true);
    // }

    /// <summary>
    /// The main update method. 
    /// </summary>
    public void UpdateAnimation(){
        
        if (!this.wip && animateQueue.Count > 0){
            this.characterAnimateCommandData = animateQueue.Dequeue();
            // Debug.Log("charanimateCommand: " + characterAnimateCommandData);
            this.wip = true; 
            this.characterAnimateCommand = GetCharacterAnimateCommand(this.characterAnimateCommandData.characterAnimateType); 
            // Debug.Log(this.characterAnimateCommand);
            this.characterAnimateCommand.InstantiateCommand();
        }
        else if (this.wip){
            // Debug.Log("else called"); 
            if (this.characterAnimateCommand.Processing()){
                // Debug.Log("preprocessing...");
                this.characterAnimateCommand.ProcessCommand(); 
            }
            else {
                this.wip = false;
                this.characterAnimateCommand.TerminateCommand();
                // ApplyQueueDefaultState();
            }
        }
        if (animateQueue.Count == 0){
            ApplyAnimationDefaultState();     
            animator.SetBool(DirectionUtility.DirectionToIdleBool(Direction.down), true);
        }
    } 

    public CharacterAnimateCommand GetCharacterAnimateCommand(CharacterAnimateType characterAnimateType)
    {
        Dictionary<CharacterAnimateType,CharacterAnimateCommand> lookup = new () {
            {CharacterAnimateType.idle, this.characterAnimateCommandIdle}, 
            {CharacterAnimateType.walk, this.characterAnimateCommandWalk}, 
        };
        if (lookup.ContainsKey(characterAnimateType)){
            return lookup[characterAnimateType];
        }         
        return characterAnimateCommandIdle; 
    }

    // public void InstantiateCommand(CharacterAnimateCommand curCommand){
    //     this.wip = true; 
    //     // this.characterAnimateCommand = curCommand;
    //     // apply bindings to the properties         
    // }

    // public void ApplyQueueDefaultState(){
        // this.wip = false; 
        // this.curTime = 0.0f; 
        // animator.SetBool(DirectionUtility.DirectionToWalkBool(direction), false);
    // }



    // assumes one unit away and destination is a valid entry
    public void AnimateAttackSword(Direction direction){
        GameObject player = character; 
        // Debug.Log(direction);
        GameObject sword = player.transform.Find("").gameObject;
        Coordinate playerCoordinate = instance.board.FindCharId(charId); 
        Coordinate enemyCoordinate = Coordinate.Add(playerCoordinate, DirectionUtility.DirectionToCoordinate(direction));
        GameObject enemy = instance.charArray[instance.board.Get(enemyCoordinate)];


        string attackCommand = DirectionUtility.DirectionToAttackSwordTrigger(direction); 
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



    /// <summary>
    ///  to deprecate below...
    /// </summary>
    // public void UpdateMovement() 
    // {
    //     if (!this.traversal && moveQueue.Count > 0){
    //         Direction curDirection = moveQueue.Dequeue();
    //         InstantiateTraversal(curDirection);
    //     }

    //     else if (this.traversal){
    //         if (curTime < endTime){            
    //             float lerpValue = Mathf.Lerp(0, 1, curTime / endTime); 
    //             character.transform.position = startPos + DirectionUtility.DirectionToVector(direction)*lerpValue;
    //             curTime += Time.deltaTime; 
    //         }         
    //         else {
    //             character.transform.position = this.endPos; 
    //             this.traversal = false; 
    //             this.curTime = 0.0f; 
    //             animator.SetBool(DirectionUtility.DirectionToWalkBool(direction), false);
    //             animator.SetBool(DirectionUtility.DirectionToIdleBool(Direction.down), true);
    //         }
    //     }
    // }
}