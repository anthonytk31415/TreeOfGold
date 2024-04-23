// Methods for Moving the character
using System;
using UnityEngine;
using System.Collections.Generic; 

/*
CharacterMove manipulates the character movement in the model itself (the Board)
and has helper functions that returns things like: 
- PossibleMoves
- PossibleAttackTargets


Do we move this into a 

*/

public class CharacterMove: MonoBehaviour
{

    public int moves; 
    public Board board;
    public int charId;  
    public GameObject character; 
    public GameManager gameManager; 
    // Other methods as needed
    public void Initialize(GameObject charInstance, int id, GameManager gameManager) 
    {
        this.moves = charInstance.GetComponent<CharacterStats>().moves; 
        this.board = gameManager.board; 
        this.charId = id;
        this.character = charInstance; 
        this.gameManager = gameManager; 
    }

    // return a set of possible moves; we will apply BFS to return 
    // a hash set of the id's possible moves. 

    public HashSet<Coordinate> PossibleMoves() {
        if (!character.GetComponent<CharacterGameState>().HasMoved){
            return CharInteraction.PlayerMoveOptions(board.FindCharId(charId), moves, gameManager);
        } else {
            return new HashSet<Coordinate>();
        }
    }

    // public void MoveChar
    // if playerSelected and mouse clicked on entry in its path/ move 
    public void MoveChar(Coordinate destination){
        // move on board
        GetComponent<CharacterAnimateController>().characterAnimateCommandWalk.DoMove(destination); 
        MoveCharBoard(destination);                       // new pos
        // move visually; need coordinate to scene function here
        // var (x, y) = board.ConvertMatToSceneCoords(destination);
        // transform.position = new Vector2((float) x, (float) y);
        // 

        character.GetComponent<CharacterGameState>().HasMoved = true;

    }

    public void MoveCharBoard(Coordinate destination){
        Coordinate start = board.FindCharId(charId);        // old pos
        board.PutEmpty(start);
        board.Put(destination, charId);     
    }

    // return a units possible attack targets at its current position w/o
    // moving. Used to determine highlights for attacks
    // and for enemy phase decision making 
    public HashSet<Coordinate> PossibleAttackTargets(){
        int range = character.GetComponent<CharacterStats>().attackRange;  
        Coordinate initialPos = board.FindCharId(charId);
        HashSet<Coordinate> possibleAttackTargets = CharInteraction.EnemiesWithinAttackRange(gameManager, character, initialPos, range); 
        return possibleAttackTargets;      
    }

    // used for cancelling moves
    public void UndoMoveChar(Coordinate destination){
        MoveCharBoard(destination);                       // new pos
        var (x, y) = board.ConvertMatToSceneCoords(destination);        
        transform.position = new Vector2((float) x, (float) y); // just "teleport back"

        // transform.position = Board.ConvertCoordinateToVector2(destination);
        character.GetComponent<CharacterGameState>().HasMoved = false;
    }
}