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
    public void Initialize(GameManager gameManager, Board board, int id) 
    {
        this.moves = gameManager.GetCharacter(id).GetComponent<CharacterStats>().moves; 
        this.board = board; 
        this.charId = id;
        this.character = gameManager.GetCharacter(id); 
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

        // HashSet<Coordinate> res = new HashSet<Coordinate>();
        // Coordinate initialPos = board.FindCharId(charId);             

        // Queue<(Coordinate, int)> queue = new Queue<(Coordinate, int)>();
        // queue.Enqueue((initialPos, moves));
        // res.Add(initialPos);
        // // Debug.Log("moves: " + moves + ", initial pos: " + initialPos);
        // while (queue.Count > 0){
        //     (Coordinate curPos, int curMove) = queue.Dequeue(); 

        //     (int, int)[] positions = {(1,0), (-1, 0), (0, 1), (0, -1)}; 
        //     if (curMove > 0) {
        //         foreach ((int dx, int dy) in positions) 
        //         {
        //             // (int dx, int dy) = delta; 
        //             Coordinate w = new Coordinate(curPos.GetX() + dx, curPos.GetY() + dy);
        //             if (!res.Contains(w) && board.IsValidEntry(w) && board.IsEmpty(w))
        //             {
        //                 res.Add(w);
        //                 queue.Enqueue((w, curMove - 1)); 
        //             }
        //         }
        //     }
        // }
        // return res; 
    // } 
}