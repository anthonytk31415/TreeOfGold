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

public class GameManager : MonoBehaviour
{
    // Singleton instance
    private static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }

    // // Board parameters
    private Board board;
    public GameObject core; // use core to attach components 

    // Use charArray to keep track of chars using the char's index
    public GameObject[] charArray; 

    private void Awake()
    {
        // Singleton pattern implementation
        if (_instance != null && _instance != this)
            Destroy(this.gameObject);    
        _instance = this;

    }

    // Initializes the board at a fixed 10x14 size board. 
    private void Start()
    {
        InitializeCore();
    }

    private void InitializeCore() 
    {
        core = new GameObject("Game Object for GameManager");          
        InitializeBoard();
        InstantiateChars();
        InitializeCursor();
    }

    private void InitializeBoard()
    {
        // Create the board matrix
        this.board = new Board(10, 14); 
    }

    // Other game management methods can go here
    private void InitializeCursor() 
    {
        GameObject curPrefab = Resources.Load("Prefabs/InterfaceElements/" + "cursor", typeof(GameObject)) as GameObject;
        GameObject cursorInstance = Instantiate(curPrefab, new Vector3(0.5f, 0.5f, 0f), Quaternion.identity);
        cursorInstance.AddComponent<ArrowMove>();
        cursorInstance.GetComponent<ArrowMove>().Initialize(board, Instance);
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // Instantiation Methods. 
    //
    // Currently we create all of our instantiation methods here inside GameManager. 
    // Eventually do we move them in separate classes? 
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public GameObject InstantiateChar(String pfFileName, Coordinate w)
    {
        (Double u, Double v) = board.ConvertMatToSceneCoords(w);
        if (u == -1 & v == -1)
        {
            throw new KeyNotFoundException(" bad coordinate " + w + " given.");
        }
        GameObject charPrefab = Resources.Load("Prefabs/Characters/" + pfFileName, typeof(GameObject)) as GameObject;            
        GameObject charInstance = Instantiate(charPrefab, new Vector3((float)u, (float)v, 0.0f), Quaternion.identity);

        return charInstance; 
        
    }
    // manage Componenets to Char here. 
    public void AddComponentsToChar(int id, int hp, int attack, int moves, Boolean player)
    {
        GameObject charInstance = charArray[id];        
        // Add components (scripts) to the char instance
        charInstance.AddComponent<CharacterStats>();
        charInstance.GetComponent<CharacterStats>().Initialize(hp, attack, moves); 
        charInstance.AddComponent<CharacterMove>();
        charInstance.GetComponent<CharacterMove>().Initialize(Instance, board, id);
        charInstance.AddComponent<CharacterGameState>(); 
        charInstance.GetComponent<CharacterGameState>().Initialize(Instance, board, id, player); 

        // CharacterStats charStats = charInstance.GetComponent<CharacterStats>();

        // // assign stats to char
        // charStats.hp = hp;
        // charStats.attack = attack;
        // charStats.moves = moves; 

        // CharacterMove charMove = charInstance.GetComponent<CharacterMove>();

        // charMove.Initialize(charStats, board, charInstance, id);
    }

    // Spawn characters from a set of predefined chars; 
    // will update later on how the level will define who you put on the map
    // should be called at the beginning and only once. 
    // Id's start at 0. 
    public void InstantiateChars()
    {
        // temporarily we'll provide the chars we want to instantiate for testing; 
        // later we'll build some mechanism to do this; perhaps move this class outside the game manager. 
        (String, int, int, int, int, int, Boolean)[] chars = {
            ("glenn", 9, 5, 3, 0, 0, true), 
            ("greenMage", 6, 11, 3, 0, 1, true), 
            ("knight", 12, 4, 3, 0, 13, false), 
            ("purpleMage", 5, 4, 3, 9, 0, false)};
        
        charArray = new GameObject[chars.Length]; 
        for (int id = 0; id < chars.Length; id ++)
        {
            // first, instantiate the char
            (String name, int hp, int atk, int moves, int x, int y, Boolean player) = chars[id];
            Coordinate w = new Coordinate(x, y);
            GameObject curChar = InstantiateChar(name, w);            
            board.Put(w, id);
            charArray[id] = curChar;

            // then, add components; anything requiring id needs to be called after the board/charArray is built
            AddComponentsToChar(id, hp, atk, moves, player);
        }

    }
    public GameObject GetCharacter(int id)
    {
        return charArray[id]; 
    }

    // printing all possibles moves for audit purposes
    public void AuditPossibleMoves()
    {
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

