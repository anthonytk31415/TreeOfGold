// lets create the board
// for the board initializer, we'll set up the 
// board, and then we'll take in as an argument characters to load and their corresponding initial positions on the board. 
// Boards will be for now rows = 10, columns = 14, which will be modified potentially later 

// we'll need methods to modify positions 
// how to deal with dead corpses when people die
// modifiers for cells 

// WIP 

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using Unity;

public class GameManager : MonoBehaviour
{
    // Singleton instance
    private static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }

    // // Board parameters
    public Board board;
    public Tile[,] tiles; 


    // screen dimensions 
    private int _width; 
    private int _height; 
    private int _totalHeight; 

    // Useful GameObjects
    // public GameObject cursor; // cursor object
    public GameObject[] charArray;      // need to update later to dynamically change
    public CursorStateMachine cursorStateMachine; 
    public GameObject moveControllerObject; 


    [SerializeField] public GameObject statMenuController; 

    private void Awake() {
        // Singleton pattern implementation
        if (_instance != null && _instance != this)
            Destroy(this.gameObject);    
        _instance = this;

    }

    // Initializes the board at a fixed 10x14 size board. 
    private void Start(){
        InitializeCore();
    }

    private void Update(){
        cursorStateMachine.Update();
        // moveController.Update();
    }

    private void InitializeCore() {       

        this._width = 6;                // dims of board
        this._height = 8;
        this._totalHeight = _height + 3; 
        this.board = new Board(_width, _height);
        this.tiles = GridManager.Initialize(_width, _totalHeight); // this is instantiated with the board + menus
        this.charArray = new GameObject[4];
        CharactersObject.Initialize(Instance, board, charArray);
        this.cursorStateMachine = new CursorStateMachine(Instance, board); 
        this.cursorStateMachine.Initialize(cursorStateMachine.chooseState);
        this.moveControllerObject = MoveControllerObject.Initialize(board, Instance);

        // adjusts camera to centered position 
        Camera.main.transform.position = new Vector3((float)_width/2 -0.5f, (float)_totalHeight / 2 - 0.5f, -10);

    }

    // Other game management methods can go here

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // Instantiation Methods. 
    //
    // Currently we create all of our instantiation methods here inside GameManager. 
    // Eventually do we move them in separate classes? 
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public GameObject GetCharacter(int id) {
        return charArray[id]; 
    }


    // Audit Helper functions
    public void AuditPossibleMoves() {
        foreach(GameObject charObj in charArray)
        {
            CharacterMove charMove = charObj.GetComponent<CharacterMove>();  
            HashSet<Coordinate> possibleMoves = charMove.PossibleMoves();
            Debug.Log("count of possible moves: " + possibleMoves.Count);
            foreach(Coordinate v in possibleMoves){
                Debug.Log("id: " + charMove.charId + " testing for v: " + v);
            }
        }
    }








 
}

