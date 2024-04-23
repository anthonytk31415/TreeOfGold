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

    public Queue<CharacterAnimateCommandData> animateQueue;

    public CharacterAnimateCommandData characterAnimateCommandData; 

    public CharacterAnimateCommandIdle characterAnimateCommandIdle; 
    public CharacterAnimateCommandWalk characterAnimateCommandWalk;
    public CharacterAnimateCommandAttackSword characterAnimateCommandAttackSword;
    public CharacterAnimateCommandBlinkBlack characterAnimateCommandBlinkBlack;
    
    public CharacterAnimateCommandBlinkRed characterAnimateCommandBlinkRed;
    public ICharacterAnimateCommand characterAnimateCommand; 
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
    /// Call this after you add component to the character object during startup. Be sure to add all 
    /// AnimateCommands here. 
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
        // this.animatorParameterNames = new HashSet<string>();
        // foreach (var parameter in this.animator.parameters){
        //     this.animatorParameterNames.Add(parameter.name); 
        // };
        this.direction = Direction.down;
        this.moveQueue = new();
        this.animateQueue = new();
        this.wip = false; 

        //  put all AnimateCommands here
        this.characterAnimateCommandIdle = new CharacterAnimateCommandIdle(this.instance, this.character, this);
        this.characterAnimateCommandWalk = new CharacterAnimateCommandWalk(this.instance, this.character, this);
        this.characterAnimateCommandAttackSword = new CharacterAnimateCommandAttackSword(this.instance, this.character, this);
        this.characterAnimateCommandBlinkBlack = new CharacterAnimateCommandBlinkBlack(this.instance, this.character, this);
        this.characterAnimateCommandBlinkRed = new CharacterAnimateCommandBlinkRed(this.instance, this.character, this);
        Debug.Log("initialize called for controller");

        // ApplyInitialState();
        // ApplyQueueDefaultState();
    }

    /// <summary>
    ///  Make all parameters false. 
    /// </summary>
    // public void ApplyAnimationDefaultState()
    // {
    //     if (animator == null){ return;}
    //     foreach (var param in animator.parameters)
    //     {
    //         if (param.name.Contains("Bool")){
    //             animator.SetBool(param.name, false);
    //         }
    //     }
    // }

    /// <summary>
    /// Make all parameters false and set to default state -> idleDownBool = false.  
    /// </summary>
    // public void ApplyInitialState()
    // {
    //     // ApplyAnimationDefaultState();
    //     // character.GetComponent<Animator>().SetTrigger("idleDownTrigger");
    //     // animator.SetBool("idleDownBool", true);
    //     // Debug.Log("current stance of idleDownBOol: " + animator.GetBool("idleDownBool"));
    // }


    // // for the series of "animate" commands, do we want to put them as an abstract class method 
    // // within the CharacterAnimateCommandXXXX class?
    // public void AnimateIdle(Direction direction){
    //     CharacterAnimateCommandData idleData = new CharacterAnimateCommandData(
    //             0.0f, 
    //             characterAnimateCommandIdle, 
    //             direction, 
    //             new Vector2(-99, -99), 
    //             false);
    //     animateQueue.Enqueue(idleData); 
    // }

    // public void AnimateAttackSword(Direction direction){
    //     Debug.Log("activating attack sword");
    //     CharacterAnimateCommandData attackSwordData = new CharacterAnimateCommandData(
    //             0.333f, 
    //             characterAnimateCommandAttackSword, 
    //             direction, 
    //             new Vector2(-99, -99), 
    //             false);
    //     animateQueue.Enqueue(attackSwordData); 

    // }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="destination"></param>
    // public void AnimateMoveChar(Coordinate destination) {
    //     Coordinate start = instance.board.FindCharId(charId);
    //     List<Coordinate> pathList = CharInteraction.ShortestPathBetweenCoordinates(instance, start, destination);
    //     Debug.Log("path to go to: ");
    //     AuditDebug.DebugIter(pathList);
    //     foreach (Coordinate nextCoordinate in pathList) {
    //         Direction nextDir = Coordinate.DirectionFromAdjacentCoordinates(start, nextCoordinate); 
    //         (double x, double y ) = instance.board.ConvertMatToSceneCoords(start);
    //         Vector2 finalVector = new Vector2((float)x, (float)y) + DirectionUtility.DirectionToVector(nextDir);
    //         CharacterAnimateCommandData moveData = new CharacterAnimateCommandData(
    //                 .12f, 
    //                 characterAnimateCommandWalk, 
    //                 nextDir, 
    //                 finalVector, 
    //                 true);
    //         animateQueue.Enqueue(moveData);
    //         start = nextCoordinate; 
    //     }     
    // }

    // public void AddAnimateCommand(CharacterAnimateCommandData characterAnimateCommandData){
    //     CharacterAnimateCommand characterAnimateCommand = characterAnimateCommandData.characterAnimateCommand; 

    // }


    /// <summary>
    /// The main update method. 
    /// </summary>
    public void UpdateAnimation(){
        
        if (!this.wip && animateQueue.Count > 0){
            this.characterAnimateCommandData = animateQueue.Dequeue();
            // Debug.Log("charanimateCommand: " + characterAnimateCommandData);
            this.wip = true; 
            this.characterAnimateCommand = characterAnimateCommandData.characterAnimateCommand; 
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
        // if (animateQueue.Count == 0){
        //     ApplyAnimationDefaultState();     
        //     animator.SetBool(DirectionUtility.DirectionToIdleBool(Direction.down), true);
        // }
    } 

    /// <summary>
    /// looksup the proper characterAnimateCommand to apply based on type
    ///  (Outstanding: whats the best way to add new commands as time goes on? )
    /// </summary>
    /// <param name="characterAnimateType"></param>
    /// <returns></returns>
    // public CharacterAnimateCommand GetCharacterAnimateCommand(CharacterAnimateType characterAnimateType)
    // {
    //     Dictionary<CharacterAnimateType,CharacterAnimateCommand> lookup = new () {
    //         {CharacterAnimateType.idle, this.characterAnimateCommandIdle}, 
    //         {CharacterAnimateType.walk, this.characterAnimateCommandWalk}, 
    //         {CharacterAnimateType.attackSword, this.characterAnimateCommandAttackSword}, 
    //     };
    //     if (lookup.ContainsKey(characterAnimateType)){
    //         return lookup[characterAnimateType];
    //     }         
    //     return characterAnimateCommandIdle; 
    // }



    // assumes one unit away and destination is a valid entry
    // public void AnimateAttackSwordOld(Direction direction){
    //     GameObject player = character; 
    //     // Debug.Log(direction);
    //     GameObject sword = player.transform.Find("").gameObject;
    //     Coordinate playerCoordinate = instance.board.FindCharId(charId); 
    //     Coordinate enemyCoordinate = Coordinate.Add(playerCoordinate, DirectionUtility.DirectionToCoordinate(direction));
    //     GameObject enemy = instance.charArray[instance.board.Get(enemyCoordinate)];


    //     string attackCommand = DirectionUtility.DirectionToAttackSwordTrigger(direction); 
    //     if (this.animatorParameterNames.Contains(attackCommand)){
    //         this.animator.ResetTrigger(attackCommand);
    //         // update orders 
    //         this.animator.SetTrigger(attackCommand);
    //     }
    // }


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

}