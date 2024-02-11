using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// this changes the position of items on a grid; 
// need to "gatekeep"


public class ArrowMove : MonoBehaviour
{


    [SerializeField] StatMenuManager statMenuManager; 

    public Board board; 
    public GameManager instance; 
    // get middle row, middleCol from board
    public Coordinate arrowCoordinate;

    private Camera mainCamera;
    public string m_Text;

    // Start is called before the first frame update
    private void Start()
    {
        // TouchSimulation.Enable;
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    private void Update()
    {
        UpdatePosition();
        UpdateMouseClicks();
    }

    public void Initialize(Board board, GameManager instance){
        this.board = board; 
        this.arrowCoordinate = board.MiddleBoardCoordinate();     
        this.instance = instance; 
    }

    // arrows version of moving cursor
    private void UpdatePosition(){
        float newHoriz = transform.position.x; 
        float newVert = transform.position.y; 
        
        if (Input.GetKeyDown(KeyCode.RightArrow)){
            newHoriz += 1.0f;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow)){
            newHoriz -= 1.0f;
        }
        // Check for vertical movement
        if (Input.GetKeyDown(KeyCode.UpArrow)){
            newVert += 1.0f;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow)){
            newVert -= 1.0f;
        }
        // Update character position

        Coordinate w = board.ConvertSceneToMatCoords((Double)newHoriz, (Double)newVert); 
        if (w.GetX() != -1 && w.GetY() != -1 && board.IsWithinBoard(w)){
            transform.position = new Vector2(newHoriz, newVert);
        }   
    }

    private void UpdatePositionMouse (){
        // get mouse 
    }



    // test for returning the position of the 
    // touch that can be tested on the device; todo's: 
    // how to reduce the scale of the canvas of the scene to fit the phone
    
    // call this when you're ready to add gesture effects; for now just use mouse. 
    void UpdateTouchExample() {
        if (Input.touchCount > 0){
            Touch touch = Input.GetTouch(0);
            // Update the Text on the screen depending on current position of the touch each frame
            m_Text = "Touch Position : " + touch.position;
        }
    }


    private void UpdateMouseClicks(){
        // Check for mouse click
        if (Input.GetMouseButtonDown(0)){
            // Get the mouse position in screen coordinates
            Vector3 mousePosition = Input.mousePosition;

            // Convert mouse position to world coordinates
            Vector3 worldPosition = mainCamera.ScreenToWorldPoint(mousePosition);
            worldPosition.z = 0; // Set the z-coordinate to 0 to ensure it's on the same plane as the board

            // add 0.5 to both x and y and take the floor to adjust to the center of the tile, which isa t0.0 for the bottom lefet corner
            Coordinate w = board.ConvertSceneToMatCoords((double) Math.Floor(worldPosition.x +0.5), (double) Math.Floor(worldPosition.y + 0.5));

            // Now you have the position of the click on the board

            // ichange the selected, stored in gameManager
            if (board.IsWithinBoard(w) && board.Get(w) != instance.SelectedId){                    
                instance.SelectedId = board.Get(w); 
            }    
        }

    }
}
