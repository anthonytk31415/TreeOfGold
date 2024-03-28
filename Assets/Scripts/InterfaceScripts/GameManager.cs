/*

lets create the board
for the board initializer, we'll set up the 
board, and then we'll take in as an argument characters to load and their corresponding initial positions on the board. 
Boards will be for now rows = 10, columns = 14, which will be modified potentially later 

we'll need methods to modify positions 
how to deal with dead corpses when people die
modifiers for cells 

WIP 


ideally, when I instantiate gameManager, this happens 

*/
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Singleton instance
    private static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }
    public Board board;
    public Tile[,] tiles; 


    // screen dimensions 
    private int _width; 
    private int _height; 
    private int _totalHeight; 

    // Useful GameObjects
    // public GameObject cursor; // cursor object
    public GameObject[] charArray;      // need to update later to dynamically change
    public GameStateMachine gameStateMachine; 
    public GameObject moveControllerObject; 
    public GameScore gameScore; 
    public GameObject battleManagerObject; 
    [SerializeField] public GameObject statMenuController; 

    public HighlightTilesManager highlightTilesManager; 

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
        gameStateMachine.Update();
        // moveController.Update();
    }

    private void InitializeCore() {       

        // dims of board
        this._width = 6;                
        this._height = 8;
        this._totalHeight = _height + 3;         
        this.board = new Board(_width, _height);
        this.tiles = GridManager.Initialize(_width, _totalHeight); // this is instantiated with the board + menus
        this.highlightTilesManager = new HighlightTilesManager(Instance);

        // characters build; Battling
        this.charArray = new GameObject[4];         // this is hard coded the length of the chars that charactersobject 
        CharactersObject.Initialize(Instance);
        this.battleManagerObject = BattleManagerObject.Initialize(Instance);


        // Movement
        this.gameStateMachine = new GameStateMachine(Instance); 

        this.moveControllerObject = MoveControllerObject.Initialize(Instance);
        this.gameStateMachine.Initialize(gameStateMachine.playerState);

        // Score
        this.gameScore = new GameScore(this.charArray);





        // adjusts camera to centered position 
        Camera.main.transform.position = new Vector3((float)_width/2 -0.5f, (float)_totalHeight / 2 - 0.5f, -10);

    }

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

    public void AuditCharArray() {
        Debug.Log("initiating charArray audit...");
        foreach(GameObject charObj in charArray){
            Debug.Log(charObj);
        }
    }
}


