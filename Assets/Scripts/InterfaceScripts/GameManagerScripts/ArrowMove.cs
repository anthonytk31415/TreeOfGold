using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// this changes the position of items on a grid; 
// need to "gatekeep"


public class ArrowMove : MonoBehaviour
{
    public Board board; 
    public GameManager instance; 
    // get middle row, middleCol from board

    public Coordinate arrowCoordinate;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePosition();
    }

    public void Initialize(Board board, GameManager instance){
        this.board = board; 
        this.arrowCoordinate = board.MiddleBoardCoordinate();     
        this.instance = instance; 

    }

    void UpdatePosition()
    {
        float newHoriz = transform.position.x; 
        float newVert = transform.position.y; 
        
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            newHoriz += 1.0f;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            newHoriz -= 1.0f;
        }
        // Check for vertical movement
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            newVert += 1.0f;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            newVert -= 1.0f;
        }
        // Update character position

        Coordinate w = board.ConvertSceneToMatCoords((Double)newHoriz, (Double)newVert); 
        if (w.GetX() != -1 && w.GetY() != -1 && board.IsValidEntry(w)){
            transform.position = new Vector2(newHoriz, newVert);
        }   
    }
}
