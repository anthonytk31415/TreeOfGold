// Methods for Moving the character
using System;
using UnityEngine;
using System.Collections.Generic; 

public class CharacterMove: MonoBehaviour
{

    public int moves; 
    public Board board;
    public int charId;  
    public GameObject character; 
    public GameManager gameManager; 
    // Other methods as needed
    public void Initialize(GameObject charInstance, Board board, int id, GameManager gameManager) 
    {
        this.moves = charInstance.GetComponent<CharacterStats>().moves; 
        this.board = board; 
        this.charId = id;
        this.character = charInstance; 
        this.gameManager = gameManager; 
    }

    // return a set of possible moves; we will apply BFS to return 
    // a hash set of the id's possible moves. 

    public HashSet<Coordinate> PossibleMoves()
    {
        if (!character.GetComponent<CharacterGameState>().HasMoved){
            return CharInteraction.PlayerMoveOptions(board, board.FindCharId(charId), moves, gameManager);

        } else {
            return new HashSet<Coordinate>();
        }
    }

    // public void MoveChar
    // if playerSelected and mouse clicked on entry in its path/ move 
    public void MoveChar(Coordinate w){
        // move on board
        Coordinate u = board.FindCharId(charId);        // old pos
        board.PutEmpty(u);
        board.Put(w, charId);                           // new pos
        // // move visually; need coordinate to scene function here
        var (x, y) = board.ConvertMatToSceneCoords(w);
        transform.position = new Vector2((float) x, (float) y);
        character.GetComponent<CharacterGameState>().HasMoved = true;

    }

    public void UndoMoveChar(Coordinate w){
        MoveChar(w);
        character.GetComponent<CharacterGameState>().HasMoved = false;
    }
}