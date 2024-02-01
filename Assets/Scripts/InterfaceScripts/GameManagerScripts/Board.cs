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
    // private Dictionary<int, Coordinate> idToLoc; 
    // private Dictionary<GameObject, int> gameObjectIds; 


    private int rows; 
    private int columns;  

    private Dictionary<Coordinate, (double, double)> matToSceneCoords;      // row, col --> x, y
    private Dictionary<(Double, Double), Coordinate> sceneToMatCoords;      // x, y --> row, col
    // create the board 
    public Board(int rows, int columns)
    {
        this.board = new int[rows, columns]; 
        // this.idToLoc = new Dictionary<int, Coordinate>(); 
        // this.gameObjectIds = new Dictionary<GameObject, int>();
        this.rows = rows; 
        this.columns = columns; 
        this.matToSceneCoords = new Dictionary<Coordinate, (double, double)>();
        this.sceneToMatCoords = new Dictionary<(double, double), Coordinate>();

        // building methods
        BuildBoard();
        BuildMatToSceneCoords(); 
    }

    // assigns id to pieces, then coordinates to pieces, the places them in board
    // IDs start at 1 onward. 

    public int Get(Coordinate w) 
    {
        return board[w.GetX(), w.GetY()];
    }

    public Boolean Put(Coordinate w, int id)
    {
        if (IsValidEntry(w) && IsEmpty(w))
        {   
            board[w.GetX(), w.GetY()] = id; 
            // idToLoc[id] = w; 

            return true; 
        }
        return false; 
    }
    // public void AssignIdToChar(GameObject character, Coordinate w, int id) 
    // {
    //     // gameObjectIds[character] = id; 
    //     Put(w, id); 
    //     // Debug.Log(String.Format("id: {0}, w: {1}", id, w ));

    // }

    // public void PrintGameObjectIdPairs()
    // {
    //     foreach (var kvp in gameObjectIds.Keys){
    //         Debug.Log(kvp +", id: " + gameObjectIds[kvp] +"; ");
    //     }
    // }

    // checks whether x, y is within the bounds of the board and whether currently, board[x,y] == 0
    public Boolean IsValidEntry(Coordinate w){
        int x = w.GetX();
        int y = w.GetY();
        return 0 <= x &&  x < rows && 0 <= y && y < columns;  
    }


    // // returns the objectId of a char/piece on the board REFACTOR
    // public int GetId(GameObject piece)
    // {
    //     if (gameObjectIds.ContainsKey(piece))
    //     {
    //         return gameObjectIds[piece];
    //     }
    //     return -1; 
    // }

    // return a tuple of rows and columns of the board
    public (int, int) GetDims()
    {
        return (rows, columns);
    }

    public void PutEmpty(Coordinate w)
    {
        Put(w, -1);
    }


    // given x, y, return board entry = 0; but do we move anything if there was something before? 
    // void AssignEmpty(Coordinate w)
    // {
    //     // do we ensure x and y are valid? 
    //     Put(w, 0); 
    // }

    // is this matrix spot empty?
    public bool IsEmpty(Coordinate w)
    {
        return Get(w) == -1;
    }

    // given an id, return the position of it, or return (-1, -1) if it fails. 
    public Coordinate FindCharId (int id)
    {
        for (int i = 0; i < board.GetLength(0); i ++)
        {
            for (int j = 0; j < board.GetLength(1); j ++)
            {
                if (board[i, j] == id){
                    return new Coordinate(i, j);
                }
            }
        }
        return new Coordinate(-1, -1);
    }

    // void RemoveIdFromBoard(int id)
    // {
    //     Coordinate w = idToLoc[id];

    //     PutEmpty(w); 
    //     idToLoc.Remove(pieceId); 
    // }

    // puts a piece ID into an x/y slot
    void MovePiece(int id, Coordinate destination)
    {        
        Coordinate v = FindCharId(id);
        if (!v.Equals(new Coordinate(-1, -1)) && (IsEmpty(destination)))
        {            
            PutEmpty(v); ; 
            Put(destination, id);         
        }
    }    

    // given x, y matrix coordinates, translate to scene coordinates
    // 10 rows, 14 columns; 0 indexed
    // -6.5 = 0; 4.5 = 0
    // 6.5 = 13, -4.5 = 9
    private void BuildMatToSceneCoords()
    {
        Double u = rows/2  - 0.5; 
        for (int i = 0; i < rows; i ++)
        {
            Double v = -columns/2 + 0.5;
            for (int j = 0; j < columns; j ++)
            {
                Coordinate w = new Coordinate(i, j); 
                matToSceneCoords[w] = (v, u); 
                sceneToMatCoords[(v,u)] = w;
                v +=1; 
            }
            u -=1; 
        }
    }

    private void BuildBoard()
    {
        for (int i = 0; i < board.GetLength(0); i ++)
        {
            for (int j = 0; j < board.GetLength(1); j ++)
            {
                board[i, j] = -1; 
            }
        }
    }


    // given matrix entries, converts to coordinates on the scene for position to use with Vector3
    public (Double, Double) ConvertMatToSceneCoords(Coordinate w)
    {
        try
        {
            if (IsValidEntry(w) && IsEmpty(w))
            {
                return matToSceneCoords[w]; 
            }
        } 
        catch (Exception ex)
        {
            Debug.Log("Exception: " + ex);
        }
        return (-1, -1);
    }

    public Coordinate ConvertSceneToMatCoords(Double x, Double y)
    {
        if (sceneToMatCoords.ContainsKey((x, y)))
        {
            return sceneToMatCoords[(x, y)]; 
        }
        return new Coordinate(-1, -1);
    }

    public Coordinate MiddleBoardCoordinate()
    {
        int row = rows/2;
        int col = columns/2;
        return new Coordinate(row, col);
    }



}

