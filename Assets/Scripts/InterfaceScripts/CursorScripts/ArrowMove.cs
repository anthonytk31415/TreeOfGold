using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// this changes the position of items on a grid; 
// need to "gatekeep"


public class ArrowMove : MonoBehaviour
{
    public Board board; 
    public GameManager instance; 
    // get middle row, middleCol from board

    public Coordinate arrowCoordinate;

    public string m_Text;

    // Start is called before the first frame update
    void Start()
    {
        // TouchSimulation.Enable;
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePosition();
        UpdateTouchExample();
    }

    public void Initialize(Board board, GameManager instance){
        this.board = board; 
        this.arrowCoordinate = board.MiddleBoardCoordinate();     
        this.instance = instance; 

    }

    private void UpdatePosition()
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


    // test for returning the position of the 
    // touch that can be tested on the device; todo's: 
    // how to reduce the scale of the canvas of the scene to fit the phone
    
    void UpdateTouchExample() 
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            // Update the Text on the screen depending on current position of the touch each frame
            m_Text = "Touch Position : " + touch.position;
            Debug.Log(m_Text);
        }

    }
}
