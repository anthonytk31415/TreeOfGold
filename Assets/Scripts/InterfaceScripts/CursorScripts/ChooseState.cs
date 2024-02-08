
using System; 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// here we choose the 


public class ChooseState : ICursorState
{
    private GameObject cursor; 
    private GameManager gameManager; 
    private Board board; 

    // some properties that determine what the cursor does


    public ChooseState(GameObject cursor, GameManager gameManager, Board board) {
        this.cursor = cursor;
        this.gameManager = gameManager;
        this.board = board;  
    }


    // trigger all the things you want to do when you enter
    public void Enter(){
        HighlightPlayerMovesCells();
    }

    public void Update(){
    }

    public void Exit(){

    }

    // call this when you begin and after you've donea move(probably a switch in state so no need to redo call)
    public void HighlightPlayerMovesCells(){
        
        // Get all moves for those chars on your team and has not moved
        HashSet<Coordinate> allPlayerMoves = new(); 
        foreach (GameObject character in gameManager.charArray) {
            // var charGameState = character.GetComponent<CharacterGameState>(); 
            if (character.GetComponent<CharacterGameState>().isYourTeam && !character.GetComponent<CharacterGameState>().hasMoved){
                HashSet<Coordinate> curMoves = character.GetComponent<CharacterMove>().PossibleMoves();
                allPlayerMoves.UnionWith(curMoves);
            }
        }

        // highlight those moves on the grid
        foreach (Tile tile in gameManager.tiles){            
            Transform targetTransform = tile.transform;
            // Retrieve the x and y position of the targetObject
            float xPosition = targetTransform.position.x;
            float yPosition = targetTransform.position.y;
            Coordinate w = board.ConvertSceneToMatCoords((double)xPosition, (double)yPosition); 
            if (allPlayerMoves.Contains(w)){
                tile.TogglePlayerPath();
            }
        }
    }

}