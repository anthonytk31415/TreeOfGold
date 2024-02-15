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




    // public delegate void SelectedAttackTargetIdChangedEventHandler(int attackTargetId);
    // // Define the event based on the delegate
    // public static event SelectedAttackTargetIdChangedEventHandler OnAttackTargetIdChanged;
    // private int attackTargetId;
    // public int AttackTargetId {
    //     get { return attackTargetId; }
    //     set {
    //         if (attackTargetId != value){
    //             attackTargetId = value;
    //             // Invoke the event whenever score changes
    //         } else {
    //             attackTargetId = -1;  
    //         }
    //         OnAttackTargetIdChanged?.Invoke(attackTargetId);
    //     }
    // }




    private Stack<Coordinate> moveStack;

    public Boolean IsSelected(){
        return selectedId != -1; 
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

            int sId = SelectedId; 
            // if unselected: select unit 
            if (!IsSelected()){
                SelectUnit(w);
                UnselectAttackTargetUnit();
                // ToggleMoveOption()
                return;
                // toggle appropriate things on the screen 

            } 
            // if currently selected something
            else if (IsSelected()){
                // same unit -> unselect
                if (board.IsWithinBoard(w) && board.Get(w) == sId){
                    UnselectUnit();
                    // UnToggleMoveOption
                    return; 
                }
                // diff unit: -> select

                // moving condition 
                // GameObject unit = instance.charArray[sId]; 


                // if selected and click is on available target, then get ready for attack
                else if (board.IsWithinBoard(w) && board.Get(w) != -1 && 
                            instance.charArray[sId].GetComponent<CharacterMove>().PossibleAttackTargets().Contains(w)){
                    SelectAttackTargetUnit(w);
                    Debug.Log("char coordinate: " + w + ", selectedEnemyId: " + selectedEnemyId);
                    return;
                }

                // if selected has not moved, and click is in possible moves, then "temp" move to later be confirmed
                else if (!instance.charArray[sId].GetComponent<CharacterGameState>().HasMoved && 
                            instance.charArray[sId].GetComponent<CharacterGameState>().isYourTeam && 
                            instance.charArray[sId].GetComponent<CharacterMove>().PossibleMoves().Contains(w)){
                    Debug.Log("Attempting move");
                    // move player
                    // enableMoveButton

                    moveStack.Push(board.FindCharId(sId)); 
                    moveStack.Push(w);
                    instance.charArray[sId].GetComponent<CharacterMove>().MoveChar(w);
                    instance.cursorStateMachine.chooseState.TriggerSelectedHighlights(sId); 
                    return;
                }

                else if (board.IsWithinBoard(w) && board.Get(w) != sId){
                    UnselectAttackTargetUnit();
                    SelectUnit(w);
                    return;
                }
            }
        }
    }

    public void UndoMove(){
        if (moveStack.Count > 1){
            Coordinate curPos = moveStack.Pop();
            Coordinate prevPos = moveStack.Pop();
            int unitId = board.Get(curPos); 
            instance.charArray[unitId].GetComponent<CharacterMove>().UndoMoveChar(prevPos);
            instance.cursorStateMachine.chooseState.ResetBoard(); 
        }
    }

    public void ClearMoveStack(){
        this.moveStack = new Stack<Coordinate>();
    }

    // once you attack the stack gets reset

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

    private void SelectAttackTargetUnit(Coordinate w){
        int charId = board.Get(w);
        Debug.Log("initiating SelectAttackTargetUnit");
        if (board.IsWithinBoard(w) && charId != SelectedId && charId != -1 &&
            !instance.charArray[charId].GetComponent<CharacterGameState>().isYourTeam){     
            Debug.Log("criteria successful. now setting ");               
            SelectedEnemyId = charId; 
            Debug.Log("selectedEnemyId: " + selectedEnemyId);
        }   
    }
    private void UnselectAttackTargetUnit(){
        SelectedEnemyId = -1;
    }


    // need a selected and a target;
    // if selected and is on your team, and a target is in site, attack is enabled. 
    // attack enemy.  will be used on button click. 
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
