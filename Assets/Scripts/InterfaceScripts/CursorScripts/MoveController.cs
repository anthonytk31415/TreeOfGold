using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// this changes the position of items on a grid; 
// need to "gatekeep"


public class MoveController : MonoBehaviour
{
    [SerializeField] StatMenuManager statMenuManager; 
    public Board board; 
    public GameManager instance; 
    public Coordinate arrowCoordinate;
    private Camera mainCamera;      // needed for mouse position;
    // public string m_Text;        // used for touch commands; tbd

    // selectedid w/ delegate and event
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

    public Boolean IsSelected(){
        return selectedId != -1; 
    }

    public Boolean IsEnemySelected(){
        return SelectedEnemyId != -1; 
    }


    // Start is called before the first frame update
    private void Start() {
        // TouchSimulation.Enable;
        mainCamera = Camera.main;
        selectedId = -1;
        selectedEnemyId = -1;
    }

    // Update is called once per frame
    public void Update() {
        // CursorUpdatePosition();
        UpdateMouseClickManager();
    }

    public void Initialize(Board board, GameManager instance){
        this.board = board; 
        this.arrowCoordinate = board.MiddleBoardCoordinate();     
        this.instance = instance; 
        this.moveStack = new Stack<Coordinate>();
    }

    private void UpdateMouseClickManager(){
        if (Input.GetMouseButtonDown(0)){
            Vector3 mousePosition = Input.mousePosition;
            Coordinate w = GetMouseClickCoordinate(mousePosition);
            int charIdLookup = board.Get(w); 
            (Boolean, Boolean) curState = GetSelectedState();
            Debug.Log("current state: " + curState);
            if ((curState == (false, false) && IsTargetOnTeam(w)) || 
                (curState == (true, false) && IsTargetOnTeam(w) && SelectedId != charIdLookup) ||
                (curState == (false, true) && IsTargetOnTeam(w) )||
                (curState == (true, true) && IsTargetOnTeam(w) && SelectedId != charIdLookup)){
                SelectedId = charIdLookup;
                UnselectEnemyUnit();

            }
            else if ((curState == (false, false) && IsTargetAnEnemy(w)) || 
                (curState == (true, false) && IsTargetAnEnemy(w) && !IsTargetAttackableEnemy(w)) ||
                (curState == (false, true) && IsTargetAnEnemy(w))){
                SelectedEnemyId = board.Get(w);
                UnselectUnit();

            }

            else if ((curState == (true, false) && SelectedId == charIdLookup) || 
                (curState == (false, true) && SelectedEnemyId == charIdLookup) ||
                (curState == (true, true) && SelectedId == charIdLookup)){
                UnselectUnit();
                UnselectEnemyUnit();

            }

            else if ((curState == (true, false) && IsTargetAnEnemy(w) && IsTargetAttackableEnemy(w)) || 
                (curState == (true, true) && IsTargetAnEnemy(w) && IsTargetAttackableEnemy(w))){
                SelectEnemyUnit(w);
            }

            else if ((curState == (true, false) || curState == (true, true)) &&
                      instance.charArray[SelectedId].GetComponent<CharacterMove>().PossibleMoves().Contains(w)){
                moveStack.Push(board.FindCharId(SelectedId)); 
                moveStack.Push(w);
                instance.charArray[SelectedId].GetComponent<CharacterMove>().MoveChar(w);
                instance.cursorStateMachine.chooseState.TriggerSelectedHighlights(); 
            }

            // deal with attack here


        }
    }

    public void UndoMove(){
        if (moveStack.Count > 1){
            Coordinate curPos = moveStack.Pop();
            Coordinate prevPos = moveStack.Pop();
            int unitId = board.Get(curPos); 
            instance.charArray[unitId].GetComponent<CharacterMove>().UndoMoveChar(prevPos);
            instance.cursorStateMachine.chooseState.ResetBoard(); 
            instance.cursorStateMachine.chooseState.TriggerSelectedHighlights();
        }
    }

    // once you attack the stack gets reset
    public void ClearMoveStack(){
        this.moveStack = new Stack<Coordinate>();
    }


    private Coordinate GetMouseClickCoordinate(Vector3 mousePosition){
        // Convert mouse position to world coordinates
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(mousePosition);
        worldPosition.z = 0; // Set the z-coordinate to 0 to ensure it's on the same plane as the board

        // add 0.5 to both x and y and take the floor to adjust to the center of the tile, which isa t0.0 for the bottom lefet corner
        Coordinate w = board.ConvertSceneToMatCoords(
            (double) Math.Floor(worldPosition.x + 0.5), 
            (double) Math.Floor(worldPosition.y + 0.5));
        return w;
    }

    
    private void SelectUnit(Coordinate w){
        // ichange the selected, stored in gameManager
        if (board.IsWithinBoard(w) && board.Get(w) != SelectedId){                    
            SelectedId = board.Get(w); 
        }   
    }

    private void UnselectUnit(){
        SelectedId = -1;
    }

    public void PrintTest(){
        Debug.Log("printing successful.");
    }

    private void SelectEnemyUnit(Coordinate w){
        int charId = board.Get(w);
        // Debug.Log("initiating SelectAttackTargetUnit");
        if (board.IsWithinBoard(w) && charId != SelectedId && charId != -1 &&
            !instance.charArray[charId].GetComponent<CharacterGameState>().isYourTeam){     
            // Debug.Log("criteria successful. now setting ");               
            SelectedEnemyId = charId; 
            // Debug.Log("selectedEnemyId: " + selectedEnemyId);
        }   
    }
    private void UnselectEnemyUnit(){
        SelectedEnemyId = -1;
    }




    private (Boolean, Boolean) GetSelectedState(){
        return (IsSelected(), IsEnemySelected());
    }

    private Boolean IsTargetSelectedPlayer(Coordinate w){
        return board.Get(w) == SelectedId; 
    }

    private Boolean IsTargetSelectedEnemy(Coordinate w){
        return board.Get(w) == SelectedEnemyId; 
    }

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




    // need a selected and a target;
    // if selected and is on your team, and a target is in site, attack is enabled. 
    // attack enemy.  will be used on button click. 

    // logic for attack: player attacks first. reduce hp based on attack and modifiers (TBD later). 
    // then if opponent is alive, then opponent attacks player. 
    public void Attack(){
        // if ()
    }







    // will need a confirm (action) /back button



        // arrows version of moving cursor; 
    // kind of want to deprecate this or at least disable it for now.  
    // private void CursorUpdatePosition(){
    //     float newHoriz = transform.position.x; 
    //     float newVert = transform.position.y; 
        
    //     if (Input.GetKeyDown(KeyCode.RightArrow)){
    //         newHoriz += 1.0f;
    //     }
    //     else if (Input.GetKeyDown(KeyCode.LeftArrow)){
    //         newHoriz -= 1.0f;
    //     }
    //     // Check for vertical movement
    //     if (Input.GetKeyDown(KeyCode.UpArrow)){
    //         newVert += 1.0f;
    //     }
    //     else if (Input.GetKeyDown(KeyCode.DownArrow)){
    //         newVert -= 1.0f;
    //     }
    //     // Update character position

    //     Coordinate w = board.ConvertSceneToMatCoords((Double)newHoriz, (Double)newVert); 
    //     if (w.GetX() != -1 && w.GetY() != -1 && board.IsWithinBoard(w)){
    //         transform.position = new Vector2(newHoriz, newVert);
    //     }   
    // }



    // mouse functions to be used later. 
    // private void UpdatePositionMouse (){
    //     // get mouse 
    // }

    // call this when you're ready to add gesture effects; for now just use mouse. 
    // void UpdateTouchExample() {
    //     if (Input.touchCount > 0){
    //         Touch touch = Input.GetTouch(0);
    //         // Update the Text on the screen depending on current position of the touch each frame
    //         m_Text = "Touch Position : " + touch.position;
    //     }
    // }
}




// old function now i am going to refactor it. 
// private void UpdateMouseClickManager(){
//         if (Input.GetMouseButtonDown(0)){
//             Vector3 mousePosition = Input.mousePosition;
//             Coordinate w = GetMouseClickCoordinate(mousePosition);
//             // int targetId = board.Get(w);

//             // if unselected: select unit 
//             if (!IsSelected()){

//                 if (IsEnemySelected() && board.Get(w) != -1 && SelectedEnemyId == board.Get(w)){
//                     UnselectEnemyUnit();                             
//                 } else if (IsEnemySelected() && board.Get(w) != -1 && SelectedEnemyId != board.Get(w)) {

//                 }

//                 else {
//                     UnselectEnemyUnit();
//                     if (board.Get(w) != -1){
//                         if (instance.charArray[board.Get(w)].GetComponent<CharacterGameState>().isYourTeam){
//                             SelectUnit(w);
//                             UnselectEnemyUnit();
//                         } else {
//                             SelectEnemyUnit(w); 
//                         }
//                     }
//                     return;
//                 }


//             } 
//             // if currently selected something
//             else if (IsSelected()){
//                 // same unit -> unselect
//                 if (board.IsWithinBoard(w) && board.Get(w) == SelectedId){
//                     UnselectUnit();
//                     UnselectEnemyUnit();
//                     return; 
//                 }



//                 // moving condition: if selected and click is on available target, 
//                 // then get ready for attack
//                 else if (board.IsWithinBoard(w) && board.Get(w) != -1 && 
//                             instance.charArray[SelectedId].GetComponent<CharacterMove>().PossibleAttackTargets().Contains(w)){
//                     SelectEnemyUnit(w);
//                     return;
//                 }
//                 // if selected has not moved, and click is in possible moves, then 
//                 // "temp" move to later be confirmed
//                 else if (!instance.charArray[SelectedId].GetComponent<CharacterGameState>().HasMoved && 
//                             instance.charArray[SelectedId].GetComponent<CharacterGameState>().isYourTeam && 
//                             instance.charArray[SelectedId].GetComponent<CharacterMove>().PossibleMoves().Contains(w)){
//                     UnselectEnemyUnit();
//                     moveStack.Push(board.FindCharId(SelectedId)); 
//                     moveStack.Push(w);
//                     instance.charArray[SelectedId].GetComponent<CharacterMove>().MoveChar(w);
//                     instance.cursorStateMachine.chooseState.TriggerSelectedHighlights(); 
//                     return;
//                 }

//                 // 
//                 else if (board.IsWithinBoard(w) && board.Get(w) != SelectedId){
//                     UnselectEnemyUnit();
//                     UnselectUnit(); 
//                     if (board.Get(w) != 1 && instance.charArray[board.Get(w)].GetComponent<CharacterGameState>().isYourTeam){
//                         SelectUnit(w);
//                     } else if (board.Get(w) != 1 && !instance.charArray[board.Get(w)].GetComponent<CharacterGameState>().isYourTeam){
//                         SelectEnemyUnit(w); 
//                     }
//                     return;
//                 }
//             }
//         }
//     }