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
    public void Initialize(GameObject charInstance, Board board, int id) 
    {
        this.moves = charInstance.GetComponent<CharacterStats>().moves; 
        this.board = board; 
        this.charId = id;
        this.character = charInstance; 
    }

    // return a set of possible moves; we will apply BFS to return 
    // a hash set of the id's possible moves. 

    public HashSet<Coordinate> PossibleMoves()
    {
        Debug.Log("moves set at: " + moves + " before calling CharInt");
        HashSet<Coordinate> res = CharInteraction.SetOfMoves(board, board.FindCharId(charId), moves);
        Debug.Log("count of moves: " + res.Count);
        return res; 
    }

}