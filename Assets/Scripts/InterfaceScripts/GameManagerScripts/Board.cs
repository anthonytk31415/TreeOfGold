// building board mechanics
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;


// Purpose of Board: 
// - maintains container of units and other objects (for now, units) on the board
// - keeps track of position, character id, and assignment of character ids  --> shoudl i move this into its own class?

// work only in matrix operations; 
// only when you move things in the board, use screen coordinates

public class Board
{
    // we have 3 data stores - board, idToLoc, and gameObjectIds. 
    // we maintain these 3 so we can have O(1) lookups: (1) with Coordinates using the board, 
    // (2) with game Ids to the location on board, and (3) with the gameObject to Id. 
    private int[, ] board;  

    private int rows; 
    private int columns;  

    // create the board 
    
    public Board(int rows, int columns)
    {
        this.board = new int[rows, columns]; 
        this.rows = rows; 
        this.columns = columns; 

        // building methods
        BuildBoard();
    }

    // assigns id to pieces, then coordinates to pieces, the places them in board
    // IDs start at 1 onward. 

    public int Get(Coordinate w) {
        return board[w.GetX(), w.GetY()];
    }

    public Boolean Put(Coordinate w, int id) {
        if (IsValidEntry(w) && IsEmpty(w)) {   
            board[w.GetX(), w.GetY()] = id; 
            return true; 
        }
        return false; 
    }

    // checks whether x, y is within the bounds of the board and whether currently, board[x,y] == 0
    public Boolean IsValidEntry (Coordinate w) {
        int x = w.GetX();
        int y = w.GetY();
        return 0 <= x &&  x < rows && 0 <= y && y < columns;  
    }

    // return a tuple of rows and columns of the board
    public (int, int) GetDims(){
        return (rows, columns);
    }

    public void PutEmpty(Coordinate w){
        Put(w, -1);
    }

    // is this matrix spot empty?
    public bool IsEmpty(Coordinate w){
        return Get(w) == -1;
    }

    // given an id, return the position of it, or return (-1, -1) if it fails. 
    public Coordinate FindCharId (int id){
        for (int i = 0; i < board.GetLength(0); i ++){
            for (int j = 0; j < board.GetLength(1); j ++){
                if (board[i, j] == id){
                    return new Coordinate(i, j);
                }
            }
        }
        return new Coordinate(-1, -1);
    }

    // puts a piece ID into an x/y slot
    void MovePiece(int id, Coordinate destination){        
        Coordinate v = FindCharId(id);
        if (!v.Equals(new Coordinate(-1, -1)) && (IsEmpty(destination)))
        {            
            PutEmpty(v); ; 
            Put(destination, id);         
        }
    }    
 
    private void BuildBoard(){
        for (int i = 0; i < board.GetLength(0); i ++)
        {
            for (int j = 0; j < board.GetLength(1); j ++)
            {
                board[i, j] = -1; 
            }
        }
    }

    // given matrix entries, converts to coordinates on the scene for position to use with Vector3
    public (Double, Double) ConvertMatToSceneCoords(Coordinate w) {
        try {
            if (IsValidEntry(w) && IsEmpty(w)) {
                return (w.GetX(), w.GetY() + 1); 
            }
        } 
        catch (Exception ex) {
            Debug.Log("Exception: " + ex);
        }
        return (-1, -1);
    }

    public Coordinate ConvertSceneToMatCoords(Double x, Double y){
        return new Coordinate((int)x, (int) y -1); 
    }

    public Coordinate MiddleBoardCoordinate(){
        int row = rows/2;
        int col = columns/2;
        return new Coordinate(row, col);
    }



}

