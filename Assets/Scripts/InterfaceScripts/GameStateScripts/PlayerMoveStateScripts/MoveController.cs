using System;
using System.Collections;
using System.Collections.Generic;
using log4net.Util;
using UnityEngine;
using UnityEngine.UI;

/*
                    *** DO WE WANT TO RENAME THIS??? ***

MoveController houses during the player phase logic for issuing commands to units. 
In other words, this handles the mouse actions that selects. 

Things it handles: 
- uses eventHandlers to signify changes in selected characters as well as enemies
- houses a mouse click manager with logic that controls given the state of the board, what 
  you are allowed to select (e.g. if you have a player unit selected vs an enemy unit 
  vs nothing selected)

- there are only two ways to control movement: 
    - clicking during the player phase, and 
    - clicking buttons during the player phase

*/

public class MoveController : MonoBehaviour
{
    public MoveController moveController;  
    [SerializeField] StatMenuManager statMenuManager; 
    public Board board; 
    public GameManager instance; 
    public Coordinate arrowCoordinate;
    private Camera mainCamera;      // needed for mouse position;

    public PlayerMoveControllerStateMachine playerMoveControllerStateMachine; 

    // selectedId w/ delegate and event
    public delegate void SelectedCharIdChangedEventHandler(int selectedId);
    // Define the event based on the delegate
    public static event SelectedCharIdChangedEventHandler OnSelectedCharIdChanged;
    private int selectedId;
    public int SelectedId {
        get { return selectedId; }
        set {
            if (selectedId != value){
                selectedId = value;
                // Invoke the event whenever score changes
            } else {
                selectedId = -1;  
            }
            OnSelectedCharIdChanged?.Invoke(selectedId);
        }
    }
    // Enemy Target 
    public delegate void SelectedEnemyIdChangedEventHandler(int selectedEnemyId);
    // Define the event based on the delegate
    public static event SelectedEnemyIdChangedEventHandler OnSelectedEnemyIdChanged;
    private int selectedEnemyId;
    public int SelectedEnemyId {
        get { return selectedEnemyId; }
        set {
            if (selectedEnemyId != value){
                selectedEnemyId = value;
                // Invoke the event whenever score changes
            } else {
                selectedEnemyId = -1;  
            }
            OnSelectedEnemyIdChanged?.Invoke(selectedEnemyId);
        }
    }

    public Stack<Coordinate> moveStack;

    public Boolean IsPlayerSelected(){
        return selectedId != -1; 
    }

    public Boolean IsEnemySelected(){
        return SelectedEnemyId != -1; 
    }
    
    private (Boolean, Boolean) GetSelectedState(){
        return (IsPlayerSelected(), IsEnemySelected());
    }

    private void Awake() {
        moveController = this;
    }


    // Start is called before the first frame update
    private void Start() {
        mainCamera = Camera.main;
        ResetSelected();
        playerMoveControllerStateMachine.Initialize();
    }

    // Update is called once per frame
    public void Update() {
        UpdateMouseClickManager();
    }

    public void Initialize(GameManager instance){
        this.instance = instance; 
        this.board = instance.board; 
        this.arrowCoordinate = instance.board.MiddleBoardCoordinate();     
        this.moveStack = new Stack<Coordinate>();
        this.playerMoveControllerStateMachine = new PlayerMoveControllerStateMachine(instance, moveController);
    }

    /// <summary>
    /// This function is called during Update and defines when click is allowed (during 
    /// player phase) and the controller used to process the click. 
    /// </summary>
    private void UpdateMouseClickManager(){
        Vector3 mousePosition = Input.mousePosition;
        Coordinate w = GetMouseClickCoordinate(mousePosition);
        if (instance.gameStateMachine.IsPlayerState() && 
                Input.GetMouseButtonDown(0) && 
                board.IsWithinBoard(w))
        {
            playerMoveControllerStateMachine.ProcessClick(w);
        }
    }

    public void UndoMove(){
        if (moveStack.Count > 1){
            Coordinate curPos = moveStack.Pop();
            Coordinate prevPos = moveStack.Pop();
            int unitId = board.Get(curPos); 
            instance.charArray[unitId].GetComponent<CharacterMove>().UndoMoveChar(prevPos);
            instance.highlightTilesManager.ResetTiles();
            instance.highlightTilesManager.TriggerSelectedHighlights();
            UnselectEnemyUnit();
        }
    }

    // once you attack the stack gets reset
    public void ClearMoveStack(){
        moveStack.Clear();
    }

    public void ResetSelected(){
        UnselectUnit();
        UnselectEnemyUnit(); 
        ClearMoveStack();
        instance.highlightTilesManager.ResetTiles();
    }

    public Boolean SelectedIdHasMoved(){
        return selectedId != -1 && instance.charArray[selectedId].GetComponent<CharacterGameState>().HasMoved;
    }

    public Boolean SelectedIdPerformedAction(){
        return selectedId != -1 && instance.charArray[selectedId].GetComponent<CharacterGameState>().PerformedAction;
    }

    // requires selectedId != -1
    public Boolean IsPossibleMove(Coordinate w){
        if (selectedId != -1){
            return instance.charArray[SelectedId].GetComponent<CharacterMove>().PossibleMoves().Contains(w);
        }
        return false; 
    }

    /// <summary>
    /// Converts mouse position to a coordinate on the board matrix
    /// </summary>
    /// <param name="mousePosition"> your click position; maybe hover too? </param>
    /// <returns></returns>
    private Coordinate GetMouseClickCoordinate(Vector3 mousePosition){
        // Convert mouse position to world coordinates
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(mousePosition);
        // Set the z-coordinate to 0 to ensure it's on the same plane as the board
        worldPosition.z = 0; 
        // add 0.5 to both x and y and take the floor to adjust to the center of the tile, 
        // which is a t0.0 for the bottom left corner
        Coordinate w = board.ConvertSceneToMatCoords(
            (double) Math.Floor(worldPosition.x + 0.5), 
            (double) Math.Floor(worldPosition.y + 0.5));
        return w;
    }

    /// <summary>
    /// Given a coordinate w, apply select unit to the model
    /// </summary>
    public void SelectUnit(Coordinate w){
        // change the selected, stored in gameManager
        if (board.IsWithinBoard(w) && board.Get(w) != SelectedId){                    
            SelectedId = board.Get(w); 
        }   
    }

    /// <summary>
    /// Given a coordinate w, apply select enemy unit to the model
    /// </summary>
    public void SelectEnemyUnit(Coordinate w){
        int charId = board.Get(w);
        if (board.IsWithinBoard(w) && charId != SelectedId && charId != -1 &&
            !instance.charArray[charId].GetComponent<CharacterGameState>().isYourTeam){     
            SelectedEnemyId = charId; 
        }   
    }

    public void UnselectUnit(){
        SelectedId = -1;
    }
    public void UnselectEnemyUnit(){
        SelectedEnemyId = -1;
    }
    /// <summary>
    /// Checks whether enemy is in the list of possible moves
    /// </summary>
    public Boolean IsTargetAttackableEnemy(Coordinate w){
        if (SelectedId == -1){
            return false; 
        }
        if (IsTargetAnEnemy(w)){
            HashSet<Coordinate> attackTargets = instance.charArray[SelectedId].GetComponent<CharacterMove>().PossibleAttackTargets();
            return attackTargets.Contains(w);
        }
        return false; 
    }

    /// <summary>
    /// Checks whether coordinate w unit is on player team
    /// </summary>
    public Boolean IsTargetOnTeam(Coordinate w){
        if (board.Get(w) == -1){
            return false; 
        }
        return instance.charArray[board.Get(w)].GetComponent<CharacterGameState>().isYourTeam; 
    }

    public Boolean IsTargetAnEnemy(Coordinate w){
        if (board.Get(w) == -1){
            return false; 
        }
        return !instance.charArray[board.Get(w)].GetComponent<CharacterGameState>().isYourTeam; 
    }

    /// <summary>
    /// Note: returns false if w or v are not character units i.e. they're blank
    /// </summary>
    public Boolean IsSameTeam(Coordinate w, Coordinate v){
        if (board.Get(w) == -1 || board.Get(v) == -1){
            return false; 
        } 
        return instance.charArray[board.Get(w)].GetComponent<CharacterGameState>().isYourTeam == 
                instance.charArray[board.Get(v)].GetComponent<CharacterGameState>().isYourTeam;
    }

    /// <summary>
    /// Could be Friend or empty
    /// </summary>
    public Boolean IsNotEnemy(Coordinate friendlyUnit, Coordinate comparedUnit){
        if (board.Get(comparedUnit) == -1){
            return true;
        }
        return IsSameTeam(friendlyUnit, comparedUnit);  
    }


    /// <summary>
    /// Note: returns false if w or v are not character units i.e. they're blank
    /// </summary>
    public Boolean IsEnemy(Coordinate w, Coordinate v){
        if (board.Get(w) == -1 || board.Get(v) == -1){
            return false; 
        } 
        return instance.charArray[board.Get(w)].GetComponent<CharacterGameState>().isYourTeam != 
                instance.charArray[board.Get(v)].GetComponent<CharacterGameState>().isYourTeam;
    }


    public ClickTarget GetClickTarget(Coordinate w){
        if (IsTargetOnTeam(w)){
            return ClickTarget.friend; 
        } else if (IsTargetAnEnemy(w)){
            return ClickTarget.enemy; 
        } else {
            return ClickTarget.empty; 
        }
    }


    private void UpdateMouseClickManagerDEPRECATED(){
        if (instance.gameStateMachine.IsPlayerState() && Input.GetMouseButtonDown(0)){
            Vector3 mousePosition = Input.mousePosition;
            // Debug.Log("mouse position: " + mousePosition); 
            Coordinate w = GetMouseClickCoordinate(mousePosition);
            // Debug.Log("click position: " + w);
            // Debug.Log("is within board? : " + board.IsWithinBoard(w));

            if (board.IsWithinBoard(w)){
                // Debug.Log("testing board" + board.Get(new Coordinate(0,0)));
                int targetBoardId = board.Get(w); 
                // Debug.Log("target boardId: " + targetBoardId);
                (Boolean, Boolean) curState = GetSelectedState();

                // you move, click elsewhere --> undo move, undo select; and if you clicked on a 
                // team unit, select that team
                if (IsPlayerSelected() && SelectedIdHasMoved() && !IsTargetAttackableEnemy(w)){     
                    UndoMove(); 
                    // UnselectUnit(); 
                    ResetSelected();
                    if (IsTargetOnTeam(w) && selectedId != targetBoardId){
                        SelectUnit(w);
                    }
                    else if (IsTargetAnEnemy(w)){
                        // Debug.Log("this is triggered: selected player, moved, and then selected enemy non-target");
                        SelectEnemyUnit(w);
                    }
                }

                // select enemy if current target has moved

                // move unit condition
                else if ((curState == (true, false) || curState == (true, true)) && IsPossibleMove(w) && SelectedId != targetBoardId){
                    moveStack.Push(board.FindCharId(SelectedId)); 
                    moveStack.Push(w);
                    instance.charArray[SelectedId].GetComponent<CharacterMove>().MoveChar(w);
                    UnselectEnemyUnit();
                }


                // you click on a team unit, and you either had: nothing selected, 
                // a player selected that's not the current target, 
                // an enemy selected, or a player selected and an enemy selected and target 
                // is not the current player selected 
                // --> unselect enemy unit, select the new player unit
                else if 
                    // ((curState == (false, false) && IsTargetOnTeam(w)) || 
                    // (curState == (true, false) && IsTargetOnTeam(w) && SelectedId != targetBoardId) ||
                    // (curState == (false, true) && IsTargetOnTeam(w) )||
                    // (curState == (true, true) && IsTargetOnTeam(w) && SelectedId != targetBoardId))

                // refactored code: 
                // if player selected and new target is on the team and different OR 
                // if not player selected and select a new team target --> select player target, unselect enemy unit
                    ((IsPlayerSelected() && IsTargetOnTeam(w) && SelectedId != targetBoardId) ||
                    (!IsPlayerSelected() && IsTargetOnTeam(w)))
                {
                    ResetSelected();
                    SelectedId = targetBoardId;
                    // UnselectEnemyUnit();
                }

                // selected a team unit; then click on blank spot, unselect all
                // refactored: if anything selected and target is a blank --> reset
                else if (targetBoardId == -1 && (IsPlayerSelected() || IsEnemySelected()))                
                {
                    // UnselectEnemyUnit();
                    // UnselectUnit();
                    ResetSelected();
                }

                // selected enemy, click on blank spot --> unselect enemy unit (reset?)
                // else if (IsEnemySelected() && targetBoardId == -1){
                //     // UnselectEnemyUnit();
                //     ResetSelected();
                // }

                // if selected, you move, and you click on something else other than a target, call undo move
                // select enemy unit for stats
                else if ((curState == (false, false) && IsTargetAnEnemy(w)) || 
                    (curState == (true, false) && IsTargetAnEnemy(w) && !IsTargetAttackableEnemy(w)) ||
                    (curState == (false, true) && IsTargetAnEnemy(w) && SelectedEnemyId != targetBoardId))            
                {
                    ResetSelected(); 
                    SelectedEnemyId = board.Get(w);
                    // UnselectUnit();
                }
                // unselect all 
                else if ((IsPlayerSelected() && SelectedId == targetBoardId) || 
                    (curState == (false, true) && SelectedEnemyId == targetBoardId) ||
                    // (curState == (true, true) && SelectedId == targetBoardId) ||
                    (IsPlayerSelected() && SelectedIdPerformedAction() && targetBoardId == -1))
                {
                    // UnselectUnit();
                    // UnselectEnemyUnit();
                    Debug.Log("AA triggered");
                    ResetSelected(); 
                }
                else if (IsPlayerSelected() && SelectedIdPerformedAction() && IsTargetAnEnemy(w)){
                    // UnselectUnit();
                    ResetSelected(); 
                    SelectEnemyUnit(w);
                }

                // select enemy unit as a target
                else if ((curState == (true, false) || curState == (true, true)) &&
                    IsTargetAnEnemy(w) && IsTargetAttackableEnemy(w))
                {
                    SelectEnemyUnit(w);
                }
                instance.highlightTilesManager.TriggerSelectedHighlights(); 
            }
        }
    }




}
