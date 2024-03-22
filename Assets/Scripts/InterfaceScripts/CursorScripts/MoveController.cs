using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


// MoveController houses during the player phase logic for issuing commands to units. 
// Things it handles: 
// - uses eventHandlers to signify changes in selected characters as well as enemies
// - houses a mouse click manager with logic that controls given the state of the board, what 
//   you are allowed to select (e.g. if you have a player unit selected vs an enemy unit 
//   vs nothing selected)


public class MoveController : MonoBehaviour
{
    [SerializeField] StatMenuManager statMenuManager; 
    public Board board; 
    public GameManager instance; 
    public Coordinate arrowCoordinate;
    private Camera mainCamera;      // needed for mouse position;
    // public string m_Text;        // used for touch commands; tbd

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

    private Stack<Coordinate> moveStack;

    public Boolean IsPlayerSelected(){
        return selectedId != -1; 
    }

    public Boolean IsEnemySelected(){
        return SelectedEnemyId != -1; 
    }
    private (Boolean, Boolean) GetSelectedState(){
        return (IsPlayerSelected(), IsEnemySelected());
    }

    // Start is called before the first frame update
    private void Start() {
        mainCamera = Camera.main;
        selectedId = -1;
        selectedEnemyId = -1;
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
    }

    private void UpdateMouseClickManager(){
        if (Input.GetMouseButtonDown(0)){
            Vector3 mousePosition = Input.mousePosition;
            Coordinate w = GetMouseClickCoordinate(mousePosition);
            if (board.IsWithinBoard(w)){
                int targetBoardId = board.Get(w); 
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
                        Debug.Log("this is triggered: selected player, moved, and then selected enemy non-target");
                        SelectEnemyUnit(w);
                    }
                }

                // select enemy if current target has moved

                // move unit condition
                else if ((curState == (true, false) || curState == (true, true)) && IsPossibleMove(w)){
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

                instance.cursorStateMachine.chooseState.TriggerSelectedHighlights(); 
            }
        }
    }

    public void UndoMove(){
        if (moveStack.Count > 1){
            Coordinate curPos = moveStack.Pop();
            Coordinate prevPos = moveStack.Pop();
            int unitId = board.Get(curPos); 
            instance.charArray[unitId].GetComponent<CharacterMove>().UndoMoveChar(prevPos);
            instance.cursorStateMachine.chooseState.ResetTiles(); 
            instance.cursorStateMachine.chooseState.TriggerSelectedHighlights();
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
        instance.cursorStateMachine.chooseState.ResetTiles(); 
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

    private Coordinate GetMouseClickCoordinate(Vector3 mousePosition){
        // Convert mouse position to world coordinates
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(mousePosition);
        // Set the z-coordinate to 0 to ensure it's on the same plane as the board
        worldPosition.z = 0; 
        // add 0.5 to both x and y and take the floor to adjust to the center of the tile, 
        // which isa t0.0 for the bottom left corner
        Coordinate w = board.ConvertSceneToMatCoords(
            (double) Math.Floor(worldPosition.x + 0.5), 
            (double) Math.Floor(worldPosition.y + 0.5));
        return w;
    }

    private void SelectUnit(Coordinate w){
        // change the selected, stored in gameManager
        if (board.IsWithinBoard(w) && board.Get(w) != SelectedId){                    
            SelectedId = board.Get(w); 
        }   
    }
    private void SelectEnemyUnit(Coordinate w){
        int charId = board.Get(w);
        if (board.IsWithinBoard(w) && charId != SelectedId && charId != -1 &&
            !instance.charArray[charId].GetComponent<CharacterGameState>().isYourTeam){     
            SelectedEnemyId = charId; 
        }   
    }

    private void UnselectUnit(){
        SelectedId = -1;
    }
    private void UnselectEnemyUnit(){
        SelectedEnemyId = -1;
    }

    // checks whether enemy is in the list of possible moves
    private Boolean IsTargetAttackableEnemy(Coordinate w){
        if (SelectedId == -1){
            return false; 
        }
        if (IsTargetAnEnemy(w)){
            HashSet<Coordinate> attackTargets = instance.charArray[SelectedId].GetComponent<CharacterMove>().PossibleAttackTargets();
            return attackTargets.Contains(w);
        }
        return false; 
    }

    private Boolean IsTargetOnTeam(Coordinate w){
        if (board.Get(w) == -1){
            return false; 
        }
        return instance.charArray[board.Get(w)].GetComponent<CharacterGameState>().isYourTeam; 
    }

    private Boolean IsTargetAnEnemy(Coordinate w){
        if (board.Get(w) == -1){
            return false; 
        }
        return !instance.charArray[board.Get(w)].GetComponent<CharacterGameState>().isYourTeam; 
    }


}
